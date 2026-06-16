# GTASA Simulation

 ---
[DOCUMENTATION - CLICK HERE](https://hitm3n1.github.io/GTASA_DOC_2/) 
 ---

## Project Description

GTASA Simulation is a simple 2D city simulation written in C# using the MonoGame framework.

The project presents a tile-based city made of pavements, buildings, gangs, police officers and citizens. The main goal of the simulation is to show how different groups can operate in a city, where gangs can occupy buildings, attack new areas and react to actions of other groups.

---

## Technologies

- C#
- .NET 8
- MonoGame
- Visual Studio

---

## Main Features

The project includes:

- tile-based city map generation,
- pavement and building generation,
- creation of building entrances,
- agent movement on the map,
- smooth agent movement,
- division of agents into groups,
- gang system,
- building occupation system,
- basic police reaction,
- basic citizen recruitment,
- pixel-art rendering.

---

## Project Structure

```text
GTASA/
├── GTASA.slnx
├── README.md
├── GTASA/
│   ├── GTASA.csproj
│   ├── Program.cs
│   ├── Game1.cs
│   ├── Content/
│   └── SymulationGeneric/
│       ├── Essentials.cs
│       ├── Symulation.cs
│       ├── Boards/
│       │   ├── Board.cs
│       │   └── Cells/
│       │       ├── Cell.cs
│       │       ├── Building.cs
│       │       ├── Pavment.cs
│       │       └── Void.cs
│       └── Groups/
│           ├── Group.cs
│           └── Agents/
│               └── Agent.cs
```



---

## Most Important Classes

### `Symulation.cs`

The main coordinator of the project. It creates the map, citizens, police and gangs.

It is also responsible for updating and drawing the whole simulation.

---

### `Board.cs`

Responsible for the game map.

Map generation process:

```text
1. Fill the map with empty cells.
2. Generate pavements.
3. Convert empty fields into buildings.
4. Connect pavements with other pavements.
5. Create entrances between pavements and buildings.
```

---

### `Group.cs`

Defines the groups used in the simulation:

- `Citizens` - citizens,
- `Police` - police,
- `Gang` - gangs.

Groups store their agents and decide what they should do.

---

### `Agent.cs`

Represents a single character in the simulation.

An agent:

- belongs to a specific group,
- has a position on the map,
- can move,
- can enter buildings,
- can be recruited,
- is drawn on the screen.

---

### `Building.cs`

Represents a building.

A building can:

- have an owner,
- be attacked,
- store agents inside,
- be taken over by another group,
- call nearby agents.

---

## Configuration

The most important settings are located in the `Essentials.cs` file.

Example:

```csharp
public static int mapSize = 15;
public static int pavmentCount = 2;
public static int pavmentOffset = 3;
public static int cellSize = 16;
public static int gangsCount = 2;
public static float speed = 0.2f;
public static float RENDER_ZOOM = 2f;
```

---

## How to Run the Project

There are two main ways to run the project: using Visual Studio or using the terminal.

### Option 1: Run with Visual Studio 2022 or newer

Requirements:

- Windows,
- Visual Studio 2022 or newer,
- .NET 8 SDK,
- MonoGame packages.

Steps:

1. Download or clone the repository.
2. Open the project folder.
3. Open the solution file:

```text
GTASA.slnx
```

4. Wait for Visual Studio to restore NuGet packages.
5. Select the startup project if needed.
6. Build the project using:

```text
Build → Build Solution
```

7. Run the project using the green Start button or by pressing:

```text
F5
```

If the project does not start, make sure that the required .NET SDK and MonoGame packages are installed correctly.

Video tutorial used as a reference:

```text
https://www.youtube.com/watch?v=F1Sj14ccuBQ
```

---

### Option 2: Run from the terminal

Requirements:

- Windows,
- .NET 8 SDK installed,
- terminal opened in the main project folder.

Commands:

```bash
dotnet restore
dotnet build
dotnet run --project GTASA/GTASA.csproj
```

If the command does not work, check whether the path to the `.csproj` file is correct.

The project is designed mainly for Windows because it uses MonoGame WindowsDX.

---

## Limitations

The current version of the project has several limitations:

- The simulation runs automatically and does not include player controls.
- The project is mainly designed for Windows because it uses MonoGame WindowsDX.

---

## Possible Future Improvements

In the future, the project could include:

- a full combat system,
- an economy system,
- a user interface,
- better artificial intelligence for agents,
- better police behavior,
- improved citizen behavior

---
