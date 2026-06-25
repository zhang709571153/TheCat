from __future__ import annotations

import csv
import hashlib
import math
from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_84_result_settlement_preflight_2026-06-25"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "result_settlement" / BATCH_SLUG
SPRITE_DIR = CANDIDATE_DIR / "sprites"
MOCKUP_DIR = CANDIDATE_DIR / "mockups"

BG_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Backgrounds" / "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png"
PANEL_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_panel_dreamglass_512x256_v001.png"
BUTTON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Frames" / "thecat_ui_button_primary_384x96_v001.png"
VICTORY_BANNER_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Banners" / "thecat_ui_battle_result_victory_banner_512x160_v001.png"
DEFEAT_BANNER_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Banners" / "thecat_ui_battle_result_defeat_banner_512x160_v001.png"
CLEARED_BANNER_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Banners" / "thecat_ui_settlement_run_cleared_banner_512x160_v001.png"
FAILED_BANNER_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Banners" / "thecat_ui_settlement_run_failed_banner_512x160_v001.png"
DREAMSHARD_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons" / "thecat_ui_reward_dreamshard_icon_128_v001.png"
FISHTREAT_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons" / "thecat_ui_reward_fishtreat_icon_128_v001.png"
SLEEP_ICON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons" / "thecat_ui_core_sleep_icon_64_v001.png"
HP_ICON_SOURCE = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons" / "thecat_ui_core_hp_icon_64_v001.png"

MANIFEST_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_manifest.csv"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_review_sheet_v001.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_process_note.md"
SPEC_PATH = CANDIDATE_DIR / "thecat_ui_result_settlement_batch84_agent_review_prompt.md"

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
    ("result_settlement_panel", "panel_frame", (960, 540), "panel", "large textless result/settlement panel frame"),
    ("result_settlement_reward_row", "reward_row_frame", (760, 96), "reward_row", "textless reward row for icon plus Unity-rendered labels"),
    ("result_settlement_stat_chip", "stat_chip_frame", (256, 72), "stat_chip", "compact stat chip frame"),
    ("result_settlement_action_button", "action_button_frame", (384, 96), "button", "textless action button frame"),
    ("result_settlement_divider", "outcome_divider", (640, 32), "divider", "thin outcome section divider"),
    ("result_settlement_stamp", "success_stamp_ring", (256, 256), "success_stamp", "symbolic success outcome stamp ring"),
    ("result_settlement_stamp", "failure_stamp_ring", (256, 256), "failure_stamp", "symbolic failure outcome stamp ring"),
)

