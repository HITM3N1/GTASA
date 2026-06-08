using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GTASA.SymulationGeneric;

namespace GTASA
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Symulation symulation;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Essentials.mapSize * 16 * (int)Essentials.RENDER_ZOOM;
            _graphics.PreferredBackBufferHeight = Essentials.mapSize * 16 * (int)Essentials.RENDER_ZOOM;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            symulation = new Symulation();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            symulation.LoadContent(this.Content);

        }

        protected override void Update(GameTime gameTime)
        {
            symulation.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            symulation.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
