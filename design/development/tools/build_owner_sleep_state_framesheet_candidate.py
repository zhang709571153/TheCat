from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_76_owner_sleep_state_framesheet_candidate_2026-06-24"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "owner_sleep_states" / BATCH_SLUG
FRAME_DIR = CANDIDATE_DIR / "frames"

SOURCE_PATH = CANDIDATE_DIR / "thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png"
ALPHA_SHEET_PATH = CANDIDATE_DIR / "thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_owner_sleep_states_batch76_contact_sheet_1920x1320_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png"
MANIFEST_PATH = CANDIDATE_DIR / "owner_sleep_states_batch76_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "owner_sleep_states_batch76_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "owner_sleep_states_batch76_process_note.md"
PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_76_owner_sleep_state_framesheet_candidate.md"
NORMALIZED_CELL_SIZE = 216
SOURCE_CELL_EDGE_TRIM = 20
FINAL_EDGE_CLEAR = 28

FRAME_STATES = (
    ("deep_sleep", "Deep sleep idle", "breathing loop, blue-lavender sleep motes"),
    ("half_awake", "Half dream / half awake", "subtle frown, finger twitch, first dream cracks"),
    ("near_awake", "Nearly awake warning", "stronger wake pulse, hand movement, amber cracks"),
    ("wake_failure", "Wake / failure transition", "eyes open, partial sit-up, dream fragments breaking"),
)

