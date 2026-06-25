from __future__ import annotations

import csv
import hashlib
import shutil
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_88_character_select_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "character_select" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_process_note.md"
AGENT_PROMPT_PATH = CANDIDATE_DIR / "thecat_ui_character_select_batch88_agent_review_prompt.md"

SOURCE_PATHS = [
    REPO_ROOT / "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_sun_charge_burst_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_saiban_shield_counter_impact_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_nephthys_obelisk_turret_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_nephthys_sandstorm_swirl_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_suzune_healing_bell_pulse_64_recommended_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/starter_skill_icon_motifs/batch_80_starter_skill_icon_motifs_2026-06-25/recommended/recommended_64/thecat_ui_skill_suzune_torii_gate_ward_64_recommended_candidate_v001.png",
]

SPRITE_SPECS = (
    ("character_select_shell", "character_select_stage_panel", (1180, 640), "wide stage panel for three starter-card layout"),
    ("character_select_card", "starter_card_frame_selected", (360, 500), "selected starter card frame"),
    ("character_select_card", "starter_card_frame_idle", (360, 500), "idle starter card frame"),
    ("character_select_chip", "starter_role_chip_strip", (360, 96), "role and trait chip strip"),
    ("character_select_badge", "starter_ready_badge", (220, 80), "ready/selected badge shell"),
    ("character_select_action", "starter_launch_button_frame", (420, 112), "start action button shell"),
)

MOCKUP_SPECS = (
    ("character_select_screen", "character_select_1920x1080", (1920, 1080), "main"),
    ("character_select_screen", "character_select_1365x768", (1365, 768), "laptop"),
    ("character_select_screen", "character_select_1280x720", (1280, 720), "compact"),
    ("character_select_screen", "character_select_1024x768", (1024, 768), "dense"),
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

SOURCE_MODEL = "deterministic_local_derivative_from_existing_ui_and_source_locked_hud_assets_not_image2"
RECOMMENDATION = "candidate_only_pending_unity_character_select_screenshots"


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
        path = SPRITE_DIR / f"thecat_ui_character_select_{variant_id}_{size[0]}x{size[1]}_candidate_v001.png"
        image.save(path)
        sprites[variant_id] = image
        generated.append(record(component_id, variant_id, size, "sprite", path, review))

    for component_id, variant_id, size, scenario in MOCKUP_SPECS:
        image = build_mockup(sources, sprites, size, scenario)
        path = MOCKUP_DIR / f"thecat_ui_character_select_{variant_id}_local_mockup_v001.png"
        image.save(path)
        generated.append(record(component_id, variant_id, size, "local_mockup", path, f"{variant_id} local mockup"))

    build_contact_sheet(generated).save(CONTACT_SHEET_PATH)
    build_review_sheet(generated).save(REVIEW_SHEET_PATH)
    write_review_note(generated)
    write_process_note()
    write_agent_prompt()
    write_manifest(generated)
    print(f"Wrote {len(generated)} Batch 88 character-select preflight row(s).")
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
        "saiban_avatar": SOURCE_PATHS[4],
        "nephthys_avatar": SOURCE_PATHS[5],
        "suzune_avatar": SOURCE_PATHS[6],
        "saiban_skill_a": SOURCE_PATHS[7],
        "saiban_skill_b": SOURCE_PATHS[8],
        "nephthys_skill_a": SOURCE_PATHS[9],
        "nephthys_skill_b": SOURCE_PATHS[10],
        "suzune_skill_a": SOURCE_PATHS[11],
        "suzune_skill_b": SOURCE_PATHS[12],
    }
    return {key: Image.open(path).convert("RGBA") for key, path in keys.items()}


