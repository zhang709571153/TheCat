from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_82_common_ui_state_candidates_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "common_components" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"

BUTTON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_button_primary_384x96_v001.png"
PANEL_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_panel_dreamglass_512x256_v001.png"
ROUTECARD_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_routecard_shop_frame_512x256_v001.png"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_common_states_batch82_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_common_states_batch82_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_common_states_batch82_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_common_states_batch82_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_common_states_batch82_process_note.md"
SPEC_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_82_common_ui_state_derivative_candidates.md"

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
    ("button_state_atlas", "button_default", (384, 96), "button", "default", "primary button default state"),
    ("button_state_atlas", "button_hover", (384, 96), "button", "hover", "brighter hover state, textless"),
    ("button_state_atlas", "button_pressed", (384, 96), "button", "pressed", "darker pressed state, textless"),
    ("button_state_atlas", "button_selected", (384, 96), "button", "selected", "selected state with warm rim, textless"),
    ("button_state_atlas", "button_disabled", (384, 96), "button", "disabled", "disabled low-contrast state, textless"),
    ("button_state_atlas", "button_secondary", (384, 96), "button", "secondary", "muted secondary state, textless"),
    ("button_state_atlas", "button_danger", (384, 96), "button", "danger", "danger confirmation state, textless"),
    ("button_state_atlas", "button_focus", (384, 96), "button", "focus", "keyboard/controller focus state, textless"),
    ("modal_dialog_frame", "modal_large", (768, 432), "panel", "large", "large modal/dialog frame"),
    ("modal_dialog_frame", "modal_medium", (640, 360), "panel", "medium", "medium confirmation modal frame"),
    ("modal_dialog_frame", "modal_compact", (512, 320), "panel", "compact", "compact prompt modal frame"),
    ("tabs_segmented_controls", "tab_selected", (192, 64), "tab", "selected", "selected tab state"),
    ("tabs_segmented_controls", "tab_unselected", (192, 64), "tab", "unselected", "unselected tab state"),
    ("tabs_segmented_controls", "tab_disabled", (192, 64), "tab", "disabled", "disabled tab state"),
    ("tabs_segmented_controls", "segmented_left_selected", (512, 72), "segmented", "left_selected", "three-slot segmented control with left selected"),
    ("tabs_segmented_controls", "segmented_middle_selected", (512, 72), "segmented", "middle_selected", "three-slot segmented control with middle selected"),
    ("tabs_segmented_controls", "segmented_right_selected", (512, 72), "segmented", "right_selected", "three-slot segmented control with right selected"),
    ("list_row_frame", "list_row_default", (640, 80), "list_row", "default", "default compact list-row frame"),
    ("list_row_frame", "list_row_selected", (640, 80), "list_row", "selected", "selected compact list-row frame"),
    ("list_row_frame", "list_row_disabled", (640, 80), "list_row", "disabled", "disabled compact list-row frame"),
    ("list_row_frame", "list_row_warning", (640, 80), "list_row", "warning", "warning/locked compact list-row frame"),
    ("list_row_frame", "list_row_badge_default", (640, 80), "list_row_badge", "default", "list-row frame with leading badge well"),
    ("list_row_frame", "list_row_badge_selected", (640, 80), "list_row_badge", "selected", "selected list-row frame with leading badge well"),
    ("list_row_frame", "list_row_badge_disabled", (640, 80), "list_row_badge", "disabled", "disabled list-row frame with leading badge well"),
    ("list_row_frame", "list_row_badge_warning", (640, 80), "list_row_badge", "warning", "warning list-row frame with leading badge well"),
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    SPRITE_DIR.mkdir(parents=True, exist_ok=True)
    clean_previous_outputs()

    button = Image.open(BUTTON_SOURCE).convert("RGBA")
    panel = Image.open(PANEL_SOURCE).convert("RGBA")
    route_card = Image.open(ROUTECARD_SOURCE).convert("RGBA")

    generated: list[dict[str, object]] = []
    for component_id, variant_id, size, source_kind, state, visual_review in SPRITE_SPECS:
        sprite = build_sprite(button, panel, route_card, source_kind, state, size)
        asset_id = f"thecat_ui_common_{variant_id}_{size[0]}x{size[1]}_candidate_v001"
        path = SPRITE_DIR / f"{asset_id}.png"
        sprite.save(path)
        generated.append(
            {
                "asset_id": asset_id,
                "component_id": component_id,
                "variant_id": variant_id,
                "size": size,
                "path": path,
                "source_kind": source_kind,
                "visual_review": visual_review,
            }
        )

    rows = build_manifest_rows(generated)
    write_manifest(rows)
    write_contact_sheet(generated)
    write_review_sheet(generated)
    write_review_note(rows)
    write_process_note(rows)
    write_spec_note()

    print(f"Wrote {len(rows)} Batch 82 common UI state candidate row(s).")
    print(to_repo_path(MANIFEST_PATH))


