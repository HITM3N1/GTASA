using GTASA.SymulationGeneric.Boards.Cells;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric.Boards
{
    public class Board
    {
        private readonly int size;
        private Cell[,] grid;
        private Queue<Cell> toDraw;
        private Citizens citizens;

        private List<Building> buildings;
        private List<Pavment> pavments;

        public Board (int size, Citizens citizens)
        {
            this.size = size;
            grid = new Cell[size,size];
            buildings = new List<Building>();
            pavments = new List<Pavment>();
            toDraw = new Queue<Cell>();
            this.citizens = citizens;
        }

        public void Initialize()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = new EmptyCell();
                }
            }

            generatePavment(Essentials.pavmentCount, Essentials.pavmentOffset);
            GenerateBuilding();
        }

        public void Update()
        {
            toDraw.Clear();

            prepareToDraw();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cell in toDraw)
            {
                cell.Draw(spriteBatch);
            }
        }


        private void prepareToDraw()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (!toDraw.Contains(grid[x, y]))
                    {
                        toDraw.Enqueue(grid[x, y]);
                    }
                }
            }
        }

        public Vector2 GetRandomPavment()
        {
            Random random = new Random();

            return pavments[random.Next(0, pavments.Count)].GetAbsolutPosition();
        }

        public Vector2 GetFreeBuilding(Group group)
        {
            Random random = new Random();
            int index = random.Next(0, buildings.Count);

            while (!buildings[index].Isfree())
            {
                index = random.Next(0, buildings.Count);
            }

            buildings[index].SetOccupation(group);
            return buildings[index].GetCenter();
        }


        private void generatePavment(int count, int minimaloffset)
        {
            Random random = new Random();

            Vector2[] centers = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                centers[i] = new Vector2(int.MaxValue, int.MaxValue);
            }

            int k = 0;

            while (k != count)
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

                    grid[j, (int)centers[i].Y] = p1;
                    grid[(int)centers[i].X, j] = p2;

                    pavments.Add(p1);
                    pavments.Add(p2);
                }
            }
        }


        void GenerateBuilding()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[x, y].GetType() == CellType.EmptyCell)
                    {
                        int subx = x;
                        int suby = y;

                        while (subx < size && grid[subx, suby].GetType() == CellType.EmptyCell)
                        {
                            subx++;
                        }

                        subx--;

                        while (suby < size && grid[subx, suby].GetType() == CellType.EmptyCell)
                        {
                            suby++;
                        }

                        subx++;

                        Building building = new Building(new Vector2(x, y), new Rectangle(x, y, subx, suby), citizens);

                        buildings.Add(building);

                        for (int i = x; i < subx; i++)
                        {
                            for (int j = y; j < suby; j++)
                            {
                                grid[i, j] = building;
                            }
                        }
                    }
                }
            }
        }

        public List<Vector2> GetNeighborPavements(Vector2 position)
        {
            int tx = (int)(position.X / Essentials.cellSize);
            int ty = (int)(position.Y / Essentials.cellSize);

            var neighbors = new List<Vector2>();
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = tx + dx[i];
                int ny = ty + dy[i];
                if (InBounds(nx, ny) && grid[nx, ny].GetType() == CellType.Pavment)
                {
                    neighbors.Add(new Vector2(nx * Essentials.cellSize / 2f, ny * Essentials.cellSize / 2f));
                }
            }
            return neighbors;
        }

        private bool InBounds(int x, int y) => x >= 0 && x < size && y >= 0 && y < size;
    }
}
