using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class Building : Cell
    {
        private Vector2 cords;
        private CellType type;
        private Rectangle bounds;

        private GroupAbstract occupation;

        public Building(Vector2 cords, Rectangle bounds, GroupAbstract occupation)
        {
            this.cords = cords;
            this.type = CellType.Building;
            this.occupation = occupation;
            
            this.bounds = bounds;
        }

        public CellType GetType()
        {
            return type;
        }

        public bool Isfree()
        {
            if (occupation.GetColor() == GroupColor.White) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetOccupation(Group group)
        {
            this.occupation = group;
        }

        public Vector2 GetCenter()
        {
            return new Vector2 ((bounds.Width - bounds.X - 1) * 8 + (bounds.X * 16), (bounds.Height - bounds.Y - 1) * 8 + (bounds.Y * 16));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = bounds.X; x < bounds.Width; x++)
            {
                for (int y = bounds.Y; y < bounds.Height; y++)
                {
                    if (x > bounds.X && y > bounds.Y && x < bounds.Width - 1 && y < bounds.Height - 1)
                    {
                        spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B0, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                    }
                    else
                    {
                        if (x == bounds.X)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B8, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == (bounds.Width - 1))
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B4, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B2, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (y == (bounds.Height - 1))
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B6, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }




                        if (x == bounds.X && y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B1, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.Width - 1 && y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B3, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.Width - 1 && y == bounds.Height - 1)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B5, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.X && y == bounds.Height - 1)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B7, occupation.GetColor())], new Vector2(16 * x, 16 * y), Color.White);
                        }
                        
                    }
                }
            }
        }
    }
}
