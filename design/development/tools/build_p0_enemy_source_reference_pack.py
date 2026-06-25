from __future__ import annotations

import csv
import hashlib
import math
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_38_p0_enemy_source_reference_pack_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "p0_enemy_batch38_source_reference_manifest.csv"
SUMMARY_PATH = BATCH_DIR / "p0_enemy_batch38_source_reference_summary.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_enemy_p0_core_batch38_source_reference_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "p0_enemy_batch38_source_reference_candidate_review.md"


@dataclass(frozen=True)
class EnemySpec:
    enemy_id: str
    display_name: str
    combat_role: str
    concept_pattern: str
    animation_pattern: str
    concept_lock_id: str
    animation_lock_id: str
    active_screenshot: str
    required_traits: tuple[str, ...]
    rejection_rules: tuple[str, ...]


ENEMIES: tuple[EnemySpec, ...] = (
    EnemySpec(
        enemy_id="black_mud_nightmare",
        display_name="Black Mud Nightmare",
        combat_role="P0 melee bed-pressure swarmer",
        concept_pattern="design/*/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png",
        animation_pattern="design/*/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png",
        concept_lock_id="black_mud_concept",
        animation_lock_id="black_mud_animation",
        active_screenshot="07-active-enemy-black-mud.png",
        required_traits=(
            "black sludge body",
            "red eyes",
            "soft-mud monster silhouette",
            "crawling pressure",
            "bed-contact threat",
        ),
        rejection_rules=(
            "Reject cute pet styling.",
            "Reject generic ghost shape without mud volume.",
            "Reject loss of red eye threat read.",
        ),
    ),
    EnemySpec(
        enemy_id="cold_light_shadow",
        display_name="Cold Light Shadow",
        combat_role="P0 ranged harasser and bed beam pressure",
        concept_pattern="design/*/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png",
        animation_pattern="design/*/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png",
        concept_lock_id="cold_light_concept",
        animation_lock_id="cold_light_animation",
        active_screenshot="08-active-enemy-cold-light.png",
        required_traits=(
            "cold lamp silhouette",
            "cyan beam language",
            "mechanical arm",
            "black mud base",
            "single red eye",
        ),
        rejection_rules=(
            "Reject ordinary desk lamp without dream shadow body.",
            "Reject warm candle/fire palette.",
            "Reject missing ranged beam cue.",
        ),
    ),
    EnemySpec(
        enemy_id="call_tyrant",
        display_name="Call Tyrant",
        combat_role="P0 boss summon and app-throw pressure",
        concept_pattern="design/*/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png",
        animation_pattern="design/*/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png",
        concept_lock_id="call_tyrant_concept",
        animation_lock_id="call_tyrant_animation",
        active_screenshot="09-active-enemy-call-tyrant.png",
        required_traits=(
            "giant phone shell",
            "red call eyes",
            "purple tie",
            "black mud body",
            "app projectile language",
            "summon portal vibration",
        ),
        rejection_rules=(
            "Reject human office boss body.",
            "Reject generic phone app icon without monster body.",
            "Reject missing purple tie or red call-eye signal.",
        ),
    ),
)


