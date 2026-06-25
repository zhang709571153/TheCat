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
BATCH_SLUG = "batch_61_starter_skill_vfx_install_2026-06-15"
SOURCE_BATCH_SLUG = "batch_55_starter_skill_vfx_candidates_2026-06-15"
SOURCE_ROOT = (
    REPO_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "vfx"
    / "starter_skills"
    / SOURCE_BATCH_SLUG
)
CANDIDATE_ROOT = (
    REPO_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "vfx"
    / "starter_skills"
    / BATCH_SLUG
)
UNITY_VFX_ROOT = REPO_ROOT / "Assets" / "TheCat" / "Art" / "VFX"
MANIFEST_PATH = CANDIDATE_ROOT / "starter_skill_vfx_batch61_install_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_ROOT / "thecat_vfx_starter_skills_batch61_install_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_ROOT / "starter_skill_vfx_batch61_install_review.md"
PROCESS_NOTE_PATH = CANDIDATE_ROOT / "starter_skill_vfx_batch61_install_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_61_starter_skill_vfx_install.md"
SOURCE_AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_55_starter_skill_vfx_candidates.md"


@dataclass(frozen=True)
class StarterSkillVfxSpec:
    subject_id: str
    asset_id: str
    source_file: str
    unity_path: str
    runtime_binding_id: str
    source_lock_id: str
    reference_asset_ids: tuple[str, ...]
    mapped_skill_ids: tuple[str, ...]
    usage: str


