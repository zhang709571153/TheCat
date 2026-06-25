from __future__ import annotations

import argparse
import csv
import shutil
from pathlib import Path

from PIL import Image


RECOMMENDED = [
    ("saiban", "shield_barrier", "v003_lightframe", "Switch to v003 for cleaner 32px shield read."),
    ("saiban", "moon_sword_sweep", "v001", "Keep current for sharper sword sweep."),
    ("saiban", "battle_flag_rally", "v003_lightframe", "Switch to v003; avoids crescent/crowd and reads cleaner than v002."),
    ("saiban", "sun_charge_burst", "v001", "Keep current for stronger sun burst."),
    ("saiban", "shield_counter_impact", "v001", "Keep current for stronger impact readability."),
    ("saiban", "crown_judgment_sigil", "v001", "Keep current for clearer crown judgment identity."),
    ("nephthys", "obelisk_turret", "v001", "Keep current for tall obelisk silhouette."),
    ("nephthys", "quicksand_trap_spiral", "v003_lightframe", "Switch to v003 for cleaner 32px swirl."),
    ("nephthys", "blocking_pyramid_cube", "v001", "Keep current for explicit pyramid cube."),
    ("nephthys", "obelisk_missile", "v001", "Keep current for stronger diagonal missile."),
    ("nephthys", "sandstorm_swirl", "v001", "Keep current for stronger sandstorm mass."),
    ("nephthys", "sand_tornado_column", "v001", "Keep current for recognizable tornado column."),
    ("suzune", "bell_strike_ring", "v001", "Keep current for simple bell silhouette."),
    ("suzune", "healing_bell_pulse", "v001", "Keep current for faster bell/heal read."),
    ("suzune", "ice_talisman_guard", "v001", "Keep current for stronger talisman guard identity."),
    ("suzune", "frost_moon_dance_swirl", "v001", "Keep current for moon/frost specificity."),
    ("suzune", "torii_gate_ward", "v001", "Keep current for unmistakable torii silhouette."),
    ("suzune", "team_heal_ice_enchant", "v003_lightframe", "Switch to v003; avoids humanoid/medical drift."),
]


def source_path(batch: Path, cat: str, skill: str, size: int, version: str) -> Path:
    if version == "v003_lightframe":
        return batch / f"icons/icons_lightframe_{size}/thecat_ui_skill_{cat}_{skill}_{size}_candidate_v003_lightframe.png"
    return batch / f"icons/icons_{size}/thecat_ui_skill_{cat}_{skill}_{size}_candidate_{version}.png"


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch-dir", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    args = parser.parse_args()

    for size in (256, 128, 64, 32):
        (args.out_dir / f"recommended_{size}").mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    contact = Image.new("RGBA", (6 * 256, 3 * 256), (0, 0, 0, 0))
    for index, (cat, skill, version, reason) in enumerate(RECOMMENDED):
        for size in (256, 128, 64, 32):
            src = source_path(args.batch_dir, cat, skill, size, version)
            dst = args.out_dir / f"recommended_{size}" / f"thecat_ui_skill_{cat}_{skill}_{size}_recommended_candidate_v001.png"
            if not src.exists():
                raise FileNotFoundError(src)
            shutil.copy2(src, dst)
            rows.append(
                {
                    "asset_id": f"thecat_ui_skill_{cat}_{skill}_{size}_recommended_candidate_v001",
                    "cat": cat,
                    "skill": skill,
                    "size": f"{size}x{size}",
                    "selected_source_version": version,
                    "path": dst.as_posix(),
                    "status": "recommended_candidate_pending_unity_import_decision",
                    "reason": reason,
                }
            )
        contact.alpha_composite(Image.open(source_path(args.batch_dir, cat, skill, 256, version)).convert("RGBA"), ((index % 6) * 256, (index // 6) * 256))

    contact.save(args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_recommended_contact_sheet_v001.png")
    with (args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=[
                "asset_id",
                "cat",
                "skill",
                "size",
                "selected_source_version",
                "path",
                "status",
                "reason",
            ],
        )
        writer.writeheader()
        writer.writerows(rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
