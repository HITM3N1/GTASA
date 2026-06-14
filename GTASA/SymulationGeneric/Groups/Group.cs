using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Boards.Cells;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
namespace GTASA.SymulationGeneric.Groups
{
    public interface GroupAbstract // każda grupa musi mieć kolor, dodawanie agentów, usuwanie agentów, liczbe i ich prędkość
    {
        public GroupColor GetColor();

        public void RemoveAgent(Agent agent);

        public void AddAgent(Agent agent);

        public int GetAgentCount();

        public float GetSpeedModifier();
    }


    public class Group : GroupAbstract // klasa gropu która zawiera wszystkie wspólne rzeczey, beda dziedziczyć po niej police citizens i gang
    {
        protected GroupColor color;
        protected List<Agent> agents;
        protected Board board;
        protected float speedModifier;
        public GroupColor GetColor() { return color; }

        public int GetAgentCount()
        {
            return agents.Count;
        }

        public float GetSpeedModifier()
        {
            return speedModifier;
        }

        public void RemoveAgent(Agent agent)
        {
            agents.Remove(agent);
        }

        public void AddAgent(Agent agent)
        {
            agents.Add(agent);
        }

        protected Group(GroupColor color, Board board, float speedModifier)
        {
            agents = new List<Agent>();
            this.color = color;
            this.board = board;
            this.speedModifier = speedModifier;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Agent agent in agents)
            {
                agent.Draw(spriteBatch);
            }
        }

        public List<Agent> GetAgents()
        {
            return agents;
        }
    }

    public class Police : Group // grupa policji
    {

        public Police(Board board) : base(Essentials.GroupSettings.PoliceColor, board, Essentials.GroupSettings.PoliceSpeedMod)// kontruktor policja wywoulje klase gropu
        {


        }

        public void Initialize(Board board) // tworzenie policjantoów
        {
            this.board = board;
            for (int i = 0; i < Essentials.GroupSettings.PoliceStarMembers; i++)
            {
                agents.Add(new Agent(this, board, board.GetRandomPavment(), Essentials.AgentSettings.PoliceHP, Essentials.AgentSettings.PoliceStrength));
            }
        }

        public void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("Police.Update wywołane");
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            List<Building> buildingsUnderAttack = board.GetAllBuildingsUnderAttack();// szukamy budynku pod atakiem aby zaeragowac i tam pojsc

            if (buildingsUnderAttack.Count > 0)
            {
                foreach (Building building in buildingsUnderAttack)
                {
                    building.CallAgents(Essentials.GroupSettings.PoliceRadiusReaction, this);
                }
            }



            foreach (Agent agent in agents)
            {
                agent.Update(gameTime);
            }
        }
    }

    public class Citizens : Group // grupa obywateli
    {
        public Citizens(Board board) : base(Essentials.GroupSettings.CitizensColor, board, Essentials.GroupSettings.CitiznesSpeedMod)
        {


        }

        public void Initialize(Board board)
        {
            this.board = board;
            for (int i = 0; i < Essentials.GroupSettings.CitizensStarMembers; i++)
            {
                agents.Add(new Agent(this, board, board.GetRandomPavment(), Essentials.AgentSettings.CitizensHP, Essentials.AgentSettings.CitizensStrength));
            }
        }

        public void Update(GameTime gameTime, int count)
        {
            if (count > 0)// dodawanie nowych mieszkańców po zabiciu
            {
                for (int i = 0; i < count; i++)
                {
                    agents.Add(new Agent(this, board, board.GetRandomBorderPavment(), Essentials.AgentSettings.CitizensHP, Essentials.AgentSettings.CitizensStrength));
                }

            }

            System.Diagnostics.Debug.WriteLine("Citizens.Update wywołane");

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Agent agent in agents.ToList()) // usuwanie miszkanca jesli zmienil grupe
            {
                if (agent.GetGroup() != this)
                {
                    agents.Remove(agent);
                }
                else
                {
                    agent.Update(gameTime);
                }
            }
        }

    }

    public class Gang : Group // grupa gangu
    {
        int gangID; // numer gangu

        private Building groupBase;// baza gangu

        private Building test; // cel gangu

        private List<Building> gangBuildings; // lista budynkow nalezacyh do gangu

        public Gang(int gangID, GroupColor color, Board board) : base(color, board, Essentials.GroupSettings.GangSpeedMod[gangID])
        {
            groupBase = board.GetFreeSpawnBuilding(this);
            gangBuildings = board.GetGangBuildings(this);
            test = board.GetRandomFreeBuilding(this);
            test.StartAttack();
            this.gangID = gangID;
        }

        public void Initialize()
        {
            for (int i = 0; i < Essentials.GroupSettings.GangStarMembers[gangID]; i++)
            {
                agents.Add(new Agent(this, board, groupBase, Essentials.AgentSettings.GangMemberHP, Essentials.AgentSettings.GangMemberStrength));
            }
        }

        public void Update(GameTime gameTime)
        {

            gangBuildings = board.GetGangBuildings(this); // odsiwieża budynki gangu


            List<Building> buildingsUnderAttack = CheckIfGangBuildingAreUnderAttack(gangBuildings); // sprwadzanie atakowanych budynkow

            if (buildingsUnderAttack.Count > 0) // jezeli jest w zasiegu to bron budynku
            {
                foreach (Building building in buildingsUnderAttack)
                {


                    building.CallAgents(Essentials.GroupSettings.GangRadiusToDefenceBuilding, this);
                }
            }


            if (!test.IsAttackEnded() && !test.IsPoliceInside()) // kontynuowanie ataku
            {
                test.CallAgents(Essentials.GroupSettings.GangRadiusToAttackBuilding, this);
            }
            else
            {
                test = board.GetRandomFreeBuilding(this);
                if (test.GetOccupation() != this)// w przeciwnym wypadu szukamy nopwego celu
                {
                    test.StartAttack();
                }
            }


            foreach (Agent agent in agents)
            {
                agent.Update(gameTime);
            }
        }





        public List<Building> CheckIfGangBuildingAreUnderAttack(List<Building> gangBuildings)
        {
            List<Building> buildingsUnderAttack = new List<Building>();

            foreach (Building building in gangBuildings)
            {
                if (building.IsUnderAttack())
                {
                    buildingsUnderAttack.Add(building);
                }

            }

            return buildingsUnderAttack;
        }
    }
}