def clean_previous_outputs() -> None:
    if CANDIDATE_DIR.name != BATCH_SLUG or SPRITE_DIR.parent != CANDIDATE_DIR:
        raise RuntimeError("Refusing to clean unexpected Batch 82 output paths.")

    for path in SPRITE_DIR.glob("*.png"):
        path.unlink()

    for path in (
        MANIFEST_PATH,
        CONTACT_SHEET_PATH,
        REVIEW_SHEET_PATH,
        REVIEW_NOTE_PATH,
        PROCESS_NOTE_PATH,
        SPEC_PATH,
    ):
        if path.exists():
            path.unlink()


def build_sprite(
    button: Image.Image,
    panel: Image.Image,
    route_card: Image.Image,
    source_kind: str,
    state: str,
    size: tuple[int, int],
) -> Image.Image:
    if source_kind == "button":
        base = nine_slice_resize(button, size, (56, 30, 56, 30))
        return style_button(base, state)

    if source_kind == "panel":
        base = nine_slice_resize(panel, size, (72, 58, 72, 58))
        return style_panel(base, state)

    if source_kind == "tab":
        base = nine_slice_resize(button, size, (46, 28, 46, 28))
        return style_tab(base, state)

    if source_kind == "segmented":
        return build_segmented_control(button, size, state)

    if source_kind == "list_row":
        base = make_plain_list_row_source(size)
        return style_list_row(base, state)

    if source_kind == "list_row_badge":
        base = nine_slice_resize(route_card, size, (48, 24, 48, 24))
        return style_list_row(base, state)

    raise ValueError(f"Unknown source kind: {source_kind}")


def style_button(image: Image.Image, state: str) -> Image.Image:
    styles = {
        "default": ((96, 116, 174), 0.10, 1.02, 1.0, None),
        "hover": ((128, 174, 230), 0.22, 1.18, 1.08, ((116, 206, 255), 5, 118)),
        "pressed": ((42, 50, 86), 0.18, 0.78, 0.9, None),
        "selected": ((238, 193, 106), 0.24, 1.10, 1.08, ((238, 193, 106), 4, 150)),
        "disabled": ((112, 116, 126), 0.35, 0.62, 0.42, None),
        "secondary": ((83, 101, 130), 0.16, 0.90, 0.86, None),
        "danger": ((207, 72, 86), 0.32, 1.02, 1.0, ((225, 82, 96), 4, 110)),
        "focus": ((104, 206, 255), 0.18, 1.08, 1.0, ((104, 206, 255), 6, 165)),
    }
    tint, tint_strength, brightness, alpha_scale, glow = styles[state]
    result = recolor(image, tint, tint_strength, brightness, alpha_scale)
    if glow:
        color, radius, opacity = glow
        result = add_glow(result, color, radius, opacity)
    if state in {"selected", "focus", "danger"}:
        result = add_rim(result, (238, 193, 106) if state == "selected" else tint, 2)
    return result


