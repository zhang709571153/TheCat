from __future__ import annotations

import csv
import hashlib
import shutil
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_90_cat_room_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "cat_room" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_process_note.md"
AGENT_PROMPT_PATH = CANDIDATE_DIR / "thecat_ui_cat_room_batch90_agent_review_prompt.md"

SOURCE_PATHS = [
    REPO_ROOT / "Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_feeder_sprite_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_litterbox_sprite_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_poop_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_ready_ring_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_restore_pulse_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_feeder_ready_marker_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_litter_urgent_marker_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_blocked_marker_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_selected_640x80_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_disabled_640x80_candidate_v001.png",
]

SPRITE_SPECS = (
    ("cat_room_stage", "cat_room_stage_panel_frame", (1180, 640), "main cat-room stage panel"),
    ("cat_room_status", "cat_room_status_rail_frame", (960, 120), "sleep hunger poop status rail"),
    ("cat_room_interaction", "cat_room_interaction_card_slot", (440, 180), "interaction prompt card slot"),
    ("cat_room_hotspot", "cat_room_prop_hotspot_frame", (256, 256), "prop hotspot frame"),
    ("cat_room_action", "cat_room_dream_entrance_button_frame", (420, 112), "dream entrance button frame"),
    ("cat_room_prompt", "cat_room_hover_disabled_prompt_chip", (360, 96), "hover and disabled prompt chip"),
)

MOCKUP_SPECS = (
    ("cat_room_screen", "cat_room_1920x1080", (1920, 1080), "main"),
    ("cat_room_screen", "cat_room_bed_hover_1365x768", (1365, 768), "bed_hover"),
    ("cat_room_screen", "cat_room_interaction_disabled_1280x720", (1280, 720), "disabled"),
    ("cat_room_screen", "cat_room_compact_1024x768", (1024, 768), "compact"),
)

FIELD_NAMES = [
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
    "review_note_sha256",
    "process_note",
    "process_note_sha256",
    "agent_prompt",
    "agent_prompt_sha256",
    "source_model",
    "recommendation",
    "visual_review",
]

SOURCE_MODEL = "deterministic_local_derivative_from_existing_bedroom_map_and_interaction_assets_not_image2"
RECOMMENDATION = "candidate_only_pending_unity_cat_room_screenshots"


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    SPRITE_DIR.mkdir(parents=True, exist_ok=True)
    MOCKUP_DIR.mkdir(parents=True, exist_ok=True)
    clean_previous_outputs()

    sources = load_sources()
    sprites: dict[str, Image.Image] = {}
    generated: list[dict[str, object]] = []

    for component_id, variant_id, size, review in SPRITE_SPECS:
        image = build_sprite(variant_id, size)
        path = SPRITE_DIR / f"thecat_ui_cat_room_{variant_id}_{size[0]}x{size[1]}_candidate_v001.png"
        image.save(path)
        sprites[variant_id] = image
        generated.append(record(component_id, variant_id, size, "sprite", path, review))

    for component_id, variant_id, size, scenario in MOCKUP_SPECS:
        image = build_mockup(sources, sprites, size, scenario)
        path = MOCKUP_DIR / f"thecat_ui_cat_room_{variant_id}_local_mockup_v001.png"
        image.save(path)
        generated.append(record(component_id, variant_id, size, "local_mockup", path, f"{variant_id} local mockup"))

    build_contact_sheet(generated).save(CONTACT_SHEET_PATH)
    build_review_sheet(generated).save(REVIEW_SHEET_PATH)
    write_review_note(generated)
    write_process_note()
    write_agent_prompt()
    write_manifest(generated)
    print(f"Wrote {len(generated)} Batch 90 cat-room preflight row(s).")
    print(to_repo_path(MANIFEST_PATH))


def clean_previous_outputs() -> None:
    for child in (SPRITE_DIR, MOCKUP_DIR):
        if child.exists():
            shutil.rmtree(child)
        child.mkdir(parents=True, exist_ok=True)
    for path in (MANIFEST_PATH, CONTACT_SHEET_PATH, REVIEW_SHEET_PATH, REVIEW_NOTE_PATH, PROCESS_NOTE_PATH, AGENT_PROMPT_PATH):
        if path.exists():
            path.unlink()


