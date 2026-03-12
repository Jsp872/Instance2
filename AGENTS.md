# AGENTS.md

## Project snapshot
- Unity project on **Unity 6** (`ProjectSettings/ProjectVersion.txt`: `6000.3.10f1`).
- Active build scene list currently contains only `Assets/Scenes/Level.unity` (`ProjectSettings/EditorBuildSettings.asset`).
- Source AI guidance files were searched via one glob (`**/{.github/copilot-instructions.md,AGENT.md,AGENTS.md,CLAUDE.md,.cursorrules,.windsurfrules,.clinerules,.cursor/rules/**,.windsurf/rules/**,.clinerules/**,README.md}`) and none were found.

## Big picture architecture (current state)
- First-party gameplay code is minimal and lives in `Assets/Scripts/`.
- Main reusable pattern is a static typed event bus in `Assets/Scripts/Utils/EventBus.cs`.
- Event payloads are `struct`s (example: `Callback` in `Assets/Scripts/Utils/TEST_EventBus.cs`) and are dispatched by type.
- Publish/subscribe flow: `Subscribe<T>(Action<T>)` / `Publish<T>(T)` / `Unsubscribe<T>(Action<T>)` in `EventBus.cs`.
- `Assets/Scenes/EventBusScene.unity` is a manual demo scene for the bus (`EventBusTEST` object with `TEST_EventBus` component), but is **not** in build settings.
- `Assets/Scripts/Obstacle.cs` is an empty `MonoBehaviour` placeholder.

## Scene and hierarchy conventions
- Scenes use top-level grouping objects named like `--- Environment---`, `--- Dynamics ---`, `--- Entities ---` (seen in both `Level.unity` and `EventBusScene.unity`).
- Keep this grouping pattern when adding runtime objects to reduce hierarchy drift.

## Input/rendering/tooling integration points
- Input System is enabled and wired through `Assets/InputSystem_Actions.inputactions`; build settings map `com.unity.input.settings.actions` to this asset.
- URP is configured (`Assets/Settings/UniversalRP.asset`, `Assets/Settings/Renderer2D.asset`, global settings assets in `Assets/`).
- Third-party integrations are vendored under `Assets/Plugins/`: FMOD (`Assets/Plugins/FMOD`) and DOTween (`Assets/Plugins/Demigiant/DOTween`).
- Treat plugin code as external: avoid edits under `Assets/Plugins/**` unless task explicitly targets vendor integration.

## Developer workflows (practical)
- Primary loop is Editor-driven: open project, edit scripts/scenes, press Play.
- If you need to validate compilation quickly, use Unity batchmode in CI/local (set your Unity executable path):
  - `Unity.exe -batchmode -projectPath "C:\Users\P0ulpy\Documents\GitHub\Instance2" -quit -logFile Logs\\batchmode.log`
- If tests are introduced, run Unity Test Framework in batchmode (`com.unity.test-framework` is installed):
  - `Unity.exe -batchmode -projectPath "C:\Users\P0ulpy\Documents\GitHub\Instance2" -runTests -testPlatform EditMode -quit -logFile Logs\\editmode-tests.log -testResults Logs\\editmode-tests.xml`

## Code conventions observed in this repo
- No custom asmdefs for game code; scripts compile into default `Assembly-CSharp`.
- Current scripts use no namespaces; match local style unless asked to refactor broadly.
- Inspector-facing data uses `[SerializeField] private ...` (example: `TEST_EventBus.data`).
- EventBus listener lifecycle is expected to be paired (`OnEnable` subscribe, `OnDisable` unsubscribe) per in-file usage docs in `EventBus.cs`.
- Keep Unity `.meta` files intact; project uses **Visible Meta Files** (`ProjectSettings/VersionControlSettings.asset`).

