using GTASA.SymulationGeneric;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GTASA 
{
    public class Game1 : Game 
    {
        private GraphicsDeviceManager _graphics; //ustawia okno gry
        private SpriteBatch _spriteBatch; //słuzy do rysowania tekstur
        private Symulation symulation; // przechowuje cała logikę gry
        private bool hasShownResults = false;

        RenderTarget2D plane;
        Rectangle plane_offset;
        public Game1()   // Game1 nie zna szczegółów mapy, gangów ani agentów, on tylko aktulaizuje i rysuje
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // widocznosc myszki w grze wł/wył

            CalculateWindowUltra();
        }

        protected override void Initialize() // tworzymi obiekt symulation
        {
            symulation = new Symulation();

            base.Initialize();

            plane = new RenderTarget2D(GraphicsDevice, Essentials.MAP_SIZE * Essentials.CELL_SIZE, Essentials.MAP_SIZE * Essentials.CELL_SIZE);
        }

        protected override void LoadContent() // ładujemy tekstury
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            symulation.LoadContent(this.Content);

        }

        protected override void Update(GameTime gameTime) // akutalizacja symulacji co klatke
        {
            symulation.Update(gameTime);
            if (symulation.IsFinished() && !hasShownResults)
            {
                hasShownResults = true;

                System.Windows.Forms.MessageBox.Show(
                    symulation.GetFinalResultText(),
                    "Koniec symulacji"
                );

                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                Essentials.ULTRA_UI_Key = true; 
            }

            if(Essentials.ULTRA_UI_Key && Keyboard.GetState().IsKeyUp(Keys.G))
            {
                Essentials.ULTRA_UI = !Essentials.ULTRA_UI;
                Essentials.ULTRA_UI_Key = false;
                if (Essentials.ULTRA_UI)
                {
                    CalculateWindowUltra();
                }
                else
                {
                    CalculateWindowBasic();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // rysowanie agentów i mapy co klatkę
        {
            GraphicsDevice.SetRenderTarget(plane);

            symulation.Draw(_spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(Essentials.background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
           
            _spriteBatch.Draw(plane, plane_offset, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void CalculateWindowUltra()
        {
            int scale = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 200) / 9;
            float plane_scale = 7f * scale / (float)(Essentials.MAP_SIZE * Essentials.CELL_SIZE);

            plane_offset = new Rectangle(8*scale, scale, 7*scale, 7* scale);
            _graphics.PreferredBackBufferHeight = scale * 9;
            _graphics.PreferredBackBufferWidth = scale * 16;
            _graphics.ApplyChanges();
        }

        void CalculateWindowBasic()
        {
            int scale = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 200) / (Essentials.MAP_SIZE* Essentials.CELL_SIZE);
            int plane_scale = scale * Essentials.MAP_SIZE * Essentials.CELL_SIZE;

            plane_offset = new Rectangle(0, 0, plane_scale, plane_scale);
            _graphics.PreferredBackBufferHeight = plane_scale;
            _graphics.PreferredBackBufferWidth = plane_scale;
            _graphics.ApplyChanges();

        }
    }
}