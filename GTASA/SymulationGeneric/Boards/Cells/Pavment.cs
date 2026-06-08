using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class Pavment : Cell
    {
        private Vector2 cords;
        private CellType type;
        public Dictionary<int, Pavment> pavmentsNearby;

        private bool isBuildingEntrace;
        private (int, Building) entrace;


        private TextureType textureType;
        public Pavment(Vector2 cords)
        {
            this.cords = cords;
            this.type = CellType.Pavment;
            this.pavmentsNearby = new Dictionary<int, Pavment>();
            this.isBuildingEntrace = false;

            Random random = new Random();
            int r = random.Next(0, 100);

            if(r < 85)
            {
                textureType = TextureType.P0;
            }
            else if(r < 90)
            {
                textureType = TextureType.P1;
            }
            else if(r < 95)
            {
                textureType = TextureType.P2;
            }
            else
            {
                textureType = TextureType.P3;
            }

        }

        public Vector2 GetSpawnAbsolutePosition()
        {
            return GetAbsolutPosition();
        }

        public Vector2 GetAbsolutPosition()
        {
            return new Vector2(cords.X * 16 , cords.Y * 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesPavment[textureType], GetAbsolutPosition(), Color.White);
        }

        public CellType GetCellType()
        {
            return type;
        }

        public bool IsBuildingEntrace()
        {
            return isBuildingEntrace;
        }

        public void SetBuildingEntrace(int directionFromPavmentToBuilding, Building building)
        {
            entrace = (directionFromPavmentToBuilding, building);
            isBuildingEntrace = true;
        }

        public void SetNeighbor(int direction ,Pavment cell)
        {
            pavmentsNearby[direction] = cell;
        }

    }
}
