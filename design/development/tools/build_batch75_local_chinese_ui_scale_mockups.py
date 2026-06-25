from __future__ import annotations

import csv
import hashlib
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_75_chinese_ui_scale_evidence_templates_2026-06-20"
BATCH_DIR = (
    PROJECT_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "ui"
    / "chinese_ui_scale_evidence"
    / BATCH_SLUG
)
OUT_DIR = BATCH_DIR / "local_scale_mockups_v001"
MOCKUP_DIR = OUT_DIR / "mockups"

SURFACES = (
    ("main_menu_character_select", "角色选择", "选择守梦猫，确认出战技能与初始祝福。"),
    ("route_map", "梦层路线", "查看十层路线、锁定下一场遭遇或休整。"),
    ("battle_hud", "卧室守卫战", "战斗中查看生命、困意、技能冷却与敌方警告。"),
    ("skill_enemy_hud", "技能与敌人", "对比技能说明、敌人状态、伤害数字与冷却。"),
    ("result_pause_settings", "结算 / 暂停 / 设置", "检查长文本、按钮、开关和奖励数字。"),
)

RESOLUTIONS = (
    ("compact_4_3", 1024, 768),
    ("baseline_16_9", 1280, 720),
    ("desktop_16_9", 1600, 900),
    ("wide_1080p", 1920, 1080),
)

CHECKS = (
    "中文标题不裁切",
    "长说明可换行",
    "按钮文字可读",
    "数字/冷却清晰",
    "窄屏不重叠",
    "控制台无错误",
)

COLORS = {
    "bg": (10, 16, 28, 255),
    "band": (25, 38, 61, 255),
    "panel": (18, 30, 48, 235),
    "panel2": (30, 43, 64, 235),
    "line": (68, 111, 142, 255),
    "cyan": (80, 205, 245, 255),
    "gold": (244, 199, 92, 255),
    "green": (105, 232, 160, 255),
    "red": (238, 92, 90, 255),
    "text": (226, 238, 255, 255),
    "muted": (166, 190, 216, 255),
    "dark": (9, 13, 22, 255),
}


@dataclass(frozen=True)
class MockupRow:
    surface_id: str
    surface_name: str
    resolution_id: str
    resolution: str
    path: str
    sha256: str
    status: str
    notes: str


def main() -> None:
    MOCKUP_DIR.mkdir(parents=True, exist_ok=True)
    rows: list[MockupRow] = []

    for surface_id, surface_name, description in SURFACES:
        for resolution_id, width, height in RESOLUTIONS:
            image = render_surface(surface_id, surface_name, description, width, height)
            filename = f"thecat_batch75_{surface_id}_{width}x{height}_local_scale_mockup_v001.png"
            path = MOCKUP_DIR / filename
            image.save(path)
            rows.append(
                MockupRow(
                    surface_id=surface_id,
                    surface_name=surface_name,
                    resolution_id=resolution_id,
                    resolution=f"{width}x{height}",
                    path=to_project_path(path),
                    sha256=sha256(path),
                    status="local_preflight_not_unity_screenshot",
                    notes="non_cat_ui_scale_mockup_uses_chinese_text_no_runtime_binding",
                )
            )

    write_manifest(OUT_DIR / "chinese_ui_scale_batch75_local_mockup_manifest.csv", rows)
    write_contact_sheet(OUT_DIR / "thecat_ui_chinese_scale_batch75_local_mockup_contact_sheet.png", rows)
    write_review_note(OUT_DIR / "chinese_ui_scale_batch75_local_mockup_review.md", rows)
    print(f"Wrote {len(rows)} local scale mockups to {to_project_path(OUT_DIR)}")


def render_surface(surface_id: str, surface_name: str, description: str, width: int, height: int) -> Image.Image:
    image = Image.new("RGBA", (width, height), COLORS["bg"])
    draw = ImageDraw.Draw(image)
    draw_background(draw, width, height)
    draw_header(draw, width, surface_name, description)

    if surface_id == "main_menu_character_select":
        draw_character_select(draw, width, height)
    elif surface_id == "route_map":
        draw_route_map(draw, width, height)
    elif surface_id == "battle_hud":
        draw_battle_hud(draw, width, height)
    elif surface_id == "skill_enemy_hud":
        draw_skill_enemy_hud(draw, width, height)
    else:
        draw_result_pause_settings(draw, width, height)

    draw_footer_checks(draw, width, height)
    return image


