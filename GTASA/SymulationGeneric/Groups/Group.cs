using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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

      
        protected Group(GroupColor color, int agentcout, Board board)
        {
            agents = new List<Agent>();
            this.color = color;
            this.agentCount = agentcout;
            this.board = board;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Agent agent in agents)
            {
                agent.Draw(spriteBatch);
            }
        }
    }

    public class Police : Group
    {

        public Police(int agentCout, Board board) : base(Essentials.PoliceColor, agentCout, board) 
        {
            



           
        }
    }

    public class Citizens : Group
    {
        public Citizens(int agentCout, Board board) : base(Essentials.CitizensColor, agentCout, board)
        {


        }

        public void Initialize(Board board)
        {
            this.board = board;
            for (int i = 0; i < agentCount; i++)
            {
                agents.Add(new Agent(this, board.GetRandomPavment()));
            }
        }
    }

    public class Gang : Group
    {
        private Vector2 groupBase;

        public Gang(int agentCout, GroupColor color, Board board ) : base(color, agentCout, board)
        {
           
        }

        public void Initialize()
        {
            groupBase = board.GetFreeBuilding(this);

            for (int i = 0; i < agentCount; i++)
            {
                agents.Add(new Agent(this, groupBase));
            }
        }
    }
}
