from __future__ import annotations

import csv
import hashlib
from pathlib import Path


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_56_formal_install_decision_packet_2026-06-15"
DECISION_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "formal_install_decisions" / BATCH_SLUG
DECISION_CSV = DECISION_DIR / "formal_install_decision_batch56.csv"
DECISION_NOTE = DECISION_DIR / "formal_install_decision_batch56_review.md"
PROCESS_NOTE = DECISION_DIR / "formal_install_decision_batch56_process_note.md"
AGENT_PROMPT = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_56_formal_install_decision_packet.md"

FIELD_NAMES = (
    "decision_id",
    "subject_group",
    "related_batches",
    "decision",
    "install_allowed",
    "candidate_manifest_path",
    "candidate_manifest_sha256",
    "candidate_review_path",
    "candidate_review_sha256",
    "candidate_process_path",
    "candidate_process_sha256",
    "unity_import_root",
    "runtime_binding_scope",
    "required_unity_evidence",
    "blocking_reason",
    "next_action",
    "rollback_note",
)

DECISIONS = (
    {
        "decision_id": "starter_cat_saiban_batch49",
        "subject_group": "starter_cat_saiban",
        "related_batches": "batch_49_saiban_low_drift_refinement_2026-06-15",
        "manifest": "design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Characters/Sprites",
        "runtime_binding_scope": "cat.combat.saiban",
        "required_unity_evidence": "04-active-cat-saiban.png;Unity Console no new errors;colored-turnaround comparison approval",
        "blocking_reason": "Saiban active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked",
        "next_action": "capture Saiban active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement",
        "rollback_note": "do not touch current locked Saiban Unity sprite until the formal import gate approves it",
    },
    {
        "decision_id": "starter_cat_nephthys_batch50",
        "subject_group": "starter_cat_nephthys",
        "related_batches": "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15",
        "manifest": "design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Characters/Sprites",
        "runtime_binding_scope": "cat.combat.nephthys",
        "required_unity_evidence": "05-active-cat-nephthys.png;Unity Console no new errors;colored-turnaround comparison approval",
        "blocking_reason": "Nephthys active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked",
        "next_action": "capture Nephthys active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement",
        "rollback_note": "do not touch current locked Nephthys Unity sprite until the formal import gate approves it",
    },
    {
        "decision_id": "starter_cat_suzune_batch51",
        "subject_group": "starter_cat_suzune",
        "related_batches": "batch_51_suzune_strict_ai_generation_candidate_2026-06-15",
        "manifest": "design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Characters/Sprites",
        "runtime_binding_scope": "cat.combat.suzune",
        "required_unity_evidence": "06-active-cat-suzune.png;Unity Console no new errors;colored-turnaround comparison approval",
        "blocking_reason": "Suzune active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked",
        "next_action": "capture Suzune active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement",
        "rollback_note": "do not touch current locked Suzune Unity sprite until the formal import gate approves it",
    },
    {
        "decision_id": "enemy_black_mud_batch40",
        "subject_group": "enemy_black_mud_nightmare",
        "related_batches": "batch_40_black_mud_cutout_candidate_2026-06-15",
        "manifest": "design/development/asset_candidates/enemies/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Enemies/Sprites",
        "runtime_binding_scope": "enemy.black_mud_nightmare",
        "required_unity_evidence": "07-active-enemy-black-mud.png;Unity Console no new errors;bed-pressure scale review;prefab/scene binding review",
        "blocking_reason": "Black Mud active-enemy screenshot and prefab/scene binding evidence are missing",
        "next_action": "capture Black Mud runtime screenshot and verify bed-pressure scale before choosing a replacement",
        "rollback_note": "keep current enemy runtime bindings unchanged until active-enemy review approves the candidate",
    },
    {
        "decision_id": "enemy_cold_light_batch42",
        "subject_group": "enemy_cold_light_shadow",
        "related_batches": "batch_42_cold_light_cutout_candidate_2026-06-15",
        "manifest": "design/development/asset_candidates/enemies/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Enemies/Sprites",
        "runtime_binding_scope": "enemy.cold_light_shadow",
        "required_unity_evidence": "08-active-enemy-cold-light.png;Unity Console no new errors;ranged warning readability;prefab/scene binding review",
        "blocking_reason": "Cold Light active-enemy screenshot and warning readability evidence are missing",
        "next_action": "capture Cold Light runtime screenshot and verify ranged warning readability before choosing a replacement",
        "rollback_note": "keep current enemy runtime bindings unchanged until active-enemy review approves the candidate",
    },
    {
        "decision_id": "enemy_call_tyrant_batch44",
        "subject_group": "enemy_call_tyrant",
        "related_batches": "batch_44_call_tyrant_cutout_candidate_2026-06-15",
        "manifest": "design/development/asset_candidates/enemies/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Enemies/Sprites",
        "runtime_binding_scope": "enemy.call_tyrant",
        "required_unity_evidence": "09-active-boss-call-tyrant.png;Unity Console no new errors;boss summon/throw readability;prefab/scene binding review",
        "blocking_reason": "Call Tyrant active-boss screenshot and Boss behavior readability evidence are missing",
        "next_action": "capture Call Tyrant runtime screenshot and verify summon/throw readability before choosing a replacement",
        "rollback_note": "keep current Boss runtime bindings unchanged until active-boss review approves the candidate",
    },
    {
        "decision_id": "bedroom_interactables_batch54",
        "subject_group": "bedroom_interactables",
        "related_batches": "batch_54_bedroom_interactable_candidates_2026-06-15",
        "manifest": "design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/Scenes/BedroomDream",
        "runtime_binding_scope": "bed;interactable.litter_box;interactable.feeder",
        "required_unity_evidence": "battle-world runtime screenshot;Unity Console no new errors;Sprite import settings;pathing readability;interaction feedback;scene binding review",
        "blocking_reason": "runtime scale, pathing readability, and scene/prefab binding evidence are missing",
        "next_action": "run a Unity install preview scene and compare Batch 54 props against current BedroomDream sprites before any replacement",
        "rollback_note": "leave current BedroomDream prop sprites unchanged unless runtime scale and interaction readability pass",
    },
    {
        "decision_id": "starter_skill_vfx_batch55",
        "subject_group": "starter_skill_vfx",
        "related_batches": "batch_55_starter_skill_vfx_candidates_2026-06-15",
        "manifest": "design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_manifest.csv",
        "unity_import_root": "Assets/TheCat/Art/VFX",
        "runtime_binding_scope": "skill.saiban;skill.nephthys;skill.suzune;status.shield;status.slow;status.mark;status.knockback;status.sleep_stable",
        "required_unity_evidence": "skill timing screenshot set;Unity Console no new errors;Sprite import settings;runtime VFX scale;HUD/world readability;scene binding review",
        "blocking_reason": "runtime VFX scale, skill timing, and scene/prefab binding evidence are missing; Saiban paw emblem needs approval or simplification",
        "next_action": "test VFX at combat scale and split full emblems into smaller per-skill sprites if readability is too busy",
        "rollback_note": "leave current generic VFX bindings unchanged until a specific VFX install target is approved",
    },
)


