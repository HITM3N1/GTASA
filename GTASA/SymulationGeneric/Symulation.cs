using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GTASA.SymulationGeneric
{
    //**************************************************************************************//
    //***** Klasa : Symulation                                                         *****//
    //**************************************************************************************//
    //***** Główna klasa symulacji, odpowiada za koordynowanie całego działania        *****//
    //**************************************************************************************//

    public class Symulation
    {
        // _board - Główny obiekt klasy Board, odpowiada za działanie całej tablicy po której poruszają się agenci.
        private Board _board;

        // _citizens - Główny obiekt klasy Citizens, odpowiada za działanie grupy obywateli.
        private Citizens _citizens;

        // _police - Główny obiekt klasy Police, odpowiada za działanie grupy policji.
        private Police _police;

        // _gangs - Lista obiektów klasy Gang, odpowiada za działanie grupy przestępczych.
        private List<Gang> _gangs;

        // startPopulation - początkowa ilość wszystki agentów nazywana też początkową populacją
        private int startPopulation;

         


        
        // Konstruktor - inicjalizuje działanie całej symulacji, tworzy wszystkie potrzebne obiekty oraz ustawia ich wartości początkowe.
        public Symulation()
        {
            Essentials.Initialize(); 
            

            // Tworzenie obiektów klas.
            _gangs = new List<Gang>(); 
            _board = new Board(Essentials.MAP_SIZE);
            _citizens = new Citizens(_board);
            _police = new Police(_board);
             
            // Liczenie populacji startowej
            startPopulation = Essentials.GroupSettings.POLICE_BASE_AGENT_COUNT + Essentials.GroupSettings.CITIZENS_BASE_AGENT_COUNT; 

            for (int i = 0; i < Essentials.GroupSettings.GANGS_COUNT; i++) 
            {
                startPopulation += Essentials.GroupSettings.GANGS_BASE_AGENT_COUNT[i];
                _gangs.Add(new Gang(i, Essentials.GroupSettings.GANGS_COLOR[i], _board));
            }

            //Inicjalizacja klas.
            _board.Initialize(_citizens);

            foreach (Gang gang in _gangs)
            {
                gang.Initialize(); 
            }

            _citizens.Initialize(); 
            _police.Initialize();
        }


        // LoadContent - ładuje tekstury w klasie Essentials
        public void LoadContent(ContentManager contentManager)
        {
            Essentials.LoadContent(contentManager);
        }



        // Update - głowna funkcja logiczna przetwarza wszytkie informacje i wywołuje Update wszystkich obiektów które potrzebują własnego przeliczenia.
        public void Update(GameTime gameTime)
        {
            // zliczanie populacji
            int populationRightNow = _citizens.GetAgentCount() + _police.GetAgentCount();
            foreach (Gang gang in _gangs)
            {
                populationRightNow += gang.GetAgentCount();
            }


            // jezeli Essentials.GroupSettings.CITIZEN_RESPAWN i populacja jest mniejsza niż początkowa dodaje do symulacji nowych obywateli

            if (populationRightNow < startPopulation && Essentials.GroupSettings.CITIZEN_RESPAWN)
            {

                _citizens.Update(gameTime, startPopulation - populationRightNow);
            }
            else
            {
                _citizens.Update(gameTime, 0);
            }



            _police.Update(gameTime);

            foreach (Gang gang in  _gangs)
            {
                gang.Update(gameTime);
            }

            _board.Update(gameTime);
        }


        // Update - głowna funkcja graficzna, rozpoczyna rysowanie następnie wywołuje funkcje Draw na każdym obiekcie który ma swoją reprezentację graficzną.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            // WARSTWA 1 - MAPA
            _board.Draw(spriteBatch);

            // WARSTWA 2 - OBYWATELE
            _citizens.Draw(spriteBatch);

            // WARSTWA 3 - GANGI
            foreach (Gang gang in _gangs)
            {
                gang.Draw(spriteBatch);
            }

            // WARSTWA 4 - POLICJANCI
            _police.Draw(spriteBatch);

            spriteBatch.End(); 
        }
    }



}