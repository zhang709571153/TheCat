# Batch 83 Loading/Start Preflight Candidate Review

Scope: `ui_loading_start` screen-level preflight.

Result: local candidate packet only. This does not prove Unity import, runtime layout, click targets, or final visual acceptance.

Generated rows:
- Total manifest rows: 8
- Textless sprite candidates: progress frame, progress fill, spinner crescent, dot sequence.
- Local mockups: 1920x1080, 1365x768, 1280x720, 1024x768.

Source truth:
- Qr1 UI/style source truth, live revision 816.
- Existing local UI shell assets: main-menu dream-entry background, title logo, dreamglass panel, primary button, sleep gauge, sleep icon.

Known limits:
- Existing title logo contains English text by design; Batch 83 adds no baked Chinese text.
- Mockups are local review evidence, not import-ready screen captures.
- Strict image2 generation is not claimed; `OPENAI_API_KEY` was missing in this shell.
- Unity screenshot, import settings, binding, and Console checks remain required.
