using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GTASA.SymulationGeneric.Boards.Cells
{

    //**************************************************************************************//
    //***** Interfejs : Cell                                                           *****//
    //**************************************************************************************//
    //***** Definiuję podstawową działalność każdej komórki na mapie                   *****//
    //**************************************************************************************//

    public interface Cell
    {
        public CellType GetCellType();

        public Vector2 GetAbsolutPosition();

        public void AddAgent(Agent agent);

        public void RemoveAgent(Agent agent);

        public void Draw(SpriteBatch spriteBatch);
    }
}
