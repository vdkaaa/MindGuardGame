# MindGuard Project - Architecture Study Guide

**Project:** MindGuard (Tower Defense / Roguelike Defense)  
**Engine:** Unity 6 LTS  
**Language:** C#  
**Architecture Pattern:** MVP (Model-View-Presenter) with Service Locator & Event-Driven Architecture

---

## 📋 Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Core Systems](#core-systems)
3. [Domain Layer](#domain-layer)
4. [Bootstrap Layer](#bootstrap-layer)
5. [Data Layer](#data-layer)
6. [Project Structure](#project-structure)
7. [Design Principles](#design-principles)
8. [Progress Timeline](#progress-timeline)

---

## 🏗️ Architecture Overview

MindGuard uses a **layered architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    BOOTSTRAP LAYER                          │
│  (MindGuard.Bootstrap)                                      │
│  - GameBootstrapper: Initializes all services              │
│  - GameLoopRunner: Bridges domain to Unity lifecycle       │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                             │
│  (MindGuard.Domain)                                         │
│  - GameLoopController: Orchestrates game states            │
│  - States: Day, Night, Transition (IState<T>)             │
│  - Events: DayStarted, NightStarted, GameOver             │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    CORE LAYER                               │
│  (MindGuard.Core)                                           │
│  - EventBus: Pub/Sub messaging system                      │
│  - ServiceLocator: Dependency injection container         │
│  - StateMachine<T>: Generic state machine                 │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    DATA LAYER                               │
│  (MindGuard.Data)                                           │
│  - TowerDataSO: Tower configuration ScriptableObject      │
│  - EnemyDataSO: Enemy configuration ScriptableObject      │
└─────────────────────────────────────────────────────────────┘
```

---

## 🎯 Core Systems

### 1. EventBus (Pub/Sub Messaging)

**File:** `Assets/Core/EventBus/`  
**Key Classes:**
- `IEventBus`: Interface defining pub/sub contract
- `EventBus`: Implementation using `Dictionary<Type, Delegate>`

**Purpose:** Loose coupling between systems through event publishing

**Key Methods:**
```csharp
void Subscribe<T>(Action<T> handler) where T : struct
void Unsubscribe<T>(Action<T> handler) where T : struct
void Publish<T>(T eventData) where T : struct
```

**Design Decisions:**
- Generic with `struct` constraint for events (value types, zero allocation)
- Uses `Delegate.Combine()` and `Delegate.Remove()` internally
- No exception if no subscribers (safe fire-and-forget)

**Example Usage:**
```csharp
var eventBus = new EventBus();
eventBus.Subscribe<DayStartedEvent>(OnDayStarted);
eventBus.Publish(new DayStartedEvent { DayNumber = 1 });
```

---

### 2. ServiceLocator (Dependency Injection)

**File:** `Assets/Core/ServiceLocator/`  
**Key Classes:**
- `ServiceLocator`: Simple DI container with `Dictionary<Type, object>`

**Purpose:** Central registry for game services, accessed globally via `GameBootstrapper.Services`

**Key Methods:**
```csharp
void Register<T>(T instance)
T Get<T>() // throws InvalidOperationException if not found
bool HasService<T>()
```

**Design Decisions:**
- Type-safe generics
- Descriptive error messages including type name
- No static instances of services (only bootstrapper is static)

**Example Usage:**
```csharp
var services = GameBootstrapper.Services;
var eventBus = services.Get<IEventBus>();
```

---

### 3. StateMachine<T> (Generic State Management)

**Files:** 
- `Assets/Core/StateMachine/IState.cs`
- `Assets/Core/StateMachine/StateMachine.cs`

**Key Classes:**
- `IState<TOwner>`: Interface for states
- `StateMachine<TOwner>`: Manages state transitions

**Purpose:** Flexible, reusable state machine for any game entity

**Key Methods:**
```csharp
void ChangeState(IState<TOwner> newState)
  // Calls Exit on old state, Enter on new state
void Update()
  // Calls Update on current state
IState<TOwner> CurrentState { get; }
```

**State Lifecycle:**
```
Enter() → [Update() every frame] → Exit() → [New State Enter()]
```

**Design Decisions:**
- Fully generic (no Unity dependencies)
- Owner passed to all state methods for context
- Null-safe transitions

**Example:**
```csharp
var stateMachine = new StateMachine<GameLoopController>(owner);
stateMachine.ChangeState(new DayState(eventBus));
// Later...
stateMachine.Update(); // Calls DayState.Update()
```

---

## 🎮 Domain Layer

### 4. Game Loop States

**Files:** `Assets/Domain/Gameplay/States/`

#### DayState
```csharp
Enter: Publishes DayStartedEvent
Update: [TODO] - Day phase logic
Exit: Logs "Day ended"
```

#### NightState
```csharp
Enter: Publishes NightStartedEvent
Update: [TODO] - Night phase logic
Exit: Logs "Night ended"
```

#### TransitionState
```csharp
Enter: Publishes TransitionStartedEvent
Update: [TODO] - Transition animations/logic
Exit: Empty
```

**All states:**
- Implement `IState<GameLoopController>`
- Receive `IEventBus` via constructor (dependency injection)
- Publish domain events when entering

---

### 5. Game Loop Events

**File:** `Assets/Domain/Gameplay/Events/GameLoopEvents.cs`

All events are **mutable structs** (value types, zero allocation):

```csharp
public struct DayStartedEvent
{
    public int DayNumber { get; set; }
}

public struct NightStartedEvent
{
    public int NightNumber { get; set; }
}

public struct TransitionStartedEvent { }

public struct GameOverEvent
{
    public bool Victory { get; set; }
}
```

**Why structs?**
- Zero heap allocation
- Events are immutable data
- Perfect for event systems

---

### 6. GameLoopController

**File:** `Assets/Domain/Gameplay/GameLoopController.cs`

**Purpose:** Orchestrates the entire game loop using StateMachine

**Key Fields:**
```csharp
private StateMachine<GameLoopController> _stateMachine
private int _currentDay
private int _currentNight
private bool _isRunning
```

**Key Methods:**
```csharp
void StartRun()
  // Initialize StateMachine, start with DayState

void Update()
  // Update state machine if running

void GoToNight()
  // _currentNight++, change to NightState

void GoToDay()
  // _currentDay++, change to DayState

void EndRun(bool victory)
  // Stop loop, publish GameOverEvent
```

**Properties (Read-Only):**
```csharp
int CurrentDay { get; }
int CurrentNight { get; }
bool IsRunning { get; }
```

**No MonoBehaviour** - Pure C# game logic

---

## 🚀 Bootstrap Layer

### 7. GameBootstrapper

**File:** `Assets/_Project/Bootstrap/GameBootstrapper.cs`

**Purpose:** Initialize all services on game start

**Key Responsibilities:**
1. Create `EventBus` instance
2. Create `ServiceLocator` instance
3. Register `EventBus` in `ServiceLocator` with key `IEventBus`
4. Mark game object as `DontDestroyOnLoad`
5. Expose `static ServiceLocator Services` property

**Execution Order:** `[DefaultExecutionOrder(-100)]`
- Runs before any other script
- Ensures services are ready for all other systems

**Example:**
```csharp
public static ServiceLocator Services { get; private set; }

private void Awake()
{
    var eventBus = new EventBus();
    var serviceLocator = new ServiceLocator();
    serviceLocator.Register<IEventBus>(eventBus);
    Services = serviceLocator;
    DontDestroyOnLoad(gameObject);
    Debug.Log("MindGuard Bootstrap OK");
}
```

---

### 8. GameLoopRunner

**File:** `Assets/_Project/Bootstrap/GameLoopRunner.cs`

**Purpose:** Bridge between Unity lifecycle and pure C# game logic

**Key Responsibilities:**
1. Get `IEventBus` from `GameBootstrapper.Services`
2. Create `GameLoopController` instance
3. Call `_controller.StartRun()` in `Start()`
4. Call `_controller.Update()` in `Update()`
5. Provide context menu commands for testing

**Context Menu Commands:**
- `[ContextMenu("Go To Night")]` → Test night transition
- `[ContextMenu("Go To Day")]` → Test day transition

**Design:** Minimal MonoBehaviour - only bridges Unity to domain

---

## 📊 Data Layer

### 9. ScriptableObject Configurations

#### TowerDataSO
```csharp
// Assets/Data/Towers/TowerDataSO.cs
public class TowerDataSO : ScriptableObject
{
    public string TowerName { get; }
    public float Cost { get; }              // Inspiration required
    public float MaxHP { get; }
    public float Range { get; }
    public float FireRate { get; }
    public float DamagePerShot { get; }
    public GameObject Prefab { get; }
}
```

**Menu Path:** `MindGuard/Data/Torre`

#### EnemyDataSO
```csharp
// Assets/Data/Enemies/EnemyDataSO.cs
public class EnemyDataSO : ScriptableObject
{
    public string EnemyName { get; }
    public float MaxHP { get; }
    public float MoveSpeed { get; }
    public float DamagePerSecond { get; }
    public float InspirationReward { get; }  // On death
    public int SpawnWeight { get; }         // 1-10 weight
}
```

**Menu Path:** `MindGuard/Data/Enemigo`

---

## 📁 Project Structure

```
Assets/
├── Core/                          # Reusable systems
│   ├── Core.asmdef
│   ├── EventBus/
│   │   ├── IEventBus.cs
│   │   └── EventBus.cs
│   ├── ServiceLocator/
│   │   └── ServiceLocator.cs
│   └── StateMachine/
│       ├── IState.cs
│       └── StateMachine.cs
│
├── Domain/                        # Game-specific logic
│   ├── Domain.asmdef
│   └── Gameplay/
│       ├── GameLoopController.cs
│       ├── Events/
│       │   └── GameLoopEvents.cs
│       └── States/
│           ├── DayState.cs
│           ├── NightState.cs
│           └── TransitionState.cs
│
├── Data/                          # Configuration data
│   ├── Towers/
│   │   └── TowerDataSO.cs
│   └── Enemies/
│       └── EnemyDataSO.cs
│
├── _Project/                      # Project-specific code
│   ├── Project.asmdef
│   └── Bootstrap/
│       ├── GameBootstrapper.cs
│       └── GameLoopRunner.cs
│
└── Tests/
    ├── Tests.asmdef
    └── EditMode/
        ├── EventBusTests.cs
        ├── ServiceLocatorTests.cs
        ├── StateMachineTests.cs
        └── GameLoopControllerTests.cs
```

---

## 🎨 Design Principles

### 1. **Separation of Concerns**
- **Core:** Reusable infrastructure
- **Domain:** Game-specific logic
- **Bootstrap:** Service initialization
- **Data:** Configuration only

### 2. **Dependency Injection**
- No `static` instances (except `GameBootstrapper.Services`)
- Dependencies passed via constructors
- `ServiceLocator` provides access to shared services

### 3. **Event-Driven Architecture**
- Systems communicate via events, not direct references
- `EventBus` enables loose coupling
- Easy to add new listeners without modifying existing code

### 4. **Generic & Flexible**
- `StateMachine<T>` works with any owner type
- `IState<T>` defines contract for any domain object
- `EventBus` handles any `struct` event

### 5. **No MonoBehaviour Contamination**
- Domain logic is pure C#
- Only `GameBootstrapper` and `GameLoopRunner` are MonoBehaviours
- Maximum reusability and testability

### 6. **Type Safety**
- Generic constraints (`where T : struct`)
- Compile-time checking via C# type system
- Descriptive error messages at runtime

---

## 📈 Progress Timeline

### **Day 1: Foundation Systems**

#### Session 1: EventBus System
- ✅ Created `IEventBus` interface
- ✅ Implemented `EventBus` with `Dictionary<Type, Delegate>`
- ✅ Wrote 3 NUnit tests
- ✅ Created `Tests.asmdef` for proper assembly organization

#### Session 2: ServiceLocator System
- ✅ Created `ServiceLocator` with generics
- ✅ Implemented `Register<T>`, `Get<T>`, `HasService<T>`
- ✅ Added 4 comprehensive tests (with bonus)
- ✅ Fixed `init` property compatibility (changed to `private set`)

#### Session 3: StateMachine System
- ✅ Created generic `IState<TOwner>` interface
- ✅ Implemented `StateMachine<TOwner>` with state lifecycle
- ✅ Wrote 4 NUnit tests with mock states
- ✅ All systems working without errors

#### Session 4: GameBootstrapper
- ✅ Created `GameBootstrapper` MonoBehaviour
- ✅ Initialized EventBus and ServiceLocator
- ✅ Exposed static `Services` property
- ✅ Set `[DefaultExecutionOrder(-100)]` for first execution

---

### **Day 2: Domain & Integration**

#### Session 1: ScriptableObject Configurations
- ✅ Created `TowerDataSO` with tower parameters
- ✅ Created `EnemyDataSO` with enemy parameters
- ✅ Both with `[CreateAssetMenu]` for editor UI

#### Session 2: Game Loop States Structure
- ✅ Created `DayState`, `NightState`, `TransitionState`
- ✅ All implement `IState<GameLoopController>`
- ✅ Created `GameLoopEvents` (4 event structs)
- ✅ Fixed property initialization issues (init → set)
- ✅ Created `Domain.asmdef` for namespace isolation

#### Session 3: GameLoopController
- ✅ Implemented `GameLoopController` orchestrator
- ✅ Wires StateMachine to game loop lifecycle
- ✅ Manages day/night transitions
- ✅ Publishes `GameOverEvent` on game end
- ✅ Wrote 5 comprehensive tests
- ✅ Created `Project.asmdef` for _Project namespace

#### Session 4: Bootstrap Bridge (GameLoopRunner)
- ✅ Created `GameLoopRunner` MonoBehaviour bridge
- ✅ Integrates with `GameBootstrapper.Services`
- ✅ Added context menu commands for editor testing
- ✅ All errors resolved, compilation successful

---

## 🔄 How Components Interact

### Game Startup Sequence:
1. **GameBootstrapper** (Awake) runs first (`DefaultExecutionOrder(-100)`)
   - Creates `EventBus` and `ServiceLocator`
   - Registers `EventBus` in services
   - Stores in static `Services` property
   - Game object marked `DontDestroyOnLoad`

2. **GameLoopRunner** (Start) runs after bootstrap
   - Gets `IEventBus` from `GameBootstrapper.Services`
   - Creates `GameLoopController` instance
   - Calls `controller.StartRun()`

3. **GameLoopController.StartRun()** initializes state machine
   - Creates `StateMachine<GameLoopController>`
   - Changes to `DayState`

4. **DayState.Enter()** publishes event
   - Publishes `DayStartedEvent` via EventBus
   - Any system subscribed receives the event

5. **Every Frame:**
   - `GameLoopRunner.Update()` calls `_controller.Update()`
   - `GameLoopController.Update()` calls `_stateMachine.Update()`
   - Current state's `Update()` is called

6. **State Transition (via context menu or game logic):**
   - `GameLoopRunner.GoToNight()` calls `_controller.GoToNight()`
   - `GameLoopController.GoToNight()`:
     - Increments `_currentNight`
     - Calls `_stateMachine.ChangeState(new NightState(...))`
   - StateMachine:
     - Calls `DayState.Exit()`
     - Sets `_currentState = NightState`
     - Calls `NightState.Enter()`
   - `NightState.Enter()` publishes `NightStartedEvent`

---

## 🧪 Testing Strategy

All core systems have **NUnit tests** in `Assets/Tests/EditMode/`:

| System | Test File | Tests |
|--------|-----------|-------|
| EventBus | EventBusTests.cs | 3 tests |
| ServiceLocator | ServiceLocatorTests.cs | 4 tests |
| StateMachine | StateMachineTests.cs | 4 tests |
| GameLoopController | GameLoopControllerTests.cs | 5 tests |

**Total: 16 tests** covering core functionality

**Run tests in Unity or via terminal:**
```bash
# Unity Test Framework runs automatically in Edit Mode
```

---

## 💡 Key Takeaways

1. **Layered Architecture** provides clear separation and testability
2. **Generic Systems** (StateMachine, Services) are reusable across projects
3. **Event Bus** enables loose coupling between systems
4. **No Static Logic** (except bootstrap entry point) makes code flexible
5. **Pure C# Domain** can be tested/debugged independent of Unity
6. **Scalable Structure** ready for economy, spawning, towers, etc.

---

## 🚀 Next Steps (Future Sessions)

Planned systems to build:
- **Economy System** (gold/inspiration management)
- **Tower System** (placement, targeting, firing)
- **Enemy Spawner** (wave management, weighted spawning)
- **Combat System** (damage calculations, effects)
- **UI System** (HUD, menus, state visualization)
- **Audio System** (SFX, music, event triggers)
- **Save/Load System** (persistence, progression)

---

**This architecture can scale to a complete game while maintaining clean code, testability, and separation of concerns.**

Last Updated: March 22, 2026  
Foundation Phase: ✅ Complete  
Integration Phase: ✅ Complete
