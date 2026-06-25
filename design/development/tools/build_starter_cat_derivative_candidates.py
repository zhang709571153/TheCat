from __future__ import annotations

import csv
import hashlib
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
OUTPUT_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats"
BATCH_SLUG = "batch_05_source_locked_derivatives_2026-06-14"
CONFORMANCE_SPEC_PATH = "design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md"


@dataclass(frozen=True)
class CandidateSpec:
    file_name: str
    asset_type: str
    source_note: str


@dataclass(frozen=True)
class CatSpec:
    cat_id: str
    display_name: str
    source_glob: str
    current_sprite: str
    active_screenshot: str
    source_lock_id: str
    traits: tuple[str, ...]
    icon_crop: tuple[int, int, int, int]
    front_anchors: tuple[str, ...]
    side_anchors: tuple[str, ...]
    back_anchors: tuple[str, ...]
    palette_anchors: tuple[str, ...]
    prop_costume_anchors: tuple[str, ...]
    prohibited_drift: tuple[str, ...]


CATS: tuple[CatSpec, ...] = (
    CatSpec(
        cat_id="saiban",
        display_name="Saiban",
        source_glob="design/*/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png",
        current_sprite="Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png",
        active_screenshot="04-active-cat-saiban.png",
        source_lock_id="saiban_turnaround_colored",
        traits=(
            "silver-blue armored non-human cat proportions",
            "front-view tabby face markings",
            "oath shield silhouette",
            "sword silhouette",
            "cape and helm read from colored turnaround",
        ),
        icon_crop=(58, 185, 290, 425),
        front_anchors=(
            "front view silver-gray tabby face stripes and pale green eyes",
            "front view red torn cape collar over silver-gold armor",
            "front view round sun shield on the left side and single sword on the right side",
        ),
        side_anchors=(
            "side view compact non-human cat muzzle and upright ears",
            "side view red cape trails behind armor with striped tail visible",
            "side view shield disk and angled sword silhouette remain readable",
        ),
        back_anchors=(
            "back view gray tabby head stripes and rounded cat head",
            "back view torn red cape covers armor with dark holes along the lower edge",
            "back view striped tail sits below the cape with sword silhouette at the side",
        ),
        palette_anchors=(
            "silver-gray fur and tabby markings",
            "deep red cape fabric",
            "silver armor, gold trim, blue gems, and warm sun shield face",
        ),
        prop_costume_anchors=(
            "round oath sun shield",
            "single sword and silver-gold armor plates",
            "red torn cape, helm, and striped tail silhouette",
        ),
        prohibited_drift=(
            "Reject generated-lineup or generic knight-cat drift over the colored three-view turnaround.",
            "Reject human knight torso, long human legs, or biped costume posture.",
            "Reject palette drift away from silver-gray fur, red cape, silver armor, gold trim, and blue gems.",
            "Reject missing front, side, or back anchors including shield, sword, cape, tabby face, and striped tail.",
        ),
    ),
    CatSpec(
        cat_id="nephthys",
        display_name="Nephthys",
        source_glob="design/*/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
        current_sprite="Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
        active_screenshot="05-active-cat-nephthys.png",
        source_lock_id="nephthys_turnaround_colored",
        traits=(
            "hooded non-human cat body",
            "moon-sand Egyptian motif read",
            "floating pyramid / obelisk prop silhouette",
            "gold and blue palette from colored turnaround",
            "dream-script controller identity",
        ),
        icon_crop=(40, 120, 225, 320),
        front_anchors=(
            "front view gold-brown tabby face, large golden eyes, and dark blue hood",
            "front view crescent hood ornament with blue tear gem and gold script border",
            "front view floating pyramid over inverted obelisk prop beside raised paw",
        ),
        side_anchors=(
            "side view hood volume wraps the cat head while ears stay visible",
            "side view blue cloak layers and gold script trim sweep behind the compact body",
            "side view floating pyramid/obelisk prop remains in front of the paw",
        ),
        back_anchors=(
            "back view dark blue hood and cloak with centered vertical gold script strip",
            "back view winged blue gem and ankh emblem on the shoulder mantle",
            "back view split cloak exposes gold-brown striped tail",
        ),
        palette_anchors=(
            "gold-brown tabby fur",
            "deep navy cloak and hood",
            "sand-gold trim with blue gems and cyan magic particles",
        ),
        prop_costume_anchors=(
            "floating pyramid over inverted obelisk controller prop",
            "crescent moon hood ornament and blue teardrop gem",
            "gold script trim, ankh symbol, winged chest and back jewel",
        ),
        prohibited_drift=(
            "Reject generated-lineup or generic Egyptian fantasy drift over the colored three-view turnaround.",
            "Reject Cleopatra costume cliche, human body language, or human robe posture.",
            "Reject palette drift away from gold-brown fur, deep navy cloth, sand-gold trim, and blue gems.",
            "Reject missing front, side, or back anchors including hood, script trim, pyramid prop, ankh, and striped tail.",
        ),
    ),
    CatSpec(
        cat_id="suzune",
        display_name="Suzune",
        source_glob="design/*/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
        current_sprite="Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
        active_screenshot="06-active-cat-suzune.png",
        source_lock_id="suzune_turnaround_colored",
        traits=(
            "calico markings from colored turnaround",
            "shrine outfit on non-human cat body",
            "bell ornaments",
            "wand / branch healer silhouette",
            "vermilion, warm white, and moon-blue healer palette",
        ),
        icon_crop=(35, 115, 215, 420),
        front_anchors=(
            "front view calico orange, black, and white face patches with blue eyes",
            "front view white shrine robe, vermilion skirt, sash, and central gold bell",
            "front view bell wand/branch cluster with blue paper talismans",
        ),
        side_anchors=(
            "side view calico head patches continue across ear and cheek",
            "side view white sleeve with blue snowflake motif and red stitch trim",
            "side view red ribbons, hanging bells, and bell wand remain readable",
        ),
        back_anchors=(
            "back view orange and black calico head patches across both ears",
            "back view large vermilion bow with gold bell over white robe",
            "back view white sleeves show blue snowflake marks and calico tail",
        ),
        palette_anchors=(
            "warm white fur and robe fabric",
            "vermilion red skirt, sash, bow, and ribbons",
            "gold bells with moon-blue talismans and sleep effects",
        ),
        prop_costume_anchors=(
            "clustered kagura bell wand/branch",
            "red-white flower hair ornament with hanging bells",
            "paper talismans, blue snowflake charms, central bell, and back bow",
        ),
        prohibited_drift=(
            "Reject generated-lineup or generic shrine-cat drift over the colored three-view turnaround.",
            "Reject human shrine maiden proportions, human sleeves-as-arms pose, or human costume posture.",
            "Reject palette drift away from calico patches, white robe, vermilion cloth, gold bells, and blue talismans.",
            "Reject missing front, side, or back anchors including calico markings, bells, wand, snowflake sleeves, and back bow.",
        ),
    ),
)


