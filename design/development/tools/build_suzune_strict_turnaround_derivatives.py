from __future__ import annotations

import csv
import hashlib
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_33_suzune_strict_turnaround_derivatives_2026-06-14"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "suzune" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "suzune_batch33_strict_turnaround_manifest.csv"
SOURCE_PATTERN = "design/*/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
SOURCE_LOCK_ID = "suzune_turnaround_colored"
ACTIVE_SCREENSHOT = "06-active-cat-suzune.png"


@dataclass(frozen=True)
class CandidateAsset:
    asset_type: str
    file_name: str
    size: tuple[int, int]
    crop_box: tuple[int, int, int, int] | None
    note: str


CANDIDATES: tuple[CandidateAsset, ...] = (
    CandidateAsset(
        "front_view_reference_512",
        "thecat_cat_suzune_front_view_source_reference_512_candidate_v001.png",
        (512, 512),
        (18, 54, 610, 870),
        "Front view reference crop: calico face patches, blue eyes, flower ornament, shrine robe, central bell, and bell wand.",
    ),
    CandidateAsset(
        "side_view_reference_512",
        "thecat_cat_suzune_side_view_source_reference_512_candidate_v001.png",
        (512, 512),
        (590, 54, 1135, 870),
        "Side view reference crop: calico cheek/ear patches, sleeve snowflake, red stitch trim, bell wand, ribbons, and tail.",
    ),
    CandidateAsset(
        "back_view_reference_512",
        "thecat_cat_suzune_back_view_source_reference_512_candidate_v001.png",
        (512, 512),
        (1190, 54, 1768, 870),
        "Back view reference crop: calico head patches, large vermilion bow, gold bell, sleeve snowflakes, tail, and bell wand.",
    ),
    CandidateAsset(
        "combat_sprite_reference_512",
        "thecat_cat_suzune_combat_sprite_source_reference_512_candidate_v001.png",
        (512, 512),
        (18, 54, 610, 870),
        "Combat sprite reference candidate derived from the exact front turnaround crop.",
    ),
    CandidateAsset(
        "hud_avatar_reference_256",
        "thecat_cat_suzune_hud_avatar_source_reference_256_candidate_v001.png",
        (256, 256),
        (165, 62, 585, 382),
        "HUD avatar reference crop: blue eyes, orange/black/white calico patches, ears, and red-white flower ornament.",
    ),
    CandidateAsset(
        "skill_icon_motif_reference_128",
        "thecat_cat_suzune_bell_wand_motif_source_reference_128_candidate_v001.png",
        (128, 128),
        (18, 270, 180, 530),
        "Skill icon motif reference crop: clustered kagura bells, paper talismans, ribbons, and blue teardrop charms.",
    ),
    CandidateAsset(
        "palette_swatch_reference_256",
        "thecat_cat_suzune_palette_swatch_source_reference_256_candidate_v001.png",
        (256, 256),
        None,
        "Palette swatches sampled from the colored three-view turnaround.",
    ),
)


SWATCH_POINTS: tuple[tuple[str, tuple[int, int]], ...] = (
    ("white fur", (330, 260)),
    ("orange fur", (230, 135)),
    ("black fur", (420, 130)),
    ("blue eye", (355, 320)),
    ("robe", (438, 520)),
    ("vermilion", (335, 640)),
    ("gold bell", (340, 505)),
    ("talisman", (70, 330)),
)


