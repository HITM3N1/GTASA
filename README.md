# GTASA Simulation

## Opis projektu

GTASA Simulation to prosta symulacja miasta 2D napisana w języku C# z użyciem frameworka MonoGame.

Projekt przedstawia kafelkowe miasto zbudowane z chodników, budynków, gangów, policji i mieszkańców. Głównym celem symulacji jest pokazanie działania grup w mieście, gdzie gangi mogą przejmować budynki, atakować nowe obszary oraz reagować na działania innych grup.

Projekt jest obecnie prototypem, dlatego część mechanik działa w podstawowej wersji, a część została przygotowana pod dalszy rozwój.



## Technologie

- C#
- .NET 8
- MonoGame
- Visual Studio

Projekt jest przygotowany głównie pod system Windows.

---

## Główne funkcje

Projekt zawiera:

- generowanie kafelkowej mapy miasta,
- generowanie chodników i budynków,
- tworzenie wejść do budynków,
- poruszanie agentów po mapie,
- płynny ruch agentów,
- podział agentów na grupy,
- system gangów,
- przejmowanie budynków,
- podstawową reakcję policji,
- podstawową rekrutację mieszkańców,
- renderowanie grafik pixel-art.

---

## Struktura projektu

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

## Architektura projektu

Ogólny przepływ działania programu:

```text
Program.cs
   ↓
Game1.cs
   ↓
Symulation.cs
   ↓
Board.cs
   ↓
Building.cs / Pavment.cs / Cell.cs
   ↓
Group.cs
   ↓
Agent.cs
```

Znaczenie głównych plików:

- `Program.cs` - uruchamia grę.
- `Game1.cs` - obsługuje MonoGame, okno gry, aktualizację i rysowanie.
- `Symulation.cs` - tworzy i koordynuje całą symulację.
- `Board.cs` - generuje i przechowuje mapę.
- `Cell.cs` - definiuje wspólny interfejs komórek mapy.
- `Pavment.cs` - reprezentuje chodniki.
- `Building.cs` - reprezentuje budynki i logikę ich przejmowania.
- `Group.cs` - definiuje grupy: gangi, policję i mieszkańców.
- `Agent.cs` - obsługuje pojedynczego agenta, jego ruch i interakcje.

---

## Najważniejsze klasy

### `Symulation.cs`

Główny koordynator projektu. Tworzy mapę, mieszkańców, policję i gangi.  
Odpowiada też za aktualizowanie oraz rysowanie całej symulacji.



### `Board.cs`

Odpowiada za mapę gry.


### `Group.cs`

Definiuje grupy występujące w symulacji:

- `Citizens` - mieszkańcy,
- `Police` - policja,
- `Gang` - gangi.



### `Agent.cs`

Reprezentuje pojedynczą postać w symulacji.

Agent:

- należy do konkretnej grupy,
- posiada pozycję na mapie,
- może się poruszać,
- może wchodzić do budynków,
- może zostać zrekrutowany,
- jest rysowany na ekranie.

Ruch agenta jest płynny dzięki interpolacji:

```csharp
absolutPosition = Vector2.Lerp(startPosition, targetPosition, t);
```

---

### `Building.cs`

Reprezentuje budynek.

Budynek może:

- mieć właściciela,
- być atakowany,
- przechowywać agentów w środku,
- zostać przejęty przez inną grupę,
- wołać agentów z okolicy.

---

## Przebieg działania symulacji

```text
Program.cs uruchamia grę.
        ↓
Game1.cs tworzy okno i symulację.
        ↓
Symulation tworzy mapę oraz grupy.
        ↓
Board generuje chodniki i budynki.
        ↓
Gangi otrzymują swoje budynki bazowe.
        ↓
Agenci gangów pojawiają się w bazach.
        ↓
Gangi wybierają budynki do ataku.
        ↓
Agenci idą do celu.
        ↓
Budynki rozstrzygają przejęcie.
        ↓
Symulacja działa dalej co klatkę.
```

---

## Konfiguracja

Najważniejsze ustawienia znajdują się w pliku `Essentials.cs`.

Przykład:

```csharp
public static int mapSize = 15;
public static int pavmentCount = 2;
public static int pavmentOffset = 3;
public static int cellSize = 16;
public static int gangsCount = 2;
public static float speed = 0.2f;
public static float RENDER_ZOOM = 2f;
```

Ustawienia grup:

```csharp
public static int PoliceStarMembers = 0;
public static int CitizensStarMembers = 0;
public static int[] GangStarMembers = { 3, 3 };
public static bool CitizenSpawn = false;
```


## Jak uruchomić projekt

in proggres


## ograniczenia

in proggres

## Możliwe dalsze usprawnienia

W przyszłości można dodać:

- pełny system walki,
- system ekonomii,
- interfejs użytkownika,
- sterowanie graczem,
- lepszą sztuczną inteligencję agentów,
- lepszą reakcję policji,
- uporządkowanie nazw klas i metod,
- zabezpieczenia przed nieskończonymi pętlami.



