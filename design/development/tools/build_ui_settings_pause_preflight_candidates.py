from __future__ import annotations

import csv
import hashlib
import math
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_85_settings_pause_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "settings_screen" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

BG_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Backgrounds" / "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png"
PANEL_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_panel_dreamglass_512x256_v001.png"
BUTTON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_button_primary_384x96_v001.png"
TITLE_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_title_logo_512x256_v001.png"
SETTINGS_MANIFEST = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "settings_controls" / "batch_78_settings_control_candidates_2026-06-24" / "settings_controls_batch78_manifest.csv"
SYSTEM_ICON_MANIFEST = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "system_icons" / "batch_79_system_icon_candidates_2026-06-24" / "system_icons_batch79_manifest.csv"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_process_note.md"
SPEC_PATH = CANDIDATE_DIR / "thecat_ui_settings_pause_batch85_agent_review_prompt.md"

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
    "contact_sheet_sha256",
    "review_sheet",
    "review_sheet_sha256",
    "review_note",
    "process_note",
    "source_model",
    "recommendation",
    "visual_review",
)

SPRITE_SPECS = (
    ("settings_pause_panel", "screen_panel_frame", (960, 640), "panel", "large textless settings/pause panel frame"),
    ("settings_pause_tabs", "tab_bar_frame", (760, 80), "tabs", "textless segmented tab bar with three slots"),
    ("settings_pause_option_row", "option_row_frame", (840, 96), "option_row", "settings row frame with icon well and control well"),
    ("settings_pause_modal", "confirm_modal_frame", (720, 420), "modal", "confirmation modal panel frame"),
    ("settings_pause_hint", "key_hint_chip_frame", (256, 72), "hint_chip", "keyboard/controller hint chip frame"),
    ("settings_pause_divider", "settings_section_divider", (640, 24), "divider", "thin settings section divider"),
)

MOCKUP_SPECS = (
    ("settings_screen", "settings_main_1920x1080", (1920, 1080), "main", "main settings screen local mockup"),
    ("settings_screen", "settings_audio_1365x768", (1365, 768), "audio", "audio settings screen local mockup"),
    ("pause_overlay", "pause_overlay_1280x720", (1280, 720), "pause", "pause overlay local mockup"),
    ("settings_screen", "settings_compact_1024x768", (1024, 768), "compact", "compact settings screen local mockup"),
)

CONTROL_IDS = ("slider_track", "slider_knob", "switch_off", "switch_on", "checkbox_unchecked", "checkbox_checked")
ICON_IDS = ("settings", "sound", "mute", "back", "close", "pause", "continue", "retry")


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    SPRITE_DIR.mkdir(parents=True, exist_ok=True)
    MOCKUP_DIR.mkdir(parents=True, exist_ok=True)
    clean_previous_outputs()

    controls = load_controls()
    icons = load_icons()
    sources = {
        "background": Image.open(BG_SOURCE).convert("RGBA"),
        "panel": Image.open(PANEL_SOURCE).convert("RGBA"),
        "button": Image.open(BUTTON_SOURCE).convert("RGBA"),
        "title": Image.open(TITLE_SOURCE).convert("RGBA"),
    }

    generated: list[dict[str, object]] = []
    for component_id, variant_id, size, kind, visual_review in SPRITE_SPECS:
        sprite = build_sprite(sources, kind, size)
        asset_id = f"thecat_ui_settings_pause_{variant_id}_{size[0]}x{size[1]}_candidate_v001"
        path = SPRITE_DIR / f"{asset_id}.png"
        sprite.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "sprite", path, visual_review))

    sprite_lookup = {
        row["variant_id"]: Image.open(row["path"]).convert("RGBA")
        for row in generated
        if row["asset_type"] == "sprite"
    }
    for component_id, variant_id, size, scenario, visual_review in MOCKUP_SPECS:
        mockup = build_mockup(sources, controls, icons, sprite_lookup, size, scenario)
        asset_id = f"thecat_ui_settings_pause_{variant_id}_local_mockup_v001"
        path = MOCKUP_DIR / f"{asset_id}.png"
        mockup.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "local_mockup", path, visual_review))

    write_contact_sheet(generated)
    write_review_sheet(generated)
    rows = build_manifest_rows(generated, controls, icons)
    write_manifest(rows)
    write_review_note(rows)
    write_process_note(rows)
    write_spec_note()

    print(f"Wrote {len(rows)} Batch 85 settings/pause preflight row(s).")
    print(to_repo_path(MANIFEST_PATH))


