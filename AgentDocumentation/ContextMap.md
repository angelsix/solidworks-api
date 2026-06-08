# solidworks-api — Context map

Orientation map for agents working on **SolidDna**, a C# SDK that wraps the SolidWorks COM API to make building SolidWorks add-ins easier and safer. For change-impact on a specific method, run the live tool against the SDK solution:

```sh
contextmap impact --member "AddInIntegration.ConnectToSW" --solution ~/Documents/GitHub/solidworks-api/SolidDna/AngelSix.SolidDna/AngelSix.SolidDna.sln
```

> **Code age warning** (from `AgentDocumentation/Glossary.md`): the codebase is old and predates many SolidWorks API changes. Before substantive coding, a review against the latest SolidWorks API is expected to surface deprecated/removed APIs and breaking changes.

## What this is

The repo is one shippable SDK plus its support cast. The SDK — `AngelSix.SolidDna` — wraps SolidWorks COM interop objects (`ModelDoc2`, `Component2`, …) in disposable .NET classes so developers write modern, leak-safe add-ins. It ships as the **`AngelSix.SolidDna` NuGet package** (`net472`, currently `Version` 1.0.1.13) and via VSIX project templates. Everything else in the repo is templates, tutorials, tools, and a script runner that consume that one SDK.

## Solution / project layout

16 `.sln` files, 17 `.csproj`. The one that matters is the SDK; the rest are consumers/scaffolding.

- **`SolidDna/AngelSix.SolidDna/`** — the SDK. `AngelSix.SolidDna.sln` → `AngelSix.SolidDna.csproj` (`net472`, `PackageId AngelSix.SolidDna`, depends on `Dna.Framework`). This is where real changes live.
- **`ScriptRunner/SolidDNA.ScriptRunner/`** — an add-in that runs ad-hoc C# scripts inside SolidWorks (testing/automation without building a full add-in). Has a WPF taskpane (`MyTaskpaneUI.cs`, `MyAddinControl.xaml`) and `ScriptWrapperFormat.cs`.
- **`Templates/`** — VSIX project templates devs scaffold from: `SolidDna.Blank`, `SolidDNA.StandAlone`, `SolidDna.WPF.Blank`, and the `VSIX Installer` (`SolidDNA.Templates`) that packages them.
- **`Tutorials/01-…` through `08-StandAlone/`** — worked examples mapped to YouTube videos (blank add-in, WPF, custom properties, selection, dynamic plug-in reload, exporting, NuGet consumption, standalone). Reference material, not the product.
- **`Tools/`** — two standalone WPF utilities: `Addin Installer` (registers/unregisters add-ins) and `CommandManager Icon Generator` (builds CommandManager icon sprite sheets).
- **`References/`, `Resources/`** — interop/assets. Agent docs live under `AgentDocumentation/` (`Project.md`, `Memory.md`, `Glossary.md`, `Sessions/`).

## Subsystems (all under `SolidDna/AngelSix.SolidDna/`)

- **Integration / lifecycle** — `SolidWorks/Integration/`. `AddIn/AddInIntegration.cs` is the heart: an abstract `ISwAddin` base SolidWorks loads via COM. `PlugInIntegration.cs` discovers and loads SolidDna plug-ins; `AppDomainBoundary*.cs` supports loading plug-ins in a detached AppDomain; `ComRegisterAddinIntegration.cs` handles COM register/unregister; `TaskpaneIntegration.cs` wires WPF taskpanes. `Base/SolidDnaPlugIn.cs` + `PlugInDetails.cs` define the plug-in contract.
- **Application shell** — `SolidWorks/Application/`. `SolidWorksApplication.cs` wraps the SolidWorks app (exposed as the static `AddInIntegration.SolidWorks`), plus version/preferences and `MessageBox/`.
- **Models** — `SolidWorks/Models/`. `Model.cs` / `ModelExtension.cs` wrap `ModelDoc2` for Part/Assembly/Drawing (`ModelType.cs`). Subfolders: `Component/`, `Configuration/`, `Documents/`, `Drawing/`, `Feature/` (Feature + FeatureData), `PackAndGo/`, `Saving/` (options/pdf/results).
- **UI building blocks** — `SolidWorks/CommandManager/` (ribbon: `CommandManager.cs` + Group/Tab/Item/Flyout), `SolidWorks/Taskpane/`, `SolidWorks/Annotations/`, `Note/`, `Balloon/`, `Dimension/`.
- **Document data** — `SolidWorks/CustomProperties/`, `Mass/`, `Material/`, `SelectionManager/`.
- **COM safety core** — `SolidWorks/Core/`: `SolidDnaObject.cs` / `SharedSolidDnaObject.cs` / `SolidDnaObjectDisposal.cs` wrap raw COM objects with managed disposal (the `UnsafeObject` escape hatch lives here).
- **Cross-cutting** — `DependencyInjection/` (`IoCContainer.cs`, `ConfigureServiceAttribute.cs`, `DependencyInjectionExtensions.cs`); `Errors/` (`SolidDnaErrors`, `SolidDnaException`, error codes); `Exception/`, `File/`, `Async/`, `Threading/`, `Reflection/`, `Enumerators/`, `Localization/` (Strings + Implementation).

