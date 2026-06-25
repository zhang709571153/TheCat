from __future__ import annotations

import argparse
import csv
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


DIGITS = ["1", "12", "99"]


def load_font(size: int, bold: bool = False) -> ImageFont.ImageFont:
    candidates = [
        "arialbd.ttf" if bold else "arial.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/msyhbd.ttc" if bold else "C:/Windows/Fonts/msyh.ttc",
    ]
    for candidate in candidates:
        try:
            return ImageFont.truetype(candidate, size)
        except OSError:
            continue
    return ImageFont.load_default()


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open("r", newline="", encoding="utf-8") as handle:
        return list(csv.DictReader(handle))


def fit(image: Image.Image, max_size: int) -> Image.Image:
    image = image.convert("RGBA")
    scale = min(max_size / image.width, max_size / image.height)
    size = (max(1, round(image.width * scale)), max(1, round(image.height * scale)))
    return image.resize(size, Image.Resampling.LANCZOS)


def paste_center(base: Image.Image, overlay: Image.Image, center: tuple[int, int]) -> None:
    base.alpha_composite(overlay, (center[0] - overlay.width // 2, center[1] - overlay.height // 2))


def draw_cooldown_digit(image: Image.Image, digit: str) -> None:
    draw = ImageDraw.Draw(image)
    font_size = 25 if len(digit) == 1 else 20
    font = load_font(font_size, bold=True)
    center = (103, 27)
    bbox = draw.textbbox((0, 0), digit, font=font, stroke_width=1)
    width = bbox[2] - bbox[0]
    height = bbox[3] - bbox[1]
    position = (center[0] - width / 2, center[1] - height / 2 - 1)
    draw.text(position, digit, font=font, fill=(255, 239, 168, 255), stroke_width=2, stroke_fill=(19, 28, 47, 255))


def compose_square(slot: Image.Image, icon: Image.Image, digit: str) -> Image.Image:
    out = slot.copy().resize((128, 128), Image.Resampling.LANCZOS)
    icon_img = fit(icon, 84)
    paste_center(out, icon_img, (64, 66))
    draw_cooldown_digit(out, digit)
    return out


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch80-dir", required=True, type=Path)
    parser.add_argument("--batch81-dir", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    args = parser.parse_args()

    rec_rows = read_csv(args.batch80_dir / "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv")
    frame_rows = read_csv(args.batch81_dir / "thecat_ui_skill_slot_frames_batch81_manifest.csv")

    rec = {(row["cat"], row["skill"], row["size"]): row for row in rec_rows}
    frames = {(row["shape"], row["state"], row["size"]): row for row in frame_rows}
    square_cooldown = Image.open(frames[("square", "cooldown", "128x128")]["path"]).convert("RGBA")

    ordered: list[tuple[str, str]] = []
    seen = set()
    for row in rec_rows:
        if row["size"] == "256x256":
            key = (row["cat"], row["skill"])
            if key not in seen:
                seen.add(key)
                ordered.append(key)

    rows: list[dict[str, str]] = []
    args.out_dir.mkdir(parents=True, exist_ok=True)
    for digit in DIGITS:
        (args.out_dir / f"digit_{digit}").mkdir(parents=True, exist_ok=True)

    board = Image.new("RGBA", (6 * 216 + 64, 3 * 242 + 104), (13, 19, 31, 255))
    draw = ImageDraw.Draw(board)
    draw.rectangle((0, 0, board.width, 58), fill=(29, 43, 68, 255))
    draw.text(
        (24, 19),
        "Batch 81 square slot cooldown digit test: 1 / 12 / 99",
        fill=(226, 238, 255, 255),
        font=load_font(16),
    )
    small = load_font(11)

    for index, (cat, skill) in enumerate(ordered):
        icon_path = Path(rec[(cat, skill, "256x256")]["path"])
        icon = Image.open(icon_path).convert("RGBA")
        version = rec[(cat, skill, "256x256")]["selected_source_version"]

        composites = []
        for digit in DIGITS:
            composite = compose_square(square_cooldown, icon, digit)
            path = args.out_dir / f"digit_{digit}" / f"thecat_ui_skill_{cat}_{skill}_square_cooldown_digit_{digit}_128_candidate_v001.png"
            composite.save(path)
            composites.append((digit, composite, path))
            rows.append(
                {
                    "cat": cat,
                    "skill": skill,
                    "selected_icon_version": version,
                    "digit": digit,
                    "path": path.as_posix(),
                    "status": "cooldown_digit_mockup_pending_visual_review",
                    "notes": "Candidate-only square cooldown digit mockup; not a Unity screenshot.",
                }
            )

        col = index % 6
        row = index // 6
        x = 32 + col * 216
        y = 82 + row * 242
        panel = (25, 38, 61, 255) if row != 1 else (220, 232, 238, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 168, y + 194), radius=8, fill=panel, outline=(80, 112, 142, 255), width=1)
        for n, (digit, composite, _) in enumerate(composites):
            board.alpha_composite(composite.resize((62, 62), Image.Resampling.LANCZOS), (x + n * 52, y + 10))
            draw.text((x + n * 52 + 23, y + 76), digit, fill=label, font=small)
        draw.text((x, y + 154), f"{cat}/{version}", fill=label, font=small)
        draw.text((x, y + 170), skill[:24], fill=label, font=small)

    board.save(args.out_dir / "thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v001.png")
    with (args.out_dir / "thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["cat", "skill", "selected_icon_version", "digit", "path", "status", "notes"],
        )
        writer.writeheader()
        writer.writerows(rows)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
