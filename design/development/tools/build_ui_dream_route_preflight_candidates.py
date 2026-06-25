from __future__ import annotations

import csv
import hashlib
import shutil
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_86_dream_route_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "dream_route" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_process_note.md"
AGENT_PROMPT_PATH = CANDIDATE_DIR / "thecat_ui_dream_route_batch86_agent_review_prompt.md"

SOURCE_PATHS = [
    REPO_ROOT / "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_partner_frame_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_shop_frame_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_blessing_frame_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_dreamevent_frame_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_restnest_frame_512x256_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_defense_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_elite_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_partner_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_shop_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_dreamevent_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_blessing_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_restnest_icon_128_v001.png",
    REPO_ROOT / "Assets/TheCat/Art/UI/Icons/thecat_ui_route_bossnode_icon_128_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/route_map_readability_batch65_manifest.csv",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_current_node_halo_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_selected_node_ring_256_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_available_path_connector_512x128_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_locked_path_connector_512x128_candidate_v001.png",
    REPO_ROOT / "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15/thecat_ui_route_boss_path_pressure_512x128_candidate_v001.png",
]

SPRITE_SPECS = (
    ("dream_route_panel", "route_map_panel_frame", (1120, 640), "large textless dream route map panel frame"),
    ("dream_route_header", "route_layer_header_frame", (760, 96), "textless route layer header frame"),
    ("dream_route_node", "route_node_socket_frame", (192, 192), "route node socket frame for icon overlays"),
    ("dream_route_choice", "route_choice_card_slot", (440, 220), "route choice card slot frame"),
    ("dream_route_path", "route_path_ribbon_frame", (640, 96), "route path ribbon frame"),
    ("dream_route_boss", "route_boss_gate_frame", (360, 260), "boss gate pressure frame"),
)

MOCKUP_SPECS = (
    ("dream_route_screen", "dream_route_1920x1080", (1920, 1080), "main"),
    ("dream_route_screen", "route_branch_1365x768", (1365, 768), "branch"),
    ("dream_route_screen", "route_boss_pressure_1280x720", (1280, 720), "boss"),
    ("dream_route_screen", "route_compact_1024x768", (1024, 768), "compact"),
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

SOURCE_MODEL = "deterministic_local_derivative_from_route_assets_not_image2"
RECOMMENDATION = "candidate_only_pending_unity_dream_route_screenshots"


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
        path = SPRITE_DIR / f"thecat_ui_dream_route_{variant_id}_{size[0]}x{size[1]}_candidate_v001.png"
        image.save(path)
        sprites[variant_id] = image
        generated.append(record(component_id, variant_id, size, "sprite", path, review))

    for component_id, variant_id, size, scenario in MOCKUP_SPECS:
        image = build_mockup(sources, sprites, size, scenario)
        path = MOCKUP_DIR / f"thecat_ui_dream_route_{variant_id}_local_mockup_v001.png"
        image.save(path)
        generated.append(record(component_id, variant_id, size, "local_mockup", path, f"{variant_id} local mockup"))

    contact = build_contact_sheet(generated)
    contact.save(CONTACT_SHEET_PATH)
    review = build_review_sheet(generated)
    review.save(REVIEW_SHEET_PATH)

    write_review_note(generated)
    write_process_note()
    write_agent_prompt()
    write_manifest(generated)
    print(f"Wrote {len(generated)} Batch 86 dream route preflight row(s).")
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
        "title": SOURCE_PATHS[3],
        "partner_card": SOURCE_PATHS[4],
        "shop_card": SOURCE_PATHS[5],
        "blessing_card": SOURCE_PATHS[6],
        "dream_event_card": SOURCE_PATHS[7],
        "rest_card": SOURCE_PATHS[8],
        "defense_icon": SOURCE_PATHS[9],
        "elite_icon": SOURCE_PATHS[10],
        "partner_icon": SOURCE_PATHS[11],
        "shop_icon": SOURCE_PATHS[12],
        "dream_event_icon": SOURCE_PATHS[13],
        "blessing_icon": SOURCE_PATHS[14],
        "rest_icon": SOURCE_PATHS[15],
        "boss_icon": SOURCE_PATHS[16],
        "current_halo": SOURCE_PATHS[18],
        "selected_ring": SOURCE_PATHS[19],
        "available_path": SOURCE_PATHS[20],
        "locked_path": SOURCE_PATHS[21],
        "boss_path": SOURCE_PATHS[22],
    }
    return {key: Image.open(path).convert("RGBA") for key, path in keys.items()}


