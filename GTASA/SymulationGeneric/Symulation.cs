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
    

        public Symulation() 
        {
            cameraMatrix = Matrix.CreateScale(Essentials.RENDER_ZOOM, Essentials.RENDER_ZOOM, 1f);
            gangs = new List<Gang>();

            citizens = new Citizens(5, tab);
            tab = new Board(Essentials.mapSize, citizens);
            tab.Initialize();

            
            police = new Police(3, tab);


            gangs.Add(new Gang(2, GroupColor.Red, tab));
            gangs.Add(new Gang(1, GroupColor.Green, tab));


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

        public void Update()
        {
            

            tab.Update();

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
