# Clash Royale-like Chest System Game

## Overview

This game implements a core Chest System inspired by popular mobile games like Clash Royale. Players can manage multiple chest slots, generate random chests, and strategically decide how to unlock them—either by waiting for a timer or instantly using premium currency (gems). The system focuses on core gameplay mechanics related to chest management, resource acquisition, and decision-making for progression.

This project was developed as a game design exercise with a strong emphasis on robust architecture, modularity, scalability, and the application of various design patterns and programming principles.

## Gameplay Features

The game features the following core mechanics:

* **Currencies:**
    * **Coins:** Acquired from chests.
    * **Gems:** Premium currency, used for instant unlocks and other actions.
* **Chest Slots:** A scrollable, dynamic list of chest slots, with a minimum of 4 available slots for placing chests.
* **Random Chest Generation:** A button allows players to generate random chests into any empty slot. Different chest types have varying chances of appearing.
* **Chest States & Interactions:** Chests can exist in several states, each with specific interactions:
    * **Locked:** The chest is in a slot, but its unlock timer has not started. Clicking a locked chest reveals a pop-up.
    * **Unlocking:** The chest's timer is actively running. Only one chest can be actively unlocking at a time.
    * **Unlocked (but not collected):** The timer has finished. Players can tap to collect rewards.
    * **Collected:** Rewards have been claimed, and the chest leaves its slot.
* **Chest Unlock Options:**
    * **Start Timer:** Begins the respective unlock timer for the chest. No cost is involved.
    * **Unlock with Gems:** Instantly unlocks the chest by spending gems.
        * **Cost Calculation:** 1 Gem for every 10 minutes of remaining unlock time. The cost dynamically reduces as the timer decreases.
        * **Rounding:** All gem costs are calculated using `Math.Ceiling` (e.g., 11 minutes remaining = 2 gems, 29 minutes remaining = 3 gems).
        * **Insufficient Gems:** A pop-up will appear if the player doesn't have enough gems.
* **Undo Option:** Players can undo the action of spending gems to unlock a chest, restoring the spent gems and reverting the chest to its locked state.
* **Chest Opening Queue:** Chests can be added to a queue. Once the currently unlocking chest finishes, the next chest in the queue will automatically begin unlocking.
* **Dynamic Values:** All numerical values related to chests (timers, chances, rewards, gem costs) are flexible and configurable by designers, not hard-coded.

## Gameplay Video

[Game-Play](https://drive.google.com/file/d/1k97SxgOwNj8NZGO6YH50f34IueceS8nk/view?usp=sharing)

## Architecture

The game's architecture is structured using a modified Model-View-Controller (MVC) pattern, adapted for the Unity environment. This approach promotes separation of concerns, making the system modular, testable, and scalable.

![mermaid](https://github.com/user-attachments/assets/6f2ea884-38da-4692-b8a2-f8664e5ba92e)

**Key Components and Their Roles:**

* **Models** (e.g., `ChestModel`, `ResourceModel`): Handle data, business logic, and state management. They are independent of the UI.
* **Views** (e.g., `ChestView`, `ResourceView`, `ChestSlotUIView`): Responsible for displaying information to the user and capturing input. They are generally "dumb" and react to changes initiated by controllers or events.
* **Controllers** (e.g., `ChestController`, `ResourceController`, `ChestSlotUIController`): Act as intermediaries between Models and Views. They process input, update models, and instruct views to update.
* **Services** (e.g., `GameService`, `EventService`, `UIService`, `ChestService`, `ResourceService`): Provide centralized functionalities and manage global systems, promoting loose coupling between different parts of the application.

## Design Patterns and Principles

The following design patterns and programming principles were applied throughout the development:

* **MVC (Modified):** The core architectural pattern, seen in pairs like `ChestModel`, `ChestView`, and `ChestController`. This ensures clean separation of data, logic, and presentation.
* **Singleton:**
    * The `GameService` acts as a central access point for all other services (`EventService`, `UIService`, `ChestService`, `ResourceService`), ensuring a single instance of each vital service throughout the game.
    * `EventService` itself is a Singleton, providing a global pub-sub mechanism.
* **Observer Pattern (via Event Service):**
    * The `EventService` facilitates communication between decoupled components. For instance, `ResourceView` or UI elements subscribe to `OnGemsCountChanged` or `OnCoinsCountChanged` events to update display automatically when resources change. `ChestController` listens for `OnChestUnlocked` and `OnChestCollected` events.
* **Command Pattern:**
    * Used for actions that can be undone, specifically for unlocking chests with gems. `ICommand` defines `Execute()` and `Undo()` methods.
    * `ChestOpenWithGems` is a concrete command.
    * `CommandInvoker` manages a history of commands, enabling the undo functionality for specific chest actions.
* **State Machine Pattern:**
    * The `ChestStateMachine` (using `IState` interface and concrete states like `LockedState`, `UnlockingState`, `UnlockedState`, `CollectedState`, `QueuedState`) manages the lifecycle and behavior of individual chests. This keeps chest logic clean and separated by state.
* **Object Pool Pattern:**
    * Implicitly used (or intended for future scaling) for `ChestController` and `ChestView` instances, managed by a `ChestService`, to efficiently reuse objects and reduce garbage collection overhead during chest generation and removal.
* **SOLID Principles:**
    * **Single Responsibility Principle (SRP):** Classes are designed to have one clear purpose. `ChestModel` manages chest data, `ChestView` handles chest visuals, `ChestController` orchestrates chest behavior, and `ResourceController` manages currency.
    * **Open/Closed Principle (OCP):** The event system and state machine pattern exemplify OCP. New event types or chest states can be added without modifying existing core classes. New chest types can be introduced by simply creating new `ChestSO` Scriptable Objects.
    * **Liskov Substitution Principle (LSP):** The use of the `IState` interface ensures that any derived state class can be used interchangeably with its base type, maintaining behavioral consistency.
    * **Dependency Inversion Principle (DIP):** The `GameService` acts as a dependency injector, providing services to controllers. The `EventService` also decouples components by allowing them to communicate via abstractions (events) rather than direct references.
    * **Interface Segregation Principle (ISP):** Evident in the `ICommand` and `IState` interfaces, which define small, specific contracts for their respective behaviors.
      
## How to Run

1.  Clone this repository.
2.  Open the project in Unity (Version 2022.3.x or newer recommended).
3.  Navigate to `Assets/Scenes/MainGame.unity` and open it.
4.  Press the Play button in the Unity Editor.

## Contributing

This project was developed as a specific assignment. If you have suggestions or find issues, please feel free to open an issue in the repository.
