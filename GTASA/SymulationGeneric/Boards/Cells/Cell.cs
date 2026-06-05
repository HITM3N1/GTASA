using Microsoft.Xna.Framework.Graphics;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    interface Cell
    {
        public CellType GetCellType();

        public void Draw(SpriteBatch spriteBatch);
    }
}
