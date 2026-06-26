# P0 Play Mode Acceptance Smoke Report

- Result: passed
- Smoke state: Passed
- Screenshot output: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke
- Evidence summary: P0 Play Mode evidence has no failures, 0 pending warning(s), and 8 passed check(s).

## Acceptance Smoke

```text
Started P0 Play Mode acceptance smoke sequence.
Screenshot smoke passed: P0 play mode screenshot smoke passed with 11 screenshot(s) in D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke.
Route-flow smoke passed: P0 play mode route flow smoke passed: nodes 10/10, battles 5, boss observed, fish 5, shards 9, 行动 切换 297/323 猫咪虚弱未能切换 26 自动锁定目标 106/181 技能锁定目标 95/210 技能 172/927 冷却中 640 没有目标 115 技能暂不可用 0 交互 41/87 距离太远 46, RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, cat-room return verified.
Defeat-flow smoke passed: P0 play mode defeat flow smoke passed: 路线失败 路线 1/10 战斗 0胜/1负 小鱼干 0 梦屑 0 猫 3 祝福 0 等级 0, failed cat-room return verified.
Evidence gate summary: P0 Play Mode evidence has no failures, 0 pending warning(s), and 8 passed check(s).
P0 play mode acceptance smoke passed: 8 evidence check(s), 0 warning(s).
```

## Screenshot Smoke

```text
Cleared 11 existing screenshot PNG(s).
Screenshot output: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke
Captured main menu: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\01-main-menu.png
Main menu start surface verified: 主菜单界面：初始猫 3 已选 3 路线 10 首领层 1 操作 5 可用 5 uiShell 6 | starters 3 selected 3 route 10 actions 5 uiShell 6
Loading/start surface hook verified: Loading start surface target P0CatRoom progress 35% spinner 月牙旋转 rows 3 screenshotHook True
Cat room surface verified: 猫房: values 4 resources 3 dreams 2 hotspots 4 actions 3 feedback 摸摸猫头，准备进入今晚的梦境。 今晚从猫房出发。
Captured cat room: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\02-cat-room.png
Captured route map layer 1: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\03-route-map-layer1.png
Route map surface verified: 路线图界面：层数 10 进度 进度：0/10 选项 0 奖励 0 分支 7 首领层 1 操作 3 uiShell 6 状态 进行中
Battle HUD enemy cards verified: 敌人 HUD：3 首领 1 预警 2 压力源 1 压床 1 远程 1
Battle HUD status indicators verified: 状态 HUD：3 床 1 敌人 1 猫 1 标签 5 护盾 2 图标 3 响应 3
Battle HUD runtime settings verified: 运行设置界面：实时 速度 1 倍 操作 4 可用 3 当前 1 暂停键 P/Esc 速度键 F1/F2/F3
Battle HUD sections verified: 战斗 HUD 分区：目标 4, 核心数值 3, 威胁 4, 队伍 3, 路线 5, 节点指标 8
Battle HUD actions verified: 战斗行动：技能 3，交互 3，可用 3
Battle HUD cat cards verified: 猫 HUD：3 当前 1 虚弱 0 护盾 1 冷却 0
Battle HUD skill cards verified: 技能 HUD：3 可用 2 冷却 0 目标问题 1 低饱肚 0
Captured battle HUD layer 1: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\04-battle-hud-layer1.png
Runtime visual bindings verified before Play Mode screenshots: P0 runtime visual bindings complete for 111 binding(s) and 111 resolved texture(s).
Captured active cat Saiban visual: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\05-active-cat-saiban.png
Captured active cat Nephthys visual: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\06-active-cat-nephthys.png
Captured active cat Suzune visual: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\07-active-cat-suzune.png
Captured battle world visual bindings: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\08-battle-world-visuals.png
Captured Call Tyrant warning VFX: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\09-call-tyrant-warning-vfx.png
Battle result screenshot surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
Captured battle result layer 1: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\10-battle-result-layer1.png
Captured settlement: D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke\11-settlement.png
P0 play mode screenshot smoke passed with 11 screenshot(s) in D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke.
```

## Route Flow Smoke

