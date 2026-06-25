from __future__ import annotations

import csv
import hashlib
import shutil
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_89_skill_selection_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "skill_selection" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_process_note.md"
AGENT_PROMPT_PATH = CANDIDATE_DIR / "thecat_ui_skill_selection_batch89_agent_review_prompt.md"

SOURCE_PATHS = [
    REPO_ROOT / "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_128/thecat_ui_skill_slot_square_selected_128_candidate_v002_light.png",
    REPO_ROOT / "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_128/thecat_ui_skill_slot_square_ready_128_candidate_v002_light.png",
    REPO_ROOT / "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_128/thecat_ui_skill_slot_square_disabled_128_candidate_v002_light.png",
    REPO_ROOT / "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/frames_square_v002_light_128/thecat_ui_skill_slot_square_cooldown_128_candidate_v002_light.png",
    REPO_ROOT / "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_selected_640x80_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_default_640x80_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_disabled_640x80_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_lock_64_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_warning_64_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_sun_charge_burst_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_shield_counter_impact_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_nephthys_obelisk_turret_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_nephthys_sandstorm_swirl_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_suzune_healing_bell_pulse_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_suzune_torii_gate_ward_64_recommended_candidate_v001.png",
]

SPRITE_SPECS = (
    ("skill_selection_panel", "skill_selection_panel_frame", (1180, 640), "main skill-selection frame"),
    ("skill_selection_card", "skill_choice_card_selected", (420, 240), "selected skill choice card"),
    ("skill_selection_card", "skill_choice_card_ready", (420, 240), "ready skill choice card"),
    ("skill_selection_card", "skill_choice_card_disabled", (420, 240), "disabled skill choice card"),
    ("skill_selection_card", "skill_choice_card_locked", (420, 240), "locked skill choice card"),
    ("skill_selection_detail", "skill_detail_panel_frame", (760, 320), "skill detail panel"),
    ("skill_selection_strip", "skill_cost_cooldown_strip", (420, 96), "cost and cooldown strip"),
    ("skill_selection_action", "skill_confirm_button_frame", (420, 112), "confirm button frame"),
)

MOCKUP_SPECS = (
    ("skill_selection_screen", "skill_selection_1920x1080", (1920, 1080), "main"),
    ("skill_selection_screen", "skill_selection_1365x768", (1365, 768), "laptop"),
    ("skill_selection_screen", "skill_selection_1280x720", (1280, 720), "compact"),
    ("skill_selection_screen", "skill_selection_1024x768", (1024, 768), "dense"),
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

SOURCE_MODEL = "deterministic_local_derivative_from_batch80_batch81_batch82_skill_ui_assets_not_image2"
RECOMMENDATION = "candidate_only_pending_unity_skill_selection_screenshots"


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
        path = SPRITE_DIR / f"thecat_ui_skill_selection_{variant_id}_{size[0]}x{size[1]}_candidate_v001.png"
        image.save(path)
        sprites[variant_id] = image
        generated.append(record(component_id, variant_id, size, "sprite", path, review))

    for component_id, variant_id, size, scenario in MOCKUP_SPECS:
        image = build_mockup(sources, sprites, size, scenario)
        path = MOCKUP_DIR / f"thecat_ui_skill_selection_{variant_id}_local_mockup_v001.png"
        image.save(path)
        generated.append(record(component_id, variant_id, size, "local_mockup", path, f"{variant_id} local mockup"))

    build_contact_sheet(generated).save(CONTACT_SHEET_PATH)
    build_review_sheet(generated).save(REVIEW_SHEET_PATH)
    write_review_note(generated)
    write_process_note()
    write_agent_prompt()
    write_manifest(generated)
    print(f"Wrote {len(generated)} Batch 89 skill-selection preflight row(s).")
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
        "background": SOURCE_PATHS[0],
        "panel": SOURCE_PATHS[1],
        "button": SOURCE_PATHS[2],
        "logo": SOURCE_PATHS[3],
        "cost_chip": SOURCE_PATHS[4],
        "cooldown": SOURCE_PATHS[5],
        "no_target": SOURCE_PATHS[6],
        "slot_selected": SOURCE_PATHS[7],
        "slot_ready": SOURCE_PATHS[8],
        "slot_disabled": SOURCE_PATHS[9],
        "slot_cooldown": SOURCE_PATHS[10],
        "row_selected": SOURCE_PATHS[11],
        "row_default": SOURCE_PATHS[12],
        "row_disabled": SOURCE_PATHS[13],
        "lock": SOURCE_PATHS[14],
        "warning": SOURCE_PATHS[15],
        "skill_1": SOURCE_PATHS[16],
        "skill_2": SOURCE_PATHS[17],
        "skill_3": SOURCE_PATHS[18],
        "skill_4": SOURCE_PATHS[19],
        "skill_5": SOURCE_PATHS[20],
        "skill_6": SOURCE_PATHS[21],
    }
    return {key: Image.open(path).convert("RGBA") for key, path in keys.items()}


