# GTASA Simulation

## English Version

## Description

GTASA Simulation is a simple tile-based city simulation written in C# using the MonoGame framework.  
The project represents a city with citizens, two rival gangs, police units, pavements, and buildings.

The main idea of the project is to create a gang war simulation. In the future, gangs will fight each other to take control of buildings on the map. Controlled buildings are planned to generate money, which gangs will be able to use to recruit citizens and increase their strength. The police will act as a third faction that affects the behavior of gangs and citizens.

Currently, the project already includes procedural map generation, buildings, pavements, citizens, gangs, police units, and basic agent movement.

## Technologies

- C#
- .NET 8
- MonoGame

## Current Features

- generation of a square tile-based map
- default map size of 32x32 tiles
- configurable map size in the code
- random pavement generation
- automatic building generation
- creation of citizens, gangs, and police groups
- assigning base buildings to gangs
- basic rendering of the map, buildings, pavements, and agents
- random wandering movement for citizens
- random wandering movement for police units
- citizens fleeing from nearby gang members
- basic group color system for agents and buildings

## Planned Features

- gang movement around the city
- fights between rival gangs
- taking control of buildings
- money generation from controlled buildings
- recruiting citizens into gangs
- police reactions to gang activity
- improved agent behavior
- user interface with simulation statistics

## Running the Project

