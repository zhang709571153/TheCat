from __future__ import annotations

import argparse
import csv
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


ACCEPTED = [
    ("saiban", "shield_barrier", "v001"),
    ("saiban", "moon_sword_sweep", "v001"),
    ("saiban", "battle_flag_rally", "v002"),
    ("saiban", "sun_charge_burst", "v001"),
    ("saiban", "shield_counter_impact", "v001"),
    ("saiban", "crown_judgment_sigil", "v001"),
    ("nephthys", "obelisk_turret", "v001"),
    ("nephthys", "quicksand_trap_spiral", "v001"),
    ("nephthys", "blocking_pyramid_cube", "v001"),
    ("nephthys", "obelisk_missile", "v001"),
    ("nephthys", "sandstorm_swirl", "v001"),
    ("nephthys", "sand_tornado_column", "v001"),
    ("suzune", "bell_strike_ring", "v001"),
    ("suzune", "healing_bell_pulse", "v001"),
    ("suzune", "ice_talisman_guard", "v001"),
    ("suzune", "frost_moon_dance_swirl", "v001"),
    ("suzune", "torii_gate_ward", "v001"),
    ("suzune", "team_heal_ice_enchant", "v002"),
]


def icon_path(batch_dir: Path, cat: str, skill: str, size: int, version: str) -> Path:
    return (
        batch_dir
        / f"icons/icons_{size}/thecat_ui_skill_{cat}_{skill}_{size}_candidate_{version}.png"
    )


def load_font(size: int) -> ImageFont.ImageFont:
    try:
        return ImageFont.truetype("arial.ttf", size)
    except OSError:
        return ImageFont.load_default()


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch-dir", required=True, type=Path)
    parser.add_argument("--out", required=True, type=Path)
    parser.add_argument("--report", required=True, type=Path)
    args = parser.parse_args()

    width = 6 * 144 + 72
    height = 3 * 176 + 120
    out = Image.new("RGBA", (width, height), (18, 24, 38, 255))
    draw = ImageDraw.Draw(out)
    font = load_font(16)
    small_font = load_font(12)

    draw.rectangle((0, 0, width, 56), fill=(28, 42, 68, 255))
    draw.text((24, 18), "Batch 80 HUD readability board: accepted v001 + v002 replacements", fill=(225, 238, 255, 255), font=font)

    rows: list[dict[str, str]] = []
    for index, (cat, skill, version) in enumerate(ACCEPTED):
        row = index // 6
        col = index % 6
        x = 36 + col * 144
        y = 80 + row * 176
        panel_color = (30, 45, 72, 255) if row != 1 else (222, 234, 239, 255)
        label_color = (225, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 112, y + 140), radius=8, fill=panel_color, outline=(84, 116, 145, 255), width=1)
        icon64 = Image.open(icon_path(args.batch_dir, cat, skill, 64, version)).convert("RGBA")
        icon32 = Image.open(icon_path(args.batch_dir, cat, skill, 32, version)).convert("RGBA")
        out.alpha_composite(icon64, (x + 8, y + 8))
        out.alpha_composite(icon32, (x + 84, y + 24))
        draw.rectangle((x + 8, y + 86, x + 104, y + 104), fill=(9, 13, 22, 180))
        draw.rectangle((x + 8, y + 110, x + 104, y + 128), fill=(245, 248, 248, 220))
        out.alpha_composite(icon32, (x + 12, y + 79))
        out.alpha_composite(icon32, (x + 12, y + 103))
        draw.text((x + 8, y + 132), f"{cat}/{version}", fill=label_color, font=small_font)
        rows.append(
            {
                "cat": cat,
                "skill": skill,
                "version": version,
                "size_checks": "64,32,dark_strip,light_strip",
                "state": "readability_board_pending_review",
            }
        )

    args.out.parent.mkdir(parents=True, exist_ok=True)
    out.save(args.out)
    args.report.parent.mkdir(parents=True, exist_ok=True)
    with args.report.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=["cat", "skill", "version", "size_checks", "state"])
        writer.writeheader()
        writer.writerows(rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
