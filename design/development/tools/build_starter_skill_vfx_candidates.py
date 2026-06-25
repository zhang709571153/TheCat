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
BATCH_SLUG = "batch_55_starter_skill_vfx_candidates_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "vfx" / "starter_skills" / BATCH_SLUG
MANIFEST_PATH = CANDIDATE_DIR / "starter_skill_vfx_batch55_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_vfx_starter_skills_batch55_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "starter_skill_vfx_batch55_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "starter_skill_vfx_batch55_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_55_starter_skill_vfx_candidates.md"

SAIBAN_TURNAROUND = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
NEPHTHYS_TURNAROUND = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
SUZUNE_TURNAROUND = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"

FIELD_NAMES = (
    "cat_id",
    "skill_family",
    "display_name",
    "authority_id",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "source_lock_id",
    "authority_icon_path",
    "authority_icon_sha256",
    "current_unity_vfx_path",
    "current_unity_vfx_sha256",
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


VFX_ENTRIES = (
    {
        "cat_id": "saiban",
        "skill_family": "defense_bedline",
        "display_name": "Saiban Bedline Oath Shield VFX",
        "authority_id": "authority_oath_bedline",
        "source_turnaround": SAIBAN_TURNAROUND,
        "source_lock_id": "saiban_turnaround_colored",
        "authority_icon": "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_oath_bedline_seal_128_v001.png",
        "current_unity_vfx": "Assets/TheCat/Art/VFX/thecat_vfx_bed_shield_pulse_256_v001.png",
        "source": "thecat_vfx_saiban_bedline_batch55_chromakey_source_v001.png",
        "alpha": "thecat_vfx_saiban_bedline_batch55_alpha_1024_candidate_v001.png",
        "preview": "thecat_vfx_saiban_bedline_batch55_alpha_512_preview_v001.png",
        "checker": "thecat_vfx_saiban_bedline_batch55_checkerboard_512_review_v001.png",
        "dark": "thecat_vfx_saiban_bedline_batch55_darkfield_512_review_v001.png",
        "warm": "thecat_vfx_saiban_bedline_batch55_warmfield_512_review_v001.png",
        "mask": "thecat_vfx_saiban_bedline_batch55_alpha_mask_512_review_v001.png",
        "review": "Usable candidate: shield arc, sword light, sun-gold oath sigil, and bedline knockback read match Saiban's defender role. Watch item: central cat paw emblem is readable but may be too literal; confirm before Unity install.",
    },
    {
        "cat_id": "nephthys",
        "skill_family": "control_moonsand",
        "display_name": "Nephthys Moon-Sand Control VFX",
        "authority_id": "authority_dominion_sandglass",
        "source_turnaround": NEPHTHYS_TURNAROUND,
        "source_lock_id": "nephthys_turnaround_colored",
        "authority_icon": "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_dominion_sandglass_seal_128_v001.png",
        "current_unity_vfx": "Assets/TheCat/Art/VFX/thecat_vfx_enemy_mark_ring_256_v001.png",
        "source": "thecat_vfx_nephthys_moonsand_batch55_chromakey_source_v001.png",
        "alpha": "thecat_vfx_nephthys_moonsand_batch55_alpha_1024_candidate_v001.png",
        "preview": "thecat_vfx_nephthys_moonsand_batch55_alpha_512_preview_v001.png",
        "checker": "thecat_vfx_nephthys_moonsand_batch55_checkerboard_512_review_v001.png",
        "dark": "thecat_vfx_nephthys_moonsand_batch55_darkfield_512_review_v001.png",
        "warm": "thecat_vfx_nephthys_moonsand_batch55_warmfield_512_review_v001.png",
        "mask": "thecat_vfx_nephthys_moonsand_batch55_alpha_mask_512_review_v001.png",
        "review": "Strong candidate: obelisk, moon-sand spiral, teal control rings, and royal eye mark communicate slow/mark control without drawing a cat body. Good candidate for future mark or trap VFX split.",
    },
    {
        "cat_id": "suzune",
        "skill_family": "healing_lullaby",
        "display_name": "Suzune Lullaby Healing VFX",
        "authority_id": "authority_rhythm_lullaby",
        "source_turnaround": SUZUNE_TURNAROUND,
        "source_lock_id": "suzune_turnaround_colored",
        "authority_icon": "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_rhythm_lullaby_seal_128_v001.png",
        "current_unity_vfx": "Assets/TheCat/Art/VFX/thecat_vfx_sleep_stable_wave_256_v001.png",
        "source": "thecat_vfx_suzune_lullaby_batch55_chromakey_source_v001.png",
        "alpha": "thecat_vfx_suzune_lullaby_batch55_alpha_1024_candidate_v001.png",
        "preview": "thecat_vfx_suzune_lullaby_batch55_alpha_512_preview_v001.png",
        "checker": "thecat_vfx_suzune_lullaby_batch55_checkerboard_512_review_v001.png",
        "dark": "thecat_vfx_suzune_lullaby_batch55_darkfield_512_review_v001.png",
        "warm": "thecat_vfx_suzune_lullaby_batch55_warmfield_512_review_v001.png",
        "mask": "thecat_vfx_suzune_lullaby_batch55_alpha_mask_512_review_v001.png",
        "review": "Strong candidate: bells, vermilion torii silhouette, moon-blue healing circle, talismans, and music notes communicate healing and sleep-stable recovery. Watch battlefield scale because the torii is a large central symbol.",
    },
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)

    for entry in VFX_ENTRIES:
        normalize_alpha(entry)
        alpha = Image.open(CANDIDATE_DIR / entry["alpha"]).convert("RGBA")
        resized = resize_square(alpha, 512)
        resized.save(CANDIDATE_DIR / entry["preview"])
        checkerboard_composite(resized).save(CANDIDATE_DIR / entry["checker"])
        flat_composite(resized, (20, 22, 32, 255)).save(CANDIDATE_DIR / entry["dark"])
        flat_composite(resized, (247, 239, 224, 255)).save(CANDIDATE_DIR / entry["warm"])
        alpha_mask(alpha).resize((512, 512), Image.Resampling.LANCZOS).save(CANDIDATE_DIR / entry["mask"])

    rows = build_manifest_rows()
    write_review_sheet()
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print("Wrote P0 Batch 55 starter skill VFX candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def normalize_alpha(entry: dict[str, str]) -> None:
    source_path = CANDIDATE_DIR / entry["source"]
    alpha_path = CANDIDATE_DIR / entry["alpha"]
    if not source_path.exists():
        raise FileNotFoundError(source_path)
    if not alpha_path.exists():
        raise FileNotFoundError(alpha_path)

    image = Image.open(alpha_path).convert("RGBA")
    if image.size != (1024, 1024):
        resize_square(image, 1024).save(alpha_path)


def build_manifest_rows() -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    for entry in VFX_ENTRIES:
        source_turnaround = resolve_repo_path(entry["source_turnaround"])
        authority_icon = resolve_repo_path(entry["authority_icon"])
        current_unity_vfx = resolve_repo_path(entry["current_unity_vfx"])
        generated_source = CANDIDATE_DIR / entry["source"]
        alpha_candidate = CANDIDATE_DIR / entry["alpha"]
        assets = (
            ("chromakey_source", generated_source),
            ("alpha_candidate_1024", alpha_candidate),
            ("alpha_preview_512", CANDIDATE_DIR / entry["preview"]),
            ("checkerboard_review_512", CANDIDATE_DIR / entry["checker"]),
            ("darkfield_review_512", CANDIDATE_DIR / entry["dark"]),
            ("warmfield_review_512", CANDIDATE_DIR / entry["warm"]),
            ("alpha_mask_review_512", CANDIDATE_DIR / entry["mask"]),
        )

        for asset_type, path in assets:
            rows.append(
                {
                    "cat_id": entry["cat_id"],
                    "skill_family": entry["skill_family"],
                    "display_name": entry["display_name"],
                    "authority_id": entry["authority_id"],
                    "batch_slug": BATCH_SLUG,
                    "asset_type": asset_type,
                    "candidate_path": to_repo_path(path),
                    "candidate_sha256": sha256(path),
                    "candidate_size": image_size(path),
                    "source_turnaround_path": entry["source_turnaround"],
                    "source_turnaround_sha256": sha256(source_turnaround),
                    "source_lock_id": entry["source_lock_id"],
                    "authority_icon_path": entry["authority_icon"],
                    "authority_icon_sha256": sha256(authority_icon),
                    "current_unity_vfx_path": entry["current_unity_vfx"],
                    "current_unity_vfx_sha256": sha256(current_unity_vfx),
                    "generated_source_path": to_repo_path(generated_source),
                    "generated_source_sha256": sha256(generated_source),
                    "alpha_candidate_path": to_repo_path(alpha_candidate),
                    "alpha_candidate_sha256": sha256(alpha_candidate),
                    "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                    "review_note": to_repo_path(REVIEW_NOTE_PATH),
                    "process_note": to_repo_path(PROCESS_NOTE_PATH),
                    "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                    "recommendation": "candidate_review_only_do_not_import",
                    "visual_review": entry["review"],
                }
            )
    return rows


def write_review_sheet() -> None:
    sheet = Image.new("RGBA", (2400, 1800), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(17)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 55 - Starter skill VFX candidates", fill=(42, 36, 32), font=title_font)
    draw.text(
        (36, 76),
        "Candidate review only. Codex-side symbolic VFX generation; do not import into Unity until a formal install batch is approved.",
        fill=(116, 47, 43),
        font=body_font,
    )
    draw.text(
        (36, 104),
        "Source locks are the colored cat turnarounds and current runtime VFX/authority icons. No cat bodies were generated in this batch.",
        fill=(78, 68, 60),
        font=body_font,
    )

    y_positions = (160, 700, 1240)
    for entry, y in zip(VFX_ENTRIES, y_positions):
        source = Image.open(resolve_repo_path(entry["source_turnaround"])).convert("RGBA")
        authority = Image.open(resolve_repo_path(entry["authority_icon"])).convert("RGBA")
        current = Image.open(resolve_repo_path(entry["current_unity_vfx"])).convert("RGBA")
        chroma = Image.open(CANDIDATE_DIR / entry["source"]).convert("RGBA")
        alpha = Image.open(CANDIDATE_DIR / entry["alpha"]).convert("RGBA")
        checker = Image.open(CANDIDATE_DIR / entry["checker"]).convert("RGBA")
        dark = Image.open(CANDIDATE_DIR / entry["dark"]).convert("RGBA")
        warm = Image.open(CANDIDATE_DIR / entry["warm"]).convert("RGBA")
        mask = Image.open(CANDIDATE_DIR / entry["mask"]).convert("RGBA")

        draw.text((36, y), f"{entry['display_name']} / {entry['authority_id']}", fill=(42, 36, 32), font=label_font)
        draw_panel(sheet, draw, source, (36, y + 34), (250, 310), "colored turnaround lock", label_font)
        draw_panel(sheet, draw, authority, (318, y + 34), (180, 180), "authority icon", label_font)
        draw_panel(sheet, draw, current, (530, y + 34), (180, 180), "current Unity VFX", label_font)
        draw_panel(sheet, draw, chroma, (742, y + 34), (240, 240), "chroma source", label_font)
        draw_panel(sheet, draw, alpha, (1014, y + 34), (240, 240), "alpha candidate", label_font)
        draw_panel(sheet, draw, checker, (1286, y + 34), (240, 240), "checkerboard", label_font)
        draw_panel(sheet, draw, dark, (1558, y + 34), (240, 240), "dark-field", label_font)
        draw_panel(sheet, draw, warm, (1830, y + 34), (240, 240), "warm-field", label_font)
        draw_panel(sheet, draw, mask, (2102, y + 34), (240, 240), "alpha mask", label_font)

    draw.text((36, 1760), "No Batch 55 output is installed into Unity. Candidate folder only.", fill=(116, 47, 43), font=body_font)
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Starter Skill VFX Batch 55 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This batch validates that systematic asset production can run in Codex first, then move into Unity only after review. It produced symbolic starter-skill VFX candidates for Saiban, Nephthys, and Suzune. The batch intentionally draws no cat bodies.",
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
        "- Saiban: usable candidate. Shield arc, sword, sun-gold oath sigil, and bedline knockback language match the defender role. Watch item: the central cat paw emblem is acceptable as a game glyph but should be confirmed before Unity install.",
        "- Nephthys: strong candidate. Obelisk, moon-sand spiral, teal control rings, and royal eye mark match slow/mark control without drawing the cat.",
        "- Suzune: strong candidate. Bells, vermilion torii, moon-blue healing circle, talismans, and music notes match healing and sleep-stable recovery without drawing the cat.",
        "",
        "## Safety",
        "",
        "- Built-in image generation outputs were copied into the workspace candidate folder.",
        "- Chroma-key removal was done locally with the imagegen helper.",
        "- No candidate file was copied into `Assets`.",
        "- No Unity `.meta` files were created.",
        "- Formal Unity import remains blocked until an install batch chooses final sizes, Sprite import settings, VFX binding names, prefab/scene hookups, Console checks, and Play Mode screenshots.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['cat_id']}` / `{row['asset_type']}` -> `{row['candidate_path']}`")
    lines.append("")
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Starter Skill VFX Batch 55 Process Note",
        "",
        "Process: built-in image_gen output, workspace copy, local chroma-key alpha removal, deterministic review-pack generation.",
        "",
        "Final prompts used a flat `#00ff00` chroma-key background and requested isolated symbolic VFX only: no cat body, no human body, no bed, no enemies, no text, and no watermark.",
        "",
        "## Prompt Set",
        "",
        "- Saiban: isolated oath bedline defense VFX with silver-blue shield barrier, round sun-shield sigil, knockback wave strokes, and small gold sword-light slash.",
        "- Nephthys: isolated moon-sand control VFX with dark blue obelisk/pyramid sigil, quicksand field, sand-gold spiral grains, moon-teal slow rings, and royal eye mark.",
        "- Suzune: isolated lullaby healing/sleep-stable VFX with moon-blue healing field, vermilion torii light silhouette, gold kagura bells, paper talismans, crescent moon notes, and outward push rings.",
        "",
        "## Chroma-Key Removal",
        "",
        "Tool:",
        "",
        "```powershell",
        "C:\\Users\\PC\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\python\\python.exe C:\\Users\\PC\\.codex\\skills\\.system\\imagegen\\scripts\\remove_chroma_key.py --auto-key border --soft-matte --transparent-threshold 18 --opaque-threshold 200 --edge-contract 1 --despill",
        "```",
        "",
        "Observed helper summaries:",
        "",
        "- Saiban key `#0aee17`, transparent pixels `1130451/1572516`, partial pixels `87491/1572516`.",
        "- Nephthys key `#04f016`, transparent pixels `999218/1572516`, partial pixels `100017/1572516`.",
        "- Suzune key `#09f012`, transparent pixels `773159/1572516`, partial pixels `156096/1572516`.",
        "",
        f"Rows: {len(rows)}",
        "",
        "No Unity import was performed.",
        "",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def resolve_repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path.replace("/", "\\")


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def image_size(path: Path) -> str:
    image = Image.open(path)
    try:
        return f"{image.width}x{image.height}"
    finally:
        image.close()


if __name__ == "__main__":
    main()