def draw_background(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    draw.rectangle((0, 0, width, 72), fill=COLORS["band"])
    draw.rectangle((0, height - 78, width, height), fill=(13, 22, 38, 245))
    for x in range(-height, width + height, 148):
        draw.line((x, 72, x + height // 4, height - 78), fill=(22, 92, 140, 48), width=2)
        draw.line((x + 54, 72, x + 54 + height // 5, height - 78), fill=(151, 111, 37, 42), width=2)


def draw_header(draw: ImageDraw.ImageDraw, width: int, title: str, description: str) -> None:
    title_font = font(28 if width < 1280 else 34, bold=True)
    body_font = font(15 if width < 1280 else 18)
    draw.text((32, 18), title, fill=COLORS["text"], font=title_font)
    draw.text((32, 54), ellipsize(description, 44 if width < 1280 else 72), fill=COLORS["muted"], font=body_font)
    draw.text((width - 282, 24), "本地预检 / 非 Unity 截图", fill=COLORS["gold"], font=font(16, bold=True))


def draw_character_select(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    margin = 36
    top = 104
    card_gap = 18 if width < 1280 else 26
    card_w = max(190, (width - margin * 2 - card_gap * 2) // 3)
    card_h = min(350, height - 250)
    cats = [("裁判", "护盾 / 反击", COLORS["cyan"]), ("奈芙蒂斯", "沙暴 / 月砂", COLORS["gold"]), ("铃音", "冰符 / 治疗", (126, 174, 255, 255))]
    for index, (name, role, accent) in enumerate(cats):
        x = margin + index * (card_w + card_gap)
        draw_panel(draw, x, top, card_w, card_h, accent)
        draw.ellipse((x + card_w // 2 - 46, top + 42, x + card_w // 2 + 46, top + 134), outline=accent, width=4, fill=(11, 19, 33, 255))
        draw.text((x + 22, top + 154), name, fill=COLORS["text"], font=font(26, bold=True))
        draw.text((x + 22, top + 194), role, fill=COLORS["muted"], font=font(16))
        draw_button(draw, x + 22, top + card_h - 74, card_w - 44, 46, "选择出战" if index == 0 else "查看技能", accent)

    draw_note_panel(draw, margin, top + card_h + 22, width - margin * 2, 82, "长文本检查：角色说明需要两行内完成，按钮不裁切，选中态和禁用态保持可见。")


def draw_route_map(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    left = 52
    top = 114
    map_w = width - 104
    map_h = height - 238
    draw_panel(draw, left, top, map_w, map_h, COLORS["cyan"])
    nodes = []
    for row in range(5):
        for col in range(4):
            x = left + 92 + col * ((map_w - 184) // 3)
            y = top + 66 + row * max(58, (map_h - 132) // 4)
            nodes.append((x, y, row, col))
    for index, (x, y, row, col) in enumerate(nodes):
        if index > 0 and col > 0:
            prev = nodes[index - 1]
            draw.line((prev[0], prev[1], x, y), fill=(88, 132, 174, 160), width=2)
        accent = COLORS["gold"] if row == 4 else COLORS["cyan"]
        draw.ellipse((x - 20, y - 20, x + 20, y + 20), fill=(13, 24, 40, 255), outline=accent, width=3)
        draw.text((x - 10, y - 9), str(row * 4 + col + 1), fill=COLORS["text"], font=font(13, bold=True))
    draw.text((left + 28, top + 22), "第 07 层 / 梦境路线", fill=COLORS["text"], font=font(24, bold=True))
    draw_button(draw, width - 230, top + 24, 150, 42, "锁定路线", COLORS["gold"])
    draw_note_panel(draw, left + 28, top + map_h - 88, map_w - 56, 58, "检查：路线名、锁定提示、层数数字与奖励图标不可重叠。")


def draw_battle_hud(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    margin = 34
    gauge_w = max(170, min(270, (width - margin * 2 - 48) // 4))
    labels = [("生命", "168/220", COLORS["green"]), ("困意", "42%", COLORS["cyan"]), ("饥饿", "18", COLORS["gold"]), ("便意", "03", (210, 148, 82, 255))]
    for index, (label, value, color) in enumerate(labels):
        x = margin + (index % 4) * (gauge_w + 16)
        y = 102 if width >= 1150 else 98 + (index // 2) * 48
        if width < 1150:
            x = margin + (index % 2) * (gauge_w + 16)
        draw_panel(draw, x, y, gauge_w, 36, color, radius=10)
        draw.text((x + 12, y + 8), label, fill=COLORS["text"], font=font(13, bold=True))
        draw.text((x + gauge_w - 82, y + 8), value, fill=color, font=font(13, bold=True))
    arena_top = 178 if width < 1150 else 152
    draw_panel(draw, margin, arena_top, width - margin * 2, height - arena_top - 174, COLORS["cyan"])
    draw.text((margin + 26, arena_top + 24), "敌人警告：冷光体正在蓄力", fill=COLORS["gold"], font=font(24, bold=True))
    draw.text((margin + 26, arena_top + 64), "伤害 1288    护盾 +42    冷却 99", fill=COLORS["text"], font=font(20, bold=True))
    slot_y = height - 138
    for index, skill in enumerate(("护盾", "裁决", "沙暴", "冰符", "治疗")):
        x = margin + index * 78
        draw.rounded_rectangle((x, slot_y, x + 58, slot_y + 58), radius=10, fill=(12, 24, 38, 255), outline=COLORS["cyan"], width=2)
        draw.text((x + 10, slot_y + 18), skill, fill=COLORS["text"], font=font(13, bold=True))
        if index == 2:
            draw.text((x + 36, slot_y + 4), "99", fill=COLORS["gold"], font=font(12, bold=True))
    draw_button(draw, width - 156, 104, 120, 40, "暂停", COLORS["cyan"])


def draw_skill_enemy_hud(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    margin = 36
    top = 108
    col_gap = 24
    left_w = max(350, (width - margin * 2 - col_gap) // 2)
    right_w = width - margin * 2 - col_gap - left_w
    panel_h = height - top - 116
    draw_panel(draw, margin, top, left_w, panel_h, COLORS["cyan"])
    draw_panel(draw, margin + left_w + col_gap, top, right_w, panel_h, COLORS["gold"])
    draw.text((margin + 24, top + 24), "技能详情", fill=COLORS["text"], font=font(26, bold=True))
    skills = [("守梦护盾", "给全队 42 护盾，持续 8 秒。"), ("月砂漩涡", "造成 1288 伤害，并降低敌人速度。"), ("冰符守护", "清除一次负面状态。")]
    y = top + 78
    for title, desc in skills:
        draw.text((margin + 24, y), title, fill=COLORS["gold"], font=font(18, bold=True))
        draw_wrapped(draw, desc, margin + 24, y + 28, left_w - 48, font(15), COLORS["muted"], line_gap=4)
        y += 92
    rx = margin + left_w + col_gap + 24
    draw.text((rx, top + 24), "敌方状态", fill=COLORS["text"], font=font(26, bold=True))
    for i, (name, value) in enumerate((("冷光体", "HP 1840"), ("黑泥", "HP 620"), ("召唤暴君", "蓄力 3 回合"))):
        yy = top + 84 + i * 86
        draw.rounded_rectangle((rx, yy, rx + right_w - 48, yy + 58), radius=12, fill=(12, 22, 36, 255), outline=COLORS["line"], width=2)
        draw.text((rx + 16, yy + 16), name, fill=COLORS["text"], font=font(17, bold=True))
        draw.text((rx + right_w - 170, yy + 16), value, fill=COLORS["gold"], font=font(15, bold=True))


def draw_result_pause_settings(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    margin = 38
    top = 108
    panel_w = width - margin * 2
    panel_h = height - top - 114
    draw_panel(draw, margin, top, panel_w, panel_h, COLORS["gold"])
    draw.text((margin + 28, top + 26), "结算奖励", fill=COLORS["text"], font=font(28, bold=True))
    draw.text((margin + 28, top + 68), "梦币 +1288    经验 +420    祝福碎片 +03", fill=COLORS["gold"], font=font(20, bold=True))
    button_y = top + 128
    for index, label in enumerate(("继续守梦", "暂停设置", "重新挑战")):
        draw_button(draw, margin + 28 + index * 156, button_y, 132, 46, label, COLORS["cyan"] if index != 1 else COLORS["gold"])
    settings_y = top + 210
    for index, label in enumerate(("音乐音量", "特效音量", "震动反馈", "自动暂停")):
        y = settings_y + index * 54
        draw.text((margin + 32, y), label, fill=COLORS["text"], font=font(17, bold=True))
        draw.rounded_rectangle((margin + 190, y + 4, margin + 470, y + 22), radius=8, fill=(12, 24, 38, 255), outline=COLORS["line"], width=2)
        draw.rectangle((margin + 194, y + 8, margin + 340 + index * 18, y + 18), fill=COLORS["cyan"])
    draw_note_panel(draw, margin + 32, top + panel_h - 86, panel_w - 64, 58, "长文本：获得的祝福说明必须换行，不可压住按钮或开关。")


def draw_footer_checks(draw: ImageDraw.ImageDraw, width: int, height: int) -> None:
    y = height - 58
    x = 30
    small = font(12 if width < 1280 else 14)
    for check in CHECKS:
        token = f"[ ] {check}"
        draw.text((x, y), token, fill=COLORS["muted"], font=small)
        x += max(128, int(draw.textlength(token, font=small)) + 20)
        if x > width - 180:
            break


def draw_panel(draw: ImageDraw.ImageDraw, x: int, y: int, w: int, h: int, accent: tuple[int, int, int, int], radius: int = 14) -> None:
    draw.rounded_rectangle((x, y, x + w, y + h), radius=radius, fill=COLORS["panel"], outline=accent, width=2)


def draw_button(draw: ImageDraw.ImageDraw, x: int, y: int, w: int, h: int, label: str, accent: tuple[int, int, int, int]) -> None:
    draw.rounded_rectangle((x, y, x + w, y + h), radius=10, fill=(15, 29, 46, 255), outline=accent, width=2)
    text_font = font(15, bold=True)
    tw = draw.textlength(label, font=text_font)
    draw.text((x + (w - tw) / 2, y + (h - 18) / 2), label, fill=COLORS["text"], font=text_font)


def draw_note_panel(draw: ImageDraw.ImageDraw, x: int, y: int, w: int, h: int, text: str) -> None:
    draw.rounded_rectangle((x, y, x + w, y + h), radius=12, fill=COLORS["panel2"], outline=COLORS["line"], width=2)
    draw_wrapped(draw, text, x + 18, y + 16, w - 36, font(15), COLORS["muted"], line_gap=4)


def draw_wrapped(draw: ImageDraw.ImageDraw, text: str, x: int, y: int, max_w: int, text_font: ImageFont.ImageFont, fill: tuple[int, int, int, int], line_gap: int = 6) -> None:
    line = ""
    cursor_y = y
    for char in text:
        candidate = line + char
        if draw.textlength(candidate, font=text_font) <= max_w or not line:
            line = candidate
            continue
        draw.text((x, cursor_y), line, fill=fill, font=text_font)
        cursor_y += text_font.size + line_gap if hasattr(text_font, "size") else 18 + line_gap
        line = char
    if line:
        draw.text((x, cursor_y), line, fill=fill, font=text_font)


def write_manifest(path: Path, rows: list[MockupRow]) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].__dict__.keys()))
        writer.writeheader()
        for row in rows:
            writer.writerow(row.__dict__)


def write_contact_sheet(path: Path, rows: list[MockupRow]) -> None:
    thumb_w, thumb_h = 360, 210
    row_h = 286
    sheet_w, sheet_h = 1920, 1600
    sheet = Image.new("RGBA", (sheet_w, sheet_h), COLORS["bg"])
    draw = ImageDraw.Draw(sheet)
    draw.text((40, 28), "Batch 75 local Chinese UI scale mockups", fill=COLORS["text"], font=font(34, bold=True))
    draw.text((40, 76), "20 local preflight boards: 5 surfaces x 4 resolutions. Not Unity screenshots.", fill=COLORS["gold"], font=font(18, bold=True))
    for index, row in enumerate(rows):
        col = index % 4
        line = index // 4
        x = 40 + col * 460
        y = 128 + line * row_h
        image = Image.open(PROJECT_ROOT / row.path).convert("RGBA")
        preview = fit(image, thumb_w, thumb_h)
        draw.rounded_rectangle((x, y, x + thumb_w + 18, y + thumb_h + 62), radius=8, fill=COLORS["panel"], outline=COLORS["line"], width=1)
        sheet.alpha_composite(preview, (x + 9, y + 9))
        draw.text((x + 10, y + thumb_h + 20), f"{row.surface_id} / {row.resolution}", fill=COLORS["muted"], font=font(13))
    sheet.save(path)


def write_review_note(path: Path, rows: list[MockupRow]) -> None:
    lines = [
        "# P0 Batch 75 - Local Chinese UI Scale Mockup Review",
        "",
        "Status: `local_preflight_not_unity_screenshot`",
        "",
        "These 20 mockups render the required Batch 75 surface/resolution matrix with Chinese UI text, cooldown numbers, reward numbers, and narrow-screen layouts. They are local visual preflight only and do not replace Unity screenshots or Console notes.",
        "",
        "## Outputs",
        "",
        "- `mockups/`: 5 P0 UI surfaces x 4 required resolutions = 20 PNGs.",
        "- `thecat_ui_chinese_scale_batch75_local_mockup_contact_sheet.png`: reviewer contact sheet.",
        "- `chinese_ui_scale_batch75_local_mockup_manifest.csv`: hash and path manifest.",
        "",
        "## Watch Items",
        "",
        "- Compact 1024x768 layouts should be checked for wrapped Chinese text and control stacking.",
        "- Battle and skill HUD numbers should stay readable beside cooldown `99`, HP, reward, and damage values.",
        "- Local mockups use deterministic composition and cannot prove Unity Canvas scaling, font fallback, Console state, prefab binding, or runtime overlap.",
        "",
        "## Matrix",
        "",
        "| Surface | 1024x768 | 1280x720 | 1600x900 | 1920x1080 |",
        "| --- | --- | --- | --- | --- |",
    ]
    by_surface = {surface_id: [] for surface_id, _, _ in SURFACES}
    for row in rows:
        by_surface[row.surface_id].append(row)
    for surface_id, surface_name, _ in SURFACES:
        cells = []
        for _, width, height in RESOLUTIONS:
            resolution = f"{width}x{height}"
            match = next(row for row in by_surface[surface_id] if row.resolution == resolution)
            cells.append(f"`{Path(match.path).name}`")
        lines.append(f"| {surface_name} | " + " | ".join(cells) + " |")
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def fit(image: Image.Image, max_w: int, max_h: int) -> Image.Image:
    scale = min(max_w / image.width, max_h / image.height)
    return image.resize((max(1, round(image.width * scale)), max(1, round(image.height * scale))), Image.Resampling.LANCZOS)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = (
        "C:/Windows/Fonts/msyhbd.ttc" if bold else "C:/Windows/Fonts/msyh.ttc",
        "C:/Windows/Fonts/simhei.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    )
    for candidate in candidates:
        try:
            return ImageFont.truetype(candidate, size)
        except OSError:
            continue
    return ImageFont.load_default()


def ellipsize(text: str, max_chars: int) -> str:
    if len(text) <= max_chars:
        return text
    return text[: max(1, max_chars - 3)] + "..."


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_project_path(path: Path) -> str:
    return path.resolve().relative_to(PROJECT_ROOT).as_posix()


if __name__ == "__main__":
    main()
