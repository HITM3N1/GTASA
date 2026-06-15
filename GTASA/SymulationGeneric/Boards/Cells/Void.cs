using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GTASA.SymulationGeneric.Boards.Cells
{
    //**************************************************************************************//
    //***** Klasa : EmptyCell - dziedziczy po Cell                                     *****//
    //**************************************************************************************//
    //***** Jedna z trzech rodzajów komórek dostępnych w symulacji, jest to komórka    *****//
    //***** podstawowa, jest ona całkowicie pusta logicznie                            *****//
    //**************************************************************************************//

    public class EmptyCell : Cell
    {
        public CellType GetCellType()
        {
            return CellType.EmptyCell;
        }

        public void Draw(SpriteBatch spriteBatch) { }

        public Vector2 GetAbsolutPosition()
        {
            return Vector2.Zero;
        }

        public void AddAgent(Agent agent) { }

        public void RemoveAgent(Agent agent) { }
    }
}
