# Traffic Education Game – Core System (Unity)

## Overview

This repository contains the core system of a traffic education game developed using Unity.

The main objective of this project is to build a structured and scalable vehicle movement system based on lane logic and road orientation. It is designed as a technical prototype for an educational driving simulation.

---

## Gameplay Preview

A short preview of the tutorial level demonstrating lane-based movement.

<p align="center">
  <img src="https://github.com/user-attachments/assets/41c9a3c7-a01b-4612-a24f-c1f5f0061b36" width="700"/>
</p>

---

## Key Features

### Lane-Based Vehicle Movement
- Forward and backward movement using input  
- Smooth lane switching (left / right)  
- Automatic alignment to road direction  

### Smart Road System
- Lane detection based on world position  
- Road-relative coordinate system (forward & right vectors)  
- Position projection to maintain alignment with road  

### Modular Level Support
- Trigger-based level structure  
- Designed for progressive learning scenarios  

---

## System Overview

The system is built using a modular approach where each component has a clear responsibility:

- **SmartRoad**  
  Acts as the spatial reference of the environment, providing lane data and directional vectors.

- **PlayerMovement**  
  Handles player input, movement behavior, and lane alignment.

- **RoadTrigger**  
  Updates the current road context when entering a new segment.

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

## Design Approach

This system is designed with a focus on separation of concerns:

- Movement logic is handled independently from road data  
- Road logic acts purely as a data provider  
- Triggers are used to dynamically update context  

This approach reduces coupling between systems and simplifies debugging and future expansion.

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

- Intersection handling system (turning logic at crossroads)  
- Interactive UI system (tutorial, checklist, feedback)  

These systems involve:

- Direction-based decision making  
- Lane synchronization across connected roads  
- Context-aware player feedback  

These systems are intentionally excluded to maintain focus on the core movement architecture and to preserve the originality of the full project.

---

## Purpose

This project is developed as part of a final academic project focused on interactive learning in traffic education.

The system is designed to be:

- structured  
- maintainable  
- adaptable for educational gameplay  

---

## Technology

- Unity Engine (2022 or later)  
- C#  

---

## Author

Faris Dzulfiqar