def clean_previous_outputs() -> None:
    for folder in (SPRITE_DIR, MOCKUP_DIR):
        if folder.parent != CANDIDATE_DIR:
            raise RuntimeError(f"Refusing to clean unexpected folder: {folder}")
        for path in folder.glob("*.png"):
            path.unlink()

    for path in (MANIFEST_PATH, CONTACT_SHEET_PATH, REVIEW_SHEET_PATH, REVIEW_NOTE_PATH, PROCESS_NOTE_PATH, SPEC_PATH):
        if not is_inside(path, CANDIDATE_DIR):
            raise RuntimeError(f"Refusing to clean path outside Batch 85 directory: {path}")
        if path.exists():
            path.unlink()


def is_inside(path: Path, root: Path) -> bool:
    try:
        path.resolve().relative_to(root.resolve())
        return True
    except ValueError:
        return False


def load_controls() -> dict[str, Image.Image]:
    rows = read_csv_by_key(SETTINGS_MANIFEST, "control_id")
    result: dict[str, Image.Image] = {}
    for control_id in CONTROL_IDS:
        row = rows[control_id]
        result[control_id] = Image.open(REPO_ROOT / row["candidate_path"]).convert("RGBA")
    return result


def load_icons() -> dict[str, Image.Image]:
    rows = read_csv_list(SYSTEM_ICON_MANIFEST)
    result: dict[str, Image.Image] = {}
    for icon_id in ICON_IDS:
        matches = [row for row in rows if row["icon_id"] == icon_id and row["size_variant"] == "64"]
        if len(matches) != 1:
            raise RuntimeError(f"Expected one 64px system icon for {icon_id}, found {len(matches)}")
        result[icon_id] = Image.open(REPO_ROOT / matches[0]["candidate_path"]).convert("RGBA")
    return result


def read_csv_by_key(path: Path, key: str) -> dict[str, dict[str, str]]:
    result: dict[str, dict[str, str]] = {}
    for row in read_csv_list(path):
        result[row[key]] = row
    return result


