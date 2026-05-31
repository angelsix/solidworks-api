# SolidDna glossary

SolidDna (DNA) is a C# SDK that wraps the SolidWorks COM API, making it easier to build add-ins and plug-ins for SolidWorks. Distributed as the `AngelSix.SolidDna` NuGet package with VSIX project templates.

> **Code age warning:** The current codebase is very old and predates many SolidWorks API changes. Before any coding work continues, a full review against the latest SolidWorks API is required — an upgrade confirmation check to identify deprecated/removed APIs, breaking changes, and compatibility gaps.

## Core SDK

**SolidDna (DNA)**
The SDK/framework name — a C# wrapper around the SolidWorks API that makes it easier, safer, and more modern to develop SolidWorks add-ins. Distributed as the `AngelSix.SolidDna` NuGet package (currently v1.0.1.13).

**SolidWorks**
The CAD software whose COM API is wrapped by SolidDna. The entire domain revolves around programming SolidWorks — parts, assemblies, drawings, features, and the SolidWorks application shell.

**Add-in (Addin)**
A component that loads at the SolidWorks application level via the COM add-in mechanism. An Add-in is the entry point that SolidWorks itself loads; it must be registered with SolidWorks. In SolidDna, Add-ins derive from `AddInIntegration`.

**Plug-in**
A component loaded at the SolidDna level within a running Add-in. Plug-ins are discovered and loaded by the SolidDna framework itself, not by SolidWorks directly. Allows modular, unloadable functionality within an Add-in. Can load in a separate AppDomain for isolation.

**NuGet Package**
The `AngelSix.SolidDna` package distributed via NuGet, containing the SDK for developing SolidWorks add-ins. Targeting .NET Framework 4.7.2.

**VSIX Template**
Visual Studio project templates distributed as a VSIX installer, allowing developers to scaffold new SolidDna add-in projects (Blank Add-In, WPF Add-In, Standalone) directly from Visual Studio.

## Model types

**Part**
A SolidWorks Part document (`.sldprt`). One of the three core model types. Represented by the `Model` class with `ModelType.Part`.

**Assembly**
A SolidWorks Assembly document (`.sldasm`). One of the three core model types. Can contain multiple Component instances. Represented by the `Model` class with `ModelType.Assembly`.

**Drawing**
A SolidWorks Drawing document (`.slddrw`). One of the three core model types. Contains DrawingView, DrawingSheet, Note, Balloon, and other annotation objects. Represented by the `Model` class with `ModelType.Drawing`.

**Model**
The SolidDna abstraction wrapping SolidWorks `ModelDoc2`. Unified representation for any model type (Part, Assembly, Drawing). Provides access to properties, configurations, features, components, and extensions.

**Configuration**
A SolidWorks model configuration — a named variant of a part or assembly with its own settings (suppressed/unsuppressed features, properties, etc.). Multiple configurations can exist per model file.

## Components and features

**Component**
A component instance within an Assembly — a reference to a Part or another Assembly placed in the parent. Supports hierarchy via children relationships.

**Feature**
A SolidWorks feature — a construction element such as an extrude, cut, fillet, or hole wizard. Features form the construction history tree of a part or assembly.

**Feature Data**
The configuration data object for a Feature — the parameters and settings that define a specific feature type (e.g., Hole Wizard parameters, mirror parameters).

## Drawings and annotations

**Taskpane**
A SolidWorks sidebar panel (similar to the default Design Library or PropertyManager panels). SolidDna supports embedding WPF controls into Taskpanes.

**Mass Properties**
The physical properties of a Part — mass, volume, surface area, density, and center of mass coordinates. Computed from the part geometry.

**Note**
A SolidWorks annotation object on a Drawing — text, dimensions, or other labels placed on drawing views.

**Balloon**
A BOM (Bill of Materials) Balloon — a numbered callout on a drawing that references assembly components. Includes upper/lower text content.

**Pack and Go**
A SolidWorks operation to save a document along with all its referenced files (parts, drawings, etc.) into a single folder — used for archiving, manufacturing, or moving assemblies.

## Technical concepts

**AppDomain**
A .NET isolation boundary. SolidDna supports loading Plug-ins in a detached AppDomain so they can be unloaded independently without restarting SolidWorks.

**COM Object**
The underlying SolidWorks COM interop object (e.g., `ModelDoc2`, `Component2`). SolidDna wraps these in `SolidDnaObject<T>` with safe disposal handling to prevent memory leaks.

**ProgId**
A COM programmatic identifier used to create COM-exposed .NET classes. SolidDna Taskpanes create WPF controls with `[ProgId]` attributes so SolidWorks can instantiate them via COM.

**UnsafeObject**
The raw underlying COM object exposed by `SolidDnaObject<T>`. Use with caution — all disposal must be handled manually from this point on.

## Tools and project structure

**ScriptRunner**
A SolidDna project (`SolidDNA.ScriptRunner`) that allows running ad-hoc C# scripts directly inside SolidWorks, useful for quick automation, testing, and experimentation without building a full add-in.

**Templates**
Pre-built project templates (Blank Add-In, WPF Add-In, Standalone) bundled with VSIX installers for Visual Studio. Located in the `Templates/` folder.

**Startup Flow**
The lifecycle of a SolidDna Add-in: `ConfigureServices` → `PreConnectToSolidWorks` → `PreLoadPlugIns` → `ApplicationStartup` → `ConnectedToSolidWorks`. On unload: `DisconnectedFromSolidWorks`.
