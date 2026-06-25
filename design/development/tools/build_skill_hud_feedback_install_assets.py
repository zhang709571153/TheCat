from __future__ import annotations

import csv
import hashlib
import shutil
import textwrap
import uuid
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_60_skill_hud_feedback_install_2026-06-15"
SOURCE_BATCH_SLUG = "batch_57_skill_hud_feedback_candidates_2026-06-15"
SOURCE_ROOT = (
    REPO_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "ui"
    / "skill_hud"
    / SOURCE_BATCH_SLUG
)
CANDIDATE_ROOT = (
    REPO_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "ui"
    / "skill_hud"
    / BATCH_SLUG
)
UNITY_ICON_ROOT = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons"
MANIFEST_PATH = CANDIDATE_ROOT / "skill_hud_feedback_batch60_install_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_ROOT / "thecat_ui_skill_hud_feedback_batch60_install_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_ROOT / "skill_hud_feedback_batch60_install_review.md"
PROCESS_NOTE_PATH = CANDIDATE_ROOT / "skill_hud_feedback_batch60_install_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_60_skill_hud_feedback_install.md"
BATCH57_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_57_skill_hud_feedback_candidates.md"


@dataclass(frozen=True)
class SkillHudFeedbackSpec:
    subject_id: str
    asset_id: str
    source_file: str
    unity_path: str
    runtime_binding_id: str
    runtime_surface_id: str
    runtime_slot_id: str
    usage: str


ASSETS: tuple[SkillHudFeedbackSpec, ...] = (
    SkillHudFeedbackSpec(
        "skill_ready_frame",
        "thecat_ui_skill_ready_frame_512_v001",
        "thecat_ui_skill_ready_frame_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png",
        "skill_hud.ready_frame",
        "skill_hud",
        "ready_frame",
        "Ready skill state frame/icon for available skill buttons.",
    ),
    SkillHudFeedbackSpec(
        "skill_cooldown_overlay",
        "thecat_ui_skill_cooldown_overlay_512_v001",
        "thecat_ui_skill_cooldown_overlay_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png",
        "skill_hud.cooldown_overlay",
        "skill_hud",
        "cooldown_overlay",
        "Cooldown state overlay/icon beside disabled cooldown skill buttons.",
    ),
    SkillHudFeedbackSpec(
        "skill_no_target_marker",
        "thecat_ui_skill_no_target_marker_512_v001",
        "thecat_ui_skill_no_target_marker_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png",
        "skill_hud.no_target_marker",
        "skill_hud",
        "no_target_marker",
        "No-target state marker for skills that require an enemy target.",
    ),
    SkillHudFeedbackSpec(
        "skill_hunger_cost_chip",
        "thecat_ui_skill_hunger_cost_chip_512_v001",
        "thecat_ui_skill_hunger_cost_chip_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png",
        "skill_hud.hunger_cost_chip",
        "skill_hud",
        "hunger_cost_chip",
        "Low-hunger and hunger-cost chip for light-penalty skill clarity.",
    ),
    SkillHudFeedbackSpec(
        "auto_target_reticle",
        "thecat_ui_auto_target_reticle_512_v001",
        "thecat_ui_auto_target_reticle_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png",
        "skill_hud.auto_target_reticle",
        "skill_hud",
        "auto_target_reticle",
        "Auto-target reticle shown for target-resolved skill cards.",
    ),
    SkillHudFeedbackSpec(
        "interaction_range_ripple",
        "thecat_ui_interaction_range_ripple_512_v001",
        "thecat_ui_interaction_range_ripple_batch57_alpha_512_candidate_v001.png",
        "Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png",
        "battle_hud.interaction_range_ripple",
        "battle_hud",
        "interaction_range_ripple",
        "Interaction range ripple for bed/litter/feeder affordance area.",
    ),
)


