using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class Pavment : Cell
    {
        private Vector2 cords;
        private CellType type;
        private Rectangle bounds;
        private TextureType textureType;
        public Pavment(Vector2 cords)
        {
            this.cords = cords;
            type = CellType.Pavment;
            bounds = new Rectangle((int)cords.X * Essentials.cellSize, (int)cords.Y * Essentials.cellSize, 16, 16);


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

        public Vector2 GetAbsolutPosition()
        {
            return new Vector2(bounds.X , bounds.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesPavment[textureType], bounds, Color.White);
        }

        public CellType GetType()
        {
            return type;
        }


    }
}
