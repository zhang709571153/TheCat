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


def load_rows(path: Path) -> list[dict[str, str]]:
    with path.open("r", newline="", encoding="utf-8") as handle:
        return list(csv.DictReader(handle))


def manifest_index(rows: list[dict[str, str]], keys: tuple[str, ...]) -> dict[tuple[str, ...], dict[str, str]]:
    return {tuple(row[key] for key in keys): row for row in rows}


def fit(image: Image.Image, max_size: int) -> Image.Image:
    image = image.convert("RGBA")
    scale = min(max_size / image.width, max_size / image.height)
    return image.resize((max(1, round(image.width * scale)), max(1, round(image.height * scale))), Image.Resampling.LANCZOS)


def paste_center(base: Image.Image, overlay: Image.Image, center: tuple[int, int]) -> None:
    base.alpha_composite(overlay, (center[0] - overlay.width // 2, center[1] - overlay.height // 2))


def compose(frame_path: Path, icon_path: Path, size: int) -> Image.Image:
    frame = Image.open(frame_path).convert("RGBA").resize((size, size), Image.Resampling.LANCZOS)
    icon = fit(Image.open(icon_path).convert("RGBA"), round(size * 0.72))
    paste_center(frame, icon, (size // 2, size // 2))
    return frame


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch80-dir", required=True, type=Path)
    parser.add_argument("--batch81-dir", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    args = parser.parse_args()

    rec_rows = load_rows(args.batch80_dir / "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv")
    frame_rows = load_rows(args.batch81_dir / "thecat_ui_skill_slot_frames_batch81_manifest.csv")
    rec = manifest_index(rec_rows, ("cat", "skill", "size"))
    frames = manifest_index(frame_rows, ("shape", "state", "size"))

    out_rows: list[dict[str, str]] = []
    for sub in ("square_ready_128", "round_ready_128", "square_cooldown_128", "round_cooldown_128"):
        (args.out_dir / sub).mkdir(parents=True, exist_ok=True)

    ordered = []
    seen = set()
    for row in rec_rows:
        if row["size"] == "256x256":
            key = (row["cat"], row["skill"])
            if key not in seen:
                seen.add(key)
                ordered.append(key)

    board = Image.new("RGBA", (6 * 216 + 64, 3 * 232 + 96), (13, 19, 31, 255))
    draw = ImageDraw.Draw(board)
    draw.rectangle((0, 0, board.width, 54), fill=(29, 43, 68, 255))
    draw.text(
        (24, 18),
        "Batch 81 skill slot fit: square/round ready + cooldown with Batch 80 recommended icons",
        fill=(226, 238, 255, 255),
        font=load_font(16),
    )
    small = load_font(11)

    for index, (cat, skill) in enumerate(ordered):
        icon_path = Path(rec[(cat, skill, "256x256")]["path"])
        version = rec[(cat, skill, "256x256")]["selected_source_version"]
        outputs = {
            "square_ready": compose(Path(frames[("square", "ready", "128x128")]["path"]), icon_path, 128),
            "round_ready": compose(Path(frames[("round", "ready", "128x128")]["path"]), icon_path, 128),
            "square_cooldown": compose(Path(frames[("square", "cooldown", "128x128")]["path"]), icon_path, 128),
            "round_cooldown": compose(Path(frames[("round", "cooldown", "128x128")]["path"]), icon_path, 128),
        }
        for state, image in outputs.items():
            directory = args.out_dir / f"{state}_128"
            filename = f"thecat_ui_skill_{cat}_{skill}_{state}_128_slotfit_v001.png"
            path = directory / filename
            image.save(path)
            out_rows.append(
                {
                    "cat": cat,
                    "skill": skill,
                    "selected_icon_version": version,
                    "slot_state": state,
                    "path": path.as_posix(),
                    "status": "slot_fit_pending_visual_review",
                }
            )

        col = index % 6
        row = index // 6
        x = 32 + col * 216
        y = 78 + row * 232
        panel = (25, 38, 61, 255) if row != 1 else (220, 232, 238, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 168, y + 184), radius=8, fill=panel, outline=(80, 112, 142, 255), width=1)
        board.alpha_composite(outputs["square_ready"].resize((72, 72), Image.Resampling.LANCZOS), (x, y))
        board.alpha_composite(outputs["round_ready"].resize((72, 72), Image.Resampling.LANCZOS), (x + 86, y))
        board.alpha_composite(outputs["square_cooldown"].resize((72, 72), Image.Resampling.LANCZOS), (x, y + 84))
        board.alpha_composite(outputs["round_cooldown"].resize((72, 72), Image.Resampling.LANCZOS), (x + 86, y + 84))
        draw.text((x, y + 162), f"{cat}/{version}", fill=label, font=small)

    args.out_dir.mkdir(parents=True, exist_ok=True)
    board.save(args.out_dir / "thecat_ui_skill_slot_frames_batch81_icon_fit_board_v001.png")
    with (args.out_dir / "thecat_ui_skill_slot_frames_batch81_icon_fit_report.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["cat", "skill", "selected_icon_version", "slot_state", "path", "status"],
        )
        writer.writeheader()
        writer.writerows(out_rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
