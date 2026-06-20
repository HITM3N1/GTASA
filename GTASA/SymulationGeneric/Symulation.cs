using GTASA.SymulationGeneric.Boards;
using GTASA.SymulationGeneric.Groups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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

        // isFinished - sprawdza czy symulacja sie juz zakończyła
        private bool isFinished = false;

        // finalResultText - tekst który pokażemy na koncu symulacji
        private string finalResultText = ""; 

        

        // cameraMatrix - Macierz obsługująca wyświetlanie oraz zoomowanie kamery.
        Matrix cameraMatrix; 



        
        // Konstruktor - inicjalizuje działanie całej symulacji, tworzy wszystkie potrzebne obiekty oraz ustawia ich wartości początkowe.
        public Symulation()
        {
            Essentials.Initialize();

            SimulationStats.Reset(); // czyści statystyki
  

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
            CheckEndCondition(gameTime);

            if (isFinished)
            {
                return;
            }

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



        public bool IsFinished()
        {
            return isFinished;
        }

        public string GetFinalResultText()
        {
            return finalResultText;
        }

        // Sprawdza, czy symulacja powinna się zakończyć:
        // policja wygrywa po wyeliminowaniu wszystkich gangsterów,
        // a gang wygrywa po przejęciu wszystkich budynków.
        private void CheckEndCondition(GameTime gameTime)
        {
            int totalGangsters = 0;

            foreach (Gang gang in _gangs)
            {
                totalGangsters += gang.GetAgentCount();
            }

            if (totalGangsters == 0)
            {
                FinishSimulation("Policja", "Policja wyeliminowała wszystkich gangsterów", gameTime);
                return;
            }

            var buildings = _board.GetBuildings();
            int totalBuildings = buildings.Count;

            foreach (Gang gang in _gangs)
            {
                int gangBuildings = buildings.Count(b => b.GetOccupation() == gang);

                if (gangBuildings == totalBuildings)
                {
                    FinishSimulation(GetGangName(gang), $"{GetGangName(gang)} przejął wszystkie budynki", gameTime);
                    return;
                }
            }
        }

        // Zwraca czytelną nazwę gangu, np. "Gang 1 (Red)".
        private string GetGangName(Gang gang)
        {
            return $"Gang {gang.GetGangID() + 1} ({gang.GetColor()})";
        }

        // Kończy symulację, ustawia flagę zakończenia,
        // przygotowuje tekst wyniku i zapisuje dane do pliku JSON.
        private void FinishSimulation(string winner, string finishReason, GameTime gameTime)
        {
            if (isFinished)
            {
                return;
            }

            isFinished = true;

            finalResultText = BuildFinalResultText(winner, finishReason, gameTime);

            SaveResultsToJson(winner, finishReason, gameTime);
        }

        // Buduje tekst z końcowymi wynikami symulacji,
        // który później zostanie pokazany w okienku.
        private string BuildFinalResultText(string winner, string finishReason, GameTime gameTime)
        {
            StringBuilder sb = new StringBuilder();

            var buildings = _board.GetBuildings();

            sb.AppendLine("KONIEC SYMULACJI");
            sb.AppendLine();
            sb.AppendLine($"Zwycięzca: {winner}");
            sb.AppendLine($"Powód zakończenia: {finishReason}");
            sb.AppendLine($"Czas trwania: {Math.Round(gameTime.TotalGameTime.TotalSeconds, 2)} sekund");
            sb.AppendLine();

            sb.AppendLine("AGENCI NA KOŃCU:");
            sb.AppendLine($"Mieszkańcy: {_citizens.GetAgentCount()}");
            sb.AppendLine($"Policja: {_police.GetAgentCount()}");

            foreach (Gang gang in _gangs)
            {
                sb.AppendLine($"{GetGangName(gang)}: {gang.GetAgentCount()} gangsterów");
            }

            sb.AppendLine();
            sb.AppendLine("PRZEJĘCI MIESZKAŃCY:");
            sb.AppendLine($"Łącznie: {SimulationStats.GetTotalCitizenTakeovers()}");

            foreach (Gang gang in _gangs)
            {
                sb.AppendLine($"{GetGangName(gang)}: {SimulationStats.GetCitizenTakeoversByGang(gang.GetGangID())}");
            }

            sb.AppendLine();
            sb.AppendLine("BUDYNKI NA KOŃCU:");
            sb.AppendLine($"Wszystkie budynki: {buildings.Count}");
            sb.AppendLine($"Budynki mieszkańców: {buildings.Count(b => b.GetOccupation() == _citizens)}");

            foreach (Gang gang in _gangs)
            {
                sb.AppendLine($"{GetGangName(gang)} budynki: {buildings.Count(b => b.GetOccupation() == gang)}");
            }

            return sb.ToString();
        }

        // Zapisuje końcowe wyniki symulacji do pliku JSON.
        private void SaveResultsToJson(string winner, string finishReason, GameTime gameTime)
        {
            var buildings = _board.GetBuildings();

            var result = new
            {
                winner = winner,
                finishReason = finishReason,
                timeSeconds = Math.Round(gameTime.TotalGameTime.TotalSeconds, 2),

                agents = new
                {
                    citizens = _citizens.GetAgentCount(),
                    police = _police.GetAgentCount(),
                    gangs = _gangs.Select(gang => new
                    {
                        gangId = gang.GetGangID() + 1,
                        color = gang.GetColor().ToString(),
                        agents = gang.GetAgentCount()
                    }).ToList()
                },

                citizenTakeovers = new
                {
                    total = SimulationStats.GetTotalCitizenTakeovers(),
                    gangs = _gangs.Select(gang => new
                    {
                        gangId = gang.GetGangID() + 1,
                        color = gang.GetColor().ToString(),
                        takeovers = SimulationStats.GetCitizenTakeoversByGang(gang.GetGangID())
                    }).ToList()
                },

                buildings = new
                {
                    total = buildings.Count,
                    citizens = buildings.Count(b => b.GetOccupation() == _citizens),
                    gangs = _gangs.Select(gang => new
                    {
                        gangId = gang.GetGangID() + 1,
                        color = gang.GetColor().ToString(),
                        buildings = buildings.Count(b => b.GetOccupation() == gang)
                    }).ToList()
                },

                settings = new
                {
                    mapSize = Essentials.MAP_SIZE,
                    gangsCount = Essentials.GroupSettings.GANGS_COUNT,
                    citizenRespawn = Essentials.GroupSettings.CITIZEN_RESPAWN,
                    resultsPath = Essentials.RESULTS_PATH
                }
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(result, options);

            File.WriteAllText(Essentials.RESULTS_PATH, json);
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