from __future__ import annotations

import csv
import hashlib
import math
from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_83_loading_start_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "loading_start" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

BG_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Backgrounds" / "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png"
LOGO_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_title_logo_512x256_v001.png"
PANEL_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_panel_dreamglass_512x256_v001.png"
BUTTON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_button_primary_384x96_v001.png"
GAUGE_FRAME_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_core_sleep_gauge_frame_384x48_v001.png"
GAUGE_FILL_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_core_sleep_gauge_fill_384x48_v001.png"
SLEEP_ICON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons" / "thecat_ui_core_sleep_icon_64_v001.png"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_process_note.md"
SPEC_PATH = CANDIDATE_DIR / "thecat_ui_loading_start_batch83_agent_review_prompt.md"

FIELD_NAMES = (
    "asset_id",
    "component_id",
    "variant_id",
    "target_size",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_assets",
    "source_sha256",
    "contact_sheet",
    "review_sheet",
    "review_note",
    "process_note",
    "source_model",
    "recommendation",
    "visual_review",
)

SPRITE_SPECS = (
    ("loading_progress", "progress_frame", (640, 48), "progress_frame", "textless loading progress frame"),
    ("loading_progress", "progress_fill", (640, 48), "progress_fill", "textless loading progress fill strip"),
    ("loading_spinner", "spinner_crescent", (128, 128), "spinner", "sleep-crescent loading spinner symbol"),
    ("loading_pulse", "dot_sequence", (384, 64), "dots", "five-step loading dot sequence"),
)

MOCKUP_SPECS = (
    ("loading_start_screen", "mockup_1920x1080", (1920, 1080), "16:9 full HD loading/start composition"),
    ("loading_start_screen", "mockup_1365x768", (1365, 768), "wide laptop loading/start composition"),
    ("loading_start_screen", "mockup_1280x720", (1280, 720), "720p loading/start composition"),
    ("loading_start_screen", "mockup_1024x768", (1024, 768), "4:3 tablet/window loading/start composition"),
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    SPRITE_DIR.mkdir(parents=True, exist_ok=True)
    MOCKUP_DIR.mkdir(parents=True, exist_ok=True)
    clean_previous_outputs()

    sources = {
        "background": Image.open(BG_SOURCE).convert("RGBA"),
        "logo": Image.open(LOGO_SOURCE).convert("RGBA"),
        "panel": Image.open(PANEL_SOURCE).convert("RGBA"),
        "button": Image.open(BUTTON_SOURCE).convert("RGBA"),
        "gauge_frame": Image.open(GAUGE_FRAME_SOURCE).convert("RGBA"),
        "gauge_fill": Image.open(GAUGE_FILL_SOURCE).convert("RGBA"),
        "sleep_icon": Image.open(SLEEP_ICON_SOURCE).convert("RGBA"),
    }

    generated: list[dict[str, object]] = []
    for component_id, variant_id, size, kind, visual_review in SPRITE_SPECS:
        sprite = build_sprite(sources, kind, size)
        asset_id = f"thecat_ui_loading_{variant_id}_{size[0]}x{size[1]}_candidate_v001"
        path = SPRITE_DIR / f"{asset_id}.png"
        sprite.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "sprite", path, visual_review))

    sprite_lookup = {row["variant_id"]: Image.open(row["path"]).convert("RGBA") for row in generated if row["asset_type"] == "sprite"}
    for component_id, variant_id, size, visual_review in MOCKUP_SPECS:
        mockup = build_mockup(sources, sprite_lookup, size)
        asset_id = f"thecat_ui_loading_start_{variant_id}_local_mockup_v001"
        path = MOCKUP_DIR / f"{asset_id}.png"
        mockup.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "local_mockup", path, visual_review))

    rows = build_manifest_rows(generated)
    write_manifest(rows)
    write_contact_sheet(generated)
    write_review_sheet(generated)
    write_review_note(rows)
    write_process_note(rows)
    write_spec_note()

    print(f"Wrote {len(rows)} Batch 83 loading/start preflight row(s).")
    print(to_repo_path(MANIFEST_PATH))


