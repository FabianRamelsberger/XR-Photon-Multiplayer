# Network VR Application Project Overview
## Introduction

This document outlines the development and features of a networked VR application built using Fusion 2 Photon and Unity. The application allows players to interact in a virtual environment where they can manipulate objects and communicate with other players.
### Features

- Player Movement: Players can navigate using a VR rig or traditional mouse and keyboard controls.
        Keyboard controls: WASD for movement, QR for turning, and mouse integration.
-  Lobby System: Players can select their preferred color and join a game lobby via a setup wizard. The chosen color is synchronized across the network.
- Player Interaction: Players can interact with multiple cubes placed in the environment.
        One cube, colored white, does not belong to any player initially. Players can gain ownership of this cube by interacting with it.
-  Players cannot change the ownership of cubes owned by others but can interact with them.
 - When a player leaves the game, their owned cubes are removed except for one, which is transferred to another player.

## Technical Details
### Tools and Resources

- Unity Version: The project utilizes Unity version 2022.3.20f1.
- Photon Fusion: Utilizing Fusion Shared Basics 2.0.0 Build 404 for network management (Fusion Documentation).
- XR Interaction Toolkit: Version 3.0.1 for VR interactions.
- I have used the ParrelSync for testing. You can see this in the toolbar

## Builds

  -  Android: Located in the specified project directory.
-  Desktop: Available in the project directory.

## Starting the Game

 - The game can be launched from the Main.scene file in Unity.
## Configuration
  - To modify settings, navigate to Hierarchy/Gameplay and change the mode via the UI settings in Unity.

## Implementation
### Managers
  - ConnectionManager: Manages all network connections and inherits most of the event functions. It also initializes the network runner.
  - PlayerManager: Central to game functionality, this manager handles player colors, spawn points, and cube references. Ensures that player prefabs and cubes are initialized correctly using Runner.WaitForSingleton<PlayerManagerScript>.
    - UIManager: Currently a simple implementation but can be expanded. Manages user interface elements.

### Project Structure
   -  Standard Component-based: Due to time constraints, this structure was chosen to organize scripts, materials, and models in general directories.
   -  Feature-based: Contains folders for scripts, models, and materials specific to each game feature. This is 
### Coding Standards
    - Followed standard C# and Unity naming conventions as recommended in Unity's C# Scripting Standards.
https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity

## Future Enhancements
### Scalability

- More Players: The system is currently tested with two clients but designed to easily include more players by adding spawn points and materials to the PlayerManager.
- Dynamic Player Left Behinds: Adapt the player leave-behind logic to accommodate more than two players by modifying how the local ID is utilized.

### Gameplay
- Event System: Integrate a more robust event system to manage UI and network events more reliably and intuitively.
- New Interactions: Introduce new interactive elements such as a game where players throw objects into a basket, or a snowball fight.
