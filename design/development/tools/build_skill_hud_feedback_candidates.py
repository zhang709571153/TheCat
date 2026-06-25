from __future__ import annotations

import csv
import hashlib
import math
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_57_skill_hud_feedback_candidates_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "skill_hud" / BATCH_SLUG
MANIFEST_PATH = CANDIDATE_DIR / "skill_hud_feedback_batch57_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_skill_hud_feedback_batch57_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "skill_hud_feedback_batch57_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "skill_hud_feedback_batch57_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_57_skill_hud_feedback_candidates.md"

FIELD_NAMES = (
    "subject_id",
    "display_name",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "reference_asset_paths",
    "reference_asset_sha256s",
    "runtime_surface_hint",
    "generated_alpha_path",
    "generated_alpha_sha256",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
    "visual_review",
)

SUBJECTS = (
    {
        "subject_id": "skill_ready_frame",
        "display_name": "Skill Ready Frame",
        "file_slug": "skill_ready_frame",
        "runtime_surface_hint": "skill_hud.ready_button",
        "references": (
            "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
            "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
        ),
        "review": "Usable candidate: dreamglass button frame with cyan ready edge and small star ticks. No cat body, fur, costume, or turnaround crop.",
    },
    {
        "subject_id": "skill_cooldown_overlay",
        "display_name": "Skill Cooldown Overlay",
        "file_slug": "skill_cooldown_overlay",
        "runtime_surface_hint": "skill_hud.cooldown_overlay",
        "references": (
            "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_dominion_sandglass_seal_128_v001.png",
            "Assets/TheCat/Art/UI/Icons/thecat_ui_status_slow_64_v001.png",
        ),
        "review": "Usable candidate: moon-sand timer wedge and dimmed crescent communicate cooldown without hiding the skill slot.",
    },
    {
        "subject_id": "skill_no_target_marker",
        "display_name": "No Target Marker",
        "file_slug": "skill_no_target_marker",
        "runtime_surface_hint": "skill_hud.no_target",
        "references": (
            "Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_64_v001.png",
            "Assets/TheCat/Art/VFX/thecat_vfx_enemy_mark_ring_256_v001.png",
        ),
        "review": "Strong candidate: broken crosshair, red warning edge, and cyan target ring read as no-target feedback at HUD scale.",
    },
    {
        "subject_id": "skill_hunger_cost_chip",
        "display_name": "Hunger Cost Chip",
        "file_slug": "skill_hunger_cost_chip",
        "runtime_surface_hint": "skill_hud.hunger_cost",
        "references": (
            "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png",
            "Assets/TheCat/Art/UI/Icons/thecat_ui_reward_fishtreat_icon_128_v001.png",
        ),
        "review": "Usable candidate: warm kibble coin and tiny cost beads communicate hunger spend without using cat imagery.",
    },
    {
        "subject_id": "auto_target_reticle",
        "display_name": "Auto Target Reticle",
        "file_slug": "auto_target_reticle",
        "runtime_surface_hint": "battle_hud.auto_target",
        "references": (
            "Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_64_v001.png",
            "Assets/TheCat/Art/VFX/thecat_vfx_enemy_mark_ring_256_v001.png",
        ),
        "review": "Strong candidate: soft lock-on reticle and small sleep-star ticks support auto-target readability without character art.",
    },
    {
        "subject_id": "interaction_range_ripple",
        "display_name": "Interaction Range Ripple",
        "file_slug": "interaction_range_ripple",
        "runtime_surface_hint": "battle_world.interaction_range",
        "references": (
            "Assets/TheCat/Art/VFX/thecat_vfx_litter_cleanse_256_v001.png",
            "Assets/TheCat/Art/VFX/thecat_vfx_feeder_kibble_256_v001.png",
            "Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png",
        ),
        "review": "Usable candidate: soft ripple and small bed/litter/feed symbols support interaction range feedback. Must be scale-tested in Unity before install.",
    },
)

