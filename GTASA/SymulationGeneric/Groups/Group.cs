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
                agents.Add(new Agent(this, board.GetRandomPavment()));
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
                agents.Add(new Agent(this, board.GetRandomPavment()));
            }
        }

        public void Update(GameTime gameTime, List<Vector2> gangPositions)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                agents.Add(new Agent(this, groupBase));
            }
        }
    }
}