FIELD_NAMES = (
    "asset_id",
    "subject_id",
    "state_id",
    "frame_index",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_prompt_path",
    "source_image_path",
    "source_image_sha256",
    "alpha_sheet_path",
    "alpha_sheet_sha256",
    "review_sheet",
    "review_note",
    "process_note",
    "recommendation",
    "visual_review",
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    FRAME_DIR.mkdir(parents=True, exist_ok=True)

    alpha_sheet = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    if alpha_sheet.size != (1536, 1024):
        raise ValueError(f"Expected alpha sheet to be 1536x1024, found {alpha_sheet.size[0]}x{alpha_sheet.size[1]}")

    frames = split_frames(alpha_sheet)
    contact = build_contact_sheet(frames)
    contact.save(CONTACT_SHEET_PATH)
    review = build_review_sheet(frames, contact)
    review.save(REVIEW_SHEET_PATH)

    rows = build_manifest_rows(frames)
    write_manifest(rows)
    write_review_note(rows)
    write_process_note(rows)
    print(f"Wrote {len(rows)} Batch 76 manifest row(s).")
    print(to_repo_path(MANIFEST_PATH))


def split_frames(alpha_sheet: Image.Image) -> list[dict[str, object]]:
    frames: list[dict[str, object]] = []
    cell_width = alpha_sheet.width // 6
    cell_height = alpha_sheet.height // 4

    for row_index, (state_id, display_name, review) in enumerate(FRAME_STATES):
        for column_index in range(6):
            frame_number = column_index + 1
            box = (
                column_index * cell_width,
                row_index * cell_height,
                (column_index + 1) * cell_width,
                (row_index + 1) * cell_height,
            )
            raw_frame = alpha_sheet.crop(box)
            frame = normalize_frame(raw_frame)
            frame_name = f"thecat_owner_sleep_state_{state_id}_f{frame_number:02d}_256_candidate_v001.png"
            frame_path = FRAME_DIR / frame_name
            frame.save(frame_path)
            frames.append(
                {
                    "asset_id": frame_path.stem,
                    "state_id": state_id,
                    "display_name": display_name,
                    "frame_index": frame_number,
                    "path": frame_path,
                    "review": review,
                }
            )

    return frames


def build_contact_sheet(frames: list[dict[str, object]]) -> Image.Image:
    canvas = Image.new("RGBA", (1920, 1320), (35, 30, 43, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(38)
    label_font = load_font(20)
    small_font = load_font(16)

    draw.text((40, 30), "P0 Batch 76 - Owner Sleep State Frame Contact Sheet", fill=(245, 238, 220), font=title_font)
    draw.text((40, 82), "Candidate-only padded alpha frames. 4 states x 6 frames. Do not import until Unity runtime review.", fill=(238, 193, 106), font=small_font)

    cell = 256
    gap_x = 34
    gap_y = 54
    start_x = 144
    start_y = 158

    for index, frame_info in enumerate(frames):
        row = index // 6
        col = index % 6
        x = start_x + col * (cell + gap_x)
        y = start_y + row * (cell + gap_y)
        if col == 0:
            label = str(frame_info["state_id"]).replace("_", "\n")
            for line_index, line in enumerate(label.splitlines()):
                draw.text((28, y + 76 + line_index * 25), line, fill=(211, 224, 255), font=label_font)
        bg = checkerboard((cell, cell))
        frame = Image.open(frame_info["path"]).convert("RGBA")
        bg.alpha_composite(frame)
        canvas.alpha_composite(bg, (x, y))
        draw.rectangle((x, y, x + cell, y + cell), outline=(126, 136, 172, 255), width=2)
        draw.text((x + 6, y + cell + 8), f"f{int(frame_info['frame_index']):02d}", fill=(245, 238, 220), font=small_font)

    return canvas


def build_review_sheet(frames: list[dict[str, object]], contact: Image.Image) -> Image.Image:
    canvas = Image.new("RGBA", (1920, 1320), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(20)
    body_font = load_font(16)

    draw.text((36, 28), "P0 Batch 76 - Owner sleep state animation candidate", fill=(44, 38, 36), font=title_font)
    draw.text((36, 76), "Decision surface: imagegen v002 overlay, alpha-cleaned, padded into 24 frames, Unity import still pending.", fill=(126, 54, 45), font=body_font)

    source = Image.open(SOURCE_PATH).convert("RGBA")
    alpha = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    source_preview = fit(source, (550, 366))
    alpha_preview = fit(flat_composite(alpha, (32, 33, 44, 255)), (550, 366))
    contact_preview = fit(contact, (760, 522))

    draw_panel(canvas, draw, source_preview, (36, 124), "imagegen chroma source", label_font)
    draw_panel(canvas, draw, alpha_preview, (628, 124), "alpha sheet after chroma removal", label_font)
    draw_panel(canvas, draw, contact_preview, (36, 548), "split-frame contact sheet", label_font)

    x = 850
    y = 548
    draw.text((x, y), "Visual Review", fill=(44, 38, 36), font=label_font)
    y += 34
    bullets = [
        "Pass: four sleep/wake states read from left to right and top to bottom.",
        "Pass: no cats, no starter-cat body parts, no costume crops, and no UI text are present.",
        "Pass: v002 uses owner/pillow/blanket overlay art rather than a full bed-frame prop.",
        "Pass: padded frame normalization keeps the active 256x256 outputs away from slice edges for review.",
        "Pass: the owner-state overlay uses the bedroom-dream palette: navy blanket, warm lamp, blue-lavender sleep motes, amber wake warning.",
        "Watch: human owner identity is generic and should remain generic; do not treat this as a named character portrait.",
        "Unity gate: Sprite import, pivot/slicing, scene binding, runtime scale, Console, and sleep-state timing screenshots are still required.",
    ]
    for bullet in bullets:
        for line in wrap(bullet, 88):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22
        y += 10

    y += 16
    draw.text((x, y), "Frame Rows", fill=(44, 38, 36), font=label_font)
    y += 34
    for state_id, display_name, review in FRAME_STATES:
        draw.text((x, y), f"- {state_id}: {display_name}; {review}", fill=(44, 38, 36), font=body_font)
        y += 28

    return canvas


def build_manifest_rows(frames: list[dict[str, object]]) -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    sheet_hash = sha256(ALPHA_SHEET_PATH)
    source_hash = sha256(SOURCE_PATH)

    for frame in frames:
        path = Path(frame["path"])
        rows.append(
            {
                "asset_id": str(frame["asset_id"]),
                "subject_id": "owner_sleep_state",
                "state_id": str(frame["state_id"]),
                "frame_index": str(frame["frame_index"]),
                "batch_slug": BATCH_SLUG,
                "asset_type": "alpha_frame_256",
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": image_size(path),
                "source_prompt_path": to_repo_path(PROMPT_PATH),
                "source_image_path": to_repo_path(SOURCE_PATH),
                "source_image_sha256": source_hash,
                "alpha_sheet_path": to_repo_path(ALPHA_SHEET_PATH),
                "alpha_sheet_sha256": sheet_hash,
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "recommendation": "candidate_review_only_do_not_import",
                "visual_review": str(frame["review"]),
            }
        )

    return rows


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Owner Sleep States Batch 76 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This batch fills a P0 art-inventory gap for the owner-in-bed sleep state animation. It creates a non-cat v002 overlay sprite-sheet candidate and 24 padded alpha frames for later Unity slicing or overlay review.",
        "",
        "## Outputs",
        "",
        f"- Chroma source: `{to_repo_path(SOURCE_PATH)}`",
        f"- Alpha sheet: `{to_repo_path(ALPHA_SHEET_PATH)}`",
        f"- Frame directory: `{to_repo_path(FRAME_DIR)}`",
        f"- Contact sheet: `{to_repo_path(CONTACT_SHEET_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(PROMPT_PATH)}`",
        "",
        "## Visual Decision",
        "",
        "- Pass: 4 states x 6 frames are present: deep sleep, half awake, near awake, wake failure.",
        "- Pass: no cat body, fur markings, paws, tails, starter-cat costumes, or colored-turnaround crops are present.",
        "- Pass: v002 uses owner/pillow/blanket overlay art instead of a full bed-frame prop, reducing collision risk with the existing bedroom scene layer.",
        "- Pass: active frame outputs are normalized into padded 256x256 canvases for slice-safety review.",
        "- Pass: the sheet uses the bedroom-dream palette from the P0 art direction: navy star blanket, warm lamp, blue-lavender sleep, and amber wake warning.",
        "- Pass: near-awake and wake-failure rows include alarm/light vibration, dream cracks, and a consciousness orb returning toward the owner body.",
        "- Watch: v001 retained historical full-bed source evidence, but v002 is the active source for the manifest and generated frames.",
        "- Watch: the owner's face is generic and should stay generic; this is not a named character portrait.",
        "",
        "## Unity Gate",
        "",
        "- Import is blocked until Unity validates Sprite import settings, slicing, pivot, runtime scale, scene/prefab binding, sleep-state timing, battle-world screenshot readability, and Console status.",
        "- Agent review correction: v001 was source-safe but not slice-safe as raw 256x256 cells; v002 plus padded normalization is the current review packet.",
        "- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['state_id']}` frame `{row['frame_index']}` -> `{row['candidate_path']}`")

    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Owner Sleep States Batch 76 Process Note",
        "",
        "Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic padded frame splitting, contact sheet creation, manifest generation, and candidate review.",
        "",
        "Generation prompt summary:",
        "",
        "- 4-row by 6-column sprite sheet for the owner sleep-state overlay.",
        "- Row 1: deep sleep idle.",
        "- Row 2: half dream / half awake.",
        "- Row 3: nearly awake warning.",
        "- Row 4: wake / failure transition.",
        "- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.",
        "- V002 revision request added owner/pillow/blanket overlay framing, cell-safety margins, alarm/light vibration, dream cracks, and consciousness-orb return cues.",
        "",
        "Frame normalization:",
        "",
        f"- Each raw 256x256 sheet cell is trimmed by {SOURCE_CELL_EDGE_TRIM}px to remove neighbor-cell slivers, scaled to {NORMALIZED_CELL_SIZE}x{NORMALIZED_CELL_SIZE}, centered on a transparent 256x256 canvas, then cleared along a {FINAL_EDGE_CLEAR}px outer edge guard.",
        "- This preserves row/column timing while adding review margins around all candidate frame outputs.",
        "",
        "Chroma-key result:",
        "",
        "- Key color sampled by helper: `#03f902`.",
        "- Transparent pixels: 790092 / 1572864.",
        "- Partially transparent pixels: 34929 / 1572864.",
        "",
        f"Manifest rows: {len(rows)}.",
        "",
        "No Unity import was performed.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def normalize_frame(frame: Image.Image) -> Image.Image:
    normalized = Image.new("RGBA", (256, 256), (0, 0, 0, 0))
    trimmed = frame.crop(
        (
            SOURCE_CELL_EDGE_TRIM,
            SOURCE_CELL_EDGE_TRIM,
            256 - SOURCE_CELL_EDGE_TRIM,
            256 - SOURCE_CELL_EDGE_TRIM,
        )
    )
    scaled = trimmed.resize((NORMALIZED_CELL_SIZE, NORMALIZED_CELL_SIZE), Image.Resampling.LANCZOS)
    offset = (256 - NORMALIZED_CELL_SIZE) // 2
    normalized.alpha_composite(scaled, (offset, offset))
    clear_edge_alpha(normalized, FINAL_EDGE_CLEAR)
    return normalized


def clear_edge_alpha(image: Image.Image, margin: int) -> None:
    draw = ImageDraw.Draw(image)
    transparent = (0, 0, 0, 0)
    draw.rectangle((0, 0, image.width - 1, margin - 1), fill=transparent)
    draw.rectangle((0, image.height - margin, image.width - 1, image.height - 1), fill=transparent)
    draw.rectangle((0, 0, margin - 1, image.height - 1), fill=transparent)
    draw.rectangle((image.width - margin, 0, image.width - 1, image.height - 1), fill=transparent)


def draw_panel(canvas: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, xy: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = xy
    width, height = image.size
    draw.rectangle((x - 8, y - 8, x + width + 8, y + height + 38), fill=(255, 252, 245, 255), outline=(187, 168, 138, 255), width=2)
    canvas.alpha_composite(image, (x, y))
    draw.text((x, y + height + 12), label, fill=(44, 38, 36), font=font)


def checkerboard(size: tuple[int, int], square: int = 16) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (238, 238, 238, 255))
    draw = ImageDraw.Draw(image)
    for y in range(0, height, square):
        for x in range(0, width, square):
            if (x // square + y // square) % 2:
                draw.rectangle((x, y, min(x + square, width), min(y + square, height)), fill=(197, 197, 197, 255))
    return image


def flat_composite(image: Image.Image, color: tuple[int, int, int, int]) -> Image.Image:
    background = Image.new("RGBA", image.size, color)
    background.alpha_composite(image.convert("RGBA"))
    return background


def fit(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    image = image.convert("RGBA")
    image.thumbnail(size, Image.Resampling.LANCZOS)
    result = Image.new("RGBA", size, (0, 0, 0, 0))
    result.alpha_composite(image, ((size[0] - image.width) // 2, (size[1] - image.height) // 2))
    return result


def wrap(text: str, width: int) -> list[str]:
    words = text.split()
    lines: list[str] = []
    current: list[str] = []
    current_length = 0
    for word in words:
        next_length = current_length + len(word) + (1 if current else 0)
        if current and next_length > width:
            lines.append(" ".join(current))
            current = [word]
            current_length = len(word)
        else:
            current.append(word)
            current_length = next_length
    if current:
        lines.append(" ".join(current))
    return lines


def load_font(size: int) -> ImageFont.ImageFont:
    for font_path in (
        "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/arial.ttf",
    ):
        if Path(font_path).exists():
            return ImageFont.truetype(font_path, size=size)
    return ImageFont.load_default()


def image_size(path: Path) -> str:
    with Image.open(path) as image:
        return f"{image.width}x{image.height}"


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
