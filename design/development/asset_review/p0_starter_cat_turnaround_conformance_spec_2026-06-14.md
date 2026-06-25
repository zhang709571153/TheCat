# P0 Starter Cat Turnaround Conformance Spec

Date: 2026-06-14

Purpose: make the user's corrected rule actionable: Saiban, Nephthys, and
Suzune assets must strictly match the colored three-view turnarounds from the
design source before any cat candidate may be approved for Unity import.

This is not an image-generation batch. It is a gate for future cat asset
production. Cat candidates may stay under `design/development/asset_candidates`,
but formal Unity import remains blocked until the active-cat Play Mode
screenshots pass side-by-side review against these anchors.

## Source Authority

| cat | source lock | colored turnaround |
| --- | --- | --- |
| Saiban | `saiban_turnaround_colored` | `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png` |
| Nephthys | `nephthys_turnaround_colored` | `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png` |
| Suzune | `suzune_turnaround_colored` | `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png` |

## Required Gate Counts

- Spec cats: 3
- Existing source files: 3
- Front-view anchors: 9
- Side-view anchors: 9
- Back-view anchors: 9
- Palette anchors: 9
- Prop/costume anchors: 9
- Prohibited drift rules: 12

These counts are mirrored by
`P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec()`.

## Saiban Anchors

Front view:

- Front view silver-gray tabby face stripes and pale green eyes.
- Front view red torn cape collar over silver-gold armor.
- Front view round sun shield on the left side and single sword on the right
  side.

Side view:

- Side view compact non-human cat muzzle and upright ears.
- Side view red cape trails behind armor with striped tail visible.
- Side view shield disk and angled sword silhouette remain readable.

Back view:

- Back view gray tabby head stripes and rounded cat head.
- Back view torn red cape covers armor with dark holes along the lower edge.
- Back view striped tail sits below the cape with sword silhouette at the side.

Palette:

- Silver-gray fur with darker tabby stripes.
- Deep red cape cloth.
- Silver armor with gold trim and blue gem accents.

Props and costume:

- Round sun oath shield.
- Single straight sword.
- Silver-gold armor, helm read, belt, and torn cape.

Reject:

- Generated-lineup or generic knight-cat drift over the colored three-view
  turnaround.
- Human knight torso, long human legs, or biped costume posture.
- Palette drift away from silver-gray fur, red cape, silver armor, gold trim,
  and blue gems.
- Missing front, side, or back anchors including shield, sword, cape, tabby
  face, and striped tail.

## Nephthys Anchors

Front view:

- Front view gold-brown tabby face, large golden eyes, and dark blue hood.
- Front view crescent hood ornament with blue tear gem and gold script border.
- Front view floating pyramid over inverted obelisk prop beside raised paw.

Side view:

- Side view hood volume wraps the cat head while ears stay visible.
- Side view blue cloak layers and gold script trim sweep behind the compact
  body.
- Side view floating pyramid/obelisk prop remains in front of the paw.

Back view:

- Back view dark blue hood and cloak with centered vertical gold script strip.
- Back view winged blue gem and ankh emblem on the shoulder mantle.
- Back view split cloak exposes gold-brown striped tail.

Palette:

- Gold-brown tabby fur.
- Deep navy cloak and hood.
- Sand-gold trim with blue gems and cyan magic particles.

Props and costume:

- Floating pyramid over inverted obelisk controller prop.
- Crescent moon hood ornament and blue teardrop gem.
- Gold script trim, ankh symbol, winged chest and back jewel.

Reject:

- Generated-lineup or generic Egyptian fantasy drift over the colored
  three-view turnaround.
- Cleopatra costume cliche, human body language, or human robe posture.
- Palette drift away from gold-brown fur, deep navy cloth, sand-gold trim, and
  blue gems.
- Missing front, side, or back anchors including hood, script trim, pyramid
  prop, ankh, and striped tail.

## Suzune Anchors

Front view:

- Front view calico orange, black, and white face patches with blue eyes.
- Front view white shrine robe, vermilion skirt, sash, and central gold bell.
- Front view bell wand/branch cluster with blue paper talismans.

Side view:

- Side view calico head patches continue across ear and cheek.
- Side view white sleeve with blue snowflake motif and red stitch trim.
- Side view red ribbons, hanging bells, and bell wand remain readable.

Back view:

- Back view orange and black calico head patches across both ears.
- Back view large vermilion bow with gold bell over white robe.
- Back view white sleeves show blue snowflake marks and calico tail.

Palette:

- Warm white fur and robe fabric.
- Vermilion red skirt, sash, bow, and ribbons.
- Gold bells with moon-blue talismans and sleep effects.

Props and costume:

- Clustered kagura bell wand/branch.
- Red-white flower hair ornament with hanging bells.
- Paper talismans, blue snowflake charms, central bell, and back bow.

Reject:

- Generated-lineup or generic shrine-cat drift over the colored three-view
  turnaround.
- Human shrine maiden proportions, human sleeves-as-arms pose, or human costume
  posture.
- Palette drift away from calico patches, white robe, vermilion cloth, gold
  bells, and blue talismans.
- Missing front, side, or back anchors including calico markings, bells, wand,
  snowflake sleeves, and back bow.

## Required Future Candidate Note

Every Saiban, Nephthys, or Suzune candidate review note must include:

- Source turnaround path and SHA-256 from the source-lock packet.
- Candidate derivative type.
- Side-by-side reference to the starter cat turnaround contact sheet.
- Trait coverage for front-view, side-view, back-view, palette, and
  prop/costume anchors.
- Explicit reject/hold/approve decision.
- Confirmation that formal Unity import remains blocked unless all three
  active-cat Play Mode screenshots are approved.
