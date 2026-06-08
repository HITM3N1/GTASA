using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric
{
    public class Symulation
    {
        Board tab;

        Citizens citizens;
        Police police;
        List<Gang> gangs;
        Matrix cameraMatrix;

        int startPopulation;
    

        public Symulation() 
        {
            Essentials.Initialize();
            cameraMatrix = Matrix.CreateScale(Essentials.RENDER_ZOOM, Essentials.RENDER_ZOOM, 1f);
            gangs = new List<Gang>();

            tab = new Board(Essentials.mapSize);
            tab.Initialize(citizens);

            citizens = new Citizens(tab);
            tab.SetCitizens(citizens);

            police = new Police(tab);

            startPopulation = Essentials.GroupSettings.PoliceStarMembers + Essentials.GroupSettings.CitizensStarMembers;

            for (int i = 0; i < Essentials.gangsCount; i++)
            {
                startPopulation += Essentials.GroupSettings.GangStarMembers[i];
                gangs.Add(new Gang(i, Essentials.GroupSettings.GangColors[i], tab));
            }


            foreach (Gang gang in gangs)
            {
                gang.Initialize();
            }

            citizens.Initialize(tab);
            police.Initialize(tab);

            
            

        }

        public void LoadContent(ContentManager contentManager)
        {
            Essentials.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            int populationRightNow = citizens.GetAgentCount() + police.GetAgentCount();
            foreach(Gang gang in gangs)
            {
                populationRightNow += gang.GetAgentCount();
            }



            if (populationRightNow < startPopulation && Essentials.GroupSettings.CitizenSpawn)
            {

                citizens.Update(gameTime, startPopulation - populationRightNow);
            }
            else
            {
                citizens.Update(gameTime, 0);
            }

            

            police.Update(gameTime);

            foreach (Gang gang in gangs)
            {
                gang.Update(gameTime);
            }



            tab.Update(gameTime);
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: cameraMatrix);
            tab.Draw(spriteBatch);

            foreach (Gang gang in gangs)
            {
                gang.Draw(spriteBatch);
            }

            citizens.Draw(spriteBatch);
            police.Draw(spriteBatch);
            spriteBatch.End();
        }
    }


    
}