MOCKUP_SPECS = (
    ("result_screen", "battle_victory_1920x1080", (1920, 1080), "victory", "battle victory result screen local mockup"),
    ("result_screen", "battle_defeat_1920x1080", (1920, 1080), "defeat", "battle defeat result screen local mockup"),
    ("settlement_screen", "run_cleared_1365x768", (1365, 768), "cleared", "run cleared settlement screen local mockup"),
    ("settlement_screen", "run_failed_1024x768", (1024, 768), "failed", "run failed settlement screen local mockup"),
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    SPRITE_DIR.mkdir(parents=True, exist_ok=True)
    MOCKUP_DIR.mkdir(parents=True, exist_ok=True)
    clean_previous_outputs()

    sources = {
        "background": Image.open(BG_SOURCE).convert("RGBA"),
        "panel": Image.open(PANEL_SOURCE).convert("RGBA"),
        "button": Image.open(BUTTON_SOURCE).convert("RGBA"),
        "victory_banner": Image.open(VICTORY_BANNER_SOURCE).convert("RGBA"),
        "defeat_banner": Image.open(DEFEAT_BANNER_SOURCE).convert("RGBA"),
        "cleared_banner": Image.open(CLEARED_BANNER_SOURCE).convert("RGBA"),
        "failed_banner": Image.open(FAILED_BANNER_SOURCE).convert("RGBA"),
        "dreamshard": Image.open(DREAMSHARD_SOURCE).convert("RGBA"),
        "fishtreat": Image.open(FISHTREAT_SOURCE).convert("RGBA"),
        "sleep_icon": Image.open(SLEEP_ICON_SOURCE).convert("RGBA"),
        "hp_icon": Image.open(HP_ICON_SOURCE).convert("RGBA"),
    }

    generated: list[dict[str, object]] = []
    for component_id, variant_id, size, kind, visual_review in SPRITE_SPECS:
        sprite = build_sprite(sources, kind, size)
        asset_id = f"thecat_ui_result_settlement_{variant_id}_{size[0]}x{size[1]}_candidate_v001"
        path = SPRITE_DIR / f"{asset_id}.png"
        sprite.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "sprite", path, visual_review))

    sprite_lookup = {
        row["variant_id"]: Image.open(row["path"]).convert("RGBA")
        for row in generated
        if row["asset_type"] == "sprite"
    }
    for component_id, variant_id, size, scenario, visual_review in MOCKUP_SPECS:
        mockup = build_mockup(sources, sprite_lookup, size, scenario)
        asset_id = f"thecat_ui_result_settlement_{variant_id}_local_mockup_v001"
        path = MOCKUP_DIR / f"{asset_id}.png"
        mockup.save(path)
        generated.append(make_generated_row(asset_id, component_id, variant_id, size, "local_mockup", path, visual_review))

    write_contact_sheet(generated)
    write_review_sheet(generated)
    rows = build_manifest_rows(generated)
    write_manifest(rows)
    write_review_note(rows)
    write_process_note(rows)
    write_spec_note()

    print(f"Wrote {len(rows)} Batch 84 result/settlement preflight row(s).")
    print(to_repo_path(MANIFEST_PATH))


def clean_previous_outputs() -> None:
    if CANDIDATE_DIR.name != BATCH_SLUG or SPRITE_DIR.parent != CANDIDATE_DIR or MOCKUP_DIR.parent != CANDIDATE_DIR:
        raise RuntimeError("Refusing to clean unexpected Batch 84 output paths.")

    for folder in (SPRITE_DIR, MOCKUP_DIR):
        for path in folder.glob("*.png"):
            path.unlink()

    for path in (MANIFEST_PATH, CONTACT_SHEET_PATH, REVIEW_SHEET_PATH, REVIEW_NOTE_PATH, PROCESS_NOTE_PATH, SPEC_PATH):
        if not is_inside(path, CANDIDATE_DIR):
            raise RuntimeError(f"Refusing to clean Batch 84 path outside candidate directory: {path}")
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
    if kind == "panel":
        base = nine_slice_resize(sources["panel"], size, (78, 62, 78, 62))
        overlay = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(overlay)
        draw.rounded_rectangle((28, 28, size[0] - 29, size[1] - 29), radius=34, outline=(239, 204, 110, 92), width=3)
        draw.line((76, 120, size[0] - 76, 120), fill=(132, 198, 226, 86), width=3)
        draw.line((76, size[1] - 110, size[0] - 76, size[1] - 110), fill=(132, 198, 226, 62), width=3)
        return Image.alpha_composite(base, overlay)

    if kind == "reward_row":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((5, 5, size[0] - 6, size[1] - 6), radius=22, fill=(49, 57, 86, 220), outline=(132, 198, 226, 210), width=3)
        draw.rounded_rectangle((18, 18, 90, size[1] - 18), radius=16, fill=(31, 41, 66, 210), outline=(239, 204, 110, 170), width=2)
        draw.line((118, 30, size[0] - 48, 30), fill=(243, 224, 165, 95), width=4)
        draw.line((118, 56, int(size[0] * 0.66), 56), fill=(132, 198, 226, 82), width=4)
        draw.line((size[0] - 182, 56, size[0] - 48, 56), fill=(239, 204, 110, 110), width=4)
        return image

    if kind == "stat_chip":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        draw.rounded_rectangle((5, 5, size[0] - 6, size[1] - 6), radius=24, fill=(36, 44, 72, 224), outline=(239, 204, 110, 210), width=3)
        draw.ellipse((20, 17, 58, 55), fill=(132, 198, 226, 180))
        draw.line((78, 26, size[0] - 30, 26), fill=(246, 238, 214, 118), width=4)
        draw.line((78, 48, size[0] - 72, 48), fill=(132, 198, 226, 88), width=4)
        return image

    if kind == "button":
        base = fit_image(sources["button"], size)
        glow = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(glow)
        draw.rounded_rectangle((8, 8, size[0] - 9, size[1] - 9), radius=34, outline=(246, 238, 214, 54), width=2)
        return Image.alpha_composite(base, glow)

    if kind == "divider":
        image = Image.new("RGBA", size, (0, 0, 0, 0))
        draw = ImageDraw.Draw(image)
        mid = size[1] // 2
        draw.line((0, mid, size[0], mid), fill=(132, 198, 226, 120), width=2)
        for x in range(72, size[0], 124):
            draw.ellipse((x - 6, mid - 6, x + 6, mid + 6), fill=(239, 204, 110, 200))
            draw.arc((x - 24, mid - 14, x + 24, mid + 14), 190, 350, fill=(139, 109, 202, 150), width=2)
        return image

    if kind in ("success_stamp", "failure_stamp"):
        return build_stamp(size, success=(kind == "success_stamp"))

    raise ValueError(f"Unhandled sprite kind: {kind}")