def main() -> None:
    OUTPUT_ROOT.mkdir(parents=True, exist_ok=True)
    manifest_rows: list[dict[str, str]] = []

    for cat in CATS:
        rows = build_cat_candidates(cat)
        manifest_rows.extend(rows)

    write_batch_manifest(manifest_rows)
    write_batch_summary(manifest_rows)
    print(f"Wrote {len(manifest_rows)} source-locked starter-cat candidate assets.")
    print(to_repo_path(OUTPUT_ROOT / BATCH_SLUG))


def build_cat_candidates(cat: CatSpec) -> list[dict[str, str]]:
    source_path = resolve_one(cat.source_glob)
    current_path = REPO_ROOT / cat.current_sprite
    if not current_path.exists():
        raise FileNotFoundError(current_path)

    output_dir = OUTPUT_ROOT / cat.cat_id / BATCH_SLUG
    output_dir.mkdir(parents=True, exist_ok=True)

    current = Image.open(current_path).convert("RGBA")
    source = Image.open(source_path).convert("RGBA")

    candidate_paths: dict[str, Path] = {}
    candidate_paths["combat_sprite_refinement_512"] = write_png(
        output_dir / f"thecat_cat_{cat.cat_id}_combat_sprite_refinement_512_candidate_v001.png",
        current,
    )
    candidate_paths["front_animation_keyframe_512"] = write_png(
        output_dir / f"thecat_cat_{cat.cat_id}_front_animation_keyframe_512_idle_center_candidate_v001.png",
        current,
    )
    candidate_paths["hud_avatar_256"] = write_png(
        output_dir / f"thecat_cat_{cat.cat_id}_hud_avatar_256_candidate_v001.png",
        make_avatar(current, 256),
    )
    candidate_paths["skill_icon_motif_128"] = write_png(
        output_dir / f"thecat_cat_{cat.cat_id}_skill_icon_motif_128_candidate_v001.png",
        make_icon(current, cat.icon_crop, 128),
    )

    review_sheet = write_review_sheet(cat, source, current, candidate_paths, output_dir)
    review_note = write_review_note(cat, source_path, current_path, candidate_paths, review_sheet, output_dir)

    rows: list[dict[str, str]] = []
    for asset_type, path in candidate_paths.items():
        rows.append(
            {
                "cat_id": cat.cat_id,
                "display_name": cat.display_name,
                "asset_type": asset_type,
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "source_turnaround_path": to_repo_path(source_path),
                "source_turnaround_sha256": sha256(source_path),
                "current_sprite_path": to_repo_path(current_path),
                "current_sprite_sha256": sha256(current_path),
                "source_lock_id": cat.source_lock_id,
                "active_screenshot": cat.active_screenshot,
                "review_sheet": to_repo_path(review_sheet),
                "review_note": to_repo_path(review_note),
                "recommendation": "candidate_review_only_pending_playmode_screenshot",
            }
        )

    return rows


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected one match for {pattern}, found {len(matches)}")
    return matches[0]