ASSETS: tuple[StarterSkillVfxSpec, ...] = (
    StarterSkillVfxSpec(
        "saiban_bedline_skill_vfx",
        "thecat_vfx_saiban_bedline_skill_512_v001",
        "thecat_vfx_saiban_bedline_batch55_alpha_1024_candidate_v001.png",
        "Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png",
        "skill_vfx.saiban_bedline",
        "saiban_turnaround_colored",
        ("thecat_ui_blessing_oath_bedline_seal_128_v001", "thecat_vfx_bed_shield_pulse_256_v001"),
        ("saiban_oath_shield", "saiban_sword_sweep", "saiban_sun_charge"),
        "Saiban defense and bedline reset skill feedback VFX.",
    ),
    StarterSkillVfxSpec(
        "nephthys_moonsand_skill_vfx",
        "thecat_vfx_nephthys_moonsand_skill_512_v001",
        "thecat_vfx_nephthys_moonsand_batch55_alpha_1024_candidate_v001.png",
        "Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png",
        "skill_vfx.nephthys_moonsand",
        "nephthys_turnaround_colored",
        ("thecat_ui_blessing_dominion_sandglass_seal_128_v001", "thecat_vfx_enemy_mark_ring_256_v001"),
        ("nephthys_moon_sand_obelisk", "nephthys_quicksand_trap", "nephthys_royal_mark"),
        "Nephthys control, slow, and mark skill feedback VFX.",
    ),
    StarterSkillVfxSpec(
        "suzune_lullaby_skill_vfx",
        "thecat_vfx_suzune_lullaby_skill_512_v001",
        "thecat_vfx_suzune_lullaby_batch55_alpha_1024_candidate_v001.png",
        "Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png",
        "skill_vfx.suzune_lullaby",
        "suzune_turnaround_colored",
        ("thecat_ui_blessing_rhythm_lullaby_seal_128_v001", "thecat_vfx_sleep_stable_wave_256_v001"),
        ("suzune_sleep_bell", "suzune_healing_bell", "suzune_moon_torii"),
        "Suzune sleep, healing, and moon torii skill feedback VFX.",
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
    "source_candidate_size",
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
    "source_lock_id",
    "reference_asset_ids",
    "mapped_skill_ids",
    "recommendation",
)


def main() -> None:
    CANDIDATE_ROOT.mkdir(parents=True, exist_ok=True)
    UNITY_VFX_ROOT.mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    previews: list[tuple[StarterSkillVfxSpec, Path]] = []
    for spec in ASSETS:
        source_path = SOURCE_ROOT / spec.source_file
        unity_path = repo_path(spec.unity_path)
        if not source_path.exists():
            raise FileNotFoundError(source_path)

        install_resized_png(source_path, unity_path)
        write_sprite_meta(unity_path.with_suffix(unity_path.suffix + ".meta"))

        rows.append(
            {
                "subject_id": spec.subject_id,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "vfx",
                "unity_import_path": spec.unity_path,
                "unity_meta_path": spec.unity_path + ".meta",
                "installed_sha256": sha256(unity_path),
                "installed_size": image_size(unity_path),
                "source_candidate_path": to_repo_path(source_path),
                "source_candidate_sha256": sha256(source_path),
                "source_candidate_size": image_size(source_path),
                "source_candidate_batch": SOURCE_BATCH_SLUG,
                "source_candidate_review_note": to_repo_path(SOURCE_ROOT / "starter_skill_vfx_batch55_candidate_review.md"),
                "source_candidate_agent_prompt": to_repo_path(SOURCE_AGENT_PROMPT_PATH),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "runtime_binding_id": spec.runtime_binding_id,
                "runtime_surface_id": "battle_feedback",
                "runtime_slot_id": "starter_skill_vfx",
                "source_lock_id": spec.source_lock_id,
                "reference_asset_ids": "|".join(spec.reference_asset_ids),
                "mapped_skill_ids": "|".join(spec.mapped_skill_ids),
                "recommendation": "installed_symbolic_starter_skill_vfx_pending_unity_timing_smoke",
            }
        )
        previews.append((spec, unity_path))

    write_manifest(rows)
    write_review_sheet(rows, previews)
    write_review_note(rows)
    write_process_note(rows)
    print(f"Installed {len(rows)} starter skill VFX asset(s).")
    print(to_repo_path(MANIFEST_PATH))


def install_resized_png(source_path: Path, unity_path: Path) -> None:
    image = Image.open(source_path).convert("RGBA")
    try:
        resized = image.resize((512, 512), Image.Resampling.LANCZOS)
        unity_path.parent.mkdir(parents=True, exist_ok=True)
        resized.save(unity_path, "PNG")
    finally:
        image.close()


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(rows: list[dict[str, str]], previews: list[tuple[StarterSkillVfxSpec, Path]]) -> None:
    sheet = Image.new("RGBA", (1540, 620), (30, 28, 40, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(18)
    body_font = load_font(14)
    draw.text((36, 28), "P0 Batch 61 - starter skill VFX install", fill=(255, 244, 212), font=title_font)
    draw.text(
        (36, 76),
        "Promoted reviewed Batch 55 symbolic VFX. No cat body art is installed or redrawn in this batch.",
        fill=(227, 207, 178),
        font=body_font,
    )

    for index, (spec, asset_path) in enumerate(previews):
        x = 42 + index * 492
        y = 132
        image = Image.open(asset_path).convert("RGBA")
        draw.rounded_rectangle((x - 14, y - 14, x + 452, y + 424), radius=9, fill=(248, 244, 235), outline=(184, 164, 132))
        draw.text((x, y), spec.subject_id, fill=(42, 36, 32), font=label_font)
        draw_checker(draw, (x, y + 34, x + 224, y + 258), 16)
        preview = image.copy()
        preview.thumbnail((208, 208), Image.Resampling.LANCZOS)
        sheet.alpha_composite(preview, (x + (224 - preview.width) // 2, y + 34 + (224 - preview.height) // 2))
        draw.text((x + 244, y + 44), spec.asset_id, fill=(60, 55, 49), font=body_font)
        draw.text((x + 244, y + 72), spec.runtime_binding_id, fill=(60, 55, 49), font=body_font)
        draw.text((x + 244, y + 104), "source lock: " + spec.source_lock_id, fill=(94, 66, 54), font=body_font)
        draw.text((x, y + 282), wrap(spec.usage, 48), fill=(74, 68, 62), font=body_font)
        draw.text((x, y + 334), wrap("skills: " + ", ".join(spec.mapped_skill_ids), 48), fill=(74, 68, 62), font=body_font)
        image.close()

    sheet.save(REVIEW_SHEET_PATH, "PNG")


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 61 - Starter Skill VFX Install Review",
        "",
        "## Decision",
        "",
        "Installed three symbolic starter skill VFX assets from Batch 55 candidates into Unity.",
        "This batch contains no cat bodies, no cat portraits, no new cat silhouettes, and no AI-generated cat-body replacement.",
        "",
        "## Runtime Scope",
        "",
        "- Saiban skill feedback routes to the bedline shield/sword/sun oath VFX.",
        "- Nephthys skill feedback routes to the moon-sand obelisk/control VFX.",
        "- Suzune skill feedback routes to the lullaby bell/torii/healing VFX.",
        "- Generic hit, shield, sleep, litter, feeder, and mark VFX remain as fallbacks.",
        "",
        "## Rows",
        "",
    ]
    for row in rows:
        lines.append(
            f"- `{row['asset_id']}` -> `{row['unity_import_path']}` "
            f"binding `{row['runtime_binding_id']}` source lock `{row['source_lock_id']}` sha256 `{row['installed_sha256']}`"
        )

    lines.extend(
        [
            "",
            "## Cat Consistency Boundary",
            "",
            "- The source locks are used as authority-symbol locks only.",
            "- No starter-cat body art is imported from Batch 55.",
            "- Any future cat-body replacement remains blocked by colored three-view turnaround comparison and active-cat Unity screenshots.",
            "",
            "## Pending Unity Checks",
            "",
            "- Refresh AssetDatabase and inspect Sprite import settings.",
            "- Trigger one Saiban, Nephthys, and Suzune skill in Play Mode and capture feedback screenshots.",
            "- Confirm VFX scale, timing, alpha edges, and Console cleanliness.",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 61 Starter Skill VFX Install Process Note",
        "",
        f"- Source candidate batch: `{SOURCE_BATCH_SLUG}`.",
        "- Action: resized accepted 1024 alpha candidate PNGs to 512 and installed them into `Assets/TheCat/Art/VFX`.",
        "- Generated deterministic `.png.meta` files with `TheCatP0ImportSettings:v1`.",
        "- Asset type: `vfx`.",
        "- Cat consistency: symbolic VFX only; no cat body candidate was imported.",
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
