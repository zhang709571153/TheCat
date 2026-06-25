# Saiban Batch 48 AI Generation Pilot Process Note

Process: built-in image_gen pilot, workspace copy, local chroma-key alpha removal, deterministic review-pack generation.

Built-in generation prompt used Batch 47 strict Saiban spec and the displayed colored three-view turnaround as source identity reference.

Chroma-key command:

```powershell
C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py --input design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_chromakey_source_v001.png --out design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_1024_candidate_v001.png --auto-key border --soft-matte --transparent-threshold 12 --opaque-threshold 220 --despill
```

Helper result: key color `#04f806`, transparent pixels `1156038/1572516`, partially transparent pixels `6875/1572516`.

Rows: 7

No Unity import was performed.