def write_png(path: Path, image: Image.Image) -> Path:
    image.save(path, "PNG")
    return path


def alpha_bbox(image: Image.Image) -> tuple[int, int, int, int]:
    bbox = image.getchannel("A").getbbox()
    if bbox is None:
        return (0, 0, image.width, image.height)
    return bbox


def square_from_bbox(
    bbox: tuple[int, int, int, int],
    image_size: tuple[int, int],
    padding: int,
) -> tuple[int, int, int, int]:
    left, top, right, bottom = bbox
    left -= padding
    top -= padding
    right += padding
    bottom += padding

    width = right - left
    height = bottom - top
    side = max(width, height)
    cx = (left + right) // 2
    cy = (top + bottom) // 2
    left = cx - side // 2
    top = cy - side // 2
    right = left + side
    bottom = top + side

    img_w, img_h = image_size
    if left < 0:
        right -= left
        left = 0
    if top < 0:
        bottom -= top
        top = 0
    if right > img_w:
        left -= right - img_w
        right = img_w
    if bottom > img_h:
        top -= bottom - img_h
        bottom = img_h

    return (max(0, left), max(0, top), min(img_w, right), min(img_h, bottom))


def make_avatar(sprite: Image.Image, size: int) -> Image.Image:
    body = alpha_bbox(sprite)
    left, top, right, bottom = body
    height = bottom - top
    upper = (left, top, right, top + int(height * 0.63))
    crop_box = square_from_bbox(upper, sprite.size, 26)
    return fit_transparent(sprite.crop(crop_box), size, int(size * 0.88))


def make_icon(sprite: Image.Image, crop: tuple[int, int, int, int], size: int) -> Image.Image:
    crop_box = square_from_bbox(crop, sprite.size, 18)
    return fit_transparent(sprite.crop(crop_box), size, int(size * 0.86))


