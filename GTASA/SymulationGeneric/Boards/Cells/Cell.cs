using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GTASA.SymulationGeneric.Boards.Cells
{
    public interface Cell
    {
        public CellType GetCellType();

        public Vector2 GetSpawnAbsolutePosition();

        public Vector2 GetAbsolutPosition();

        public void Draw(SpriteBatch spriteBatch);
    }
}