## Key flows

- **Add-in startup** (in `AddInIntegration.ConnectToSW`): `ConfigureServices` → `PreConnectToSolidWorks` → `PreLoadPlugIns` → `ApplicationStartup` → `ConnectedToSolidWorks` (also fans out to `PlugInIntegration.ConnectedToSolidWorks`). On unload: `DisconnectedFromSolidWorks`. The three `Pre*`/startup methods are `abstract` — a concrete add-in must implement them.
- **COM registration** — SolidWorks discovers an add-in through COM `ComRegister`/`ComUnregister` entry points (`ComRegisterAddinIntegration.cs`); the `Addin Installer` tool automates that registration outside Visual Studio.
- **Plug-in loading** — `PlugInIntegration` enumerates `SolidDnaPlugIn` implementations (optionally in a separate AppDomain via `AppDomainBoundary`) and forwards SolidWorks lifecycle events to them.
- **COM object access** — wrap any raw interop object in a `SolidDnaObject<T>`; consume via the SDK wrapper and let disposal flow through `SolidDnaObjectDisposal`. Reaching for `.UnsafeObject` means you own disposal from there.

## Invariants / conventions

- **COM disposal is the prime directive.** Every raw SolidWorks COM object goes through `SolidDnaObject<T>`; do not hold or leak `UnsafeObject` references — leaks crash/hang SolidWorks. New wrappers follow the `Core/` disposal pattern.
- **Target stays `net472`** (SolidWorks add-ins load into the SolidWorks process). The SDK depends on `Dna.Framework` for IoC/logging — use the existing `IoCContainer` rather than introducing another DI mechanism.
- **`AddInIntegration.SolidWorks` is the static gateway** to the running app; the lifecycle events (`ConnectedToSolidWorks`/`DisconnectedFromSolidWorks`) can fire multiple times across unload/reload — guard one-time setup accordingly.
- **C# follows Luke's house style** (member order, named `#region`/`#endregion`, plain section comments). Mind the macOS case trap: SolidDna folders mix `SolidDna`/`SolidDNA` casing across projects — verify casing with `git ls-files`, never `ls`.
- **Bump `Version`/`AssemblyVersion`** in `AngelSix.SolidDna.csproj` for a NuGet release; keep the VSIX templates in step when the public surface changes.
- Tutorials/templates are teaching copies — fixing a bug in the SDK does not auto-fix the duplicated example code; update the relevant `Tutorials/`/`Templates/` copy if it mirrors the changed API.

## To change X

Before touching a public SDK member, run the live tool to see the blast radius across the solution (templates, tutorials, ScriptRunner all consume the SDK):

```sh
contextmap impact --member "Model.SaveAs" --solution ~/Documents/GitHub/solidworks-api/SolidDna/AngelSix.SolidDna/AngelSix.SolidDna.sln
```

Changing the startup lifecycle → `SolidWorks/Integration/AddIn/AddInIntegration.cs` (impact `AddInIntegration.ConnectToSW`). Changing COM disposal → `SolidWorks/Core/`. A new model/feature capability → the matching `SolidWorks/Models/` subfolder. A new ribbon control → `SolidWorks/CommandManager/`.