FIELD_NAMES = (
    "subject_id",
    "batch_slug",
    "asset_id",
    "asset_type",
    "unity_import_path",
    "unity_meta_path",
    "installed_sha256",
    "installed_size",
    "source_candidate_path",
    "source_candidate_sha256",
    "source_candidate_batch",
    "source_candidate_review_note",
    "source_candidate_agent_prompt",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "runtime_binding_id",
    "runtime_surface_id",
    "runtime_slot_id",
    "recommendation",
)


def main() -> None:
    CANDIDATE_ROOT.mkdir(parents=True, exist_ok=True)
    UNITY_ICON_ROOT.mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    previews: list[tuple[SkillHudFeedbackSpec, Path]] = []
    for spec in ASSETS:
        source_path = SOURCE_ROOT / spec.source_file
        unity_path = repo_path(spec.unity_path)
        if not source_path.exists():
            raise FileNotFoundError(source_path)

        shutil.copyfile(source_path, unity_path)
        write_sprite_meta(unity_path.with_suffix(unity_path.suffix + ".meta"))

        rows.append(
            {
                "subject_id": spec.subject_id,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "skill_hud_feedback",
                "unity_import_path": spec.unity_path,
                "unity_meta_path": spec.unity_path + ".meta",
                "installed_sha256": sha256(unity_path),
                "installed_size": image_size(unity_path),
                "source_candidate_path": to_repo_path(source_path),
                "source_candidate_sha256": sha256(source_path),
                "source_candidate_batch": SOURCE_BATCH_SLUG,
                "source_candidate_review_note": to_repo_path(SOURCE_ROOT / "skill_hud_feedback_batch57_candidate_review.md"),
                "source_candidate_agent_prompt": to_repo_path(BATCH57_PROMPT_PATH),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "runtime_binding_id": spec.runtime_binding_id,
                "runtime_surface_id": spec.runtime_surface_id,
                "runtime_slot_id": spec.runtime_slot_id,
                "recommendation": "installed_non_cat_skill_hud_feedback_pending_unity_visual_smoke",
            }
        )
        previews.append((spec, unity_path))

    write_manifest(rows)
    write_review_sheet(rows, previews)
    write_review_note(rows)
    write_process_note(rows)
    print(f"Installed {len(rows)} skill HUD feedback asset(s).")
    print(to_repo_path(MANIFEST_PATH))


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(rows: list[dict[str, str]], previews: list[tuple[SkillHudFeedbackSpec, Path]]) -> None:
    sheet = Image.new("RGBA", (1860, 980), (245, 241, 234, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(18)
    body_font = load_font(14)
    draw.text((36, 28), "P0 Batch 60 - skill HUD feedback install", fill=(42, 36, 32), font=title_font)
    draw.text(
        (36, 76),
        "Formal Unity install from Batch 57 non-cat candidate PNGs. Runtime hooks cover skill state cues and interaction range hint.",
        fill=(106, 55, 42),
        font=body_font,
    )

    for index, (spec, asset_path) in enumerate(previews):
        x = 42 + (index % 3) * 600
        y = 132 + (index // 3) * 392
        image = Image.open(asset_path).convert("RGBA")
        draw.rounded_rectangle((x - 14, y - 14, x + 544, y + 334), radius=9, fill=(255, 252, 247), outline=(178, 158, 130))
        draw.text((x, y), spec.subject_id, fill=(42, 36, 32), font=label_font)
        draw_checker(draw, (x, y + 34, x + 224, y + 258), 16)
        preview = image.copy()
        preview.thumbnail((196, 196), Image.Resampling.LANCZOS)
        sheet.alpha_composite(preview, (x + (224 - preview.width) // 2, y + 34 + (224 - preview.height) // 2))
        draw.text((x + 244, y + 44), spec.asset_id, fill=(60, 55, 49), font=body_font)
        draw.text((x + 244, y + 72), spec.runtime_binding_id, fill=(60, 55, 49), font=body_font)
        draw.text((x + 244, y + 108), wrap(spec.usage, 34), fill=(74, 68, 62), font=body_font)

    sheet.save(REVIEW_SHEET_PATH, "PNG")


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 60 - Skill HUD Feedback Install Review",
        "",
        "## Decision",
        "",
        "Installed six non-cat Skill HUD feedback assets from Batch 57 candidates into Unity.",
        "This batch contains no cat bodies, no cat portraits, and no starter-cat turnaround crops.",
        "",
        "## Runtime Scope",
        "",
        "- Ready, cooldown, no-target, and low-hunger skill cards now resolve state feedback assets.",
        "- Target-resolved skill cards can surface the auto-target reticle.",
        "- Interaction controls can surface the interaction-range ripple.",
        "",
        "## Rows",
        "",
    ]
    for row in rows:
        lines.append(
            f"- `{row['asset_id']}` -> `{row['unity_import_path']}` "
            f"binding `{row['runtime_binding_id']}` sha256 `{row['installed_sha256']}`"
        )

    lines.extend(
        [
            "",
            "## Pending Unity Checks",
            "",
            "- Refresh AssetDatabase and inspect Sprite import settings.",
            "- Run a battle HUD screenshot pass for skill-state readability at runtime scale.",
            "- Confirm Console remains clean after HUD icons are resolved in play mode.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 60 Skill HUD Feedback Install Process Note",
        "",
        f"- Source candidate batch: `{SOURCE_BATCH_SLUG}`.",
        "- Action: copied accepted alpha candidate PNGs into Unity `Assets/TheCat/Art/UI/Icons`.",
        "- Generated deterministic `.png.meta` files with `TheCatP0ImportSettings:v1`.",
        "- Asset type: `skill_hud_feedback`.",
        "- Cat consistency: non-cat UI/VFX only; starter-cat assets were not read or modified.",
        "",
        "## Installed Assets",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['asset_id']}` from `{row['source_candidate_path']}`")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_sprite_meta(path: Path) -> None:
    guid = uuid.uuid5(uuid.NAMESPACE_URL, to_repo_path(path.with_suffix(""))).hex
    sprite_id = uuid.uuid5(uuid.NAMESPACE_DNS, to_repo_path(path.with_suffix(""))).hex[:24] + "00000000"
    path.write_text(
        f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
    flipGreenChannel: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMipmapLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 0
    wrapV: 0
    wrapW: 0
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {{x: 0.5, y: 0.5}}
  spritePixelsToUnits: 100
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  swizzle: 50462976
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 0
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  - serializedVersion: 4
    buildTarget: Standalone
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 0
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    customData:
    physicsShape: []
    bones: []
    spriteID: {sprite_id}
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    spriteCustomMetadata:
      entries: []
    nameFileIdTable: {{}}
  mipmapLimitGroupName:
  pSDRemoveMatte: 0
  userData: TheCatP0ImportSettings:v1
  assetBundleName:
  assetBundleVariant:
""",
        encoding="utf-8",
    )


def draw_checker(draw: ImageDraw.ImageDraw, rect: tuple[int, int, int, int], cell: int) -> None:
    x0, y0, x1, y1 = rect
    for y in range(y0, y1, cell):
        for x in range(x0, x1, cell):
            fill = (226, 218, 205, 255) if ((x // cell) + (y // cell)) % 2 == 0 else (246, 241, 234, 255)
            draw.rectangle((x, y, min(x + cell, x1), min(y + cell, y1)), fill=fill)
    draw.rectangle(rect, outline=(151, 131, 108, 255), width=2)


def repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path.replace("/", "\\")


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def image_size(path: Path) -> str:
    image = Image.open(path)
    try:
        return f"{image.width}x{image.height}"
    finally:
        image.close()


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


def wrap(text: str, width: int) -> str:
    return "\n".join(textwrap.wrap(text, width=width))


def sha256(path: Path) -> str:
    hasher = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


if __name__ == "__main__":
    main()
