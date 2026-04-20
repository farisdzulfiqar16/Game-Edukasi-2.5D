# Traffic Education Game – Core System (Unity)

## Overview

This repository contains the core system of a traffic education game developed using Unity.

The main objective of this project is to build a structured and scalable vehicle movement system based on lane logic and road orientation. It is designed as a technical prototype for an educational driving simulation.

---

## Key Features

### Lane-Based Vehicle Movement

* Forward and backward movement using input
* Smooth lane switching (left / right)
* Automatic alignment to road direction

### Smart Road System

* Lane detection based on world position
* Road-relative coordinate system (forward & right vectors)
* Position projection to maintain alignment with road

### Modular Level Support

* Trigger-based level structure
* Designed for progressive learning scenarios

---

## System Architecture

The system is built around three core components:

* **SmartRoad**
  Provides lane data, directional vectors, and spatial calculations.

* **PlayerMovement**
  Handles player input, movement logic, and lane transitions.

* **RoadTrigger**
  Updates the current road context when entering a new segment.

---

## System Overview

The system follows a modular approach where each component has a clear responsibility:

* **SmartRoad** acts as the spatial reference of the environment
* **PlayerMovement** uses SmartRoad data to control movement and alignment
* **RoadTrigger** ensures the player always interacts with the correct road

This separation improves maintainability and allows future expansion such as intersections and traffic systems.

---

## System Flow (Simplified)

```
Player Input (W / A / S / D)
            │
            ▼
    PlayerMovement
            │
            ▼
      SmartRoad
 (lane data & direction)
            │
            ▼
   Final World Position
            │
            ▼
     Rigidbody Movement
```

---

## Project Structure

```
Assets/
└── Scripts/
    ├── Core/
    │   ├── SmartRoad.cs
    │   ├── PlayerMovement.cs
    │   └── RoadTrigger.cs
    │
    ├── Level/
    │   ├── LevelManager.cs
    │   └── Trigger_Finish.cs
    │
    └── Camera/
        └── Camera.cs
```

---

## Notes on Scope

This repository only includes the **core gameplay system**.

The following systems are intentionally excluded to preserve project originality:

* Intersection handling system (turning logic at crossroads)
* Interactive UI system (tutorial, checklist, feedback)

These systems involve:

* Direction-based decision making
* Lane synchronization across connected roads
* Context-aware player feedback

---

## Purpose

This project is developed as part of a final academic project focused on interactive learning in traffic education.

The system is designed to be:

* structured
* maintainable
* adaptable for educational gameplay

---

## Technology

* Unity Engine (2022 or later)
* C#

---

## Author

Faris Dzulfiqar
