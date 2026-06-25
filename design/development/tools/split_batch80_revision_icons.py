from __future__ import annotations

import argparse
import csv
from pathlib import Path

import numpy as np
from PIL import Image


REVISED = [
    ("saiban", "battle_flag_rally"),
    ("suzune", "team_heal_ice_enchant"),
]


def alpha_bbox(image: Image.Image, threshold: int = 16) -> tuple[int, int, int, int]:
    alpha = np.array(image.getchannel("A"))
    ys, xs = np.where(alpha > threshold)
    if xs.size == 0 or ys.size == 0:
        return (0, 0, image.width, image.height)
    return (int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1)


def fit_icon(icon: Image.Image, size: int, max_fill: int) -> Image.Image:
    cropped = icon.crop(alpha_bbox(icon))
    scale = min(max_fill / cropped.width, max_fill / cropped.height, 1.0)
    new_size = (max(1, round(cropped.width * scale)), max(1, round(cropped.height * scale)))
    resized = cropped.resize(new_size, Image.Resampling.LANCZOS)
    out = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    out.alpha_composite(resized, ((size - resized.width) // 2, (size - resized.height) // 2))
    return out


def append_manifest(manifest: Path, rows: list[dict[str, str]]) -> None:
    existing = []
    if manifest.exists():
        with manifest.open("r", newline="", encoding="utf-8") as handle:
            existing = list(csv.DictReader(handle))
    all_rows = existing + rows
    with manifest.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["asset_id", "cat", "skill", "size", "path", "status", "source"],
        )
        writer.writeheader()
        writer.writerows(all_rows)


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--input", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    parser.add_argument("--manifest", required=True, type=Path)
    args = parser.parse_args()

    source = Image.open(args.input).convert("RGBA")
    cell_w = source.width / 2
    cell_h = source.height

    rows: list[dict[str, str]] = []
    contact = Image.new("RGBA", (2 * 256, 256), (0, 0, 0, 0))
    for index, (cat, skill) in enumerate(REVISED):
        box = (round(index * cell_w), 0, round((index + 1) * cell_w), round(cell_h))
        cell = source.crop(box)
        icon_256 = fit_icon(cell, 256, 232)
        contact.alpha_composite(icon_256, (index * 256, 0))
        asset_id_base = f"thecat_ui_skill_{cat}_{skill}"
        for size, max_fill in ((256, 232), (128, 116), (64, 58), (32, 29)):
            output = fit_icon(icon_256, size, max_fill)
            directory = args.out_dir / f"icons_{size}"
            directory.mkdir(parents=True, exist_ok=True)
            filename = f"{asset_id_base}_{size}_candidate_v002.png"
            output_path = directory / filename
            output.save(output_path)
            rows.append(
                {
                    "asset_id": f"{asset_id_base}_{size}_candidate_v002",
                    "cat": cat,
                    "skill": skill,
                    "size": f"{size}x{size}",
                    "path": output_path.as_posix(),
                    "status": "candidate_pending_visual_review",
                    "source": args.input.as_posix(),
                }
            )

    contact.save(args.out_dir / "thecat_ui_starter_skill_icon_motifs_batch80_v002_contact_sheet.png")
    append_manifest(args.manifest, rows)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