def clean_previous_outputs() -> None:
    if CANDIDATE_DIR.name != BATCH_SLUG or SPRITE_DIR.parent != CANDIDATE_DIR or MOCKUP_DIR.parent != CANDIDATE_DIR:
        raise RuntimeError("Refusing to clean unexpected Batch 83 output paths.")

    for folder in (SPRITE_DIR, MOCKUP_DIR):
        for path in folder.glob("*.png"):
            path.unlink()

    for path in (MANIFEST_PATH, CONTACT_SHEET_PATH, REVIEW_SHEET_PATH, REVIEW_NOTE_PATH, PROCESS_NOTE_PATH, SPEC_PATH):
        if not is_inside(path, CANDIDATE_DIR):
            raise RuntimeError(f"Refusing to clean Batch 83 path outside candidate directory: {path}")
        if path.exists():
            path.unlink()


def is_inside(path: Path, root: Path) -> bool:
    try:
        path.resolve().relative_to(root.resolve())
        return True
    except ValueError:
        return False


def make_generated_row(
    asset_id: str,
    component_id: str,
    variant_id: str,
    size: tuple[int, int],
    asset_type: str,
    path: Path,
    visual_review: str,
) -> dict[str, object]:
    return {
        "asset_id": asset_id,
        "component_id": component_id,
        "variant_id": variant_id,
        "size": size,
        "asset_type": asset_type,
        "path": path,
        "visual_review": visual_review,
    }


def build_sprite(sources: dict[str, Image.Image], kind: str, size: tuple[int, int]) -> Image.Image:
    if kind == "progress_frame":
        base = nine_slice_resize(sources["gauge_frame"], size, (32, 18, 32, 18))
        glow = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(glow)
        draw.rounded_rectangle((8, 8, size[0] - 9, size[1] - 9), radius=18, outline=(239, 204, 110, 126), width=3)
        return Image.alpha_composite(base, glow)

    if kind == "progress_fill":
        base = nine_slice_resize(sources["gauge_fill"], size, (32, 18, 32, 18))
        overlay = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(overlay)
        for x in range(size[0]):
            t = x / max(1, size[0] - 1)
            color = (
                int(116 + 96 * t),
                int(164 + 52 * t),
                int(220 + 24 * t),
                120,
            )
            draw.line((x, 10, x, size[1] - 11), fill=color)
        for x in range(48, size[0], 96):
            draw.line((x, 12, x + 38, size[1] - 14), fill=(245, 221, 151, 82), width=3)
        return Image.alpha_composite(base, overlay)

    if kind == "spinner":
        return build_spinner(sources["sleep_icon"], size)

    if kind == "dots":
        return build_dot_sequence(size)

    raise ValueError(f"Unhandled sprite kind: {kind}")


