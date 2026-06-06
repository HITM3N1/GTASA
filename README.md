# GTASA Simulation

## Opis

GTASA to prosta symulacja miasta napisana w jêzyku C# z u¿yciem frameworka MonoGame.
Projekt przedstawia kafelkowe miasto, w którym znajduj¹ siê mieszkañcy, dwa rywalizuj¹ce gangi oraz policja.

Docelowo symulacja ma opieraæ siê na wojnie gangów. Gangi bêd¹ walczyæ miêdzy sob¹ o przejmowanie budynków na planszy.
Kontrolowane budynki bêd¹ generowaæ pieni¹dze, które gangi bêd¹ mog³y wykorzystywaæ do rekrutowania mieszkañców i powiêkszania swojej si³y.
Policja ma pe³niæ rolê trzeciej frakcji, która bêdzie reagowaæ na dzia³ania gangów i wp³ywaæ na przebieg rozgrywki.

## Technologie

- C#
- .NET 8
- MonoGame

## Funkcje

- generowanie kwadratowej planszy kafelkowej
- losowe tworzenie chodników
- generowanie budynków
- tworzenie agentów
- tworzenie mieszkañców, gangów i policji
- przypisywanie gangom baz w budynkach
- podstawowe renderowanie mapy i agentów

## Uruchomienie

1. Zainstaluj .NET 8 SDK.
2. Zainstaluj [MonoGame](https://youtu.be/F1Sj14ccuBQ).
3. Otwórz folder projektu.
4. Uruchom:

```bash
dotnet run
```

## Struktura projektu

- `Program.cs` - punkt startowy programu.
- `Game1.cs` - g³ówna klasa gry.
- `Symulation.cs` - zarz¹dza symulacj¹.
- `Board.cs` - generuje planszê.
- `Agent.cs` - reprezentuje agenta.
- `Group.cs` - zawiera klasy grup.

## Status

Projekt jest w trakcie rozwoju. Aktualnie dzia³a generowanie mapy i podstawowe rysowanie agentów.

# GTASA Simulation

## Description

GTASA is a simple city simulation written in C# using the MonoGame framework.  
The project represents a tile-based city with citizens, two rival gangs, and the police.

The main goal of the simulation is to create a gang war system. Gangs will fight each other to take control of buildings on the map. Controlled buildings will generate money, which gangs will be able to use to recruit citizens and increase their strength. The police will act as a third faction that reacts to gang activity and affects the course of the simulation.

The map is tile-based and can have any square size.

## Technologies

- C#
- .NET 8
- MonoGame

## Features

- generation of a square tile-based map
- random pavement generation
- building generation
- agent creation
- citizens, gangs, and police groups
- assigning base buildings to gangs
- basic rendering of the map and agents

## Running the Project

1. Install .NET 8 SDK.
2. Install MonoGame using this [video tutorial](https://youtu.be/F1Sj14ccuBQ).
3. Open the project folder.
4. Run the project:

```bash
dotnet run
```

## Project Structure

- `Program.cs` - the entry point of the program.
- `Game1.cs` - the main MonoGame game class.
- `Symulation.cs` - manages the simulation.
- `Board.cs` - generates the board.
- `Agent.cs` - represents a single agent.
- `Group.cs` - contains group classes.

## Status

The project is currently in development. Map generation, pavement generation, building generation, and basic agent rendering are already implemented.  
In the future, the project will be expanded with gang fights, building control, money generation, citizen recruitment, and police behavior.