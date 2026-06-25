from __future__ import annotations

import argparse
import csv
from pathlib import Path

import numpy as np
from PIL import Image


STATES = ["ready", "cooldown", "disabled", "selected"]
VERSION = "v002_light"


def alpha_bbox(image: Image.Image, threshold: int = 16) -> tuple[int, int, int, int]:
    alpha = np.array(image.getchannel("A"))
    ys, xs = np.where(alpha > threshold)
    if xs.size == 0 or ys.size == 0:
        return (0, 0, image.width, image.height)
    return (int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1)


def fit_frame(frame: Image.Image, size: int, max_fill: int) -> Image.Image:
    cropped = frame.crop(alpha_bbox(frame))
    scale = min(max_fill / cropped.width, max_fill / cropped.height, 1.0)
    fitted_size = (max(1, round(cropped.width * scale)), max(1, round(cropped.height * scale)))
    fitted = cropped.resize(fitted_size, Image.Resampling.LANCZOS)
    out = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    out.alpha_composite(fitted, ((size - fitted.width) // 2, (size - fitted.height) // 2))
    return out


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--input", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    parser.add_argument("--manifest", required=True, type=Path)
    args = parser.parse_args()

    source = Image.open(args.input).convert("RGBA")
    cell_w = source.width / 4
    rows: list[dict[str, str]] = []

    for size in (256, 128, 64):
        (args.out_dir / f"frames_square_{VERSION}_{size}").mkdir(parents=True, exist_ok=True)

    contact = Image.new("RGBA", (4 * 256, 256), (0, 0, 0, 0))
    for col, state in enumerate(STATES):
        box = (
            round(col * cell_w),
            0,
            round((col + 1) * cell_w),
            source.height,
        )
        cell = source.crop(box)
        frame_256 = fit_frame(cell, 256, 244)
        contact.alpha_composite(frame_256, (col * 256, 0))

        for size, max_fill in ((256, 244), (128, 122), (64, 61)):
            output = fit_frame(frame_256, size, max_fill)
            asset_id = f"thecat_ui_skill_slot_square_{state}_{size}_candidate_{VERSION}"
            path = args.out_dir / f"frames_square_{VERSION}_{size}" / f"{asset_id}.png"
            output.save(path)
            rows.append(
                {
                    "asset_id": asset_id,
                    "shape": "square",
                    "state": state,
                    "size": f"{size}x{size}",
                    "path": path.as_posix(),
                    "status": "candidate_pending_visual_review",
                    "source": args.input.as_posix(),
                    "version": VERSION,
                    "notes": "Square-only lighter ornament variant; candidate-only, not Unity import approval.",
                }
            )

    contact.save(args.out_dir / f"thecat_ui_skill_slot_frames_batch81_contact_sheet_{VERSION}.png")
    args.manifest.parent.mkdir(parents=True, exist_ok=True)
    with args.manifest.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["asset_id", "shape", "state", "size", "path", "status", "source", "version", "notes"],
        )
        writer.writeheader()
        writer.writerows(rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
