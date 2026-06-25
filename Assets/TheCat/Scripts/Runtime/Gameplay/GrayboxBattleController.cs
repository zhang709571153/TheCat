using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Core;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Inputs;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Gameplay
{
    public sealed class GrayboxBattleController : MonoBehaviour
    {
        private const float AutoAttackIntervalSeconds = 1.15f;
        private const float CatPressureIntervalSeconds = 2f;
        private const float CatPressureDamageScale = 0.35f;
        private const float EnemyPathDistance = 8f;
        private const int SkillIndicatorRingSegments = 48;

        [SerializeField] private Transform bedAnchor;
        [SerializeField] private Transform litterBoxAnchor;
        [SerializeField] private Transform feederAnchor;
        [SerializeField] private Transform activeCatMarker;
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private P0SkillIndicatorView skillIndicatorView;
        [SerializeField] private P0StatusIndicatorView activeCatStatusIndicatorView;
        [SerializeField] private P0StatusIndicatorView bedStatusIndicatorView;
        [SerializeField] private bool autoStartBattle = true;

        private readonly Dictionary<int, GrayboxEnemyView> enemyViewsById = new Dictionary<int, GrayboxEnemyView>();
        private readonly Dictionary<string, SkillDefinition> skillsById = new Dictionary<string, SkillDefinition>();
        private readonly Dictionary<string, float> skillCooldownsById = new Dictionary<string, float>();
        private readonly List<CatBattleState> cats = new List<CatBattleState>();
        private readonly List<CatDefinition> catDefinitions = new List<CatDefinition>();
        private readonly P0RuntimeSettings runtimeSettings = new P0RuntimeSettings();
        private readonly P0BattleNavigationState navigation = new P0BattleNavigationState();

        private P0ObjectPool<GrayboxEnemyView> enemyViewPool;
        private BattleSimulation battle;
        private P0BattleStartContext battleStartContext;
        private RunMetrics runMetrics;
        private int activeCatIndex;
        private int skillIndicatorSlotIndex;
        private float autoAttackTimer;
        private float catPressureTimer;
        private bool routeCompletionRecorded;
        private bool showDiagnosticsHud;
        private bool showSmokeTools;
        private bool restartConfirmationOpen;
        private RunNodeCompletionReport lastCompletionReport;
        private string message = "就绪";
        private P0HudMessageChannel messageChannel = P0HudMessageChannel.Player;
        private Vector2 hudScrollPosition;
        private float hudPanelInnerWidth = P0ImGuiLayout.ReferenceWidth;
        private bool isResponsiveRowStacked;
        private GUIStyle wrappedHudLabel;
        private GUIStyle wrappedHudButton;
        private GUIStyle hudSectionLabel;
        private GUIStyle hudPanelStyle;
        private P0BattleFeedback lastFeedback;
        private float feedbackAgeSeconds;
        private P0WorldVisualAssetView bedVisualView;
        private P0WorldVisualAssetView litterBoxVisualView;
        private P0WorldVisualAssetView feederVisualView;
        private P0WorldVisualAssetView activeCatVisualView;
        private P0WorldVisualAssetView backgroundVisualView;
        private Transform backgroundAnchor;

        public BattleSimulation Battle => battle;

        public int ActiveCatIndex => activeCatIndex;

        public P0RuntimeSettings RuntimeSettings => runtimeSettings;

        public P0BattleFeedback LastFeedback => lastFeedback;

        public P0BattleFeedbackVisualState LastFeedbackVisual => P0BattleFeedbackVisualPresenter.Build(lastFeedback, feedbackAgeSeconds);

        private CatBattleState ActiveCat => cats.Count == 0 ? null : cats[activeCatIndex];

        private void Awake()
        {
            EnsureGrayboxSceneObjects();
            enemyViewPool = new P0ObjectPool<GrayboxEnemyView>(
                CreateEnemyViewInstance,
                onRent: PrepareRentedEnemyView,
                onRelease: PrepareReleasedEnemyView);
            LoadPrototypeData(null);
        }

        private void OnDestroy()
        {
            ClearEnemyViews();
            if (enemyViewPool != null)
            {
                enemyViewPool.Clear(DestroyPooledEnemyView);
            }
        }

        private void Start()
        {
            if (autoStartBattle)
            {
                BeginBattle();
            }
        }

        private void Update()
        {
            HandleKeyboardInput();
            AdvanceGraybox(Time.deltaTime);
        }

        public void AdvanceGraybox(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            TickFeedbackVisual(deltaSeconds);

            if (battle == null)
            {
                SyncEnemyViews();
                UpdateSkillIndicatorView();
                UpdateRuntimeStatusIndicators();
                return;
            }

            if (battle.Outcome != BattleOutcome.InProgress)
            {
                SyncEnemyViews();
                UpdateOutcomeMessage();
                UpdateSkillIndicatorView();
                UpdateRuntimeStatusIndicators();
                return;
            }

            float effectiveDeltaSeconds = runtimeSettings.ApplyToDelta(deltaSeconds);
            if (effectiveDeltaSeconds <= 0f)
            {
                SyncEnemyViews();
                UpdateOutcomeMessage();
                UpdateSkillIndicatorView();
                UpdateRuntimeStatusIndicators();
                return;
            }

            TickSkillCooldowns(effectiveDeltaSeconds);
            TickCats(effectiveDeltaSeconds);
            ApplyPlayerMovement(effectiveDeltaSeconds);
            battle.Tick(effectiveDeltaSeconds);
            ApplyAutoAttack(effectiveDeltaSeconds);
            ApplyEnemyPressureToActiveCat(effectiveDeltaSeconds);
            SyncEnemyViews();
            UpdateOutcomeMessage();
            UpdateSkillIndicatorView();
            UpdateRuntimeStatusIndicators();
        }

        private void OnGUI()
        {
            Rect panelRect = showDiagnosticsHud
                ? P0ImGuiLayout.BuildLeftPanelRect(340f, 620f, 0.42f)
                : P0ImGuiLayout.BuildLeftPanelRect(320f, 500f, 0.32f);
            hudPanelInnerWidth = P0ImGuiLayout.ScrollContentWidth(panelRect);
            GUILayout.BeginArea(panelRect, HudPanelStyle);
            hudScrollPosition = GUILayout.BeginScrollView(hudScrollPosition, false, true);
            GUILayout.Label("猫眠所 守床战斗");
            if (showDiagnosticsHud)
            {
                DrawRuntimeControls();
                DrawSmokeControls();
            }

            DrawBattleState();
            if (!showDiagnosticsHud)
            {
                DrawRuntimeControls();
            }

            DrawCatControls();
            DrawSkillControls();
            DrawInteractionControls();
            DrawHudMessage();
            P0BattleFeedbackVisualState feedbackVisual = P0BattleFeedbackVisualPresenter.Build(lastFeedback, feedbackAgeSeconds);
            if (feedbackVisual.HasVisual)
            {
                DrawFeedbackVisual(feedbackVisual);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawHudMessage()
        {
            string visibleMessage = P0HudMessagePresenter.BuildVisibleMessage(
                message,
                messageChannel,
                showDiagnosticsHud);
            if (string.IsNullOrWhiteSpace(visibleMessage))
            {
                return;
            }

            GUILayout.Space(8f);
            GUILayout.Label(visibleMessage, WrappedHudLabel);
        }

        private void SetPlayerMessage(string value)
        {
            message = value ?? string.Empty;
            messageChannel = P0HudMessageChannel.Player;
        }

        private void SetDiagnosticsMessage(string value)
        {
            message = value ?? string.Empty;
            messageChannel = P0HudMessageChannel.Diagnostics;
        }

        private void DrawPlayerCommandDeck()
        {
            P0BattleCommandDeck deck = BuildBattleCommandDeckForSmoke();
            if (!P0BattleCommandDeckPresenter.HasP0BattleCommandDeck(deck))
            {
                return;
            }

            GUILayout.Space(4f);
            GUILayout.Label(deck.BuildCompactPlayerLine(), WrappedHudLabel);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            DrawSkillIndicatorGizmos(BuildSkillIndicatorState());
        }

        public void BeginBattle()
        {
            RunProgressionState run = P0RunSession.EnsureProgression();
            ClearEnemyViews();
            skillCooldownsById.Clear();
            LoadPrototypeData(run);
            battleStartContext = P0BattleStartContext.Create(run);
            runMetrics = battleStartContext.ShouldCompleteRouteNode ? run.Metrics : new RunMetrics();
            RunPendingBattleModifierSnapshot pendingBattleModifiers = battleStartContext.ShouldCompleteRouteNode
                ? run.PendingBattleModifiers.Consume()
                : default(RunPendingBattleModifierSnapshot);
            BattleModifierSet battleModifiers = pendingBattleModifiers.ApplyTo(P0BlessingCatalog.CreateBattleModifiers(run.Blessings));
            P0Tuning battleTuning = pendingBattleModifiers.ApplyTo(P0Tuning.Default);

            BattleSimulationConfig config = new BattleSimulationConfig(
                battleStartContext.Wave,
                P0PrototypeCatalog.CreateCoreEnemies(),
                battleTuning,
                startingSleep: run.CoreValues.OwnerSleepCurrent,
                startingSleepMax: run.CoreValues.OwnerSleepMax,
                startingSleepBaseMax: run.CoreValues.OwnerSleepBaseMax,
                startingPoop: run.CoreValues.TeamPoop,
                startingHunger: run.CoreValues.TeamHunger,
                statusTags: P0PrototypeCatalog.CreateStatusTags(),
                modifiers: battleModifiers);

            battle = new BattleSimulation(config, runMetrics);
            ResetCats();
            navigation.Reset();
            SyncActiveCatMarker();
            activeCatIndex = 0;
            SelectFirstAvailableCat();
            SelectDefaultSkillIndicatorSlot();
            UpdateActiveCatMarkerAppearance();
            UpdateSkillIndicatorView();
            UpdateRuntimeStatusIndicators();
            autoAttackTimer = 0f;
            catPressureTimer = 0f;
            routeCompletionRecorded = false;
            restartConfirmationOpen = false;
            lastCompletionReport = null;
            runtimeSettings.SetPaused(false);
            lastFeedback = default(P0BattleFeedback);
            feedbackAgeSeconds = 0f;
            string startMessage = battleStartContext.StartMessage;
            if (pendingBattleModifiers.HasPending)
            {
                startMessage += " 梦境事件压力已生效。";
            }

            SetPlayerMessage(startMessage);
        }

        public void RestartRun()
        {
            P0RunSession.StartNewRun();
            BeginBattle();
        }

        public void RequestRestartRun()
        {
            restartConfirmationOpen = true;
            runtimeSettings.SetPaused(true);
            SetFeedback(P0BattleFeedbackPresenter.BuildRuntimeSettings(P0RuntimeSettingsPresenter.Build(runtimeSettings)));
            SetPlayerMessage("已打开重开确认；再次确认才会开始新一轮。");
        }

        public void ConfirmRestartRun()
        {
            if (!restartConfirmationOpen)
            {
                RequestRestartRun();
                return;
            }

            restartConfirmationOpen = false;
            RestartRun();
        }

        public void CancelRestartRun()
        {
            restartConfirmationOpen = false;
            SetFeedback(P0BattleFeedbackPresenter.BuildRuntimeSettings(P0RuntimeSettingsPresenter.Build(runtimeSettings)));
        }

        public void ContinueRoute()
        {
            if (battle == null || battle.Outcome == BattleOutcome.InProgress)
            {
                return;
            }

            SceneManager.LoadScene(P0SceneFlow.GetPostBattleSceneName(battle.Outcome));
        }

        public void ReturnToCatRoom()
        {
            if (battle == null || battle.Outcome == BattleOutcome.InProgress)
            {
                return;
            }

            P0CatRoomSession.RecordBattleReturn(
                battle.Outcome,
                P0RunSession.CurrentRun,
                lastCompletionReport ?? P0RunSession.LastCompletionReport);
            SceneManager.LoadScene(P0SceneFlow.CatRoomSceneName);
        }

        public void SelectCat(int index)
        {
            if (index < 0 || index >= cats.Count)
            {
                return;
            }

            bool isSwitchAttempt = index != activeCatIndex;
            if (!cats[index].Vital.CanSwitchTo)
            {
                if (isSwitchAttempt && battle != null && battle.Outcome == BattleOutcome.InProgress)
                {
                    battle.NodeMetrics.RecordCatSwitchBlockedByWeak();
                }

                SetFeedback(P0BattleFeedbackPresenter.BuildSkillBlocked(
                    catDefinitions[index].DisplayName,
                    "猫咪虚弱，无法切换"));
                return;
            }

            if (isSwitchAttempt && battle != null && battle.Outcome == BattleOutcome.InProgress)
            {
                battle.NodeMetrics.RecordCatSwitchSuccess();
            }

            activeCatIndex = index;
            UpdateActiveCatMarkerAppearance();
            SelectDefaultSkillIndicatorSlot();
            UpdateSkillIndicatorView();
            UpdateRuntimeStatusIndicators();
            SetFeedback(P0BattleFeedbackPresenter.BuildCatSwitch(catDefinitions[index].DisplayName));
        }

        public void SelectSkillIndicatorSlot(int slotIndex)
        {
            if (ActiveCat == null || slotIndex < 0 || slotIndex >= ActiveCat.Definition.SkillIds.Count)
            {
                return;
            }

            skillIndicatorSlotIndex = slotIndex;
            string skillId = ActiveCat.Definition.SkillIds[slotIndex];
            SetPlayerMessage(skillsById.TryGetValue(skillId, out SkillDefinition skill)
                ? "正在追踪 " + P0SkillPresenter.Describe(skill).DisplayName + " 指示。"
                : "该技能暂不可用：" + skillId);
            UpdateSkillIndicatorView();
            UpdateRuntimeStatusIndicators();
        }

        public void CastSkillBySlot(int slotIndex)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress || ActiveCat == null)
            {
                return;
            }

            IReadOnlyList<string> skillIds = ActiveCat.Definition.SkillIds;
            if (slotIndex < 0 || slotIndex >= skillIds.Count)
            {
                return;
            }

            string skillId = skillIds[slotIndex];
            skillIndicatorSlotIndex = slotIndex;
            if (!skillsById.TryGetValue(skillId, out SkillDefinition skill))
            {
                battle.NodeMetrics.RecordSkillCastBlockedByMissingDefinition();
                SetFeedback(P0BattleFeedbackPresenter.BuildSkillBlocked(skillId, "技能暂不可用"));
                return;
            }

            float cooldown = GetSkillCooldown(skill.Id);
            SkillPresentation presentation = P0SkillPresenter.Describe(skill);
            if (cooldown > 0f)
            {
                battle.NodeMetrics.RecordSkillCastBlockedByCooldown();
                SetFeedback(P0BattleFeedbackPresenter.BuildSkillBlocked(
                    presentation.DisplayName,
                    "冷却 " + cooldown.ToString("0.0") + "s"));
                return;
            }

            P0SkillTargetResult target = ResolveSkillTarget(skill);
            if (target.RequiresEnemyTarget)
            {
                if (target.HasEnemyTarget)
                {
                    battle.NodeMetrics.RecordSkillTargetAcquired();
                }
                else
                {
                    battle.NodeMetrics.RecordSkillTargetMissed();
                }
            }

            if (!target.CanCast)
            {
                battle.NodeMetrics.RecordSkillCastBlockedByTarget();
                SetFeedback(P0BattleFeedbackPresenter.BuildSkillBlocked(
                    P0SkillPresenter.Describe(skill).DisplayName,
                    "需要 " + target.Range.ToString("0.0") + "m 内目标"));
                return;
            }

            SkillCastResult result = battle.CastSkill(skill, ActiveCat, target.Enemy);
            skillCooldownsById[skill.Id] = skill.CooldownSeconds;
            SetFeedback(P0BattleFeedbackPresenter.BuildSkillCast(skill, result, target));
        }

        public void UseLitterBox()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            if (!CanUseInteractable(litterBoxAnchor, P0BattleNavigationState.DefaultInteractionRange))
            {
                battle.NodeMetrics.RecordInteractionBlockedByRange();
                SetFeedback(P0BattleFeedbackPresenter.BuildInteractionBlocked(
                    "猫砂盆",
                    "请靠近一点"));
                return;
            }

            battle.RecordLitterBoxUse();
            SetFeedback(P0BattleFeedbackPresenter.BuildInteractionSuccess("猫砂盆", "已使用"));
        }

        public void UseFeeder()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            if (!CanUseInteractable(feederAnchor, P0BattleNavigationState.DefaultInteractionRange))
            {
                battle.NodeMetrics.RecordInteractionBlockedByRange();
                SetFeedback(P0BattleFeedbackPresenter.BuildInteractionBlocked(
                    "喂食器",
                    "请靠近一点"));
                return;
            }

            battle.RecordFeederUse();
            SetFeedback(P0BattleFeedbackPresenter.BuildInteractionSuccess("喂食器", "已使用"));
        }

        public void UseBedCare()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            if (!CanUseInteractable(bedAnchor, P0BattleNavigationState.DefaultBedCareRange))
            {
                battle.NodeMetrics.RecordInteractionBlockedByRange();
                SetFeedback(P0BattleFeedbackPresenter.BuildInteractionBlocked(
                    "守床照看",
                    "请靠近一点"));
                return;
            }

            float restored = battle.RecordBedCareUse();
            SetFeedback(P0BattleFeedbackPresenter.BuildInteractionSuccess(
                "守床照看",
                "睡眠 +" + restored.ToString("0")));
        }

        public void TogglePause()
        {
            runtimeSettings.TogglePause();
            SetFeedback(P0BattleFeedbackPresenter.BuildRuntimeSettings(P0RuntimeSettingsPresenter.Build(runtimeSettings)));
        }

        public void SetBattleSpeed(float speedMultiplier)
        {
            runtimeSettings.SetBattleSpeed(speedMultiplier);
            SetFeedback(P0BattleFeedbackPresenter.BuildRuntimeSettings(P0RuntimeSettingsPresenter.Build(runtimeSettings)));
        }

        public void CollapseDiagnosticsHudForSmoke()
        {
            showDiagnosticsHud = false;
            showSmokeTools = false;
            hudScrollPosition = Vector2.zero;
        }

        private void ToggleDiagnosticsHud()
        {
            showDiagnosticsHud = !showDiagnosticsHud;
            if (!showDiagnosticsHud)
            {
                showSmokeTools = false;
                hudScrollPosition = Vector2.zero;
            }

            SetDiagnosticsMessage(showDiagnosticsHud
                ? "诊断 HUD 已开启，F10 收起。"
                : "诊断 HUD 已收起。");
        }

        private void SetFeedback(P0BattleFeedback feedback)
        {
            bool sameFeedback = lastFeedback.HasFeedback
                && feedback.HasFeedback
                && lastFeedback.BuildSummary() == feedback.BuildSummary();
            lastFeedback = feedback;
            if (!sameFeedback)
            {
                feedbackAgeSeconds = 0f;
            }

            SetPlayerMessage(feedback.HasFeedback ? feedback.BuildSummary() : "就绪");
        }

        private void TickFeedbackVisual(float deltaSeconds)
        {
            if (!lastFeedback.HasFeedback)
            {
                feedbackAgeSeconds = 0f;
                return;
            }

            feedbackAgeSeconds += deltaSeconds;
        }

        public void ExecuteInputCommand(P0InputCommand command)
        {
            switch (command)
            {
                case P0InputCommand.SelectCat1:
                    SelectCat(0);
                    break;
                case P0InputCommand.SelectCat2:
                    SelectCat(1);
                    break;
                case P0InputCommand.SelectCat3:
                    SelectCat(2);
                    break;
                case P0InputCommand.Skill1:
                    CastSkillBySlot(0);
                    break;
                case P0InputCommand.Skill2:
                    CastSkillBySlot(1);
                    break;
                case P0InputCommand.Skill3:
                    CastSkillBySlot(2);
                    break;
                case P0InputCommand.TogglePause:
                    TogglePause();
                    break;
                case P0InputCommand.SpeedHalf:
                    SetBattleSpeed(0.5f);
                    break;
                case P0InputCommand.SpeedNormal:
                    SetBattleSpeed(1f);
                    break;
                case P0InputCommand.SpeedFast:
                    SetBattleSpeed(1.5f);
                    break;
                case P0InputCommand.ToggleDiagnosticsHud:
                    ToggleDiagnosticsHud();
                    break;
                case P0InputCommand.UseBedCare:
                    UseBedCare();
                    break;
                case P0InputCommand.UseLitterBox:
                    UseLitterBox();
                    break;
                case P0InputCommand.UseFeeder:
                    UseFeeder();
                    break;
                case P0InputCommand.ContinueRoute:
                    ContinueRoute();
                    break;
                case P0InputCommand.RestartRun:
                    ConfirmRestartRun();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, "Unknown P0 input command.");
            }
        }

        private void HandleKeyboardInput()
        {
            TryExecuteKeyboardCommand(P0InputCommand.TogglePause);
            TryExecuteKeyboardCommand(P0InputCommand.SpeedHalf);
            TryExecuteKeyboardCommand(P0InputCommand.SpeedNormal);
            TryExecuteKeyboardCommand(P0InputCommand.SpeedFast);
            TryExecuteKeyboardCommand(P0InputCommand.ToggleDiagnosticsHud);
            TryExecuteKeyboardCommand(P0InputCommand.SelectCat1);
            TryExecuteKeyboardCommand(P0InputCommand.SelectCat2);
            TryExecuteKeyboardCommand(P0InputCommand.SelectCat3);
            TryExecuteKeyboardCommand(P0InputCommand.Skill1);
            TryExecuteKeyboardCommand(P0InputCommand.Skill2);
            TryExecuteKeyboardCommand(P0InputCommand.Skill3);
            TryExecuteKeyboardCommand(P0InputCommand.UseBedCare);
            TryExecuteKeyboardCommand(P0InputCommand.UseLitterBox);
            TryExecuteKeyboardCommand(P0InputCommand.UseFeeder);
            TryExecuteKeyboardCommand(P0InputCommand.ContinueRoute);
            TryExecuteKeyboardCommand(P0InputCommand.RestartRun);
        }

        private void TryExecuteKeyboardCommand(P0InputCommand command)
        {
            if (P0KeyboardInputMap.WasPressedThisFrame(command))
            {
                ExecuteInputCommand(command);
            }
        }

        private void DebugSpawnEnemy(string enemyId)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            BattleEnemyState enemy = battle.DebugSpawnEnemy(enemyId, GetSmokeTimeToBed(enemyId), "center");
            if (enemy.Definition.BehaviorType == EnemyBehaviorType.BossCallTyrant)
            {
                enemy.DebugSetBossTimers(
                    EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                    EnemyWarningFormatter.BossThrowWarningThresholdSeconds);
            }

            SyncEnemyViews();
            SetDiagnosticsMessage("调试生成：" + enemy.Definition.DisplayName + "。");
        }

        private void DebugApplyEnemyStatus(string statusTagId)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            BattleEnemyState target = GetFirstActiveEnemy();
            if (target == null)
            {
                SetDiagnosticsMessage("调试需要一个存活敌人。");
                return;
            }

            if (battle.DebugApplyStatusToEnemy(target, statusTagId))
            {
                SyncEnemyViews();
                SetDiagnosticsMessage("调试已应用：" + P0StatusIndicatorPresenter.Build(target.Statuses, target.Definition.DisplayName).Text);
            }
        }

        private void DebugApplyCatShield()
        {
            if (ActiveCat == null)
            {
                return;
            }

            ActiveCat.ApplyStatus(GetSmokeStatusDefinition(StatusTagIds.Shield));
            UpdateRuntimeStatusIndicators();
            SetDiagnosticsMessage("调试护盾已应用给 " + ActiveCat.Definition.DisplayName + "。");
        }

        private void DebugApplyBedStatus(string statusTagId, float magnitude = -1f)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            battle.DebugApplyBedStatus(statusTagId, magnitude);
            UpdateRuntimeStatusIndicators();
            SetDiagnosticsMessage("调试床标签已应用：" + statusTagId + "。");
        }

        public bool PrimeStatusHudForSmoke()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress || ActiveCat == null)
            {
                return false;
            }

            BattleEnemyState target = GetFirstActiveEnemy();
            if (target == null)
            {
                target = battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, GetSmokeTimeToBed(P0PrototypeCatalog.BlackMudNightmareId), "center");
            }

            battle.DebugApplyStatusToEnemy(target, StatusTagIds.Slow);
            battle.DebugApplyStatusToEnemy(target, StatusTagIds.Mark);
            battle.DebugApplyStatusToEnemy(target, StatusTagIds.Knockback);
            target.ApplyKnockback(1.5f);
            ActiveCat.ApplyStatus(GetSmokeStatusDefinition(StatusTagIds.Shield), 25f);
            battle.DebugApplyBedStatus(StatusTagIds.SleepStable);
            battle.DebugApplyBedStatus(StatusTagIds.Shield, 20f);

            SyncEnemyViews();
            UpdateRuntimeStatusIndicators();
            IReadOnlyList<P0StatusHudEntry> statusHudEntries = BuildStatusHudEntriesForSmoke();
            SetDiagnosticsMessage("调试状态 HUD 已准备：" + P0StatusHudPresenter.BuildCompactSummary(statusHudEntries) + "。");
            return P0StatusHudPresenter.HasP0StatusHudEntries(statusHudEntries);
        }

        public bool PrimeEnemyHudForSmoke()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return false;
            }

            EnsureSmokeEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            EnsureSmokeEnemy(P0PrototypeCatalog.ColdLightShadowId);
            BattleEnemyState boss = EnsureSmokeEnemy(P0PrototypeCatalog.CallTyrantId);
            if (boss != null)
            {
                boss.DebugSetBossTimers(
                    EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                    EnemyWarningFormatter.BossThrowWarningThresholdSeconds);
            }

            SyncEnemyViews();
            IReadOnlyList<P0EnemyHudCard> cards = BuildEnemyHudCardsForSmoke();
            SetDiagnosticsMessage("调试敌人 HUD 已准备：" + P0EnemyHudPresenter.BuildCompactSummary(cards) + "。");
            return P0EnemyHudPresenter.HasP0EnemyHudCards(cards);
        }

        private void DebugDamageOwnerSleep(float amount)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            battle.DebugDamageOwnerSleep(amount);
            UpdateOutcomeMessage();
            SetDiagnosticsMessage("调试睡眠 -" + amount.ToString("0") + "。");
        }

        private void DebugSpendHunger(float amount)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            battle.DebugSpendHunger(amount);
            SetDiagnosticsMessage("调试饱肚 -" + amount.ToString("0") + "。");
        }

        private void DebugForcePoopCountdown()
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            battle.DebugForcePoopCountdown();
            SetDiagnosticsMessage("调试已触发屎意倒计时。");
        }

        private BattleEnemyState GetFirstActiveEnemy()
        {
            if (battle == null)
            {
                return null;
            }

            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                if (battle.ActiveEnemies[i] != null && battle.ActiveEnemies[i].IsAlive)
                {
                    return battle.ActiveEnemies[i];
                }
            }

            return null;
        }

        private BattleEnemyState EnsureSmokeEnemy(string enemyId)
        {
            BattleEnemyState existing = FindActiveEnemy(enemyId);
            if (existing != null)
            {
                return existing;
            }

            return battle.DebugSpawnEnemy(enemyId, GetSmokeTimeToBed(enemyId), "center");
        }

        private BattleEnemyState FindActiveEnemy(string enemyId)
        {
            if (battle == null || string.IsNullOrWhiteSpace(enemyId))
            {
                return null;
            }

            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                if (enemy != null && enemy.IsAlive && enemy.Definition.Id == enemyId)
                {
                    return enemy;
                }
            }

            return null;
        }

        private StatusTagDefinition GetSmokeStatusDefinition(string statusTagId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusTagId)
                {
                    return statuses[i];
                }
            }

            throw new InvalidOperationException("Missing smoke status: " + statusTagId);
        }

        private float GetSmokeTimeToBed(string enemyId)
        {
            switch (enemyId)
            {
                case P0PrototypeCatalog.BlackMudNightmareId:
                    return EnemyWarningFormatter.BedWarningThresholdSeconds;
                case P0PrototypeCatalog.DreamRailToyTrainId:
                    return EnemyWarningFormatter.ChargeWarningThresholdSeconds;
                case P0PrototypeCatalog.ColdLightShadowId:
                case P0PrototypeCatalog.RedEyeAlarmId:
                    return EnemyWarningFormatter.RangedPressureWarningThresholdSeconds;
                case P0PrototypeCatalog.UnreadRedDotFlyerId:
                    return EnemyWarningFormatter.FlyingAttachWarningThresholdSeconds;
                case P0PrototypeCatalog.FallingDreamTeddyId:
                    return EnemyWarningFormatter.JumpSlamWarningThresholdSeconds;
                case P0PrototypeCatalog.CallTyrantId:
                    return 10f;
                default:
                    return -1f;
            }
        }

        private void EnsureGrayboxSceneObjects()
        {
            if (enemyRoot == null)
            {
                enemyRoot = new GameObject("EnemyRoot").transform;
                enemyRoot.SetParent(transform, false);
            }

            if (bedAnchor == null)
            {
                bedAnchor = CreateMarker("Bed", new Vector3(0f, 0.3f, -3.5f), new Vector3(2.8f, 0.35f, 1.2f), new Color(0.45f, 0.65f, 1f));
            }

            if (litterBoxAnchor == null)
            {
                litterBoxAnchor = CreateMarker("LitterBox", new Vector3(-3.4f, 0.15f, -2.8f), new Vector3(0.7f, 0.3f, 0.7f), new Color(0.62f, 0.48f, 0.38f));
            }

            if (feederAnchor == null)
            {
                feederAnchor = CreateMarker("Feeder", new Vector3(3.4f, 0.15f, -2.8f), new Vector3(0.7f, 0.3f, 0.7f), new Color(0.25f, 0.75f, 0.45f));
            }

            if (activeCatMarker == null)
            {
                activeCatMarker = CreateMarker("ActiveCat", new Vector3(0f, 0.35f, -2.6f), new Vector3(0.55f, 0.45f, 0.55f), new Color(0.95f, 0.85f, 0.35f));
            }

            if (backgroundAnchor == null)
            {
                GameObject backgroundObject = new GameObject("BedroomDreamBattleBackground");
                backgroundObject.transform.SetParent(transform, false);
                backgroundObject.transform.position = new Vector3(0f, 1.55f, 5.6f);
                backgroundAnchor = backgroundObject.transform;
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                spawnPoints = new[]
                {
                    CreateMarker("Spawn_North", new Vector3(-3f, 0.1f, 4.5f), new Vector3(0.4f, 0.2f, 0.4f), new Color(0.9f, 0.25f, 0.3f)),
                    CreateMarker("Spawn_Center", new Vector3(0f, 0.1f, 4.5f), new Vector3(0.4f, 0.2f, 0.4f), new Color(0.9f, 0.25f, 0.3f)),
                    CreateMarker("Spawn_East", new Vector3(3f, 0.1f, 4.5f), new Vector3(0.4f, 0.2f, 0.4f), new Color(0.9f, 0.25f, 0.3f))
                };
            }

            if (enemyPrefab == null)
            {
                enemyPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                enemyPrefab.name = "EnemyPrefab_Runtime";
                enemyPrefab.SetActive(false);
                enemyPrefab.transform.SetParent(transform, false);
            }

            if (skillIndicatorView == null)
            {
                GameObject indicatorObject = new GameObject("SkillIndicatorView");
                indicatorObject.transform.SetParent(transform, false);
                skillIndicatorView = indicatorObject.AddComponent<P0SkillIndicatorView>();
            }

            activeCatStatusIndicatorView = EnsureStatusIndicatorView(
                activeCatStatusIndicatorView,
                activeCatMarker,
                "ActiveCatStatusIndicator",
                new Vector3(0f, 0.9f, 0f));
            bedStatusIndicatorView = EnsureStatusIndicatorView(
                bedStatusIndicatorView,
                bedAnchor,
                "BedStatusIndicator",
                new Vector3(0f, 0.75f, 0f));
            EnsureWorldVisualAssetViews();
            SyncStaticWorldVisualAssets();
        }

        private void ApplyAutoAttack(float deltaSeconds)
        {
            if (ActiveCat == null || battle.ActiveEnemies.Count == 0)
            {
                autoAttackTimer = 0f;
                return;
            }

            autoAttackTimer += deltaSeconds;
            if (autoAttackTimer < AutoAttackIntervalSeconds)
            {
                return;
            }

            autoAttackTimer = 0f;
            float damage = CalculateAutoAttackDamage(ActiveCat);
            P0AutoAttackTargetResult target = P0AutoAttackTargetResolver.FindBestTarget(
                battle.ActiveEnemies,
                navigation.Position,
                ActiveCat,
                GetEnemyNavigationPosition);
            if (target.HasTarget)
            {
                battle.NodeMetrics.RecordAutoTargetAcquired();
            }
            else
            {
                battle.NodeMetrics.RecordAutoTargetMissed();
            }

            if (target.HasTarget && battle.ApplyDamageToEnemy(target.Enemy, damage, ActiveCat))
            {
                SetPlayerMessage(ActiveCat.Definition.DisplayName
                    + " 自动攻击 "
                    + target.Enemy.Definition.DisplayName
                    + "，伤害 "
                    + damage.ToString("0")
                    + "，距离 "
                    + target.Distance.ToString("0.0")
                    + "m");
            }
        }

        private void ApplyPlayerMovement(float deltaSeconds)
        {
            if (ActiveCat == null)
            {
                return;
            }

            Vector2 axis = P0KeyboardInputMap.ReadMovementAxis();
            if (axis.sqrMagnitude <= 0f)
            {
                return;
            }

            navigation.Move(axis, deltaSeconds, ActiveCat.Definition.MoveSpeedMultiplier);
            SyncActiveCatMarker();
        }

        private bool CanUseInteractable(Transform target, float range)
        {
            return target != null && navigation.IsWithinRange(ToNavigationPosition(target), range);
        }

        private Vector2 ToNavigationPosition(Transform target)
        {
            if (target == null)
            {
                return Vector2.zero;
            }

            Vector3 position = target.position;
            return new Vector2(position.x, position.z);
        }

        private void SyncActiveCatMarker()
        {
            if (activeCatMarker == null)
            {
                return;
            }

            Vector2 position = navigation.Position;
            activeCatMarker.position = new Vector3(position.x, 0.35f, position.y);
            UpdateSkillIndicatorView();
            UpdateRuntimeStatusIndicators();
        }

        private P0StatusIndicatorView EnsureStatusIndicatorView(
            P0StatusIndicatorView existing,
            Transform parent,
            string objectName,
            Vector3 localOffset)
        {
            if (existing != null || parent == null)
            {
                return existing;
            }

            Transform found = parent.Find(objectName);
            P0StatusIndicatorView view = found == null
                ? null
                : found.GetComponent<P0StatusIndicatorView>();
            if (view == null)
            {
                GameObject statusObject = new GameObject(objectName);
                statusObject.transform.SetParent(parent, false);
                view = statusObject.AddComponent<P0StatusIndicatorView>();
            }

            view.SetLocalOffset(localOffset);
            return view;
        }

        private void UpdateActiveCatMarkerAppearance()
        {
            if (activeCatMarker == null || catDefinitions.Count == 0 || activeCatIndex < 0 || activeCatIndex >= catDefinitions.Count)
            {
                return;
            }

            Color fallbackColor = GetCatColor(catDefinitions[activeCatIndex].Role);
            Renderer renderer = activeCatMarker.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = fallbackColor;
            }

            EnsureWorldVisualAssetViews();
            bool hasSprite = activeCatVisualView != null
                && activeCatVisualView.SetAsset(
                    P0VisualAssetCatalog.GetStarterCatCombatSprite(catDefinitions[activeCatIndex].Id),
                    fallbackColor,
                    new Vector2(1.25f, 1.25f),
                    25,
                    new Vector3(0f, 0.55f, 0f));
            SetMarkerFallbackVisible(activeCatMarker, !hasSprite);
        }

        private float CalculateAutoAttackDamage(CatBattleState cat)
        {
            switch (cat.Definition.Role)
            {
                case CatRole.Defender:
                    return 10f;
                case CatRole.Controller:
                    return 8f;
                case CatRole.Healer:
                    return 6f;
                default:
                    return 7f;
            }
        }

        private Transform CreateMarker(string markerName, Vector3 position, Vector3 scale, Color color)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = markerName;
            marker.transform.SetParent(transform, false);
            marker.transform.position = position;
            marker.transform.localScale = scale;

            Renderer renderer = marker.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }

            return marker.transform;
        }

        private void EnsureWorldVisualAssetViews()
        {
            backgroundVisualView = EnsureWorldVisualAssetView(backgroundVisualView, backgroundAnchor, "BedroomDreamBattleBackgroundVisual");
            bedVisualView = EnsureWorldVisualAssetView(bedVisualView, bedAnchor, "BedWorldVisual");
            litterBoxVisualView = EnsureWorldVisualAssetView(litterBoxVisualView, litterBoxAnchor, "LitterBoxWorldVisual");
            feederVisualView = EnsureWorldVisualAssetView(feederVisualView, feederAnchor, "FeederWorldVisual");
            activeCatVisualView = EnsureWorldVisualAssetView(activeCatVisualView, activeCatMarker, "ActiveCatWorldVisual");
        }

        private P0WorldVisualAssetView EnsureWorldVisualAssetView(
            P0WorldVisualAssetView existing,
            Transform parent,
            string componentName)
        {
            _ = componentName;
            if (existing != null || parent == null)
            {
                return existing;
            }

            P0WorldVisualAssetView view = parent.GetComponent<P0WorldVisualAssetView>();
            if (view == null)
            {
                view = parent.gameObject.AddComponent<P0WorldVisualAssetView>();
            }

            return view;
        }

        private void SyncStaticWorldVisualAssets()
        {
            if (backgroundVisualView != null)
            {
                backgroundVisualView.SetAsset(
                    P0VisualAssetCatalog.GetBedroomDreamBattleBackground(),
                    new Color(0.18f, 0.16f, 0.3f),
                    new Vector2(10.8f, 6.1f),
                    -20,
                    Vector3.zero);
            }

            if (bedVisualView != null)
            {
                bool hasSprite = bedVisualView.SetAsset(
                    P0VisualAssetCatalog.GetBedSprite(),
                    new Color(0.45f, 0.65f, 1f),
                    new Vector2(2.8f, 1.4f),
                    5,
                    new Vector3(0f, 0.6f, 0f));
                SetMarkerFallbackVisible(bedAnchor, !hasSprite);
            }

            if (litterBoxVisualView != null)
            {
                bool hasSprite = litterBoxVisualView.SetAsset(
                    P0VisualAssetCatalog.GetLitterBoxSprite(),
                    new Color(0.62f, 0.48f, 0.38f),
                    new Vector2(0.9f, 0.9f),
                    7,
                    new Vector3(0f, 0.4f, 0f));
                SetMarkerFallbackVisible(litterBoxAnchor, !hasSprite);
            }

            if (feederVisualView != null)
            {
                bool hasSprite = feederVisualView.SetAsset(
                    P0VisualAssetCatalog.GetFeederSprite(),
                    new Color(0.25f, 0.75f, 0.45f),
                    new Vector2(0.9f, 0.9f),
                    7,
                    new Vector3(0f, 0.4f, 0f));
                SetMarkerFallbackVisible(feederAnchor, !hasSprite);
            }
        }

        private static void SetMarkerFallbackVisible(Transform marker, bool visible)
        {
            if (marker == null)
            {
                return;
            }

            Renderer renderer = marker.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = visible;
            }
        }

        private void LoadPrototypeData(RunProgressionState run)
        {
            catDefinitions.Clear();
            IReadOnlyList<CatDefinition> starters = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < starters.Count; i++)
            {
                if (run == null || run.Roster.HasCat(starters[i].Id))
                {
                    catDefinitions.Add(P0CatUpgradeRuntimeCatalog.ApplyRunUpgrades(starters[i], run == null ? null : run.CatUpgrades));
                }
            }

            if (catDefinitions.Count == 0)
            {
                catDefinitions.AddRange(starters);
            }

            skillsById.Clear();
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                skillsById[skills[i].Id] = skills[i];
            }

            IReadOnlyList<SkillDefinition> upgradedSkills = P0CatUpgradeRuntimeCatalog.CreateSelectedSkillDefinitions(
                run == null ? null : run.CatUpgrades);
            for (int i = 0; i < upgradedSkills.Count; i++)
            {
                skillsById[upgradedSkills[i].Id] = upgradedSkills[i];
            }
        }

        private void ResetCats()
        {
            cats.Clear();
            RunProgressionState run = P0RunSession.CurrentRun;
            for (int i = 0; i < catDefinitions.Count; i++)
            {
                if (run == null)
                {
                    cats.Add(new CatBattleState(catDefinitions[i]));
                    continue;
                }

                RunCatVitalSnapshot snapshot = run.CatVitals.GetOrCreate(catDefinitions[i]);
                cats.Add(new CatBattleState(catDefinitions[i], snapshot.CurrentHp, snapshot.WeakRemainingSeconds));
            }
        }

        private void TickSkillCooldowns(float deltaSeconds)
        {
            List<string> keys = new List<string>(skillCooldownsById.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string skillId = keys[i];
                skillCooldownsById[skillId] = Mathf.Max(0f, skillCooldownsById[skillId] - deltaSeconds);
            }
        }

        private void TickCats(float deltaSeconds)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                bool wasWeak = cats[i].Vital.IsWeak;
                cats[i].Tick(deltaSeconds);
                if (wasWeak && !cats[i].Vital.IsWeak)
                {
                    SetPlayerMessage(catDefinitions[i].DisplayName + " 已恢复。");
                }
            }
        }

        private void ApplyEnemyPressureToActiveCat(float deltaSeconds)
        {
            if (ActiveCat == null || battle.ActiveEnemies.Count == 0)
            {
                catPressureTimer = 0f;
                return;
            }

            catPressureTimer += deltaSeconds;
            if (catPressureTimer < CatPressureIntervalSeconds)
            {
                return;
            }

            catPressureTimer = 0f;
            P0EnemyPressureResult pressure = P0EnemyPressureResolver.FindBestPressureSource(
                battle.ActiveEnemies,
                navigation.Position,
                GetEnemyNavigationPosition);
            if (!pressure.HasEnemy)
            {
                return;
            }

            BattleEnemyState enemy = pressure.Enemy;
            P0CatPressureApplication application = P0CatPressureApplier.Apply(
                battle.NodeMetrics,
                ActiveCat,
                enemy,
                CatPressureDamageScale,
                pressure.DamageMultiplier);
            if (application.BecameWeak)
            {
                SetFeedback(P0BattleFeedbackPresenter.BuildCatPressure(
                    enemy.Definition.DisplayName,
                    ActiveCat.Definition.DisplayName,
                    application,
                    pressure.Distance));
                SelectFirstAvailableCat();
                return;
            }

            if (application.DamageTaken > 0f || application.DamageAbsorbed > 0f)
            {
                SetFeedback(P0BattleFeedbackPresenter.BuildCatPressure(
                    enemy.Definition.DisplayName,
                    ActiveCat.Definition.DisplayName,
                    application,
                    pressure.Distance));
            }
        }

        private void SelectFirstAvailableCat()
        {
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Vital.CanSwitchTo)
                {
                    activeCatIndex = i;
                    UpdateActiveCatMarkerAppearance();
                    SelectDefaultSkillIndicatorSlot();
                    return;
                }
            }
        }

        private Vector2 GetEnemyNavigationPosition(BattleEnemyState enemy)
        {
            if (enemy == null)
            {
                return Vector2.zero;
            }

            Transform spawnPoint = GetSpawnPointForGate(enemy.SpawnGateId, enemy.InstanceId);
            Vector3 spawnPosition = spawnPoint == null ? Vector3.zero : spawnPoint.position;
            Vector3 bedPosition = bedAnchor == null ? Vector3.zero : bedAnchor.position;
            float initialTimeToBed = enemy.Definition.MoveSpeed <= 0f
                ? Mathf.Max(0.1f, enemy.TimeToBedSeconds)
                : Mathf.Max(0.1f, EnemyPathDistance / enemy.Definition.MoveSpeed);
            float progress = 1f - Mathf.Clamp01(enemy.TimeToBedSeconds / initialTimeToBed);
            Vector3 position = Vector3.Lerp(spawnPosition, bedPosition, progress);
            return new Vector2(position.x, position.z);
        }

        private void SyncEnemyViews()
        {
            if (battle == null)
            {
                return;
            }

            bool showDiagnosticWorldLabels = ShouldShowWorldDiagnosticLabels();
            bool showWarningWorldVisuals = ShouldShowWorldWarningVisuals();
            HashSet<int> activeIds = new HashSet<int>();
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                activeIds.Add(enemy.InstanceId);

                if (!enemyViewsById.TryGetValue(enemy.InstanceId, out GrayboxEnemyView view))
                {
                    view = CreateEnemyView(enemy);
                    enemyViewsById.Add(enemy.InstanceId, view);
                }

                view.Sync(showDiagnosticWorldLabels, showWarningWorldVisuals);
            }

            List<int> idsToRemove = new List<int>();
            foreach (KeyValuePair<int, GrayboxEnemyView> pair in enemyViewsById)
            {
                if (!activeIds.Contains(pair.Key))
                {
                    if (pair.Value != null)
                    {
                        ReleaseEnemyView(pair.Value);
                    }

                    idsToRemove.Add(pair.Key);
                }
            }

            for (int i = 0; i < idsToRemove.Count; i++)
            {
                enemyViewsById.Remove(idsToRemove[i]);
            }
        }

        private GrayboxEnemyView CreateEnemyView(BattleEnemyState enemy)
        {
            Transform spawnPoint = GetSpawnPointForGate(enemy.SpawnGateId, enemy.InstanceId);
            GrayboxEnemyView view = enemyViewPool == null ? CreateEnemyViewInstance() : enemyViewPool.Rent();
            GameObject enemyObject = view.gameObject;
            enemyObject.transform.SetParent(enemyRoot, false);
            enemyObject.transform.position = spawnPoint.position;
            enemyObject.transform.rotation = Quaternion.identity;
            enemyObject.name = "Enemy_" + enemy.InstanceId + "_" + enemy.Definition.Id;
            enemyObject.SetActive(true);

            view.Initialize(
                enemy,
                spawnPoint.position,
                bedAnchor.position,
                GetEnemyColor(enemy.Definition.Id),
                P0VisualAssetCatalog.GetEnemyCombatSprite(enemy.Definition.Id),
                ShouldShowWorldDiagnosticLabels(),
                ShouldShowWorldWarningVisuals());
            return view;
        }

        private bool ShouldShowWorldDiagnosticLabels()
        {
            return showDiagnosticsHud
                && battle != null
                && battle.Outcome == BattleOutcome.InProgress;
        }

        private bool ShouldShowWorldWarningVisuals()
        {
            return battle != null && battle.Outcome == BattleOutcome.InProgress;
        }

        private GrayboxEnemyView CreateEnemyViewInstance()
        {
            GameObject enemyObject = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, enemyRoot);
            enemyObject.name = "Enemy_Pooled";
            GrayboxEnemyView view = enemyObject.GetComponent<GrayboxEnemyView>();
            if (view == null)
            {
                view = enemyObject.AddComponent<GrayboxEnemyView>();
            }

            enemyObject.SetActive(false);
            return view;
        }

        private static void PrepareRentedEnemyView(GrayboxEnemyView view)
        {
            view.gameObject.SetActive(true);
        }

        private void PrepareReleasedEnemyView(GrayboxEnemyView view)
        {
            view.ResetForPool();
            view.gameObject.name = "Enemy_Pooled";
            if (enemyRoot != null)
            {
                view.transform.SetParent(enemyRoot, false);
            }

            view.gameObject.SetActive(false);
        }

        private void ReleaseEnemyView(GrayboxEnemyView view)
        {
            if (enemyViewPool == null || !enemyViewPool.Release(view))
            {
                Destroy(view.gameObject);
            }
        }

        private static void DestroyPooledEnemyView(GrayboxEnemyView view)
        {
            if (view != null)
            {
                Destroy(view.gameObject);
            }
        }

        private Transform GetSpawnPointForGate(string spawnGateId, int instanceId)
        {
            if (!string.IsNullOrWhiteSpace(spawnGateId))
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (spawnPoints[i] != null && spawnPoints[i].name.IndexOf(spawnGateId, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return spawnPoints[i];
                    }
                }
            }

            return spawnPoints[(instanceId - 1) % spawnPoints.Length];
        }

        private Color GetEnemyColor(string enemyId)
        {
            if (enemyId == P0PrototypeCatalog.CallTyrantId)
            {
                return new Color(1f, 0.2f, 0.2f);
            }

            if (enemyId == P0PrototypeCatalog.ColdLightShadowId)
            {
                return new Color(0.35f, 0.85f, 1f);
            }

            return new Color(0.08f, 0.06f, 0.12f);
        }

        private Color GetCatColor(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return new Color(0.95f, 0.82f, 0.25f);
                case CatRole.Controller:
                    return new Color(0.62f, 0.48f, 0.95f);
                case CatRole.Healer:
                    return new Color(0.35f, 0.85f, 0.65f);
                default:
                    return new Color(0.9f, 0.9f, 0.9f);
            }
        }

        private void ClearEnemyViews()
        {
            foreach (KeyValuePair<int, GrayboxEnemyView> pair in enemyViewsById)
            {
                if (pair.Value != null)
                {
                    ReleaseEnemyView(pair.Value);
                }
            }

            enemyViewsById.Clear();
        }

        private void UpdateOutcomeMessage()
        {
            if (battle.Outcome == BattleOutcome.Victory)
            {
                RunNodeCompletionReport report = RecordRouteCompletionIfNeeded(NodeResult.Success);
                string resultMessage = report == null
                    ? "胜利。用时 " + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s"
                    : report.BuildSummary() + "。用时 " + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s";
                SetFeedback(P0BattleFeedbackPresenter.BuildBattleResult(
                    BattleOutcome.Victory,
                    battle.NodeMetrics.DurationSeconds,
                    report == null ? string.Empty : report.BuildSummary()));
                SetPlayerMessage(resultMessage);
            }
            else if (battle.Outcome == BattleOutcome.Defeat)
            {
                RunNodeCompletionReport report = RecordRouteCompletionIfNeeded(NodeResult.Failure);
                string resultMessage = report == null
                    ? "失败。主人睡眠度崩溃。"
                    : report.BuildSummary() + "。主人睡眠度崩溃。";
                SetFeedback(P0BattleFeedbackPresenter.BuildBattleResult(
                    BattleOutcome.Defeat,
                    battle.NodeMetrics.DurationSeconds,
                    report == null ? "主人睡眠度崩溃" : report.BuildSummary()));
                SetPlayerMessage(resultMessage);
            }
        }

        private RunNodeCompletionReport RecordRouteCompletionIfNeeded(NodeResult result)
        {
            if (routeCompletionRecorded)
            {
                return lastCompletionReport;
            }

            RunProgressionState run = P0RunSession.EnsureProgression();
            if (!battleStartContext.ShouldPersistRunState)
            {
                routeCompletionRecorded = true;
                lastCompletionReport = null;
                return null;
            }

            if (battle != null)
            {
                run.CoreValues.Capture(battle.OwnerSleep, battle.TeamPoop, battle.TeamHunger);
                CaptureCatVitals(run);
            }

            lastCompletionReport = P0RunSession.CompleteCurrentNode(result);
            routeCompletionRecorded = true;
            return lastCompletionReport;
        }

        private void DrawRuntimeControls()
        {
            P0PauseSettingsSurface settingsSurface = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(runtimeSettings, restartConfirmationOpen);
            BeginResponsiveRow();
            for (int i = 0; i < settingsSurface.Actions.Count; i++)
            {
                P0RuntimeSettingsAction action = settingsSurface.Actions[i];
                GUI.enabled = action.IsEnabled;
                if (GUILayout.Button(
                    action.Label,
                    GUILayout.MinWidth(0f),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
                {
                    ExecuteRuntimeSettingsAction(action);
                }
            }

            GUI.enabled = true;
            EndResponsiveRow();
            GUILayout.Label(settingsSurface.RuntimeSettings.BuildSummary(), WrappedHudLabel);
            GUILayout.Label(settingsSurface.BuildSummary(), WrappedHudLabel);
            if (settingsSurface.IsRestartConfirmationOpen)
            {
                GUILayout.Label(settingsSurface.RestartConfirmationTitle, WrappedHudLabel);
                GUILayout.Label(settingsSurface.RestartConfirmationDetail, WrappedHudLabel);
            }

            GUILayout.Space(6f);
        }

        private void DrawFeedbackVisual(P0BattleFeedbackVisualState visual)
        {
            bool hasAsset = visual.VisualAsset.HasAsset;
            float feedbackHeight = IsHudNarrow
                ? P0ImGuiLayout.Scaled(hasAsset ? 112f : 96f)
                : P0ImGuiLayout.Scaled(hasAsset ? 86f : 74f);
            Rect rect = GUILayoutUtility.GetRect(1f, feedbackHeight, GUILayout.ExpandWidth(true));
            Rect backgroundRect = new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 2f);
            Rect accentRect = new Rect(backgroundRect.x, backgroundRect.y, P0ImGuiLayout.Scaled(5f), backgroundRect.height);
            float iconSize = Mathf.Min(P0ImGuiLayout.Scaled(46f), backgroundRect.width * 0.18f);
            float contentX = backgroundRect.x + (hasAsset ? iconSize + P0ImGuiLayout.Scaled(26f) : P0ImGuiLayout.Scaled(10f));
            float contentWidth = Mathf.Max(P0ImGuiLayout.Scaled(24f), backgroundRect.xMax - contentX - P0ImGuiLayout.Scaled(8f));
            Rect titleRect = new Rect(contentX, backgroundRect.y + P0ImGuiLayout.Scaled(6f), contentWidth, P0ImGuiLayout.Scaled(20f));
            Rect detailRect = new Rect(contentX, backgroundRect.y + P0ImGuiLayout.Scaled(28f), contentWidth, P0ImGuiLayout.Scaled(26f));
            Rect metaRect = new Rect(contentX, backgroundRect.y + P0ImGuiLayout.Scaled(56f), contentWidth, P0ImGuiLayout.Scaled(40f));
            Rect barBackgroundRect = new Rect(
                contentX,
                backgroundRect.yMax - P0ImGuiLayout.Scaled(7f),
                Mathf.Max(0f, contentWidth),
                P0ImGuiLayout.Scaled(3f));
            Rect barFillRect = new Rect(
                barBackgroundRect.x,
                barBackgroundRect.y,
                barBackgroundRect.width * visual.PulseFill01,
                barBackgroundRect.height);

            Color previousColor = GUI.color;
            Color previousContentColor = GUI.contentColor;
            Color accentColor = ToUnityColor(visual.AccentColor);

            GUI.color = ToUnityColor(visual.BackgroundColor);
            GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture);
            GUI.color = accentColor;
            GUI.DrawTexture(accentRect, Texture2D.whiteTexture);
            if (hasAsset)
            {
                Rect iconRect = new Rect(
                    backgroundRect.x + P0ImGuiLayout.Scaled(14f),
                    backgroundRect.y + P0ImGuiLayout.Scaled(14f),
                    iconSize,
                    iconSize);
                Rect iconBackingRect = new Rect(
                    iconRect.x - P0ImGuiLayout.Scaled(4f),
                    iconRect.y - P0ImGuiLayout.Scaled(4f),
                    iconRect.width + P0ImGuiLayout.Scaled(8f),
                    iconRect.height + P0ImGuiLayout.Scaled(8f));
                GUI.color = new Color(0f, 0f, 0f, 0.22f);
                GUI.DrawTexture(iconBackingRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
                P0ImGuiVisualAssetDrawer.DrawTexture(visual.VisualAsset, iconRect, ScaleMode.ScaleToFit);
            }

            GUI.color = new Color(0f, 0f, 0f, 0.3f);
            GUI.DrawTexture(barBackgroundRect, Texture2D.whiteTexture);
            GUI.color = accentColor;
            GUI.DrawTexture(barFillRect, Texture2D.whiteTexture);

            GUI.color = previousColor;
            GUI.contentColor = ToUnityColor(visual.TextColor);
            GUI.Label(titleRect, visual.BuildTitleLabel(), HudSectionLabel);
            GUI.Label(detailRect, visual.Feedback.Detail, WrappedHudLabel);
            GUI.Label(
                metaRect,
                "反馈 " + visual.RemainingSeconds.ToString("0.00") + "s  " + visual.BuildSummary(),
                WrappedHudLabel);

            GUI.contentColor = previousContentColor;
            GUI.color = previousColor;
        }

        private static Color ToUnityColor(P0BattleFeedbackColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private static Color ToUnityColor(P0CatHudColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private static Color ToUnityColor(P0SkillHudColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private void DrawSmokeControls()
        {
            GUI.enabled = battle != null && battle.Outcome == BattleOutcome.InProgress;
            if (GUILayout.Button(showSmokeTools ? "隐藏调试工具" : "显示调试工具", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                showSmokeTools = !showSmokeTools;
            }

            if (!showSmokeTools)
            {
                GUI.enabled = true;
                return;
            }

            GUILayout.Label("调试：生成 P0 敌人");
            BeginResponsiveRow();
            if (GUILayout.Button("黑泥", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            }

            if (GUILayout.Button("火车", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.DreamRailToyTrainId);
            }

            if (GUILayout.Button("冷光", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.ColdLightShadowId);
            }

            if (GUILayout.Button("闹铃", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.RedEyeAlarmId);
            }

            EndResponsiveRow();
            BeginResponsiveRow();
            if (GUILayout.Button("飞虫", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.UnreadRedDotFlyerId);
            }

            if (GUILayout.Button("玩具熊", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.FallingDreamTeddyId);
            }

            if (GUILayout.Button("首领", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpawnEnemy(P0PrototypeCatalog.CallTyrantId);
            }

            EndResponsiveRow();

            GUILayout.Label("调试：状态标签");
            BeginResponsiveRow();
            if (GUILayout.Button("敌人缓速", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyEnemyStatus(StatusTagIds.Slow);
            }

            if (GUILayout.Button("敌人标记", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyEnemyStatus(StatusTagIds.Mark);
            }

            if (GUILayout.Button("敌人击退", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyEnemyStatus(StatusTagIds.Knockback);
            }

            EndResponsiveRow();
            BeginResponsiveRow();
            if (GUILayout.Button("猫护盾", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyCatShield();
            }

            if (GUILayout.Button("床安眠", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyBedStatus(StatusTagIds.SleepStable);
            }

            if (GUILayout.Button("床护盾", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugApplyBedStatus(StatusTagIds.Shield, 20f);
            }

            EndResponsiveRow();

            GUILayout.Label("调试：核心数值");
            BeginResponsiveRow();
            if (GUILayout.Button("睡眠 -35", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugDamageOwnerSleep(35f);
            }

            if (GUILayout.Button("饱肚 -35", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugSpendHunger(35f);
            }

            if (GUILayout.Button("屎意倒计时", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                DebugForcePoopCountdown();
            }

            EndResponsiveRow();
            GUI.enabled = true;
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
        }

        private void DrawBattleState()
        {
            if (battle == null)
            {
                DrawHudSection(new P0BattleHudSection("目标", new[] { "战斗：未开始" }));
                if (GUILayout.Button("开始战斗", GUILayout.Height(P0ImGuiLayout.ButtonHeight)))
                {
                    BeginBattle();
                }

                return;
            }

            if (!showDiagnosticsHud)
            {
                DrawPlayerBattleState();
                return;
            }

            DrawDiagnosticBattleState();
        }

        private void DrawDiagnosticBattleState()
        {
            IReadOnlyList<P0BattleHudSection> sections = BuildBattleHudSectionsForSmoke();
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].Title == "核心数值")
                {
                    DrawCoreValueIcons();
                }

                DrawHudSection(sections[i]);
            }

            DrawWarningSummary();

            IReadOnlyList<P0EnemyHudCard> enemyHudCards = BuildEnemyHudCardsForSmoke();
            if (enemyHudCards.Count > 0)
            {
                DrawEnemyHudSection(enemyHudCards);
            }

            IReadOnlyList<P0StatusHudEntry> statusHudEntries = BuildStatusHudEntriesForSmoke();
            if (statusHudEntries.Count > 0)
            {
                DrawStatusHudSection(statusHudEntries);
            }
        }

        private void DrawPlayerBattleState()
        {
            DrawCoreValueIcons();
            P0BattlePlayerBrief brief = BuildBattlePlayerBriefForSmoke();
            if (P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief))
            {
                DrawHudSection(brief.ToHudSection());
            }

            if (battle.Outcome != BattleOutcome.InProgress)
            {
                DrawPlayerResultActions();
            }
            DrawHudSection(new P0BattleHudSection(
                "目标",
                new[]
                {
                    P0BattleHudPromptPresenter.Build(battle, cats).BuildSummary(),
                    BuildPlayerBattlePace()
                }));
            DrawPlayerWarningSummary();

            IReadOnlyList<P0EnemyHudCard> enemyHudCards = BuildEnemyHudCardsForSmoke();
            string threatBrief = BuildPlayerThreatBrief(enemyHudCards);
            if (!string.IsNullOrWhiteSpace(threatBrief))
            {
                GUILayout.Label(threatBrief, WrappedHudLabel);
            }

            IReadOnlyList<P0StatusHudEntry> statusHudEntries = BuildStatusHudEntriesForSmoke();
            string statusBrief = BuildPlayerStatusBrief(statusHudEntries);
            if (!string.IsNullOrWhiteSpace(statusBrief))
            {
                GUILayout.Label(statusBrief, WrappedHudLabel);
            }

            DrawRouteState();
            if (battle.Outcome != BattleOutcome.InProgress)
            {
                DrawResultSummary();
            }
        }

        private string BuildPlayerBattlePace()
        {
            if (battle == null)
            {
                return "战况：未开始";
            }

            return "战况："
                + FormatBattleOutcome(battle.Outcome)
                + "  时间 "
                + battle.BattleTimeSeconds.ToString("0.0")
                + "s  敌人 "
                + battle.ActiveEnemies.Count
                + "  "
                + BuildNavigationSummary();
        }

        private static string BuildPlayerThreatBrief(IReadOnlyList<P0EnemyHudCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                return string.Empty;
            }

            P0EnemyHudCard primary = cards[0];
            int warningCount = 0;
            int bossCount = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].HasWarning)
                {
                    warningCount++;
                }

                if (cards[i].IsBoss)
                {
                    bossCount++;
                }

                if (cards[i].IsPressureSource || cards[i].HasWarning)
                {
                    primary = cards[i];
                    if (cards[i].IsPressureSource)
                    {
                        break;
                    }
                }
            }

            string brief = "威胁：" + primary.DisplayName + " " + primary.ThreatToken
                + "，目标 " + primary.TargetToken
                + "，应对 " + primary.CounterHint;
            if (primary.HasWarning)
            {
                brief += "，预警 " + primary.WarningText;
            }
            else if (warningCount > 0)
            {
                brief += "，场上有预警";
            }

            if (bossCount > 0 && !primary.IsBoss)
            {
                brief += "，首领在场";
            }

            return brief;
        }

        private static string BuildPlayerStatusBrief(IReadOnlyList<P0StatusHudEntry> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                return string.Empty;
            }

            List<string> parts = new List<string>();
            AddFirstStatusPart(entries, parts, P0StatusHudTargetKind.Bed, "床");
            AddFirstStatusPart(entries, parts, P0StatusHudTargetKind.Enemy, "敌人");
            AddFirstStatusPart(entries, parts, P0StatusHudTargetKind.Cat, "猫");

            return parts.Count == 0 ? string.Empty : "状态：" + string.Join("；", parts.ToArray());
        }

        private static void AddFirstStatusPart(
            IReadOnlyList<P0StatusHudEntry> entries,
            List<string> parts,
            P0StatusHudTargetKind targetKind,
            string label)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].TargetKind != targetKind || string.IsNullOrWhiteSpace(entries[i].ResponseSummary))
                {
                    continue;
                }

                parts.Add(label + " " + entries[i].TargetLabel + "，" + entries[i].ResponseSummary);
                return;
            }
        }

        private static string FormatBattleOutcome(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return "胜利";
                case BattleOutcome.Defeat:
                    return "失败";
                case BattleOutcome.InProgress:
                default:
                    return "进行中";
            }
        }

        public IReadOnlyList<P0BattleHudSection> BuildBattleHudSectionsForSmoke()
        {
            RunNodeCompletionReport completionReport = lastCompletionReport ?? P0RunSession.LastCompletionReport;
            return P0BattleHudSummaryPresenter.BuildSections(
                battle,
                cats,
                activeCatIndex,
                BuildNavigationSummary(),
                P0RunSession.CurrentRun,
                completionReport);
        }

        public IReadOnlyList<P0StatusHudEntry> BuildStatusHudEntriesForSmoke()
        {
            return P0StatusHudPresenter.BuildEntries(battle, cats);
        }

        public IReadOnlyList<P0EnemyHudCard> BuildEnemyHudCardsForSmoke()
        {
            if (battle == null)
            {
                return Array.Empty<P0EnemyHudCard>();
            }

            P0EnemyPressureResult pressure = P0EnemyPressureResolver.FindBestPressureSource(
                battle.ActiveEnemies,
                navigation.Position,
                GetEnemyNavigationPosition);
            return P0EnemyHudPresenter.BuildCards(battle.ActiveEnemies, pressure);
        }

        private string BuildNavigationSummary()
        {
            return navigation.BuildDistanceSummary(
                ToNavigationPosition(bedAnchor),
                ToNavigationPosition(litterBoxAnchor),
                ToNavigationPosition(feederAnchor));
        }

        private void DrawHudSection(P0BattleHudSection section)
        {
            GUILayout.Space(6f);
            GUILayout.Label(section.Title, HudSectionLabel);
            for (int i = 0; i < section.Lines.Count; i++)
            {
                GUILayout.Label(section.Lines[i], WrappedHudLabel);
            }
        }

        private void DrawStatusHudSection(IReadOnlyList<P0StatusHudEntry> entries)
        {
            GUILayout.Space(6f);
            GUILayout.Label("状态 HUD", HudSectionLabel);
            GUILayout.Label(P0StatusHudPresenter.BuildCompactSummary(entries), WrappedHudLabel);
            for (int i = 0; i < entries.Count; i++)
            {
                if (IsHudNarrow)
                {
                    DrawStatusHudIcons(entries[i]);
                    GUILayout.Label(entries[i].BuildSummary(), WrappedHudLabel);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    DrawStatusHudIcons(entries[i]);
                    GUILayout.Label(entries[i].BuildSummary(), WrappedHudLabel);
                    GUILayout.EndHorizontal();
                }
            }
        }

        private void DrawStatusHudIcons(P0StatusHudEntry entry)
        {
            if (entry.StatusIcons.Count == 0)
            {
                return;
            }

            for (int i = 0; i < entry.StatusIcons.Count; i++)
            {
                P0StatusHudIconEntry icon = entry.StatusIcons[i];
                if (P0ImGuiVisualAssetDrawer.DrawInlineIcon(icon.CompactIconAsset, 22f)
                    || P0ImGuiVisualAssetDrawer.DrawInlineIcon(icon.IconAsset, 22f))
                {
                    continue;
                }

                GUILayout.Label(icon.StatusTagId, GUILayout.Width(58f));
            }
        }

        private void DrawEnemyHudSection(IReadOnlyList<P0EnemyHudCard> cards)
        {
            List<string> lines = new List<string>
            {
                P0EnemyHudPresenter.BuildCompactSummary(cards)
            };
            for (int i = 0; i < cards.Count; i++)
            {
                lines.Add(cards[i].BuildSummary());
            }

            DrawHudSection(new P0BattleHudSection("敌人 HUD", lines));
        }

        private void DrawWarningSummary()
        {
            if (battle == null)
            {
                return;
            }

            List<string> summaries = null;
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                string warningText = EnemyWarningFormatter.Format(enemy);
                if (string.IsNullOrWhiteSpace(warningText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                    enemy,
                    GetEnemyNavigationPosition(enemy),
                    ToNavigationPosition(bedAnchor));
                if (warning.VisualAsset.HasAsset)
                {
                    P0ImGuiVisualAssetDrawer.DrawIcon(warning.VisualAsset, 52f);
                }

                summaries.Add(enemy.Definition.DisplayName + "：" + warningText);
            }

            if (summaries != null)
            {
                GUILayout.Label("预警：" + string.Join("；", summaries));
            }
        }

        private void DrawPlayerWarningSummary()
        {
            if (battle == null)
            {
                return;
            }

            List<string> summaries = null;
            List<P0VisualAssetReference> visualAssets = null;
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                string warningText = EnemyWarningFormatter.Format(enemy);
                if (string.IsNullOrWhiteSpace(warningText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                summaries.Add(enemy.Definition.DisplayName + "：" + warningText);

                P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                    enemy,
                    GetEnemyNavigationPosition(enemy),
                    ToNavigationPosition(bedAnchor));
                if (!warning.VisualAsset.HasAsset)
                {
                    continue;
                }

                if (visualAssets == null)
                {
                    visualAssets = new List<P0VisualAssetReference>();
                }

                visualAssets.Add(warning.VisualAsset);
            }

            if (summaries == null)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            if (visualAssets != null)
            {
                int iconCount = Math.Min(visualAssets.Count, 3);
                for (int i = 0; i < iconCount; i++)
                {
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(visualAssets[i], P0ImGuiLayout.Scaled(22f));
                }

                if (visualAssets.Count > iconCount)
                {
                    GUILayout.Label("+" + (visualAssets.Count - iconCount), GUILayout.Width(P0ImGuiLayout.Scaled(24f)));
                }
            }

            GUILayout.Label("预警：" + string.Join("；", summaries), WrappedHudLabel);
            GUILayout.EndHorizontal();
        }

        private void DrawStatusSummary()
        {
            if (battle == null)
            {
                return;
            }

            string bedStatuses = StatusDisplayFormatter.FormatCollection(battle.BedStatuses);
            if (!string.IsNullOrWhiteSpace(bedStatuses))
            {
                GUILayout.Label("床标签：" + bedStatuses);
            }

            string enemyStatuses = BuildEnemyStatusSummary();
            if (!string.IsNullOrWhiteSpace(enemyStatuses))
            {
                GUILayout.Label("敌人标签：" + enemyStatuses);
            }

            string catStatuses = BuildCatStatusSummary();
            if (!string.IsNullOrWhiteSpace(catStatuses))
            {
                GUILayout.Label("猫咪标签：" + catStatuses);
            }
        }

        private string BuildEnemyStatusSummary()
        {
            List<string> summaries = null;
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                string statusText = StatusDisplayFormatter.FormatCollection(battle.ActiveEnemies[i].Statuses);
                if (string.IsNullOrWhiteSpace(statusText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                summaries.Add(battle.ActiveEnemies[i].Definition.DisplayName + "：" + statusText);
            }

            return summaries == null ? string.Empty : string.Join("；", summaries);
        }

        private string BuildCatStatusSummary()
        {
            List<string> summaries = null;
            for (int i = 0; i < cats.Count; i++)
            {
                string statusText = StatusDisplayFormatter.FormatCollection(cats[i].Statuses);
                if (string.IsNullOrWhiteSpace(statusText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                summaries.Add(catDefinitions[i].DisplayName + "：" + statusText);
            }

            return summaries == null ? string.Empty : string.Join("；", summaries);
        }

        private void DrawRunState()
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null)
            {
                return;
            }

            GUILayout.Label("路线：梦屑 " + run.Wallet.DreamShards + " 小鱼干 " + run.Wallet.FishTreats + " 祝福 " + run.Blessings.Count + " 等级 " + run.Blessings.TotalLevel);
            GUILayout.Label("祝福：" + run.Blessings.BuildSummary());
            GUILayout.Label("路线核心：" + P0CoreValuePresenter.BuildRunCoreSummary(run.CoreValues));
            GUILayout.Label("路线猫咪：" + BuildRunCatVitalSummary(run));
            GUILayout.Label("待触发事件：" + run.PendingBattleModifiers.BuildSummary());
        }

        private void CaptureCatVitals(RunProgressionState run)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                CatBattleState cat = cats[i];
                run.CatVitals.Capture(
                    cat.Definition.Id,
                    cat.Vital.MaxHp,
                    cat.Vital.CurrentHp,
                    cat.Vital.WeakRemainingSeconds);
            }
        }

        private string BuildRunCatVitalSummary(RunProgressionState run)
        {
            if (run == null || run.CatVitals.Count == 0)
            {
                return "未初始化";
            }

            List<string> summaries = new List<string>();
            for (int i = 0; i < run.Roster.CatIds.Count; i++)
            {
                if (!run.CatVitals.TryGet(run.Roster.CatIds[i], out RunCatVitalSnapshot snapshot))
                {
                    continue;
                }

                CatPresentation presentation = P0CatPresenter.Describe(snapshot.CatId);
                summaries.Add(presentation.BuildVitalLabel(
                    snapshot.CurrentHp,
                    snapshot.MaxHp,
                    snapshot.IsWeak,
                    snapshot.WeakRemainingSeconds));
            }

            return summaries.Count == 0 ? "未初始化" : string.Join("; ", summaries);
        }

        private void DrawRouteState()
        {
            RunRouteState route = P0RunSession.CurrentRoute;
            if (route == null)
            {
                return;
            }

            string routeText = "路线：" + route.CompletedCount + "/" + route.Route.LayerCount;
            if (route.CurrentNode != null)
            {
                routeText += " 下一层 " + route.CurrentNode.Layer + " " + P0RouteNodePresenter.Describe(route.CurrentNode, P0RunSession.CurrentRun).Title;
            }
            else if (route.IsCleared)
            {
                routeText += " 已通关";
            }
            else if (route.IsFailed)
            {
                routeText += " 已失败";
            }

            GUILayout.Label(routeText);
        }

        private void DrawPlayerResultActions()
        {
            GUILayout.Space(6f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("继续路线", WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                ContinueRoute();
            }

            if (GUILayout.Button("回到猫窝", WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                ReturnToCatRoom();
            }

            if (GUILayout.Button("重新开始", WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                RequestRestartRun();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawResultSummary()
        {
            GUILayout.Space(8f);
            GUILayout.Label("结算");
            GUILayout.Label("用时：" + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s");
            RunNodeCompletionReport report = lastCompletionReport ?? P0RunSession.LastCompletionReport;
            if (report != null)
            {
                GUILayout.Label("路线结果：" + report.BuildSummary());
            }

            GUILayout.Label("睡眠变化：" + battle.NodeMetrics.SleepDelta.ToString("0.0"));
            GUILayout.Label("拉屎次数：" + battle.NodeMetrics.PoopIncidents + "  睡眠上限损失：" + battle.NodeMetrics.SleepMaxLost.ToString("0"));
            GUILayout.Label("床 / 猫砂盆 / 喂食器：" + battle.NodeMetrics.BedCareUses + " / " + battle.NodeMetrics.LitterBoxUses + " / " + battle.NodeMetrics.FeederUses);
            GUILayout.Label("虚弱次数：" + battle.NodeMetrics.WeakIncidents);
        }

        private void DrawCatControls()
        {
            if (cats.Count == 0)
            {
                return;
            }

            GUILayout.Space(8f);
            GUILayout.Label("猫咪切换", HudSectionLabel);
            IReadOnlyList<P0CatHudCard> cards = BuildCatHudCardsForSmoke();
            if (!showDiagnosticsHud)
            {
                DrawPlayerCatControls(cards);
                return;
            }

            GUILayout.Label(P0CatHudPresenter.BuildCompactSummary(cards), WrappedHudLabel);
            for (int i = 0; i < cards.Count; i++)
            {
                P0CatHudCard card = cards[i];
                if (IsHudNarrow)
                {
                    GUILayout.BeginHorizontal();
                    GUI.enabled = true;
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.PrimaryHudIcon, P0ImGuiLayout.Scaled(44f));
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HpIcon, P0ImGuiLayout.Scaled(24f));
                    GUILayout.EndHorizontal();

                    GUI.enabled = card.CanSwitch;
                    if (GUILayout.Button(card.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(92f))))
                    {
                        SelectCat(i);
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUI.enabled = true;
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.PrimaryHudIcon, P0ImGuiLayout.Scaled(54f));
                    GUI.enabled = card.CanSwitch;
                    if (GUILayout.Button(card.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(72f))))
                    {
                        SelectCat(i);
                    }

                    GUI.enabled = true;
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HpIcon, P0ImGuiLayout.Scaled(28f));
                    GUILayout.EndHorizontal();
                }
                DrawCatHudBar(card);
            }

            GUI.enabled = true;
        }

        private void DrawPlayerCatControls(IReadOnlyList<P0CatHudCard> cards)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < cards.Count; i++)
            {
                P0CatHudCard card = cards[i];
                GUILayout.BeginVertical(GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true));
                GUILayout.BeginHorizontal();
                GUI.enabled = true;
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.PrimaryHudIcon, P0ImGuiLayout.Scaled(32f));
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HpIcon, P0ImGuiLayout.Scaled(20f));
                GUILayout.EndHorizontal();

                GUI.enabled = card.CanSwitch || card.IsActive;
                if (GUILayout.Button(
                    BuildPlayerCatButtonLabel(card, i),
                    WrappedHudButton,
                    GUILayout.MinWidth(0f),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(P0ImGuiLayout.Scaled(58f))))
                {
                    SelectCat(i);
                }

                GUI.enabled = true;
                DrawCatHudBar(card);
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }

        private static string BuildPlayerCatButtonLabel(P0CatHudCard card, int index)
        {
            return (index + 1)
                + " "
                + card.SlotState
                + " "
                + card.DisplayName
                + " "
                + card.RoleToken
                + "\n"
                + card.CurrentHp.ToString("0")
                + "/"
                + card.MaxHp.ToString("0")
                + " "
                + card.HpStateToken;
        }

        private void DrawCoreValueIcons()
        {
            if (battle == null)
            {
                return;
            }

            DrawCoreValueGauge(P0CoreValuePresenter.DescribeOwnerSleep(battle.OwnerSleep));
            if (ActiveCat != null)
            {
                DrawCatHpGauge(P0CatHudPresenter.BuildCard(ActiveCat, true, GetSkillCooldown));
            }

            DrawCoreValueGauge(P0CoreValuePresenter.DescribeTeamPoop(battle.TeamPoop));
            DrawCoreValueGauge(P0CoreValuePresenter.DescribeTeamHunger(battle.TeamHunger));
        }

        private void DrawCoreValueGauge(CoreValuePresentation presentation)
        {
            GUILayout.BeginHorizontal();
            P0ImGuiVisualAssetDrawer.DrawInlineIcon(presentation.VisualAsset, P0ImGuiLayout.Scaled(26f));
            P0ImGuiVisualAssetDrawer.DrawGaugeBar(
                presentation.GaugeFrameAsset,
                presentation.GaugeFillAsset,
                presentation.Label + " " + presentation.ValueText + " " + presentation.StateLabel,
                presentation.FillRatio,
                22f);
            GUILayout.EndHorizontal();
        }

        private void DrawCatHpGauge(P0CatHudCard card)
        {
            GUILayout.BeginHorizontal();
            P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HpIcon, P0ImGuiLayout.Scaled(26f));
            P0ImGuiVisualAssetDrawer.DrawGaugeBar(
                card.HpGaugeFrameAsset,
                card.HpGaugeFillAsset,
                "猫生命 " + card.HpLabel + " " + card.HpStateToken,
                card.HpRatio,
                22f);
            GUILayout.EndHorizontal();
        }

        private void DrawCatHudBar(P0CatHudCard card)
        {
            bool previousEnabled = GUI.enabled;
            GUI.enabled = true;
            if (P0ImGuiVisualAssetDrawer.DrawGaugeBar(
                card.HpGaugeFrameAsset,
                card.HpGaugeFillAsset,
                card.HpLabel + " " + card.HpStateToken,
                card.HpRatio,
                16f))
            {
                GUI.enabled = previousEnabled;
                return;
            }

            Rect rect = GUILayoutUtility.GetRect(1f, 10f, GUILayout.ExpandWidth(true));
            Rect backgroundRect = new Rect(rect.x + 6f, rect.y + 2f, Mathf.Max(0f, rect.width - 12f), 6f);
            Rect fillRect = new Rect(backgroundRect.x, backgroundRect.y, backgroundRect.width * card.HpRatio, backgroundRect.height);
            Rect accentRect = new Rect(backgroundRect.x, backgroundRect.y, 4f, backgroundRect.height);
            Color previousColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.32f);
            GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture);
            GUI.color = ToUnityColor(card.HpFillColor);
            GUI.DrawTexture(fillRect, Texture2D.whiteTexture);
            GUI.color = ToUnityColor(card.AccentColor);
            GUI.DrawTexture(accentRect, Texture2D.whiteTexture);
            GUI.color = previousColor;
            GUI.enabled = previousEnabled;
        }

        private void DrawSkillControls()
        {
            if (ActiveCat == null)
            {
                return;
            }

            GUILayout.Space(8f);
            GUILayout.Label("技能", HudSectionLabel);
            IReadOnlyList<P0SkillHudCard> skillCards = BuildSkillHudCardsForSmoke();
            if (!showDiagnosticsHud)
            {
                DrawPlayerSkillControls(skillCards);
                return;
            }

            GUILayout.Label(P0SkillHudPresenter.BuildCompactSummary(skillCards), WrappedHudLabel);
            for (int i = 0; i < ActiveCat.Definition.SkillIds.Count; i++)
            {
                string skillId = ActiveCat.Definition.SkillIds[i];
                if (!skillsById.TryGetValue(skillId, out SkillDefinition skill))
                {
                    GUILayout.Label("缺失技能：" + skillId);
                    continue;
                }

                float cooldown = GetSkillCooldown(skillId);
                P0SkillTargetResult target = ResolveSkillTarget(skill);
                P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                    skill,
                    cooldown,
                    battle == null ? 0f : battle.TeamHunger.Current,
                    target,
                    IsBattleInProgress);
                P0SkillHudCard card = P0SkillHudPresenter.BuildCard(
                    skill,
                    affordance,
                    cooldown,
                    battle == null ? 0f : battle.TeamHunger.Current,
                    target);

                if (IsHudNarrow)
                {
                    GUILayout.BeginHorizontal();
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.StatusVisualAsset, P0ImGuiLayout.Scaled(32f));
                    GUI.enabled = true;
                    if (!P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.TargetReticleAsset, P0ImGuiLayout.Scaled(26f)))
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HungerCostVisualAsset, P0ImGuiLayout.Scaled(26f));
                    }

                    GUI.enabled = i != skillIndicatorSlotIndex;
                    if (GUILayout.Button(
                        i == skillIndicatorSlotIndex ? "已显示" : "追踪",
                        GUILayout.MinWidth(0f),
                        GUILayout.ExpandWidth(true),
                        GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
                    {
                        SelectSkillIndicatorSlot(i);
                    }

                    GUILayout.EndHorizontal();

                    GUI.enabled = affordance.IsEnabled;
                    if (GUILayout.Button(card.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(108f))))
                    {
                        CastSkillBySlot(i);
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.StatusVisualAsset, P0ImGuiLayout.Scaled(38f));
                    GUI.enabled = affordance.IsEnabled;
                    if (GUILayout.Button(card.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(90f))))
                    {
                        CastSkillBySlot(i);
                    }

                    GUI.enabled = true;
                    if (!P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.TargetReticleAsset, P0ImGuiLayout.Scaled(30f)))
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HungerCostVisualAsset, P0ImGuiLayout.Scaled(30f));
                    }

                    GUI.enabled = i != skillIndicatorSlotIndex;
                    if (GUILayout.Button(i == skillIndicatorSlotIndex ? "已显示" : "追踪", GUILayout.Width(P0ImGuiLayout.Scaled(64f)), GUILayout.Height(P0ImGuiLayout.Scaled(68f))))
                    {
                        SelectSkillIndicatorSlot(i);
                    }

                    GUILayout.EndHorizontal();
                }

                GUI.enabled = true;
                DrawSkillHudBar(card);
            }

            P0SkillIndicatorState indicator = BuildSkillIndicatorState();
            if (indicator.HasSkill)
            {
                GUILayout.Label(indicator.BuildSummary());
            }
        }

        private void DrawPlayerSkillControls(IReadOnlyList<P0SkillHudCard> skillCards)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < skillCards.Count; i++)
            {
                P0SkillHudCard card = skillCards[i];
                GUILayout.BeginVertical(GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true));
                GUILayout.BeginHorizontal();
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.StatusVisualAsset, P0ImGuiLayout.Scaled(28f));
                if (!P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.TargetReticleAsset, P0ImGuiLayout.Scaled(22f)))
                {
                    P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HungerCostVisualAsset, P0ImGuiLayout.Scaled(22f));
                }

                GUI.enabled = i != skillIndicatorSlotIndex;
                if (GUILayout.Button(
                    i == skillIndicatorSlotIndex ? "●" : "○",
                    GUILayout.MinWidth(0f),
                    GUILayout.Width(P0ImGuiLayout.Scaled(32f)),
                    GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
                {
                    SelectSkillIndicatorSlot(i);
                }

                GUILayout.EndHorizontal();

                GUI.enabled = card.IsEnabled;
                if (GUILayout.Button(
                    BuildPlayerSkillButtonLabel(card),
                    WrappedHudButton,
                    GUILayout.MinWidth(0f),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(P0ImGuiLayout.Scaled(62f))))
                {
                    CastSkillBySlot(i);
                }

                GUI.enabled = true;
                DrawSkillHudBar(card);
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
            P0SkillIndicatorState indicator = BuildSkillIndicatorState();
            if (indicator.HasSkill)
            {
                GUILayout.Label(indicator.BuildSummary(), WrappedHudLabel);
            }

            GUI.enabled = true;
        }

        private static string BuildPlayerSkillButtonLabel(P0SkillHudCard card)
        {
            return card.SlotToken
                + " "
                + card.DisplayName
                + "\n"
                + card.StatusLabel
                + " | "
                + FormatPlayerTargetLabel(card.TargetLabel);
        }

        private static string FormatPlayerTargetLabel(string targetLabel)
        {
            if (string.IsNullOrWhiteSpace(targetLabel))
            {
                return "无需目标";
            }

            int diagnosticIndex = targetLabel.IndexOf(" | ", StringComparison.Ordinal);
            return diagnosticIndex < 0 ? targetLabel : targetLabel.Substring(0, diagnosticIndex);
        }

        private void DrawSkillHudBar(P0SkillHudCard card)
        {
            bool previousEnabled = GUI.enabled;
            GUI.enabled = true;
            Rect rect = GUILayoutUtility.GetRect(1f, 10f, GUILayout.ExpandWidth(true));
            Rect backgroundRect = new Rect(rect.x + 6f, rect.y + 2f, Mathf.Max(0f, rect.width - 12f), 6f);
            float fillRatio = card.IsCoolingDown ? 1f - card.CooldownRatio : 1f;
            Rect fillRect = new Rect(backgroundRect.x, backgroundRect.y, backgroundRect.width * fillRatio, backgroundRect.height);
            Rect accentRect = new Rect(backgroundRect.x, backgroundRect.y, 4f, backgroundRect.height);
            Color previousColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.32f);
            GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture);
            GUI.color = ToUnityColor(card.CooldownFillColor);
            GUI.DrawTexture(fillRect, Texture2D.whiteTexture);
            GUI.color = ToUnityColor(card.AccentColor);
            GUI.DrawTexture(accentRect, Texture2D.whiteTexture);
            GUI.color = previousColor;
            GUI.enabled = previousEnabled;
        }

        private void DrawInteractionControls()
        {
            GUILayout.Space(8f);
            GUILayout.BeginHorizontal();
            P0ImGuiVisualAssetDrawer.DrawInlineIcon(P0VisualAssetCatalog.GetInteractionRangeRipple(), 24f);
            GUILayout.Label("交互", HudSectionLabel);
            GUILayout.EndHorizontal();
            P0BattleActionAffordance bedCare = BuildBedCareAffordance();
            P0BattleActionAffordance litterBox = BuildLitterBoxAffordance();
            P0BattleActionAffordance feeder = BuildFeederAffordance();

            if (!showDiagnosticsHud)
            {
                DrawPlayerInteractionControls(bedCare, litterBox, feeder);
                return;
            }

            GUI.enabled = bedCare.IsEnabled;
            if (GUILayout.Button(bedCare.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(52f))))
            {
                UseBedCare();
            }

            GUI.enabled = litterBox.IsEnabled;
            if (GUILayout.Button(litterBox.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(52f))))
            {
                UseLitterBox();
            }

            GUI.enabled = feeder.IsEnabled;
            if (GUILayout.Button(feeder.BuildButtonLabel(), WrappedHudButton, GUILayout.MinWidth(0f), GUILayout.ExpandWidth(true), GUILayout.Height(P0ImGuiLayout.Scaled(52f))))
            {
                UseFeeder();
            }

            GUI.enabled = true;

            P0BattleResultSurface resultSurface = BuildBattleResultSurfaceForSmoke();
            if (resultSurface.IsResolved)
            {
                GUILayout.Space(6f);
                if (resultSurface.OutcomeBannerAsset.HasAsset)
                {
                    P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(resultSurface.OutcomeBannerAsset, P0ImGuiLayout.Scaled(260f), P0ImGuiLayout.Scaled(82f));
                }

                GUILayout.Label(resultSurface.Title + " - " + resultSurface.PromptText, WrappedHudLabel);
                DrawBattleResultRoutePreview(resultSurface);
            }

            for (int i = 0; i < resultSurface.Actions.Count; i++)
            {
                P0BattleResultAction action = resultSurface.Actions[i];
                if (action.ActionId == P0BattleResultActionIds.ContinueRoute && !resultSurface.IsResolved)
                {
                    continue;
                }

                GUI.enabled = action.IsEnabled;
                if (GUILayout.Button(action.BuildButtonLabel(), GUILayout.Height(P0ImGuiLayout.ButtonHeight)))
                {
                    ExecuteBattleResultAction(action);
                }
            }

            GUI.enabled = true;
        }

        private void DrawPlayerInteractionControls(
            P0BattleActionAffordance bedCare,
            P0BattleActionAffordance litterBox,
            P0BattleActionAffordance feeder)
        {
            GUILayout.BeginHorizontal();
            DrawPlayerInteractionButton(bedCare, UseBedCare);
            DrawPlayerInteractionButton(litterBox, UseLitterBox);
            DrawPlayerInteractionButton(feeder, UseFeeder);
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            P0BattleResultSurface resultSurface = BuildBattleResultSurfaceForSmoke();
            if (resultSurface.IsResolved)
            {
                GUILayout.Space(6f);
                GUILayout.Label(resultSurface.Title + " - " + resultSurface.PromptText, WrappedHudLabel);
                DrawBattleResultRoutePreview(resultSurface);
            }

            for (int i = 0; i < resultSurface.Actions.Count; i++)
            {
                P0BattleResultAction action = resultSurface.Actions[i];
                if (action.ActionId == P0BattleResultActionIds.ContinueRoute && !resultSurface.IsResolved)
                {
                    continue;
                }

                GUI.enabled = action.IsEnabled;
                if (GUILayout.Button(action.BuildButtonLabel(), GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
                {
                    ExecuteBattleResultAction(action);
                }
            }

            GUI.enabled = true;
        }

        private void DrawPlayerInteractionButton(P0BattleActionAffordance affordance, Action handler)
        {
            GUI.enabled = affordance.IsEnabled;
            if (GUILayout.Button(
                BuildPlayerInteractionButtonLabel(affordance),
                WrappedHudButton,
                GUILayout.MinWidth(0f),
                GUILayout.ExpandWidth(true),
                GUILayout.Height(P0ImGuiLayout.Scaled(46f))))
            {
                handler();
            }
        }

        private static string BuildPlayerInteractionButtonLabel(P0BattleActionAffordance affordance)
        {
            return affordance.Title + "\n" + affordance.Status;
        }

        private void DrawBattleResultRoutePreview(P0BattleResultSurface resultSurface)
        {
            int maxRows = Mathf.Min(2, resultSurface.RouteRows.Count);
            for (int i = 0; i < maxRows; i++)
            {
                GUILayout.Label(resultSurface.RouteRows[i], WrappedHudLabel);
            }
        }

        private void ExecuteBattleResultAction(P0BattleResultAction action)
        {
            switch (action.ActionId)
            {
                case P0BattleResultActionIds.ContinueRoute:
                    ContinueRoute();
                    break;
                case P0BattleResultActionIds.ReturnCatRoom:
                    ReturnToCatRoom();
                    break;
                case P0BattleResultActionIds.RestartRun:
                    RestartRun();
                    break;
            }
        }

        public IReadOnlyList<P0BattleActionAffordance> BuildBattleActionAffordancesForSmoke()
        {
            List<P0BattleActionAffordance> affordances = new List<P0BattleActionAffordance>();
            if (ActiveCat != null)
            {
                for (int i = 0; i < ActiveCat.Definition.SkillIds.Count; i++)
                {
                    string skillId = ActiveCat.Definition.SkillIds[i];
                    if (!skillsById.TryGetValue(skillId, out SkillDefinition skill))
                    {
                        affordances.Add(P0BattleActionAffordancePresenter.BuildSkill(
                            null,
                            0f,
                            battle == null ? 0f : battle.TeamHunger.Current,
                            default(P0SkillTargetResult),
                            IsBattleInProgress));
                        continue;
                    }

                    affordances.Add(BuildSkillAffordance(skill));
                }
            }

            affordances.Add(BuildBedCareAffordance());
            affordances.Add(BuildLitterBoxAffordance());
            affordances.Add(BuildFeederAffordance());
            return affordances.AsReadOnly();
        }

        public P0BattleCommandDeck BuildBattleCommandDeckForSmoke()
        {
            P0BattleActionAffordance[] interactions =
            {
                BuildBedCareAffordance(),
                BuildLitterBoxAffordance(),
                BuildFeederAffordance()
            };
            return P0BattleCommandDeckPresenter.BuildDeck(
                BuildCatHudCardsForSmoke(),
                BuildSkillHudCardsForSmoke(),
                interactions);
        }

        public P0BattlePlayerBrief BuildBattlePlayerBriefForSmoke()
        {
            return P0BattlePlayerBriefPresenter.Build(
                battle,
                cats,
                activeCatIndex,
                BuildNavigationSummary(),
                P0RunSession.CurrentRun,
                lastCompletionReport ?? P0RunSession.LastCompletionReport,
                BuildBattleCommandDeckForSmoke());
        }

        public P0BattleResultSurface BuildBattleResultSurfaceForSmoke()
        {
            RunNodeCompletionReport completionReport = lastCompletionReport ?? P0RunSession.LastCompletionReport;
            return P0BattleResultPresenter.Build(battle, P0RunSession.CurrentRun, completionReport);
        }

        public P0RuntimeSettingsPresentation BuildRuntimeSettingsPresentationForSmoke()
        {
            return P0RuntimeSettingsPresenter.Build(runtimeSettings);
        }

        public IReadOnlyList<P0CatHudCard> BuildCatHudCardsForSmoke()
        {
            return P0CatHudPresenter.BuildCards(cats, activeCatIndex, GetSkillCooldown);
        }

        public IReadOnlyList<P0SkillHudCard> BuildSkillHudCardsForSmoke()
        {
            List<P0SkillHudCard> cards = new List<P0SkillHudCard>();
            if (ActiveCat == null)
            {
                return cards.AsReadOnly();
            }

            for (int i = 0; i < ActiveCat.Definition.SkillIds.Count; i++)
            {
                string skillId = ActiveCat.Definition.SkillIds[i];
                float currentHunger = battle == null ? 0f : battle.TeamHunger.Current;
                if (!skillsById.TryGetValue(skillId, out SkillDefinition skill))
                {
                    P0BattleActionAffordance missing = P0BattleActionAffordancePresenter.BuildSkill(
                        null,
                        0f,
                        currentHunger,
                        default(P0SkillTargetResult),
                        IsBattleInProgress);
                    cards.Add(P0SkillHudPresenter.BuildCard(
                        null,
                        missing,
                        0f,
                        currentHunger,
                        default(P0SkillTargetResult)));
                    continue;
                }

                float cooldown = GetSkillCooldown(skill.Id);
                P0SkillTargetResult target = ResolveSkillTarget(skill);
                P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                    skill,
                    cooldown,
                    currentHunger,
                    target,
                    IsBattleInProgress);
                cards.Add(P0SkillHudPresenter.BuildCard(
                    skill,
                    affordance,
                    cooldown,
                    currentHunger,
                    target));
            }

            return cards.AsReadOnly();
        }

        private bool IsBattleInProgress => battle != null && battle.Outcome == BattleOutcome.InProgress;

        private void ExecuteRuntimeSettingsAction(P0RuntimeSettingsAction action)
        {
            if (action.ActionId == P0RuntimeSettingsActionIds.RequestRestart)
            {
                RequestRestartRun();
                return;
            }

            if (action.ActionId == P0RuntimeSettingsActionIds.ConfirmRestart)
            {
                ConfirmRestartRun();
                return;
            }

            switch (action.Command)
            {
                case P0InputCommand.TogglePause:
                    TogglePause();
                    restartConfirmationOpen = false;
                    break;
                case P0InputCommand.SpeedHalf:
                case P0InputCommand.SpeedNormal:
                case P0InputCommand.SpeedFast:
                    SetBattleSpeed(action.TargetSpeedMultiplier);
                    break;
            }
        }

        private P0BattleActionAffordance BuildSkillAffordance(SkillDefinition skill)
        {
            return P0BattleActionAffordancePresenter.BuildSkill(
                skill,
                skill == null ? 0f : GetSkillCooldown(skill.Id),
                battle == null ? 0f : battle.TeamHunger.Current,
                skill == null ? default(P0SkillTargetResult) : ResolveSkillTarget(skill),
                IsBattleInProgress);
        }

        private P0BattleActionAffordance BuildBedCareAffordance()
        {
            return P0BattleActionAffordancePresenter.BuildBedCare(
                IsBattleInProgress,
                CanUseInteractable(bedAnchor, P0BattleNavigationState.DefaultBedCareRange),
                battle == null ? null : battle.OwnerSleep,
                battle == null ? null : battle.TeamHunger);
        }

        private P0BattleActionAffordance BuildLitterBoxAffordance()
        {
            return P0BattleActionAffordancePresenter.BuildLitterBox(
                IsBattleInProgress,
                CanUseInteractable(litterBoxAnchor, P0BattleNavigationState.DefaultInteractionRange),
                battle == null ? null : battle.TeamPoop);
        }

        private P0BattleActionAffordance BuildFeederAffordance()
        {
            return P0BattleActionAffordancePresenter.BuildFeeder(
                IsBattleInProgress,
                CanUseInteractable(feederAnchor, P0BattleNavigationState.DefaultInteractionRange),
                battle == null ? null : battle.TeamHunger);
        }

        private GUIStyle WrappedHudLabel
        {
            get
            {
                if (wrappedHudLabel == null)
                {
                    wrappedHudLabel = new GUIStyle(GUI.skin.label)
                    {
                        wordWrap = true
                    };
                }

                return wrappedHudLabel;
            }
        }

        private GUIStyle WrappedHudButton
        {
            get
            {
                if (wrappedHudButton == null)
                {
                    wrappedHudButton = new GUIStyle(GUI.skin.button)
                    {
                        wordWrap = true,
                        stretchWidth = true
                    };
                }

                return wrappedHudButton;
            }
        }

        private GUIStyle HudSectionLabel
        {
            get
            {
                if (hudSectionLabel == null)
                {
                    hudSectionLabel = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                return hudSectionLabel;
            }
        }

        private GUIStyle HudPanelStyle
        {
            get
            {
                if (hudPanelStyle == null)
                {
                    hudPanelStyle = new GUIStyle(GUI.skin.box)
                    {
                        wordWrap = true
                    };
                }

                hudPanelStyle.padding = P0ImGuiLayout.Padding();
                return hudPanelStyle;
            }
        }

        private bool IsHudNarrow => P0ImGuiLayout.ShouldStackControls(hudPanelInnerWidth);

        private void BeginResponsiveRow()
        {
            isResponsiveRowStacked = IsHudNarrow;
            if (isResponsiveRowStacked)
            {
                GUILayout.BeginVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
            }
        }

        private void EndResponsiveRow()
        {
            if (isResponsiveRowStacked)
            {
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.EndHorizontal();
            }

            isResponsiveRowStacked = false;
        }

        private float GetSkillCooldown(string skillId)
        {
            return skillCooldownsById.TryGetValue(skillId, out float cooldown) ? cooldown : 0f;
        }

        private P0SkillTargetResult ResolveSkillTarget(SkillDefinition skill)
        {
            if (battle == null || skill == null)
            {
                return default(P0SkillTargetResult);
            }

            return P0SkillTargetResolver.Resolve(
                skill,
                battle.ActiveEnemies,
                navigation.Position,
                GetEnemyNavigationPosition);
        }

        private void SelectDefaultSkillIndicatorSlot()
        {
            skillIndicatorSlotIndex = 0;
            if (ActiveCat == null)
            {
                return;
            }

            IReadOnlyList<string> skillIds = ActiveCat.Definition.SkillIds;
            for (int i = 0; i < skillIds.Count; i++)
            {
                if (!skillsById.TryGetValue(skillIds[i], out SkillDefinition skill))
                {
                    continue;
                }

                if (P0SkillTargetResolver.RequiresEnemyTarget(skill))
                {
                    skillIndicatorSlotIndex = i;
                    return;
                }
            }
        }

        private P0SkillIndicatorState BuildSkillIndicatorState()
        {
            if (battle == null || ActiveCat == null || ActiveCat.Definition.SkillIds.Count == 0)
            {
                return default(P0SkillIndicatorState);
            }

            int slotIndex = Mathf.Clamp(skillIndicatorSlotIndex, 0, ActiveCat.Definition.SkillIds.Count - 1);
            if (slotIndex != skillIndicatorSlotIndex)
            {
                skillIndicatorSlotIndex = slotIndex;
            }

            string skillId = ActiveCat.Definition.SkillIds[slotIndex];
            if (!skillsById.TryGetValue(skillId, out SkillDefinition skill))
            {
                return default(P0SkillIndicatorState);
            }

            return P0SkillIndicatorPresenter.Build(
                skill,
                GetSkillCooldown(skill.Id),
                navigation.Position,
                ResolveSkillTarget(skill),
                GetEnemyNavigationPosition);
        }

        private void UpdateSkillIndicatorView()
        {
            if (skillIndicatorView == null)
            {
                return;
            }

            skillIndicatorView.Sync(BuildSkillIndicatorState());
        }

        private void UpdateRuntimeStatusIndicators()
        {
            bool showDiagnosticWorldLabels = ShouldShowWorldDiagnosticLabels();

            if (activeCatStatusIndicatorView != null)
            {
                P0StatusIndicatorState catState = battle == null || ActiveCat == null
                    ? default(P0StatusIndicatorState)
                    : P0StatusIndicatorPresenter.Build(ActiveCat.Statuses, ActiveCat.Definition.DisplayName);
                activeCatStatusIndicatorView.Sync(showDiagnosticWorldLabels
                    ? catState
                    : default(P0StatusIndicatorState));
            }

            if (bedStatusIndicatorView != null)
            {
                P0StatusIndicatorState bedState = battle == null
                    ? default(P0StatusIndicatorState)
                    : P0StatusIndicatorPresenter.Build(battle.BedStatuses, "床");
                bedStatusIndicatorView.Sync(showDiagnosticWorldLabels
                    ? bedState
                    : default(P0StatusIndicatorState));
            }
        }

        private void DrawSkillIndicatorGizmos(P0SkillIndicatorState indicator)
        {
            if (!indicator.HasSkill)
            {
                return;
            }

            Vector3 origin = ToWorldPosition(indicator.Origin, 0.08f);
            if (indicator.ShowsRange)
            {
                Gizmos.color = GetSkillIndicatorRangeColor(indicator);
                DrawGizmoCircle(origin, indicator.Range);
            }

            if (indicator.ShowsTarget)
            {
                Vector3 target = ToWorldPosition(indicator.TargetPosition, 0.16f);
                Gizmos.color = indicator.CanCast
                    ? new Color(0.22f, 0.85f, 0.72f, 0.34f)
                    : new Color(0.48f, 0.55f, 0.68f, 0.24f);
                Gizmos.DrawLine(origin, target);
                Gizmos.DrawWireSphere(target, 0.2f);
                return;
            }

            if (indicator.RequiresEnemyTarget)
            {
                Gizmos.color = new Color(0.78f, 0.48f, 0.38f, 0.28f);
                Gizmos.DrawLine(origin + new Vector3(-0.11f, 0f, -0.11f), origin + new Vector3(0.11f, 0f, 0.11f));
                Gizmos.DrawLine(origin + new Vector3(-0.11f, 0f, 0.11f), origin + new Vector3(0.11f, 0f, -0.11f));
            }
        }

        private Color GetSkillIndicatorRangeColor(P0SkillIndicatorState indicator)
        {
            if (indicator.CanCast)
            {
                return new Color(0.18f, 0.78f, 0.46f, 0.3f);
            }

            return indicator.IsCoolingDown
                ? new Color(0.42f, 0.54f, 0.72f, 0.24f)
                : new Color(0.7f, 0.48f, 0.32f, 0.26f);
        }

        private static Vector3 ToWorldPosition(Vector2 navigationPosition, float y)
        {
            return new Vector3(navigationPosition.x, y, navigationPosition.y);
        }

        private static void DrawGizmoCircle(Vector3 center, float radius)
        {
            if (radius <= 0f)
            {
                return;
            }

            Vector3 previous = center + new Vector3(radius, 0f, 0f);
            for (int i = 1; i <= SkillIndicatorRingSegments; i++)
            {
                float angle = i * Mathf.PI * 2f / SkillIndicatorRingSegments;
                Vector3 next = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(previous, next);
                previous = next;
            }
        }

    }
}
