using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Boards.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using GTASA.SymulationGeneric;
using GTASA.SymulationGeneric.Groups;

namespace GTASA.SymulationGeneric.Groups.Agents
{
    //**************************************************************************************//
    //***** Klasa : Agent                                                              *****//
    //**************************************************************************************//
    //***** Odpowiada za wszystkich agentów poruszających się po planszy               *****//
    //**************************************************************************************//

    public class Agent
    {
        // Grupa do której należy Agent
        GroupAbstract group;

        // Tablica po której Agent się porusza
        Board board;

        // Komórka w której Agent jest w tym momencie
        Cell cell;

        // Absolutna pozycja agenta w oknie
        Vector2 absolutPosition;

        // Kolejka kolejnych wektorów tworzących scieżkę do celu agenta
        Queue<Vector2> targetPath;

        // kierunek poruszanie się agenta
        int? direction;

        // szybkość poruszania się agenta
        float speed;

        // sprawdza czy agent jest w trakcie poruszania się między komórkami
        bool isMoving;

        // sprawdza czy agent jest aktualnie w stanie się swobodnie poruszać
        bool canBeMoved;

        // sprawdza czy agent jest rekrutowany
        bool isBeingRecruted;

        // nowa grupa do której agent zostanie przypisany po rekrutacji
        GroupAbstract newOccupation;

        // licznik od czasu rozpoczęcia rekrutacji
        float recrutationTimmer;

        // liczni od czssu rozpoczecie przemieszczanie się o kratkę
        float moveTimer;

        // startowa pozycja agenta przed ruchem
        Vector2 startPosition;

        // pojedyńczy aktualny cel agenta
        Vector2 targetPosition;


        // Konsturktor - ustawia wszystki potrzebne wartości
        public Agent(GroupAbstract group, Board board, Cell spawnCell)
        {
            this.group = group;
            this.board = board;
            this.cell = spawnCell;
            this.absolutPosition = spawnCell is Building ? ((Building)spawnCell).GetSpawnAbsolutePosition() : spawnCell.GetAbsolutPosition();
            this.targetPath = new Queue<Vector2>();
            this.direction = null;
            
            if(Essentials.GroupSettings.RANDOMIZE_SPEED_OF_EACH_AGNET)
            {
                this.speed = ((Essentials.RANDOM.Next( (int)((0 - Essentials.GroupSettings.BASE_SPEED) * 100), (int)(Essentials.GroupSettings.BASE_SPEED * 100)) / 200f + Essentials.GroupSettings.BASE_SPEED) );
            }
            else
            {
                this.speed = Essentials.GroupSettings.BASE_SPEED;
            }
            this.speed *= group.GetSpeedModifier();
            this.isMoving = false;
            this.canBeMoved = true;
            this.moveTimer = 0f;
            this.isBeingRecruted = false;
            this.recrutationTimmer = 0f;
        }

        // Update - funckja aktualizuje stan agenta i przemieszcza go odpowiednio do ustawionego celu.
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;



            if (isBeingRecruted)
            {
                if (recrutationTimmer == 0f)
                {
                    recrutationTimmer = (float)gameTime.TotalGameTime.TotalSeconds;

                    if (group != newOccupation)
                    {
                        newOccupation.AddAgent(this);
                    }
                }
                else
                {
                    if ((float)gameTime.TotalGameTime.TotalSeconds - recrutationTimmer > Essentials.GroupSettings.TIME_TO_RECRUTE)
                    {
                        if (group is Citizens && newOccupation is Gang gang)// sprwadzanie który gang przejął mieszkańca i zliczamy
                        {
                            SimulationStats.RegisterCitizenTakeover(gang.GetGangID()); 
                        }

                        group = newOccupation;
                        UnlockAgent();
                    }
                }
            }
            else
            {
                if (targetPath.Count() == 0 && canBeMoved && !isMoving)//gdy nie ma celu losowo/domyślnnie sie porusza
                {
                    Wander();
                }


                if (!isMoving && targetPath.Count() > 0)
                {
                    startPosition = absolutPosition;
                    targetPosition = targetPath.Dequeue();
                    moveTimer = 0f;
                    isMoving = true;
                    canBeMoved = false;
                }

                if (isMoving)
                {
                    moveTimer += dt;

                    float t = moveTimer / speed;

                    if (t >= 1f)
                    {
                        absolutPosition = targetPosition;

                        if (cell != board.GetCell(absolutPosition))
                        {
                            cell.RemoveAgent(this);
                            cell = board.GetCell(absolutPosition);
                            cell.AddAgent(this);
                        }

                        isMoving = false;
                        moveTimer = 0f;

                        if (targetPath.Count() == 0)
                        {
                            canBeMoved = true;
                        }
                    }
                    else
                    {
                        absolutPosition = Vector2.Lerp(startPosition, targetPosition, t);
                    }
                }
            }
        }


