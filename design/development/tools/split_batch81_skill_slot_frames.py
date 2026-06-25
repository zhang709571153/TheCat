from __future__ import annotations

import argparse
import csv
from pathlib import Path

import numpy as np
from PIL import Image


FRAMES = [
    ("square", "ready"),
    ("square", "cooldown"),
    ("square", "disabled"),
    ("square", "selected"),
    ("round", "ready"),
    ("round", "cooldown"),
    ("round", "disabled"),
    ("round", "selected"),
]


def alpha_bbox(image: Image.Image, threshold: int = 16) -> tuple[int, int, int, int]:
    alpha = np.array(image.getchannel("A"))
    ys, xs = np.where(alpha > threshold)
    if xs.size == 0 or ys.size == 0:
        return (0, 0, image.width, image.height)
    return (int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1)


def fit_frame(frame: Image.Image, size: int, max_fill: int) -> Image.Image:
    cropped = frame.crop(alpha_bbox(frame))
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
    cell_w = source.width / 4
    cell_h = source.height / 2
    rows: list[dict[str, str]] = []

    for size in (256, 128, 64):
        (args.out_dir / f"frames_{size}").mkdir(parents=True, exist_ok=True)

    contact = Image.new("RGBA", (4 * 256, 2 * 256), (0, 0, 0, 0))
    for index, (shape, state) in enumerate(FRAMES):
        row = index // 4
        col = index % 4
        box = (
            round(col * cell_w),
            round(row * cell_h),
            round((col + 1) * cell_w),
            round((row + 1) * cell_h),
        )
        cell = source.crop(box)
        frame_256 = fit_frame(cell, 256, 240)
        contact.alpha_composite(frame_256, (col * 256, row * 256))
        for size, max_fill in ((256, 240), (128, 120), (64, 60)):
            output = fit_frame(frame_256, size, max_fill)
            filename = f"thecat_ui_skill_slot_{shape}_{state}_{size}_candidate_v001.png"
            path = args.out_dir / f"frames_{size}" / filename
            output.save(path)
            rows.append(
                {
                    "asset_id": f"thecat_ui_skill_slot_{shape}_{state}_{size}_candidate_v001",
                    "shape": shape,
                    "state": state,
                    "size": f"{size}x{size}",
                    "path": path.as_posix(),
                    "status": "candidate_pending_visual_review",
                    "source": args.input.as_posix(),
                }
            )

    contact.save(args.out_dir / "thecat_ui_skill_slot_frames_batch81_contact_sheet_v001.png")
    args.manifest.parent.mkdir(parents=True, exist_ok=True)
    with args.manifest.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["asset_id", "shape", "state", "size", "path", "status", "source"],
        )
        writer.writeheader()
        writer.writerows(rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
