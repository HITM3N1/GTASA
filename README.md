# GTASA Simulation

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

...

---

## Limitations

...

## Possible Future Improvements

In the future, the project could include:

- a full combat system,
- an economy system,
- a user interface,
- better artificial intelligence for agents,
- better police behavior,
- improved citizen behavior,

---
