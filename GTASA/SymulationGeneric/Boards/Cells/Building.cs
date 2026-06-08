using GTASA.SymulationGeneric.Groups;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using System.Collections.Generic;


namespace GTASA.SymulationGeneric.Boards.Cells
{
    public class Building : Cell
    {
        private Vector2 cords;
        private CellType type;
        public Rectangle bounds;

        private (int, Pavment) exit;
        private bool hasExit;

        private Vector2 exitAbsoultPosition;

        private List<Agent> agentsInside;

        private GroupAbstract occupation;

        public Building(Vector2 cords, Rectangle bounds, GroupAbstract occupation)
        {
            this.cords = cords;
            this.type = CellType.Building;
            this.occupation = occupation;
            this.hasExit = false;
            this.agentsInside = new List<Agent>();
            this.bounds = bounds;
        }

        
        public GroupAbstract GetOccupation()
        {
            return occupation;
        }


        public bool IsExitSet()
        {
            return hasExit;
        }

        public void SetExit(int directionFromPavmentToBuilding , Pavment pavment)
        {
            exitAbsoultPosition = pavment.GetAbsolutPosition() + Essentials.direction[directionFromPavmentToBuilding] * 16;

            exit = ((directionFromPavmentToBuilding + 2) % 4,  pavment);
            hasExit = true;
        }

        public CellType GetCellType()
        {
            return type;
        }

        public bool Isfree()
        {
            return occupation == null;
        }

        public void SetOccupation(Group group)
        {
            this.occupation = group;
        }

        public Vector2 GetSpawnAbsolutePosition()
        {
            return new Vector2 ((bounds.Width - bounds.X - 1) * 8 + (bounds.X * 16), (bounds.Height - bounds.Y - 1) * 8 + (bounds.Y * 16));
        }

        public Vector2 GetAbsolutPosition()
        {
            return new Vector2 (bounds.X*16, bounds.Y*16);
        }

        public void Update()
        {

        }

        public void AddAgent(Agent agent)
        {
            agentsInside.Add(agent);
        }

        public void RemoveAgent(Agent agent)
        {
            agentsInside.Remove(agent);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GroupColor color = occupation != null ? occupation.GetColor() : GroupColor.White;

            for (int x = bounds.X; x < bounds.Width; x++)
            {
                for (int y = bounds.Y; y < bounds.Height; y++)
                {
                    if (x > bounds.X && y > bounds.Y && x < bounds.Width - 1 && y < bounds.Height - 1)
                    {
                        spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B0, color)], new Vector2(16 * x, 16 * y), Color.White);
                    }
                    else
                    {
                        if (x == bounds.X)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B8, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == (bounds.Width - 1))
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B4, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B2, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (y == (bounds.Height - 1))
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B6, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }




                        if (x == bounds.X && y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B1, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.Width - 1 && y == bounds.Y)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B3, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.Width - 1 && y == bounds.Height - 1)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B5, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }

                        if (x == bounds.X && y == bounds.Height - 1)
                        {
                            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.B7, color)], new Vector2(16 * x, 16 * y), Color.White);
                        }
                        
                    }
                }
            }


            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.D, color)], exitAbsoultPosition, Color.White);

        }
    }
}