def build_sprite(variant_id: str, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    w, h = size
    cyan = (112, 214, 240, 214)
    blue = (24, 40, 70, 210)
    gold = (222, 184, 91, 226)
    soft = (138, 178, 204, 120)
    magenta = (210, 96, 166, 190)

    if variant_id == "character_select_stage_panel":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 32, (15, 26, 52, 205), cyan, 4)
        rounded_frame(draw, 34, 42, w - 34, 128, 24, (28, 42, 72, 190), soft, 2)
        for index in range(3):
            x = 66 + index * ((w - 132) // 3)
            rounded_frame(draw, x, 160, x + 310, h - 62, 26, (25, 38, 66, 188), gold if index == 0 else soft, 3)
    elif variant_id in ("starter_card_frame_selected", "starter_card_frame_idle"):
        selected = variant_id.endswith("selected")
        rounded_frame(draw, 8, 8, w - 8, h - 8, 30, blue, gold if selected else cyan, 4)
        rounded_frame(draw, 32, 34, w - 32, 214, 28, (38, 54, 88, 216), gold if selected else soft, 2)
        draw.ellipse((w // 2 - 78, 62, w // 2 + 78, 218), outline=(242, 224, 166, 180) if selected else (112, 214, 240, 145), width=5)
        for row in range(3):
            y = 254 + row * 46
            draw.rounded_rectangle((42, y, w - 42, y + 18), radius=9, fill=(180, 216, 226, 86))
        for index in range(2):
            x = 92 + index * 96
            rounded_frame(draw, x, h - 102, x + 72, h - 30, 16, (32, 48, 78, 220), gold if selected else soft, 2)
    elif variant_id == "starter_role_chip_strip":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, (28, 44, 74, 205), cyan, 3)
        for index in range(3):
            x = 34 + index * 104
            rounded_frame(draw, x, 26, x + 82, 70, 18, (40, 56, 86, 220), gold if index == 0 else soft, 2)
            draw.line((x + 18, 48, x + 64, 48), fill=(232, 220, 184, 160), width=5)
    elif variant_id == "starter_ready_badge":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (40, 42, 72, 220), magenta, 3)
        draw.ellipse((30, 22, 66, 58), outline=gold, width=4)
        draw.line((82, 40, w - 34, 40), fill=(240, 226, 184, 170), width=7)
    elif variant_id == "starter_launch_button_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 28, (32, 45, 78, 222), gold, 4)
        rounded_frame(draw, 28, 26, w - 28, h - 26, 22, (23, 38, 68, 220), cyan, 2)
        draw.polygon(((w - 78, h // 2), (w - 118, h // 2 - 24), (w - 118, h // 2 + 24)), fill=(238, 215, 142, 210))
        draw.line((54, h // 2, w - 150, h // 2), fill=(228, 228, 210, 150), width=8)
    else:
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, blue, cyan, 4)

    return image


def build_mockup(sources: dict[str, Image.Image], sprites: dict[str, Image.Image], size: tuple[int, int], scenario: str) -> Image.Image:
    w, h = size
    image = fit_image(sources["background"], size)
    image.alpha_composite(Image.new("RGBA", size, (8, 12, 28, 82 if scenario != "main" else 62)))
    scale = min(w / 1920, h / 1080)
    margin = max(18, int(34 * scale))

    logo_w = min(int(w * 0.28), int(420 * max(0.78, scale)))
    logo_h = max(94, int(190 * max(0.55, scale)))
    logo = fit_image_contained(sources["logo"], (logo_w, logo_h))
    image.alpha_composite(logo, ((w - logo.width) // 2, margin))

    panel_w = min(w - margin * 2, max(int(w * 0.82), 900 if w > 1100 else 720))
    panel_h = min(h - margin * 3 - logo.height, max(int(h * 0.62), 500 if h > 760 else 440))
    panel_x = (w - panel_w) // 2
    panel_y = margin + logo.height + max(10, int(12 * scale))
    panel = fit_image(sprites["character_select_stage_panel"], (panel_w, panel_h))
    image.alpha_composite(panel, (panel_x, panel_y))

    card_gap = max(10, int(panel_w * 0.025))
    button_w = min(420, int(panel_w * 0.34))
    button_h = max(72, int(112 * max(0.64, scale)))
    controls_y = panel_y + panel_h - button_h - card_gap
    card_w = int((panel_w - card_gap * 4) / 3)
    card_y = panel_y + int(panel_h * 0.29)
    card_h = min(int(panel_h * 0.58), int(card_w * 1.25), controls_y - card_y - card_gap)
    card_h = max(260, card_h)
    avatars = ("saiban_avatar", "nephthys_avatar", "suzune_avatar")
    skill_pairs = (
        ("saiban_skill_a", "saiban_skill_b"),
        ("nephthys_skill_a", "nephthys_skill_b"),
        ("suzune_skill_a", "suzune_skill_b"),
    )
    for index in range(3):
        card_x = panel_x + card_gap + index * (card_w + card_gap)
        frame_key = "starter_card_frame_selected" if index == 0 else "starter_card_frame_idle"
        card = fit_image(sprites[frame_key], (card_w, card_h))
        image.alpha_composite(card, (card_x, card_y))
        draw_card_content(image, sources, (card_x, card_y), (card_w, card_h), avatars[index], skill_pairs[index], selected=index == 0)

    button = fit_image(sprites["starter_launch_button_frame"], (button_w, button_h))
    image.alpha_composite(button, (panel_x + panel_w - button_w - card_gap, controls_y))

    chip_w = min(360, int(panel_w * 0.28))
    chip_h = max(64, int(96 * max(0.62, scale)))
    chip = fit_image(sprites["starter_role_chip_strip"], (chip_w, chip_h))
    image.alpha_composite(chip, (panel_x + card_gap, controls_y))

    return flatten_opaque(image, size)


def draw_card_content(image: Image.Image, sources: dict[str, Image.Image], pos: tuple[int, int], size: tuple[int, int], avatar_key: str, skills: tuple[str, str], selected: bool) -> None:
    draw = ImageDraw.Draw(image)
    x, y = pos
    w, h = size
    avatar_size = max(72, min(136, int(w * 0.38)))
    avatar = fit_image_contained(sources[avatar_key], (avatar_size, avatar_size))
    image.alpha_composite(avatar, (x + (w - avatar.width) // 2, y + int(h * 0.09)))
    if selected:
        badge = Image.new("RGBA", (max(72, int(w * 0.42)), max(30, int(h * 0.09))), (0, 0, 0, 0))
        badge_draw = ImageDraw.Draw(badge)
        rounded_frame(badge_draw, 2, 2, badge.width - 2, badge.height - 2, 14, (54, 42, 76, 210), (222, 184, 91, 220), 2)
        image.alpha_composite(badge, (x + (w - badge.width) // 2, y + int(h * 0.43)))
    line_y = y + int(h * 0.52)
    for row in range(3):
        bar_w = int(w * (0.68 - row * 0.08))
        draw.rounded_rectangle((x + int(w * 0.16), line_y + row * int(h * 0.08), x + int(w * 0.16) + bar_w, line_y + row * int(h * 0.08) + max(8, int(h * 0.025))), radius=6, fill=(184, 215, 226, 92))
    icon_size = max(42, min(64, int(w * 0.18)))
    for index, skill_key in enumerate(skills):
        icon = fit_image_contained(sources[skill_key], (icon_size, icon_size))
        ix = x + int(w * 0.31) + index * int(w * 0.22)
        iy = y + h - icon_size - int(h * 0.08)
        image.alpha_composite(icon, (ix, iy))


def build_contact_sheet(generated: list[dict[str, object]]) -> Image.Image:
    thumb_w, thumb_h = 300, 176
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
    draw.text((40, 28), "Batch 88 Character Select Preflight - Candidate Only", fill=(224, 224, 214, 255), font=font(32))
    draw.text((40, 70), "Textless screen UI using source-locked HUD avatars; not Unity accepted; not image2 provenance.", fill=(154, 210, 230, 255), font=font(19))
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
        "# Batch 88 Character Select Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers character-select screen composition, three starter cards, selected/idle card states, role chips, ready badge, and start button shell.",
        "- Reuses existing main-menu background, dreamglass panel, title logo, source-locked HUD avatars, and symbolic starter skill icon motifs.",
        "- Does not generate, crop, recolor, or import starter-cat body art or new character poses.",
        "- Does not bake Chinese text into sprites; Unity-rendered names, roles, descriptions, and button labels remain required.",
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
            "- Character-select screenshots at 1920x1080, 1365x768, 1280x720, and 1024x768.",
            "- Unity-rendered Chinese names, roles, descriptions, start labels, and selection state labels.",
            "- Source-locked HUD avatars remain consistent with the starter-cat colored turnarounds.",
            "- Selected/idle cards, ready badge, skill motif icons, and start button remain readable at 1024x768.",
            "- Click-target proof for three starter cards and start action.",
            "- Sprite import settings, scene/menu binding proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note() -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 88 Character Select Process Note",
                "",
                "- Lane: `ui_character_select` / screen-level preflight.",
                "- UI source truth: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c` (`Qr1`) live section 9 fetch passed in this shell.",
                "- Character identity source truth: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf` (`IAd`) is ACL-blocked for live fetch, so this packet uses local IAd-derived source-lock packets and existing source-locked HUD avatars only.",
                "- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.",
                "- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.",
                "- Built-in `image_gen` does not expose an explicit `image2` model selector here, so this batch avoids model-claimed generation and uses deterministic local derivation instead.",
                "- Feishu ACL note: `MDr`, `IAd`, `IZp`, `HDo`, and the `FoW9` Drive folder remain live-read/list blocked for this CLI user; do not claim current-live coverage for those sources.",
                "- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.",
                "- Text rule: no baked Chinese text; Unity-rendered names, roles, descriptions, and button labels remain required.",
                "- Starter-cat rule: HUD avatar reuse is allowed; no new body, pose, costume, color, portrait, or framesheet generation is included.",
                "- Runtime gate: Unity-rendered character-select screenshots, source-lock consistency, click targets, import settings, binding, and Console.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_agent_prompt() -> None:
    AGENT_PROMPT_PATH.write_text(
        "\n".join(
            [
                "# Agent Prompt - Batch 88 Character Select Preflight",
                "",
                "Review this candidate-only character-select screen packet before Unity import.",
                "",
                "Check visual consistency with Qr1 UI/style, current main-menu background, dreamglass panel language, source-locked HUD avatars, and Batch 80 symbolic skill icons.",
                "Confirm there is no baked Chinese text, no new starter-cat body art, no new character pose/portrait, no `Assets` writes, and no Unity `.meta` files.",
                "Pay special attention to 1024x768 card density, selected-vs-idle card readability, avatar identity, skill motif fit, and click-target space for three starter cards plus start action.",
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
