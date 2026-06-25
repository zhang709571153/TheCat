from __future__ import annotations

import csv
import hashlib
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_46_p0_asset_production_dashboard_2026-06-15"
CANDIDATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "p0_asset_dashboard"
BATCH_DIR = CANDIDATE_ROOT / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "p0_asset_batch46_production_dashboard_manifest.csv"
REVIEW_SHEET_PATH = BATCH_DIR / "thecat_p0_asset_batch46_production_dashboard_review_sheet.png"
REVIEW_NOTE_PATH = BATCH_DIR / "p0_asset_batch46_production_dashboard_review.md"
PROCESS_NOTE_PATH = BATCH_DIR / "p0_asset_batch46_production_dashboard_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_46_p0_asset_production_dashboard.md"

DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"
CAT_BATCH45_MANIFEST = "design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv"


@dataclass(frozen=True)
class EnemyDashboardSpec:
    subject_id: str
    display_name: str
    combat_role: str
    manifest_path: str
    preview_asset_type: str
    source_lock_ids: str
    concept_source_path: str
    animation_source_path: str
    current_runtime_asset_path: str
    current_runtime_asset_type: str
    unity_install_target_path: str
    active_screenshot: str
    next_action: str


ENEMIES: tuple[EnemyDashboardSpec, ...] = (
    EnemyDashboardSpec(
        "enemy:black_mud_nightmare",
        "Black Mud Nightmare",
        "P0 melee bed-pressure swarmer",
        "design/development/asset_candidates/enemies/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_manifest.csv",
        "cutout_alpha_512_preview",
        "black_mud_concept;black_mud_animation",
        DESIGN_ROOT + "/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png",
        DESIGN_ROOT + "/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png",
        "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png",
        "installed_combat_sprite",
        "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png",
        "07-active-enemy-black-mud.png",
        "Compare Batch 40 cutout against the active-enemy screenshot, then replace the runtime sprite only after Console and prefab binding checks pass.",
    ),
    EnemyDashboardSpec(
        "enemy:cold_light_shadow",
        "Cold Light Shadow",
        "P0 ranged player-pressure caster",
        "design/development/asset_candidates/enemies/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_manifest.csv",
        "cutout_beam_alpha_512_preview",
        "cold_light_concept;cold_light_animation",
        DESIGN_ROOT + "/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png",
        DESIGN_ROOT + "/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png",
        "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png",
        "installed_combat_sprite",
        "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png",
        "08-active-enemy-cold-light.png",
        "Compare Batch 42 beam-preserving cutout against the active-enemy screenshot, then replace the runtime sprite only after warning-beam readability passes.",
    ),
    EnemyDashboardSpec(
        "enemy:call_tyrant",
        "Call Tyrant",
        "P0 Boss summon and app-throw pressure",
        "design/development/asset_candidates/enemies/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_manifest.csv",
        "cutout_boss_alpha_512_preview",
        "call_tyrant_concept;call_tyrant_animation",
        DESIGN_ROOT + "/enemies/en06_call_tyrant/concept/call_tyrant_concept.png",
        DESIGN_ROOT + "/enemies/en06_call_tyrant/animation/call_tyrant_animation.png",
        "Assets/TheCat/Art/Enemies/Concepts/thecat_enemy_calltyrant_concept_2048_v001.png",
        "installed_boss_concept_proxy",
        "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_calltyrant_combat_sprite_512_v001.png",
        "09-active-enemy-call-tyrant.png",
        "Create a formal boss combat sprite binding only after Batch 44 cutout, app-throw VFX, summon VFX, and active Boss screenshot are reviewed together.",
    ),
)