def style_panel(image: Image.Image, state: str) -> Image.Image:
    if state == "large":
        result = recolor(image, (88, 112, 164), 0.08, 1.03, 1.0)
        return add_rim(add_glow(result, (102, 180, 255), 4, 80), (238, 193, 106), 2)
    if state == "medium":
        result = recolor(image, (72, 92, 142), 0.10, 0.98, 1.0)
        return add_rim(result, (128, 174, 230), 2)
    if state == "compact":
        result = recolor(image, (92, 96, 124), 0.08, 0.92, 0.94)
        return add_rim(result, (126, 136, 172), 1)
    raise ValueError(f"Unknown panel state: {state}")


def style_tab(image: Image.Image, state: str) -> Image.Image:
    if state == "selected":
        return add_rim(add_glow(recolor(image, (238, 193, 106), 0.20, 1.08, 1.0), (238, 193, 106), 3, 110), (238, 193, 106), 2)
    if state == "unselected":
        return recolor(image, (76, 90, 128), 0.16, 0.82, 0.86)
    if state == "disabled":
        return recolor(image, (110, 112, 120), 0.36, 0.58, 0.42)
    raise ValueError(f"Unknown tab state: {state}")


def style_list_row(image: Image.Image, state: str) -> Image.Image:
    if state == "default":
        return recolor(image, (76, 90, 128), 0.08, 0.92, 0.82)
    if state == "selected":
        return add_rim(add_glow(recolor(image, (106, 164, 218), 0.16, 1.05, 0.95), (102, 180, 255), 3, 90), (238, 193, 106), 2)
    if state == "disabled":
        return recolor(image, (112, 112, 120), 0.32, 0.58, 0.36)
    if state == "warning":
        return add_rim(recolor(image, (207, 72, 86), 0.18, 0.92, 0.82), (225, 82, 96), 2)
    raise ValueError(f"Unknown list row state: {state}")


def build_segmented_control(button: Image.Image, size: tuple[int, int], state: str) -> Image.Image:
    canvas = Image.new("RGBA", size, (0, 0, 0, 0))
    tab_w = 184
    gap = -20
    selected_index = {
        "left_selected": 0,
        "middle_selected": 1,
        "right_selected": 2,
    }[state]
    tabs = []
    for index in range(3):
        tab_state = "selected" if index == selected_index else "unselected"
        tabs.append(style_tab(nine_slice_resize(button, (tab_w, 64), (46, 28, 46, 28)), tab_state))
    x = 8
    for tab in tabs:
        canvas.alpha_composite(tab, (x, 4))
        x += tab_w + gap
    return add_rim(canvas, (126, 136, 172), 1)


def make_plain_list_row_source(size: tuple[int, int]) -> Image.Image:
    w, h = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    outer = (10, 8, w - 10, h - 8)
    inner = (20, 18, w - 20, h - 18)
    draw.rounded_rectangle(outer, radius=18, fill=(58, 66, 92, 140), outline=(176, 146, 76, 190), width=2)
    draw.rounded_rectangle(inner, radius=14, fill=(70, 85, 124, 96), outline=(112, 186, 220, 120), width=1)
    return image


def recolor(
    image: Image.Image,
    tint: tuple[int, int, int],
    tint_strength: float,
    brightness: float,
    alpha_scale: float,
) -> Image.Image:
    image = image.convert("RGBA")
    alpha = image.getchannel("A").point(lambda value: max(0, min(255, round(value * alpha_scale))))
    rgb = image.convert("RGB")
    rgb = ImageEnhance.Color(rgb).enhance(0.88)
    rgb = ImageEnhance.Brightness(rgb).enhance(brightness)
    if tint_strength > 0:
        rgb = Image.blend(rgb, Image.new("RGB", image.size, tint), tint_strength)
    result = Image.merge("RGBA", (*rgb.split(), alpha))
    return result


def add_glow(image: Image.Image, color: tuple[int, int, int], radius: int, opacity: int) -> Image.Image:
    alpha = image.getchannel("A")
    glow_alpha = alpha.filter(ImageFilter.GaussianBlur(radius)).point(lambda value: min(opacity, value))
    glow = Image.new("RGBA", image.size, (*color, 0))
    glow.putalpha(glow_alpha)
    result = Image.new("RGBA", image.size, (0, 0, 0, 0))
    result.alpha_composite(glow)
    result.alpha_composite(image)
    return result