def main() -> None:
    DECISION_DIR.mkdir(parents=True, exist_ok=True)
    rows = build_rows()
    write_csv(rows)
    write_review(rows)
    write_process(rows)
    print("Wrote P0 Batch 56 blocked formal install decision packet.")
    print(to_repo_path(DECISION_CSV))


def build_rows() -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    for decision in DECISIONS:
        manifest_path = resolve(decision["manifest"])
        first_manifest_row = read_first_manifest_row(manifest_path)
        review_path = resolve(first_manifest_row.get("review_note") or first_manifest_row.get("review_sheet"))
        process_path = resolve(first_manifest_row.get("process_note") or first_manifest_row.get("review_note"))
        rows.append(
            {
                "decision_id": decision["decision_id"],
                "subject_group": decision["subject_group"],
                "related_batches": decision["related_batches"],
                "decision": "blocked_pending_unity_evidence",
                "install_allowed": "false",
                "candidate_manifest_path": decision["manifest"],
                "candidate_manifest_sha256": sha256(manifest_path),
                "candidate_review_path": to_repo_path(review_path),
                "candidate_review_sha256": sha256(review_path),
                "candidate_process_path": to_repo_path(process_path),
                "candidate_process_sha256": sha256(process_path),
                "unity_import_root": decision["unity_import_root"],
                "runtime_binding_scope": decision["runtime_binding_scope"],
                "required_unity_evidence": decision["required_unity_evidence"],
                "blocking_reason": decision["blocking_reason"],
                "next_action": decision["next_action"],
                "rollback_note": decision["rollback_note"],
            }
        )
    return rows


