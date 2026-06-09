using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Boards.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASA.SymulationGeneric.Groups.Agents
{
    public class Agent
    {
        // Group which Agent belongs to
        GroupAbstract group;

        //Board in which target is moving 
        Board board;

        //Cell in which Agent is right now
        Cell cell;

        //Absolut position of Agent
        Vector2 absolutPosition;

        //Path to the target
        Queue<Vector2> targetPath;

        //Direction in which target is moving
        int? direction;

        //Speed of moving
        float speed;

        //Check if agent is moving form one cell to another
        bool isMoving;

        //Check if agent can get new target or is locked by interaction
        bool canBeMoved;

        bool isBeingRecruted;

        GroupAbstract newOccupation;

        float recrutationTimmer;

        float moveTimer;

        Vector2 startPosition;

        Vector2 targetPosition;

        int hp;

        int strength;


        public Agent(GroupAbstract group, Board board, Cell spawnCell, int hp, int strength)
        {
            this.group = group;
            this.board = board;
            this.cell = spawnCell;
            this.absolutPosition = spawnCell.GetSpawnAbsolutePosition();
            this.targetPath = new Queue<Vector2>();
            this.direction = null;
            
            if(Essentials.GroupSettings.ranodmizeSpeed)
            {
                this.speed = ((Essentials.random.Next( (int)((0 - Essentials.speed) * 100), (int)((Essentials.speed) * 100)) / 200f + Essentials.speed) );
            }
            else
            {
                this.speed = Essentials.speed;
            }
            this.speed *= group.GetSpeedModifier();
            this.isMoving = false;
            this.canBeMoved = true;
            this.moveTimer = 0f;
            this.isBeingRecruted = false;
            this.recrutationTimmer = 0f;
            this.hp = hp;
            this.strength = strength;

            spawnCell.AddAgent(this);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            if(isBeingRecruted)
            {
                if(recrutationTimmer ==  0f)
                {
                    recrutationTimmer = (float)gameTime.TotalGameTime.TotalSeconds;

                    if(group != newOccupation)
                    {
                        newOccupation.AddAgent(this);
                    }
                }
                else
                {
                    if((float)gameTime.TotalGameTime.TotalSeconds - recrutationTimmer > Essentials.GroupSettings.timeToRecrute)
                    {
                        group = newOccupation;
                        UnlockAgent();
                    }
                }
            }
            else
            {
                if (targetPath.Count() == 0 && canBeMoved && !isMoving)
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

        public GroupAbstract GetGroup()
        {
            return group;
        }

        public void SetTargetPath(Queue<Vector2> targetPath)
        {
            this.targetPath = targetPath;
        }

        public void LockAgent(GroupAbstract occup)
        {
            newOccupation = occup;
            isBeingRecruted = true;
        }

        public bool IsBeingRecruted()
        {
            return isBeingRecruted;
        }

        public void UnlockAgent()
        {
            isBeingRecruted = false;
            recrutationTimmer = 0f;
        }

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
                if(agent.GetGroup() is Police)
                {
                    return;
                }

                if(agent.GetGroup() is Citizens && !agent.IsBeingRecruted())
                {
                    citizensNearby.Add(agent);
                }
            }

            if (citizensNearby.Count > 0)
            {
                int i = Essentials.random.Next(0, 100);

                if(i < Essentials.GroupSettings.RecrutationChance)
                {
                    Agent recrute = citizensNearby[Essentials.random.Next(0, citizensNearby.Count)];

                    recrute.LockAgent(group);
                    LockAgent(group);
                }
            }
        }

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
                        if(!actuallCell.pavmentsNearby.ContainsKey(direction.Value))
                        {
                            direction = Math.Abs(direction.Value + 2) % 4;
                        }
                        
                        targetPath.Enqueue(actuallCell.pavmentsNearby[direction.Value].GetAbsolutPosition());

                    }

                    if (actuallCell.pavmentsNearby.Count() == 4)
                    {
                        int i = Essentials.random.Next(1, 6);

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
            else if(cell.GetCellType() == CellType.Building)
            { 
                Building building = (Building)cell;

                if(building.IsAttackEnded())
                {
                    if(absolutPosition == building.GetExitPos1())
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
                    int x = (Essentials.random.Next(0, 2) == 0 ? 2 : -2) * Essentials.random.Next(0, 2);
                    int y = x == 0 ? (Essentials.random.Next(0, 2) == 0 ? 2 : -2) : 0;

                    Vector2 target = Vector2.Clamp(new Vector2(x + absolutPosition.X, y + absolutPosition.Y), new Vector2(building.bounds.X * 16, building.bounds.Y * 16), new Vector2((building.bounds.Width - 1) * 16, (building.bounds.Height - 1) * 16));

                    targetPath.Enqueue(target);
                }
                
            }
        }

        public bool CanBeMoved()
        {
            return canBeMoved;
        }


        public Vector2 GetPosition()
        {
            return absolutPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.A, group.GetColor())], absolutPosition, Color.White);
        }
    }
}
       
       
    

