from __future__ import annotations

import csv
import hashlib
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_45_starter_cat_source_lock_audit_pack_2026-06-15"
CANDIDATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats"
BATCH_DIR = CANDIDATE_ROOT / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "starter_cat_batch45_source_lock_audit_manifest.csv"
REVIEW_SHEET_PATH = BATCH_DIR / "thecat_starter_cat_batch45_source_lock_audit_review_sheet.png"
REVIEW_NOTE_PATH = BATCH_DIR / "starter_cat_batch45_source_lock_audit_review.md"
PROCESS_NOTE_PATH = BATCH_DIR / "starter_cat_batch45_source_lock_audit_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_45_starter_cat_source_lock_audit_pack.md"

DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"


@dataclass(frozen=True)
class CatSpec:
    cat_id: str
    display_name: str
    source_lock_id: str
    source_turnaround_path: str
    unity_sprite_path: str
    latest_cutout_manifest: str
    active_screenshot: str
    required_traits: tuple[str, ...]
    reject_rules: tuple[str, ...]


CATS: tuple[CatSpec, ...] = (
    CatSpec(
        "saiban",
        "Saiban / Sword Saint",
        "saiban_turnaround_colored",
        DESIGN_ROOT + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png",
        "design/development/asset_candidates/starter_cats/batch_31_cutout_candidate_2026-06-14/saiban_batch31_cutout_manifest.csv",
        "04-active-cat-saiban.png",
        (
            "silver-gray tabby non-human cat body",
            "shield and sword silhouette",
            "red cape, helm, silver armor",
            "gold trim and blue gem accents",
            "front, side, and back turnaround anchors",
        ),
        (
            "human knight body proportions",
            "generic armored kitten drift",
            "missing shield, sword, cape, helm, or striped tail",
        ),
    ),
    CatSpec(
        "nephthys",
        "Nephthys / Moon-Sand Agent",
        "nephthys_turnaround_colored",
        DESIGN_ROOT + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
        "design/development/asset_candidates/starter_cats/batch_37_nephthys_cutout_candidate_2026-06-15/nephthys_batch37_cutout_manifest.csv",
        "05-active-cat-nephthys.png",
        (
            "hooded non-human cat body",
            "gold-brown tabby fur",
            "deep navy cloak and sand-gold trim",
            "floating pyramid or obelisk prop",
            "blue gems, ankh, and dream-script marks",
        ),
        (
            "human Cleopatra posture",
            "generic Egyptian costume drift",
            "missing hood, pyramid or obelisk prop, or gold-blue palette",
        ),
    ),
    CatSpec(
        "suzune",
        "Suzune / Sleep Shrine Healer",
        "suzune_turnaround_colored",
        DESIGN_ROOT + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
        "design/development/asset_candidates/starter_cats/batch_35_suzune_cutout_candidate_2026-06-15/suzune_batch35_cutout_manifest.csv",
        "06-active-cat-suzune.png",
        (
            "calico markings from the colored turnaround",
            "shrine outfit on non-human cat body",
            "bell ornaments",
            "wand or branch healer silhouette",
            "vermilion, warm white, and moon-blue palette",
        ),
        (
            "human shrine maiden proportions",
            "generic healer kitten drift",
            "missing calico markings, bells, wand, or shrine outfit",
        ),
    ),
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    rows: list[dict[str, str]] = []
    cards: dict[str, Path] = {}

    for spec in CATS:
        cat_dir = CANDIDATE_ROOT / spec.cat_id / BATCH_SLUG
        cat_dir.mkdir(parents=True, exist_ok=True)

        source = resolve_repo_path(spec.source_turnaround_path)
        sprite = resolve_repo_path(spec.unity_sprite_path)
        cutout_row = find_cutout_preview(resolve_repo_path(spec.latest_cutout_manifest))
        cutout = resolve_repo_path(cutout_row["candidate_path"])
        card_path = cat_dir / f"thecat_cat_{spec.cat_id}_batch45_source_lock_lineage_card_v001.png"

        write_lineage_card(spec, source, sprite, cutout, card_path)
        cards[spec.cat_id] = card_path

        rows.append(
            {
                "cat_id": spec.cat_id,
                "display_name": spec.display_name,
                "batch_slug": BATCH_SLUG,
                "asset_type": "source_lock_lineage_card_1000x640",
                "candidate_path": to_repo_path(card_path),
                "candidate_sha256": sha256(card_path),
                "candidate_size": "1000x640",
                "source_turnaround_path": spec.source_turnaround_path,
                "source_turnaround_sha256": sha256(source),
                "source_lock_id": spec.source_lock_id,
                "unity_sprite_path": spec.unity_sprite_path,
                "unity_sprite_sha256": sha256(sprite),
                "latest_cutout_preview_path": cutout_row["candidate_path"],
                "latest_cutout_preview_sha256": sha256(cutout),
                "latest_cutout_manifest": spec.latest_cutout_manifest,
                "active_screenshot": spec.active_screenshot,
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "source_lock_audit_only_do_not_import",
            }
        )

    write_review_sheet(cards)
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print(f"Wrote {len(rows)} starter-cat source-lock audit card(s).")
    print(to_repo_path(MANIFEST_PATH))


def find_cutout_preview(manifest_path: Path) -> dict[str, str]:
    with manifest_path.open("r", encoding="utf-8-sig", newline="") as handle:
        rows = list(csv.DictReader(handle))

    for row in rows:
        if row.get("asset_type") == "cutout_alpha_512_preview":
            return row

    raise ValueError(f"Missing cutout_alpha_512_preview row in {manifest_path}")


def write_lineage_card(spec: CatSpec, source_path: Path, sprite_path: Path, cutout_path: Path, output_path: Path) -> None:
    canvas = Image.new("RGBA", (1000, 640), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(24)
    label_font = load_font(16)
    body_font = load_font(13)
    small_font = load_font(11)

    draw.text((28, 22), f"Batch 45 source-lock audit - {spec.display_name}", fill=(42, 36, 32), font=title_font)
    draw.text((28, 54), "Strictly match the colored three-view turnaround. Candidate audit only; do not import into Unity yet.", fill=(110, 45, 42), font=body_font)

    source = Image.open(source_path).convert("RGBA")
    sprite = Image.open(sprite_path).convert("RGBA")
    cutout = Image.open(cutout_path).convert("RGBA")

    draw_panel(canvas, draw, source, (28, 92), (470, 250), "locked colored three-view source", label_font)
    draw_panel(canvas, draw, sprite, (530, 92), (190, 190), "current Unity sprite", label_font)
    draw_panel(canvas, draw, cutout, (762, 92), (190, 190), "latest cutout candidate", label_font)

    y = 374
    draw.text((28, y), "Required identity anchors", fill=(42, 36, 32), font=label_font)
    y += 24
    for trait in spec.required_traits:
        draw.text((42, y), "- " + trait, fill=(42, 36, 32), font=body_font)
        y += 20

    y = 374
    draw.text((530, y), "Reject immediately", fill=(42, 36, 32), font=label_font)
    y += 24
    for rule in spec.reject_rules:
        draw.text((544, y), "- " + rule, fill=(42, 36, 32), font=body_font)
        y += 20

    y = 504
    draw.text((530, y), f"Source lock: {spec.source_lock_id}", fill=(42, 36, 32), font=body_font)
    draw.text((530, y + 20), f"Required active screenshot: {spec.active_screenshot}", fill=(42, 36, 32), font=body_font)
    draw.text((530, y + 40), "Formal import remains blocked until Play Mode screenshot comparison passes.", fill=(110, 45, 42), font=body_font)
    draw.text((28, 604), short_path(spec.source_turnaround_path), fill=(78, 68, 60), font=small_font)
    canvas.save(output_path)


def write_review_sheet(cards: dict[str, Path]) -> None:
    sheet = Image.new("RGBA", (3200, 900), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    body_font = load_font(16)

    draw.text((42, 28), "P0 Batch 45 - starter cat source-lock audit pack", fill=(42, 36, 32), font=title_font)
    draw.text((42, 74), "No new model art. No Unity import. Use this sheet as the strict gate before any future cat asset generation or replacement.", fill=(110, 45, 42), font=body_font)

    positions = ((54, 120), (1100, 120), (2146, 120))
    for spec, position in zip(CATS, positions):
        card = Image.open(cards[spec.cat_id]).convert("RGBA")
        x, y = position
        draw.rounded_rectangle((x - 18, y - 18, x + 1018, y + 658), radius=10, fill=(255, 252, 246), outline=(178, 158, 130))
        sheet.alpha_composite(card, position)

    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 45 Starter Cat Source-Lock Audit Review",
        "",
        "Decision: source-lock audit only; do not import into Unity yet.",
        "",
        "This pack exists because starter cat assets must strictly match the colored three-view turnaround, not merely the general project style.",
        "",
        "## Scope",
        "",
        "- Covers Saiban, Nephthys, and Suzune only.",
        "- Uses the locked colored three-view turnarounds as the hard authority.",
        "- Compares each locked turnaround with the current Unity combat sprite and latest transparent cutout candidate.",
        "- Keeps all Batch 45 outputs outside `Assets`.",
        "- Creates no Unity `.meta` files.",
        "- Formal import remains blocked until active-cat Play Mode screenshot comparison passes.",
        "",
        "## Outputs",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Cat Rows",
        "",
    ]
    for row in rows:
        lines.extend(
            [
                f"### {row['display_name']}",
                "",
                f"- Source lock: `{row['source_lock_id']}`",
                f"- Source turnaround: `{row['source_turnaround_path']}`",
                f"- Current Unity sprite: `{row['unity_sprite_path']}`",
                f"- Latest cutout preview: `{row['latest_cutout_preview_path']}`",
                f"- Active screenshot gate: `{row['active_screenshot']}`",
                f"- Audit card: `{row['candidate_path']}`",
                "",
            ]
        )

    lines.extend(
        [
            "## Rejection Rules",
            "",
            "- Reject any future starter-cat asset that drifts from the colored three-view turnaround.",
            "- Reject any human body proportions, human costume pose, or generic cute mascot replacement.",
            "- Reject missing side/back anchors even if the front view looks acceptable.",
            "- Reject palette drift from the locked colored turnaround.",
            "- Reject any candidate that cannot be compared against the active-cat Play Mode screenshot.",
            "",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 45 Process Note",
        "",
        "- Generation mode: deterministic local composition only.",
        "- Built-in image generation was not used in this batch.",
        "- Inputs: locked colored three-view turnarounds, current Unity combat sprites, and latest cutout preview manifests.",
        "- Outputs remain in `design/development/asset_candidates/starter_cats`.",
        "- No files were copied into `Assets`.",
        "- No Unity `.meta` files were created.",
        "- Import state: blocked pending active-cat Play Mode screenshot review.",
        "",
        "## Verified Lineage Inputs",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['cat_id']}` source `{row['source_turnaround_sha256']}` sprite `{row['unity_sprite_sha256']}` cutout `{row['latest_cutout_preview_sha256']}`")
    lines.append("")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def draw_panel(
    canvas: Image.Image,
    draw: ImageDraw.ImageDraw,
    image: Image.Image,
    origin: tuple[int, int],
    size: tuple[int, int],
    label: str,
    font: ImageFont.ImageFont,
) -> None:
    x, y = origin
    w, h = size
    draw.rounded_rectangle((x - 10, y - 10, x + w + 10, y + h + 36), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    preview = fit_to_canvas(image, size)
    canvas.alpha_composite(preview, origin)
    draw.text((x, y + h + 10), label, fill=(42, 36, 32), font=font)


def fit_to_canvas(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    preview = image.copy()
    preview.thumbnail((size[0] - 12, size[1] - 12), Image.Resampling.LANCZOS)
    canvas = Image.new("RGBA", size, (250, 246, 237, 255))
    canvas.alpha_composite(preview, ((size[0] - preview.width) // 2, (size[1] - preview.height) // 2))
    return canvas


def resolve_repo_path(repo_path: str) -> Path:
    path = REPO_ROOT / repo_path.replace("/", "\\")
    if not path.exists():
        raise FileNotFoundError(repo_path)
    return path


def load_font(size: int) -> ImageFont.ImageFont:
    for candidate in (
        Path("C:/Windows/Fonts/msyh.ttc"),
        Path("C:/Windows/Fonts/msyh.ttf"),
        Path("C:/Windows/Fonts/segoeui.ttf"),
        Path("C:/Windows/Fonts/arial.ttf"),
    ):
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size=size)
    return ImageFont.load_default()


def sha256(path: Path) -> str:
    hasher = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(65536), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def short_path(path: str) -> str:
    return path if len(path) <= 130 else "..." + path[-127:]


if __name__ == "__main__":
    main()
