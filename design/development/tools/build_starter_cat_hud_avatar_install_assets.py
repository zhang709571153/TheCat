from __future__ import annotations

import csv
import hashlib
import textwrap
import uuid
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_58_starter_cat_hud_avatar_install_2026-06-15"
CANDIDATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
UNITY_ICON_ROOT = REPO_ROOT / "Assets" / "TheCat" / "Art" / "UI" / "Icons"
MANIFEST_PATH = CANDIDATE_ROOT / "starter_cat_batch58_hud_avatar_install_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_ROOT / "thecat_starter_cat_batch58_hud_avatar_install_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_ROOT / "starter_cat_batch58_hud_avatar_install_review.md"
PROCESS_NOTE_PATH = CANDIDATE_ROOT / "starter_cat_batch58_hud_avatar_install_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_58_starter_cat_hud_avatar_install.md"
DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"


@dataclass(frozen=True)
class CatAvatarSpec:
    cat_id: str
    display_name: str
    source_lock_id: str
    source_turnaround_path: str
    locked_sprite_path: str
    asset_id: str
    unity_path: str
    required_traits: tuple[str, ...]


CATS: tuple[CatAvatarSpec, ...] = (
    CatAvatarSpec(
        "saiban",
        "Saiban / Sword Saint",
        "saiban_turnaround_colored",
        DESIGN_ROOT + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png",
        "thecat_cat_saiban_hud_avatar_256_v001",
        "Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png",
        (
            "silver-gray tabby face markings",
            "helmet and red cape collar read",
            "silver armor and gold trim stay visible",
            "non-human cat muzzle, ears, and whiskers preserved",
        ),
    ),
    CatAvatarSpec(
        "nephthys",
        "Nephthys / Moon-Sand Agent",
        "nephthys_turnaround_colored",
        DESIGN_ROOT + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
        "thecat_cat_nephthys_hud_avatar_256_v001",
        "Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png",
        (
            "gold-brown tabby face and golden eyes",
            "deep navy hood silhouette",
            "sand-gold trim and blue gem accents",
            "non-human cat muzzle, ears, and whiskers preserved",
        ),
    ),
    CatAvatarSpec(
        "suzune",
        "Suzune / Sleep Shrine Healer",
        "suzune_turnaround_colored",
        DESIGN_ROOT + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
        "thecat_cat_suzune_hud_avatar_256_v001",
        "Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png",
        (
            "calico orange, black, and white face markings",
            "flower and bell ornament read",
            "vermilion and white shrine outfit collar visible",
            "non-human cat muzzle, ears, and whiskers preserved",
        ),
    ),
)


