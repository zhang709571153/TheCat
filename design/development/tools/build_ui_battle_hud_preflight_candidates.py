from __future__ import annotations

import csv
import hashlib
import shutil
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_87_battle_hud_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "battle_hud" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_process_note.md"
AGENT_PROMPT_PATH = CANDIDATE_DIR / "thecat_ui_battle_hud_batch87_agent_review_prompt.md"

SOURCE_PATHS = [
    REPO_ROOT / "Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hp_gauge_frame_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hp_gauge_fill_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_sleep_gauge_frame_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_sleep_gauge_fill_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_poop_gauge_frame_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_poop_gauge_fill_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hunger_gauge_frame_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hunger_gauge_fill_384x48_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hp_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_poop_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_status_sleep_stable_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_status_shield_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_status_slow_64_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_pause_icon_128_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_speed_fast_icon_128_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15/thecat_ui_runtime_restart_icon_128_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_sun_charge_burst_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_shield_counter_impact_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_nephthys_obelisk_turret_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_suzune_healing_bell_pulse_64_recommended_candidate_v001.png",
]

SPRITE_SPECS = (
    ("battle_hud_top", "battle_top_resource_rail_frame", (1240, 144), "top resource rail for HP/sleep/poop/hunger gauges"),
    ("battle_hud_party", "battle_cat_party_panel", (520, 188), "cat party avatar and status frame"),
    ("battle_hud_enemy", "battle_enemy_status_panel", (520, 156), "enemy target and warning status frame"),
    ("battle_hud_skill", "battle_skill_tray_frame", (900, 180), "bottom skill tray frame"),
    ("battle_hud_status", "battle_status_chip_strip", (480, 96), "status chip strip frame"),
    ("battle_hud_runtime", "battle_runtime_control_cluster", (360, 96), "pause speed restart cluster frame"),
)

MOCKUP_SPECS = (
    ("battle_hud_screen", "battle_hud_1920x1080", (1920, 1080), "main"),
    ("battle_hud_screen", "battle_hud_pressure_1365x768", (1365, 768), "pressure"),
    ("battle_hud_screen", "battle_hud_compact_1280x720", (1280, 720), "compact"),
    ("battle_hud_screen", "battle_hud_dense_1024x768", (1024, 768), "dense"),
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

SOURCE_MODEL = "deterministic_local_derivative_from_existing_hud_assets_not_image2"
RECOMMENDATION = "candidate_only_pending_unity_battle_hud_screenshots"


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
        path = SPRITE_DIR / f"thecat_ui_battle_hud_{variant_id}_{size[0]}x{size[1]}_candidate_v001.png"
        image.save(path)
        sprites[variant_id] = image
        generated.append(record(component_id, variant_id, size, "sprite", path, review))

    for component_id, variant_id, size, scenario in MOCKUP_SPECS:
        image = build_mockup(sources, sprites, size, scenario)
        path = MOCKUP_DIR / f"thecat_ui_battle_hud_{variant_id}_local_mockup_v001.png"
        image.save(path)
        generated.append(record(component_id, variant_id, size, "local_mockup", path, f"{variant_id} local mockup"))

    build_contact_sheet(generated).save(CONTACT_SHEET_PATH)
    build_review_sheet(generated).save(REVIEW_SHEET_PATH)
    write_review_note(generated)
    write_process_note()
    write_agent_prompt()
    write_manifest(generated)
    print(f"Wrote {len(generated)} Batch 87 battle HUD preflight row(s).")
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
        "hp_frame": SOURCE_PATHS[3],
        "hp_fill": SOURCE_PATHS[4],
        "sleep_frame": SOURCE_PATHS[5],
        "sleep_fill": SOURCE_PATHS[6],
        "poop_frame": SOURCE_PATHS[7],
        "poop_fill": SOURCE_PATHS[8],
        "hunger_frame": SOURCE_PATHS[9],
        "hunger_fill": SOURCE_PATHS[10],
        "hp_icon": SOURCE_PATHS[11],
        "sleep_icon": SOURCE_PATHS[12],
        "poop_icon": SOURCE_PATHS[13],
        "hunger_icon": SOURCE_PATHS[14],
        "status_sleep": SOURCE_PATHS[15],
        "status_shield": SOURCE_PATHS[16],
        "status_mark": SOURCE_PATHS[17],
        "status_slow": SOURCE_PATHS[18],
        "skill_frame": SOURCE_PATHS[19],
        "skill_cooldown": SOURCE_PATHS[20],
        "skill_no_target": SOURCE_PATHS[21],
        "skill_cost": SOURCE_PATHS[22],
        "saiban_avatar": SOURCE_PATHS[23],
        "nephthys_avatar": SOURCE_PATHS[24],
        "suzune_avatar": SOURCE_PATHS[25],
        "blackmud_enemy": SOURCE_PATHS[26],
        "coldlight_enemy": SOURCE_PATHS[27],
        "pause_icon": SOURCE_PATHS[28],
        "speed_icon": SOURCE_PATHS[29],
        "restart_icon": SOURCE_PATHS[30],
        "skill_1": SOURCE_PATHS[31],
        "skill_2": SOURCE_PATHS[32],
        "skill_3": SOURCE_PATHS[33],
        "skill_4": SOURCE_PATHS[34],
    }
    return {key: Image.open(path).convert("RGBA") for key, path in keys.items()}


