using Microsoft.Xna.Framework.Graphics;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class EmptyCell : Cell
    {
        public CellType GetType()
        {
            return CellType.EmptyCell;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
