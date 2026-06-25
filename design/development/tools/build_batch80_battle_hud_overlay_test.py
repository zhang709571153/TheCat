from __future__ import annotations

import argparse
import csv
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


def load_font(size: int) -> ImageFont.ImageFont:
    try:
        return ImageFont.truetype("arial.ttf", size)
    except OSError:
        return ImageFont.load_default()


def load_manifest(path: Path) -> dict[tuple[str, str, str], dict[str, str]]:
    with path.open("r", newline="", encoding="utf-8") as handle:
        rows = list(csv.DictReader(handle))
    return {(row["cat"], row["skill"], row["size"]): row for row in rows}


def alpha_fit(image: Image.Image, size: int) -> Image.Image:
    image = image.convert("RGBA")
    scale = min(size / image.width, size / image.height)
    new_size = (max(1, round(image.width * scale)), max(1, round(image.height * scale)))
    return image.resize(new_size, Image.Resampling.LANCZOS)


def center_paste(base: Image.Image, overlay: Image.Image, center: tuple[int, int]) -> None:
    x = center[0] - overlay.width // 2
    y = center[1] - overlay.height // 2
    base.alpha_composite(overlay, (x, y))


def make_ready_card(frame: Image.Image, icon: Image.Image, size: int) -> Image.Image:
    card = frame.resize((size, size), Image.Resampling.LANCZOS)
    icon_img = alpha_fit(icon, round(size * 0.52))
    center_paste(card, icon_img, (size // 2, round(size * 0.49)))
    return card


def make_cooldown_card(frame: Image.Image, overlay: Image.Image, icon: Image.Image, size: int) -> Image.Image:
    card = make_ready_card(frame, icon, size)
    dim = Image.new("RGBA", (size, size), (2, 6, 16, 92))
    card.alpha_composite(dim)
    cooldown = overlay.resize((size, size), Image.Resampling.LANCZOS)
    card.alpha_composite(cooldown)
    return card


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch-dir", required=True, type=Path)
    parser.add_argument("--ready-frame", required=True, type=Path)
    parser.add_argument("--cooldown-overlay", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    args = parser.parse_args()

    manifest = load_manifest(
        args.batch_dir / "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv"
    )
    frame = Image.open(args.ready_frame).convert("RGBA")
    cooldown = Image.open(args.cooldown_overlay).convert("RGBA")

    ready_dir = args.out_dir / "ready_128"
    cooldown_dir = args.out_dir / "cooldown_64"
    ready_dir.mkdir(parents=True, exist_ok=True)
    cooldown_dir.mkdir(parents=True, exist_ok=True)

    ordered = []
    seen = set()
    for key in manifest:
        cat, skill, size = key
        if size == "256x256" and (cat, skill) not in seen:
            seen.add((cat, skill))
            ordered.append((cat, skill))

    rows = []
    board = Image.new("RGBA", (6 * 192 + 64, 3 * 184 + 96), (13, 19, 31, 255))
    draw = ImageDraw.Draw(board)
    draw.rectangle((0, 0, board.width, 54), fill=(29, 43, 68, 255))
    draw.text(
        (24, 18),
        "Batch 80 recommended set in actual Battle HUD skill frame / cooldown overlay",
        fill=(226, 238, 255, 255),
        font=load_font(16),
    )
    small = load_font(11)

    for index, (cat, skill) in enumerate(ordered):
        icon256_path = Path(manifest[(cat, skill, "256x256")]["path"])
        icon128_path = Path(manifest[(cat, skill, "128x128")]["path"])
        icon64_path = Path(manifest[(cat, skill, "64x64")]["path"])
        version = manifest[(cat, skill, "256x256")]["selected_source_version"]

        icon256 = Image.open(icon256_path).convert("RGBA")
        icon64 = Image.open(icon64_path).convert("RGBA")
        ready_card = make_ready_card(frame, icon256, 128)
        cooldown_card = make_cooldown_card(frame, cooldown, icon64, 64)

        ready_name = f"thecat_ui_skill_{cat}_{skill}_ready_128_hudtest_v001.png"
        cooldown_name = f"thecat_ui_skill_{cat}_{skill}_cooldown_64_hudtest_v001.png"
        ready_path = ready_dir / ready_name
        cooldown_path = cooldown_dir / cooldown_name
        ready_card.save(ready_path)
        cooldown_card.save(cooldown_path)

        col = index % 6
        row = index // 6
        x = 32 + col * 192
        y = 78 + row * 184
        panel = (25, 38, 61, 255) if row != 1 else (220, 232, 238, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 144, y + 144), radius=8, fill=panel, outline=(80, 112, 142, 255), width=1)
        board.alpha_composite(ready_card, (x, y))
        board.alpha_composite(cooldown_card, (x + 78, y + 46))
        draw.text((x, y + 132), f"{cat}/{version}", fill=label, font=small)

        rows.append(
            {
                "cat": cat,
                "skill": skill,
                "selected_source_version": version,
                "ready_128": ready_path.as_posix(),
                "cooldown_64": cooldown_path.as_posix(),
                "status": "hud_overlay_test_pending_visual_review",
                "notes": "Candidate-only composite using actual skill_ready_frame and skill_cooldown_overlay assets.",
            }
        )

    args.out_dir.mkdir(parents=True, exist_ok=True)
    board.save(args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_board_v001.png")
    with (args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_report.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=[
                "cat",
                "skill",
                "selected_source_version",
                "ready_128",
                "cooldown_64",
                "status",
                "notes",
            ],
        )
        writer.writeheader()
        writer.writerows(rows)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
