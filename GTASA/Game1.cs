using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; // przeznaczone głownie na windows
using GTASA.SymulationGeneric;

namespace GTASA // głowna klasa Monogame zarządzajaca oknem rysowaniem i pętlą (game1.cs)
{
    public class Game1 : Game 
    {
        private GraphicsDeviceManager _graphics; //ustawia okno gry
        private SpriteBatch _spriteBatch; //słuzy do rysowania tekstur
        private Symulation symulation; // przewowuje cała logikę gry

        public Game1()   // Game1 nie zna szczegółów mapy, gangów ani agentów, on tylko aktulaizuje i rysuje
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // widocznosc myszki w grze wł/wył

            _graphics.IsFullScreen = false; // full scren wł/wył
            _graphics.PreferredBackBufferWidth = Essentials.mapSize * Essentials.cellSize * (int)Essentials.RENDER_ZOOM; // rozmiar mapy x rozmiar kafelka x zoom - szerokosc
            _graphics.PreferredBackBufferHeight = Essentials.mapSize * Essentials.cellSize * (int)Essentials.RENDER_ZOOM;  //// rozmiar mapy x rozmiar kafelka x zoom - wysokosć
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

        protected override void Update(GameTime gameTime) // akutalizacja symulaci co klatke
        {
            symulation.Update(gameTime);

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
