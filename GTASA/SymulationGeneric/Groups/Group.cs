using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using GTASA.SymulationGeneric;
namespace GTASA.SymulationGeneric.Groups
{
    public interface GroupAbstract
    {
        public GroupColor GetColor();
    }


    public class Group : GroupAbstract
    {
        protected GroupColor color;
        protected int agentCount;
        protected List<Agent> agents;
        protected Board board;

        public GroupColor GetColor() { return color; }

      
        protected Group(GroupColor color, int agentcount, Board board)
        {
            agents = new List<Agent>();
            this.color = color;
            this.agentCount = agentcount;
            this.board = board;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Agent agent in agents)
            {
                agent.Draw(spriteBatch);
            }
        }

        public List<Vector2> GetAgentPositions()
        {
            return agents.Select(a => a.GetPosition()).ToList();
        }
    }

    public class Police : Group
    {

        public Police(int agentCount, Board board) : base(Essentials.PoliceColor, agentCount, board)
        {


        }

        public void Initialize(Board board)
        {
            this.board = board;
            for (int i = 0; i < agentCount; i++)
            {
                agents.Add(new Agent(this, board.GetRandomPavment(), board));
            }
        }
    }

    public class Citizens : Group
    {
        public Citizens(int agentCount, Board board) : base(Essentials.CitizensColor, agentCount, board)
        {


        }

        public void Initialize(Board board)
        {
            this.board = board;
            for (int i = 0; i < agentCount; i++)
            {
                agents.Add(new Agent(this, board.GetRandomPavment(), board));
            }
        }

        public void Update(GameTime gameTime, List<Vector2> gangPositions)
        {
            System.Diagnostics.Debug.WriteLine("Citizens.Update wywołane");

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Agent agent in agents)
            {
                Vector2? nearestGang = FindNearestGang(agent.GetPosition(), gangPositions);
                    if (nearestGang.HasValue)
                        agent.Flee(nearestGang.Value, dt);
                    else
                        agent.Wander(dt);
            }
        }

        private Vector2? FindNearestGang(Vector2 agentPosition, List<Vector2> gangPositions)
        {
            Vector2? nearest = null;
            float minDist = Essentials.fleeDistance;

            foreach (Vector2 gangPos in gangPositions)
            {
                float dist = Vector2.Distance(agentPosition, gangPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = gangPos;
                }
            }
            return nearest;
        }


    }

    public class Gang : Group
    {
        private Vector2 groupBase;

        public Gang(int agentCount, GroupColor color, Board board ) : base(color, agentCount, board)
        {
           
        }

        public void Initialize()
        {
            groupBase = board.GetFreeBuilding(this);

            for (int i = 0; i < agentCount; i++)
            {
                agents.Add(new Agent(this, groupBase, board));
            }
        }
    }
}
