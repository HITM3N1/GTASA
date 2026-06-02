using Microsoft.Xna.Framework.Graphics;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    interface Cell
    {
        public CellType GetType();

        public void Draw(SpriteBatch spriteBatch);
    }
}