def build_sprite(variant_id: str, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    w, h = size
    cyan = (112, 214, 240, 214)
    blue = (24, 40, 70, 210)
    gold = (222, 184, 91, 226)
    soft = (138, 178, 204, 126)
    muted = (82, 98, 126, 150)
    warning = (212, 84, 128, 205)

    if variant_id == "skill_selection_panel_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 32, blue, cyan, 4)
        rounded_frame(draw, 42, 42, w - 42, 126, 22, (32, 48, 80, 186), gold, 2)
        rounded_frame(draw, 58, 160, w - 58, h - 54, 28, (20, 32, 60, 146), soft, 2)
        for index in range(3):
            x0 = 86 + index * ((w - 172) // 3)
            rounded_frame(draw, x0, 188, x0 + 300, 400, 24, (31, 48, 78, 165), gold if index == 0 else soft, 2)
    elif variant_id.startswith("skill_choice_card"):
        state = variant_id.rsplit("_", 1)[-1]
        outline = {"selected": gold, "ready": cyan, "disabled": muted, "locked": warning}[state]
        fill = (30, 46, 76, 215) if state != "disabled" else (30, 38, 54, 156)
        rounded_frame(draw, 8, 8, w - 8, h - 8, 24, fill, outline, 4)
        draw.ellipse((30, 34, 126, 130), outline=outline, width=4)
        draw.line((150, 54, w - 40, 54), fill=(224, 216, 196, 145 if state != "disabled" else 75), width=7)
        draw.line((150, 90, w - 76, 90), fill=(126, 199, 225, 130 if state != "disabled" else 65), width=5)
        for index in range(3):
            x0 = 150 + index * 74
            rounded_frame(draw, x0, h - 68, x0 + 56, h - 26, 12, (39, 55, 86, 205), outline, 2)
        if state == "locked":
            draw.line((42, 180, w - 42, 46), fill=(224, 116, 154, 170), width=8)
    elif variant_id == "skill_detail_panel_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 26, (26, 42, 72, 214), cyan, 4)
        rounded_frame(draw, 36, 34, 180, 178, 24, (34, 52, 82, 210), gold, 2)
        for row in range(4):
            y = 48 + row * 42
            draw.rounded_rectangle((218, y, w - 54 - row * 28, y + 14), radius=7, fill=(204, 222, 224, 98))
        rounded_frame(draw, 218, h - 82, w - 56, h - 34, 18, (37, 54, 86, 190), soft, 2)
    elif variant_id == "skill_cost_cooldown_strip":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (28, 44, 74, 205), cyan, 3)
        for index in range(3):
            x0 = 34 + index * 126
            rounded_frame(draw, x0, 24, x0 + 92, h - 24, 16, (38, 54, 84, 218), gold if index == 0 else soft, 2)
            draw.ellipse((x0 + 12, 34, x0 + 42, 64), outline=(238, 216, 154, 180), width=3)
            draw.line((x0 + 52, 49, x0 + 76, 49), fill=(220, 224, 212, 150), width=5)
    elif variant_id == "skill_confirm_button_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 28, (32, 45, 78, 224), gold, 4)
        rounded_frame(draw, 28, 26, w - 28, h - 26, 22, (23, 38, 68, 222), cyan, 2)
        draw.line((54, h // 2, w - 128, h // 2), fill=(228, 228, 210, 150), width=8)
        draw.polygon(((w - 72, h // 2), (w - 112, h // 2 - 23), (w - 112, h // 2 + 23)), fill=(238, 215, 142, 210))
    else:
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, blue, cyan, 4)

    return image


def build_mockup(sources: dict[str, Image.Image], sprites: dict[str, Image.Image], size: tuple[int, int], scenario: str) -> Image.Image:
    w, h = size
    image = fit_image(sources["background"], size)
    image.alpha_composite(Image.new("RGBA", size, (8, 12, 28, 82 if scenario != "main" else 66)))
    scale = min(w / 1920, h / 1080)
    margin = max(18, int(32 * scale))

    logo_w = min(int(w * 0.25), int(360 * max(0.75, scale)))
    logo_h = max(82, int(168 * max(0.52, scale)))
    logo = fit_image_contained(sources["logo"], (logo_w, logo_h))
    image.alpha_composite(logo, ((w - logo.width) // 2, margin))

    panel_w = min(w - margin * 2, max(int(w * 0.84), 860 if w > 1100 else 720))
    panel_h = min(h - margin * 3 - logo.height, max(int(h * 0.64), 490 if h > 760 else 430))
    panel_x = (w - panel_w) // 2
    panel_y = margin + logo.height + max(8, int(10 * scale))
    image.alpha_composite(fit_image(sprites["skill_selection_panel_frame"], (panel_w, panel_h)), (panel_x, panel_y))

    card_count = 3 if w >= 1180 else 2
    card_gap = max(10, int(panel_w * 0.022))
    detail_w = min(int(panel_w * 0.36), 460 if w > 1100 else 340)
    card_area_w = panel_w - detail_w - card_gap * 4
    card_w = max(218, int((card_area_w - card_gap * (card_count - 1)) / card_count))
    card_h = max(154, min(int(panel_h * 0.39), int(card_w * 0.58)))
    card_y = panel_y + int(panel_h * 0.29)
    card_variants = ["skill_choice_card_selected", "skill_choice_card_ready", "skill_choice_card_disabled"]
    if scenario in ("dense", "compact"):
        card_variants = ["skill_choice_card_selected", "skill_choice_card_locked"]
    card_count = min(card_count, len(card_variants))
    for index in range(card_count):
        x = panel_x + card_gap * 2 + index * (card_w + card_gap)
        card = fit_image(sprites[card_variants[index]], (card_w, card_h))
        image.alpha_composite(card, (x, card_y))
        draw_skill_card_content(image, sources, (x, card_y), (card_w, card_h), index, card_variants[index])

    detail_x = panel_x + panel_w - detail_w - card_gap * 2
    detail_y = card_y
    detail_h = max(216, min(int(panel_h * 0.48), panel_h - int(panel_h * 0.3) - 86))
    image.alpha_composite(fit_image(sprites["skill_detail_panel_frame"], (detail_w, detail_h)), (detail_x, detail_y))
    draw_detail_content(image, sources, (detail_x, detail_y), (detail_w, detail_h), scenario)

    strip_w = min(420, int(panel_w * 0.33))
    strip_h = max(64, int(96 * max(0.62, scale)))
    strip_x = panel_x + card_gap * 2
    strip_y = panel_y + panel_h - strip_h - card_gap
    image.alpha_composite(fit_image(sprites["skill_cost_cooldown_strip"], (strip_w, strip_h)), (strip_x, strip_y))

    button_w = min(420, int(panel_w * 0.32))
    button_h = max(72, int(112 * max(0.64, scale)))
    button = fit_image(sprites["skill_confirm_button_frame"], (button_w, button_h))
    image.alpha_composite(button, (panel_x + panel_w - button_w - card_gap * 2, panel_y + panel_h - button_h - card_gap))

    return flatten_opaque(image, size)


def draw_skill_card_content(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], size: tuple[int, int], index: int, variant: str) -> None:
    x, y = pos
    w, h = size
    slot_size = max(58, min(92, int(h * 0.42)))
    slot_key = "slot_selected" if "selected" in variant else "slot_disabled" if ("disabled" in variant or "locked" in variant) else "slot_ready"
    image.alpha_composite(fit_image(sources[slot_key], (slot_size, slot_size)), (x + 24, y + max(24, int(h * 0.16))))
    skill_key = f"skill_{(index % 6) + 1}"
    icon = fit_image_contained(sources[skill_key], (int(slot_size * 0.62), int(slot_size * 0.62)))
    image.alpha_composite(icon, (x + 24 + (slot_size - icon.width) // 2, y + max(24, int(h * 0.16)) + (slot_size - icon.height) // 2))
    if "locked" in variant:
        lock = fit_image_contained(sources["lock"], (int(slot_size * 0.46), int(slot_size * 0.46)))
        image.alpha_composite(lock, (x + 24 + slot_size - lock.width - 2, y + max(24, int(h * 0.16)) + slot_size - lock.height - 2))
    draw = ImageDraw.Draw(image)
    for row in range(3):
        line_w = int(w * (0.44 - row * 0.06))
        draw.rounded_rectangle((x + int(w * 0.42), y + int(h * 0.24) + row * max(20, h // 7), x + int(w * 0.42) + line_w, y + int(h * 0.24) + row * max(20, h // 7) + max(7, h // 23)), radius=5, fill=(220, 224, 212, 120))
    cost = fit_image_contained(sources["cost_chip"], (max(54, int(w * 0.18)), max(32, int(h * 0.16))))
    image.alpha_composite(cost, (x + w - cost.width - 22, y + h - cost.height - 18))


def draw_detail_content(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], size: tuple[int, int], scenario: str) -> None:
    x, y = pos
    w, h = size
    icon_size = max(70, min(118, int(h * 0.34)))
    frame = fit_image(sources["slot_selected"], (icon_size, icon_size))
    image.alpha_composite(frame, (x + 30, y + 34))
    icon = fit_image_contained(sources["skill_1"], (int(icon_size * 0.62), int(icon_size * 0.62)))
    image.alpha_composite(icon, (x + 30 + (icon_size - icon.width) // 2, y + 34 + (icon_size - icon.height) // 2))
    if scenario == "dense":
        warning = fit_image_contained(sources["warning"], (46, 46))
        image.alpha_composite(warning, (x + w - warning.width - 30, y + 34))
    draw = ImageDraw.Draw(image)
    for row in range(5):
        yy = y + 42 + row * max(24, h // 8)
        draw.rounded_rectangle((x + int(w * 0.36), yy, x + w - 34 - row * 14, yy + max(8, h // 28)), radius=5, fill=(210, 226, 226, 105))
    cooldown = fit_image_contained(sources["cooldown"], (68, 68))
    no_target = fit_image_contained(sources["no_target"], (68, 68))
    image.alpha_composite(cooldown, (x + 34, y + h - cooldown.height - 34))
    image.alpha_composite(no_target, (x + 118, y + h - no_target.height - 34))


def build_contact_sheet(generated: list[dict[str, object]]) -> Image.Image:
    thumb_w, thumb_h = 280, 168
    cols = 3
    rows = (len(generated) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * thumb_w, rows * thumb_h + 58), (13, 17, 31, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((24, 18), "Batch 89 Skill Selection Preflight", fill=(236, 206, 116, 255), font=font(25))
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        thumb = fit_image_contained(Image.open(path).convert("RGBA"), (thumb_w - 24, thumb_h - 48))
        x = (index % cols) * thumb_w + 12
        y = (index // cols) * thumb_h + 62
        sheet.alpha_composite(thumb, (x + (thumb_w - 24 - thumb.width) // 2, y + 4))
        draw.text((x, y + thumb_h - 34), str(item["variant_id"]), fill=(224, 224, 214, 255), font=font(13))
        draw.text((x, y + thumb_h - 18), f"{item['size'][0]}x{item['size'][1]}", fill=(148, 184, 206, 255), font=font(12))
    return sheet


def build_review_sheet(generated: list[dict[str, object]]) -> Image.Image:
    sheet = Image.new("RGBA", (1440, 1840), (10, 14, 28, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((40, 28), "Batch 89 Skill Selection Preflight - Candidate Only", fill=(224, 224, 214, 255), font=font(32))
    draw.text((40, 70), "Textless screen UI using Batch 80/81/82 assets; not Unity accepted; not image2 provenance.", fill=(154, 210, 230, 255), font=font(19))
    card_w, card_h = 660, 250
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        col = index % 2
        row = index // 2
        x = 40 + col * (card_w + 40)
        y = 122 + row * (card_h + 22)
        rounded_frame(draw, x, y, x + card_w, y + card_h, 14, (28, 42, 70, 255), (112, 214, 240, 180), 2)
        preview = fit_image_contained(Image.open(path).convert("RGBA"), (card_w - 32, card_h - 76))
        sheet.alpha_composite(preview, (x + (card_w - preview.width) // 2, y + 14))
        draw.text((x + 18, y + card_h - 52), str(item["variant_id"]), fill=(232, 226, 210, 255), font=font(19))
        draw.text((x + 18, y + card_h - 28), f"{item['asset_type']} | {item['size'][0]}x{item['size'][1]}", fill=(164, 202, 222, 255), font=font(15))
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
        "# Batch 89 Skill Selection Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers skill-selection screen composition, selected/ready/disabled/locked skill cards, detail panel, cost/cooldown strip, and confirm button.",
        "- Reuses existing Qr1-style UI shell plus Batch 80 symbolic skill icons, Batch 81 skill slots, Batch 82 common UI state rows, and Batch 79 lock/warning icons.",
        "- Does not generate, crop, recolor, or import starter-cat body art or new character poses.",
        "- Does not bake Chinese text into sprites; Unity-rendered skill names, descriptions, values, cost, cooldown, and confirm labels remain required.",
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
            "- Skill-selection screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.",
            "- Unity-rendered Chinese skill names, descriptions, numerical values, cooldown digits, cost chips, and confirm labels.",
            "- Selected/ready/disabled/locked states must remain distinct at low height.",
            "- Cooldown, low-resource, and no-target semantics must not conflict with battle HUD skill states.",
            "- Click-target proof for cards, detail panel controls, and confirm action.",
            "- Sprite import settings, screen binding proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note() -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 89 Skill Selection Process Note",
                "",
                "- Lane: `ui_skill_selection` / screen-level preflight.",
                "- UI source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c` (`Qr1`) live section 9 fetch passed in this shell.",
                "- Character identity source truth: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf` (`IAd`) is ACL-blocked for live fetch, so this packet avoids new character body art.",
                "- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.",
                "- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.",
                "- Built-in `image_gen` does not expose an explicit `image2` model selector here, so this batch avoids model-claimed generation and uses deterministic local derivation instead.",
                "- Feishu ACL note: `MDr`, `IAd`, `IZp`, `HDo`, and the `FoW9` Drive folder remain live-read/list blocked for this CLI user; do not claim current-live coverage for those sources.",
                "- Source packs reused: Batch 80 symbolic skill icons, Batch 81 v002 light skill slot frames, Batch 82 common UI states, Batch 79 lock/warning icons.",
                "- Candidate-only boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.",
                "- Text rule: no baked Chinese text; Unity-rendered skill names, descriptions, numbers, cost, cooldown, and confirm labels remain required.",
                "- Starter-cat rule: no body, pose, costume, color, portrait, or framesheet generation is included.",
                "- Runtime gate: Unity-rendered skill-selection screenshots, selected/disabled/locked state proof, cooldown/low-resource/no-target semantics, click targets, import settings, binding, and Console.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_agent_prompt() -> None:
    AGENT_PROMPT_PATH.write_text(
        "\n".join(
            [
                "# Agent Prompt - Batch 89 Skill Selection Preflight",
                "",
                "Review this candidate-only skill-selection screen packet before Unity import.",
                "",
                "Check visual consistency with Qr1 UI/style, Batch 80 symbolic skill icons, Batch 81 v002 light skill slot frames, Batch 82 common UI states, and Batch 79 lock/warning icons.",
                "Confirm there is no baked Chinese text, no new starter-cat body art, no `Assets` writes, and no Unity `.meta` files.",
                "Pay special attention to selected/ready/disabled/locked contrast, cooldown and low-resource semantics, 1024x768 density, and click-target space for cards plus confirm action.",
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
