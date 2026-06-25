# Batch 85 Agent Review Prompt

Review the Batch 85 settings/pause preflight candidate packet.

Primary checks:

- Match `Qr1` UI/style tone: dark dreamglass panels, cyan/gold accents, restrained lavender controls.
- Confirm sprites remain textless and candidate-only.
- Confirm screen mockups show usable composition for settings main, audio settings, pause overlay, and compact settings.
- Check 1024x768 density, tab bar readability, option rows, slider/switch/checkbox affordance, and close/back icon clarity.
- Check no character-body generation or source-lock violation.
- Check no `.meta` files and no path under `Assets`.

Return PASS / PASS_WITH_P2 / FAIL_P1 / FAIL_P0 and concrete file-specific findings.
