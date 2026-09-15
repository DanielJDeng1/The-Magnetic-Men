# The Magnetic Men

A two-player 2D puzzle-platformer built in Unity around magnetic attraction and repulsion.

The project placed **4th nationally** in the 2026 TSA Video Game Design competition.

## Gameplay

Each player has a magnetic polarity and can toggle their magnetic field. Nearby magnetic objects react based on polarity:

- opposite poles attract
- matching poles repel
- static magnetic objects can pull or push the player
- movable magnetic objects can be pushed, pulled, or attached to a player

The levels combine the magnetic system with platforming, checkpoints, buttons, levers, moving objects, and other physics-based obstacles.

The build contains a main menu and three enabled levels.

## Code

- `Assets/Scripts/PlayerController.cs` — platforming movement and player magnetism
- `Assets/Scripts/MagneticObject.cs` — polarity and magnetic-object behavior
- `Assets/Scripts/MagnetismRadius.cs` — tracks magnetic objects within range
- `Assets/Scripts/Checkpoint.cs` — two-player checkpoint handling
- `Assets/Scripts/Interactibles/` — buttons, levers, doors, and related puzzle objects
- `Assets/Scripts/PArkour/` — level movement and platforming mechanics

## Running the Project

The repository uses Unity `2022.3.9f1`.

Clone the repository and open it through Unity Hub:

```bash
git clone https://github.com/DanielJDeng1/The-Magnetic-Men.git
```

Start from `Assets/Scenes/Main Menu.unity`, or open one of the level scenes directly:

```text
Assets/Scenes/Level 1.unity
Assets/Scenes/Level 2.unity
Assets/Scenes/Level 3.unity
```
