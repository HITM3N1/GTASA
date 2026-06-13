using GTASA.SymulationGeneric.Boards; // daje dostęp do klasy board
using GTASA.SymulationGeneric.Groups; // daje dostęp do klas citizens Police Gang
using GTASA.SymulationGeneric.Groups.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content; //daje nam obeikty z folderu content
using Microsoft.Xna.Framework.Graphics; // SpriteBatch i SampleState - rzczy potrzebne do rysowania
using System.Collections.Generic;

namespace GTASA.SymulationGeneric
{
    public class Symulation // główny koordynato świata
    {
        Board tab; // mapa gry

        Citizens citizens; // mieszkańcy
        Police police; // policja
        List<Gang> gangs; // lista gangów
        Matrix cameraMatrix; // macierz kamery która powiększa obraz

        int startPopulation; // to zapmiętuje ile agentów powinno być na początku (jeżeli ta wartosc spadła to CitizenSpawn może dodac nowych mieszkańców)
    

        public Symulation() //konstruktor
        {
            Essentials.Initialize(); // inicjalizacja podstawowanych danych z klasy Essentials
            cameraMatrix = Matrix.CreateScale(Essentials.RENDER_ZOOM, Essentials.RENDER_ZOOM, 1f); // ustawienia kamrey
            gangs = new List<Gang>(); // tworzenie pustej listy gangów na początku

            tab = new Board(Essentials.mapSize); // tworzenie mapy
            tab.Initialize(citizens);

            citizens = new Citizens(tab); // tworzenie mieszkańców
            tab.SetCitizens(citizens);

            police = new Police(tab); // tworzenie policji

            startPopulation = Essentials.GroupSettings.PoliceStarMembers + Essentials.GroupSettings.CitizensStarMembers; // liczenie początkowej liczby populacji

            for (int i = 0; i < Essentials.gangsCount; i++) // dodanie do liczby populacji dwóch gangów osobno i stworzenie gangów
            {
                startPopulation += Essentials.GroupSettings.GangStarMembers[i];
                gangs.Add(new Gang(i, Essentials.GroupSettings.GangColors[i], tab));
            }


            foreach (Gang gang in gangs)
            {
                gang.Initialize(); // inicjalizajca gangów
            }

            citizens.Initialize(tab); // inicjalizacja mieszkańców
            police.Initialize(tab); // inicjalizacjia policji

            
            

        }

        public void LoadContent(ContentManager contentManager)
        {
            Essentials.LoadContent(contentManager); // ladowanie tekstur z "content"
        }

        public void Update(GameTime gameTime) // wykonuje sie co klatke gry
        {
            // zliczanie populacji
            int populationRightNow = citizens.GetAgentCount() + police.GetAgentCount();
            foreach(Gang gang in gangs)
            {
                populationRightNow += gang.GetAgentCount();
            }
            

            // jezeli jest włączone odradzanie mieszkańców i mieszkaniec nie żyje to dodaj brakującyhc mieszkańców
            if (populationRightNow < startPopulation && Essentials.GroupSettings.CitizenSpawn)
            {

                citizens.Update(gameTime, startPopulation - populationRightNow);
            }
            else
            {
                citizens.Update(gameTime, 0);
            }


            // aktualizuje decyzje policjantów
            //policja reaguje na ataki
            police.Update(gameTime);

            //aktualizuje decyzje gangów
            //gang wybiera lub kontynuuje atak
            //woła agentów
            //agenci gangu się ruszają
            foreach (Gang gang in gangs)
            {
                gang.Update(gameTime);
            }


            // aktualizacja mapy na końcu
            tab.Update(gameTime);
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: cameraMatrix); // poprawne i ładne rysowanie
            tab.Draw(spriteBatch);//mapa rysuje chodini budynki drzwi jako pierwosza warstwa

            // rysowanie gangów zieloni/czerowni
            foreach (Gang gang in gangs)
            {
                gang.Draw(spriteBatch);
            }

            citizens.Draw(spriteBatch); // rysowanie mieskzanców
            police.Draw(spriteBatch); // rysowanie policji
            spriteBatch.End(); // koniec rysowania
        }
    }


    
}