def add_rim(image: Image.Image, color: tuple[int, int, int], width: int) -> Image.Image:
    result = image.copy()
    draw = ImageDraw.Draw(result)
    bbox = image.getchannel("A").getbbox()
    if bbox is None:
        return result
    inset = max(2, width)
    left, top, right, bottom = bbox
    draw.rounded_rectangle(
        (left + inset, top + inset, right - inset - 1, bottom - inset - 1),
        radius=max(12, min(image.size) // 5),
        outline=(*color, 210),
        width=width,
    )
    return result


def nine_slice_resize(image: Image.Image, size: tuple[int, int], border: tuple[int, int, int, int]) -> Image.Image:
    image = image.convert("RGBA")
    src_w, src_h = image.size
    dst_w, dst_h = size
    left, top, right, bottom = border
    if dst_w <= left + right or dst_h <= top + bottom:
        resized = image.copy()
        resized.thumbnail((dst_w, dst_h), Image.Resampling.LANCZOS)
        canvas = Image.new("RGBA", size, (0, 0, 0, 0))
        canvas.alpha_composite(resized, ((dst_w - resized.width) // 2, (dst_h - resized.height) // 2))
        return canvas

    out = Image.new("RGBA", size, (0, 0, 0, 0))
    boxes = {
        "tl": (0, 0, left, top),
        "tr": (src_w - right, 0, src_w, top),
        "bl": (0, src_h - bottom, left, src_h),
        "br": (src_w - right, src_h - bottom, src_w, src_h),
        "top": (left, 0, src_w - right, top),
        "bottom": (left, src_h - bottom, src_w - right, src_h),
        "left": (0, top, left, src_h - bottom),
        "right": (src_w - right, top, src_w, src_h - bottom),
        "center": (left, top, src_w - right, src_h - bottom),
    }
    paste_resized(out, image.crop(boxes["tl"]), (0, 0), (left, top))
    paste_resized(out, image.crop(boxes["tr"]), (dst_w - right, 0), (right, top))
    paste_resized(out, image.crop(boxes["bl"]), (0, dst_h - bottom), (left, bottom))
    paste_resized(out, image.crop(boxes["br"]), (dst_w - right, dst_h - bottom), (right, bottom))
    paste_resized(out, image.crop(boxes["top"]), (left, 0), (dst_w - left - right, top))
    paste_resized(out, image.crop(boxes["bottom"]), (left, dst_h - bottom), (dst_w - left - right, bottom))
    paste_resized(out, image.crop(boxes["left"]), (0, top), (left, dst_h - top - bottom))
    paste_resized(out, image.crop(boxes["right"]), (dst_w - right, top), (right, dst_h - top - bottom))
    paste_resized(out, image.crop(boxes["center"]), (left, top), (dst_w - left - right, dst_h - top - bottom))
    return out


def paste_resized(out: Image.Image, piece: Image.Image, xy: tuple[int, int], size: tuple[int, int]) -> None:
    if size[0] <= 0 or size[1] <= 0:
        return
    resized = piece.resize(size, Image.Resampling.LANCZOS)
    out.alpha_composite(resized, xy)


def build_manifest_rows(generated: list[dict[str, object]]) -> list[dict[str, str]]:
    source_assets = ";".join(to_repo_path(path) for path in (BUTTON_SOURCE, PANEL_SOURCE, ROUTECARD_SOURCE))
    source_sha = ";".join(sha256(path) for path in (BUTTON_SOURCE, PANEL_SOURCE, ROUTECARD_SOURCE))
    rows: list[dict[str, str]] = []
    for item in generated:
        path = Path(item["path"])
        rows.append(
            {
                "asset_id": str(item["asset_id"]),
                "component_id": str(item["component_id"]),
                "variant_id": str(item["variant_id"]),
                "target_size": f"{item['size'][0]}x{item['size'][1]}",
                "batch_slug": BATCH_SLUG,
                "asset_type": "ui_common_state_candidate",
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": image_size(path),
                "source_assets": source_assets,
                "source_sha256": source_sha,
                "contact_sheet": to_repo_path(CONTACT_SHEET_PATH),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "source_model": "derived_from_existing_ui_assets_not_image2",
                "recommendation": "candidate_review_only_do_not_import",
                "visual_review": str(item["visual_review"]),
            }
        )
    return rows


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_contact_sheet(generated: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1800, 1320), (32, 30, 43, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(18)
    small_font = load_font(14)
    draw.text((36, 28), "P0 Batch 82 - Common UI State Candidates", fill=(245, 238, 220), font=title_font)
    draw.text((36, 78), "Derived from existing UI button/panel/card art. Candidate-only, textless, no Unity import.", fill=(238, 193, 106), font=small_font)

    positions = layout_positions(generated)
    for item, (x, y) in zip(generated, positions):
        sprite = Image.open(item["path"]).convert("RGBA")
        panel = checkerboard(sprite.size)
        panel.alpha_composite(sprite)
        canvas.alpha_composite(panel, (x, y))
        draw.rectangle((x, y, x + sprite.width, y + sprite.height), outline=(126, 136, 172, 255), width=2)
        draw.text((x, y + sprite.height + 8), str(item["variant_id"]).replace("_", " "), fill=(211, 224, 255), font=label_font)
        draw.text((x, y + sprite.height + 32), f"{sprite.width}x{sprite.height}", fill=(245, 238, 220), font=small_font)

    canvas.save(CONTACT_SHEET_PATH)


def write_review_sheet(generated: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1800, 1220), (26, 28, 38, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(18)
    small_font = load_font(14)
    draw.text((36, 28), "Batch 82 Review - Common UI Candidate Coverage", fill=(245, 238, 220), font=title_font)
    draw.text((36, 76), "Fills prior missing rows: buttons, modal/dialog, tabs/segmented controls, list rows.", fill=(238, 193, 106), font=small_font)

    groups = (
        ("button_state_atlas", 44, 128),
        ("modal_dialog_frame", 44, 660),
        ("tabs_segmented_controls", 930, 128),
        ("list_row_frame", 930, 660),
    )
    by_component: dict[str, list[dict[str, object]]] = {}
    for item in generated:
        by_component.setdefault(str(item["component_id"]), []).append(item)

    for component, x, y in groups:
        draw.text((x, y - 34), component, fill=(211, 224, 255), font=label_font)
        cursor_x = x
        cursor_y = y
        for item in by_component[component]:
            sprite = Image.open(item["path"]).convert("RGBA")
            preview = sprite.copy()
            max_w, max_h = (220, 96) if component != "modal_dialog_frame" else (260, 146)
            preview.thumbnail((max_w, max_h), Image.Resampling.LANCZOS)
            panel = checkerboard((max_w + 16, max_h + 16), square=8)
            panel.alpha_composite(preview, ((panel.width - preview.width) // 2, (panel.height - preview.height) // 2))
            canvas.alpha_composite(panel, (cursor_x, cursor_y))
            draw.text((cursor_x, cursor_y + panel.height + 4), str(item["variant_id"]).replace("_", " "), fill=(245, 238, 220), font=small_font)
            cursor_x += panel.width + 22
            if cursor_x > x + 760:
                cursor_x = x
                cursor_y += panel.height + 40

    draw.text((44, 1088), "Review gates: no baked Chinese text, no character/body art, candidate-only, Unity screenshots and click-target proof still required.", fill=(245, 238, 220), font=small_font)
    canvas.save(REVIEW_SHEET_PATH)


def layout_positions(generated: list[dict[str, object]]) -> list[tuple[int, int]]:
    positions: list[tuple[int, int]] = []
    cursor_x = 44
    cursor_y = 132
    row_h = 0
    for item in generated:
        width, height = item["size"]
        if cursor_x + width > 1740:
            cursor_x = 44
            cursor_y += row_h + 74
            row_h = 0
        positions.append((cursor_x, cursor_y))
        cursor_x += width + 36
        row_h = max(row_h, height)
    return positions


def write_review_note(rows: list[dict[str, str]]) -> None:
    component_counts: dict[str, int] = {}
    for row in rows:
        component_counts[row["component_id"]] = component_counts.get(row["component_id"], 0) + 1
    lines = [
        "# Batch 82 Common UI State Candidate Review",
        "",
        "Status: candidate review only; do not import into Unity yet.",
        "",
        "This batch fills the four previously missing common UI component rows from the UI common component inventory.",
        "",
        "| Component | Candidate sprites |",
        "| --- | ---: |",
    ]
    for component, count in sorted(component_counts.items()):
        lines.append(f"| `{component}` | {count} |")
    lines.extend(
        [
            "",
            "## Review Notes",
            "",
            "- Sprites are textless so Unity can render localized Chinese text.",
            "- Source art is existing project UI button, panel, and route-card framing; no AI/image2 generation happened in this derivative pass.",
            "- No starter-cat body, character frame, portrait, or runtime replacement art is included.",
            "- Keep all files under `design/development/asset_candidates/...` until Unity import settings, screenshots, click-target proof, and Console checks pass.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 82 Process Note",
                "",
                "Generation method: deterministic derivative sprites from existing local UI art.",
                "",
                "Source assets:",
                f"- `{to_repo_path(BUTTON_SOURCE)}`",
                f"- `{to_repo_path(PANEL_SOURCE)}`",
                f"- `{to_repo_path(ROUTECARD_SOURCE)}`",
                "",
                "Why no image2 in this pass:",
                "- Strict CLI `gpt-image-2` generation requires `OPENAI_API_KEY`, which is not set in this shell.",
                "- Built-in `image_gen` does not expose a model selector, so using it here would not satisfy the explicit image2 provenance requirement.",
                "- These missing rows are stateful UI frames that can be safely derived from the existing style assets without adding new art-language drift.",
                "",
                "Output:",
                f"- {len(rows)} transparent PNG candidate sprites.",
                f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
                f"- Contact sheet: `{to_repo_path(CONTACT_SHEET_PATH)}`",
                f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
                "",
                "Unity gate:",
                "- Candidate-only until screen-level UI priority, import settings, click-target/readability screenshots, binding proof, and Console checks pass.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_spec_note() -> None:
    SPEC_PATH.parent.mkdir(parents=True, exist_ok=True)
    SPEC_PATH.write_text(
        "\n".join(
            [
                "# P0 Asset Batch 82 - Common UI State Derivative Candidates",
                "",
                "Purpose: fill missing textless common UI component rows without using character art or broad AI generation.",
                "",
                "Rows:",
                "- button state atlas",
                "- modal/dialog frame",
                "- tabs and segmented controls",
                "- compact list-row frame",
                "",
                "Source truth:",
                "- Qr1 UI/style source truth",
                "- existing local UI button, dreamglass panel, and route-card frame assets",
                "",
                "Constraints:",
                "- no baked Chinese text",
                "- no cat body, no character portrait, no runtime replacement art",
                "- candidate-only output under `design/development/asset_candidates/...`",
                "- do not claim image2 provenance for this deterministic derivative pass",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def checkerboard(size: tuple[int, int], square: int = 12) -> Image.Image:
    w, h = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    colors = ((44, 48, 62, 255), (66, 70, 86, 255))
    for y in range(0, h, square):
        for x in range(0, w, square):
            draw.rectangle((x, y, x + square - 1, y + square - 1), fill=colors[((x // square) + (y // square)) % 2])
    return image


def load_font(size: int) -> ImageFont.ImageFont:
    for name in ("arial.ttf", "segoeui.ttf", "DejaVuSans.ttf"):
        try:
            return ImageFont.truetype(name, size)
        except OSError:
            continue
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
