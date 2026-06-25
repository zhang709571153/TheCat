# Batch 84 Agent Review Prompt

Review the Batch 84 result/settlement preflight candidate packet.

Primary checks:

- Match `Qr1` UI/style tone: dark dreamglass panels, cyan/gold accent language, red/purple failure language.
- Confirm sprites remain textless and candidate-only.
- Confirm screen mockups show usable composition for victory, defeat, run cleared, and run failed states.
- Check 1024x768 density and reward-row/stat-chip readability risk.
- Check no character-body generation or source-lock violation.
- Check no `.meta` files and no path under `Assets`.

Return PASS / PASS_WITH_P2 / FAIL_P1 / FAIL_P0 and concrete file-specific findings.