        // Draw - funkcja rysuje agenta z odpowiednia dla niego teksturą
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.TEXTURES_BUILDING[(TextureType.A, group.GetColor())], absolutPosition, Color.White);
        }


        // GetGroup - zwraca grupę do której należy agent
        public GroupAbstract GetGroup()
        {
            return group;
        }

        // CanBeMoved - sprawdza czy Agent może zostać ruszony 
        public bool CanBeMoved()
        {
            return canBeMoved;
        }


        // SetTargetPath - ustawia sieżkę celów
        public void SetTargetPath(Queue<Vector2> targetPath)
        {
            this.targetPath = targetPath;
        }


        // LockAgent - zamraża agenta na czas rekrutowania
        public void LockAgent(GroupAbstract occup)
        {
            newOccupation = occup;
            isBeingRecruted = true;
        }

        // IsBeingRecruted - sprawdza czy agent jest rekrutowany
        public bool IsBeingRecruted()
        {
            return isBeingRecruted;
        }

        // UnlockAgent - odblokowuje agenta po rekrutacji
        public void UnlockAgent()
        {
            isBeingRecruted = false;
            recrutationTimmer = 0f;
        }


        // TryRecrute - poszukje celu do rekrutacji i próbuje go zrekrótować jeżeli cel znajduje się w odpowiednim dystansie
        public void TryRecrute(Pavment actuallCell)
        {
            if (group is Police || group is Citizens)
                return;

            List<Agent> agentsNearby = new List<Agent>(actuallCell.GetAgents());

            foreach (var pavment in actuallCell.pavmentsNearby)
            {
                agentsNearby.AddRange(pavment.Value.GetAgents());
            }

            List<Agent> citizensNearby = new List<Agent>();

            foreach (Agent agent in agentsNearby)
            {
                if (agent.GetGroup() is Police)
                {
                    return;
                }

                if (agent.GetGroup() is Citizens && !agent.IsBeingRecruted())
                {
                    citizensNearby.Add(agent);
                }
            }

            if (citizensNearby.Count > 0)
            {
                int i = Essentials.RANDOM.Next(0, 100);

                if (i < Essentials.GroupSettings.RECRUTATION_CHANCE)
                {
                    Agent recrute = citizensNearby[Essentials.RANDOM.Next(0, citizensNearby.Count)];

                    recrute.LockAgent(group);
                    LockAgent(group);
                }
            }
        }

        
        // Wander - gdy agent nie posiada celu podróży funkcja ta odpowiada za jego losowe poruszanie się 
        public void Wander()
        {
            if (cell.GetCellType() == CellType.Pavment)
            {
                Pavment actuallCell = (Pavment)cell;

                TryRecrute(actuallCell);

                if (direction == null)
                {
                    int i = 0;
                    while (!actuallCell.pavmentsNearby.ContainsKey(i))
                    {
                        i++;
                    }
                    direction = i;
                    targetPath.Enqueue(actuallCell.pavmentsNearby[direction.Value].GetAbsolutPosition());
                }
                else
                {
                    if (actuallCell.pavmentsNearby.Count() == 1)
                    {
                        if (!actuallCell.pavmentsNearby.ContainsKey(direction.Value))
                        {
                            direction = Math.Abs(direction.Value + 2) % 4;
                        }

                        targetPath.Enqueue(actuallCell.pavmentsNearby[direction.Value].GetAbsolutPosition());

                    }

                    if (actuallCell.pavmentsNearby.Count() == 4)
                    {
                        int i = Essentials.RANDOM.Next(1, 6);

                        if (i == 1)
                        {
                            direction = Math.Abs(direction.Value + 1) % 4;
                        }

                        if (i == 2)
                        {
                            direction = Math.Abs(direction.Value - 1) % 4;
                        }

                        targetPath.Enqueue(actuallCell.pavmentsNearby[direction.Value].GetAbsolutPosition());
                    }

                    if (actuallCell.pavmentsNearby.Count() == 2)
                    {
                        targetPath.Enqueue(actuallCell.pavmentsNearby[direction.Value].GetAbsolutPosition());
                    }
                }
            }
            else if (cell.GetCellType() == CellType.Building)
            {
                Building building = (Building)cell;

                if (building.IsAttackEnded())
                {
                    if (absolutPosition == building.GetExitPos1())
                    {
                        targetPath.Enqueue(building.GetExitPos2());
                    }
                    else
                    {
                        targetPath.Enqueue(building.GetExitPos1());
                    }

                    direction = null;
                }
                else
                {
                    int x = (Essentials.RANDOM.Next(0, 2) == 0 ? 2 : -2) * Essentials.RANDOM.Next(0, 2);
                    int y = x == 0 ? (Essentials.RANDOM.Next(0, 2) == 0 ? 2 : -2) : 0;

                    Vector2 target = Vector2.Clamp(new Vector2(x + absolutPosition.X, y + absolutPosition.Y), new Vector2(building.bounds.X * 16, building.bounds.Y * 16), new Vector2((building.bounds.Width - 1) * 16, (building.bounds.Height - 1) * 16));

                    targetPath.Enqueue(target);
                }

            }
        }
    }
}