def build_sprite(variant_id: str, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    w, h = size
    cyan = (118, 214, 240, 210)
    blue = (30, 45, 78, 190)
    gold = (222, 184, 91, 220)
    soft = (148, 177, 200, 100)

    if variant_id == "route_map_panel_frame":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 30, blue, cyan, 5)
        rounded_frame(draw, 34, 34, w - 34, h - 34, 22, (70, 79, 104, 172), gold, 2)
        draw.line((70, 76, w - 70, 76), fill=gold, width=4)
        draw.line((70, h - 58, w - 70, h - 58), fill=(112, 202, 224, 160), width=6)
    elif variant_id == "route_layer_header_frame":
        rounded_frame(draw, 6, 12, w - 6, h - 12, 20, (26, 42, 75, 210), cyan, 3)
        for i in range(3):
            x0 = 38 + i * ((w - 76) // 3)
            x1 = 38 + (i + 1) * ((w - 76) // 3) - 12
            stroke = gold if i == 0 else (130, 170, 195, 150)
            rounded_frame(draw, x0, 30, x1, h - 30, 16, (36, 52, 84, 210), stroke, 2)
            draw.line((x0 + 24, h // 2, x1 - 24, h // 2), fill=(210, 210, 205, 170), width=5)
    elif variant_id == "route_node_socket_frame":
        rounded_frame(draw, 18, 18, w - 18, h - 18, 44, (28, 44, 74, 210), cyan, 4)
        draw.ellipse((46, 46, w - 46, h - 46), outline=gold, width=5)
        draw.ellipse((74, 74, w - 74, h - 74), outline=(115, 210, 235, 130), width=4)
    elif variant_id == "route_choice_card_slot":
        rounded_frame(draw, 8, 8, w - 8, h - 8, 22, (35, 47, 78, 214), cyan, 3)
        rounded_frame(draw, 24, 30, 138, h - 30, 18, (31, 51, 82, 210), gold, 2)
        draw.line((166, 56, w - 44, 56), fill=(215, 214, 200, 178), width=7)
        draw.line((166, 92, w - 102, 92), fill=(126, 199, 225, 145), width=5)
        draw.line((166, 138, w - 72, 138), fill=soft, width=4)
    elif variant_id == "route_path_ribbon_frame":
        draw.line((42, h // 2, w - 42, h // 2), fill=(85, 170, 207, 120), width=13)
        draw.line((52, h // 2, w - 52, h // 2), fill=gold, width=3)
        for x in range(84, w - 60, 110):
            draw.ellipse((x - 8, h // 2 - 8, x + 8, h // 2 + 8), fill=(222, 184, 91, 230))
    elif variant_id == "route_boss_gate_frame":
        rounded_frame(draw, 14, 10, w - 14, h - 10, 30, (58, 36, 70, 214), (210, 84, 128, 220), 5)
        rounded_frame(draw, 44, 36, w - 44, h - 36, 22, (28, 44, 74, 180), gold, 3)
        draw.arc((72, 56, w - 72, h + 90), 202, 338, fill=(212, 82, 132, 200), width=10)
        draw.line((88, h - 68, w - 88, h - 68), fill=(122, 205, 231, 160), width=6)
    else:
        rounded_frame(draw, 8, 8, w - 8, h - 8, 20, blue, cyan, 4)

    return image


def build_mockup(sources: dict[str, Image.Image], sprites: dict[str, Image.Image], size: tuple[int, int], scenario: str) -> Image.Image:
    w, h = size
    image = fit_image(sources["background"], size)
    overlay = Image.new("RGBA", size, (8, 11, 28, 60 if scenario != "boss" else 95))
    image.alpha_composite(overlay)

    panel_w = min(int(w * 0.74), 1120)
    panel_h = min(int(h * 0.68), 640)
    px = (w - panel_w) // 2
    py = max(34, int(h * 0.12))
    panel = fit_image(sprites["route_map_panel_frame"], (panel_w, panel_h))
    image.alpha_composite(panel, (px, py))

    title = fit_image(sources["title"], (min(270, w // 4), min(128, h // 6)))
    image.alpha_composite(title, (px + 44, max(20, py - title.height // 2)))
    header = fit_image(sprites["route_layer_header_frame"], (min(760, panel_w - 210), 96))
    image.alpha_composite(header, (px + panel_w - header.width - 70, py + 60))

    nodes = route_nodes(panel_w, panel_h, scenario)
    draw_paths(image, sources, px, py, nodes, scenario)
    draw_nodes(image, sources, sprites, px, py, nodes, scenario)
    draw_choice_cards(image, sources, sprites, px, py, panel_w, panel_h, scenario)

    if scenario == "boss":
        gate = fit_image(sprites["route_boss_gate_frame"], (min(360, panel_w // 3), min(260, panel_h // 2)))
        image.alpha_composite(gate, (px + panel_w - gate.width - 58, py + panel_h - gate.height - 70))
        image.alpha_composite(fit_image(sources["boss_icon"], (96, 96)), (px + panel_w - gate.width // 2 - 48 - 58, py + panel_h - gate.height // 2 - 48 - 70))

    return image


def route_nodes(panel_w: int, panel_h: int, scenario: str) -> list[tuple[int, int, str, str]]:
    compact = scenario == "compact"
    scale_x = panel_w / 1120
    scale_y = panel_h / 640
    base = [
        (150, 350, "defense_icon", "current"),
        (325, 235, "shop_icon", "available"),
        (330, 465, "rest_icon", "available"),
        (535, 340, "partner_icon", "selected"),
        (735, 230, "dream_event_icon", "locked" if compact else "available"),
        (750, 455, "blessing_icon", "available"),
        (930, 340, "boss_icon" if scenario == "boss" else "elite_icon", "boss" if scenario == "boss" else "available"),
    ]
    if compact:
        base = base[:5]
    return [(int(x * scale_x), int(y * scale_y), icon, state) for x, y, icon, state in base]


def draw_paths(image: Image.Image, sources: dict[str, Image.Image], px: int, py: int, nodes: list[tuple[int, int, str, str]], scenario: str) -> None:
    draw = ImageDraw.Draw(image)
    for i in range(len(nodes) - 1):
        x0, y0, _, state0 = nodes[i]
        x1, y1, _, state1 = nodes[i + 1]
        color = (224, 185, 88, 178) if "locked" not in (state0, state1) else (116, 132, 155, 110)
        if state1 == "boss" or scenario == "boss" and i >= len(nodes) - 2:
            color = (221, 92, 138, 195)
        draw.line((px + x0, py + y0, px + x1, py + y1), fill=color, width=8)
        draw.line((px + x0, py + y0, px + x1, py + y1), fill=(100, 204, 235, 90), width=3)


def draw_nodes(image: Image.Image, sources: dict[str, Image.Image], sprites: dict[str, Image.Image], px: int, py: int, nodes: list[tuple[int, int, str, str]], scenario: str) -> None:
    socket = fit_image(sprites["route_node_socket_frame"], (112, 112))
    halo = fit_image(sources["current_halo"], (132, 132))
    ring = fit_image(sources["selected_ring"], (128, 128))
    for x, y, icon_key, state in nodes:
        cx, cy = px + x, py + y
        if state == "current":
            image.alpha_composite(halo, (cx - 66, cy - 66))
        if state == "selected":
            image.alpha_composite(ring, (cx - 64, cy - 64))
        image.alpha_composite(socket, (cx - 56, cy - 56))
        image.alpha_composite(fit_image(sources[icon_key], (72, 72)), (cx - 36, cy - 36))
        if state == "locked":
            ImageDraw.Draw(image).line((cx - 42, cy + 42, cx + 42, cy - 42), fill=(138, 148, 170, 180), width=6)


def draw_choice_cards(image: Image.Image, sources: dict[str, Image.Image], sprites: dict[str, Image.Image], px: int, py: int, panel_w: int, panel_h: int, scenario: str) -> None:
    if scenario == "compact":
        card_size = (260, 132)
        positions = [(px + 78, py + panel_h - 170), (px + panel_w - 338, py + panel_h - 170)]
        icon_keys = ["shop_icon", "partner_icon"]
    else:
        card_size = (300, 150)
        y = py + panel_h - 190
        positions = [(px + 86, y), (px + panel_w // 2 - 150, y), (px + panel_w - 386, y)]
        icon_keys = ["shop_icon", "partner_icon", "blessing_icon" if scenario != "boss" else "boss_icon"]
    card = fit_image(sprites["route_choice_card_slot"], card_size)
    for pos, icon_key in zip(positions, icon_keys):
        image.alpha_composite(card, pos)
        image.alpha_composite(fit_image(sources[icon_key], (58, 58)), (pos[0] + 34, pos[1] + (card.height - 58) // 2))


def build_contact_sheet(generated: list[dict[str, object]]) -> Image.Image:
    thumb_w, thumb_h = 260, 164
    cols = 5
    rows = (len(generated) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * thumb_w, rows * thumb_h + 70), (11, 14, 28, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((24, 20), "Batch 86 Dream Route Preflight Contact Sheet", fill=(236, 206, 116, 255), font=font(28))
    for index, item in enumerate(generated):
        x = (index % cols) * thumb_w + 18
        y = (index // cols) * thumb_h + 72
        path = item["path"]
        assert isinstance(path, Path)
        image = Image.open(path).convert("RGBA")
        thumb = fit_image_contained(image, (210, 96))
        sheet.alpha_composite(thumb, (x + (210 - thumb.width) // 2, y))
        draw.text((x, y + 104), str(item["variant_id"]), fill=(232, 232, 224, 255), font=font(16))
        draw.text((x, y + 126), str(item["size"][0]) + "x" + str(item["size"][1]), fill=(148, 184, 206, 255), font=font(14))
    return sheet.convert("RGB")


def build_review_sheet(generated: list[dict[str, object]]) -> Image.Image:
    w, h = 1720, 1420
    sheet = Image.new("RGBA", (w, h), (11, 14, 28, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((46, 44), "Batch 86 Dream Route Preflight Review", fill=(236, 206, 116, 255), font=font(34))
    draw.text((46, 92), "Candidate-only route screen sprites and local mockups. No Unity import approval.", fill=(236, 236, 226, 255), font=font(20))
    card_w, card_h = 786, 210
    for index, item in enumerate(generated):
        col = index % 2
        row = index // 2
        x = 46 + col * (card_w + 58)
        y = 144 + row * (card_h + 20)
        rounded_frame(draw, x, y, x + card_w, y + card_h, 18, (39, 48, 78, 255), (118, 214, 240, 255), 2)
        path = item["path"]
        assert isinstance(path, Path)
        thumb = fit_image_contained(Image.open(path).convert("RGBA"), (250, 118))
        sheet.alpha_composite(thumb, (x + 24, y + 24))
        tx = x + 300
        draw.text((tx, y + 34), str(item["variant_id"]), fill=(236, 236, 226, 255), font=font(23))
        draw.text((tx, y + 70), str(item["visual_review"]), fill=(170, 182, 202, 255), font=font(17))
        draw.text((tx, y + 128), "Gate: dream-route screenshots, text replacement,", fill=(225, 190, 82, 255), font=font(16))
        draw.text((tx, y + 152), "node/path semantics, import settings, binding, Console.", fill=(225, 190, 82, 255), font=font(16))
    return sheet.convert("RGB")


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
        "# Batch 86 Dream Route Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers dream entry / route-map screen compositions and route-choice cards.",
        "- Provides textless panel, header, node socket, choice card, path ribbon, and boss gate sprites for later Unity import testing.",
        "- Reuses existing P0 UI shell, route node icons, route card frames, and Batch 65 route readability accents without altering them.",
        "- Does not generate, crop, recolor, or import starter-cat body art.",
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
            "- Dream-entry and route-map screenshots at target resolutions.",
            "- Unity-rendered Chinese title, route labels, node labels, card labels, and route rewards; no baked Chinese text in sprites.",
            "- Current, selected, available, locked, and Boss-pressure route states must remain distinct at 1024x768.",
            "- Route-choice cards must not occlude path connectors or node intent.",
            "- Sprite import settings, scene/prefab binding proof, pointer/click target proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note() -> None:
    PROCESS_NOTE_PATH.write_text(
        "\n".join(
            [
                "# Batch 86 Dream Route Process Note",
                "",
                "- Lane: `dream_route` / route-map screen-level preflight.",
                "- Source truth: Qr1 UI/style; local route icons, route card frames, and Batch 65 route-map readability accents.",
                "- Generation mode: deterministic local derivative from existing raster assets; not image2 provenance.",
                "- Reason: `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.",
                "- Candidate boundary: all PNGs stay under `design/development/asset_candidates`; do not import into `Assets` before Unity review.",
                "- Text rule: no baked Chinese text; Unity-rendered text replacement remains required.",
                "- Starter-cat rule: no body, pose, costume, color, or framesheet generation is included.",
                "- Runtime gate: Unity-rendered dream-route screenshots, node/path semantics, route-card click targets, import settings, binding, and Console.",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_agent_prompt() -> None:
    AGENT_PROMPT_PATH.write_text(
        "\n".join(
            [
                "# Agent Prompt - Batch 86 Dream Route Preflight",
                "",
                "Review this candidate-only route screen packet before Unity import.",
                "",
                "Check visual consistency with Qr1 UI/style, current route node icons, route card frames, and Batch 65 route-map readability accents.",
                "Confirm there is no baked Chinese text, no starter-cat body art, no `Assets` writes, and no Unity `.meta` files.",
                "Pay special attention to 1024x768 route-node/path/card crowding and Boss-pressure semantics.",
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
