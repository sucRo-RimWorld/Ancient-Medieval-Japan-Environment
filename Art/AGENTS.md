# AMJ Environment — Art Asset Transfer Agent Instructions

Scope: files under `Art/` (accepted source masters and their paired production derivatives). These rules govern the entire corresponding `Textures/` transfer step. Do not duplicate painting/style rules here; see `../Docs/ArtDirection.md` and `../Docs/GoldenPaths/TextureAssetPipeline.md`.

## Before any accepted image is pushed

1. Read repository root `AGENTS.md` and **main** `Docs/Coordination.md`; obey Project `AGENTS.md`, `Docs/SharedRules.md`, and PR-only main protection.
2. Identify the *actual author-accepted* image and its provenance. Never substitute a regenerated/reconstructed image or a comparison-sheet crop. Determine the source path under `Art/Sources/` and its deterministic production derivative under `Textures/` from the owning Def/art spec.
3. Verify actual PNG data, dimensions, alpha and SHA-256. Preserve the exact source bytes. An accepted 1254px master must never be overwritten with its 256px derivative.

## Chat → GitHub binary transfer: proven route

**Do not claim that image transfer is impossible because the GitHub Contents text endpoint, a local Git network connection, or a particular plugin endpoint cannot upload PNGs.** The project has successfully transferred image binaries through the following tool chain, including Environment Yoshi on 2026-10-12 (PR #128, binary commit `a3a8e6346f38d7bb49cd18b4148da02b47c2cdd4`).

1. From the *real local PNG bytes*, encode Base64 **without modifying the PNG**, with standard padding, and stage each as a temporary `.b64.txt` file in `/mnt/data/`. Do not emit Base64 in the user-visible answer and do not create a ZIP.
2. Use `files__manage_library` (`upload` with `container_path`) to place the temporary Base64 text in a uniquely named **personal Library temporary folder**. This is an internal transfer bridge, not the artwork's permanent repository.
3. In the *same `functions.exec` call*, use `tools.files__read({read:[{ref_id:<returned file_id>,mode:"full"}]})` and pass the **complete returned Base64 string** directly to `tools.mcp__GitHub__create_blob({repository_full_name,encoding:"base64",content})`. Do not print, truncate, manually reconstruct, or excerpt the string; validate length and expected PNG Base64 prefix/suffix before uploading. Large sources may require one file/blob per call.
4. From the current **topic branch** head, get its commit tree SHA. Use `mcp__GitHub__create_tree` with `base_tree_sha` and new `{path,mode:"100644",type:"blob",sha}` entries for **both** `Art/Sources/...` and `Textures/...`. Use `create_commit` with the existing topic head as parent, then `update_ref` with `expected_sha` for that **topic branch only**. Never write directly to main.
5. Verify the resulting remote paths, exact blob SHA-1 (Git blob framing `"blob "+byte_length+"\\0"` plus bytes), actual image SHA-256 when independently readable, full content/PNG integrity, and changed-file list. Blob creation alone is **not** a successful push. Keep the PR open until checks for its exact final head are complete.
6. After a verified transfer, **delete the temporary Library files** through `files__manage_library`. Preserve source masters and production images; never delete accepted art. Do not leave large Base64 transfer intermediates in Library.

If one step fails, inspect the actual tool result and attempt the already established alternate stage; report the exact failed step only after exhausting authorized paths. Do not replace a requested binary push with a documentation-only PR. Do not claim tests, main merge, or in-game acceptance without the corresponding evidence.

## Completion and audit

For image-only acceptance, distinguish **binary pushed to topic PR** from **PR merged**, **Def wired**, and **runtime validated**. Follow `../Docs/GoldenPaths/TextureAssetPipeline.md` for the actual export/installation/loaded-game gates. Report actual commit SHA, PR and tests performed, including **test case names and intended assertions** rather than only PASS.