def main() -> None:
    source_path = resolve_one(SOURCE_PATTERN)
    source = Image.open(source_path).convert("RGBA")

    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    review_note = CANDIDATE_DIR / "suzune_batch33_strict_turnaround_candidate_review.md"
    review_sheet = CANDIDATE_DIR / "thecat_cat_suzune_batch33_strict_turnaround_review_sheet.png"

    rows: list[dict[str, str]] = []
    produced: dict[str, Path] = {}
    for spec in CANDIDATES:
        output_path = CANDIDATE_DIR / spec.file_name
        image = build_palette_swatch(source, spec.size) if spec.crop_box is None else build_reference_crop(source, spec.crop_box, spec.size)
        image.save(output_path)
        produced[spec.asset_type] = output_path
        rows.append(
            {
                "cat_id": "suzune",
                "display_name": "Suzune / Sleep Shrine Maiden",
                "batch_slug": BATCH_SLUG,
                "asset_type": spec.asset_type,
                "candidate_path": to_repo_path(output_path),
                "candidate_sha256": sha256(output_path),
                "candidate_size": f"{spec.size[0]}x{spec.size[1]}",
                "source_turnaround_path": to_repo_path(source_path),
                "source_turnaround_sha256": sha256(source_path),
                "source_crop_box": format_crop(spec.crop_box),
                "source_lock_id": SOURCE_LOCK_ID,
                "active_screenshot": ACTIVE_SCREENSHOT,
                "review_sheet": to_repo_path(review_sheet),
                "review_note": to_repo_path(review_note),
                "recommendation": "candidate_review_only_pending_playmode_screenshot",
            }
        )

    write_review_sheet(source, produced, review_sheet)
    write_review_note(source_path, review_sheet, produced, review_note)
    write_manifest(rows)
    write_batch_summary(rows, source_path, review_sheet, review_note)
    print(f"Wrote {len(rows)} Suzune strict-turnaround candidate assets.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def build_reference_crop(source: Image.Image, crop_box: tuple[int, int, int, int], size: tuple[int, int]) -> Image.Image:
    crop = source.crop(crop_box)
    return fit_to_canvas(crop, size, (250, 246, 237, 255))


def fit_to_canvas(image: Image.Image, size: tuple[int, int], background: tuple[int, int, int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, background)
    work = image.copy()
    work.thumbnail((size[0] - 28, size[1] - 28), Image.Resampling.LANCZOS)
    canvas.alpha_composite(work, ((size[0] - work.width) // 2, (size[1] - work.height) // 2))
    return canvas


def build_palette_swatch(source: Image.Image, size: tuple[int, int]) -> Image.Image:
    image = Image.new("RGBA", size, (250, 246, 237, 255))
    draw = ImageDraw.Draw(image)
    font = load_font(12)
    title_font = load_font(14)
    draw.text((14, 10), "Suzune locked palette", fill=(42, 36, 32), font=title_font)
    draw.text((14, 30), "sampled from colored turnaround", fill=(78, 68, 60), font=font)

    y = 58
    for label, point in SWATCH_POINTS:
        color = source.getpixel(point)
        draw.rounded_rectangle((14, y, 70, y + 24), radius=4, fill=color, outline=(52, 46, 42))
        draw.text((82, y + 5), f"{label} #{color[0]:02x}{color[1]:02x}{color[2]:02x}", fill=(42, 36, 32), font=font)
        y += 24

    draw.rectangle((0, 248, size[0], size[1]), fill=(156, 44, 32, 255))
    return image


def write_review_sheet(source: Image.Image, produced: dict[str, Path], output_path: Path) -> None:
    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 26), "P0 Batch 33 - Suzune strict turnaround derivatives", fill=(42, 36, 32), font=title_font)
    draw.text((36, 66), "Candidate review only. All files stay outside Assets. Formal import remains blocked until active-cat Play Mode screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_preview = fit_to_canvas(source.copy(), (760, 420), (250, 246, 237, 255))
    sheet.alpha_composite(source_preview, (36, 100))
    draw.text((36, 530), "Source: colored three-view turnaround (front / side / back)", fill=(42, 36, 32), font=label_font)

    panel_specs = (
        ("front", "front_view_reference_512", (840, 104)),
        ("side", "side_view_reference_512", (1064, 104)),
        ("back", "back_view_reference_512", (1288, 104)),
        ("combat", "combat_sprite_reference_512", (840, 410)),
        ("hud", "hud_avatar_reference_256", (1064, 448)),
        ("icon", "skill_icon_motif_reference_128", (1288, 494)),
        ("palette", "palette_swatch_reference_256", (1064, 650)),
    )
    for label, key, pos in panel_specs:
        asset = Image.open(produced[key]).convert("RGBA")
        if asset.width > 190 or asset.height > 190:
            asset.thumbnail((190, 190), Image.Resampling.LANCZOS)
        x, y = pos
        draw.rounded_rectangle((x - 10, y - 10, x + 202, y + 224), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
        sheet.alpha_composite(asset, (x + (190 - asset.width) // 2, y))
        draw.text((x, y + 194), label, fill=(42, 36, 32), font=label_font)

    rule_y = 625
    rules = (
        "Review locks: calico markings, blue eyes, white robe, vermilion cloth, bells, wand, snowflake sleeves, back bow.",
        "Reject: generic shrine-cat drift, human shrine maiden proportions, or missing front/side/back anchors.",
        "Next gate: compare active screenshot 06-active-cat-suzune.png against this source before Unity import.",
    )
    for rule in rules:
        draw.text((36, rule_y), "- " + rule, fill=(42, 36, 32), font=body_font)
        rule_y += 28

    output_path.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(output_path)


def write_review_note(source_path: Path, review_sheet: Path, produced: dict[str, Path], output_path: Path) -> None:
    lines = [
        "# Suzune Batch 33 Strict Turnaround Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "formal import remains blocked until active-cat Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        f"- Colored three-view turnaround: `{to_repo_path(source_path)}`",
        f"- Source lock id: `{SOURCE_LOCK_ID}`",
        f"- Active screenshot required before import: `{ACTIVE_SCREENSHOT}`",
        f"- Review sheet: `{to_repo_path(review_sheet)}`",
        "",
        "## Output Policy",
        "",
        "- Candidate files are under `design/development/asset_candidates/starter_cats/suzune`.",
        "- Candidate files stay outside Assets.",
        "- No Unity `.meta` files are created in this batch.",
        "- No runtime cat sprite is changed.",
        "",
        "## Produced Candidate Assets",
        "",
    ]
    for asset_type, path in produced.items():
        lines.append(f"- `{asset_type}`: `{to_repo_path(path)}`")

    lines.extend(
        [
            "",
            "## Turnaround Checks",
            "",
            "- Front: preserve orange, black, and white calico face patches, blue eyes, white shrine robe, vermilion skirt/sash, central gold bell, flower ornament, and bell wand/branch cluster.",
            "- Side: preserve calico ear and cheek patches, white sleeve with blue snowflake motif, red stitch trim, red ribbons, hanging bells, bell wand, and tail.",
            "- Back: preserve orange/black calico head patches, large vermilion bow, gold bell, white robe, blue snowflake sleeves, and calico tail.",
            "- Palette: preserve warm white fur/robe, vermilion cloth, gold bells, moon-blue talismans, and pale sleep effects.",
            "",
            "## Rejection Rules",
            "",
            "- Prohibited drift: generic shrine-cat or generated-lineup drift over the colored turnaround.",
            "- Reject human shrine maiden proportions, human sleeves-as-arms pose, human costume posture, or human body language.",
            "- Reject palette drift away from calico patches, white robe, vermilion cloth, gold bells, and blue talismans.",
            "- Reject missing front, side, or back anchors including calico markings, bells, wand, flower ornament, snowflake sleeves, and back bow.",
            "",
            "## Next Gate",
            "",
            "Capture `06-active-cat-suzune.png` in Play Mode and compare it against the colored three-view turnaround and this Batch 33 review sheet before any Unity import approval.",
            "",
        ]
    )
    output_path.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_batch_summary(rows: list[dict[str, str]], source_path: Path, review_sheet: Path, review_note: Path) -> None:
    summary = BATCH_DIR / "suzune_batch33_strict_turnaround_summary.md"
    lines = [
        "# Suzune Batch 33 Strict Turnaround Derivatives",
        "",
        "This batch creates source-derived Suzune candidate assets in Codex, outside Unity, using the locked colored three-view turnaround as the only visual authority.",
        "",
        f"- Source: `{to_repo_path(source_path)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(review_sheet)}`",
        f"- Review note: `{to_repo_path(review_note)}`",
        "- Unity import: blocked pending active-cat Play Mode screenshot review.",
        "",
        "## Candidate Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['asset_type']}` -> `{row['candidate_path']}`")
    lines.append("")
    summary.write_text("\n".join(lines), encoding="utf-8")


def load_font(size: int) -> ImageFont.ImageFont:
    candidates = (
        Path("C:/Windows/Fonts/arial.ttf"),
        Path("C:/Windows/Fonts/segoeui.ttf"),
    )
    for candidate in candidates:
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size=size)
    return ImageFont.load_default()


def format_crop(crop_box: tuple[int, int, int, int] | None) -> str:
    if crop_box is None:
        return "sampled_palette"
    return ",".join(str(value) for value in crop_box)


def sha256(path: Path) -> str:
    hasher = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(65536), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
