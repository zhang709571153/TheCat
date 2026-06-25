from __future__ import annotations

import argparse
import csv
from pathlib import Path

import numpy as np
from PIL import Image


ICONS = [
    ("saiban", "shield_barrier"),
    ("saiban", "moon_sword_sweep"),
    ("saiban", "battle_flag_rally"),
    ("saiban", "sun_charge_burst"),
    ("saiban", "shield_counter_impact"),
    ("saiban", "crown_judgment_sigil"),
    ("nephthys", "obelisk_turret"),
    ("nephthys", "quicksand_trap_spiral"),
    ("nephthys", "blocking_pyramid_cube"),
    ("nephthys", "obelisk_missile"),
    ("nephthys", "sandstorm_swirl"),
    ("nephthys", "sand_tornado_column"),
    ("suzune", "bell_strike_ring"),
    ("suzune", "healing_bell_pulse"),
    ("suzune", "ice_talisman_guard"),
    ("suzune", "frost_moon_dance_swirl"),
    ("suzune", "torii_gate_ward"),
    ("suzune", "team_heal_ice_enchant"),
]


def alpha_bbox(image: Image.Image, threshold: int = 16) -> tuple[int, int, int, int]:
    alpha = np.array(image.getchannel("A"))
    ys, xs = np.where(alpha > threshold)
    if xs.size == 0 or ys.size == 0:
        return (0, 0, image.width, image.height)
    return (int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1)


def fit_icon(icon: Image.Image, size: int, max_fill: int) -> Image.Image:
    bbox = alpha_bbox(icon)
    cropped = icon.crop(bbox)
    scale = min(max_fill / cropped.width, max_fill / cropped.height, 1.0)
    new_size = (max(1, round(cropped.width * scale)), max(1, round(cropped.height * scale)))
    resized = cropped.resize(new_size, Image.Resampling.LANCZOS)
    out = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    out.alpha_composite(resized, ((size - resized.width) // 2, (size - resized.height) // 2))
    return out


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--input", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    parser.add_argument("--manifest", required=True, type=Path)
    args = parser.parse_args()

    source = Image.open(args.input).convert("RGBA")
    args.out_dir.mkdir(parents=True, exist_ok=True)
    for size in (256, 128, 64, 32):
        (args.out_dir / f"icons_{size}").mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    cell_w = source.width / 6
    cell_h = source.height / 3

    for index, (cat, skill) in enumerate(ICONS):
        row = index // 6
        col = index % 6
        box = (
            round(col * cell_w),
            round(row * cell_h),
            round((col + 1) * cell_w),
            round((row + 1) * cell_h),
        )
        cell = source.crop(box)
        icon_256 = fit_icon(cell, 256, 232)
        asset_id_base = f"thecat_ui_skill_{cat}_{skill}"
        for size, max_fill in ((256, 232), (128, 116), (64, 58), (32, 29)):
            output = fit_icon(icon_256, size, max_fill)
            filename = f"{asset_id_base}_{size}_candidate_v001.png"
            output_path = args.out_dir / f"icons_{size}" / filename
            output.save(output_path)
            rows.append(
                {
                    "asset_id": f"{asset_id_base}_{size}_candidate_v001",
                    "cat": cat,
                    "skill": skill,
                    "size": f"{size}x{size}",
                    "path": output_path.as_posix(),
                    "status": "candidate_pending_visual_review",
                    "source": args.input.as_posix(),
                }
            )

    contact = Image.new("RGBA", (6 * 256, 3 * 256), (0, 0, 0, 0))
    for index, (cat, skill) in enumerate(ICONS):
        path = args.out_dir / "icons_256" / f"thecat_ui_skill_{cat}_{skill}_256_candidate_v001.png"
        contact.alpha_composite(Image.open(path).convert("RGBA"), ((index % 6) * 256, (index // 6) * 256))
    contact.save(args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_contact_sheet_v001.png")

    args.manifest.parent.mkdir(parents=True, exist_ok=True)
    with args.manifest.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["asset_id", "cat", "skill", "size", "path", "status", "source"],
        )
        writer.writeheader()
        writer.writerows(rows)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
