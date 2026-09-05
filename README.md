# VR Career Guidance

[![Engine: Unity](https://img.shields.io/badge/Unity-6000.0.30f1-100000?style=flat&logo=unity&logoColor=white)](#)
[![Platform: XR](https://img.shields.io/badge/Platform-XR%2FVR-blue.svg)](#)

> **Executive Summary:** A VR career-exploration simulation built in Unity, letting users step into and experience a range of professions — firefighter, mechanic, doctor, lawyer, teacher, and software developer — through interactive scenes, followed by a knowledge-check quiz. Built for the CMU601 project.

## Career Scenarios

Each scenario is its own Unity scene under `UnityProject/Assets/Scenes`:

- **FireFighter** — firefighting simulation using the Vefects VFX pack
- **Mechanic** — garage/vehicle repair simulation
- **Doctor** — hospital environment simulation
- **Lawyer**, **Teacher**, **SoftwareDeveloper** — office-based career scenarios
- **QuizScene** / **TutorialScene** — onboarding and knowledge-check flow

## Tech Stack

- **Engine:** Unity 6 (6000.0.30f1)
- **XR:** Unity XR Interaction Toolkit, XR Hands
- **Environments/Assets:** Third-party Unity Asset Store packs (PolygonalMind LowPolyOffice, Free Fire VFX URP, garage/hospital environment sets)

## Repository Structure

- `UnityProject/` — the Unity project source (scenes, scripts, prefabs, settings)
- `docs/Recordings/` — full gameplay walkthrough recordings
- `docs/Reports/VR_Game.mp4` — condensed gameplay demo
- `docs/Presentation/`, `docs/ProgressReports/` — course presentation and progress report slide decks

Large binaries (recordings, 3D models, textures, office documents) are tracked via **Git LFS** — run `git lfs install` before cloning to pull them correctly.

## Setup

1. Install [Git LFS](https://git-lfs.com/) and run `git lfs install`.
2. Clone the repo and open `UnityProject/` in Unity 6000.0.30f1 (or later).
3. Open any scene under `Assets/Scenes` and enter Play mode with a connected XR headset (or the XR Device Simulator).
