using GTASA.SymulationGeneric.Boards.Cells;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric.Boards
{
    public class Board // mapa gry
    {
        private readonly int size; // wielkosc z Essentials

        private Dictionary<Vector2, Cell> grid; // pozycja kafełka i co tam stoi
        private List<Pavment> pavments; // lista wszystkich chodników po to aby np przejśc po wszystkich chodnikach bez sprwadzania całej mapy
        private List<Building> buildings; //lista wszystkich budynków np żeby wylosowac budynek

        private Citizens citizens; //referencja do grupy mieszkańców

        public Board(int size)// konstruktor przygotowujący "puste pojejmniki"
        {
            this.size = size;
            this.grid = new Dictionary<Vector2, Cell>();


            buildings = new List<Building>();
            pavments = new List<Pavment>();
        }

        public void Initialize(Citizens citizens) // tworzy całą mape 
        {
            this.citizens = citizens;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[new Vector2(x, y)] = new EmptyCell(); //tworzy pierwszo wszędzie psute pola
                }
            }

            GeneratePavment(Essentials.pavmentCount, Essentials.pavmentOffset); // tworzy chodniki
            GenerateBuilding(); // tworzy budynki
            GenerateNeighbors(); // tworzy sąsiadów - łączy chodniki z chodnikami i budynki z budynkami
        }


        private void GeneratePavment(int count, int minimaloffset)
        {
            Random random = new Random();

            Vector2[] centers = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                centers[i] = new Vector2(int.MaxValue, int.MaxValue);
            }

            int k = 0;

            while (k != count) // moze nie działać dla z małych map jak na razie
            {
                int y = random.Next(3, size - 3);
                int x = random.Next(3, size - 3);

                bool canBeANewCenter = true;

                for (int i = 0; i < count; ++i)
                {
                    if (Math.Abs(centers[i].X - x) < minimaloffset || Math.Abs(centers[i].Y - y) < minimaloffset)
                    {
                        canBeANewCenter = false;
                    }
                }

                if (canBeANewCenter)
                {
                    centers[k] = new Vector2(x, y);
                    k++;
                }
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Pavment p1 = new Pavment(new Vector2(j, (int)centers[i].Y));
                    Pavment p2 = new Pavment(new Vector2((int)centers[i].X, j));

                    grid[new Vector2(j, (int)centers[i].Y)] = p1;
                    grid[new Vector2((int)centers[i].X, j)] = p2;
                }
            }
        }

        void GenerateBuilding()// tworzenie budynku
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[new Vector2(x, y)].GetCellType() == CellType.EmptyCell) // szukanie pustego budynku jesli tak to zacyznami budowe nowego budynku od tego miejsca
                    {
                        int subx = x;
                        int suby = y;

                        while (subx < size && grid[new Vector2(subx, suby)].GetCellType() == CellType.EmptyCell) // idziemy w prawo
                        {
                            subx++;
                        }

                        subx--;

                        while (suby < size && grid[new Vector2(subx, suby)].GetCellType() == CellType.EmptyCell) // idziemy w dół
                        {
                            suby++;
                        }

                        subx++;

                        Building building = new Building(new Vector2(x, y), new Rectangle(x, y, subx, suby), citizens); // tworzenie budynku 

                        buildings.Add(building);// dodanie buydnku

                        for (int i = x; i < subx; i++)
                        {
                            for (int j = y; j < suby; j++)
                            {
                                grid[new Vector2(i, j)] = building; // wpisanie budynku do grida- wszykite kafleki sta należą do jedneog budynku - jeden obiekt
                            }
                        }
                    }
                }
            }
        }

        void GenerateNeighbors()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[new Vector2(x, y)].GetCellType() == CellType.Pavment) // szukanie chodnikow
                    {
                        Pavment pavment = (Pavment)grid[new Vector2(x, y)];
                        pavments.Add(pavment);


                        if (x > 0)
                        {
                            CheckNeighbor(pavment, grid[new Vector2(x - 1, y)], 0);
                        }

                        if (y > 0)
                        {
                            CheckNeighbor(pavment, grid[new Vector2(x, y - 1)], 1);
                        }

                        if (x < size - 1)
                        {
                            CheckNeighbor(pavment, grid[new Vector2(x + 1, y)], 2);
                        }

                        if (y < size - 1)
                        {
                            CheckNeighbor(pavment, grid[new Vector2(x, y + 1)], 3);
                        }
                    }
                }
            }
        }


        public void CheckNeighbor(Pavment pavment, Cell neighbor, int direction) // sprawdzanie co ejst obok chodnika
        {
            if (neighbor.GetCellType() == CellType.Pavment) // spradzanie czy obok jest chodnik
            {
                pavment.SetNeighbor(direction, (Pavment)neighbor);
            }
            else
            {
                if (neighbor.GetCellType() == CellType.Building && !pavment.IsBuildingEntrace()) // jezeli bok chodnia jest budynek to dajemy tam wejście do budynku pod warunkiem ze nie ma juz tam choidnika
                {
                    TryMakeEntrace((Building)neighbor, pavment, direction);
                }
            }
        }

        public bool TryMakeEntrace(Building building, Pavment pavment, int directionFromPavmentToBuilding)
        {
            if (building.IsExitSet())
            {
                return true;
            }
            else
            {
                building.SetExit(directionFromPavmentToBuilding, pavment);
                pavment.SetBuildingEntrace(directionFromPavmentToBuilding, building);


                return true;
            }
        }

        public void Update(GameTime gameTime) //aktrualzijce tylko budynki
        {
            foreach (Building building in buildings)
            {
                building.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) // rysuje mape
        {

            foreach (Pavment cell in pavments)
            {
                cell.Draw(spriteBatch);
            }

            foreach (Building cell in buildings)
            {
                cell.Draw(spriteBatch);
            }
        }


        public Cell GetCell(Vector2 absolutPosition) // zamiana pozycji pikselowej na kafelkmowa(XD, nie iwem jak to nazwac)
        {
            return grid[new Vector2(MathF.Floor(absolutPosition.X / Essentials.cellSize), MathF.Floor(absolutPosition.Y / Essentials.cellSize))];
        }


        public List<Building> GetGangBuildings(GroupAbstract group) //zwraca wszystkie budynki które należą do danej grupy
        {
            List<Building> result = new List<Building>();

            foreach (Building building in buildings)
            {
                if (building.GetOccupation() == group)
                {
                    result.Add(building);
                }
            }

            return result;
        }

        public List<Building> GetAllBuildingsUnderAttack() // zwwraca wsszytkie budynki w trakcie ataku
        {
            List<Building> result = new List<Building>();

            foreach (Building building in buildings)
            {
                if (building.IsUnderAttack())
                {
                    result.Add(building);
                }
            }

            return result;
        }



        public Cell GetRandomPavment() // zwraca losowy choidnik
        {
            Random random = new Random();
            return pavments[random.Next(0, pavments.Count)];
        }

        public Cell GetRandomBorderPavment() //losowe końcówki chodników do respienia mieszkańców
        {
            List<Pavment> borderPavments = new List<Pavment>();

            foreach (Pavment pavment in pavments)
            {
                if (pavment.pavmentsNearby.Count == 1)
                {
                    borderPavments.Add(pavment);
                }
            }

            Random random = new Random();
            return borderPavments[random.Next(0, borderPavments.Count)];
        }

        public Building GetFreeSpawnBuilding(Group group) // wolny buydenk do pojawienia sie bazy gangu bez przypadku gdy jest za malo budynku
        {
            Random random = new Random();
            int index = random.Next(0, buildings.Count);

            while (!buildings[index].Isfree())
            {
                index = random.Next(0, buildings.Count);
            }

            buildings[index].SetOccupation(group);
            return buildings[index];
        }

        public Building GetRandomFreeBuilding(GroupAbstract groupAbstract) // losowy budynek który nioe należy do tej grupy
        {
            Random random = new Random();
            int index = random.Next(0, buildings.Count);

            while (buildings[index].GetOccupation() == groupAbstract)
            {
                index = random.Next(0, buildings.Count);
            }

            return buildings[index];
        }

        public void SetCitizens(Citizens citizens)
        {
            this.citizens = citizens;
        }




        /*
        public Vector2? GetNeighborInDirection(Vector2 position, int directionIndex)
        {
            int tx = (int)(position.X / Essentials.cellSize);
            int ty = (int)(position.Y / Essentials.cellSize);

            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, -1, 0, 1 };

            int nx = tx + dx[directionIndex];
            int ny = ty + dy[directionIndex];

            if (InBounds(nx, ny) && grid[nx, ny].GetCellType() == CellType.Pavment)
            {
                return new Vector2(nx * Essentials.cellSize, ny * Essentials.cellSize);
            }
            return null;
        }

        private bool InBounds(int x, int y) => x >= 0 && x < size && y >= 0 && y < size;

        */
    }
}