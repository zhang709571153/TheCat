from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw

from build_saiban_ai_generation_pilot import (
    alpha_mask,
    checkerboard_composite,
    draw_panel,
    fit_image,
    flat_composite,
    load_font,
    resize_square,
    wrap,
)


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_54_bedroom_interactable_candidates_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "props" / "bedroom_interactables" / BATCH_SLUG
MANIFEST_PATH = CANDIDATE_DIR / "bedroom_interactables_batch54_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_props_bedroom_interactables_batch54_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "bedroom_interactables_batch54_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "bedroom_interactables_batch54_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_54_bedroom_interactable_candidates.md"

BEDROOM_MAP = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/levels/lv01_bedroom_dream/concept/bedroom_dream_map_concept.png"
BEDROOM_MID_SPRITES = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_mid_background_sprites.png"
BEDROOM_FOREGROUND_SPRITES = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_foreground_sprites.png"

FIELD_NAMES = (
    "subject_id",
    "display_name",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_reference_path",
    "source_reference_sha256",
    "source_lock_id",
    "current_unity_sprite_path",
    "current_unity_sprite_sha256",
    "generated_source_path",
    "generated_source_sha256",
    "alpha_candidate_path",
    "alpha_candidate_sha256",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
    "visual_review",
)


PROPS = (
    {
        "subject_id": "bed",
        "display_name": "Protected Bed",
        "source_reference": BEDROOM_MAP,
        "source_lock_id": "bedroom_map_concept",
        "unity_sprite": "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png",
        "version": "v001",
        "source": "thecat_prop_bed_batch54_interactable_chromakey_source_v001.png",
        "alpha": "thecat_prop_bed_batch54_interactable_alpha_1024_candidate_v001.png",
        "preview": "thecat_prop_bed_batch54_interactable_alpha_512_preview_v001.png",
        "checker": "thecat_prop_bed_batch54_interactable_checkerboard_512_review_v001.png",
        "dark": "thecat_prop_bed_batch54_interactable_darkfield_512_review_v001.png",
        "warm": "thecat_prop_bed_batch54_interactable_warmfield_512_review_v001.png",
        "mask": "thecat_prop_bed_batch54_interactable_alpha_mask_512_review_v001.png",
        "review": "Strong candidate: independent protected-bed sprite, clear navy star blanket, crescent, wooden posts, and readable rug base. Watch scale because the rug footprint is larger than the current map crop.",
    },
    {
        "subject_id": "litter_box",
        "display_name": "Cat Litter Box",
        "source_reference": BEDROOM_MID_SPRITES,
        "source_lock_id": "bedroom_mid_background_sprites",
        "unity_sprite": "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_litterbox_sprite_256_v001.png",
        "version": "v002",
        "source": "thecat_prop_litterbox_batch54_interactable_chromakey_source_v002.png",
        "alpha": "thecat_prop_litterbox_batch54_interactable_alpha_1024_candidate_v002.png",
        "preview": "thecat_prop_litterbox_batch54_interactable_alpha_512_preview_v002.png",
        "checker": "thecat_prop_litterbox_batch54_interactable_checkerboard_512_review_v002.png",
        "dark": "thecat_prop_litterbox_batch54_interactable_darkfield_512_review_v002.png",
        "warm": "thecat_prop_litterbox_batch54_interactable_warmfield_512_review_v002.png",
        "mask": "thecat_prop_litterbox_batch54_interactable_alpha_mask_512_review_v002.png",
        "review": "Usable candidate: keeps blue rounded box, tan litter, and paw emblem. v002 replaces the green-halo v001 attempt; watch small-scale readability because the generated bowl is more close-up than the current sprite.",
    },
    {
        "subject_id": "feeder",
        "display_name": "Automatic Feeder",
        "source_reference": BEDROOM_MID_SPRITES,
        "source_lock_id": "bedroom_mid_background_sprites",
        "unity_sprite": "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_feeder_sprite_256_v001.png",
        "version": "v001",
        "source": "thecat_prop_feeder_batch54_interactable_chromakey_source_v001.png",
        "alpha": "thecat_prop_feeder_batch54_interactable_alpha_1024_candidate_v001.png",
        "preview": "thecat_prop_feeder_batch54_interactable_alpha_512_preview_v001.png",
        "checker": "thecat_prop_feeder_batch54_interactable_checkerboard_512_review_v001.png",
        "dark": "thecat_prop_feeder_batch54_interactable_darkfield_512_review_v001.png",
        "warm": "thecat_prop_feeder_batch54_interactable_warmfield_512_review_v001.png",
        "mask": "thecat_prop_feeder_batch54_interactable_alpha_mask_512_review_v001.png",
        "review": "Strong candidate: keeps pink-lavender feeder body, transparent kibble tank, paw mark, moon/star accents, and high hunger-readability. Watch height and close-up scale versus current 256 sprite.",
    },
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)

    for prop in PROPS:
        normalize_alpha(prop)
        alpha = Image.open(CANDIDATE_DIR / prop["alpha"]).convert("RGBA")
        resize_square(alpha, 512).save(CANDIDATE_DIR / prop["preview"])
        checkerboard_composite(resize_square(alpha, 512)).save(CANDIDATE_DIR / prop["checker"])
        flat_composite(resize_square(alpha, 512), (27, 29, 38, 255)).save(CANDIDATE_DIR / prop["dark"])
        flat_composite(resize_square(alpha, 512), (247, 239, 224, 255)).save(CANDIDATE_DIR / prop["warm"])
        alpha_mask(alpha).resize((512, 512), Image.Resampling.LANCZOS).save(CANDIDATE_DIR / prop["mask"])

    rows = build_manifest_rows()
    write_review_sheet()
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print("Wrote P0 Batch 54 bedroom interactable candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def normalize_alpha(prop: dict[str, str]) -> None:
    source_path = CANDIDATE_DIR / prop["source"]
    alpha_path = CANDIDATE_DIR / prop["alpha"]
    if not source_path.exists():
        raise FileNotFoundError(source_path)
    if not alpha_path.exists():
        raise FileNotFoundError(alpha_path)

    image = Image.open(alpha_path).convert("RGBA")
    if image.size != (1024, 1024):
        resize_square(image, 1024).save(alpha_path)


def build_manifest_rows() -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    for prop in PROPS:
        source_reference = resolve_repo_path(prop["source_reference"])
        unity_sprite = resolve_repo_path(prop["unity_sprite"])
        generated_source = CANDIDATE_DIR / prop["source"]
        alpha_candidate = CANDIDATE_DIR / prop["alpha"]
        assets = (
            ("chromakey_source", generated_source),
            ("alpha_candidate_1024", alpha_candidate),
            ("alpha_preview_512", CANDIDATE_DIR / prop["preview"]),
            ("checkerboard_review_512", CANDIDATE_DIR / prop["checker"]),
            ("darkfield_review_512", CANDIDATE_DIR / prop["dark"]),
            ("warmfield_review_512", CANDIDATE_DIR / prop["warm"]),
            ("alpha_mask_review_512", CANDIDATE_DIR / prop["mask"]),
        )

        for asset_type, path in assets:
            rows.append(
                {
                    "subject_id": prop["subject_id"],
                    "display_name": prop["display_name"],
                    "batch_slug": BATCH_SLUG,
                    "asset_type": asset_type,
                    "candidate_path": to_repo_path(path),
                    "candidate_sha256": sha256(path),
                    "candidate_size": image_size(path),
                    "source_reference_path": prop["source_reference"],
                    "source_reference_sha256": sha256(source_reference),
                    "source_lock_id": prop["source_lock_id"],
                    "current_unity_sprite_path": prop["unity_sprite"],
                    "current_unity_sprite_sha256": sha256(unity_sprite),
                    "generated_source_path": to_repo_path(generated_source),
                    "generated_source_sha256": sha256(generated_source),
                    "alpha_candidate_path": to_repo_path(alpha_candidate),
                    "alpha_candidate_sha256": sha256(alpha_candidate),
                    "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                    "review_note": to_repo_path(REVIEW_NOTE_PATH),
                    "process_note": to_repo_path(PROCESS_NOTE_PATH),
                    "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                    "recommendation": "candidate_review_only_do_not_import",
                    "visual_review": prop["review"],
                }
            )
    return rows


def write_review_sheet() -> None:
    sheet = Image.new("RGBA", (2400, 1800), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(17)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 54 - Bedroom interactable candidates", fill=(42, 36, 32), font=title_font)
    draw.text(
        (36, 76),
        "Candidate review only. Codex-side prop generation; do not install into Unity until runtime screenshot review.",
        fill=(116, 47, 43),
        font=body_font,
    )

    source_map = Image.open(resolve_repo_path(BEDROOM_MAP)).convert("RGBA")
    mid_sheet = Image.open(resolve_repo_path(BEDROOM_MID_SPRITES)).convert("RGBA")
    foreground_sheet = Image.open(resolve_repo_path(BEDROOM_FOREGROUND_SPRITES)).convert("RGBA")
    draw_panel(sheet, draw, source_map, (36, 124), (530, 310), "bedroom map source lock", label_font)
    draw_panel(sheet, draw, mid_sheet, (600, 124), (530, 310), "mid/background sprite source lock", label_font)
    draw_panel(sheet, draw, foreground_sheet, (1164, 124), (530, 310), "foreground sprite context", label_font)

    x = 1728
    y = 124
    draw.text((x, y), "Batch policy", fill=(42, 36, 32), font=label_font)
    notes = [
        "These images are candidate-only and stay outside Assets.",
        "No Unity .meta files are allowed in the candidate folder.",
        "Bed, litter_box, and feeder gameplay ids remain stable.",
        "Formal install needs Sprite import settings, scene/prefab binding, Console checks, HUD/world scale, and Play Mode screenshots.",
    ]
    y += 30
    for note in notes:
        for line in wrap(note, 72):
            draw.text((x, y), line, fill=(42, 36, 32), font=body_font)
            y += 19
        y += 8

    y_positions = (506, 910, 1314)
    for prop, y in zip(PROPS, y_positions):
        unity = Image.open(resolve_repo_path(prop["unity_sprite"])).convert("RGBA")
        chroma = Image.open(CANDIDATE_DIR / prop["source"]).convert("RGBA")
        alpha = Image.open(CANDIDATE_DIR / prop["alpha"]).convert("RGBA")
        checker = Image.open(CANDIDATE_DIR / prop["checker"]).convert("RGBA")
        dark = Image.open(CANDIDATE_DIR / prop["dark"]).convert("RGBA")
        warm = Image.open(CANDIDATE_DIR / prop["warm"]).convert("RGBA")
        mask = Image.open(CANDIDATE_DIR / prop["mask"]).convert("RGBA")

        draw.text((36, y), prop["display_name"], fill=(42, 36, 32), font=label_font)
        draw_panel(sheet, draw, unity, (36, y + 34), (220, 220), "current Unity sprite", label_font)
        draw_panel(sheet, draw, chroma, (292, y + 34), (220, 220), f"chroma source {prop['version']}", label_font)
        draw_panel(sheet, draw, alpha, (548, y + 34), (220, 220), f"alpha candidate {prop['version']}", label_font)
        draw_panel(sheet, draw, checker, (804, y + 34), (220, 220), "checkerboard review", label_font)
        draw_panel(sheet, draw, dark, (1060, y + 34), (220, 220), "dark-field review", label_font)
        draw_panel(sheet, draw, warm, (1316, y + 34), (220, 220), "warm-field review", label_font)
        draw_panel(sheet, draw, mask, (1572, y + 34), (220, 220), "alpha mask", label_font)

        text_x = 1828
        text_y = y + 38
        for line in wrap(prop["review"], 68):
            draw.text((text_x, text_y), line, fill=(42, 36, 32), font=body_font)
            text_y += 19

    draw.text((36, 1760), "No Batch 54 output is installed into Unity. Candidate folder only.", fill=(116, 47, 43), font=body_font)
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Bedroom Interactables Batch 54 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "Batch 54 produces Codex-side candidates for the P0 bed, litter box, and feeder interactables. The batch improves review options without changing runtime visual bindings or manifest catalog counts.",
        "",
        "## Output Summary",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Visual Review",
        "",
        "- Bed: independent protected-bed sprite, no room crop, strong navy blanket, gold stars, crescent, wooden posts, pillow, and readable rug base.",
        "- Litter box: v002 selected because v001 had a visible green chroma-key glow; v002 keeps the blue plastic box, tan clean litter, and paw emblem with cleaner edges.",
        "- Feeder: high-readability pink-lavender automatic feeder with visible kibble, transparent tank, paw emblem, and moon/star accents.",
        "- Watch item: all three AI candidates are more polished and close-up than the current source-extracted sprites, so game-scale comparison is required before install.",
        "- Watch item: bed includes a rug base and may need runtime scale reduction if it overlaps pathing or HUD screenshots.",
        "",
        "## Safety",
        "",
        "- The built-in image_gen outputs were copied into the workspace candidate folder.",
        "- Chroma-key removal was done locally with the imagegen skill helper.",
        "- Normalized alpha candidates are 1024x1024; review variants are 512x512.",
        "- No candidate file was copied into `Assets`.",
        "- No Unity `.meta` files were created.",
        "- Formal Unity import remains blocked pending Sprite import settings, scene/prefab binding, Console checks, runtime scale, and active battle-world screenshots.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['subject_id']}` `{row['asset_type']}` -> `{row['candidate_path']}`")
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Bedroom Interactables Batch 54 Process Note",
        "",
        "Process: built-in image_gen prop generation, workspace copy, local chroma-key alpha removal, 1024 normalization, deterministic review-pack generation.",
        "",
        "Prompt goal: keep the Bedroom Dream source identity for bed, litter box, and feeder while producing cleaner isolated interactable candidates.",
        "",
        "Chroma-key helper results:",
        "",
        "- Bed v001: key color auto-sampled near `#0ee414`; initial helper output reported 981151 transparent pixels and 3360 partially transparent pixels out of 1572516.",
        "- Litter box v001: rejected for visible green glow after chroma-key removal.",
        "- Litter box v002: key color `#ff00ff`; helper output reported 958973 transparent pixels and 30467 partially transparent pixels out of 1572516.",
        "- Feeder v001: key color auto-sampled near `#12f411`; helper output reported 963452 transparent pixels and 3027 partially transparent pixels out of 1572516.",
        "",
        f"Rows: {len(rows)}",
        "",
        "No Unity import was performed.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def image_size(path: Path) -> str:
    image = Image.open(path)
    try:
        return f"{image.width}x{image.height}"
    finally:
        image.close()


def resolve_repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


if __name__ == "__main__":
    main()
