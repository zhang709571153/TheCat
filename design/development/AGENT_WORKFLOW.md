# Agent Workflow And Phase Gates

Date: 2026-06-13

This project uses the main session as architect, integrator, and final quality
gate. Sub-agents are not a rigid ceremony; they are tools for reaching the final
goal efficiently. The main session owns scope, delegation, merge decisions,
test standards, design deviations, and local development logs.

The final goal is the controlling scope:

- Build `猫眠所 / 梦境支配者` into a playable, testable, extensible P0 Unity
  version.
- Preserve enough architecture quality to support P1 expansion.
- Keep momentum toward playable code, not paperwork for its own sake.

Current operating rule:

- The main session may grant wider or narrower authority to agents according to
  the current milestone risk.
- Code development agents should focus on implementing working, maintainable
  code inside a clear write scope. They do not need to perform a broad product
  audit unless the task asks for it.
- Review agents should review the specific risks that matter for the current
  gate. A release gate can request a full review; a small code slice should get
  a focused review.
- Agent prompts are templates, not shackles. The main session can rewrite,
  combine, or skip prompt sections when that improves progress toward the final
  playable P0 goal.

## 1. Delegation Rules

Agents are assigned by practical need and current risk, not by a fixed ritual.
For a large phase, the main session should usually use some combination of
planning, implementation, and review, but it may skip, combine, or narrow roles
when that better serves the final goal.

Common modes:

- Plan agent: used when the next phase is broad, ambiguous, or needs task slicing.
- Code development agent: used for bounded implementation work; primary focus is writing working, maintainable code.
- Content/asset agent: used for asset import planning, manifest work, placeholder production, or style consistency.
- Review agent: used when the main session needs a second pass on a concrete risk. The review scope should match the risk, rather than always covering every possible angle.
- Main session: can implement directly when that is faster or safer, integrates results, resolves conflicts, updates docs/logs, and decides whether to advance.

Review scope examples:

- Code review: architecture, correctness, tests, compile risk.
- Gameplay review: loop clarity, feel, pacing, design consistency.
- Unity integration review: scenes, prefabs, component references, Console errors, MCP validation.
- Asset review: naming, import paths, style consistency, manifest coverage.
- Release gate review: all major acceptance areas before declaring a milestone complete.

Sub-agent prompts must include:

- Task scope.
- Relevant design document paths.
- Code files to read.
- Expected output.
- Forbidden modification scope.
- Acceptance criteria.
- Test command or Unity MCP validation method.

Execution agents must be told:

- They are not alone in the codebase.
- They must not revert unrelated or user-made changes.
- They must keep writes inside their assigned scope.
- They must list all changed files in the final response.

Review agents should report only what is relevant to their assigned scope:

- Blocking issues.
- Non-blocking issues.
- Suggested next steps.
- Residual risk and missing tests.

They should avoid expanding a narrow assignment into every possible angle unless
the current phase gate explicitly requires that breadth.

## 2. A-Stage Agents Used

Plan agent:

- Agent id: `019ebcf3-7323-7513-a2f1-aaf997bea1eb`
- Task: read `design/梦境支配者核心玩法/docs` and summarize P0 gameplay, systems, characters, enemies, UI, assets, risks, and acceptance criteria.
- Result: design extraction completed without file modification.

Execution / reconnaissance agent:

- Agent id: `019ebcf3-b1f8-7270-a095-c38178f22f0c`
- Task: read Unity project structure, package dependencies, scene/build/input state, and missing engineering pieces.
- Result: confirmed this is a Unity 6 URP template project with no gameplay architecture yet.

Review agent:

- Optional for this document-only A-stage. A focused doc/spec review is useful,
  but the main session may proceed to B-stage if the development blueprint is
  coherent and no blocking ambiguity remains.

## 3. Standard Plan Agent Prompt

```text
You are the plan agent for phase <PHASE>.

Task scope:
<Describe exactly what this phase should decide and decompose. Do not modify files.>

Relevant design docs:
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\...

Code files to read:
- <List exact files or folders. Prefer rg --files first.>

Expected output:
- Phase task breakdown.
- Architecture implications.
- Data/scene/UI/test requirements.
- Risks and open decisions.
- Suggested execution-agent slices with disjoint write scopes.

Forbidden scope:
- Do not edit files.
- Do not move assets.
- Do not run destructive commands.
- Do not generate assets.

Acceptance criteria:
- Every recommendation cites local design or code evidence.
- Output separates P0 must-have from P1/post-P0.

Unity MCP validation:
- Do not run Unity writes.
- List any Unity MCP checks needed after implementation.
```

## 4. Standard Execution Agent Prompt

```text
You are an execution agent for phase <PHASE>.
You are not alone in this codebase. Do not revert edits made by others. Keep all writes inside your assigned scope and adapt to existing changes.

Task scope:
<Concrete slice, one system or one small group of related files.>

Relevant design docs:
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\...

Code files to read:
- <Exact files or directories.>

Write scope:
- <Exact files/directories the agent may create or edit.>

Forbidden scope:
- Do not edit <listed files/directories>.
- Do not move design source assets.
- Do not touch Unity-generated Library/Temp/Logs.
- Do not make broad refactors.

Expected output:
- Implemented code/assets within the write scope.
- Changed file list.
- Tests run and results.
- Any design deviations or follow-up tasks.

Acceptance criteria:
- Compiles in Unity.
- Follows existing architecture and naming.
- Includes focused tests for the behavior if practical.
- Updates local development record if this completes a system.

Validation:
- Local command: <test command if available>.
- Unity MCP: <console / scene / command / capture check>.
```

