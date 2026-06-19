using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
using GTASA.SymulationGeneric;

namespace GTASA 
{
    public class Game1 : Game 
    {
        private GraphicsDeviceManager _graphics; //ustawia okno gry
        private SpriteBatch _spriteBatch; //słuzy do rysowania tekstur
        private Symulation symulation; // przechowuje cała logikę gry
        private bool hasShownResults = false;

        public Game1()   // Game1 nie zna szczegółów mapy, gangów ani agentów, on tylko aktulaizuje i rysuje
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // widocznosc myszki w grze wł/wył

            _graphics.IsFullScreen = false; // full scren wł/wył
            _graphics.PreferredBackBufferWidth = Essentials.MAP_SIZE * Essentials.MAP_SIZE * (int)Essentials.RENDER_ZOOM; // rozmiar mapy x rozmiar kafelka x zoom - szerokosc
            _graphics.PreferredBackBufferHeight = Essentials.MAP_SIZE * Essentials.MAP_SIZE * (int)Essentials.RENDER_ZOOM;  //// rozmiar mapy x rozmiar kafelka x zoom - wysokosć
            _graphics.ApplyChanges();
        }

        protected override void Initialize() // tworzymi obiekt symulation
        {
            symulation = new Symulation();

            base.Initialize();
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // rysowanie agentów i mapy co klatkę
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            symulation.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}