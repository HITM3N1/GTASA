using GTASA.SymulationGeneric.Groups;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class Pavment : Cell
    {
        private Vector2 cords;
        private CellType type;
        public Dictionary<int, Pavment> pavmentsNearby;

        private bool isBuildingEntrace;
        private (int, Building) entrace;

        private List<Agent> agentsInside;

        private TextureType textureType;
        public Pavment(Vector2 cords)
        {
            this.cords = cords;
            this.type = CellType.Pavment;
            this.pavmentsNearby = new Dictionary<int, Pavment>();
            this.isBuildingEntrace = false;
            this.agentsInside = new List<Agent>();

            int r = Essentials.random.Next(0, 100);

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


        public List<Agent> GetAgents()
        {
            return agentsInside;
        }

        public void AddAgent(Agent agent)
        {
            agentsInside.Add(agent);
        }

        public void RemoveAgent(Agent agent)
        {
            agentsInside.Remove(agent);
        }

        public Vector2 GetSpawnAbsolutePosition()
        {
            return GetAbsolutPosition();
        }

        public Vector2 GetAbsolutPosition()
        {
            return new Vector2(cords.X * 16 , cords.Y * 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesPavment[textureType], GetAbsolutPosition(), Color.White);
        }

        public CellType GetCellType()
        {
            return type;
        }

        public bool IsBuildingEntrace()
        {
            return isBuildingEntrace;
        }

        public void SetBuildingEntrace(int directionFromPavmentToBuilding, Building building)
        {
            entrace = (directionFromPavmentToBuilding, building);
            isBuildingEntrace = true;
        }

        public void SetNeighbor(int direction ,Pavment cell)
        {
            pavmentsNearby[direction] = cell;
        }

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
