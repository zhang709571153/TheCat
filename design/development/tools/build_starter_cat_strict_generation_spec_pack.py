from __future__ import annotations

import csv
import hashlib
import json
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_47_starter_cat_strict_generation_spec_pack_2026-06-15"
CANDIDATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats"
BATCH_DIR = CANDIDATE_ROOT / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "starter_cat_batch47_strict_generation_spec_manifest.csv"
REVIEW_SHEET_PATH = BATCH_DIR / "thecat_starter_cat_batch47_strict_generation_spec_review_sheet.png"
REVIEW_NOTE_PATH = BATCH_DIR / "starter_cat_batch47_strict_generation_spec_review.md"
PROCESS_NOTE_PATH = BATCH_DIR / "starter_cat_batch47_strict_generation_spec_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_47_starter_cat_strict_generation_spec_pack.md"
BATCH45_MANIFEST = "design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv"


@dataclass(frozen=True)
class CatGenerationSpec:
    cat_id: str
    display_name: str
    authority: str
    body_rule: str
    composition_rule: str
    must_keep: tuple[str, ...]
    reject: tuple[str, ...]
    prompt_focus: str
    negative_prompt: str


CATS: tuple[CatGenerationSpec, ...] = (
    CatGenerationSpec(
        "saiban",
        "Saiban / Sword Saint",
        "silver-gray tabby sword saint defender",
        "non-human cat body with short legs, compact paws, cat head, cat muzzle, ears, whiskers, and striped tail",
        "single clean combat sprite, three-quarter front-facing stance, readable shield and sword silhouette, no human torso",
        (
            "silver-gray tabby fur and forehead stripes",
            "round shield in left side read",
            "short sword held as a small heroic defender prop",
            "red cape, silver armor, helmet, gold trim, and blue gem accent",
            "striped tail visible behind armor",
        ),
        (
            "human knight body",
            "long human arms or legs",
            "missing shield or sword",
            "generic armored kitten without red cape",
            "palette drift away from silver, red, gold, and blue",
        ),
        "faithful P0 battle sprite for Saiban, a tiny non-human silver tabby sword saint cat defending a bedline",
        "human, humanoid, anime boy, anime girl, human knight, bipedal warrior, long legs, hands, fingers, realistic armor, dog, mascot redesign, missing shield, missing sword, missing red cape",
    ),
    CatGenerationSpec(
        "nephthys",
        "Nephthys / Moon-Sand Agent",
        "gold-brown hooded moon-sand control cat",
        "non-human cat body with compact paws, feline face, visible ears inside hood, whiskers, and small tail silhouette",
        "single clean combat sprite, three-quarter front-facing stance, hood and floating pyramid/obelisk prop must read at game scale",
        (
            "gold-brown tabby fur and mask-like face markings",
            "deep navy hooded cloak with sand-gold trim",
            "floating pyramid or obelisk prop",
            "blue gem accents, ankh or dream-script symbols",
            "mystic controller silhouette without human posture",
        ),
        (
            "human Cleopatra body",
            "ordinary Egyptian costume on a person",
            "missing hood",
            "missing pyramid or obelisk prop",
            "palette drift away from navy, gold, brown, and blue",
        ),
        "faithful P0 battle sprite for Nephthys, a non-human hooded gold-brown tabby moon-sand agent cat controlling dreams with a floating pyramid prop",
        "human, humanoid, Cleopatra, human priestess, long human arms, long human legs, hands, fingers, ordinary costume, dog, mascot redesign, missing hood, missing pyramid, missing blue gems",
    ),
    CatGenerationSpec(
        "suzune",
        "Suzune / Sleep Shrine Healer",
        "calico sleep shrine healer cat",
        "non-human cat body with compact paws, round feline face, triangular ears, whiskers, and calico tail",
        "single clean combat sprite, three-quarter front-facing stance, shrine healer props and bell ornaments must remain readable",
        (
            "calico orange, black, and white markings from the turnaround",
            "warm white and vermilion shrine outfit",
            "moon-blue accents",
            "bell ornaments",
            "wand or branch healer prop",
        ),
        (
            "human shrine maiden body",
            "generic white healer kitten",
            "missing calico markings",
            "missing bell ornaments",
            "missing wand or branch healer prop",
            "palette drift away from vermilion, warm white, moon-blue, and calico fur",
        ),
        "faithful P0 battle sprite for Suzune, a non-human calico sleep shrine healer cat with bells, shrine outfit, and a small healer wand",
        "human, humanoid, shrine maiden girl, long human arms, long human legs, hands, fingers, generic healer mascot, dog, missing calico patches, missing bells, missing wand, palette drift",
    ),
)


