using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
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

        public Vector2 GetSpawnAbsolutePosition()
        {
            return Vector2.Zero;
        }

        public Vector2 GetAbsolutPosition()
        {
            return Vector2.Zero;
        }

        public void AddAgent(Agent agent)
        {

        }

        public void RemoveAgent(Agent agent)
        {

        }
    }
}
