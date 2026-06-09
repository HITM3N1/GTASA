using GTASA.SymulationGeneric.Groups;
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using System.Collections.Generic;
using System.Reflection;


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

        private bool isAttackStarted;

        private bool isUnderAttack;

        private double timeWhenOcccupationStarted;

        private bool isPoliceInside;

        public Building(Vector2 cords, Rectangle bounds, GroupAbstract occupation)
        {
            this.cords = cords;
            this.type = CellType.Building;
            this.occupation = occupation;
            this.hasExit = false;
            this.agentsInside = new List<Agent>();
            this.bounds = bounds;
            this.isUnderAttack = false;
            this.isPoliceInside = false;
            this.timeWhenOcccupationStarted = 0;
        }

        public void CallAgents(int k, Group group)
        {
            exit.Item2.CallAgents(k, group, GetEntryPath(), new HashSet<Pavment>());
        }

        public void StartAttack()
        {
            isAttackStarted = true;
        }

        public bool IsPoliceInside()
        {
            return isPoliceInside;
        }

        public bool IsAttackEnded()
        {
            return !isAttackStarted;
        }

        public void Update(GameTime gameTime)
        {
            if(isAttackStarted)
            {
                if (agentsInside.Count > 0)
                {
                    isUnderAttack = true;
                }
            }
            

            if(isUnderAttack)
            {
                HashSet<GroupAbstract> groupsInside = new HashSet<GroupAbstract>();
                foreach (Agent agent in agentsInside)
                {
                    groupsInside.Add(agent.GetGroup());
                    if(agent.GetGroup() is Police)
                    {
                        isPoliceInside = true;
                    }
                }

                if (timeWhenOcccupationStarted == 0)
                {
                    timeWhenOcccupationStarted = gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    if (gameTime.TotalGameTime.TotalSeconds - timeWhenOcccupationStarted >= Essentials.GroupSettings.timeToOccupyBuilding)
                    {
                        Dictionary<GroupAbstract, List<Agent>> GangMembersCountInside = new Dictionary<GroupAbstract, List<Agent>>();

                        foreach(GroupAbstract group  in groupsInside)
                        {
                            GangMembersCountInside[group] = new List<Agent>();
                        }

                        foreach (Agent agent in agentsInside)
                        {
                            GangMembersCountInside[agent.GetGroup()].Add(agent);
                        }

                        int max = -1;
                        GroupAbstract winner = null;

                        foreach(var pair in GangMembersCountInside)
                        {
                            if(pair.Value.Count > max)
                            {
                                max = pair.Value.Count;
                                winner = pair.Key;
                            }
                        }

                        foreach(GroupAbstract groupAbstract in groupsInside)
                        {
                            if(groupAbstract is Police)
                            {
                                winner = groupAbstract;
                            }
                        }

                        foreach (var pair in GangMembersCountInside)
                        {
                            if (pair.Key != winner)
                            {
                                foreach(Agent agent in pair.Value)
                                {
                                    pair.Key.RemoveAgent(agent);
                                    agentsInside.Remove(agent);
                                }
                            }
                        }


                        occupation = winner;
                        isUnderAttack = false;
                        isAttackStarted = false;
                        timeWhenOcccupationStarted = 0;
                    }

                }


            }
        }


        public bool IsUnderAttack()
        {
            return this.isUnderAttack;
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

        public Vector2 GetExitPos1()
        {
            return exitAbsoultPosition;
        }

        public Vector2 GetExitPos2()
        {
            return exit.Item2.GetAbsolutPosition();
        }

        public Queue<Vector2> GetEntryPath()
        {
            Queue<Vector2> path = new Queue<Vector2>();

            
            path.Enqueue(exitAbsoultPosition);
            path.Enqueue(GetSpawnAbsolutePosition());

            return path;
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