```text
Started default route from P0MainMenu.
Battle layer_01_defense -> Victory ticks 156 route 1/10.
Battle result surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
DreamEvent catnip residue queued next-battle modifier after reward layer_02_dream_event.
Reward layer_02_dream_event resolved route 2/10.
DreamEvent catnip next-battle modifier verified at layer_03_elite: sources 1, skill x1.2, poop growth 0.45/s.
Battle layer_03_elite -> Victory ticks 140 route 3/10.
Battle result surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
Resolved pending cat upgrade before reward node layer_04_partner: 塞班 获得 被动《床线守誓》.
Reward layer_04_partner resolved route 4/10.
Seeded Shop bed-patch smoke state before layer_05_shop: sleep 60/100, fish 3.
Shop bed-patch state verified after reward layer_05_shop: sleep 80, fish 0.
Shop bed patch queued next-battle sleep verification after reward layer_05_shop.
Reward layer_05_shop resolved route 5/10.
Shop bed-patch next-battle sleep verified at layer_06_defense: sleep 80.
Battle layer_06_defense -> Victory ticks 225 route 6/10.
Battle result surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
Reward layer_07_blessing resolved route 7/10.
Seeded RestNest recovery smoke state before layer_08_rest_nest: saiban hp 44/220 weak 12s.
RestNest recovery state verified after reward layer_08_rest_nest: saiban hp 154/220.
Reward layer_08_rest_nest resolved route 8/10.
RestNest next-battle recovery verified at layer_09_elite: 当前 塞班 防守 生命 154/235 健康 技能 3 技能就绪.
Battle layer_09_elite -> Victory ticks 179 route 9/10.
Battle result surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
Resolved pending cat upgrade before battle node layer_10_boss_call_tyrant: 塞班 获得 小技能《床线拦截》.
Battle layer_10_boss_call_tyrant -> Victory ticks 227 route 10/10.
Battle result surface verified: 战斗结果：胜利 指标 6 核心 3 路线 6 操作 3.
Settlement rows verified: 路线通关 路线 10/10 战斗 5胜/0负 小鱼干 5 梦屑 9 猫 4 祝福 1 等级 1.
Settlement action telemetry verified: 行动 切换 297/323 猫咪虚弱未能切换 26 自动锁定目标 106/181 技能锁定目标 95/210 技能 172/927 冷却中 640 没有目标 115 技能暂不可用 0 交互 41/87 距离太远 46.
P0 play mode route flow settlement ready for screenshot: 路线通关 路线 10/10 战斗 5胜/0负 小鱼干 5 梦屑 9 猫 4 祝福 1 等级 1.
Route settlement cat-room return verified: 本轮梦境完成，奖励回收到猫房。 路线结算已回收到猫房。.
P0 play mode route flow smoke passed: nodes 10/10, battles 5, boss observed, fish 5, shards 9, 行动 切换 297/323 猫咪虚弱未能切换 26 自动锁定目标 106/181 技能锁定目标 95/210 技能 172/927 冷却中 640 没有目标 115 技能暂不可用 0 交互 41/87 距离太远 46, RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, cat-room return verified.
```

## Defeat Flow Smoke

```text
Started default route from P0MainMenu.
Defeat battle result surface verified: 战斗结果：失败 指标 6 核心 3 路线 5 操作 3.
Failed settlement rows verified: 路线失败 路线 1/10 战斗 0胜/1负 小鱼干 0 梦屑 0 猫 3 祝福 0 等级 0.
Failed settlement cat-room return verified: 本轮梦境中断，猫房保留复盘提示。 路线结算已回收到猫房。.
P0 play mode defeat flow smoke passed: 路线失败 路线 1/10 战斗 0胜/1负 小鱼干 0 梦屑 0 猫 3 祝福 0 等级 0, failed cat-room return verified.
```

## Play Mode Evidence

```text
P0 Play Mode evidence has no failures, 0 pending warning(s), and 8 passed check(s).
[Passed] Screenshot Capture Plan: Passed - Expected 11-capture plan is registered.
[Passed] Runtime Visual Screenshot Plan: Passed - Active cat, battle-world, and Call Tyrant warning VFX screenshots are registered.
[Passed] Runtime Visual Contact Sheet: Passed - Runtime visual contact sheet is present for screenshot comparison: design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png.
[Passed] Screenshot File Evidence: Passed - P0 Play Mode screenshot file evidence complete for 11/11 expected validated capture(s).
[Passed] Unity Runtime Validation Plan: Passed - P0 Unity runtime validation plan ready for 18 step(s), 11 screenshot(s), and 20 Chinese UI scale capture row(s).
[Passed] Screenshot Smoke: Passed - P0 play mode screenshot smoke passed with 11 screenshot(s) in D:\Unity Workspace\TheCat\design\development\screenshots\p0-playmode-smoke.
[Passed] Route Flow Smoke: Passed - P0 play mode route flow smoke passed: nodes 10/10, battles 5, boss observed, fish 5, shards 9, 行动 切换 297/323 猫咪虚弱未能切换 26 自动锁定目标 106/181 技能锁定目标 95/210 技能 172/927 冷却中 640 没有目标 115 技能暂不可用 0 交互 41/87 距离太远 46, RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, cat-room return verified.
[Passed] Defeat Flow Smoke: Passed - P0 play mode defeat flow smoke passed: 路线失败 路线 1/10 战斗 0胜/1负 小鱼干 0 梦屑 0 猫 3 祝福 0 等级 0, failed cat-room return verified.
```

