using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GTASA.SymulationGeneric.Boards;

namespace GTASA.SymulationGeneric.Groups.Agents
{
    public class Agent
    {
        GroupAbstract group;
        Vector2 position;
        private Vector2 nextCellCenter;
        private float speed = Essentials.speed;
        private float fleeSpeed = Essentials.fleespeed;
        private int currentDirection = 0; // 0: left, 1: up, 2: right, 3: down
        private Random rng;
        Board board;

        public Agent(GroupAbstract group, Vector2 startPosition, Board board) 
        {
            System.Diagnostics.Debug.WriteLine($"startPosition: {startPosition}");
            this.group = group;
            this.position = startPosition;
            this.nextCellCenter = startPosition;
            this.board = board;
            this.rng = new Random();
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.A, group.GetColor())], position, Color.White);
        }

        public void Wander(float dt)
        {

            if (Vector2.Distance(position, nextCellCenter) < 2f)
            {
                position = nextCellCenter;
                Vector2? straight = board.GetNeighborInDirection(position, currentDirection);
                if (straight.HasValue && rng.Next(4) != 0)
                {
                    nextCellCenter = straight.Value;
                }
                else
                {
                    int oppositeDirection = (currentDirection + 2) % 4;
                    List<int> available = new List<int>();

                    for (int i = 0; i < 4; i++)
                    {
                        if (i == oppositeDirection) continue;

                        Vector2? neighbor = board.GetNeighborInDirection(position, i);
                        if (neighbor.HasValue)
                        {
                            available.Add(i);
                        }
                    }

                    if (available.Count > 0)
                    {
                        currentDirection = available[rng.Next(available.Count)];
                        nextCellCenter = board.GetNeighborInDirection(position, currentDirection)!.Value;
                    }
                    else
                    {
                        currentDirection = oppositeDirection;
                        Vector2? back = board.GetNeighborInDirection(position, currentDirection);
                        if (back.HasValue)
                        {
                            nextCellCenter = back.Value;
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2? any = board.GetNeighborInDirection(position, i);
                                if (any.HasValue)
                                {
                                    currentDirection = i;
                                    nextCellCenter = any.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
               
            }

            MoveTowards(nextCellCenter, speed, dt);
        }

        public void Flee(Vector2 gangPosition, float dt)
        {
            if (Vector2.Distance(position, nextCellCenter) < 2f)
            {
                List<int> available = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    Vector2? neighbor = board.GetNeighborInDirection(position, i);
                    if (neighbor.HasValue)
                    {
                        available.Add(i);
                    }
                }

                if (available.Count > 0)
                {
                    currentDirection = available.OrderByDescending(i => Vector2.Distance(board.GetNeighborInDirection(position, i)!.Value, gangPosition)).First();

                    nextCellCenter = board.GetNeighborInDirection(position, currentDirection)!.Value;
                }
            }

            MoveTowards(nextCellCenter, fleeSpeed, dt);
        }

        private void MoveTowards(Vector2 target, float speed, float dt)
        {
            Vector2 dir = target - position;
            if (dir.LengthSquared() < 0.01f) return;
            dir.Normalize();
            position += dir * speed * dt;
        }
       
    }
}