1. Install .NET 8 SDK.
2. Install MonoGame using this [video tutorial](https://youtu.be/F1Sj14ccuBQ).
3. Open the project folder in the terminal.
4. Run the project:

```bash
dotnet run
```

## Project Structure

- `Program.cs` - the entry point of the program.
- `Game1.cs` - the main MonoGame game class.
- `Symulation.cs` - manages the main simulation logic.
- `Board.cs` - generates and stores the tile-based map.
- `Cell.cs` - defines the common interface for map cells.
- `EmptyCell.cs` - represents an empty map cell.
- `Pavment.cs` - represents pavement tiles.
- `Building.cs` - represents buildings on the map.
- `Agent.cs` - represents a single moving agent.
- `Group.cs` - contains group classes such as citizens, police, and gangs.
- `Essentials.cs` - stores global settings, textures, and enums.

## How the Simulation Works

When the program starts, it creates a square tile-based board.  
The board is first filled with empty cells. Then, the program generates pavements and creates buildings in the remaining empty spaces.

After the map is generated, the simulation creates different groups of agents:

- citizens
- police
- red gang
- green gang

Gangs receive base buildings on the map. Citizens and police units spawn on pavements and move around the city. Citizens wander randomly, but when they detect a nearby gang member, they try to flee.

## Configuration

Some basic project settings can be changed in `Essentials.cs`.

Examples:

- map size
- tile size
- number of generated pavements
- render zoom
- agent speed
- flee speed
- flee distance

The default map size is currently set to 32x32 tiles, but the board is designed to work as a square map with a configurable size.

## Status

The project is currently in development.

Already implemented:

- map generation
- pavement generation
- building generation
- gang base assignment
- rendering system
- basic citizen movement
- citizen fleeing behavior
- basic police movement

Not implemented yet:

- gang movement
- gang fights
- building takeover system
- money system
- recruitment system
- advanced police behavior

---

# Wersja polska

## Opis

GTASA Simulation to prosta symulacja miasta oparta na kafelkach, napisana w jêzyku C# z u¿yciem frameworka MonoGame.  
Projekt przedstawia miasto z mieszkañcami, dwoma rywalizuj¹cymi gangami, policj¹, chodnikami oraz budynkami.

G³ównym celem projektu jest stworzenie symulacji wojny gangów. W przysz³oœci gangi bêd¹ walczyæ miêdzy sob¹ o przejmowanie budynków na mapie. Kontrolowane budynki maj¹ generowaæ pieni¹dze, które gangi bêd¹ mog³y wykorzystywaæ do rekrutowania mieszkañców i zwiêkszania swojej si³y. Policja bêdzie pe³niæ rolê trzeciej frakcji, która wp³ywa na zachowanie gangów i mieszkañców.

Aktualnie projekt zawiera ju¿ proceduralne generowanie mapy, budynki, chodniki, mieszkañców, gangi, policjê oraz podstawowy ruch agentów.

## Technologie

- C#
- .NET 8
- MonoGame

## Aktualne funkcje

- generowanie kwadratowej mapy kafelkowej
- domyœlny rozmiar mapy 32x32 kafelki
- mo¿liwoœæ zmiany rozmiaru mapy w kodzie
- losowe generowanie chodników
- automatyczne generowanie budynków
- tworzenie mieszkañców, gangów i policji
- przypisywanie gangom baz w budynkach
- podstawowe renderowanie mapy, budynków, chodników i agentów
- losowe poruszanie siê mieszkañców
- losowe poruszanie siê policji
- uciekanie mieszkañców przed pobliskimi cz³onkami gangów
- podstawowy system kolorów grup dla agentów i budynków

## Planowane funkcje

- poruszanie siê gangów po mieœcie
- walki miêdzy rywalizuj¹cymi gangami
- przejmowanie budynków
- generowanie pieniêdzy z kontrolowanych budynków
- rekrutowanie mieszkañców do gangów
- reakcje policji na dzia³ania gangów
- ulepszone zachowanie agentów
- interfejs u¿ytkownika ze statystykami symulacji

## Uruchomienie projektu

1. Zainstaluj .NET 8 SDK.
2. Zainstaluj MonoGame, korzystaj¹c z [poradnika wideo](https://youtu.be/F1Sj14ccuBQ).
3. Otwórz folder projektu w terminalu.
4. Uruchom projekt:

```bash
dotnet run
```

## Struktura projektu

- `Program.cs` - punkt startowy programu.
- `Game1.cs` - g³ówna klasa gry MonoGame.
- `Symulation.cs` - zarz¹dza g³ówn¹ logik¹ symulacji.
- `Board.cs` - generuje i przechowuje mapê kafelkow¹.
- `Cell.cs` - definiuje wspólny interfejs dla pól mapy.
- `EmptyCell.cs` - reprezentuje puste pole mapy.
- `Pavment.cs` - reprezentuje kafelki chodnika.
- `Building.cs` - reprezentuje budynki na mapie.
- `Agent.cs` - reprezentuje pojedynczego poruszaj¹cego siê agenta.
- `Group.cs` - zawiera klasy grup, takie jak mieszkañcy, policja i gangi.
- `Essentials.cs` - przechowuje globalne ustawienia, tekstury i typy wyliczeniowe.

## Jak dzia³a symulacja

Po uruchomieniu program tworzy kwadratow¹ mapê kafelkow¹.  
Na pocz¹tku mapa jest wype³niana pustymi polami. Nastêpnie program generuje chodniki i tworzy budynki w pozosta³ych pustych przestrzeniach.

Po wygenerowaniu mapy symulacja tworzy ró¿ne grupy agentów:

- mieszkañców
- policjê
- czerwony gang
- zielony gang

Gangi otrzymuj¹ swoje budynki bazowe na mapie. Mieszkañcy i policja pojawiaj¹ siê na chodnikach i poruszaj¹ siê po mieœcie. Mieszkañcy chodz¹ losowo, ale gdy wykryj¹ w pobli¿u cz³onka gangu, próbuj¹ uciekaæ.

## Konfiguracja

Niektóre podstawowe ustawienia projektu mo¿na zmieniæ w pliku `Essentials.cs`.

Przyk³ady:

- rozmiar mapy
- rozmiar kafelka
- liczba generowanych chodników
- powiêkszenie renderowania
- prêdkoœæ agentów
- prêdkoœæ ucieczki
- dystans ucieczki

Domyœlny rozmiar mapy jest obecnie ustawiony na 32x32 kafelki, ale plansza zosta³a zaprojektowana jako kwadratowa mapa z mo¿liwoœci¹ zmiany rozmiaru.

## Status

Projekt jest aktualnie w trakcie rozwoju.

Ju¿ zaimplementowano:

- generowanie mapy
- generowanie chodników
- generowanie budynków
- przypisywanie baz gangom
- system renderowania
- podstawowy ruch mieszkañców
- uciekanie mieszkañców przed gangami
- podstawowy ruch policji

Nie zaimplementowano jeszcze:

- ruchu gangów
- walk miêdzy gangami
- systemu przejmowania budynków
- systemu pieniêdzy
- systemu rekrutacji
- zaawansowanego zachowania policji