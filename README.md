# Traffic Education Game – Core System (Unity)

## Overview

This repository contains the core system of a traffic education game developed using Unity.

The focus of this project is to design a structured and scalable vehicle movement system based on road lanes and directional logic. It is intended as a technical prototype for an educational simulation game.

---

## Key Features

### Lane-Based Vehicle Movement

* Forward and backward movement using input
* Smooth lane switching (left / right)
* Automatic alignment to road structure

### Smart Road System

* Lane detection based on world position
* Road-relative coordinate system (forward & right vectors)
* Position projection to maintain alignment with road direction

### Modular Level Support

* Simple trigger-based level structure
* Designed to support progressive learning scenarios

---

## System Architecture (Simplified)

The system is built around three main components:

* **SmartRoad**
  Provides lane data, directional vectors, and spatial calculations.

* **PlayerMovement**
  Handles input, movement logic, and lane transitions.

* **RoadTrigger**
  Assigns the current road context to the player.

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

The goal is to create a system that is:

* structured
* maintainable
* suitable for educational gameplay

---

## Technology

* Unity Engine (2022 or later)
* C#

---

## Author

Faris Dzulfiqar