def build_sprite(variant_id: str, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    w, h = size
    cyan = (112, 214, 240, 214)
    blue = (24, 40, 70, 205)
    gold = (222, 184, 91, 226)
    soft = (138, 178, 204, 116)
    danger = (212, 84, 128, 210)

    if variant_id == "battle_top_resource_rail_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 28, blue, cyan, 4)
        for i in range(4):
            x0 = 42 + i * 292
            rounded_frame(draw, x0, 36, x0 + 240, h - 32, 18, (30, 50, 82, 196), gold if i == 0 else soft, 2)
            draw.line((x0 + 62, h // 2, x0 + 210, h // 2), fill=(210, 228, 228, 160), width=8)
    elif variant_id == "battle_cat_party_panel":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 26, (28, 42, 72, 210), cyan, 4)
        for i in range(3):
            x = 36 + i * 150
            rounded_frame(draw, x, 30, x + 116, h - 30, 20, (40, 55, 82, 210), gold if i == 0 else soft, 2)
            draw.ellipse((x + 22, 48, x + 94, 120), outline=(210, 215, 200, 150), width=4)
    elif variant_id == "battle_enemy_status_panel":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 24, (38, 38, 70, 214), danger, 4)
        rounded_frame(draw, 34, 32, 150, h - 32, 18, (28, 44, 74, 204), gold, 2)
        draw.line((176, 54, w - 50, 54), fill=(224, 216, 196, 170), width=8)
        draw.line((176, 96, w - 92, 96), fill=(212, 84, 128, 160), width=6)
    elif variant_id == "battle_skill_tray_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 30, (24, 36, 66, 210), cyan, 4)
        for i in range(6):
            x = 42 + i * 135
            rounded_frame(draw, x, 32, x + 104, h - 32, 20, (32, 46, 76, 214), gold if i < 3 else soft, 2)
            draw.ellipse((x + 16, 48, x + 88, 120), outline=(116, 214, 240, 140), width=4)
    elif variant_id == "battle_status_chip_strip":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (28, 44, 74, 204), cyan, 3)
        for i in range(4):
            x = 34 + i * 108
            draw.ellipse((x, 24, x + 48, 72), outline=gold if i == 0 else soft, width=4)
            draw.line((x + 62, 48, x + 94, 48), fill=(210, 215, 200, 140), width=5)
    elif variant_id == "battle_runtime_control_cluster":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (24, 39, 68, 210), cyan, 3)
        for i in range(3):
            x = 36 + i * 100
            rounded_frame(draw, x, 22, x + 68, h - 22, 18, (36, 52, 84, 214), gold if i == 0 else soft, 2)
            draw.ellipse((x + 18, 38, x + 50, 70), outline=(210, 215, 200, 140), width=3)
    else:
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, blue, cyan, 4)

    return image


