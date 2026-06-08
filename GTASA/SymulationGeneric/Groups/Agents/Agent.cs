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

        float moveTimer;

        Vector2 startPosition;

        Vector2 targetPosition;

        public Agent(GroupAbstract group, Board board, Cell spawnCell)
        {
            this.group = group;
            this.board = board;
            this.cell = spawnCell;
            this.absolutPosition = spawnCell.GetSpawnAbsolutePosition();
            this.targetPath = new Queue<Vector2>();
            this.direction = null;
            this.speed = Essentials.speed;
            this.isMoving = false;
            this.canBeMoved = true;
            this.moveTimer = 0f;

        }

        public void Update(float dt)
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
            }

            if (isMoving)
            {
                moveTimer += dt;

                float t = moveTimer / speed;

                if (t >= 1f)
                {
                    absolutPosition = targetPosition;
                    cell = board.GetCell(absolutPosition);
                    isMoving = false;
                }
                else
                {
                    absolutPosition = Vector2.Lerp(startPosition, targetPosition, t);
                }
            }


           

            

        }

        void SetTargetPath(Queue<Vector2> targetPath)
        {
            this.targetPath = targetPath;
        }

        void LockAgent()
        {
            canBeMoved = false;
        }

        public void Wander()
        {
            Random random = new Random();

            if (cell.GetCellType() == CellType.Pavment)
            {
                Pavment actuallCell = (Pavment)cell;

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
                        int i = random.Next(1, 6);

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

                int x = (random.Next(0, 2) == 0 ? 2 : -2) * random.Next(0,2);
                int y = x == 0 ? (random.Next(0, 2) == 0 ? 2 : -2) : 0;

                Vector2 target = Vector2.Clamp(new Vector2(x + absolutPosition.X, y + absolutPosition.Y), new Vector2(building.bounds.X * 16, building.bounds.Y * 16), new Vector2((building.bounds.Width - 1) * 16, (building.bounds.Height - 1) * 16));

                targetPath.Enqueue(target);
            }
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
       
       
    