ASSET_TYPES = (
    "alpha_candidate_512",
    "checkerboard_review_512",
    "darkfield_review_512",
    "warmfield_review_512",
    "alpha_mask_review_512",
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    for subject in SUBJECTS:
        alpha = render_subject(subject)
        alpha_path = CANDIDATE_DIR / f"thecat_ui_{subject['file_slug']}_batch57_alpha_512_candidate_v001.png"
        checker_path = CANDIDATE_DIR / f"thecat_ui_{subject['file_slug']}_batch57_checkerboard_512_review_v001.png"
        dark_path = CANDIDATE_DIR / f"thecat_ui_{subject['file_slug']}_batch57_darkfield_512_review_v001.png"
        warm_path = CANDIDATE_DIR / f"thecat_ui_{subject['file_slug']}_batch57_warmfield_512_review_v001.png"
        mask_path = CANDIDATE_DIR / f"thecat_ui_{subject['file_slug']}_batch57_alpha_mask_512_review_v001.png"
        alpha.save(alpha_path)
        checkerboard_composite(alpha).save(checker_path)
        flat_composite(alpha, (18, 20, 30, 255)).save(dark_path)
        flat_composite(alpha, (247, 239, 224, 255)).save(warm_path)
        alpha_mask(alpha).save(mask_path)

    rows = build_manifest_rows()
    write_review_sheet()
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print("Wrote P0 Batch 57 skill HUD feedback candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def render_subject(subject: dict[str, object]) -> Image.Image:
    canvas = Image.new("RGBA", (512, 512), (0, 0, 0, 0))
    draw = ImageDraw.Draw(canvas)
    subject_id = str(subject["subject_id"])
    if subject_id == "skill_ready_frame":
        draw_ready_frame(draw)
    elif subject_id == "skill_cooldown_overlay":
        draw_cooldown_overlay(draw)
    elif subject_id == "skill_no_target_marker":
        draw_no_target_marker(draw)
    elif subject_id == "skill_hunger_cost_chip":
        draw_hunger_cost_chip(draw)
    elif subject_id == "auto_target_reticle":
        draw_auto_target_reticle(draw)
    elif subject_id == "interaction_range_ripple":
        draw_interaction_range_ripple(draw)
    return canvas


def draw_ready_frame(draw: ImageDraw.ImageDraw) -> None:
    rounded_rect(draw, (84, 132, 428, 380), 44, (24, 32, 55, 176), (137, 231, 246, 238), 8)
    rounded_rect(draw, (110, 158, 402, 354), 34, (55, 74, 110, 112), (249, 226, 148, 210), 3)
    for x, y in ((156, 188), (356, 188), (156, 324), (356, 324)):
        draw_star(draw, x, y, 17, (249, 226, 148, 230))
    draw.arc((150, 172, 362, 384), 205, 335, fill=(138, 239, 250, 220), width=9)
    draw.arc((174, 196, 338, 360), 205, 335, fill=(255, 255, 255, 150), width=4)


def draw_cooldown_overlay(draw: ImageDraw.ImageDraw) -> None:
    draw.ellipse((92, 92, 420, 420), fill=(24, 28, 45, 148), outline=(116, 215, 230, 220), width=8)
    draw.pieslice((116, 116, 396, 396), start=270, end=65, fill=(55, 84, 128, 190))
    draw.arc((116, 116, 396, 396), 270, 65, fill=(249, 226, 148, 230), width=13)
    rounded_rect(draw, (218, 108, 294, 404), 25, (41, 53, 82, 235), (226, 195, 129, 235), 5)
    draw.polygon([(236, 150), (276, 150), (256, 220)], fill=(226, 195, 129, 220))
    draw.polygon([(236, 362), (276, 362), (256, 292)], fill=(226, 195, 129, 220))
    draw.line((256, 220, 256, 292), fill=(226, 195, 129, 180), width=5)


def draw_no_target_marker(draw: ImageDraw.ImageDraw) -> None:
    draw.ellipse((106, 106, 406, 406), fill=(18, 20, 30, 96), outline=(132, 229, 241, 230), width=8)
    draw.ellipse((166, 166, 346, 346), outline=(132, 229, 241, 170), width=5)
    draw.line((256, 98, 256, 180), fill=(132, 229, 241, 210), width=6)
    draw.line((256, 332, 256, 414), fill=(132, 229, 241, 210), width=6)
    draw.line((98, 256, 180, 256), fill=(132, 229, 241, 210), width=6)
    draw.line((332, 256, 414, 256), fill=(132, 229, 241, 210), width=6)
    draw.line((156, 156, 356, 356), fill=(226, 72, 80, 240), width=19)
    draw.line((356, 156, 156, 356), fill=(226, 72, 80, 240), width=19)
    draw.line((156, 156, 356, 356), fill=(255, 210, 190, 210), width=6)
    draw.line((356, 156, 156, 356), fill=(255, 210, 190, 210), width=6)


def draw_hunger_cost_chip(draw: ImageDraw.ImageDraw) -> None:
    draw.ellipse((108, 108, 404, 404), fill=(82, 55, 44, 225), outline=(250, 189, 113, 240), width=10)
    draw.ellipse((146, 146, 366, 366), fill=(141, 89, 57, 185), outline=(255, 232, 159, 220), width=4)
    for angle in range(0, 360, 45):
        x = 256 + math.cos(math.radians(angle)) * 118
        y = 256 + math.sin(math.radians(angle)) * 118
        draw.ellipse((x - 10, y - 10, x + 10, y + 10), fill=(255, 226, 150, 210))
    draw.pieslice((188, 178, 324, 322), 20, 340, fill=(255, 199, 93, 235), outline=(110, 67, 45, 220), width=4)
    draw.ellipse((282, 220, 306, 244), fill=(119, 75, 49, 230))
    draw.arc((192, 232, 320, 346), 200, 340, fill=(255, 238, 179, 190), width=6)


def draw_auto_target_reticle(draw: ImageDraw.ImageDraw) -> None:
    for inset, alpha in ((70, 90), (110, 130), (152, 170)):
        draw.ellipse((inset, inset, 512 - inset, 512 - inset), outline=(130, 226, 238, alpha), width=5)
    for angle in (20, 160, 200, 340):
        draw_arc_segment(draw, angle, angle + 50, 74, (130, 226, 238, 235), 11)
    draw.line((256, 116, 256, 188), fill=(249, 226, 148, 220), width=6)
    draw.line((256, 324, 256, 396), fill=(249, 226, 148, 220), width=6)
    draw.line((116, 256, 188, 256), fill=(249, 226, 148, 220), width=6)
    draw.line((324, 256, 396, 256), fill=(249, 226, 148, 220), width=6)
    draw_star(draw, 256, 256, 28, (249, 226, 148, 230))


def draw_interaction_range_ripple(draw: ImageDraw.ImageDraw) -> None:
    for radius, alpha in ((166, 72), (132, 108), (96, 150)):
        draw.ellipse((256 - radius, 256 - radius, 256 + radius, 256 + radius), outline=(146, 231, 239, alpha), width=8)
    rounded_rect(draw, (188, 184, 324, 286), 28, (55, 77, 108, 190), (247, 229, 158, 220), 5)
    draw.arc((192, 198, 320, 346), 200, 340, fill=(247, 229, 158, 220), width=7)
    draw.ellipse((224, 302, 248, 326), fill=(146, 231, 239, 210))
    draw.ellipse((264, 302, 288, 326), fill=(146, 231, 239, 210))
    draw.ellipse((244, 274, 268, 298), fill=(146, 231, 239, 210))
    draw_star(draw, 180, 180, 16, (249, 226, 148, 210))
    draw_star(draw, 334, 334, 16, (249, 226, 148, 210))


def rounded_rect(
    draw: ImageDraw.ImageDraw,
    box: tuple[int, int, int, int],
    radius: int,
    fill: tuple[int, int, int, int],
    outline: tuple[int, int, int, int],
    width: int,
) -> None:
    draw.rounded_rectangle(box, radius=radius, fill=fill, outline=outline, width=width)


def draw_star(draw: ImageDraw.ImageDraw, x: float, y: float, radius: float, fill: tuple[int, int, int, int]) -> None:
    points = []
    for i in range(8):
        r = radius if i % 2 == 0 else radius * 0.42
        angle = math.radians(-90 + i * 45)
        points.append((x + math.cos(angle) * r, y + math.sin(angle) * r))
    draw.polygon(points, fill=fill)


def draw_arc_segment(
    draw: ImageDraw.ImageDraw,
    start: int,
    end: int,
    inset: int,
    fill: tuple[int, int, int, int],
    width: int,
) -> None:
    draw.arc((inset, inset, 512 - inset, 512 - inset), start=start, end=end, fill=fill, width=width)


def checkerboard_composite(image: Image.Image) -> Image.Image:
    board = Image.new("RGBA", image.size, (255, 255, 255, 255))
    draw = ImageDraw.Draw(board)
    tile = 32
    for y in range(0, image.height, tile):
        for x in range(0, image.width, tile):
            if ((x // tile) + (y // tile)) % 2 == 0:
                draw.rectangle((x, y, x + tile - 1, y + tile - 1), fill=(210, 214, 222, 255))
    board.alpha_composite(image)
    return board


def flat_composite(image: Image.Image, color: tuple[int, int, int, int]) -> Image.Image:
    background = Image.new("RGBA", image.size, color)
    background.alpha_composite(image)
    return background


def alpha_mask(image: Image.Image) -> Image.Image:
    _, _, _, alpha = image.split()
    return Image.merge("RGBA", (alpha, alpha, alpha, Image.new("L", image.size, 255)))


def build_manifest_rows() -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    for subject in SUBJECTS:
        file_slug = str(subject["file_slug"])
        alpha_path = CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_alpha_512_candidate_v001.png"
        references = tuple(str(path) for path in subject["references"])
        reference_paths = [resolve_repo_path(path) for path in references]
        reference_hashes = [sha256(path) for path in reference_paths]
        assets = (
            ("alpha_candidate_512", alpha_path),
            ("checkerboard_review_512", CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_checkerboard_512_review_v001.png"),
            ("darkfield_review_512", CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_darkfield_512_review_v001.png"),
            ("warmfield_review_512", CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_warmfield_512_review_v001.png"),
            ("alpha_mask_review_512", CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_alpha_mask_512_review_v001.png"),
        )

        for asset_type, path in assets:
            rows.append(
                {
                    "subject_id": str(subject["subject_id"]),
                    "display_name": str(subject["display_name"]),
                    "batch_slug": BATCH_SLUG,
                    "asset_type": asset_type,
                    "candidate_path": to_repo_path(path),
                    "candidate_sha256": sha256(path),
                    "candidate_size": image_size(path),
                    "reference_asset_paths": ";".join(references),
                    "reference_asset_sha256s": ";".join(reference_hashes),
                    "runtime_surface_hint": str(subject["runtime_surface_hint"]),
                    "generated_alpha_path": to_repo_path(alpha_path),
                    "generated_alpha_sha256": sha256(alpha_path),
                    "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                    "review_note": to_repo_path(REVIEW_NOTE_PATH),
                    "process_note": to_repo_path(PROCESS_NOTE_PATH),
                    "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                    "recommendation": "candidate_review_only_do_not_import",
                    "visual_review": str(subject["review"]),
                }
            )
    return rows


def write_review_sheet() -> None:
    sheet = Image.new("RGBA", (2400, 1600), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(36)
    label_font = load_font(20)
    body_font = load_font(15)
    small_font = load_font(13)

    draw.text((36, 28), "P0 Batch 57 - Skill HUD feedback candidates", fill=(42, 36, 32), font=title_font)
    draw.text(
        (36, 78),
        "Candidate review only. Non-cat UI/VFX symbols for skill readiness, cooldown, targeting, hunger cost, auto target, and interaction range.",
        fill=(116, 47, 43),
        font=body_font,
    )
    draw.text(
        (36, 106),
        "No Unity install, no .meta files, no starter-cat body redraws, no colored-turnaround crops.",
        fill=(78, 68, 60),
        font=body_font,
    )

    for index, subject in enumerate(SUBJECTS):
        col = index % 3
        row = index // 3
        x = 36 + col * 780
        y = 160 + row * 690
        draw.text((x, y), str(subject["display_name"]), fill=(42, 36, 32), font=label_font)
        draw.text((x, y + 28), str(subject["runtime_surface_hint"]), fill=(78, 68, 60), font=small_font)
        file_slug = str(subject["file_slug"])
        alpha = Image.open(CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_alpha_512_candidate_v001.png").convert("RGBA")
        checker = Image.open(CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_checkerboard_512_review_v001.png").convert("RGBA")
        dark = Image.open(CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_darkfield_512_review_v001.png").convert("RGBA")
        warm = Image.open(CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_warmfield_512_review_v001.png").convert("RGBA")
        mask = Image.open(CANDIDATE_DIR / f"thecat_ui_{file_slug}_batch57_alpha_mask_512_review_v001.png").convert("RGBA")
        draw_panel(sheet, draw, alpha, (x, y + 62), (220, 220), "alpha", small_font)
        draw_panel(sheet, draw, checker, (x + 238, y + 62), (220, 220), "checker", small_font)
        draw_panel(sheet, draw, dark, (x + 476, y + 62), (220, 220), "dark", small_font)
        draw_panel(sheet, draw, warm, (x, y + 328), (220, 220), "warm", small_font)
        draw_panel(sheet, draw, mask, (x + 238, y + 328), (220, 220), "mask", small_font)
        draw.text((x + 476, y + 328), wrap(str(subject["review"]), 34), fill=(55, 48, 44), font=small_font)

    draw.text((36, 1540), "Batch 57 stays outside Assets until a formal Unity install decision approves a specific runtime surface.", fill=(116, 47, 43), font=body_font)
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
    w, h = size
    draw.rounded_rectangle((x, y, x + w, y + h), radius=14, fill=(232, 226, 216, 255), outline=(168, 154, 132, 255), width=2)
    fitted = image.resize(size, Image.Resampling.LANCZOS)
    sheet.alpha_composite(fitted, origin)
    draw.text((x, y + h + 6), label, fill=(78, 68, 60), font=font)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Skill HUD Feedback Batch 57 Candidate Review",
        "",
        "## Verdict",
        "",
        "Candidate review only; do not import into Unity yet.",
        "",
        "Batch 57 is a non-cat UI/VFX candidate pack for P0 skill HUD and battle",
        "operation feedback. It does not modify runtime visual bindings, manifest",
        "catalog counts, prefabs, scenes, or Unity import settings.",
        "",
        "## Candidate Subjects",
        "",
    ]
    for subject in SUBJECTS:
        lines.append("- `" + str(subject["subject_id"]) + "`: " + str(subject["review"]))
    lines.extend(
        [
            "",
            "## Unity Install Blockers",
            "",
            "- HUD-scale readability must be checked in Play Mode.",
            "- Skill cooldown, no-target, hunger-cost, auto-target, and interaction",
            "  range timing must be checked against live gameplay.",
            "- Unity Console must have no new errors.",
            "- Sprite import settings, scene/prefab binding, and screenshot evidence",
            "  must pass before formal install.",
            "- No Batch 57 file may be copied into `Assets` until a formal install",
            "  decision row is approved.",
            "",
            "## Consistency Notes",
            "",
            "- Non-cat UI/VFX only.",
            "- No cat bodies, starter-cat portraits, fur patterns, costume fragments,",
            "  symbolic props copied from colored turnarounds, or turnaround crops.",
            "- The visual language reuses existing dreamglass, status, hunger, mark,",
            "  and battle-feedback symbols.",
            "",
            "## Manifest",
            "",
            "- Rows: " + str(len(rows)),
            "- Recommendation: `candidate_review_only_do_not_import`",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Skill HUD Feedback Batch 57 Process Note",
        "",
        "## Process",
        "",
        "- Generated six deterministic transparent PNG candidates with PIL.",
        "- Built checkerboard, dark-field, warm-field, and alpha-mask review views.",
        "- Wrote a CSV manifest with hashes and reference asset hashes.",
        "- Built one review sheet for side-by-side candidate inspection.",
        "",
        "## Output",
        "",
        "- Candidate directory: `" + to_repo_path(CANDIDATE_DIR) + "`",
        "- Manifest: `" + to_repo_path(MANIFEST_PATH) + "`",
        "- Review sheet: `" + to_repo_path(REVIEW_SHEET_PATH) + "`",
        "- Review note: `" + to_repo_path(REVIEW_NOTE_PATH) + "`",
        "- Rows: " + str(len(rows)),
        "",
        "## Install Status",
        "",
        "No Unity asset install was performed. Batch 57 remains review-only until",
        "runtime HUD readability, Console, Sprite import, scene/prefab binding,",
        "and Play Mode screenshot checks approve a formal install decision.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def resolve_repo_path(relative_path: str) -> Path:
    path = REPO_ROOT / relative_path
    if not path.exists():
        raise FileNotFoundError(path)
    return path


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def image_size(path: Path) -> str:
    with Image.open(path) as image:
        return f"{image.width}x{image.height}"


def load_font(size: int) -> ImageFont.ImageFont:
    for font_name in ("arial.ttf", "DejaVuSans.ttf"):
        try:
            return ImageFont.truetype(font_name, size)
        except OSError:
            pass
    return ImageFont.load_default()


def wrap(text: str, width: int) -> str:
    words = text.split()
    lines: list[str] = []
    current: list[str] = []
    for word in words:
        candidate = " ".join(current + [word])
        if len(candidate) > width and current:
            lines.append(" ".join(current))
            current = [word]
        else:
            current.append(word)
    if current:
        lines.append(" ".join(current))
    return "\n".join(lines)


if __name__ == "__main__":
    main()