def read_csv_list(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8-sig", newline="") as handle:
        return list(csv.DictReader(handle))


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
    if kind == "panel":
        image = nine_slice_resize(sources["panel"], size, (78, 62, 78, 62))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((28, 28, size[0] - 29, size[1] - 29), radius=34, outline=(132, 198, 226, 105), width=3)
        draw.line((76, 118, size[0] - 76, 118), fill=(239, 204, 110, 104), width=3)
        draw.line((76, size[1] - 96, size[0] - 76, size[1] - 96), fill=(132, 198, 226, 62), width=2)
        return image

    if kind == "tabs":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((4, 4, size[0] - 5, size[1] - 5), radius=28, fill=(28, 36, 60, 220), outline=(132, 198, 226, 190), width=3)
        slot_w = (size[0] - 28) // 3
        for index in range(3):
            x0 = 14 + index * slot_w
            x1 = x0 + slot_w - 8
            fill = (52, 71, 105, 216) if index == 0 else (25, 32, 54, 196)
            outline = (239, 204, 110, 160) if index == 0 else (132, 198, 226, 78)
            draw.rounded_rectangle((x0, 13, x1, size[1] - 14), radius=20, fill=fill, outline=outline, width=2)
            draw_placeholder(draw, x0 + 34, size[1] // 2 - 3, x1 - 34, size[1] // 2 + 3, (246, 238, 214, 135))
        return image

    if kind == "option_row":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((5, 5, size[0] - 6, size[1] - 6), radius=22, fill=(34, 43, 70, 220), outline=(132, 198, 226, 160), width=3)
        draw.rounded_rectangle((20, 18, 78, size[1] - 18), radius=16, fill=(22, 30, 52, 210), outline=(239, 204, 110, 126), width=2)
        draw_placeholder(draw, 106, 28, 350, 34, (246, 238, 214, 135))
        draw_placeholder(draw, 106, 56, 270, 61, (132, 198, 226, 86))
        draw.rounded_rectangle((size[0] - 280, 20, size[0] - 28, size[1] - 20), radius=20, outline=(166, 126, 206, 98), width=2)
        return image

    if kind == "modal":
        image = nine_slice_resize(sources["panel"], size, (78, 62, 78, 62))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((34, 34, size[0] - 35, size[1] - 35), radius=32, outline=(166, 126, 206, 120), width=3)
        draw_placeholder(draw, 92, 92, size[0] - 92, 100, (246, 238, 214, 140))
        draw_placeholder(draw, 120, 150, size[0] - 120, 156, (132, 198, 226, 88))
        draw_placeholder(draw, 120, 184, size[0] - 180, 190, (132, 198, 226, 70))
        return image

    if kind == "hint_chip":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((5, 5, size[0] - 6, size[1] - 6), radius=24, fill=(29, 37, 61, 218), outline=(239, 204, 110, 178), width=3)
        draw.rounded_rectangle((18, 17, 72, size[1] - 18), radius=15, fill=(45, 59, 88, 220), outline=(132, 198, 226, 150), width=2)
        draw_placeholder(draw, 92, 27, size[0] - 34, 33, (246, 238, 214, 140))
        draw_placeholder(draw, 92, 49, size[0] - 78, 54, (132, 198, 226, 80))
        return image

    if kind == "divider":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        mid = size[1] // 2
        draw.line((0, mid, size[0], mid), fill=(132, 198, 226, 116), width=2)
        for x in range(72, size[0], 110):
            draw.ellipse((x - 5, mid - 5, x + 5, mid + 5), fill=(239, 204, 110, 184))
        return image

    raise ValueError(f"Unhandled sprite kind: {kind}")


def build_mockup(
    sources: dict[str, Image.Image],
    controls: dict[str, Image.Image],
    icons: dict[str, Image.Image],
    sprites: dict[str, Image.Image],
    size: tuple[int, int],
    scenario: str,
) -> Image.Image:
    w, h = size
    image = Image.alpha_composite(
        cover_image(sources["background"], size),
        Image.new("RGBA", size, (6, 10, 28, 100)),
    )
    draw = ImageDraw.Draw(image)

    panel_w = min(int(w * (0.70 if scenario != "compact" else 0.82)), 1080)
    panel_h = min(int(h * (0.72 if scenario != "pause" else 0.66)), 720)
    panel = fit_image(sprites["screen_panel_frame"], (panel_w, panel_h))
    px = (w - panel_w) // 2
    py = max(38, int(h * 0.12))
    image.alpha_composite(panel, (px, py))

    header_icon = icons["settings"] if scenario != "pause" else icons["pause"]
    image.alpha_composite(fit_image(header_icon, (58, 58)), (px + 62, py + 42))
    draw_placeholder(draw, px + 142, py + 57, px + min(panel_w - 220, 430), py + 66, (246, 238, 214, 150))
    draw_placeholder(draw, px + 142, py + 88, px + min(panel_w - 290, 360), py + 94, (132, 198, 226, 82))
    image.alpha_composite(fit_image(icons["close"], (44, 44)), (px + panel_w - 104, py + 50))

    if scenario != "pause":
        tabs = fit_image(sprites["tab_bar_frame"], (min(panel_w - 160, 760), 80))
        image.alpha_composite(tabs, (px + (panel_w - tabs.width) // 2, py + 136))
        row_y = py + 242
        row_specs = [
            ("sound", "slider"),
            ("mute", "switch_on" if scenario == "audio" else "switch_off"),
            ("settings", "checkbox_checked"),
            ("back", "checkbox_unchecked"),
        ]
        if scenario == "compact":
            row_specs = row_specs[:3]
        draw_rows(image, sprites, controls, icons, px, row_y, panel_w, panel_h, row_specs)
    else:
        modal = fit_image(sprites["confirm_modal_frame"], (min(panel_w - 170, 720), min(panel_h - 180, 420)))
        image.alpha_composite(modal, (px + (panel_w - modal.width) // 2, py + 156))
        image.alpha_composite(fit_image(icons["continue"], (64, 64)), (px + panel_w // 2 - 130, py + 246))
        image.alpha_composite(fit_image(icons["retry"], (64, 64)), (px + panel_w // 2 + 66, py + 246))
        button = fit_image(sources["button"], (260, 66))
        by = py + panel_h - 116
        image.alpha_composite(button, (px + panel_w // 2 - 292, by))
        image.alpha_composite(button, (px + panel_w // 2 + 32, by))
        draw_button_placeholder(draw, px + panel_w // 2 - 292, by, button.width, button.height)
        draw_button_placeholder(draw, px + panel_w // 2 + 32, by, button.width, button.height)

    hint = fit_image(sprites["key_hint_chip_frame"], (min(256, panel_w // 4), 72))
    hx = px + 72
    hy = py + panel_h - 88
    image.alpha_composite(hint, (hx, hy))
    if scenario in ("main", "compact"):
        image.alpha_composite(fit_image(icons["back"], (42, 42)), (hx + 22, hy + 15))

    draw_safe_area(image)
    return image


def draw_rows(
    image: Image.Image,
    sprites: dict[str, Image.Image],
    controls: dict[str, Image.Image],
    icons: dict[str, Image.Image],
    panel_x: int,
    row_y: int,
    panel_w: int,
    panel_h: int,
    row_specs: list[tuple[str, str]],
) -> None:
    row_w = min(panel_w - 150, 840)
    row = fit_image(sprites["option_row_frame"], (row_w, 96))
    gap = 18
    x = panel_x + (panel_w - row_w) // 2
    for index, (icon_id, control_id) in enumerate(row_specs):
        y = row_y + index * (row.height + gap)
        if y + row.height > row_y + panel_h - 230:
            break
        image.alpha_composite(row, (x, y))
        image.alpha_composite(fit_image(icons[icon_id], (48, 48)), (x + 25, y + 24))
        if control_id == "slider":
            image.alpha_composite(fit_image(controls["slider_track"], (300, 50)), (x + row_w - 348, y + 23))
            image.alpha_composite(fit_image(controls["slider_knob"], (58, 58)), (x + row_w - 180, y + 18))
        else:
            control = controls[control_id]
            image.alpha_composite(fit_image(control, (min(control.width, 168), min(control.height, 74))), (x + row_w - 224, y + 11))


def draw_placeholder(draw: ImageDraw.ImageDraw, x0: int, y0: int, x1: int, y1: int, color: tuple[int, int, int, int]) -> None:
    draw.rounded_rectangle((x0, y0, x1, y1), radius=max(2, (y1 - y0) // 2), fill=color)


def draw_button_placeholder(draw: ImageDraw.ImageDraw, x: int, y: int, width: int, height: int) -> None:
    draw_placeholder(draw, x + int(width * 0.23), y + height // 2 - 3, x + int(width * 0.77), y + height // 2 + 3, (246, 238, 214, 150))


def draw_safe_area(image: Image.Image) -> None:
    draw = ImageDraw.Draw(image)
    w, h = image.size
    mx = max(22, int(w * 0.035))
    my = max(18, int(h * 0.035))
    draw.rounded_rectangle((mx, my, w - mx, h - my), radius=18, outline=(246, 238, 214, 28), width=2)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    return image.resize(size, Image.Resampling.LANCZOS)


def cover_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    w, h = size
    scale = max(w / image.width, h / image.height)
    resized = image.resize((int(image.width * scale), int(image.height * scale)), Image.Resampling.LANCZOS)
    x = (resized.width - w) // 2
    y = (resized.height - h) // 2
    return resized.crop((x, y, x + w, y + h))


def nine_slice_resize(image: Image.Image, size: tuple[int, int], borders: tuple[int, int, int, int]) -> Image.Image:
    left, top, right, bottom = borders
    source_w, source_h = image.size
    target_w, target_h = size
    result = Image.new("RGBA", size, (0, 0, 0, 0))
    x_segments = ((0, left, 0, left), (left, source_w - right, left, target_w - right), (source_w - right, source_w, target_w - right, target_w))
    y_segments = ((0, top, 0, top), (top, source_h - bottom, top, target_h - bottom), (source_h - bottom, source_h, target_h - bottom, target_h))
    for sx0, sx1, tx0, tx1 in x_segments:
        for sy0, sy1, ty0, ty1 in y_segments:
            if sx1 <= sx0 or sy1 <= sy0 or tx1 <= tx0 or ty1 <= ty0:
                continue
            crop = image.crop((sx0, sy0, sx1, sy1)).resize((tx1 - tx0, ty1 - ty0), Image.Resampling.LANCZOS)
            result.alpha_composite(crop, (tx0, ty0))
    return result


def build_manifest_rows(
    generated: list[dict[str, object]],
    controls: dict[str, Image.Image],
    icons: dict[str, Image.Image],
) -> list[dict[str, str]]:
    del controls, icons
    source_paths = [
        BG_SOURCE,
        PANEL_SOURCE,
        BUTTON_SOURCE,
        TITLE_SOURCE,
        SETTINGS_MANIFEST,
        SYSTEM_ICON_MANIFEST,
    ]
    settings_rows = read_csv_list(SETTINGS_MANIFEST)
    icon_rows = read_csv_list(SYSTEM_ICON_MANIFEST)
    source_paths.extend(REPO_ROOT / row["candidate_path"] for row in settings_rows if row["control_id"] in CONTROL_IDS)
    source_paths.extend(REPO_ROOT / row["candidate_path"] for row in icon_rows if row["icon_id"] in ICON_IDS and row["size_variant"] == "64")
    source_asset_string = ";".join(to_repo_path(path) for path in source_paths)
    source_hash_string = ";".join(sha256(path) for path in source_paths)

    rows: list[dict[str, str]] = []
    for item in generated:
        path = item["path"]
        assert isinstance(path, Path)
        width, height = item["size"]
        rows.append(
            {
                "asset_id": str(item["asset_id"]),
                "component_id": str(item["component_id"]),
                "variant_id": str(item["variant_id"]),
                "target_size": f"{width}x{height}",
                "batch_slug": BATCH_SLUG,
                "asset_type": str(item["asset_type"]),
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": f"{Image.open(path).width}x{Image.open(path).height}",
                "source_assets": source_asset_string,
                "source_sha256": source_hash_string,
                "contact_sheet": to_repo_path(CONTACT_SHEET_PATH),
                "contact_sheet_sha256": sha256(CONTACT_SHEET_PATH),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_sheet_sha256": sha256(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "source_model": "deterministic_local_derivative_from_batch78_imagegen_not_image2",
                "recommendation": "candidate_only_pending_unity_settings_pause_screenshots",
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
    thumb_w, thumb_h = 360, 220
    cols = 2
    rows = math.ceil(len(generated) / cols)
    sheet = Image.new("RGBA", (cols * thumb_w, rows * (thumb_h + 50) + 44), (18, 22, 38, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((24, 16), "Batch 85 settings/pause preflight candidates", fill=(239, 204, 110, 255), font=font(20, bold=True))
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        col = index % cols
        row = index // cols
        x = col * thumb_w + 18
        y = 52 + row * (thumb_h + 50)
        draw.rounded_rectangle((x, y, x + thumb_w - 36, y + thumb_h), radius=12, fill=(33, 39, 63, 255), outline=(132, 198, 226, 120), width=2)
        image = Image.open(path).convert("RGBA")
        image.thumbnail((thumb_w - 64, thumb_h - 42), Image.Resampling.LANCZOS)
        sheet.alpha_composite(image, (x + (thumb_w - 36 - image.width) // 2, y + (thumb_h - image.height) // 2))
        draw.text((x, y + thumb_h + 8), str(item["variant_id"]), fill=(246, 238, 214, 255), font=font(16))
    sheet.save(CONTACT_SHEET_PATH)


def write_review_sheet(generated: list[dict[str, object]]) -> None:
    sheet = Image.new("RGBA", (1800, 1540), (15, 18, 32, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((48, 42), "Batch 85 Settings / Pause Preflight Review", fill=(239, 204, 110, 255), font=font(34, bold=True))
    draw.text((48, 90), "Candidate-only settings screen sprites and local mockups. No Unity import approval.", fill=(246, 238, 214, 220), font=font(20))
    card_w, card_h = 820, 220
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        image = Image.open(path).convert("RGBA")
        image.thumbnail((250, 145), Image.Resampling.LANCZOS)
        col = index % 2
        row = index // 2
        x = 48 + col * (card_w + 60)
        y = 150 + row * (card_h + 20)
        draw.rounded_rectangle((x, y, x + card_w, y + card_h), radius=18, fill=(37, 43, 68, 255), outline=(132, 198, 226, 96), width=2)
        sheet.alpha_composite(image, (x + 24, y + 24 + (145 - image.height) // 2))
        text_x = x + 310
        draw.text((text_x, y + 28), str(item["variant_id"]), fill=(246, 238, 214, 255), font=font(21, bold=True))
        draw.text((text_x, y + 64), str(item["visual_review"]), fill=(188, 204, 222, 255), font=font(16))
        draw.text((text_x, y + 126), "Gate: settings/pause screenshots, text replacement,", fill=(239, 204, 110, 230), font=font(15))
        draw.text((text_x, y + 150), "import settings, binding, Console.", fill=(239, 204, 110, 230), font=font(15))
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 85 Settings / Pause Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers settings main, audio settings, pause overlay, and compact settings local screen compositions.",
        "- Provides textless panel, tab bar, option row, modal, key hint, and divider sprites for later Unity import testing.",
        "- Reuses existing P0 UI shell, Batch 78 settings controls, and Batch 79 system icons without altering them.",
        "- Does not generate, crop, recolor, or import starter-cat body art.",
        "",
        "## Candidate Rows",
        "",
        "| Variant | Type | Size | Path |",
        "| --- | --- | --- | --- |",
    ]
    for row in rows:
        lines.append(f"| `{row['variant_id']}` | `{row['asset_type']}` | `{row['target_size']}` | `{row['candidate_path']}` |")
    lines.extend(
        [
            "",
            "## Required Unity Gates",
            "",
            "- Settings and pause screen screenshots at target resolutions.",
            "- Unity-rendered Chinese title, option, button, and value text replacement proof; no baked Chinese text in sprites.",
            "- Slider, switch, checkbox, tab, button, and close/back affordances must remain readable on 1024x768.",
            "- Sprite import settings, scene/prefab binding proof, pointer/click target proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 85 Settings / Pause Preflight Process Note",
        "",
        "Status: `candidate_only_pending_unity_settings_pause_screenshots`",
        "",
        "Authority: `Qr1` UI/style source truth plus existing local P0 UI asset inventory.",
        "",
        "This is a deterministic local derivative packet from existing UI shell assets, Batch 78 imagegen-derived settings controls, and Batch 79 system icons. It is not image2 provenance because `OPENAI_API_KEY` is unavailable in this shell.",
        "",
        "Controls:",
        "",
        "- candidate-only",
        "- no baked Chinese text",
        "- no `.meta` files",
        "- no writes under `Assets`",
        "- not image2 provenance",
        "- Unity-rendered settings/pause screenshots remain required",
        "",
        "Rows:",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['variant_id']}` -> `{row['candidate_path']}`")
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_spec_note() -> None:
    lines = [
        "# Batch 85 Agent Review Prompt",
        "",
        "Review the Batch 85 settings/pause preflight candidate packet.",
        "",
        "Primary checks:",
        "",
        "- Match `Qr1` UI/style tone: dark dreamglass panels, cyan/gold accents, restrained lavender controls.",
        "- Confirm sprites remain textless and candidate-only.",
        "- Confirm screen mockups show usable composition for settings main, audio settings, pause overlay, and compact settings.",
        "- Check 1024x768 density, tab bar readability, option rows, slider/switch/checkbox affordance, and close/back icon clarity.",
        "- Check no character-body generation or source-lock violation.",
        "- Check no `.meta` files and no path under `Assets`.",
        "",
        "Return PASS / PASS_WITH_P2 / FAIL_P1 / FAIL_P0 and concrete file-specific findings.",
    ]
    SPEC_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def sha256(path: Path) -> str:
    h = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            h.update(chunk)
    return h.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT.resolve()).as_posix()


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = [
        Path("C:/Windows/Fonts/msyhbd.ttc" if bold else "C:/Windows/Fonts/msyh.ttc"),
        Path("C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf"),
    ]
    for candidate in candidates:
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size)
    return ImageFont.load_default()


if __name__ == "__main__":
    main()
