using GTASA.SymulationGeneric.Boards.Cells;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric.Boards
{
    //**************************************************************************************//
    //***** Klasa : Board                                                              *****//
    //**************************************************************************************//
    //***** Board odpowiada za cała mapę gry, tworzy ją i następnie zarządza           *****//
    //**************************************************************************************//

    public class Board 
    {
        // size - rozmair mapy
        private readonly int size; 

        // grid - siatka całej mapy, używana tylko podczas tworzenia 
        private Dictionary<Vector2, Cell> grid;

        // pavments - lista wszystkich chodników mapy
        private List<Pavment> pavments;

        // buildings - lista wszystkich budynków mapy
        private List<Building> buildings;

        // citizens - referenacja do neutalnej klasy w grze
        private Citizens citizens;

        // Konstruktor - przygotowujący "puste pojejmniki"
        public Board(int size)
        {
            this.size = size;
            this.grid = new Dictionary<Vector2, Cell>();


            buildings = new List<Building>();
            pavments = new List<Pavment>();
        }


        // Initialize - tworzy cała mapę. Proces jest podzielony na 4 etapy
        public void Initialize(Citizens citizens) 
        {

            // ETAP 1 - stworzenie pustej siatki
            this.citizens = citizens;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[new Vector2(x, y)] = new EmptyCell();
                }
            }

            // ETAP 2 - tworzenie chodników
            GeneratePavment(Essentials.PAVMENT_COUNT, Essentials.PAVMENT_OFFSET + 1); 

            // ETAP 3 - tworzenie budynków
            GenerateBuilding();

            // ETAP 4 - tworzenie sąsiadów i nadanie im zalezności
            GenerateNeighbors();
        }

        // Update - wywołuje aktualizajce wszystkich budyjnków na mapie
        public void Update(GameTime gameTime) 
        {
            foreach (Building building in buildings)
            {
                building.Update(gameTime);
            }
        }

        // Draw - wywołuje rysowanie budynków i chodników na mapie
        public void Draw(SpriteBatch spriteBatch) 
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



        // GeneratePavment - funkcja odpowiadająca za tworzenie chodników na mapie 
        private void GeneratePavment(int count, int minimaloffset)
        {
            Vector2[] centers = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                centers[i] = new Vector2(int.MaxValue, int.MaxValue);
            }

            int k = 0;

            while (k != count) 
            {
                int y = Essentials.RANDOM.Next(3, size - 3);
                int x = Essentials.RANDOM.Next(3, size - 3);

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


        // GenerateBuilding - funkcja odpowiadająca za tworzenie budynków na mapie 
        void GenerateBuilding()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[new Vector2(x, y)].GetCellType() == CellType.EmptyCell) // szukanie pustej komórki jesli tak to zaczunamy budowe nowego budynku od tego miejsca
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

                        Building building = new Building(new Rectangle(x, y, subx, suby), citizens); // tworzenie budynku 

                        buildings.Add(building);// dodanie buydnku

                        for (int i = x; i < subx; i++)
                        {
                            for (int j = y; j < suby; j++)
                            {
                                grid[new Vector2(i, j)] = building; // wpisanie budynku do grida - wszykite komórki należą do jedneog budynku - jeden obiekt
                            }
                        }
                    }
                }
            }
        }

        // GenerateNeighbors - funkcja odpowiadająca za tworzenie sąsiadów na mapie 

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

        // CheckNeighbor - funkcja sprawdza co znajduje się wokół chodnika
        public void CheckNeighbor(Pavment pavment, Cell neighbor, int direction) 
        {
            if (neighbor.GetCellType() == CellType.Pavment) // spradzanie czy obok jest chodnik
            {
                pavment.SetNeighbor(direction, (Pavment)neighbor);
            }
            else
            {
                if (neighbor.GetCellType() == CellType.Building && !pavment.IsBuildingEntrace()) // jezeli bok chodnia jest budynek to dajemy tam wejście do budynku pod warunkiem ze to miejsce nie jest już wejście dla innego budynku
                {
                    TryMakeEntrace((Building)neighbor, pavment, direction);
                }
            }
        }


        // TryMakeEntrace - funkcja która próbuje stworzyć wejście do budynku o ile nie jest jeszcze ustawione
        public void TryMakeEntrace(Building building, Pavment pavment, int directionFromPavmentToBuilding)
        {
            if (building.IsExitSet())
            {
                if(Essentials.RANDOM.Next(0, 100) < 20)
                {
                    building.SetExit(directionFromPavmentToBuilding, pavment);
                }
            }
            else
            {
                building.SetExit(directionFromPavmentToBuilding, pavment);
            }
        }



        //--------------------------------
        //      GETTERY I SETTERY
        //--------------------------------

        public Cell GetCell(Vector2 absolutPosition) // zamiana pozycji pikselowej na kafelkmowa(XD, nie iwem jak to nazwac)
        {
            return grid[new Vector2(MathF.Floor(absolutPosition.X / Essentials.CELL_SIZE), MathF.Floor(absolutPosition.Y / Essentials.CELL_SIZE))];
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
            return pavments[Essentials.RANDOM.Next(0, pavments.Count)];
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

            return borderPavments[Essentials.RANDOM.Next(0, borderPavments.Count)];
        }

        public Building GetFreeSpawnBuilding(Group group) // wolny buydenk do pojawienia sie bazy gangu bez przypadku gdy jest za malo budynku
        {
            int index = Essentials.RANDOM.Next(0, buildings.Count);

            while (!buildings[index].Isfree())
            {
                index = Essentials.RANDOM.Next(0, buildings.Count);
            }

            buildings[index].SetOccupation(group);
            return buildings[index];
        }

        public Building GetRandomFreeBuilding(GroupAbstract groupAbstract) // losowy budynek który nioe należy do tej grupy
        {
            int index = Essentials.RANDOM.Next(0, buildings.Count);

            while (buildings[index].GetOccupation() == groupAbstract)
            {
                index = Essentials.RANDOM.Next(0, buildings.Count);
            }

            return buildings[index];
        }

        public void SetCitizens(Citizens citizens)
        {
            this.citizens = citizens;
        }
        public List<Building> GetBuildings()
        {
            return buildings;
        }




    }
}