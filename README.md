# GTASA Simulation

## Opis

GTASA Simulation to prosta symulacja miasta napisana w jêzyku C# z u¿yciem MonoGame. 
Projekt generuje kafelkow¹ mapê miasta z chodnikami, budynkami oraz agentami nale¿¹cymi do ró¿nych grup.

## Technologie

- C#
- .NET 8
- MonoGame

## Funkcje

- generowanie planszy 32x32
- losowe tworzenie chodników
- generowanie budynków
- tworzenie agentów
- przypisywanie gangom baz

## Uruchomienie

1. Zainstaluj .NET 8 SDK.
2. Zainstaluj MonoGame.
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