def build_mockup(sources: dict[str, Image.Image], sprites: dict[str, Image.Image], size: tuple[int, int], scenario: str) -> Image.Image:
    w, h = size
    image = fit_image(sources["background"], size)
    image.alpha_composite(Image.new("RGBA", size, (7, 10, 26, 70 if scenario != "pressure" else 100)))

    scale = min(w / 1920, h / 1080)
    margin = max(18, int(28 * scale))
    top_w = min(w - margin * 2, max(int(1240 * max(0.74, scale)), int(w * 0.92)))
    top_h = max(92, int(144 * max(0.72, scale)))
    top_x = (w - top_w) // 2
    top = fit_image(sprites["battle_top_resource_rail_frame"], (top_w, top_h))
    image.alpha_composite(top, (top_x, margin))
    draw_gauges(image, sources, top_x + int(46 * top_w / 1240), margin + int(34 * top_h / 144), top_w, top_h, scenario)

    party_w = min(520, int(w * 0.34))
    party_h = max(126, int(188 * max(0.68, scale)))
    party = fit_image(sprites["battle_cat_party_panel"], (party_w, party_h))
    party_pos = (margin, h - party_h - margin)
    image.alpha_composite(party, party_pos)
    draw_party(image, sources, party_pos, party_w, party_h)

    control_w = min(360, int(w * 0.26))
    control_h = max(72, int(96 * max(0.7, scale)))
    control_x = w - control_w - margin
    control_y = margin + top_h + max(8, int(10 * scale))
    image.alpha_composite(fit_image(sprites["battle_runtime_control_cluster"], (control_w, control_h)), (control_x, control_y))
    draw_runtime_controls(image, sources, (control_x, control_y), control_w, control_h)

    enemy_w = min(520, int(w * 0.36))
    enemy_h = max(112, int(156 * max(0.68, scale)))
    enemy = fit_image(sprites["battle_enemy_status_panel"], (enemy_w, enemy_h))
    enemy_pos = (w - enemy_w - margin, control_y + control_h + max(10, int(12 * scale)))
    image.alpha_composite(enemy, enemy_pos)
    draw_enemy(image, sources, enemy_pos, enemy_w, enemy_h, scenario)

    tray_w = min(w - margin * 2 - party_w - 24, int(900 * max(0.74, scale)))
    tray_h = max(126, int(180 * max(0.68, scale)))
    tray_x = min(w - tray_w - margin, party_pos[0] + party_w + max(18, int(24 * scale)))
    tray_y = h - tray_h - margin
    tray = fit_image(sprites["battle_skill_tray_frame"], (tray_w, tray_h))
    image.alpha_composite(tray, (tray_x, tray_y))
    draw_skill_slots(image, sources, (tray_x, tray_y), tray_w, tray_h, scenario)

    status_w = min(480, int(w * 0.34))
    status_h = max(72, int(96 * max(0.7, scale)))
    status_x = margin
    status_y = max(margin + top_h + 16, int(h * 0.24))
    image.alpha_composite(fit_image(sprites["battle_status_chip_strip"], (status_w, status_h)), (status_x, status_y))
    draw_status_strip(image, sources, (status_x, status_y), status_w, status_h)

    return image