def fit_transparent(image: Image.Image, canvas_size: int, max_content_size: int) -> Image.Image:
    bbox = alpha_bbox(image)
    trimmed = image.crop(bbox)
    scale = min(max_content_size / trimmed.width, max_content_size / trimmed.height)
    new_size = (max(1, int(trimmed.width * scale)), max(1, int(trimmed.height * scale)))
    resized = trimmed.resize(new_size, Image.Resampling.LANCZOS)
    canvas = Image.new("RGBA", (canvas_size, canvas_size), (0, 0, 0, 0))
    offset = ((canvas_size - new_size[0]) // 2, (canvas_size - new_size[1]) // 2)
    canvas.alpha_composite(resized, offset)
    return canvas


def write_review_sheet(
    cat: CatSpec,
    source: Image.Image,
    current: Image.Image,
    candidate_paths: dict[str, Path],
    output_dir: Path,
) -> Path:
    sheet = Image.new("RGB", (1600, 900), (248, 244, 236))
    draw = ImageDraw.Draw(sheet)
    font = ImageFont.load_default()
    draw.text((36, 28), f"{cat.display_name} Batch 05 Source-Locked Derivative Review", fill=(20, 20, 20), font=font)
    draw.text((36, 52), "Source authority: locked colored turnaround. Candidates are review-only, not Unity imports.", fill=(50, 50, 50), font=font)

    paste_panel(sheet, source, (36, 95, 636, 405), "Locked colored turnaround", font)
    paste_panel(sheet, current, (690, 95, 1010, 405), "Current Unity combat sprite", font)

    x_positions = [36, 420, 805, 1190]
    labels = [
        "combat_sprite_refinement_512",
        "front_animation_keyframe_512",
        "hud_avatar_256",
        "skill_icon_motif_128",
    ]
    for x, label in zip(x_positions, labels):
        img = Image.open(candidate_paths[label]).convert("RGBA")
        paste_panel(sheet, img, (x, 485, x + 320, 805), label, font)

    y = 828
    draw.text((36, y), "Trait lock:", fill=(20, 20, 20), font=font)
    draw.text((110, y), " | ".join(cat.traits[:3]), fill=(40, 40, 40), font=font)
    draw.text((36, y + 22), "Decision: candidate review only; import is blocked until active-cat Play Mode screenshots exist.", fill=(80, 36, 36), font=font)

    path = output_dir / f"thecat_cat_{cat.cat_id}_batch05_source_locked_review_sheet.png"
    sheet.save(path, "PNG")
    return path


def paste_panel(
    sheet: Image.Image,
    image: Image.Image,
    box: tuple[int, int, int, int],
    label: str,
    font: ImageFont.ImageFont,
) -> None:
    draw = ImageDraw.Draw(sheet)
    x1, y1, x2, y2 = box
    draw.rectangle(box, outline=(188, 179, 165), width=2)
    draw.text((x1, y1 - 18), label, fill=(25, 25, 25), font=font)
    target_w = x2 - x1 - 28
    target_h = y2 - y1 - 28
    preview = image.copy()
    if preview.mode == "RGBA":
        preview = checkerboard(preview)
    scale = min(target_w / preview.width, target_h / preview.height)
    resized = preview.resize((max(1, int(preview.width * scale)), max(1, int(preview.height * scale))), Image.Resampling.LANCZOS)
    px = x1 + (x2 - x1 - resized.width) // 2
    py = y1 + (y2 - y1 - resized.height) // 2
    sheet.paste(resized.convert("RGB"), (px, py))


def checkerboard(image: Image.Image) -> Image.Image:
    tile = 16
    background = Image.new("RGBA", image.size, (230, 230, 230, 255))
    draw = ImageDraw.Draw(background)
    for y in range(0, image.height, tile):
        for x in range(0, image.width, tile):
            color = (245, 245, 245, 255) if ((x // tile) + (y // tile)) % 2 == 0 else (215, 215, 215, 255)
            draw.rectangle((x, y, x + tile - 1, y + tile - 1), fill=color)
    background.alpha_composite(image)
    return background


def write_review_note(
    cat: CatSpec,
    source_path: Path,
    current_path: Path,
    candidate_paths: dict[str, Path],
    review_sheet: Path,
    output_dir: Path,
) -> Path:
    lines: list[str] = []
    lines.append(f"# {cat.display_name} Batch 05 Source-Locked Candidate Review")
    lines.append("")
    lines.append("## Decision")
    lines.append("")
    lines.append("- Recommendation: candidate review only; do not import into Unity yet.")
    lines.append("- Reason: candidates are deterministic derivatives of the locked current sprite, but active-cat Play Mode screenshots are still missing.")
    lines.append("- Import gate: regenerate the 10-file Play Mode screenshot set and compare the active-cat capture against the colored turnaround contact sheet.")
    lines.append("")
    lines.append("## Evidence")
    lines.append("")
    lines.append(f"- Source lock id: `{cat.source_lock_id}`")
    lines.append(f"- Locked colored turnaround: `{to_repo_path(source_path)}`")
    lines.append(f"- Source SHA-256: `{sha256(source_path)}`")
    lines.append(f"- Current Unity combat sprite: `{to_repo_path(current_path)}`")
    lines.append(f"- Current sprite SHA-256: `{sha256(current_path)}`")
    lines.append(f"- Registered active-cat screenshot: `{cat.active_screenshot}`")
    lines.append(f"- Side-by-side review sheet: `{to_repo_path(review_sheet)}`")
    lines.append(f"- Turnaround conformance spec: `{CONFORMANCE_SPEC_PATH}`")
    lines.append("")
    lines.append("## Candidate PNGs")
    lines.append("")
    for asset_type, path in candidate_paths.items():
        lines.append(f"- `{asset_type}`: `{to_repo_path(path)}`")
        lines.append(f"  - SHA-256: `{sha256(path)}`")
    lines.append("")
    lines.append("## Trait Coverage")
    lines.append("")
    for trait in cat.traits:
        lines.append(f"- Preserved: {trait}")
    lines.append("")
    lines.append("## Turnaround Conformance Checklist")
    lines.append("")
    lines.append(f"- Review basis: `{CONFORMANCE_SPEC_PATH}`")
    lines.append("- Decision state: hold unless every front, side, back, palette, and prop/costume anchor below passes against the locked colored turnaround.")
    lines.append("")
    append_anchor_section(lines, "Front-view anchors", cat.front_anchors)
    append_anchor_section(lines, "Side-view anchors", cat.side_anchors)
    append_anchor_section(lines, "Back-view anchors", cat.back_anchors)
    append_anchor_section(lines, "Palette anchors", cat.palette_anchors)
    append_anchor_section(lines, "Prop/costume anchors", cat.prop_costume_anchors)
    append_anchor_section(lines, "Prohibited drift", cat.prohibited_drift)
    lines.append("")
    lines.append("## Rejection Rules")
    lines.append("")
    lines.append("- Reject if a later candidate drifts from the colored turnaround markings, costume, props, or silhouette.")
    lines.append("- Reject if it introduces human proportions or human costume posture.")
    lines.append("- Reject if the palette shifts away from the locked colored turnaround.")
    lines.append("- Reject if any required cat-specific trait is missing.")
    lines.append("")

    path = output_dir / f"{cat.cat_id}_batch05_source_locked_candidate_review.md"
    path.write_text("\n".join(lines), encoding="utf-8")
    return path


def append_anchor_section(lines: list[str], title: str, anchors: tuple[str, ...]) -> None:
    lines.append(f"### {title}")
    lines.append("")
    for anchor in anchors:
        lines.append(f"- Required: {anchor}")
    lines.append("")


def write_batch_manifest(rows: Iterable[dict[str, str]]) -> None:
    rows = list(rows)
    path = OUTPUT_ROOT / BATCH_SLUG / "starter_cat_batch05_candidate_manifest.csv"
    path.parent.mkdir(parents=True, exist_ok=True)
    fieldnames = [
        "cat_id",
        "display_name",
        "asset_type",
        "candidate_path",
        "candidate_sha256",
        "source_turnaround_path",
        "source_turnaround_sha256",
        "current_sprite_path",
        "current_sprite_sha256",
        "source_lock_id",
        "active_screenshot",
        "review_sheet",
        "review_note",
        "recommendation",
    ]
    with path.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(rows)


def write_batch_summary(rows: Iterable[dict[str, str]]) -> None:
    rows = list(rows)
    path = OUTPUT_ROOT / BATCH_SLUG / "README.md"
    path.parent.mkdir(parents=True, exist_ok=True)
    lines: list[str] = []
    lines.append("# Batch 05 Source-Locked Starter Cat Derivatives")
    lines.append("")
    lines.append("This batch contains deterministic review candidates derived from the locked current starter-cat combat sprites. It does not replace Unity import assets.")
    lines.append("")
    lines.append("## Gate Status")
    lines.append("")
    lines.append("- Candidate output is under `design/development/asset_candidates/starter_cats`.")
    lines.append("- Source authority remains the locked colored turnaround for each cat.")
    lines.append(f"- Candidate notes must include front/side/back, palette, prop/costume, and drift anchors from `{CONFORMANCE_SPEC_PATH}`.")
    lines.append("- Generated starter-cat lineup is not used as primary source.")
    lines.append("- Unity import is blocked until active-cat Play Mode screenshots exist and pass visual review.")
    lines.append("")
    lines.append("## Files")
    lines.append("")
    for row in rows:
        lines.append(f"- `{row['asset_type']}` `{row['cat_id']}`: `{row['candidate_path']}`")
    lines.append("")
    lines.append("## Candidate Manifest")
    lines.append("")
    lines.append("- `design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/starter_cat_batch05_candidate_manifest.csv`")
    lines.append("")
    path.write_text("\n".join(lines), encoding="utf-8")


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
