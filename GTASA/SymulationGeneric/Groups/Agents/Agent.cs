
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTASA.SymulationGeneric.Groups.Agents
{
    public class Agent
    {
        GroupAbstract group;
        Vector2 position;

        public Agent(GroupAbstract group, Vector2 startPosition) 
        { 
            this.group = group;
            this.position = startPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Essentials.texturesBuilding[(TextureType.A, group.GetColor())], position, Color.White);
        }
    }
}