def read_first_manifest_row(path: Path) -> dict[str, str]:
    with path.open("r", encoding="utf-8", newline="") as handle:
        reader = csv.DictReader(handle)
        for row in reader:
            return {key: value for key, value in row.items() if key is not None}
    raise ValueError(f"Manifest has no rows: {path}")


def write_csv(rows: list[dict[str, str]]) -> None:
    with DECISION_CSV.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Formal Install Decision Batch 56 Review",
        "",
        "Decision: blocked pending Unity evidence. No candidate is approved for install.",
        "",
        "This packet keeps the Codex candidate pipeline honest after Batch 54 and Batch 55: candidates can be produced outside Unity, but installation still requires Unity Console, screenshot, Sprite import, runtime scale, HUD/world readability, and scene/prefab binding evidence.",
        "",
        "## Unity MCP Status",
        "",
        "- Local setup check found Unity `6000.4.10f1`, `com.unity.ai.assistant` `2.12.0-pre.1`, relay `C:/Users/PC/.unity/relay/relay_win.exe`, and Codex config entries for Unity MCP.",
        "- Current Codex tool discovery did not expose Unity MCP editor tools, so Console, AssetDatabase, Play Mode, screenshot, and scene/prefab checks are deferred.",
        "- Existing connection registry includes approved records and older capacity/plan-limit records; current validation still requires live Unity MCP tool calls.",
        "",
        "## Decisions",
        "",
    ]
    for row in rows:
        lines.extend(
            [
                "### " + row["subject_group"],
                "",
                "- Decision: `" + row["decision"] + "`",
                "- Install allowed: `" + row["install_allowed"] + "`",
                "- Related batches: `" + row["related_batches"] + "`",
                "- Candidate manifest: `" + row["candidate_manifest_path"] + "`",
                "- Candidate review: `" + row["candidate_review_path"] + "`",
                "- Unity import root: `" + row["unity_import_root"] + "`",
                "- Runtime binding scope: `" + row["runtime_binding_scope"] + "`",
                "- Required Unity evidence: `" + row["required_unity_evidence"] + "`",
                "- Blocking reason: " + row["blocking_reason"],
                "- Next action: " + row["next_action"],
                "- Rollback note: " + row["rollback_note"],
                "",
            ]
        )

    lines.extend(
        [
            "## Safety",
            "",
            "- No files were copied into `Assets`.",
            "- No Unity `.meta` files were created.",
            "- Runtime visual bindings and manifest catalogs remain unchanged.",
            "- Batch 54 and Batch 55 are complete candidate packs, not install approvals.",
            "",
        ]
    )
    DECISION_NOTE.write_text("\n".join(lines), encoding="utf-8")


def write_process(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Formal Install Decision Batch 56 Process Note",
        "",
        "Process: generated a blocked decision packet from current candidate manifests and review records.",
        "",
        "The packet intentionally records install blockers instead of approving assets, because Unity MCP editor tools are not exposed in the current Codex tool surface.",
        "",
        "Local MCP setup command run:",
        "",
        "```powershell",
        "powershell -NoProfile -ExecutionPolicy Bypass -File C:/Users/PC/.codex/skills/unity-mcp-smoke-test/scripts/check-unity-mcp-local.ps1 -ProjectPath D:/Unity Workspace/TheCat",
        "```",
        "",
        "Observed local setup summary: Unity `6000.4.10f1`, Unity AI Assistant `2.12.0-pre.1`, relay exists, Codex config contains Unity, connection registry exists with approved records and older capacity/plan-limit records.",
        "",
        "Rows: " + str(len(rows)),
        "",
        "No Unity import was performed.",
        "",
    ]
    PROCESS_NOTE.write_text("\n".join(lines), encoding="utf-8")


def resolve(relative_path: str | None) -> Path:
    if not relative_path:
        raise ValueError("Missing path.")
    path = REPO_ROOT / relative_path.replace("/", "\\")
    if not path.exists():
        raise FileNotFoundError(path)
    return path


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
