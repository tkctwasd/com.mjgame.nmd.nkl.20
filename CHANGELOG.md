# Changelog

All notable changes to this package will be documented in this file.

---

## [2.0.2] - 2026-10-11

### Fixed

* FSM: transitions now fire automatically when `OnCheck` returns true (as documented). Previously `ShouldBegin()` / `TransitionCallback()` were never evaluated.
* FSM: `DefaultState.OnEnter` is now called on the first update (any of Update / FixedUpdate / LateUpdate). Previously the default state was assigned without `OnEnter` and the first frame was skipped.
* FSM: `ToState<T>()` logs a warning when the state is not registered instead of failing silently.
* FSM: `AIBehaviour` no longer throws `NullReferenceException` when `Init()` did not create `StateMachine`; it logs an error instead.

### Added

* FSM: Any State transitions (`from = null`, `AIBehaviour.AddAnyTransition(to, check)`).
* FSM: `Transition(from, to, check)` constructor and `AIBehaviour.AddTransition(from, to, check)`.
* FSM: `StateMachine.AddState`, `GetState<T>()`, `IsInState<T>()`, public `ToState(IState)`, `Start()`, event `OnStateChanged`. `DefaultState` setter registers the state.

### Documentation

* `README.md`: FSM section updated with automatic / Any State transitions and evaluation order.

---

## [2.0.1] - 2026-10-03

### Changed

* Ads, IAP, Firebase and Remote Config now live in the separate package `com.mjgame.nmd.nkl.20.monetization` (depends on Core `2.0.x`).
* `DataMonetization` is no longer listed as Core save data. It belongs to Monetization and is stored by the game `DataSave`, exposed through `DataMonetization.Current`.

### Documentation

* `README.md`: added the Monetization package reference, how package data is stored through the game `DataSave`, and that `DataSave` must exist at runtime.
* `README.md`: added troubleshooting for missing PlayerPrefs keys.

---

# [2.0.0] - 2026-09-23

> [!IMPORTANT]
>
> ## MJGame Core 2.0
>
> Core `2.x` is a **new generation** of the MJGame framework.
>
> The framework has been **rebuilt from the ground up** with a new architecture, coding structure, technology stack, and development workflow.
>
> Core `2.x` **replaces the legacy Core `1.x` architecture**.

> [!WARNING]
>
> ## Breaking Changes
>
> Core `2.x` is **not backward compatible** with Core `1.x`.
>
> Existing projects using Core `1.x` may require migration or system rewrites.
>
> Legacy APIs, components, and implementation patterns are not guaranteed to work with Core `2.x`.

###  New Generation

* Completely rebuilt the framework with a new core architecture.
* Replaced the legacy `1.x` core implementation.
* Introduced a new coding architecture and development workflow.
* Redesigned the framework for modern Unity development.
* Standardized coding conventions and naming conventions.
* Redesigned core systems with modularity and reusability as primary goals.
* Separated core framework systems from game-specific implementations.
* Improved portability for reuse across multiple Unity projects.

### Rebuilt Systems

* `UI` - New UI architecture based on `UIPanel`.
* `Pool` - New object pooling architecture.
* `StateMachine` - New FSM architecture based on `IState`.
* `Save` - New JSON-based data and save architecture.
* `Audio` - Rebuilt audio management system.
* `Camera` - New camera architecture and utilities.
* `Joystick` - New mobile input system.
* `Indicator` - New world and UI indicator architecture.
* `Trigger` - New trigger and interaction utilities.
* `Extensions` - Reorganized and expanded shared extension methods.
* `Editor` - New editor and development tools.

###  Architecture

* Introduced a modular core structure.
* Introduced standardized base classes and interfaces.
* Redesigned system initialization and lifecycle.
* Reduced dependencies between core modules.
* Removed game-specific logic from the core framework.
* Reorganized `Runtime` and `Editor` assemblies.
* Standardized package structure.
* Improved extensibility for project-specific systems.

### Technology

* Migrated to the new Unity package architecture.
* Updated assembly definition structure.
* Standardized `uGUI` and `TextMeshPro` integration.
* Standardized `DOTween` integration.
* Standardized JSON serialization using `Newtonsoft Json`.
* Updated `Cinemachine` integration.
* Updated package dependencies for the new core architecture.

###  Dependencies

* `Unity UI (uGUI)` - 1.0.0
* `TextMeshPro` - 3.0.7
* `Cinemachine` - 2.10.7
* `Newtonsoft Json` - 3.2.2

> [!NOTE]
> **Core 2.0 is a fresh foundation.**
>
> Version `2.0.0` is not an incremental update of Core `1.x`.
> It represents a complete transition to the next generation of the MJGame framework.

---

## [1.0.16] - 2024-01-10

### Added

* `FPS`
* `ButtonExtension`

### Fixed

* `StateMachine`

---

## [1.0.15] - 2024-01-04

### Added

* `API`

---

## [1.0.14] - 2024-01-04

### Fixed

* Build errors related to the `Editor` folder.

---

## [1.0.13] - 2024-12-28

### Updated

* `DataGameProfile`

---

## [1.0.12] - 2024-12-27

### Fixed

* `UIManager`

---

## [1.0.11] - 2024-12-23

### Fixed

* Folder structure.
* `LogExtension`

---

## [1.0.10] - 2024-12-13

### Fixed

* `Audio`
* `UI`
* `Samples~`

---

## [1.0.9] - 2024-12-11

### Fixed

* `UI`

---

## [1.0.8] - 2024-12-09

### Added

* `Audio`

### Fixed

* `Editor Window`
* `Samples~`

---

## [1.0.7] - 2024-12-08

### Fixed

* Assembly definitions.

---

## [1.0.6] - 2024-12-07

### Added

* `CHANGELOG.md`
* `LICENSE`

### Fixed

* `README.md`
* `StateMachine`
* `Pool`
* Renamed `ScriptTracker.cs` to `TrackerAttribute.cs`

---

## Legacy

Core `1.x` represents the original generation of the MJGame framework.

Core `2.x` introduces a completely new architecture, coding structure, technology stack, development workflow, and implementation of the framework's primary systems.
