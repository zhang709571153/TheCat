# 梦境支配者资产与策划结构索引

## 顶层结构

- `docs/`：按策划模块整理的 Markdown，可继续扩展新系统文档。
- `markdown/`：云文档 Markdown 原始克隆，不直接编辑。
- `docx/`：云文档 DOCX 原始导出，不直接编辑。
- `assets/`：从文档和云盘文件落档的图片资产。
- `assets/_raw_extracted/`：DOCX 原始媒体抽取，便于追溯。

## 策划文档结构

- `docs/00_overview/`：P0 最小版本、范围、验收目标。
- `docs/01_core_gameplay/`：核心玩法、玩家路径、单局循环。
- `docs/02_combat_and_systems/`：核心数值、状态标签、战斗系统评估。
- `docs/03_characters/`：角色设定、台词、角色动画方案。
- `docs/04_art_production/`：美术/数字资产清单与制作拆分。
- `docs/05_references/`：外部系统拆解与参考。

## 角色资产结构

- `assets/characters/ch01_saiban_swordsaint`：塞班 / 圣剑士
- `assets/characters/ch02_nephthys_moonsand_agent`：奈芙蒂斯 / 月沙特工
- `assets/characters/ch03_suzune_sleep_shrine_maiden`：铃音 / 安眠巫女
- `assets/characters/ch04_kagemaru_shadowstalker`：影丸 / 逐影者
- `assets/characters/ch05_mianhua_dreamweaver`：棉花 / 梦织女
- `assets/characters/ch06_yuheng_yinyang_order`：玉衡 / 阴阳令
- `assets/characters/ch07_mailuo_noon_ranger`：麦洛 / 正午游侠
- `assets/characters/ch08_jiangtai_opera_cat`：绛台 / 梨园伶猫

每个角色目录统一保留：`concept/`、`turnaround/`、`animation/`、`references/`、`notes/`、`_source/`。新增角色时复制 `assets/characters/_CHARACTER_TEMPLATE.md` 并建立同构目录。

## 资产统计

- `character_animation`：8
- `character_concept`：8
- `character_group`：1
- `character_reference`：1
- `character_turnaround`：8
- `enemy_animation`：6
- `enemy_concept`：6
- `key_art`：1
- `level_concept`：4
- `level_reference`：2
- `level_sprite`：6