def build_mockup(
    sources: dict[str, Image.Image],
    sprites: dict[str, Image.Image],
    size: tuple[int, int],
    scenario: str,
) -> Image.Image:
    w, h = size
    palette = get_scenario_palette(scenario)
    bg = cover_image(sources["background"], size)
    dim = Image.new("RGBA", size, palette["bg_overlay"])
    image = Image.alpha_composite(bg, dim)

    panel_width = min(int(w * 0.72), 1160)
    panel_height = min(int(h * 0.72), 710)
    panel = fit_image(sprites["panel_frame"], (panel_width, panel_height))
    panel_x = (w - panel.width) // 2
    panel_y = max(42, int(h * 0.13))
    image.alpha_composite(panel, (panel_x, panel_y))

    banner_key = {
        "victory": "victory_banner",
        "defeat": "defeat_banner",
        "cleared": "cleared_banner",
        "failed": "failed_banner",
    }[scenario]
    banner = fit_image(sources[banner_key], (min(panel_width - 120, 620), int((panel_width - 120) * 0.3125)))
    banner_y = panel_y + max(30, int(panel_height * 0.06))
    image.alpha_composite(banner, ((w - banner.width) // 2, banner_y))

    stamp_variant = "success_stamp_ring" if scenario in ("victory", "cleared") else "failure_stamp_ring"
    stamp = tint_alpha(fit_image(sprites[stamp_variant], (min(172, panel_width // 6), min(172, panel_width // 6))), palette["stamp_tint"])
    stamp_x = panel_x + panel_width - stamp.width - 74
    stamp_y = banner_y + banner.height // 2 - stamp.height // 2
    if panel_width < 760:
        stamp_x = panel_x + panel_width - stamp.width - 34
        stamp_y = banner_y + banner.height + 8
    image.alpha_composite(stamp, (stamp_x, stamp_y))

    divider = tint_alpha(fit_image(sprites["outcome_divider"], (min(panel_width - 160, 720), 32)), palette["divider_tint"])
    divider_y = banner_y + banner.height + max(28, int(panel_height * 0.045))
    image.alpha_composite(divider, ((w - divider.width) // 2, divider_y))

    row_width = min(panel_width - 150, 760)
    row_height = max(78, min(96, int(h * 0.09)))
    row = fit_image(sprites["reward_row_frame"], (row_width, row_height))
    row_start_y = divider_y + 52
    reward_icons = [sources["dreamshard"], sources["fishtreat"], sources["sleep_icon"]]
    if scenario in ("defeat", "failed"):
        reward_icons = [sources["hp_icon"], sources["sleep_icon"], sources["dreamshard"]]
    row_count = 3 if panel_height > 520 else 2
    for index in range(row_count):
        ry = row_start_y + index * (row_height + 16)
        if ry + row_height > panel_y + panel_height - 144:
            break
        image.alpha_composite(row, ((w - row_width) // 2, ry))
        icon = fit_image(reward_icons[index % len(reward_icons)], (52, 52))
        image.alpha_composite(icon, ((w - row_width) // 2 + 28, ry + (row_height - icon.height) // 2))
        draw_placeholder_text(image, (w - row_width) // 2 + 112, ry + 27, row_width - 310, palette["line"])
        draw_placeholder_text(image, (w - row_width) // 2 + row_width - 164, ry + 37, 104, palette["accent"], thick=True)

    chip = fit_image(sprites["stat_chip_frame"], (min(236, panel_width // 4), 68))
    chip_y = panel_y + panel_height - 132
    chip_gap = max(16, int(w * 0.018))
    chip_total = chip.width * 3 + chip_gap * 2
    chip_x = (w - chip_total) // 2
    for index in range(3):
        tinted = tint_alpha(chip, palette["chip_tints"][index])
        image.alpha_composite(tinted, (chip_x + index * (chip.width + chip_gap), chip_y))

    button_width = min(300, max(210, int(w * 0.18)))
    button = tint_alpha(fit_image(sprites["action_button_frame"], (button_width, int(button_width * 0.25))), palette["button_tint"])
    button_y = panel_y + panel_height + max(22, int(h * 0.025))
    if button_y + button.height > h - 28:
        button_y = h - button.height - 28
    image.alpha_composite(button, (w // 2 - button.width - 16, button_y))
    image.alpha_composite(button, (w // 2 + 16, button_y))
    draw_button_placeholder(image, w // 2 - button.width - 16, button_y, button.width, button.height)
    draw_button_placeholder(image, w // 2 + 16, button_y, button.width, button.height)

    draw_safe_area(image)
    return image


def get_scenario_palette(scenario: str) -> dict[str, object]:
    success = scenario in ("victory", "cleared")
    if success:
        return {
            "bg_overlay": (6, 12, 30, 90),
            "stamp_tint": (154, 214, 238, 255),
            "divider_tint": (154, 214, 238, 255),
            "line": (224, 238, 246, 168),
            "accent": (239, 204, 110, 188),
            "button_tint": (142, 198, 228, 255),
            "chip_tints": ((130, 198, 228, 255), (239, 204, 110, 255), (166, 126, 206, 255)),
        }
    return {
        "bg_overlay": (16, 8, 28, 114),
        "stamp_tint": (234, 92, 115, 255),
        "divider_tint": (166, 126, 206, 255),
        "line": (236, 210, 226, 160),
        "accent": (234, 92, 115, 190),
        "button_tint": (166, 126, 206, 255),
        "chip_tints": ((234, 92, 115, 255), (166, 126, 206, 255), (239, 204, 110, 255)),
    }


def build_stamp(size: tuple[int, int], success: bool) -> Image.Image:
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    cx = size[0] // 2
    cy = size[1] // 2
    outer = (239, 204, 110, 218) if success else (234, 92, 115, 220)
    inner = (132, 198, 226, 190) if success else (166, 126, 206, 190)
    mark = (132, 198, 226, 210) if success else (234, 92, 115, 220)

    draw.ellipse((30, 30, size[0] - 30, size[1] - 30), outline=outer, width=8)
    draw.arc((52, 52, size[0] - 52, size[1] - 52), 30, 260, fill=inner, width=6)
    if success:
        draw.line((cx - 55, cy + 2, cx - 16, cy + 40), fill=mark, width=10)
        draw.line((cx - 18, cy + 38, cx + 62, cy - 54), fill=mark, width=10)
    else:
        draw.line((cx - 48, cy - 48, cx + 48, cy + 48), fill=mark, width=11)
        draw.line((cx + 48, cy - 48, cx - 48, cy + 48), fill=mark, width=11)

    for angle in (18, 118, 238):
        x = cx + math.cos(math.radians(angle)) * 86
        y = cy + math.sin(math.radians(angle)) * 86
        draw_star(draw, x, y, 13, outer)
    return image.filter(ImageFilter.UnsharpMask(radius=1.1, percent=80, threshold=3))


def draw_placeholder_text(image: Image.Image, x: int, y: int, width: int, color: tuple[int, int, int, int], thick: bool = False) -> None:
    draw = ImageDraw.Draw(image)
    height = 5 if thick else 4
    draw.rounded_rectangle((x, y, x + max(40, width), y + height), radius=height // 2, fill=color)
    if not thick:
        draw.rounded_rectangle((x, y + 18, x + max(28, width // 2), y + 18 + height), radius=height // 2, fill=(color[0], color[1], color[2], max(52, color[3] // 2)))


def draw_button_placeholder(image: Image.Image, x: int, y: int, width: int, height: int) -> None:
    draw = ImageDraw.Draw(image)
    line_y = y + height // 2
    draw.rounded_rectangle((x + width * 0.22, line_y - 3, x + width * 0.78, line_y + 3), radius=3, fill=(246, 238, 214, 150))


def draw_safe_area(image: Image.Image) -> None:
    draw = ImageDraw.Draw(image)
    w, h = image.size
    margin_x = max(24, int(w * 0.035))
    margin_y = max(18, int(h * 0.035))
    draw.rounded_rectangle(
        (margin_x, margin_y, w - margin_x, h - margin_y),
        radius=18,
        outline=(246, 238, 214, 28),
        width=2,
    )


def draw_star(draw: ImageDraw.ImageDraw, x: float, y: float, radius: float, color: tuple[int, int, int, int]) -> None:
    points: list[tuple[float, float]] = []
    for i in range(10):
        angle = math.radians(-90 + i * 36)
        r = radius if i % 2 == 0 else radius * 0.42
        points.append((x + math.cos(angle) * r, y + math.sin(angle) * r))
    draw.polygon(points, fill=color)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    return image.resize(size, Image.Resampling.LANCZOS)


def cover_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    w, h = size
    scale = max(w / image.width, h / image.height)
    resized = image.resize((int(image.width * scale), int(image.height * scale)), Image.Resampling.LANCZOS)
    x = (resized.width - w) // 2
    y = (resized.height - h) // 2
    return resized.crop((x, y, x + w, y + h))


def tint_alpha(image: Image.Image, tint: tuple[int, int, int, int]) -> Image.Image:
    r, g, b, a = tint
    color = Image.new("RGBA", image.size, (r, g, b, 0))
    alpha = image.getchannel("A")
    lum = image.convert("L")
    color.putalpha(Image.eval(alpha, lambda px: int(px * a / 255)))
    shaded = Image.composite(Image.new("RGBA", image.size, (r, g, b, a)), image, lum)
    shaded.putalpha(color.getchannel("A"))
    return Image.alpha_composite(image, shaded)


def nine_slice_resize(
    image: Image.Image,
    size: tuple[int, int],
    borders: tuple[int, int, int, int],
) -> Image.Image:
    left, top, right, bottom = borders
    source_w, source_h = image.size
    target_w, target_h = size
    result = Image.new("RGBA", size, (0, 0, 0, 0))

    x_segments = (
        (0, left, 0, left),
        (left, source_w - right, left, target_w - right),
        (source_w - right, source_w, target_w - right, target_w),
    )
    y_segments = (
        (0, top, 0, top),
        (top, source_h - bottom, top, target_h - bottom),
        (source_h - bottom, source_h, target_h - bottom, target_h),
    )

    for sx0, sx1, tx0, tx1 in x_segments:
        for sy0, sy1, ty0, ty1 in y_segments:
            if sx1 <= sx0 or sy1 <= sy0 or tx1 <= tx0 or ty1 <= ty0:
                continue
            crop = image.crop((sx0, sy0, sx1, sy1)).resize((tx1 - tx0, ty1 - ty0), Image.Resampling.LANCZOS)
            result.alpha_composite(crop, (tx0, ty0))

    return result


def build_manifest_rows(generated: list[dict[str, object]]) -> list[dict[str, str]]:
    source_assets = [
        BG_SOURCE,
        PANEL_SOURCE,
        BUTTON_SOURCE,
        VICTORY_BANNER_SOURCE,
        DEFEAT_BANNER_SOURCE,
        CLEARED_BANNER_SOURCE,
        FAILED_BANNER_SOURCE,
        DREAMSHARD_SOURCE,
        FISHTREAT_SOURCE,
        SLEEP_ICON_SOURCE,
        HP_ICON_SOURCE,
    ]
    source_asset_string = ";".join(to_repo_path(path) for path in source_assets)
    source_hash_string = ";".join(sha256(path) for path in source_assets)

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
                "source_model": "deterministic_local_derivative_not_image2",
                "recommendation": "candidate_only_pending_unity_result_settlement_screenshots",
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
    draw.text((24, 16), "Batch 84 result/settlement preflight candidates", fill=(239, 204, 110, 255), font=font(20, bold=True))

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
    sheet = Image.new("RGBA", (1800, 1700), (15, 18, 32, 255))
    draw = ImageDraw.Draw(sheet)
    draw.text((48, 42), "Batch 84 Result / Settlement Preflight Review", fill=(239, 204, 110, 255), font=font(34, bold=True))
    draw.text((48, 90), "Candidate-only, textless UI sprites plus local screen mockups. No Unity import approval.", fill=(246, 238, 214, 220), font=font(20))

    card_w = 820
    card_h = 220
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
        draw.text((text_x, y + 126), "Gate: Unity screenshots, placeholder replacement,", fill=(239, 204, 110, 230), font=font(15))
        draw.text((text_x, y + 150), "import settings, binding, Console.", fill=(239, 204, 110, 230), font=font(15))
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 84 Result / Settlement Preflight Candidate Review",
        "",
        "Result: local candidate packet generated; not Unity accepted.",
        "",
        "## Scope",
        "",
        "- Covers battle victory, battle defeat, run-cleared settlement, and run-failed settlement local screen compositions.",
        "- Provides textless panel, reward-row, stat-chip, action-button, divider, and stamp sprites for later Unity import testing.",
        "- Reuses existing P0 UI shell/banner/reward sources from `Assets/TheCat/Art/UI` without altering them.",
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
            "- Result/settlement screen screenshots at target resolutions.",
            "- Unity-rendered Chinese title/button/reward text replacement proof; no baked Chinese text in sprites.",
            "- Reward rows and stat chips must remain readable on 1024x768.",
            "- Sprite import settings, scene/prefab binding proof, and clean Console.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 84 Result / Settlement Preflight Process Note",
        "",
        "Status: `candidate_only_pending_unity_result_settlement_screenshots`",
        "",
        "Authority: `Qr1` UI/style source truth plus existing local P0 UI asset inventory.",
        "",
        "This is a deterministic local derivative packet, not image2 provenance. It uses existing UI shell, banner, reward, and icon assets as source material.",
        "",
        "Controls:",
        "",
        "- candidate-only",
        "- no baked Chinese text",
        "- no `.meta` files",
        "- no writes under `Assets`",
        "- not image2 provenance",
        "- Unity-rendered result/settlement screenshots remain required",
        "",
        "Rows:",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['variant_id']}` -> `{row['candidate_path']}`")
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_spec_note() -> None:
    lines = [
        "# Batch 84 Agent Review Prompt",
        "",
        "Review the Batch 84 result/settlement preflight candidate packet.",
        "",
        "Primary checks:",
        "",
        "- Match `Qr1` UI/style tone: dark dreamglass panels, cyan/gold accent language, red/purple failure language.",
        "- Confirm sprites remain textless and candidate-only.",
        "- Confirm screen mockups show usable composition for victory, defeat, run cleared, and run failed states.",
        "- Check 1024x768 density and reward-row/stat-chip readability risk.",
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