def load_sources() -> dict[str, Image.Image]:
    keys = {
        "bedroom": SOURCE_PATHS[0],
        "bed": SOURCE_PATHS[1],
        "feeder": SOURCE_PATHS[2],
        "litter": SOURCE_PATHS[3],
        "dream_bg": SOURCE_PATHS[4],
        "panel": SOURCE_PATHS[5],
        "button": SOURCE_PATHS[6],
        "logo": SOURCE_PATHS[7],
        "sleep_icon": SOURCE_PATHS[8],
        "hunger_icon": SOURCE_PATHS[9],
        "poop_icon": SOURCE_PATHS[10],
        "range_ripple": SOURCE_PATHS[11],
        "bed_ready": SOURCE_PATHS[12],
        "bed_restore": SOURCE_PATHS[13],
        "feeder_ready": SOURCE_PATHS[14],
        "litter_urgent": SOURCE_PATHS[15],
        "blocked": SOURCE_PATHS[16],
        "row_selected": SOURCE_PATHS[17],
        "row_disabled": SOURCE_PATHS[18],
    }
    return {key: Image.open(path).convert("RGBA") for key, path in keys.items()}


def build_sprite(variant_id: str, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    w, h = size
    cyan = (112, 214, 240, 214)
    blue = (24, 40, 70, 205)
    gold = (222, 184, 91, 226)
    soft = (138, 178, 204, 126)
    disabled = (108, 122, 146, 145)
    warning = (212, 84, 128, 205)

    if variant_id == "cat_room_stage_panel_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 32, blue, cyan, 4)
        rounded_frame(draw, 40, 40, w - 40, h - 46, 30, (16, 28, 54, 128), gold, 2)
        for index in range(3):
            x0 = 130 + index * 310
            rounded_frame(draw, x0, 420, x0 + 220, h - 86, 22, (34, 50, 78, 166), soft, 2)
    elif variant_id == "cat_room_status_rail_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 28, (28, 44, 74, 210), cyan, 4)
        for index in range(3):
            x0 = 42 + index * 298
            rounded_frame(draw, x0, 32, x0 + 250, h - 32, 18, (36, 52, 82, 214), gold if index == 0 else soft, 2)
            draw.line((x0 + 74, h // 2, x0 + 216, h // 2), fill=(218, 226, 216, 145), width=7)
    elif variant_id == "cat_room_interaction_card_slot":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 24, (30, 46, 76, 214), cyan, 4)
        rounded_frame(draw, 28, 28, 132, h - 28, 20, (38, 56, 86, 210), gold, 2)
        draw.line((160, 48, w - 44, 48), fill=(224, 216, 196, 150), width=7)
        draw.line((160, 84, w - 86, 84), fill=(126, 199, 225, 130), width=5)
        draw.line((160, 126, w - 72, 126), fill=soft, width=4)
    elif variant_id == "cat_room_prop_hotspot_frame":
        rounded_frame(draw, 16, 16, w - 16, h - 16, 52, (26, 42, 72, 120), cyan, 4)
        draw.ellipse((44, 44, w - 44, h - 44), outline=gold, width=5)
        draw.ellipse((72, 72, w - 72, h - 72), outline=(112, 214, 240, 130), width=4)
        draw.arc((36, 36, w - 36, h - 36), 210, 330, fill=(232, 216, 154, 170), width=8)
    elif variant_id == "cat_room_dream_entrance_button_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 28, (32, 45, 78, 224), gold, 4)
        rounded_frame(draw, 28, 26, w - 28, h - 26, 22, (23, 38, 68, 222), cyan, 2)
        draw.arc((w - 110, 28, w - 40, h - 28), 110, 430, fill=(232, 216, 154, 200), width=6)
        draw.line((54, h // 2, w - 142, h // 2), fill=(228, 228, 210, 145), width=8)
    elif variant_id == "cat_room_hover_disabled_prompt_chip":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (28, 44, 74, 205), cyan, 3)
        rounded_frame(draw, 28, 24, 156, h - 24, 18, (38, 54, 84, 218), gold, 2)
        rounded_frame(draw, 178, 24, w - 28, h - 24, 18, (38, 48, 66, 174), disabled, 2)
        draw.line((52, h // 2, 128, h // 2), fill=(228, 222, 196, 150), width=5)
        draw.line((202, h // 2, w - 54, h // 2), fill=warning, width=5)
    else:
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, blue, cyan, 4)

    return image


def build_mockup(sources: dict[str, Image.Image], sprites: dict[str, Image.Image], size: tuple[int, int], scenario: str) -> Image.Image:
    w, h = size
    image = fit_image(sources["bedroom"], size)
    image.alpha_composite(Image.new("RGBA", size, (7, 10, 24, 50 if scenario == "main" else 72)))
    scale = min(w / 1920, h / 1080)
    margin = max(18, int(30 * scale))

    logo = fit_image_contained(sources["logo"], (min(300, int(w * 0.22)), max(82, int(150 * max(0.52, scale)))))
    image.alpha_composite(logo, (margin, margin))

    status_w = min(int(w * 0.58), 960)
    status_h = max(78, int(120 * max(0.64, scale)))
    status_x = w - status_w - margin
    status_y = margin
    image.alpha_composite(fit_image(sprites["cat_room_status_rail_frame"], (status_w, status_h)), (status_x, status_y))
    draw_status_icons(image, sources, (status_x, status_y), status_w, status_h)

    stage_w = min(w - margin * 2, max(int(w * 0.78), 780 if w > 1100 else 650))
    stage_h = min(h - margin * 3 - status_h, max(int(h * 0.62), 430 if h > 760 else 380))
    stage_x = (w - stage_w) // 2
    stage_y = max(margin + status_h + 20, int(h * 0.18))
    image.alpha_composite(fit_image(sprites["cat_room_stage_panel_frame"], (stage_w, stage_h)), (stage_x, stage_y))

    draw_room_props(image, sources, sprites, (stage_x, stage_y), (stage_w, stage_h), scenario)

    card_w = min(440, int(stage_w * 0.36))
    card_h = max(118, int(180 * max(0.66, scale)))
    card_x = stage_x + max(24, int(stage_w * 0.05))
    card_y = stage_y + stage_h - card_h - max(20, int(28 * scale))
    image.alpha_composite(fit_image(sprites["cat_room_interaction_card_slot"], (card_w, card_h)), (card_x, card_y))
    draw_interaction_card_content(image, sources, (card_x, card_y), (card_w, card_h), scenario)

    chip_w = min(360, int(stage_w * 0.3))
    chip_h = max(62, int(96 * max(0.62, scale)))
    image.alpha_composite(fit_image(sprites["cat_room_hover_disabled_prompt_chip"], (chip_w, chip_h)), (stage_x + stage_w - chip_w - 28, card_y))

    button_w = min(420, int(stage_w * 0.32))
    button_h = max(72, int(112 * max(0.64, scale)))
    image.alpha_composite(
        fit_image(sprites["cat_room_dream_entrance_button_frame"], (button_w, button_h)),
        (stage_x + stage_w - button_w - 28, stage_y + 34),
    )

    return flatten_opaque(image, size)


def draw_status_icons(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], rail_w: int, rail_h: int) -> None:
    icons = ("sleep_icon", "hunger_icon", "poop_icon")
    icon_size = max(34, min(56, int(rail_h * 0.5)))
    slot_w = rail_w // 3
    draw = ImageDraw.Draw(image)
    for index, key in enumerate(icons):
        x = pos[0] + index * slot_w + max(26, int(slot_w * 0.1))
        y = pos[1] + (rail_h - icon_size) // 2
        image.alpha_composite(fit_image(sources[key], (icon_size, icon_size)), (x, y))
        draw.rounded_rectangle((x + icon_size + 12, y + icon_size // 2 - 5, pos[0] + (index + 1) * slot_w - 34, y + icon_size // 2 + 7), radius=6, fill=(220, 226, 218, 135))


def draw_room_props(image: Image.Image, sources: dict[str, Image.Image], sprites: dict[str, Image.Image], pos: tuple[int, int], size: tuple[int, int], scenario: str) -> None:
    x, y = pos
    w, h = size
    prop_specs = [
        ("bed", "bed_ready" if scenario != "disabled" else "blocked", int(w * 0.19), int(h * 0.42), int(w * 0.24)),
        ("feeder", "feeder_ready" if scenario in ("main", "compact") else "blocked", int(w * 0.56), int(h * 0.56), int(w * 0.12)),
        ("litter", "litter_urgent" if scenario != "bed_hover" else "blocked", int(w * 0.73), int(h * 0.58), int(w * 0.12)),
    ]
    hotspot = fit_image(sprites["cat_room_prop_hotspot_frame"], (max(92, int(w * 0.12)), max(92, int(w * 0.12))))
    for prop_key, marker_key, cx, cy, prop_width in prop_specs:
        prop = fit_image_contained(sources[prop_key], (max(72, prop_width), max(72, int(prop_width * 0.82))))
        px = x + cx - prop.width // 2
        py = y + cy - prop.height // 2
        if scenario == "bed_hover" and prop_key == "bed":
            ripple = fit_image_contained(sources["range_ripple"], (int(w * 0.28), int(w * 0.28)))
            image.alpha_composite(ripple, (x + cx - ripple.width // 2, y + cy - ripple.height // 2))
        image.alpha_composite(hotspot, (x + cx - hotspot.width // 2, y + cy - hotspot.height // 2))
        image.alpha_composite(prop, (px, py))
        marker = fit_image_contained(sources[marker_key], (max(46, int(w * 0.07)), max(46, int(w * 0.07))))
        image.alpha_composite(marker, (x + cx + int(prop_width * 0.18), y + cy - int(prop_width * 0.34)))


def draw_interaction_card_content(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], size: tuple[int, int], scenario: str) -> None:
    x, y = pos
    w, h = size
    icon_key = "sleep_icon" if scenario == "bed_hover" else "hunger_icon" if scenario == "main" else "poop_icon"
    icon = fit_image_contained(sources[icon_key], (max(54, int(h * 0.46)), max(54, int(h * 0.46))))
    image.alpha_composite(icon, (x + 38, y + (h - icon.height) // 2))
    draw = ImageDraw.Draw(image)
    for row in range(3):
        yy = y + int(h * 0.28) + row * max(20, h // 6)
        draw.rounded_rectangle((x + int(w * 0.34), yy, x + w - 42 - row * 28, yy + max(7, h // 18)), radius=5, fill=(220, 224, 212, 120))


def build_contact_sheet(generated: list[dict[str, object]]) -> Image.Image:
    thumb_w, thumb_h = 280, 168
    cols = 2
    rows = (len(generated) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * thumb_w, rows * thumb_h + 60), (13, 17, 31, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((24, 18), "Batch 90 Cat Room Preflight", fill=(236, 206, 116, 255), font=font(25))
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        thumb = fit_image_contained(Image.open(path).convert("RGBA"), (thumb_w - 24, thumb_h - 48))
        x = (index % cols) * thumb_w + 12
        y = (index // cols) * thumb_h + 64
        sheet.alpha_composite(thumb, (x + (thumb_w - 24 - thumb.width) // 2, y + 4))
        draw.text((x, y + thumb_h - 34), str(item["variant_id"]), fill=(224, 224, 214, 255), font=font(13))
        draw.text((x, y + thumb_h - 18), f"{item['size'][0]}x{item['size'][1]}", fill=(148, 184, 206, 255), font=font(12))
    return sheet


def build_review_sheet(generated: list[dict[str, object]]) -> Image.Image:
    sheet = Image.new("RGBA", (1440, 1680), (10, 14, 28, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((40, 28), "Batch 90 Cat Room Preflight - Candidate Only", fill=(224, 224, 214, 255), font=font(32))
    draw.text((40, 70), "Textless room UI using BedroomDream props and Batch 67 affordances; not image2 provenance.", fill=(154, 210, 230, 255), font=font(19))
    card_w, card_h = 660, 282
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        col = index % 2
        row = index // 2
        x = 40 + col * (card_w + 40)
        y = 122 + row * (card_h + 22)
        rounded_frame(draw, x, y, x + card_w, y + card_h, 14, (28, 42, 70, 255), (112, 214, 240, 180), 2)
        preview = fit_image_contained(Image.open(path).convert("RGBA"), (card_w - 32, card_h - 80))
        sheet.alpha_composite(preview, (x + (card_w - preview.width) // 2, y + 16))
        draw.text((x + 18, y + card_h - 56), str(item["variant_id"]), fill=(232, 226, 210, 255), font=font(20))
        draw.text((x + 18, y + card_h - 30), f"{item['asset_type']} | {item['size'][0]}x{item['size'][1]}", fill=(164, 202, 222, 255), font=font(15))
    return sheet


def record(component_id: str, variant_id: str, size: tuple[int, int], asset_type: str, path: Path, review: str) -> dict[str, object]:
    return {
        "component_id": component_id,
        "variant_id": variant_id,
        "size": size,
        "asset_type": asset_type,
        "path": path,
        "visual_review": review,
    }


def write_manifest(generated: list[dict[str, object]]) -> None:
    contact_hash = sha256(CONTACT_SHEET_PATH)
    review_hash = sha256(REVIEW_SHEET_PATH)
    review_note_hash = sha256(REVIEW_NOTE_PATH)
    process_note_hash = sha256(PROCESS_NOTE_PATH)
    agent_prompt_hash = sha256(AGENT_PROMPT_PATH)
    source_asset_string = ";".join(to_repo_path(path) for path in SOURCE_PATHS)
    source_hash_string = ";".join(sha256(path) for path in SOURCE_PATHS)
    rows: list[dict[str, str]] = []
    for item in generated:
        path = item["path"]
        size = item["size"]
        assert isinstance(path, Path)
        assert isinstance(size, tuple)
        rows.append(
            {
                "asset_id": path.stem,
                "component_id": str(item["component_id"]),
                "variant_id": str(item["variant_id"]),
                "target_size": f"{size[0]}x{size[1]}",
                "batch_slug": BATCH_SLUG,
                "asset_type": str(item["asset_type"]),
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": image_size(path),
                "source_assets": source_asset_string,
                "source_sha256": source_hash_string,
                "contact_sheet": to_repo_path(CONTACT_SHEET_PATH),
                "contact_sheet_sha256": contact_hash,
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_sheet_sha256": review_hash,
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "review_note_sha256": review_note_hash,
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "process_note_sha256": process_note_hash,
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "agent_prompt_sha256": agent_prompt_hash,
                "source_model": SOURCE_MODEL,
                "recommendation": RECOMMENDATION,
                "visual_review": str(item["visual_review"]),
            }
        )
    with MANIFEST_PATH.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_note(generated: list[dict[str, object]]) -> None:
    lines = [
        "# Batch 90 Cat Room Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers cat room screen composition, status rail, prop hotspots, interaction card, hover/disabled chip, and dream entrance button.",
        "- Reuses existing BedroomDream background and props, Batch 67 interaction affordances, Qr1-style UI shell, and core status icons.",
        "- Does not generate, crop, recolor, or import starter-cat body art or new cat room character poses.",
        "- Does not bake Chinese text into sprites; Unity-rendered interaction labels, status values, and dream entrance labels remain required.",
        "",
        "## Candidate Rows",
        "",
        "| Variant | Type | Size | Path |",
        "| --- | --- | --- | --- |",
    ]
    for item in generated:
        size = item["size"]
        path = item["path"]
        assert isinstance(size, tuple)
        assert isinstance(path, Path)
        lines.append(f"| `{item['variant_id']}` | `{item['asset_type']}` | `{size[0]}x{size[1]}` | `{to_repo_path(path)}` |")
    lines.extend(
        [
            "",
            "## Required Unity Gates",
            "",
            "- Cat room screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.",
            "- Unity-rendered Chinese interaction text, status values, prompts, and dream entrance labels.",
            "- Bed, feeder, litter, dream entrance, hover, disabled, blocked, and range states must be distinct.",
            "- Prop scale must remain consistent with BedroomDream and Batch 54/67 bedroom source references.",
            "- Click-target proof for bed, feeder, litter, dream entrance, and close/back affordances.",
            "- Sprite import settings, screen binding proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note() -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 90 Cat Room Process Note",
                "",
                "- Lane: `ui_cat_room` / screen-level preflight.",
                "- UI source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c` (`Qr1`) live section 9 fetch passed in this shell.",
                "- Character identity source truth: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf` (`IAd`) is ACL-blocked for live fetch, so this packet avoids new cat body art.",
                "- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.",
                "- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.",
                "- Built-in `image_gen` does not expose an explicit `image2` model selector here, so this batch avoids model-claimed generation and uses deterministic local derivation instead.",
                "- Feishu ACL note: `MDr`, `IAd`, `IZp`, `HDo`, and the `FoW9` Drive folder remain live-read/list blocked for this CLI user; do not claim current-live coverage for those sources.",
                "- Source packs reused: BedroomDream background and props, Batch 67 bedroom interaction affordances, Batch 82 common UI states, Qr1-style UI shell, and core sleep/hunger/poop icons.",
                "- Candidate-only boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.",
                "- Text rule: no baked Chinese text; Unity-rendered interaction labels, status values, prompts, and dream entrance labels remain required.",
                "- Cat-body rule: no new cat body art, pose, costume, color, portrait, or framesheet generation is included.",
                "- Runtime gate: Unity-rendered cat-room screenshots, bed/feeder/litter/dream entrance interaction proof, hover/disabled/range state proof, click targets, import settings, binding, and Console.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_agent_prompt() -> None:
    AGENT_PROMPT_PATH.write_text(
        "\n".join(
            [
                "# Agent Prompt - Batch 90 Cat Room Preflight",
                "",
                "Review this candidate-only cat room screen packet before Unity import.",
                "",
                "Check visual consistency with Qr1 UI/style, BedroomDream background and props, Batch 67 bedroom interaction affordances, Batch 82 common UI states, and core sleep/hunger/poop icons.",
                "Confirm there is no baked Chinese text, no new cat body art, no `Assets` writes, and no Unity `.meta` files.",
                "Pay special attention to 1024x768 crowding, bed/feeder/litter/dream entrance click-target space, hover/disabled/range semantics, and whether dream entrance could be confused with battle or route entry.",
                "",
                "Return `PASS`, `PASS_WITH_P2`, or `FAIL`, with file paths and concrete findings.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def rounded_frame(draw: ImageDraw.ImageDraw, x0: int, y0: int, x1: int, y1: int, radius: int, fill: tuple[int, int, int, int], outline: tuple[int, int, int, int], width: int) -> None:
    draw.rounded_rectangle((x0, y0, x1, y1), radius=radius, fill=fill, outline=outline, width=width)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    return image.resize(size, Image.Resampling.LANCZOS)


def fit_image_contained(image: Image.Image, box: tuple[int, int]) -> Image.Image:
    image = image.copy()
    image.thumbnail(box, Image.Resampling.LANCZOS)
    return image


def flatten_opaque(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    flattened = Image.new("RGBA", size, (10, 14, 28, 255))
    flattened.alpha_composite(image)
    return flattened


def font(size: int) -> ImageFont.ImageFont:
    for path in ("C:/Windows/Fonts/arial.ttf", "C:/Windows/Fonts/segoeui.ttf"):
        if Path(path).exists():
            return ImageFont.truetype(path, size)
    return ImageFont.load_default()


def image_size(path: Path) -> str:
    image = Image.open(path)
    try:
        return f"{image.width}x{image.height}"
    finally:
        image.close()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(65536), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT.resolve()).as_posix()


if __name__ == "__main__":
    main()