FIELD_NAMES = (
    "cat_id",
    "display_name",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "spec_json_path",
    "spec_json_sha256",
    "generation_prompt_path",
    "generation_prompt_sha256",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "source_lock_id",
    "unity_sprite_path",
    "unity_sprite_sha256",
    "latest_cutout_preview_path",
    "latest_cutout_preview_sha256",
    "palette_hex",
    "visible_bbox_source",
    "active_screenshot",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    batch45_rows = {row["cat_id"]: row for row in read_manifest(resolve_repo_path(BATCH45_MANIFEST))}

    rows: list[dict[str, str]] = []
    cards: dict[str, Path] = {}

    for spec in CATS:
        source_row = batch45_rows[spec.cat_id]
        cat_dir = CANDIDATE_ROOT / spec.cat_id / BATCH_SLUG
        cat_dir.mkdir(parents=True, exist_ok=True)

        source_path = resolve_repo_path(source_row["source_turnaround_path"])
        sprite_path = resolve_repo_path(source_row["unity_sprite_path"])
        cutout_path = resolve_repo_path(source_row["latest_cutout_preview_path"])
        palette = extract_palette(source_path)
        bbox = visible_bbox(Image.open(source_path).convert("RGBA"))

        prompt_path = cat_dir / f"thecat_cat_{spec.cat_id}_batch47_generation_prompt_v001.md"
        json_path = cat_dir / f"thecat_cat_{spec.cat_id}_batch47_strict_generation_spec_v001.json"
        card_path = cat_dir / f"thecat_cat_{spec.cat_id}_batch47_strict_generation_spec_card_v001.png"

        write_prompt(spec, source_row, palette, prompt_path)
        write_json_spec(spec, source_row, palette, bbox, json_path)
        write_card(spec, source_row, palette, bbox, source_path, sprite_path, cutout_path, card_path)
        cards[spec.cat_id] = card_path

        rows.append(
            {
                "cat_id": spec.cat_id,
                "display_name": spec.display_name,
                "batch_slug": BATCH_SLUG,
                "asset_type": "strict_generation_spec_card_1100x900",
                "candidate_path": to_repo_path(card_path),
                "candidate_sha256": sha256(card_path),
                "candidate_size": "1100x900",
                "spec_json_path": to_repo_path(json_path),
                "spec_json_sha256": sha256(json_path),
                "generation_prompt_path": to_repo_path(prompt_path),
                "generation_prompt_sha256": sha256(prompt_path),
                "source_turnaround_path": source_row["source_turnaround_path"],
                "source_turnaround_sha256": sha256(source_path),
                "source_lock_id": source_row["source_lock_id"],
                "unity_sprite_path": source_row["unity_sprite_path"],
                "unity_sprite_sha256": sha256(sprite_path),
                "latest_cutout_preview_path": source_row["latest_cutout_preview_path"],
                "latest_cutout_preview_sha256": sha256(cutout_path),
                "palette_hex": ";".join(palette),
                "visible_bbox_source": ",".join(str(value) for value in bbox),
                "active_screenshot": source_row["active_screenshot"],
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "strict_generation_spec_only_do_not_import",
            }
        )

    write_review_sheet(rows, cards)
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print(f"Wrote {len(rows)} starter-cat strict generation spec row(s).")
    print(to_repo_path(MANIFEST_PATH))


def write_prompt(spec: CatGenerationSpec, source_row: dict[str, str], palette: list[str], path: Path) -> None:
    lines = [
        f"# Batch 47 Strict Generation Prompt - {spec.display_name}",
        "",
        "Use this prompt only with the locked colored three-view turnaround visible as the primary reference.",
        "Do not import into Unity from this prompt output. The result must return through review, cutout, manifest, and active-cat screenshot gates first.",
        "",
        "## Positive Prompt",
        "",
        (
            f"{spec.prompt_focus}; {spec.body_rule}; {spec.composition_rule}; "
            "hand-painted dream defense roguelite game sprite; transparent background; "
            "soft ink outline; compact readable silhouette; preserve exact costume, props, palette, and role identity from the source turnaround."
        ),
        "",
        "## Negative Prompt",
        "",
        spec.negative_prompt,
        "",
        "## Hard Source Lock",
        "",
        f"- Source lock id: `{source_row['source_lock_id']}`",
        f"- Source turnaround: `{source_row['source_turnaround_path']}`",
        f"- Active screenshot gate: `{source_row['active_screenshot']}`",
        f"- Palette guard: `{', '.join(palette)}`",
        "",
        "## Must Keep",
        "",
    ]
    lines.extend(f"- {item}" for item in spec.must_keep)
    lines.extend(["", "## Reject", ""])
    lines.extend(f"- {item}" for item in spec.reject)
    lines.append("")
    path.write_text("\n".join(lines), encoding="utf-8")


def write_json_spec(
    spec: CatGenerationSpec,
    source_row: dict[str, str],
    palette: list[str],
    bbox: tuple[int, int, int, int],
    path: Path,
) -> None:
    data = {
        "batch_slug": BATCH_SLUG,
        "cat_id": spec.cat_id,
        "display_name": spec.display_name,
        "source_lock_id": source_row["source_lock_id"],
        "source_turnaround_path": source_row["source_turnaround_path"],
        "unity_sprite_path": source_row["unity_sprite_path"],
        "latest_cutout_preview_path": source_row["latest_cutout_preview_path"],
        "active_screenshot": source_row["active_screenshot"],
        "palette_hex": palette,
        "visible_bbox_source": bbox,
        "authority": spec.authority,
        "body_rule": spec.body_rule,
        "composition_rule": spec.composition_rule,
        "must_keep": list(spec.must_keep),
        "reject": list(spec.reject),
        "positive_prompt": (
            f"{spec.prompt_focus}; {spec.body_rule}; {spec.composition_rule}; "
            "hand-painted dream defense roguelite game sprite; transparent background; "
            "soft ink outline; compact readable silhouette; preserve exact costume, props, palette, and role identity from the source turnaround."
        ),
        "negative_prompt": spec.negative_prompt,
        "recommendation": "strict_generation_spec_only_do_not_import",
        "unity_validation_required": [
            "active-cat screenshot comparison",
            "Console clean",
            "AssetDatabase refresh",
            "Sprite import settings",
            "prefab/scene binding",
            "HUD readability",
        ],
    }
    path.write_text(json.dumps(data, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")


def write_card(
    spec: CatGenerationSpec,
    source_row: dict[str, str],
    palette: list[str],
    bbox: tuple[int, int, int, int],
    source_path: Path,
    sprite_path: Path,
    cutout_path: Path,
    output_path: Path,
) -> None:
    canvas = Image.new("RGBA", (1100, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(27)
    label_font = load_font(16)
    body_font = load_font(13)
    small_font = load_font(11)

    draw.text((30, 24), f"Batch 47 strict generation spec - {spec.display_name}", fill=(42, 36, 32), font=title_font)
    draw.text((30, 62), "Spec only. Generate nothing that drifts from the colored three-view turnaround. Do not import into Unity.", fill=(116, 47, 43), font=body_font)

    draw_panel(canvas, draw, Image.open(source_path).convert("RGBA"), (30, 104), (455, 245), "locked colored three-view source", label_font)
    draw_panel(canvas, draw, Image.open(sprite_path).convert("RGBA"), (520, 104), (215, 215), "current Unity sprite", label_font)
    draw_panel(canvas, draw, Image.open(cutout_path).convert("RGBA"), (780, 104), (215, 215), "latest cutout candidate", label_font)

    draw.text((30, 385), "Palette guard", fill=(42, 36, 32), font=label_font)
    swatch_x = 30
    for color in palette:
        rgb = tuple(int(color[index : index + 2], 16) for index in (1, 3, 5))
        draw.rounded_rectangle((swatch_x, 414, swatch_x + 76, 470), radius=6, fill=rgb, outline=(70, 60, 52))
        draw.text((swatch_x, 478), color.upper(), fill=(42, 36, 32), font=small_font)
        swatch_x += 88

    y = 530
    draw.text((30, y), "Must keep", fill=(42, 36, 32), font=label_font)
    y += 24
    for item in spec.must_keep:
        for line in wrap("- " + item, 56):
            draw.text((44, y), line, fill=(42, 36, 32), font=body_font)
            y += 18

    y = 530
    draw.text((555, y), "Reject immediately", fill=(42, 36, 32), font=label_font)
    y += 24
    for item in spec.reject:
        for line in wrap("- " + item, 58):
            draw.text((569, y), line, fill=(42, 36, 32), font=body_font)
            y += 18

    draw.text((30, 812), f"Source bbox: {bbox} | Active screenshot: {source_row['active_screenshot']}", fill=(78, 68, 60), font=body_font)
    draw.text((30, 834), f"Prompt file and JSON spec must travel with any future generated candidate for {spec.cat_id}.", fill=(116, 47, 43), font=body_font)
    draw.text((30, 862), short_path(source_row["source_turnaround_path"]), fill=(78, 68, 60), font=small_font)
    canvas.save(output_path)


def draw_panel(
    canvas: Image.Image,
    draw: ImageDraw.ImageDraw,
    image: Image.Image,
    position: tuple[int, int],
    size: tuple[int, int],
    label: str,
    font: ImageFont.ImageFont,
) -> None:
    x, y = position
    width, height = size
    draw.rounded_rectangle((x, y, x + width, y + height), radius=8, fill=(240, 233, 222), outline=(181, 166, 141))
    draw_checker(draw, (x + 8, y + 8, x + width - 8, y + height - 32))
    fitted = fit_image(image, (width - 18, height - 48))
    canvas.alpha_composite(fitted, (x + (width - fitted.width) // 2, y + 10 + (height - 48 - fitted.height) // 2))
    draw.text((x + 10, y + height - 26), label, fill=(42, 36, 32), font=font)


def write_review_sheet(rows: list[dict[str, str]], cards: dict[str, Path]) -> None:
    sheet = Image.new("RGBA", (3600, 1120), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(38)
    body_font = load_font(17)

    draw.text((48, 34), "P0 Batch 47 - starter cat strict generation spec pack", fill=(42, 36, 32), font=title_font)
    draw.text(
        (48, 84),
        "Generation spec only. All future cat images must obey the colored three-view turnaround, palette guard, body rule, and active screenshot gate.",
        fill=(116, 47, 43),
        font=body_font,
    )

    positions = ((50, 145), (1248, 145), (2446, 145))
    for row, position in zip(rows, positions):
        card = Image.open(cards[row["cat_id"]]).convert("RGBA")
        sheet.alpha_composite(card, position)

    draw.text(
        (48, 1082),
        "Do not import prompt output directly. Any generated image must return through candidate review, cutout, manifest, and Unity active-cat screenshot validation.",
        fill=(78, 68, 60),
        font=body_font,
    )
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 47 Starter Cat Strict Generation Spec Review",
        "",
        "Decision: strict generation spec only; do not import into Unity.",
        "",
        "This pack exists because starter-cat images must match the locked colored three-view turnaround, not just the broader dream-cat art style.",
        "",
        "## Scope",
        "",
        "- Covers Saiban, Nephthys, and Suzune only.",
        "- Produces machine-readable JSON generation specs, prompt files, and visual spec cards.",
        "- Samples a source palette guard from each locked colored turnaround.",
        "- Keeps all Batch 47 outputs outside `Assets`.",
        "- Creates no Unity `.meta` files.",
        "- Does not generate replacement art and does not approve any sprite import.",
        "- Formal import remains blocked until active-cat Play Mode screenshot comparison passes.",
        "",
        "## Outputs",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Rows",
        "",
    ]
    for row in rows:
        lines.extend(
            [
                f"### {row['display_name']}",
                "",
                f"- Source lock: `{row['source_lock_id']}`",
                f"- Palette guard: `{row['palette_hex']}`",
                f"- Prompt file: `{row['generation_prompt_path']}`",
                f"- JSON spec: `{row['spec_json_path']}`",
                f"- Active screenshot: `{row['active_screenshot']}`",
                f"- Recommendation: `{row['recommendation']}`",
                "",
            ]
        )
    lines.extend(
        [
            "## Blocking Items",
            "",
            "- Any future image generation must use these specs plus the source turnaround as the primary reference.",
            "- Any generated output must return through cutout, manifest, review sheet, and active-cat screenshot validation before Unity import.",
            "- Reject human body proportions, generic mascot drift, missing required props, and palette drift from the source lock.",
            "",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 47 Process Note",
        "",
        "Process: deterministic local source-lock analysis and prompt/spec generation.",
        "",
        "- No image model call was made in this batch.",
        "- No replacement sprite was generated.",
        "- No candidate file was copied into `Assets`.",
        "- No Unity `.meta` file was created.",
        "- The pack extracts source palette guards from the locked colored turnarounds.",
        "- The pack creates prompt files and JSON specs for future generation agents.",
        "- Unity-side validation remains required before any future cat sprite install.",
        "",
        "Rows: " + str(len(rows)),
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['cat_id']}` -> `{row['generation_prompt_path']}`")
    lines.append("")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def read_manifest(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8-sig", newline="") as handle:
        return list(csv.DictReader(handle))


def extract_palette(path: Path, count: int = 7) -> list[str]:
    image = Image.open(path).convert("RGBA")
    samples: list[tuple[int, int, int]] = []
    for pixel in iter_pixels(image.resize((180, 180), Image.Resampling.LANCZOS)):
        r, g, b, a = pixel
        if a < 48:
            continue
        if r > 238 and g > 238 and b > 238:
            continue
        if abs(r - g) < 4 and abs(g - b) < 4 and r > 220:
            continue
        samples.append((r, g, b))

    if not samples:
        samples = [(r, g, b) for r, g, b, _ in iter_pixels(image.resize((128, 128), Image.Resampling.LANCZOS))]

    palette_image = Image.new("RGB", (len(samples), 1))
    palette_image.putdata(samples)
    quantized = palette_image.quantize(colors=count, method=Image.Quantize.MEDIANCUT)
    palette = quantized.getpalette() or []
    colors = quantized.getcolors()
    if colors is None:
        return []

    sorted_indexes = [index for _, index in sorted(colors, reverse=True)]
    output: list[str] = []
    for index in sorted_indexes:
        base = index * 3
        if base + 2 >= len(palette):
            continue
        rgb = tuple(palette[base + offset] for offset in range(3))
        hex_color = "#{:02x}{:02x}{:02x}".format(*rgb)
        if hex_color not in output:
            output.append(hex_color)
        if len(output) >= count:
            break
    return output


def visible_bbox(image: Image.Image) -> tuple[int, int, int, int]:
    pixels = image.load()
    xs: list[int] = []
    ys: list[int] = []
    for y in range(image.height):
        for x in range(image.width):
            r, g, b, a = pixels[x, y]
            if a < 48:
                continue
            if r > 238 and g > 238 and b > 238:
                continue
            xs.append(x)
            ys.append(y)
    if not xs or not ys:
        return (0, 0, image.width, image.height)
    return (min(xs), min(ys), max(xs) + 1, max(ys) + 1)


def iter_pixels(image: Image.Image):
    flattened = getattr(image, "get_flattened_data", None)
    if flattened is not None:
        return flattened()
    return image.getdata()


def draw_checker(draw: ImageDraw.ImageDraw, bounds: tuple[int, int, int, int]) -> None:
    x1, y1, x2, y2 = bounds
    cell = 14
    for yy in range(y1, y2, cell):
        for xx in range(x1, x2, cell):
            tone = (228, 222, 212) if ((xx // cell) + (yy // cell)) % 2 == 0 else (249, 246, 240)
            draw.rectangle((xx, yy, min(xx + cell, x2), min(yy + cell, y2)), fill=tone)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    output = image.copy()
    output.thumbnail(size, Image.Resampling.LANCZOS)
    return output


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


def short_path(path: str, max_length: int = 130) -> str:
    if len(path) <= max_length:
        return path
    return "..." + path[-(max_length - 3) :]


if __name__ == "__main__":
    main()
