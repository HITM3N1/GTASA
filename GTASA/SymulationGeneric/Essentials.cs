using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GTASA.SymulationGeneric
{
    //******************************************************************//
    //***** Klasa : Essentials                                     *****//
    //******************************************************************//
    //***** Ustawienie i Parametry całej gry.                      *****//
    //******************************************************************//

    public static class Essentials
    {

        // SEED - parametr kontrouljący losowość całej gry.
        private readonly static int SEED = 0;

        // RANDOM - obiekt tworzący wszystkie liczby losowe w grze.
        public readonly static Random RANDOM = new Random(SEED);

        public static bool ULTRA_UI = true;

        public static bool ULTRA_UI_Key = false;

        // MAP_SIZE - rozmiar mapy gry  
        public readonly static int MAP_SIZE = 30;

        // PAVMENT_COUNT - ilość chodników horyzontalnych i wertykalnych.
        public readonly static int PAVMENT_COUNT = 2;

        // PAVMENT_OFFSET - minimalny odstęp między chodnikami
        public readonly static int PAVMENT_OFFSET = 3;

        // CELL_SIZE - rozmiar każdej kratki w pikselach
        public readonly static int CELL_SIZE = 16;


        public static class GroupSettings
        {
            // GANGS_COUNT - ilość ganów na mapie (z racji na ilość tekstur przyjmuje wartość od 0-2)
            public readonly static int GANGS_COUNT = 2;

            // _BASE_AGENT_COUNT - kolejno ilość policjantów, cywili, i przestępców w ganach w momencie startu symulacji
            public readonly static int POLICE_BASE_AGENT_COUNT = 0;
            public readonly static int CITIZENS_BASE_AGENT_COUNT = 5;
            public readonly static int[] GANGS_BASE_AGENT_COUNT = { 1, 2 };

            // _COLOR - kolejno kolor policji, obywateli i gangów
            public readonly static GroupColor POLICE_COLOR = GroupColor.Blue;
            public readonly static GroupColor CITIZENS_COLOR = GroupColor.White;
            public readonly static GroupColor[] GANGS_COLOR = { GroupColor.Red, GroupColor.Green };

            // CITIZEN_RESPAWN - odpowiada za fakt czy obywatele pojawiają się utrzymując cały czas ten sam poziom populacji
            public readonly static bool CITIZEN_RESPAWN = true;

            // TIME_TO_TAKE_OVER_BUILDING - czas w sekundach potrzebny na przejęcie budynku przez gang
            public readonly static float TIME_TO_TAKE_OVER_BUILDING = 5;


            // CAN_GANGS_RECRUTE - odpowida za fakt czy gangi są w stanie rekrutować obywateli
            public readonly static bool CAN_GANGS_RECRUTE = true;

            // RECRUTATION_CHANCE - szansa w % na to że w każdej klatce przestępca zrekrutuje obywatela obok
            public readonly static int RECRUTATION_CHANCE = 5;

            // TIME_TO_RECRUTE - czas w sekundach potrzebny na zrekrutowanie obywatela
            public readonly static float TIME_TO_RECRUTE = 3;


            // GANG_RADIUS_TO_DEFENCE_BUILIDNG - zasięg w jakim przestępcy reagują na attakowany przez inną grupę ich budynek
            public readonly static int GANG_RADIUS_TO_DEFENCE_BUILIDNG = 10;

            // GANG_RADIUS_TO_ATTACK_BUILIDNG - zasięg w jakim przestępcy reagują na attakowany przez grupę budynek
            public readonly static int GANG_RADIUS_TO_ATTACK_BUILIDNG = 5;

            // POLICE_RADIUS_TO_REACTE - zasięg w jakim policja reaguje na dowolny przejmowany budynek 
            public readonly static int POLICE_RADIUS_TO_REACTE = 20;

            // BASE_SPEED - podstawowa wartość prędkości każdego z agentów podawana w kratkach na tick
            public readonly static float BASE_SPEED = 0.2f;

            // _SPEED_MODIFIRE - kolejno modyfikatory prędkości dla policji, cywili i ganów. Nim wartośc modyfikatora większa tym bardziej spowolniona jest dana grupa. Wartość 1 jest domyślna i nie zmienia prędkości grupy
            public readonly static float POLICE_SPEED_MODIFIRE = 2f;
            public readonly static float CITIZNES_SPEED_MODIFIRE = 1.1f;
            public readonly static float[] GANG_SPEED_MODIFIRE = { 1, 1 };

            // RANDOMIZE_SPEED_OF_EACH_AGNET - Losowe zmienniane prędkości każdego agenta z osobna.
            public readonly static bool RANDOMIZE_SPEED_OF_EACH_AGNET = false;
        }

        public static class GroupSettings1
        {
            public readonly static int PoliceStarMembers = 0;
            public readonly static int CitizensStarMembers = 0;
            public readonly static int[] GangStarMembers = { 2, 3 };
            public readonly static GroupColor[] GangColors = { GroupColor.Red, GroupColor.Green };

            public readonly static bool CitizenSpawn = false;

            public readonly static float timeToOccupyBuilding = 5;
            public readonly static bool canRecrute = true;

            public readonly static float timeToRecrute = 3;

            public readonly static GroupColor PoliceColor = GroupColor.Blue;
            public readonly static GroupColor CitizensColor = GroupColor.White;


            public readonly static int GangRadiusToDefenceBuilding = 10;
            public readonly static int GangRadiusToAttackBuilding = 5;
            public readonly static int PoliceRadiusReaction = 20;

            public readonly static int RecrutationChance = 5;

            public readonly static float PoliceSpeedMod = 2f;
            public readonly static float CitiznesSpeedMod = 0.8f;
            public readonly static float[] GangSpeedMod = { 1, 1 };

            public readonly static bool ranodmizeSpeed = true;
        }

        public static class GroupSettings2
        {
            public readonly static int PoliceStarMembers = 1;
            public readonly static int CitizensStarMembers = 1;
            public readonly static int[] GangStarMembers = { 2, 3 };
            public readonly static GroupColor[] GangColors = { GroupColor.Red, GroupColor.Green };

            public readonly static bool CitizenSpawn = true;

            public readonly static float timeToOccupyBuilding = 5;
            public readonly static bool canRecrute = true;

            public readonly static float timeToRecrute = 3;

            public readonly static GroupColor PoliceColor = GroupColor.Blue;
            public readonly static GroupColor CitizensColor = GroupColor.White;


            public readonly static int GangRadiusToDefenceBuilding = 10;
            public readonly static int GangRadiusToAttackBuilding = 5;
            public readonly static int PoliceRadiusReaction = 20;

            public readonly static int RecrutationChance = 5;

            public readonly static float PoliceSpeedMod = 2f;
            public readonly static float CitiznesSpeedMod = 1.1f;
            public readonly static float[] GangSpeedMod = { 1, 1 };

            public readonly static bool ranodmizeSpeed = false;
        }

        public static class GroupSettings4
        {
            public readonly static int PoliceStarMembers = 0;
            public readonly static int CitizensStarMembers = 0;
            public readonly static int[] GangStarMembers = { 2, 3 };
            public readonly static GroupColor[] GangColors = { GroupColor.Red, GroupColor.Green };

            public readonly static bool CitizenSpawn = false; //respawn mieszkańców po smierći

            public readonly static float timeToOccupyBuilding = 5; // czas w jakim gangi przejmują budynki
            public readonly static bool canRecrute = true; // czy gangi mogą rekrutować mieszkanców

            public readonly static float timeToRecrute = 3; // czas rekrutowania

            public readonly static GroupColor PoliceColor = GroupColor.Blue;
            public readonly static GroupColor CitizensColor = GroupColor.White;


            public readonly static int GangRadiusToDefenceBuilding = 10;
            public readonly static int GangRadiusToAttackBuilding = 5;
            public readonly static int PoliceRadiusReaction = 20;

            public readonly static int RecrutationChance = 5;

            public readonly static float PoliceSpeedMod = 2f;
            public readonly static float CitiznesSpeedMod = 0.8f;
            public readonly static float[] GangSpeedMod = { 1, 1 };

            public readonly static bool ranodmizeSpeed = true;
        }

        // TEXTURES_PAVMENT - przechowuje typ tekstury i odpowiadającą mu rzeczywistą teksturę chodnika
        public static Dictionary<TextureType, Texture2D> TEXTURES_PAVMENT = new Dictionary<TextureType, Texture2D>();

        // TEXTURES_BUILDING - przechowuje typ tekstury oraz kolor i odpowiadającą mu rzeczywistą teksturę budynku
        public static Dictionary<(TextureType, GroupColor), Texture2D> TEXTURES_BUILDING = new Dictionary<(TextureType, GroupColor), Texture2D>();

        // DIRECTIONS - przechowuje kierunek przemieszczanie się
        public static Dictionary<int, Vector2> DIRECTIONS = new Dictionary<int, Vector2>();

        public static Texture2D background;

        //******************************************************************//
        //***** void Initialize() - inicjalizuje wartości klasy        *****//
        //******************************************************************//
        public static void Initialize() // kierunku ruchu
        {
            DIRECTIONS[0] = new Vector2(-1, 0); //lewo
            DIRECTIONS[1] = new Vector2(0, -1); // góra
            DIRECTIONS[2] = new Vector2(1, 0); // prawo
            DIRECTIONS[3] = new Vector2(0, 1); // dół
        }


        //******************************************************************************//
        //***** void LoadContent() - ładuje tekstury do odpowiednich obiektów      *****//
        //******************************************************************************//
        public static void LoadContent(ContentManager contentManager)
        {

            TEXTURES_BUILDING.Add((TextureType.B1, GroupColor.White), contentManager.Load<Texture2D>("B1-White"));
            TEXTURES_BUILDING.Add((TextureType.B2, GroupColor.White), contentManager.Load<Texture2D>("B2-White"));
            TEXTURES_BUILDING.Add((TextureType.B3, GroupColor.White), contentManager.Load<Texture2D>("B3-White"));
            TEXTURES_BUILDING.Add((TextureType.B4, GroupColor.White), contentManager.Load<Texture2D>("B4-White"));
            TEXTURES_BUILDING.Add((TextureType.B5, GroupColor.White), contentManager.Load<Texture2D>("B5-White"));
            TEXTURES_BUILDING.Add((TextureType.B6, GroupColor.White), contentManager.Load<Texture2D>("B6-White"));
            TEXTURES_BUILDING.Add((TextureType.B7, GroupColor.White), contentManager.Load<Texture2D>("B7-White"));
            TEXTURES_BUILDING.Add((TextureType.B8, GroupColor.White), contentManager.Load<Texture2D>("B8-White"));
            TEXTURES_BUILDING.Add((TextureType.B0, GroupColor.White), contentManager.Load<Texture2D>("B9-White"));

            TEXTURES_BUILDING.Add((TextureType.B1, GroupColor.Blue), contentManager.Load<Texture2D>("B1-White"));
            TEXTURES_BUILDING.Add((TextureType.B2, GroupColor.Blue), contentManager.Load<Texture2D>("B2-White"));
            TEXTURES_BUILDING.Add((TextureType.B3, GroupColor.Blue), contentManager.Load<Texture2D>("B3-White"));
            TEXTURES_BUILDING.Add((TextureType.B4, GroupColor.Blue), contentManager.Load<Texture2D>("B4-White"));
            TEXTURES_BUILDING.Add((TextureType.B5, GroupColor.Blue), contentManager.Load<Texture2D>("B5-White"));
            TEXTURES_BUILDING.Add((TextureType.B6, GroupColor.Blue), contentManager.Load<Texture2D>("B6-White"));
            TEXTURES_BUILDING.Add((TextureType.B7, GroupColor.Blue), contentManager.Load<Texture2D>("B7-White"));
            TEXTURES_BUILDING.Add((TextureType.B8, GroupColor.Blue), contentManager.Load<Texture2D>("B8-White"));
            TEXTURES_BUILDING.Add((TextureType.B0, GroupColor.Blue), contentManager.Load<Texture2D>("B9-White"));

            TEXTURES_BUILDING.Add((TextureType.B1, GroupColor.Green), contentManager.Load<Texture2D>("B1-Green"));
            TEXTURES_BUILDING.Add((TextureType.B2, GroupColor.Green), contentManager.Load<Texture2D>("B2-Green"));
            TEXTURES_BUILDING.Add((TextureType.B3, GroupColor.Green), contentManager.Load<Texture2D>("B3-Green"));
            TEXTURES_BUILDING.Add((TextureType.B4, GroupColor.Green), contentManager.Load<Texture2D>("B4-Green"));
            TEXTURES_BUILDING.Add((TextureType.B5, GroupColor.Green), contentManager.Load<Texture2D>("B5-Green"));
            TEXTURES_BUILDING.Add((TextureType.B6, GroupColor.Green), contentManager.Load<Texture2D>("B6-Green"));
            TEXTURES_BUILDING.Add((TextureType.B7, GroupColor.Green), contentManager.Load<Texture2D>("B7-Green"));
            TEXTURES_BUILDING.Add((TextureType.B8, GroupColor.Green), contentManager.Load<Texture2D>("B8-Green"));
            TEXTURES_BUILDING.Add((TextureType.B0, GroupColor.Green), contentManager.Load<Texture2D>("B0-Green"));

            TEXTURES_BUILDING.Add((TextureType.B1, GroupColor.Red), contentManager.Load<Texture2D>("B1-Red"));
            TEXTURES_BUILDING.Add((TextureType.B2, GroupColor.Red), contentManager.Load<Texture2D>("B2-Red"));
            TEXTURES_BUILDING.Add((TextureType.B3, GroupColor.Red), contentManager.Load<Texture2D>("B3-Red"));
            TEXTURES_BUILDING.Add((TextureType.B4, GroupColor.Red), contentManager.Load<Texture2D>("B4-Red"));
            TEXTURES_BUILDING.Add((TextureType.B5, GroupColor.Red), contentManager.Load<Texture2D>("B5-Red"));
            TEXTURES_BUILDING.Add((TextureType.B6, GroupColor.Red), contentManager.Load<Texture2D>("B6-Red"));
            TEXTURES_BUILDING.Add((TextureType.B7, GroupColor.Red), contentManager.Load<Texture2D>("B7-Red"));
            TEXTURES_BUILDING.Add((TextureType.B8, GroupColor.Red), contentManager.Load<Texture2D>("B8-Red"));
            TEXTURES_BUILDING.Add((TextureType.B0, GroupColor.Red), contentManager.Load<Texture2D>("B0-Red"));

            TEXTURES_BUILDING.Add((TextureType.D, GroupColor.White), contentManager.Load<Texture2D>("DoorWhite"));
            TEXTURES_BUILDING.Add((TextureType.D, GroupColor.Blue), contentManager.Load<Texture2D>("DoorWhite"));
            TEXTURES_BUILDING.Add((TextureType.D, GroupColor.Red), contentManager.Load<Texture2D>("DoorRed"));
            TEXTURES_BUILDING.Add((TextureType.D, GroupColor.Green), contentManager.Load<Texture2D>("DoorGreen"));

            TEXTURES_BUILDING.Add((TextureType.A, GroupColor.White), contentManager.Load<Texture2D>("Agent-White"));
            TEXTURES_BUILDING.Add((TextureType.A, GroupColor.Red), contentManager.Load<Texture2D>("Agent-Red"));
            TEXTURES_BUILDING.Add((TextureType.A, GroupColor.Green), contentManager.Load<Texture2D>("Agent-Green"));
            TEXTURES_BUILDING.Add((TextureType.A, GroupColor.Blue), contentManager.Load<Texture2D>("Agent-Blue"));

            TEXTURES_PAVMENT.Add(TextureType.P0, contentManager.Load<Texture2D>("P0"));
            TEXTURES_PAVMENT.Add(TextureType.P1, contentManager.Load<Texture2D>("P1"));
            TEXTURES_PAVMENT.Add(TextureType.P2, contentManager.Load<Texture2D>("P2"));
            TEXTURES_PAVMENT.Add(TextureType.P3, contentManager.Load<Texture2D>("P3"));

            background = contentManager.Load<Texture2D>("background");
        }
    }


    // CellType - przechowuje informacje o czysto logicznym stanie każdej komórki 
    public enum CellType
    {
        EmptyCell,
        Pavment,
        Building,
    }


    // TextureType - przechowuje informacje o teksturze każdej komórki
    public enum TextureType
    {
        // Różne tekstury chodników
        P0, P1, P2, P3, 

        //Różne tekstury budynków
        B0, B1, B2, B3, B4, B5, B6, B7, B8, 

        // Tekstura drzwi
        D, 

        // Tekstura Agenta
        A, 
    }

    // GroupColor - przechowuje kolor konkretnej grupy

    public enum GroupColor
    {
        White,
        Red,
        Green,
        Blue,
    }
}