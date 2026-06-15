using GTASA.SymulationGeneric.Groups;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GTASA.SymulationGeneric.Boards.Cells
{

    //**************************************************************************************//
    //***** Klasa : Pavment - dziedziczy po Cell                                       *****//
    //**************************************************************************************//
    //***** Jedna z trzech rodzajów komórek dostępnych w symulacji, jest to komórka    *****//
    //***** odpowiadająca za logikę wszystich chodników na mapie                       *****//
    //**************************************************************************************//

    public class Pavment : Cell
    {
        // cords - przetrzymuje wekotr który jest koordynatami konkretnego chodnika
        private Vector2 cords;

        // type - przechowuje typ komórki
        private CellType type;

        // pavmentsNearby - przechowuje sąsiednie komórki w zależności od kierunku
        public Dictionary<int, Pavment> pavmentsNearby;

        // isBuildingEntrace - sprawdza czy z chodnika da się wejść do budynku 
        private bool isBuildingEntrace;

        // agentsInside - przechowuje agentów w środku
        private List<Agent> agentsInside;

        // textureType - przechowuje rodzaj tekstury chodnika
        private TextureType textureType;

        // Konstruktor - przypisujący podstawowe wartości przy tworzeniu
        public Pavment(Vector2 cords)
        {
            this.cords = cords;
            this.type = CellType.Pavment;
            this.pavmentsNearby = new Dictionary<int, Pavment>();
            this.isBuildingEntrace = false;
            this.agentsInside = new List<Agent>();

            // Losowanie konretnego typu tekstury chodnika 
            int r = Essentials.RANDOM.Next(0, 100);

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

        // Draw - rysuje teksture chodnika
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.TEXTURES_PAVMENT[textureType], GetAbsolutPosition(), Color.White);
        }


        // GetAgents - zwraca listę agentów w środku komórki
        public List<Agent> GetAgents()
        {
            return agentsInside;
        }

        // AddAgent - dodaje agenta do listy agentów w środku
        public void AddAgent(Agent agent)
        {
            agentsInside.Add(agent);
        }

        // RemoveAgent - usuwa agent z listy agentów w środku
        public void RemoveAgent(Agent agent)
        {
            agentsInside.Remove(agent);
        }

        // GetAbsolutPosition - zwraca absolutna pozycje komórki
        public Vector2 GetAbsolutPosition()
        {
            return new Vector2(cords.X * 16 , cords.Y * 16);
        }


        //GetCellType - zwraca typ komórki
        public CellType GetCellType()
        {
            return type;
        }


        //IsBuildingEntrace - sprawdza czy jest wejście do budynku
        public bool IsBuildingEntrace()
        {
            return isBuildingEntrace;
        }

        //SetNeighbor - ustawia sąsiadów komórki
        public void SetNeighbor(int direction ,Pavment cell)
        {
            pavmentsNearby[direction] = cell;
        }


        // CallAgents - przywołuje agentów konretnej grupy w konretnym dystansie do siebie (algorymt Disktry)
        public void CallAgents(int k, Group group, Queue<Vector2> pathToTarget, HashSet<Pavment> visited)
        {
            foreach (Agent agent in agentsInside)
            {
                if (group.GetAgents().Contains(agent))
                {
                    if(agent.CanBeMoved())
                    {
                        agent.SetTargetPath(new Queue<Vector2>(pathToTarget));
                    }
                }
                   
            }

            if (k <= 0) return;

            visited.Add(this);

            for (int direction = 0; direction <= 3; direction++)
            {
                if (!pavmentsNearby.ContainsKey(direction)) continue;

                Pavment neighbor = pavmentsNearby[direction];
                if (visited.Contains(neighbor)) continue;

                Queue<Vector2> extendedPath = new Queue<Vector2>();
                extendedPath.Enqueue(GetAbsolutPosition());
                foreach (Vector2 step in pathToTarget)
                    extendedPath.Enqueue(step);

                neighbor.CallAgents(k - 1, group, extendedPath, visited);
            }
        }

    }
}
