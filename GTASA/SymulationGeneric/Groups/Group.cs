using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Boards.Cells;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
namespace GTASA.SymulationGeneric.Groups
{

    //**************************************************************************************//
    //***** Interfejs : GroupAbstract                                                  *****//
    //**************************************************************************************//
    //***** Definiuję podstawowe wymogi dla  każdej grupy                              *****//
    //**************************************************************************************//
    public interface GroupAbstract 
    {
        // każda grupa musi mieć kolor, dodawanie agentów, usuwanie agentów, liczbe i ich prędkość

        public GroupColor GetColor();

        public void RemoveAgent(Agent agent);

        public void AddAgent(Agent agent);

        public int GetAgentCount();

        public float GetSpeedModifier();
    }


    //**************************************************************************************//
    //***** Klasa : Group - dziedziczy po GroupAbstract                                *****//
    //**************************************************************************************//
    //***** Definiuję wspólną logikę dla wszystkich grup                               *****//
    //**************************************************************************************//

    public class Group : GroupAbstract
    {
        //color - przechowuje kolor którym definiuje się grupa
        protected GroupColor color;

        //agents - przechowuje listę agentów grupy
        protected List<Agent> agents;

        //board - przechowuje tablice na której działa grupa
        protected Board board;

        //speedModifier - przechowuje modyfikator szybkości grupy
        protected float speedModifier;

        protected Group(GroupColor color, Board board, float speedModifier)
        {
            agents = new List<Agent>();
            this.color = color;
            this.board = board;
            this.speedModifier = speedModifier;
        }


        // Draw - wywołuje rysowanie wszystkich agentów grupy
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Agent agent in agents)
            {
                agent.Draw(spriteBatch);
            }
        }


        //--------------------------------
        //      GETTERY I SETTERY
        //--------------------------------

        public GroupColor GetColor() 
        { 
            return color; 
        }

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

        public List<Agent> GetAgents()
        {
            return agents;
        }
    }




    //**************************************************************************************//
    //***** Klasa : Police - dziedziczy po Group                                       *****//
    //**************************************************************************************//
    //***** Definiuję logikę grupy policja                                             *****//
    //**************************************************************************************//

    public class Police : Group 
    {

        public Police(Board board) : base(Essentials.GroupSettings.POLICE_COLOR, board, Essentials.GroupSettings.POLICE_SPEED_MODIFIRE) { } // kontruktor policja wywoulje klase gropu


        //Initialize - Inicjalizacja grupy i tworzenie agentów tej grupy.
        public void Initialize() 
        {
            for (int i = 0; i < Essentials.GroupSettings.POLICE_BASE_AGENT_COUNT; i++)
            {
                agents.Add(new Agent(this, board, board.GetRandomPavment()));
            }
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Szukamy budynku pod atakiem aby zaeragowac i tam pojsc
            List<Building> buildingsUnderAttack = board.GetAllBuildingsUnderAttack();
            
            if (buildingsUnderAttack.Count > 0)
            {
                foreach (Building building in buildingsUnderAttack)
                {
                    building.CallAgents(Essentials.GroupSettings.POLICE_RADIUS_TO_REACTE, this);
                }
            }

            foreach (Agent agent in agents)
            {
                agent.Update(gameTime);
            }
        }
    }



    //**************************************************************************************//
    //***** Klasa : Police - dziedziczy po Group                                       *****//
    //**************************************************************************************//
    //***** Definiuję logikę grupy obywateli                                           *****//
    //**************************************************************************************//

    public class Citizens : Group // grupa obywateli
    {
        public Citizens(Board board) : base(Essentials.GroupSettings.CITIZENS_COLOR, board, Essentials.GroupSettings.CITIZNES_SPEED_MODIFIRE) { }

        //Initialize - Inicjalizacja grupy i tworzenie agentów tej grupy.

        public void Initialize()
        {
            for (int i = 0; i < Essentials.GroupSettings.CITIZENS_BASE_AGENT_COUNT; i++)
            {
                agents.Add(new Agent(this, board, board.GetRandomPavment()));
            }
        }

        public void Update(GameTime gameTime, int count)
        {

            // dodawanie nowych mieszkańców po zabiciu
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    agents.Add(new Agent(this, board, board.GetRandomBorderPavment()));
                }

            }

            System.Diagnostics.Debug.WriteLine("Citizens.Update wywołane");

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Usuwanie miszkanca jesli zmienil grupe
            foreach (Agent agent in agents.ToList()) 
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


    //**************************************************************************************//
    //***** Klasa : Police - dziedziczy po Group                                       *****//
    //**************************************************************************************//
    //***** Definiuję logikę Gangu                                                     *****//
    //**************************************************************************************//

    public class Gang : Group // grupa gangu
    {

        // gangID - uniklany numer gangu
        int gangID;

        // groupBase - budynek będący bazą gangu
        private Building groupBase;

        // target - budynek będący celem gangu
        private Building target;

        // gangBuildings - lista budynków należących do gangu
        private List<Building> gangBuildings; 

        public Gang(int gangID, GroupColor color, Board board) : base(color, board, Essentials.GroupSettings.GANG_SPEED_MODIFIRE[gangID])
        {
            this.gangID = gangID;
        }


        //Initialize - Inicjalizacja grupy, szukanie miejsca na jej siedzibę oraz pierwszego celu, i tworzenie agentów tej grupy.
        public void Initialize()
        {
            groupBase = board.GetFreeSpawnBuilding(this);
            gangBuildings = board.GetGangBuildings(this);
            target = board.GetRandomFreeBuilding(this);
            target.StartAttack();

            for (int i = 0; i < Essentials.GroupSettings.GANGS_BASE_AGENT_COUNT[gangID]; i++)
            {
                agents.Add(new Agent(this, board, groupBase));
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
                    building.CallAgents(Essentials.GroupSettings.GANG_RADIUS_TO_DEFENCE_BUILIDNG, this);
                }
            }


            if (!(target.IsAttackEnded() || target.IsPoliceInside())) // kontynuowanie ataku
            {
                target.CallAgents(Essentials.GroupSettings.GANG_RADIUS_TO_ATTACK_BUILIDNG, this);
            }
            else
            {
                target = board.GetRandomFreeBuilding(this);
                if (target.GetOccupation() != this)// w przeciwnym wypadu szukamy nopwego celu
                {
                    target.StartAttack();
                }
            }


            foreach (Agent agent in agents)
            {
                agent.Update(gameTime);
            }
        }


        //CheckIfGangBuildingAreUnderAttack - sprawdza czy któryś z budynków gangu jest attakowany.
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