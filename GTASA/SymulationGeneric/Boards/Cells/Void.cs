using Microsoft.Xna.Framework.Graphics;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class EmptyCell : Cell
    {
        public CellType GetCellType()
        {
            return CellType.EmptyCell;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
