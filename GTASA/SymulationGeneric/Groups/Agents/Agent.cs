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

            System.Diagnostics.Debug.WriteLine($"position: {position}, nextTileCenter: {nextCellCenter}");

            if (Vector2.Distance(position, nextCellCenter) < 2f)
            {
                List<Vector2> neighbors = board.GetNeighborPavements(position);
                
                if (neighbors.Count == 0) return;
                nextCellCenter = neighbors[rng.Next(neighbors.Count)];
               
            }

            MoveTowards(nextCellCenter, speed, dt);
        }

        public void Flee(Vector2 gangPosition, float dt)
        {
            if (Vector2.Distance(position, nextCellCenter) < 2f)
            {
                List<Vector2> neighbors = board.GetNeighborPavements(position);
                if (neighbors.Count == 0) return;
                nextCellCenter = neighbors.OrderByDescending(n => Vector2.Distance(n, gangPosition)).First();
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