## 5. Standard Review Agent Prompt

```text
You are the review agent for phase <PHASE>.

Task scope:
Review the implementation/specification for the assigned phase from this assigned angle:
<code quality | gameplay feel | Unity integration | asset consistency | release gate | custom scope>.
Do not edit files.

Relevant design docs:
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\...

Code/docs to read:
- <Changed files and relevant surrounding files.>

Review angles:
- Focus first on the assigned angle.
- Mention cross-cutting risks only if they are likely to block the current milestone.
- Do not turn a narrow code review into a full product audit unless explicitly asked.

Expected output:
- Blocking issues with file/line references where possible.
- Non-blocking issues.
- Suggested next steps.
- Residual risks.

Forbidden scope:
- Do not modify files.
- Do not revert changes.
- Do not generate assets.

Acceptance criteria:
- Findings are ordered by severity.
- Each blocking issue explains why it blocks the phase gate.
- The review is specific enough to act on, with file/line references where possible.
```

## 6. Immediate B-Stage Plan Agent Prompt

```text
You are the B-stage plan agent for TheCat.

Task scope:
Read the A-stage development docs and the current Unity project. Decompose B. Engineering framework setup into small implementation slices. Do not modify files.

Relevant design docs:
- D:\Unity Workspace\TheCat\design\development\P0_DEVELOPMENT_BLUEPRINT.md
- D:\Unity Workspace\TheCat\design\development\DEVELOPMENT_LOG.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\00_overview\p0_minimum_design.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\01_core_gameplay\core_gameplay_rules_and_player_path.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\02_combat_and_systems\core_numeric_system_v1.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\02_combat_and_systems\status_tag_system.md

Code files to read:
- D:\Unity Workspace\TheCat\Packages\manifest.json
- D:\Unity Workspace\TheCat\ProjectSettings\EditorBuildSettings.asset
- D:\Unity Workspace\TheCat\Assets\InputSystem_Actions.inputactions
- Existing files under D:\Unity Workspace\TheCat\Assets

Expected output:
- Proposed `Assets/TheCat` folder and asmdef layout.
- Runtime system slices for Core, Data, Input, Gameplay state machine, Metrics, and Debug.
- Test setup slices.
- Which slice should be implemented first and why.
- Write scopes for each execution agent.

Forbidden scope:
- Do not edit files.
- Do not create scenes.
- Do not import art.

Acceptance criteria:
- B-stage can be implemented in bounded PR-like chunks.
- Slices have minimal overlap.
- Recommendations preserve P0.0 graybox-first strategy.

Unity MCP validation:
- Suggest console/build settings/scene checks only; do not run write commands.
```

## 7. B-Stage First Execution Prompt

```text
You are a B-stage execution agent for TheCat.
You are not alone in this codebase. Do not revert edits made by others. Keep writes inside your assigned scope.

Task scope:
Create the initial Unity C# engineering skeleton for TheCat: folder structure, runtime asmdef, test asmdef, core event bus, game state enum/state machine shell, P0 tuning data model, and metrics model. Do not implement gameplay behaviors yet.

Relevant design docs:
- D:\Unity Workspace\TheCat\design\development\P0_DEVELOPMENT_BLUEPRINT.md
- D:\Unity Workspace\TheCat\design\development\AGENT_WORKFLOW.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\02_combat_and_systems\core_numeric_system_v1.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\02_combat_and_systems\numeric_system_review_and_optimization.md
- D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs\02_combat_and_systems\status_tag_system.md

Code files to read:
- D:\Unity Workspace\TheCat\Packages\manifest.json
- D:\Unity Workspace\TheCat\Assets\TutorialInfo\Scripts\Readme.cs
- D:\Unity Workspace\TheCat\Assets\TutorialInfo\Scripts\Editor\ReadmeEditor.cs

Write scope:
- D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Core
- D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Gameplay
- D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Data
- D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode
- asmdef files under D:\Unity Workspace\TheCat\Assets\TheCat

Forbidden scope:
- Do not edit ProjectSettings.
- Do not edit Packages.
- Do not edit existing TutorialInfo files.
- Do not create or edit scenes.
- Do not import art assets.

Expected output:
- Compilable runtime skeleton.
- Focused EditMode tests for pure C# models where feasible.
- Changed file list.
- Any assumptions and follow-up tasks.

Acceptance criteria:
- Unity compiles with zero Console errors.
- No scene or asset dependency is required for the pure skeleton.
- P0 tuning exposes the seven v1.1 knobs.
- Metrics model can record layer time, sleep delta, litter/feeder uses, poop incidents, weak incidents, and node success/failure.

Validation:
- Local: inspect generated files and C# syntax.
- Unity MCP: `Unity_GetConsoleLogs`, read-only compile/state command.
```

## 8. Phase Gate Checklist

Before advancing a phase:

- The phase has a local record in `DEVELOPMENT_LOG.md`.
- Relevant tests or MCP checks ran, or missing validation is explicitly recorded.
- Any review blocking findings, if a review was run, are resolved or consciously deferred with rationale.
- New code has a clear owner folder and does not sprawl across unrelated systems.
- Any design deviation is written down.
- Next execution slice is small enough to review.

The main session may advance without a separate review agent when:

- The change is document-only or low-risk.
- The main session has already inspected the relevant files and validation results.
- Waiting for another agent would slow down a clear next implementation step.

The main session should request a review agent when:

- Code touches shared runtime architecture, scene contracts, input, save/run state, or content schemas.
- A system is being declared complete.
- Unity Console, tests, or scene validation show ambiguous failures.
- Asset batches enter Unity and consistency matters.
- A milestone is close to playable acceptance.
