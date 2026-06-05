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
        private Vector2 nextTileCenter;
        private float speed = 60f;
        private float fleeSpeed = 130f;
        private Random rng;
        Board board;

        public Agent(GroupAbstract group, Vector2 startPosition) 
        { 
            this.group = group;
            this.position = startPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.A, group.GetColor())], position, Color.White);
        }

        public void Wander(float dt)
        {
            if (Vector2.Distance(position, nextTileCenter) < 2f)
            {
                List<Vector2> neighbors = board.GetNeighborPavements(position);
                if (neighbors.Count == 0) return;
                nextTileCenter = neighbors[rng.Next(neighbors.Count)];
            }
            MoveTowards(nextTileCenter, speed, dt);
        }

        public void Flee(Vector2 gangPosition, float dt)
        {
            if (Vector2.Distance(position, nextTileCenter) < 2f)
            {
                List<Vector2> neighbors = board.GetNeighborPavements(position);
                if (neighbors.Count == 0) return;
                nextTileCenter = neighbors.OrderByDescending(n => Vector2.Distance(n, gangPosition)).First();
            }

            MoveTowards(nextTileCenter, fleeSpeed, dt);
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
