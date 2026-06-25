# P0 Dream Event Choice Cards

- Batch: `p0_asset_batch_21_dream_event_choice_cards`
- Output directory: `Assets/TheCat/Art/UI/Cards`
- Prompt: `design/development/prompts/p0_dream_event_choice_cards.md`
- Scope: deterministic non-cat UI cards for existing DreamEvent reward choice ids.
- Cat constraint: no cat silhouette, no fur markings, no starter-cat turnaround derivative, no civilization costume motif.
- Runtime bindings: `dream_event_choice.clear_notifications`, `dream_event_choice.catnip_residue`, `dream_event_choice.mark_all_read`.

## Generated Assets

- `thecat_ui_dreamevent_clear_notifications_card_384x160_v001`
  - subject: `dream_event_clear_notifications`
  - motif: red notification rain swept into fish treat reward
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_clear_notifications_card_384x160_v001.png`
  - md5: `94fec8e74fe8771d30a5b02883bbe1c4`
- `thecat_ui_dreamevent_catnip_residue_card_384x160_v001`
  - subject: `dream_event_catnip_residue`
  - motif: residue cloud, skill up arrow, and poop-growth warning
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_catnip_residue_card_384x160_v001.png`
  - md5: `ebf2dc290c598a741e2165b8261e647c`
- `thecat_ui_dreamevent_mark_all_read_card_384x160_v001`
  - subject: `dream_event_mark_all_read`
  - motif: message stack check mark and owner sleep stabilization
  - size: 384x160
  - path: `Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_mark_all_read_card_384x160_v001.png`
  - md5: `4c989208fcf11d4f02d41bdb381a605b`

## Consistency Check

- Uses the accepted dreamglass, dream-event card-frame, route choice icon, and Batch 19 dream-event summary banner visual language.
- Keeps all forms symbolic: notification dots, message cards, residue cloud, fish treat, arrows, check mark, sleep wave, and stars.
- Does not create or modify any starter cat asset; colored turnaround conformance remains untouched.
- `.meta` files carry `TheCatP0ImportSettings:v1`, `batch:p0_asset_batch_21_dream_event_choice_cards`, and `nonCatSymbolicOnly:true`.