ASSET_TYPES: tuple[tuple[str, tuple[int, int], str], ...] = (
    ("concept_reference_512", (512, 512), "Full concept reference fitted to a square review canvas."),
    ("animation_reference_512", (512, 512), "Full animation source reference fitted to a square review canvas."),
    ("combat_sprite_reference_512", (512, 512), "Auto-cropped concept silhouette for future combat sprite review."),
    ("warning_motif_reference_256", (256, 256), "Auto-cropped animation motif for warning and behavior VFX review."),
    ("palette_swatch_reference_256", (256, 256), "Palette swatches sampled from concept and animation source images."),
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    produced: dict[tuple[str, str], Path] = {}
    source_images: dict[tuple[str, str], Image.Image] = {}
    source_paths: dict[tuple[str, str], Path] = {}

    for enemy in ENEMIES:
        enemy_dir = CANDIDATE_DIR / enemy.enemy_id
        enemy_dir.mkdir(parents=True, exist_ok=True)

        concept_path = resolve_one(enemy.concept_pattern)
        animation_path = resolve_one(enemy.animation_pattern)
        concept = Image.open(concept_path).convert("RGBA")
        animation = Image.open(animation_path).convert("RGBA")
        source_images[(enemy.enemy_id, "concept")] = concept
        source_images[(enemy.enemy_id, "animation")] = animation
        source_paths[(enemy.enemy_id, "concept")] = concept_path
        source_paths[(enemy.enemy_id, "animation")] = animation_path

        for asset_type, size, note in ASSET_TYPES:
            output_path = enemy_dir / file_name(enemy.enemy_id, asset_type, size)
            if asset_type == "concept_reference_512":
                image = fit_to_canvas(concept.copy(), size, (248, 244, 236, 255))
                crop_box = "full_concept"
                source_lock_ids = enemy.concept_lock_id
            elif asset_type == "animation_reference_512":
                image = fit_to_canvas(animation.copy(), size, (248, 244, 236, 255))
                crop_box = "full_animation"
                source_lock_ids = enemy.animation_lock_id
            elif asset_type == "combat_sprite_reference_512":
                crop_box_tuple = find_visible_bounds(concept)
                image = fit_to_canvas(concept.crop(crop_box_tuple), size, (248, 244, 236, 255))
                crop_box = format_crop(crop_box_tuple)
                source_lock_ids = enemy.concept_lock_id
            elif asset_type == "warning_motif_reference_256":
                crop_box_tuple = find_visible_bounds(animation)
                image = fit_to_canvas(animation.crop(crop_box_tuple), size, (248, 244, 236, 255))
                crop_box = format_crop(crop_box_tuple)
                source_lock_ids = enemy.animation_lock_id
            elif asset_type == "palette_swatch_reference_256":
                image = build_palette_swatch(enemy, concept, animation, size)
                crop_box = "sampled_palette"
                source_lock_ids = enemy.concept_lock_id + ";" + enemy.animation_lock_id
            else:
                raise ValueError(asset_type)

            image.save(output_path)
            produced[(enemy.enemy_id, asset_type)] = output_path
            rows.append(
                {
                    "enemy_id": enemy.enemy_id,
                    "display_name": enemy.display_name,
                    "combat_role": enemy.combat_role,
                    "batch_slug": BATCH_SLUG,
                    "asset_type": asset_type,
                    "candidate_path": to_repo_path(output_path),
                    "candidate_sha256": sha256(output_path),
                    "candidate_size": f"{size[0]}x{size[1]}",
                    "concept_source_path": to_repo_path(concept_path),
                    "concept_source_sha256": sha256(concept_path),
                    "animation_source_path": to_repo_path(animation_path),
                    "animation_source_sha256": sha256(animation_path),
                    "source_lock_ids": source_lock_ids,
                    "source_crop_box": crop_box,
                    "active_screenshot": enemy.active_screenshot,
                    "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                    "review_note": to_repo_path(REVIEW_NOTE_PATH),
                    "recommendation": "candidate_review_only_pending_unity_import_gate",
                    "notes": note,
                }
            )

    write_review_sheet(source_images, produced)
    write_review_note(source_paths, produced)
    write_manifest(rows)
    write_summary(rows)
    print("Wrote P0 enemy source reference pack.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda item: item.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def file_name(enemy_id: str, asset_type: str, size: tuple[int, int]) -> str:
    width, height = size
    return f"thecat_enemy_{enemy_id}_batch38_{asset_type}_{width}x{height}_candidate_v001.png"


def fit_to_canvas(image: Image.Image, size: tuple[int, int], background: tuple[int, int, int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, background)
    image.thumbnail((size[0] - 28, size[1] - 28), Image.Resampling.LANCZOS)
    x = (size[0] - image.width) // 2
    y = (size[1] - image.height) // 2
    canvas.alpha_composite(image, (x, y))
    return canvas


def find_visible_bounds(image: Image.Image) -> tuple[int, int, int, int]:
    rgba = image.convert("RGBA")
    width, height = rgba.size
    bg = border_average(rgba)
    min_x = width
    min_y = height
    max_x = -1
    max_y = -1
    step = max(1, min(width, height) // 768)

    for y in range(0, height, step):
        for x in range(0, width, step):
            r, g, b, a = rgba.getpixel((x, y))
            if a <= 12:
                continue
            if is_foreground((r, g, b), bg):
                min_x = min(min_x, x)
                min_y = min(min_y, y)
                max_x = max(max_x, x)
                max_y = max(max_y, y)

    if max_x < min_x or max_y < min_y:
        return (0, 0, width, height)

    padding = max(24, int(min(width, height) * 0.04))
    min_x = max(0, min_x - padding)
    min_y = max(0, min_y - padding)
    max_x = min(width - 1, max_x + padding)
    max_y = min(height - 1, max_y + padding)
    return (min_x, min_y, max_x + 1, max_y + 1)


def border_average(image: Image.Image) -> tuple[int, int, int]:
    width, height = image.size
    points: list[tuple[int, int]] = []
    for x in range(0, width, max(1, width // 40)):
        points.append((x, 0))
        points.append((x, height - 1))
    for y in range(0, height, max(1, height // 40)):
        points.append((0, y))
        points.append((width - 1, y))

    pixels = [image.getpixel(point)[:3] for point in points]
    count = max(1, len(pixels))
    return (
        sum(pixel[0] for pixel in pixels) // count,
        sum(pixel[1] for pixel in pixels) // count,
        sum(pixel[2] for pixel in pixels) // count,
    )


def is_foreground(color: tuple[int, int, int], bg: tuple[int, int, int]) -> bool:
    distance = math.sqrt(sum((color[i] - bg[i]) ** 2 for i in range(3)))
    luminance = (color[0] * 0.2126) + (color[1] * 0.7152) + (color[2] * 0.0722)
    saturation = max(color) - min(color)
    return distance > 42 or luminance < 210 or saturation > 54


def build_palette_swatch(enemy: EnemySpec, concept: Image.Image, animation: Image.Image, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (248, 244, 236, 255))
    draw = ImageDraw.Draw(image)
    title_font = load_font(13)
    font = load_font(10)
    draw.text((12, 10), enemy.display_name, fill=(38, 34, 32), font=title_font)
    draw.text((12, 28), "source palette", fill=(72, 64, 58), font=font)

    swatches = extract_swatches(concept, animation, count=7)
    y = 52
    for index, color in enumerate(swatches):
        label = f"#{color[0]:02x}{color[1]:02x}{color[2]:02x}"
        draw.rounded_rectangle((14, y, 74, y + 22), radius=4, fill=color + (255,), outline=(52, 46, 42))
        draw.text((86, y + 5), f"{index + 1} {label}", fill=(38, 34, 32), font=font)
        y += 25

    draw.rectangle((0, size[1] - 8, size[0], size[1]), fill=swatches[0] + (255,))
    return image


def extract_swatches(concept: Image.Image, animation: Image.Image, count: int) -> list[tuple[int, int, int]]:
    samples: dict[tuple[int, int, int], int] = {}
    for source in (concept, animation):
        rgba = source.convert("RGBA")
        bg = border_average(rgba)
        width, height = rgba.size
        step = max(1, min(width, height) // 120)
        for y in range(0, height, step):
            for x in range(0, width, step):
                r, g, b, a = rgba.getpixel((x, y))
                if a <= 12 or not is_foreground((r, g, b), bg):
                    continue
                key = ((r // 24) * 24, (g // 24) * 24, (b // 24) * 24)
                samples[key] = samples.get(key, 0) + 1

    ranked = sorted(samples.items(), key=lambda item: (item[1], color_interest(item[0])), reverse=True)
    colors: list[tuple[int, int, int]] = []
    for color, _ in ranked:
        if all(color_distance(color, existing) > 36 for existing in colors):
            colors.append(color)
        if len(colors) >= count:
            break

    while len(colors) < count:
        colors.append((48, 42, 56))
    return colors


def color_interest(color: tuple[int, int, int]) -> float:
    luminance = (color[0] * 0.2126) + (color[1] * 0.7152) + (color[2] * 0.0722)
    saturation = max(color) - min(color)
    return saturation + abs(luminance - 128) * 0.2


def color_distance(a: tuple[int, int, int], b: tuple[int, int, int]) -> float:
    return math.sqrt(sum((a[i] - b[i]) ** 2 for i in range(3)))


def write_review_sheet(
    source_images: dict[tuple[str, str], Image.Image],
    produced: dict[tuple[str, str], Path],
) -> None:
    sheet = Image.new("RGBA", (2400, 1350), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    header_font = load_font(22)
    body_font = load_font(16)
    small_font = load_font(13)

    draw.text((44, 32), "P0 Batch 38 - Core Enemy Source Reference Pack", fill=(36, 32, 30), font=title_font)
    draw.text(
        (44, 78),
        "Candidate review only. Source-derived references for Black Mud, Cold Light, and Call Tyrant. No Unity import or .meta files.",
        fill=(100, 44, 42),
        font=body_font,
    )

    column_width = 740
    start_x = 44
    start_y = 130
    for index, enemy in enumerate(ENEMIES):
        x = start_x + index * (column_width + 36)
        draw.rounded_rectangle((x, start_y, x + column_width, 1260), radius=12, fill=(255, 252, 246), outline=(176, 158, 132), width=2)
        draw.text((x + 22, start_y + 20), enemy.display_name, fill=(36, 32, 30), font=header_font)
        draw.text((x + 22, start_y + 52), enemy.combat_role, fill=(78, 68, 62), font=small_font)

        concept = thumbnail(source_images[(enemy.enemy_id, "concept")].copy(), (330, 235))
        animation = thumbnail(source_images[(enemy.enemy_id, "animation")].copy(), (330, 235))
        sheet.alpha_composite(concept, (x + 24, start_y + 90))
        sheet.alpha_composite(animation, (x + 384, start_y + 90))
        draw.text((x + 24, start_y + 330), "source concept", fill=(36, 32, 30), font=small_font)
        draw.text((x + 384, start_y + 330), "source animation", fill=(36, 32, 30), font=small_font)

        panels = (
            ("combat crop", "combat_sprite_reference_512", (x + 24, start_y + 370), (208, 208)),
            ("warning motif", "warning_motif_reference_256", (x + 274, start_y + 370), (208, 208)),
            ("palette", "palette_swatch_reference_256", (x + 524, start_y + 370), (180, 180)),
        )
        for label, asset_type, pos, max_size in panels:
            panel = Image.open(produced[(enemy.enemy_id, asset_type)]).convert("RGBA")
            panel.thumbnail(max_size, Image.Resampling.LANCZOS)
            px, py = pos
            draw.rounded_rectangle((px - 10, py - 10, px + max_size[0] + 10, py + max_size[1] + 40), radius=8, fill=(248, 244, 236), outline=(178, 158, 130))
            sheet.alpha_composite(panel, (px + (max_size[0] - panel.width) // 2, py + (max_size[1] - panel.height) // 2))
            draw.text((px, py + max_size[1] + 12), label, fill=(36, 32, 30), font=small_font)

        y = start_y + 648
        draw.text((x + 24, y), "Required read", fill=(36, 32, 30), font=body_font)
        y += 28
        for trait in enemy.required_traits:
            draw.text((x + 34, y), "- " + trait, fill=(50, 44, 40), font=small_font)
            y += 23

        y += 12
        draw.text((x + 24, y), "Reject drift", fill=(36, 32, 30), font=body_font)
        y += 28
        for rule in enemy.rejection_rules:
            draw.text((x + 34, y), "- " + rule, fill=(92, 48, 42), font=small_font)
            y += 23

        y += 12
        draw.text((x + 24, y), "Unity gate", fill=(36, 32, 30), font=body_font)
        y += 28
        gate_lines = (
            f"Screenshot: {enemy.active_screenshot}",
            "Check Console, import settings, runtime scale, hitbox readability.",
            "Compare against source concept and animation before install.",
        )
        for line in gate_lines:
            draw.text((x + 34, y), "- " + line, fill=(50, 44, 40), font=small_font)
            y += 23

    REVIEW_SHEET_PATH.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(REVIEW_SHEET_PATH)


def thumbnail(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, (248, 244, 236, 255))
    image.thumbnail((size[0] - 10, size[1] - 10), Image.Resampling.LANCZOS)
    canvas.alpha_composite(image, ((size[0] - image.width) // 2, (size[1] - image.height) // 2))
    return canvas


def write_review_note(
    source_paths: dict[tuple[str, str], Path],
    produced: dict[tuple[str, str], Path],
) -> None:
    lines = [
        "# P0 Enemy Batch 38 Source Reference Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.",
        "",
        "## Output Policy",
        "",
        "- Candidate files stay under `design/development/asset_candidates/enemies`.",
        "- Candidate files stay outside `Assets`.",
        "- No Unity `.meta` files are created in this batch.",
        "- No runtime enemy sprite, VFX, framesheet, prefab, scene, or manifest count is changed.",
        "",
        "## Produced Assets",
        "",
    ]
    for enemy in ENEMIES:
        lines.extend(
            [
                f"### {enemy.display_name}",
                "",
                f"- Combat role: {enemy.combat_role}",
                f"- Source concept: `{to_repo_path(source_paths[(enemy.enemy_id, 'concept')])}`",
                f"- Source animation: `{to_repo_path(source_paths[(enemy.enemy_id, 'animation')])}`",
                f"- Source locks: `{enemy.concept_lock_id}`, `{enemy.animation_lock_id}`",
                f"- Required active screenshot: `{enemy.active_screenshot}`",
                "- Required traits: " + "; ".join(enemy.required_traits),
                "- Rejection rules: " + " ".join(enemy.rejection_rules),
                "",
            ]
        )
        for asset_type, _, _ in ASSET_TYPES:
            lines.append(f"- `{asset_type}`: `{to_repo_path(produced[(enemy.enemy_id, asset_type)])}`")
        lines.append("")

    lines.extend(
        [
            "## Review Sheet",
            "",
            f"- `{to_repo_path(REVIEW_SHEET_PATH)}`",
            "",
            "## Next Gate",
            "",
            "Use Unity MCP/editor to refresh the AssetDatabase only after a separate import batch exists.",
            "Before runtime installation, capture active-enemy screenshots for all three P0 core enemies and compare against this review sheet.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_summary(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Enemy Batch 38 Source Reference Summary",
        "",
        f"- Batch slug: `{BATCH_SLUG}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Review note: `{to_repo_path(REVIEW_NOTE_PATH)}`",
        f"- Rows: {len(rows)}",
        "- Status: candidate review only; Unity import blocked.",
        "",
        "## Enemy Coverage",
        "",
    ]
    for enemy in ENEMIES:
        lines.append(f"- `{enemy.enemy_id}`: concept, animation, combat crop, warning motif, palette.")
    SUMMARY_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def format_crop(crop: tuple[int, int, int, int]) -> str:
    return f"{crop[0]},{crop[1]},{crop[2]},{crop[3]}"


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(65536), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def load_font(size: int) -> ImageFont.ImageFont:
    candidates = (
        Path("C:/Windows/Fonts/msyh.ttc"),
        Path("C:/Windows/Fonts/arial.ttf"),
        Path("C:/Windows/Fonts/segoeui.ttf"),
    )
    for path in candidates:
        if path.exists():
            return ImageFont.truetype(str(path), size)
    return ImageFont.load_default()


if __name__ == "__main__":
    main()