def draw_gauges(image: Image.Image, sources: dict[str, Image.Image], pos_x: int, pos_y: int, top_w: int, top_h: int, scenario: str) -> None:
    gauge_keys = [
        ("hp_icon", "hp_frame", "hp_fill", 0.72 if scenario == "pressure" else 0.88),
        ("sleep_icon", "sleep_frame", "sleep_fill", 0.42 if scenario == "dense" else 0.66),
        ("poop_icon", "poop_frame", "poop_fill", 0.35 if scenario == "compact" else 0.52),
        ("hunger_icon", "hunger_frame", "hunger_fill", 0.56 if scenario != "pressure" else 0.28),
    ]
    slot_gap = max(8, int(top_w * 0.02))
    slot_w = max(160, int((top_w - slot_gap * 5) / 4))
    icon_size = max(34, int(top_h * 0.38))
    gauge_h = max(28, int(top_h * 0.34))
    for index, (icon_key, frame_key, fill_key, fill_amount) in enumerate(gauge_keys):
        x = pos_x + index * (slot_w + slot_gap)
        y = pos_y
        image.alpha_composite(fit_image(sources[icon_key], (icon_size, icon_size)), (x, y + (gauge_h - icon_size) // 2))
        frame = fit_image(sources[frame_key], (slot_w - icon_size - 8, gauge_h))
        image.alpha_composite(frame, (x + icon_size + 8, y))
        fill_w = max(4, int((slot_w - icon_size - 24) * fill_amount))
        fill = fit_image(sources[fill_key], (fill_w, max(8, gauge_h - 12)))
        image.alpha_composite(fill, (x + icon_size + 16, y + 6))


def draw_party(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], panel_w: int, panel_h: int) -> None:
    avatar_size = max(58, min(92, int(panel_h * 0.48)))
    gap = max(8, int((panel_w - avatar_size * 3) / 5))
    for index, key in enumerate(("saiban_avatar", "nephthys_avatar", "suzune_avatar")):
        x = pos[0] + gap + index * (avatar_size + gap)
        y = pos[1] + max(26, int(panel_h * 0.22))
        avatar = fit_image_contained(sources[key], (avatar_size, avatar_size))
        image.alpha_composite(avatar, (x + (avatar_size - avatar.width) // 2, y + (avatar_size - avatar.height) // 2))


def draw_enemy(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], panel_w: int, panel_h: int, scenario: str) -> None:
    enemy_key = "coldlight_enemy" if scenario in ("pressure", "dense") else "blackmud_enemy"
    enemy_size = max(74, min(116, int(panel_h * 0.78)))
    enemy = fit_image_contained(sources[enemy_key], (enemy_size, enemy_size))
    image.alpha_composite(enemy, (pos[0] + 34, pos[1] + (panel_h - enemy.height) // 2))
    draw = ImageDraw.Draw(image)
    x0 = pos[0] + 166
    y0 = pos[1] + int(panel_h * 0.34)
    draw.line((x0, y0, pos[0] + panel_w - 46, y0), fill=(225, 218, 196, 155), width=max(4, panel_h // 18))
    draw.line((x0, y0 + int(panel_h * 0.28), pos[0] + panel_w - 90, y0 + int(panel_h * 0.28)), fill=(212, 84, 128, 145), width=max(4, panel_h // 20))


def draw_skill_slots(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], tray_w: int, tray_h: int, scenario: str) -> None:
    skill_keys = ("skill_1", "skill_2", "skill_3", "skill_4", "skill_no_target", "skill_cooldown")
    count = 5 if scenario == "dense" else 6
    slot = max(58, min(96, int(tray_h * 0.56)))
    gap = max(6, int((tray_w - slot * count) / (count + 1)))
    y = pos[1] + (tray_h - slot) // 2
    for index in range(count):
        x = pos[0] + gap + index * (slot + gap)
        frame = fit_image(sources["skill_frame"], (slot, slot))
        image.alpha_composite(frame, (x, y))
        icon_key = skill_keys[index]
        icon = fit_image_contained(sources[icon_key], (int(slot * 0.58), int(slot * 0.58)))
        image.alpha_composite(icon, (x + (slot - icon.width) // 2, y + (slot - icon.height) // 2))
        if index == count - 1 or (scenario == "pressure" and index == 2):
            overlay = fit_image(sources["skill_cooldown"], (slot, slot))
            image.alpha_composite(overlay, (x, y))
        if index in (1, 3):
            chip = fit_image_contained(sources["skill_cost"], (int(slot * 0.38), int(slot * 0.3)))
            image.alpha_composite(chip, (x + slot - chip.width - 4, y + slot - chip.height - 4))


def draw_status_strip(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], strip_w: int, strip_h: int) -> None:
    icons = ("status_sleep", "status_shield", "status_mark", "status_slow")
    icon_size = max(32, min(54, int(strip_h * 0.56)))
    gap = max(10, int((strip_w - icon_size * 4) / 5))
    y = pos[1] + (strip_h - icon_size) // 2
    for index, key in enumerate(icons):
        x = pos[0] + gap + index * (icon_size + gap)
        image.alpha_composite(fit_image(sources[key], (icon_size, icon_size)), (x, y))


def draw_runtime_controls(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], cluster_w: int, cluster_h: int) -> None:
    icons = ("pause_icon", "speed_icon", "restart_icon")
    icon_size = max(34, min(54, int(cluster_h * 0.56)))
    gap = max(10, int((cluster_w - icon_size * 3) / 4))
    y = pos[1] + (cluster_h - icon_size) // 2
    for index, key in enumerate(icons):
        x = pos[0] + gap + index * (icon_size + gap)
        image.alpha_composite(fit_image(sources[key], (icon_size, icon_size)), (x, y))


def build_contact_sheet(generated: list[dict[str, object]]) -> Image.Image:
    thumb_w, thumb_h = 280, 168
    cols = 2
    rows = (len(generated) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * thumb_w, rows * thumb_h), (13, 17, 31, 255))
    draw = ImageDraw.Draw(sheet)
    for index, item in enumerate(generated):
        path = item["path"]
        assert isinstance(path, Path)
        thumb = fit_image_contained(Image.open(path).convert("RGBA"), (thumb_w - 24, thumb_h - 42))
        x = (index % cols) * thumb_w + 12
        y = (index // cols) * thumb_h + 12
        sheet.alpha_composite(thumb, (x + (thumb_w - 24 - thumb.width) // 2, y + 6))
        draw.text((x, y + thumb_h - 25), str(item["variant_id"]), fill=(224, 224, 214, 255), font=font(13))
    return sheet


def build_review_sheet(generated: list[dict[str, object]]) -> Image.Image:
    sheet = Image.new("RGBA", (1440, 1680), (10, 14, 28, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((40, 28), "Batch 87 Battle HUD Preflight - Candidate Only", fill=(224, 224, 214, 255), font=font(34))
    draw.text((40, 72), "Textless sprites + local mockups; not Unity accepted; not image2 provenance.", fill=(154, 210, 230, 255), font=font(20))
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
        "# Batch 87 Battle HUD Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers battle HUD composition, top resource rail, cat party panel, enemy status panel, skill tray, status chips, and runtime controls.",
        "- Reuses existing bedroom battle background, core gauges, status icons, HUD avatars, enemy sprites, skill frames, recommended symbolic skill icons, and runtime control icons.",
        "- Does not generate, crop, recolor, or import starter-cat body art.",
        "- Does not bake Chinese text into sprites; Unity-rendered labels, numbers, cooldowns, and tooltips remain required.",
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
            "- Battle HUD screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.",
            "- Unity-rendered Chinese labels, dynamic gauge values, cooldown digits, cost chips, and status counts.",
            "- Skill slot selected/ready/cooldown/disabled states remain distinguishable at actual HUD scale.",
            "- Top resource rail and bottom skill tray must not cover enemies, bed/props, or attack telegraphs.",
            "- Runtime pause/speed/restart controls need click-target proof and do not conflict with gameplay input.",
            "- Sprite import settings, scene/prefab binding proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note() -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 87 Battle HUD Process Note",
                "",
                "- Lane: `battle_hud` / screen-level preflight.",
                "- Source truth: Qr1 UI/style; existing core gauges, status icons, skill slot frames, recommended symbolic skill icons, HUD avatars, bedroom battle background, and runtime control icons.",
                "- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.",
                "- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.",
                "- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.",
                "- Text rule: no baked Chinese text; Unity-rendered text, numbers, cooldowns, and status counts remain required.",
                "- Starter-cat rule: HUD avatar reuse is allowed; no new body, pose, costume, color, or framesheet generation is included.",
                "- Runtime gate: Unity-rendered battle HUD screenshots, skill state readability, gauge value replacement, click targets, import settings, binding, and Console.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_agent_prompt() -> None:
    AGENT_PROMPT_PATH.write_text(
        "\n".join(
            [
                "# Agent Prompt - Batch 87 Battle HUD Preflight",
                "",
                "Review this candidate-only battle HUD packet before Unity import.",
                "",
                "Check visual consistency with Qr1 UI/style, current core gauges, status icons, skill slot frames, recommended symbolic skill icons, battle background, HUD avatars, and runtime controls.",
                "Confirm there is no baked Chinese text, no new starter-cat body art, no `Assets` writes, and no Unity `.meta` files.",
                "Pay special attention to 1024x768 bottom-tray density, enemy/bed occlusion, skill state readability, cooldown/cost proof, and pause/speed/restart click targets.",
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