FIELD_NAMES = (
    "cat_id",
    "display_name",
    "batch_slug",
    "asset_id",
    "asset_type",
    "unity_import_path",
    "unity_meta_path",
    "installed_sha256",
    "installed_size",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "source_lock_id",
    "locked_sprite_path",
    "locked_sprite_sha256",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


def main() -> None:
    CANDIDATE_ROOT.mkdir(parents=True, exist_ok=True)
    UNITY_ICON_ROOT.mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    previews: list[tuple[CatAvatarSpec, Path]] = []

    for spec in CATS:
        source_path = resolve_repo_path(spec.source_turnaround_path)
        sprite_path = resolve_repo_path(spec.locked_sprite_path)
        unity_path = repo_path(spec.unity_path)

        sprite = Image.open(sprite_path).convert("RGBA")
        avatar = make_avatar(sprite, 256)
        avatar.save(unity_path, "PNG")
        write_sprite_meta(unity_path.with_suffix(unity_path.suffix + ".meta"))

        previews.append((spec, unity_path))
        rows.append(
            {
                "cat_id": spec.cat_id,
                "display_name": spec.display_name,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "avatar_icon",
                "unity_import_path": spec.unity_path,
                "unity_meta_path": spec.unity_path + ".meta",
                "installed_sha256": sha256(unity_path),
                "installed_size": "256x256",
                "source_turnaround_path": spec.source_turnaround_path,
                "source_turnaround_sha256": sha256(source_path),
                "source_lock_id": spec.source_lock_id,
                "locked_sprite_path": spec.locked_sprite_path,
                "locked_sprite_sha256": sha256(sprite_path),
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "installed_source_locked_avatar_pending_unity_visual_smoke",
            }
        )

    write_review_sheet(rows, previews)
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print(f"Wrote and installed {len(rows)} source-locked starter-cat HUD avatar(s).")
    print(to_repo_path(MANIFEST_PATH))


def make_avatar(sprite: Image.Image, size: int) -> Image.Image:
    bbox = alpha_bbox(sprite)
    left, top, right, bottom = bbox
    height = bottom - top
    width = right - left
    crop = (
        left - int(width * 0.10),
        top - int(height * 0.04),
        right + int(width * 0.10),
        top + int(height * 0.66),
    )
    crop = square_crop(crop, sprite.size)
    trimmed = sprite.crop(crop)
    inner = alpha_bbox(trimmed)
    trimmed = trimmed.crop(inner)
    trimmed.thumbnail((int(size * 0.88), int(size * 0.88)), Image.Resampling.LANCZOS)

    canvas = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    offset = ((size - trimmed.width) // 2, (size - trimmed.height) // 2)
    canvas.alpha_composite(trimmed, offset)
    return canvas


def alpha_bbox(image: Image.Image) -> tuple[int, int, int, int]:
    bbox = image.getchannel("A").getbbox()
    if bbox is None:
        return (0, 0, image.width, image.height)
    return bbox


def square_crop(crop: tuple[int, int, int, int], image_size: tuple[int, int]) -> tuple[int, int, int, int]:
    left, top, right, bottom = crop
    side = max(right - left, bottom - top)
    cx = (left + right) // 2
    cy = (top + bottom) // 2
    left = cx - side // 2
    top = cy - side // 2
    right = left + side
    bottom = top + side
    width, height = image_size

    if left < 0:
        right -= left
        left = 0
    if top < 0:
        bottom -= top
        top = 0
    if right > width:
        left -= right - width
        right = width
    if bottom > height:
        top -= bottom - height
        bottom = height

    return (max(0, left), max(0, top), min(width, right), min(height, bottom))


def write_review_sheet(rows: list[dict[str, str]], previews: list[tuple[CatAvatarSpec, Path]]) -> None:
    sheet = Image.new("RGBA", (2100, 760), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(18)
    body_font = load_font(14)

    draw.text((40, 28), "P0 Batch 58 - source-locked starter-cat HUD avatars installed", fill=(42, 36, 32), font=title_font)
    draw.text(
        (40, 76),
        "Installed into Assets from locked turnaround-derived combat sprites. No AI candidate art was imported.",
        fill=(112, 49, 43),
        font=body_font,
    )

    positions = ((54, 132), (738, 132), (1422, 132))
    for (spec, avatar_path), (x, y) in zip(previews, positions):
        source = Image.open(resolve_repo_path(spec.source_turnaround_path)).convert("RGBA")
        sprite = Image.open(resolve_repo_path(spec.locked_sprite_path)).convert("RGBA")
        avatar = Image.open(avatar_path).convert("RGBA")

        draw.rounded_rectangle((x - 18, y - 18, x + 620, y + 554), radius=9, fill=(255, 252, 246), outline=(178, 158, 130))
        draw.text((x, y), spec.display_name, fill=(42, 36, 32), font=label_font)
        draw_panel(sheet, draw, source, (x, y + 42), (260, 170), "colored three-view source", body_font)
        draw_panel(sheet, draw, sprite, (x + 292, y + 42), (150, 150), "locked sprite", body_font)
        draw_panel(sheet, draw, avatar, (x + 456, y + 42), (128, 128), "installed avatar", body_font)

        yy = y + 248
        draw.text((x, yy), "Must stay visible:", fill=(42, 36, 32), font=body_font)
        yy += 22
        for trait in spec.required_traits:
            for line in textwrap.wrap("- " + trait, width=52, break_long_words=False):
                draw.text((x + 12, yy), line, fill=(42, 36, 32), font=body_font)
                yy += 19
        draw.text((x, y + 500), "Unity visual smoke still pending.", fill=(112, 49, 43), font=body_font)

    sheet.save(REVIEW_SHEET_PATH)


def draw_panel(
    sheet: Image.Image,
    draw: ImageDraw.ImageDraw,
    image: Image.Image,
    origin: tuple[int, int],
    size: tuple[int, int],
    label: str,
    font: ImageFont.ImageFont,
) -> None:
    x, y = origin
    width, height = size
    draw.rounded_rectangle((x, y, x + width, y + height + 24), radius=7, fill=(242, 236, 226), outline=(186, 168, 140))
    background = Image.new("RGBA", (width - 12, height - 28), (250, 246, 239, 255))
    preview = image.copy()
    preview.thumbnail((background.width - 8, background.height - 8), Image.Resampling.LANCZOS)
    background.alpha_composite(preview, ((background.width - preview.width) // 2, (background.height - preview.height) // 2))
    sheet.alpha_composite(background, (x + 6, y + 6))
    draw.text((x + 6, y + height + 3), label, fill=(42, 36, 32), font=font)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 58 Starter Cat HUD Avatar Install Review",
        "",
        "Decision: installed source-locked HUD avatars; Unity visual smoke still pending.",
        "",
        "This batch responds to the current consistency risk by installing only deterministic avatars derived from the already locked starter-cat combat sprites. No AI-generated cat body candidate was imported.",
        "",
        "## Scope",
        "",
        "- Covers Saiban, Nephthys, and Suzune only.",
        "- Installs 256x256 transparent HUD avatar icons under `Assets/TheCat/Art/UI/Icons`.",
        "- Uses the locked colored three-view turnaround and locked combat sprite as lineage evidence.",
        "- Adds Unity `.png.meta` files with `TheCatP0ImportSettings:v1`.",
        "- Does not modify or replace starter-cat combat sprites.",
        "- Does not approve Batch 48, 49, 50, or 51 AI candidates for body-art import.",
        "",
        "## Outputs",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Installed Rows",
        "",
    ]
    for row in rows:
        lines.extend(
            [
                f"### {row['display_name']}",
                "",
                f"- Asset id: `{row['asset_id']}`",
                f"- Unity path: `{row['unity_import_path']}`",
                f"- Source lock: `{row['source_lock_id']}`",
                f"- Source turnaround: `{row['source_turnaround_path']}`",
                f"- Locked sprite: `{row['locked_sprite_path']}`",
                f"- Active screenshot gate remains required before final visual acceptance.",
                "",
            ]
        )
    lines.extend(
        [
            "## Remaining Unity Gate",
            "",
            "- Refresh Unity AssetDatabase.",
            "- Confirm all three avatar icons import as Sprite, Single mode, no mipmaps, alpha transparency on.",
            "- Capture HUD / character-selection screenshots showing the avatar icons at runtime scale.",
            "- Compare screenshots against the Batch 58 review sheet and the colored three-view source.",
            "",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 58 Process Note",
        "",
        "- Production mode: deterministic local image processing with Pillow.",
        "- Image generation model: not used.",
        "- Source inputs: locked starter-cat combat sprites, whose hashes are already tied to the colored three-view turnarounds.",
        "- Installed outputs: three 256x256 transparent avatar PNGs plus Unity `.png.meta` files.",
        "- Runtime catalog and manifest updates are required for these installed avatars.",
        "- Unity MCP / Editor validation remains pending.",
        "",
        "## Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['asset_id']}` -> `{row['unity_import_path']}` sha256 `{row['installed_sha256']}`")
    lines.append("")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


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


def resolve_repo_path(repo_path: str) -> Path:
    path = REPO_ROOT / repo_path.replace("/", "\\")
    if not path.exists():
        raise FileNotFoundError(repo_path)
    return path


def repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path.replace("/", "\\")


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
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