def build_spinner(sleep_icon: Image.Image, size: tuple[int, int]) -> Image.Image:
    w, h = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    bbox = (14, 14, w - 14, h - 14)
    draw.arc(bbox, start=28, end=318, fill=(240, 205, 111, 230), width=8)
    draw.arc((25, 25, w - 25, h - 25), start=34, end=224, fill=(132, 181, 226, 180), width=5)
    icon = fit_image(sleep_icon, (44, 44))
    image.alpha_composite(icon, ((w - icon.width) // 2, (h - icon.height) // 2))
    for angle in (30, 154, 276):
        x = w / 2 + math.cos(math.radians(angle)) * 45
        y = h / 2 + math.sin(math.radians(angle)) * 45
        draw.ellipse((x - 3, y - 3, x + 3, y + 3), fill=(247, 210, 116, 230))
    return image.filter(ImageFilter.UnsharpMask(radius=1.2, percent=90, threshold=3))


def build_dot_sequence(size: tuple[int, int]) -> Image.Image:
    w, h = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    cx0 = w // 2 - 120
    for index in range(5):
        alpha = 76 + index * 36
        radius = 11 + index
        cx = cx0 + index * 60
        cy = h // 2
        draw.ellipse((cx - radius - 5, cy - radius - 5, cx + radius + 5, cy + radius + 5), fill=(83, 130, 184, max(24, alpha // 3)))
        draw.ellipse((cx - radius, cy - radius, cx + radius, cy + radius), fill=(239, 205, 110, alpha))
        draw.ellipse((cx - 4, cy - 4, cx + 4, cy + 4), fill=(246, 238, 214, min(255, alpha + 20)))
    return image


def build_mockup(
    sources: dict[str, Image.Image],
    sprites: dict[str, Image.Image],
    size: tuple[int, int],
) -> Image.Image:
    w, h = size
    bg = cover_image(sources["background"], size)
    overlay = Image.new("RGBA", size, (7, 10, 24, 68))
    image = Image.alpha_composite(bg, overlay)

    logo_width = min(int(w * 0.36), 520)
    logo = fit_image(sources["logo"], (logo_width, logo_width // 2))
    logo_y = max(int(h * 0.15), 42)
    image.alpha_composite(logo, ((w - logo.width) // 2, logo_y))

    panel_width = min(int(w * 0.52), 720)
    panel_height = max(int(panel_width * 0.22), 150)
    panel = nine_slice_resize(sources["panel"], (panel_width, panel_height), (70, 58, 70, 58))
    panel = ImageEnhance.Brightness(panel).enhance(0.88)
    panel_y = int(h * 0.48)
    image.alpha_composite(panel, ((w - panel.width) // 2, panel_y))

    spinner_size = max(72, min(116, int(w * 0.075)))
    spinner = fit_image(sprites["spinner_crescent"], (spinner_size, spinner_size))
    spinner_x = w // 2 - spinner.width // 2
    spinner_y = panel_y + max(18, int(panel_height * 0.14))
    image.alpha_composite(spinner, (spinner_x, spinner_y))

    bar_width = min(int(w * 0.44), 640)
    bar_height = max(34, int(bar_width * 0.075))
    frame = fit_image(sprites["progress_frame"], (bar_width, bar_height))
    fill_full = fit_image(sprites["progress_fill"], (bar_width, bar_height))
    mask = Image.new("L", (bar_width, bar_height), 0)
    mdraw = ImageDraw.Draw(mask)
    mdraw.rounded_rectangle((0, 0, int(bar_width * 0.62), bar_height - 1), radius=bar_height // 2, fill=255)
    fill = Image.new("RGBA", (bar_width, bar_height), (0, 0, 0, 0))
    fill.alpha_composite(fill_full)
    fill.putalpha(Image.composite(fill.getchannel("A"), Image.new("L", (bar_width, bar_height), 0), mask))
    bar_x = (w - bar_width) // 2
    bar_y = panel_y + panel_height - int(panel_height * 0.32)
    image.alpha_composite(fill, (bar_x, bar_y))
    image.alpha_composite(frame, (bar_x, bar_y))

    dots = fit_image(sprites["dot_sequence"], (min(280, int(w * 0.22)), max(42, int(h * 0.045))))
    image.alpha_composite(dots, ((w - dots.width) // 2, bar_y + bar_height + max(10, h // 90)))

    button_width = min(int(w * 0.28), 384)
    button_height = max(48, int(button_width * 0.25))
    button = nine_slice_resize(sources["button"], (button_width, button_height), (56, 30, 56, 30))
    button = ImageEnhance.Brightness(button).enhance(0.82)
    button.putalpha(ImageEnhance.Brightness(button.getchannel("A")).enhance(0.72))
    button_y = min(h - button_height - 48, panel_y + panel_height + max(58, h // 14))
    image.alpha_composite(button, ((w - button_width) // 2, button_y))

    return image


def nine_slice_resize(image: Image.Image, size: tuple[int, int], borders: tuple[int, int, int, int]) -> Image.Image:
    src = image.convert("RGBA")
    target_w, target_h = size
    left, top, right, bottom = borders
    src_w, src_h = src.size
    left = min(left, src_w // 3)
    right = min(right, src_w // 3)
    top = min(top, src_h // 3)
    bottom = min(bottom, src_h // 3)
    center_w = max(1, src_w - left - right)
    center_h = max(1, src_h - top - bottom)
    target_center_w = max(1, target_w - left - right)
    target_center_h = max(1, target_h - top - bottom)
    out = Image.new("RGBA", size, (0, 0, 0, 0))
    pieces = [
        ((0, 0, left, top), (0, 0, left, top)),
        ((left, 0, left + center_w, top), (left, 0, left + target_center_w, top)),
        ((src_w - right, 0, src_w, top), (target_w - right, 0, target_w, top)),
        ((0, top, left, top + center_h), (0, top, left, top + target_center_h)),
        ((left, top, left + center_w, top + center_h), (left, top, left + target_center_w, top + target_center_h)),
        ((src_w - right, top, src_w, top + center_h), (target_w - right, top, target_w, top + target_center_h)),
        ((0, src_h - bottom, left, src_h), (0, target_h - bottom, left, target_h)),
        ((left, src_h - bottom, left + center_w, src_h), (left, target_h - bottom, left + target_center_w, target_h)),
        ((src_w - right, src_h - bottom, src_w, src_h), (target_w - right, target_h - bottom, target_w, target_h)),
    ]
    for src_box, dst_box in pieces:
        piece = src.crop(src_box)
        dst_w = max(1, dst_box[2] - dst_box[0])
        dst_h = max(1, dst_box[3] - dst_box[1])
        if piece.size != (dst_w, dst_h):
            piece = piece.resize((dst_w, dst_h), Image.Resampling.LANCZOS)
        out.alpha_composite(piece, (dst_box[0], dst_box[1]))
    return out


def fit_image(image: Image.Image, box: tuple[int, int]) -> Image.Image:
    w, h = image.size
    max_w, max_h = box
    scale = min(max_w / w, max_h / h)
    new_size = (max(1, int(w * scale)), max(1, int(h * scale)))
    return image.resize(new_size, Image.Resampling.LANCZOS)


def cover_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    target_w, target_h = size
    src_w, src_h = image.size
    scale = max(target_w / src_w, target_h / src_h)
    resized = image.resize((int(src_w * scale), int(src_h * scale)), Image.Resampling.LANCZOS)
    left = (resized.width - target_w) // 2
    top = (resized.height - target_h) // 2
    return resized.crop((left, top, left + target_w, top + target_h)).convert("RGBA")


def build_manifest_rows(generated: list[dict[str, object]]) -> list[dict[str, str]]:
    source_paths = [BG_SOURCE, LOGO_SOURCE, PANEL_SOURCE, BUTTON_SOURCE, GAUGE_FRAME_SOURCE, GAUGE_FILL_SOURCE, SLEEP_ICON_SOURCE]
    source_assets = ";".join(to_repo_path(path) for path in source_paths)
    source_hashes = ";".join(sha256(path) for path in source_paths)
    rows: list[dict[str, str]] = []
    for item in generated:
        path = item["path"]
        assert isinstance(path, Path)
        size = item["size"]
        assert isinstance(size, tuple)
        rows.append(
            {
                "asset_id": str(item["asset_id"]),
                "component_id": str(item["component_id"]),
                "variant_id": str(item["variant_id"]),
                "target_size": f"{size[0]}x{size[1]}",
                "batch_slug": BATCH_SLUG,
                "asset_type": str(item["asset_type"]),
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": f"{Image.open(path).size[0]}x{Image.open(path).size[1]}",
                "source_assets": source_assets,
                "source_sha256": source_hashes,
                "contact_sheet": to_repo_path(CONTACT_SHEET_PATH),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "source_model": "deterministic_local_derivative_not_image2",
                "recommendation": "candidate_only_pending_unity_loading_start_screenshot",
                "visual_review": str(item["visual_review"]),
            }
        )
    return rows


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_contact_sheet(generated: list[dict[str, object]]) -> None:
    thumb_w, thumb_h = 300, 190
    cols = 4
    rows = math.ceil(len(generated) / cols)
    sheet = Image.new("RGBA", (cols * thumb_w + 60, rows * (thumb_h + 42) + 80), (24, 27, 38, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(26)
    label_font = load_font(14)
    draw.text((30, 24), "P0 Batch 83 - Loading/Start Preflight", fill=(245, 238, 220), font=title_font)
    for index, item in enumerate(generated):
        col = index % cols
        row = index // cols
        x = 30 + col * thumb_w
        y = 70 + row * (thumb_h + 42)
        path = item["path"]
        assert isinstance(path, Path)
        img = Image.open(path).convert("RGBA")
        draw_checker(sheet, (x, y, x + thumb_w - 20, y + thumb_h - 26))
        fitted = fit_image(img, (thumb_w - 34, thumb_h - 42))
        sheet.alpha_composite(fitted, (x + (thumb_w - 20 - fitted.width) // 2, y + (thumb_h - 26 - fitted.height) // 2))
        draw.text((x, y + thumb_h - 22), str(item["variant_id"]), fill=(235, 236, 244), font=label_font)
    sheet.convert("RGB").save(CONTACT_SHEET_PATH)


def write_review_sheet(generated: list[dict[str, object]]) -> None:
    sheet = Image.new("RGBA", (1800, 1260), (22, 25, 35, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(16)
    note_font = load_font(15)
    draw.text((36, 28), "Batch 83 Review - Loading/Start Local Preflight", fill=(245, 238, 220), font=title_font)
    draw.text((36, 78), "Uses existing Qr1 UI shell primitives; candidate-only, no new cat/body art, no Unity acceptance.", fill=(239, 205, 110), font=note_font)

    sprites = [item for item in generated if item["asset_type"] == "sprite"]
    mockups = [item for item in generated if item["asset_type"] == "local_mockup"]
    draw.text((44, 122), "textless candidate sprites", fill=(205, 215, 245), font=label_font)
    for index, item in enumerate(sprites):
        x = 44 + index * 420
        y = 158
        path = item["path"]
        assert isinstance(path, Path)
        img = Image.open(path).convert("RGBA")
        draw_checker(sheet, (x, y, x + 360, y + 170))
        fitted = fit_image(img, (330, 130))
        sheet.alpha_composite(fitted, (x + (360 - fitted.width) // 2, y + (150 - fitted.height) // 2))
        draw.text((x, y + 176), str(item["variant_id"]), fill=(245, 245, 245), font=label_font)

    draw.text((44, 400), "local screen mockups", fill=(205, 215, 245), font=label_font)
    for index, item in enumerate(mockups):
        x = 44 + (index % 2) * 860
        y = 438 + (index // 2) * 350
        path = item["path"]
        assert isinstance(path, Path)
        img = Image.open(path).convert("RGBA")
        fitted = fit_image(img, (800, 288))
        sheet.alpha_composite(fitted, (x, y))
        draw.rectangle((x, y, x + fitted.width, y + fitted.height), outline=(102, 137, 186, 160), width=2)
        draw.text((x, y + fitted.height + 8), str(item["variant_id"]), fill=(245, 245, 245), font=label_font)

    draw.text((44, 1190), "Review gates: verify loading/start in Unity; keep text rendered by UI except existing title logo; no image2 provenance claimed.", fill=(245, 238, 220), font=note_font)
    sheet.convert("RGB").save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    REVIEW_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 83 Loading/Start Preflight Candidate Review",
                "",
                "Scope: `ui_loading_start` screen-level preflight.",
                "",
                "Result: local candidate packet only. This does not prove Unity import, runtime layout, click targets, or final visual acceptance.",
                "",
                "Generated rows:",
                f"- Total manifest rows: {len(rows)}",
                "- Textless sprite candidates: progress frame, progress fill, spinner crescent, dot sequence.",
                "- Local mockups: 1920x1080, 1365x768, 1280x720, 1024x768.",
                "",
                "Source truth:",
                "- Qr1 UI/style source truth, live revision 816.",
                "- Existing local UI shell assets: main-menu dream-entry background, title logo, dreamglass panel, primary button, sleep gauge, sleep icon.",
                "",
                "Known limits:",
                "- Existing title logo contains English text by design; Batch 83 adds no baked Chinese text.",
                "- Mockups are local review evidence, not import-ready screen captures.",
                "- Strict image2 generation is not claimed; `OPENAI_API_KEY` was missing in this shell.",
                "- Unity screenshot, import settings, binding, and Console checks remain required.",
                "",
            ]
        ),
        encoding="utf-8",
    )


def write_process_note(rows: list[dict[str, str]]) -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 83 Process Note",
                "",
                "Lane: ui_loading_start.",
                "",
                "Production method: deterministic local derivative from existing Qr1 UI/style shell assets.",
                "",
                "Controls:",
                "- candidate-only under `design/development/asset_candidates/ui/loading_start/`",
                "- no files written under `Assets/`",
                "- no Unity `.meta` files generated",
                "- no starter-cat body, starter-cat frame, or character replacement art",
                "- no baked Chinese text in new candidate sprites",
                "- not image2 provenance; strict `gpt-image-2` CLI generation remains blocked without `OPENAI_API_KEY`",
                "",
                f"Manifest rows: {len(rows)}",
                "",
                "Promotion gate: use this packet only to decide whether the current loading/start primitives are sufficient. Formal acceptance requires Unity-rendered loading/start screenshots across target resolutions plus import/binding/Console proof.",
                "",
            ]
        ),
        encoding="utf-8",
    )


def write_spec_note() -> None:
    SPEC_PATH.write_text(
        "\n".join(
            [
                "# P0 Asset Batch 83 - Loading/Start Preflight",
                "",
                "Reviewer brief:",
                "- Check whether the loading/start packet follows Qr1 UI/style truth.",
                "- Confirm it does not introduce starter-cat body/frame art or character replacements.",
                "- Confirm new candidate sprites remain textless and candidate-only.",
                "- Check whether the progress/spinner treatment is readable on the four local mockups.",
                "- Treat all Unity runtime acceptance as pending.",
                "",
                "Primary evidence:",
                f"- `{to_repo_path(MANIFEST_PATH)}`",
                f"- `{to_repo_path(REVIEW_SHEET_PATH)}`",
                f"- `{to_repo_path(PROCESS_NOTE_PATH)}`",
                "",
            ]
        ),
        encoding="utf-8",
    )


def draw_checker(image: Image.Image, box: tuple[int, int, int, int]) -> None:
    draw = ImageDraw.Draw(image)
    x0, y0, x1, y1 = box
    cell = 14
    for y in range(y0, y1, cell):
        for x in range(x0, x1, cell):
            fill = (48, 53, 68, 255) if ((x - x0) // cell + (y - y0) // cell) % 2 == 0 else (37, 42, 56, 255)
            draw.rectangle((x, y, min(x + cell, x1), min(y + cell, y1)), fill=fill)


def load_font(size: int) -> ImageFont.ImageFont:
    for font_path in (
        "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/calibri.ttf",
    ):
        path = Path(font_path)
        if path.exists():
            return ImageFont.truetype(str(path), size)
    return ImageFont.load_default()


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
