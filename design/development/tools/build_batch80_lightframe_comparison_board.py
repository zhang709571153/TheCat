from __future__ import annotations

import argparse
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


def font(size: int) -> ImageFont.ImageFont:
    try:
        return ImageFont.truetype("arial.ttf", size)
    except OSError:
        return ImageFont.load_default()


def current_path(batch: Path, cat: str, skill: str, size: int, version: str) -> Path:
    return batch / f"icons/icons_{size}/thecat_ui_skill_{cat}_{skill}_{size}_candidate_{version}.png"


def light_path(batch: Path, cat: str, skill: str, size: int) -> Path:
    return batch / f"icons/icons_lightframe_{size}/thecat_ui_skill_{cat}_{skill}_{size}_candidate_v003_lightframe.png"


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch-dir", required=True, type=Path)
    parser.add_argument("--out", required=True, type=Path)
    args = parser.parse_args()

    width = 6 * 160 + 64
    height = 3 * 228 + 88
    board = Image.new("RGBA", (width, height), (15, 21, 34, 255))
    draw = ImageDraw.Draw(board)
    draw.rectangle((0, 0, width, 54), fill=(30, 44, 71, 255))
    draw.text((24, 18), "Batch 80 comparison: accepted heavy/current vs v003 lightframe", fill=(230, 240, 255, 255), font=font(16))

    small = font(11)
    for index, (cat, skill, version) in enumerate(ACCEPTED):
        row = index // 6
        col = index % 6
        x = 32 + col * 160
        y = 78 + row * 228
        panel = (28, 42, 65, 255) if row != 1 else (222, 234, 239, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 128, y + 184), radius=8, fill=panel, outline=(82, 116, 146, 255), width=1)

        current64 = Image.open(current_path(args.batch_dir, cat, skill, 64, version)).convert("RGBA")
        current32 = Image.open(current_path(args.batch_dir, cat, skill, 32, version)).convert("RGBA")
        light64 = Image.open(light_path(args.batch_dir, cat, skill, 64)).convert("RGBA")
        light32 = Image.open(light_path(args.batch_dir, cat, skill, 32)).convert("RGBA")

        board.alpha_composite(current64, (x + 6, y + 6))
        board.alpha_composite(current32, (x + 86, y + 22))
        draw.text((x + 6, y + 72), "cur", fill=label, font=small)

        board.alpha_composite(light64, (x + 6, y + 98))
        board.alpha_composite(light32, (x + 86, y + 114))
        draw.text((x + 6, y + 164), "light", fill=label, font=small)

    args.out.parent.mkdir(parents=True, exist_ok=True)
    board.save(args.out)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