FIELD_NAMES = (
    "subject_id",
    "display_name",
    "category",
    "combat_role",
    "batch_slug",
    "source_lock_ids",
    "source_reference_paths",
    "source_reference_sha256s",
    "current_runtime_asset_path",
    "current_runtime_asset_sha256",
    "current_runtime_asset_type",
    "latest_candidate_preview_path",
    "latest_candidate_preview_sha256",
    "latest_candidate_manifest",
    "latest_candidate_batch_slug",
    "active_screenshot",
    "unity_install_target_path",
    "unity_validation_gate",
    "next_action",
    "blockers",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    rows = build_cat_rows()
    rows.extend(build_enemy_rows())
    write_manifest(rows)
    write_review_sheet(rows)
    write_review_note(rows)
    write_process_note(rows)
    print(f"Wrote {len(rows)} P0 asset dashboard row(s).")
    print(to_repo_path(MANIFEST_PATH))


def build_cat_rows() -> list[dict[str, str]]:
    cat_rows = read_manifest(resolve_repo_path(CAT_BATCH45_MANIFEST))
    output: list[dict[str, str]] = []

    for row in cat_rows:
        subject_id = "cat:" + row["cat_id"]
        source_path = row["source_turnaround_path"]
        runtime_path = row["unity_sprite_path"]
        candidate_path = row["latest_cutout_preview_path"]
        output.append(
            {
                "subject_id": subject_id,
                "display_name": row["display_name"],
                "category": "starter_cat",
                "combat_role": starter_cat_role(row["cat_id"]),
                "batch_slug": BATCH_SLUG,
                "source_lock_ids": row["source_lock_id"],
                "source_reference_paths": source_path,
                "source_reference_sha256s": sha256(resolve_repo_path(source_path)),
                "current_runtime_asset_path": runtime_path,
                "current_runtime_asset_sha256": sha256(resolve_repo_path(runtime_path)),
                "current_runtime_asset_type": "installed_combat_sprite",
                "latest_candidate_preview_path": candidate_path,
                "latest_candidate_preview_sha256": sha256(resolve_repo_path(candidate_path)),
                "latest_candidate_manifest": row["latest_cutout_manifest"],
                "latest_candidate_batch_slug": Path(row["latest_cutout_manifest"]).parent.name,
                "active_screenshot": row["active_screenshot"],
                "unity_install_target_path": runtime_path,
                "unity_validation_gate": "active-cat screenshot, Console clean, Sprite import settings, prefab/scene binding",
                "next_action": "Do not generate or install a replacement until the colored three-view turnaround and active-cat screenshot agree.",
                "blockers": "Unity editor screenshot capture and human conformance review remain pending.",
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "dashboard_only_unity_validation_pending",
            }
        )

    return output


def build_enemy_rows() -> list[dict[str, str]]:
    output: list[dict[str, str]] = []

    for spec in ENEMIES:
        manifest = read_manifest(resolve_repo_path(spec.manifest_path))
        preview = first_row(manifest, "asset_type", spec.preview_asset_type)
        source_paths = f"{spec.concept_source_path};{spec.animation_source_path}"
        source_hashes = ";".join(
            [
                sha256(resolve_repo_path(spec.concept_source_path)),
                sha256(resolve_repo_path(spec.animation_source_path)),
            ]
        )
        output.append(
            {
                "subject_id": spec.subject_id,
                "display_name": spec.display_name,
                "category": "core_enemy",
                "combat_role": spec.combat_role,
                "batch_slug": BATCH_SLUG,
                "source_lock_ids": spec.source_lock_ids,
                "source_reference_paths": source_paths,
                "source_reference_sha256s": source_hashes,
                "current_runtime_asset_path": spec.current_runtime_asset_path,
                "current_runtime_asset_sha256": sha256(resolve_repo_path(spec.current_runtime_asset_path)),
                "current_runtime_asset_type": spec.current_runtime_asset_type,
                "latest_candidate_preview_path": preview["candidate_path"],
                "latest_candidate_preview_sha256": sha256(resolve_repo_path(preview["candidate_path"])),
                "latest_candidate_manifest": spec.manifest_path,
                "latest_candidate_batch_slug": Path(spec.manifest_path).parent.name,
                "active_screenshot": spec.active_screenshot,
                "unity_install_target_path": spec.unity_install_target_path,
                "unity_validation_gate": "active-enemy screenshot, Console clean, Sprite import settings, prefab/scene binding",
                "next_action": spec.next_action,
                "blockers": "Unity editor screenshot capture and runtime enemy/boss readability review remain pending.",
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "dashboard_only_unity_validation_pending",
            }
        )

    return output


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(rows: list[dict[str, str]]) -> None:
    sheet = Image.new("RGBA", (3200, 1800), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(42)
    subtitle_font = load_font(20)
    body_font = load_font(16)

    draw.text((48, 34), "P0 Batch 46 - asset production dashboard", fill=(42, 36, 32), font=title_font)
    draw.text(
        (48, 88),
        "Codex may produce candidate art; Unity remains the install and runtime validation gate. Candidate-only dashboard; do not import into Unity.",
        fill=(116, 47, 43),
        font=subtitle_font,
    )

    tile_positions = (
        (54, 150),
        (1624, 150),
        (54, 690),
        (1624, 690),
        (54, 1230),
        (1624, 1230),
    )

    for row, position in zip(rows, tile_positions):
        draw_tile(sheet, row, position)

    draw.text(
        (48, 1754),
        "Gate rule: any future replacement must preserve source-lock identity, pass screenshot comparison, and update manifest/runtime records in the same batch.",
        fill=(78, 68, 60),
        font=body_font,
    )
    sheet.save(REVIEW_SHEET_PATH)


def draw_tile(sheet: Image.Image, row: dict[str, str], position: tuple[int, int]) -> None:
    x, y = position
    width, height = 1518, 500
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(24)
    label_font = load_font(15)
    body_font = load_font(13)
    small_font = load_font(11)

    draw.rounded_rectangle((x, y, x + width, y + height), radius=10, fill=(255, 252, 246), outline=(178, 158, 130), width=2)
    draw.text((x + 24, y + 18), row["display_name"], fill=(42, 36, 32), font=title_font)
    draw.text((x + 24, y + 50), row["combat_role"], fill=(78, 68, 60), font=body_font)
    draw.text((x + 24, y + 72), row["subject_id"], fill=(116, 47, 43), font=body_font)

    source_preview_path = row["source_reference_paths"].split(";")[0]
    panels = (
        ("source lock", source_preview_path, (x + 24, y + 110), (280, 230)),
        ("current Unity", row["current_runtime_asset_path"], (x + 332, y + 110), (230, 230)),
        ("latest candidate", row["latest_candidate_preview_path"], (x + 590, y + 110), (230, 230)),
    )
    for label, path, panel_position, panel_size in panels:
        draw_image_panel(sheet, draw, resolve_repo_path(path), panel_position, panel_size, label, label_font)

    text_x = x + 850
    text_y = y + 112
    text_lines = [
        ("source lock", row["source_lock_ids"]),
        ("active screenshot", row["active_screenshot"]),
        ("candidate batch", row["latest_candidate_batch_slug"]),
        ("install target", row["unity_install_target_path"]),
        ("status", row["recommendation"]),
    ]

    for label, value in text_lines:
        draw.text((text_x, text_y), label.upper(), fill=(116, 47, 43), font=small_font)
        text_y += 15
        for line in wrap(value, 62):
            draw.text((text_x, text_y), line, fill=(42, 36, 32), font=body_font)
            text_y += 17
        text_y += 5

    text_y = y + 365
    draw.text((x + 24, text_y), "next action", fill=(116, 47, 43), font=label_font)
    text_y += 22
    for line in wrap(row["next_action"], 112):
        draw.text((x + 24, text_y), line, fill=(42, 36, 32), font=body_font)
        text_y += 18

    draw.text((x + 24, y + height - 34), row["blockers"], fill=(116, 47, 43), font=small_font)


def draw_image_panel(
    sheet: Image.Image,
    draw: ImageDraw.ImageDraw,
    path: Path,
    position: tuple[int, int],
    size: tuple[int, int],
    label: str,
    label_font: ImageFont.ImageFont,
) -> None:
    x, y = position
    width, height = size
    draw.rounded_rectangle((x, y, x + width, y + height), radius=8, fill=(240, 233, 222), outline=(181, 166, 141))
    draw_checker(draw, (x + 8, y + 8, x + width - 8, y + height - 30))
    image = Image.open(path).convert("RGBA")
    fitted = fit_image(image, (width - 18, height - 46))
    px = x + (width - fitted.width) // 2
    py = y + 10 + (height - 48 - fitted.height) // 2
    sheet.alpha_composite(fitted, (px, py))
    draw.text((x + 10, y + height - 24), label, fill=(42, 36, 32), font=label_font)


def draw_checker(draw: ImageDraw.ImageDraw, bounds: tuple[int, int, int, int]) -> None:
    x1, y1, x2, y2 = bounds
    cell = 16
    for yy in range(y1, y2, cell):
        for xx in range(x1, x2, cell):
            tone = (228, 222, 212) if ((xx // cell) + (yy // cell)) % 2 == 0 else (249, 246, 240)
            draw.rectangle((xx, yy, min(xx + cell, x2), min(yy + cell, y2)), fill=tone)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    output = image.copy()
    output.thumbnail(size, Image.Resampling.LANCZOS)
    return output


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Asset Batch 46 Production Dashboard Review",
        "",
        "Decision: candidate-only production dashboard; do not import into Unity.",
        "",
        "This batch answers the production question: Codex can generate, clean, compare, and package candidate art outside Unity, while Unity remains the install-time and runtime validation gate.",
        "",
        "## Scope",
        "",
        "- Covers the three starter cats and three P0 core enemy/Boss subjects.",
        "- Consolidates source lock, current runtime asset, latest candidate preview, install target, and active screenshot gate.",
        "- Generates no new model art and performs no Unity asset install.",
        "- Keeps all Batch 46 outputs outside `Assets`.",
        "- Creates no Unity `.meta` files.",
        "- Explicitly preserves the starter-cat colored three-view turnaround gate.",
        "- Marks all rows as Unity validation pending.",
        "",
        "## Outputs",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Dashboard Rows",
        "",
    ]

    for row in rows:
        lines.extend(
            [
                f"### {row['display_name']}",
                "",
                f"- Subject: `{row['subject_id']}`",
                f"- Source lock: `{row['source_lock_ids']}`",
                f"- Current Unity asset: `{row['current_runtime_asset_path']}`",
                f"- Latest candidate preview: `{row['latest_candidate_preview_path']}`",
                f"- Install target: `{row['unity_install_target_path']}`",
                f"- Active screenshot: `{row['active_screenshot']}`",
                f"- Unity validation gate: {row['unity_validation_gate']}",
                f"- Next action: {row['next_action']}",
                "",
            ]
        )

    lines.extend(
        [
            "## Blocking Items",
            "",
            "- Unity MCP/editor validation is still pending for active-cat and active-enemy screenshots.",
            "- Console, AssetDatabase refresh, Sprite import settings, prefab/scene binding, and screenshot readability must pass before any candidate is installed.",
            "- Call Tyrant still needs a formal boss combat sprite binding decision because the current runtime asset is a concept proxy.",
            "",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Asset Batch 46 Process Note",
        "",
        "Process: deterministic local dashboard composition only.",
        "",
        "- No image model call was made in this batch.",
        "- No candidate file was copied into `Assets`.",
        "- No Unity `.meta` file was created.",
        "- The dashboard exists to make the Codex-to-Unity production boundary explicit.",
        "- Codex-side asset production is allowed for future batches: source-lock references, model output, clean-up, alpha preparation, manifest rows, review sheet, and process notes can all be produced outside Unity.",
        "- Unity-side work remains required for import settings, prefab/scene connection, Play Mode screenshots, Console checks, and runtime feel validation.",
        "",
        "Row count: " + str(len(rows)),
        "",
        "Required active screenshot gates:",
    ]
    for row in rows:
        lines.append(f"- `{row['subject_id']}` -> `{row['active_screenshot']}`")
    lines.append("")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def read_manifest(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8-sig", newline="") as handle:
        return list(csv.DictReader(handle))


def first_row(rows: list[dict[str, str]], key: str, value: str) -> dict[str, str]:
    for row in rows:
        if row.get(key) == value:
            return row
    raise ValueError(f"Missing manifest row where {key}={value}")


def starter_cat_role(cat_id: str) -> str:
    roles = {
        "saiban": "P0 defender and bedline shield",
        "nephthys": "P0 controller and mark/slow setup",
        "suzune": "P0 healer and sleep-stable support",
    }
    return roles[cat_id]


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


def load_font(size: int) -> ImageFont.ImageFont:
    font_candidates = [
        Path("C:/Windows/Fonts/msyh.ttc"),
        Path("C:/Windows/Fonts/arial.ttf"),
    ]
    for path in font_candidates:
        if path.exists():
            return ImageFont.truetype(str(path), size)
    return ImageFont.load_default()


def wrap(text: str, width: int) -> list[str]:
    return textwrap.wrap(text, width=width, break_long_words=False, break_on_hyphens=False) or [""]


if __name__ == "__main__":
    main()
