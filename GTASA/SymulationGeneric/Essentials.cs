using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GTASA.SymulationGeneric
{
    public static class Essentials
    {
        public static int mapSize = 40;
        public static int pavmentCount = 5;
        public static int pavmentOffset = 3;
        public static int cellSize = 16;
        public static int gangsCount = 0;

        public static float speed = 0.2f;
        public static float fleespeed = 130f;
        public static float fleeDistance = 100f;

        public static bool NEW_RENDER = true;
        public static float RENDER_ZOOM = 2f;


        public static class GroupSettings
        {
            public static GroupColor PoliceColor = GroupColor.Blue;
            public static GroupColor CitizensColor = GroupColor.White;
        }
        


        public static class AgentSettings
        {
            public static int GangMemberHP = 100;
            public static int GangMemberStrength = 10;
            public static int StenghtForKill = 5;

            public static int PoliceHP = 10000;
            public static int PoliceStrength = 10000;

            public static int CitizensHP = 1;
            public static int CitizensStrength = 0;

            public static int Regeneration = 1;
        }



        public static Dictionary<TextureType, Texture2D> texturesPavment = new Dictionary<TextureType, Texture2D>();

        public static Dictionary<(TextureType, GroupColor), Texture2D> texturesBuilding = new Dictionary<(TextureType, GroupColor), Texture2D>();

        public static Dictionary<int, Vector2> direction = new Dictionary<int, Vector2>();


        public static void Initialize()
        {
            direction[0] = new Vector2(-1, 0);
            direction[1] = new Vector2(0, -1);
            direction[2] = new Vector2(1, 0);
            direction[3] = new Vector2(0, 1);
        }

        public static void LoadContent(ContentManager contentManager)
        {

            texturesBuilding.Add((TextureType.B1, GroupColor.White), contentManager.Load<Texture2D>("B1-White"));
            texturesBuilding.Add((TextureType.B2, GroupColor.White), contentManager.Load<Texture2D>("B2-White"));
            texturesBuilding.Add((TextureType.B3, GroupColor.White), contentManager.Load<Texture2D>("B3-White"));
            texturesBuilding.Add((TextureType.B4, GroupColor.White), contentManager.Load<Texture2D>("B4-White"));
            texturesBuilding.Add((TextureType.B5, GroupColor.White), contentManager.Load<Texture2D>("B5-White"));
            texturesBuilding.Add((TextureType.B6, GroupColor.White), contentManager.Load<Texture2D>("B6-White"));
            texturesBuilding.Add((TextureType.B7, GroupColor.White), contentManager.Load<Texture2D>("B7-White"));
            texturesBuilding.Add((TextureType.B8, GroupColor.White), contentManager.Load<Texture2D>("B8-White"));
            texturesBuilding.Add((TextureType.B0, GroupColor.White), contentManager.Load<Texture2D>("B9-White"));

            texturesBuilding.Add((TextureType.B1, GroupColor.Blue), contentManager.Load<Texture2D>("B1-White"));
            texturesBuilding.Add((TextureType.B2, GroupColor.Blue), contentManager.Load<Texture2D>("B2-White"));
            texturesBuilding.Add((TextureType.B3, GroupColor.Blue), contentManager.Load<Texture2D>("B3-White"));
            texturesBuilding.Add((TextureType.B4, GroupColor.Blue), contentManager.Load<Texture2D>("B4-White"));
            texturesBuilding.Add((TextureType.B5, GroupColor.Blue), contentManager.Load<Texture2D>("B5-White"));
            texturesBuilding.Add((TextureType.B6, GroupColor.Blue), contentManager.Load<Texture2D>("B6-White"));
            texturesBuilding.Add((TextureType.B7, GroupColor.Blue), contentManager.Load<Texture2D>("B7-White"));
            texturesBuilding.Add((TextureType.B8, GroupColor.Blue), contentManager.Load<Texture2D>("B8-White"));
            texturesBuilding.Add((TextureType.B0, GroupColor.Blue), contentManager.Load<Texture2D>("B9-White"));

            texturesBuilding.Add((TextureType.B1, GroupColor.Green), contentManager.Load<Texture2D>("B1-Green"));
            texturesBuilding.Add((TextureType.B2, GroupColor.Green), contentManager.Load<Texture2D>("B2-Green"));
            texturesBuilding.Add((TextureType.B3, GroupColor.Green), contentManager.Load<Texture2D>("B3-Green"));
            texturesBuilding.Add((TextureType.B4, GroupColor.Green), contentManager.Load<Texture2D>("B4-Green"));
            texturesBuilding.Add((TextureType.B5, GroupColor.Green), contentManager.Load<Texture2D>("B5-Green"));
            texturesBuilding.Add((TextureType.B6, GroupColor.Green), contentManager.Load<Texture2D>("B6-Green"));
            texturesBuilding.Add((TextureType.B7, GroupColor.Green), contentManager.Load<Texture2D>("B7-Green"));
            texturesBuilding.Add((TextureType.B8, GroupColor.Green), contentManager.Load<Texture2D>("B8-Green"));
            texturesBuilding.Add((TextureType.B0, GroupColor.Green), contentManager.Load<Texture2D>("B0-Green"));

            texturesBuilding.Add((TextureType.B1, GroupColor.Red), contentManager.Load<Texture2D>("B1-Red"));
            texturesBuilding.Add((TextureType.B2, GroupColor.Red), contentManager.Load<Texture2D>("B2-Red"));
            texturesBuilding.Add((TextureType.B3, GroupColor.Red), contentManager.Load<Texture2D>("B3-Red"));
            texturesBuilding.Add((TextureType.B4, GroupColor.Red), contentManager.Load<Texture2D>("B4-Red"));
            texturesBuilding.Add((TextureType.B5, GroupColor.Red), contentManager.Load<Texture2D>("B5-Red"));
            texturesBuilding.Add((TextureType.B6, GroupColor.Red), contentManager.Load<Texture2D>("B6-Red"));
            texturesBuilding.Add((TextureType.B7, GroupColor.Red), contentManager.Load<Texture2D>("B7-Red"));
            texturesBuilding.Add((TextureType.B8, GroupColor.Red), contentManager.Load<Texture2D>("B8-Red"));
            texturesBuilding.Add((TextureType.B0, GroupColor.Red), contentManager.Load<Texture2D>("B0-Red"));

            texturesBuilding.Add((TextureType.D, GroupColor.White), contentManager.Load<Texture2D>("DoorWhite"));
            texturesBuilding.Add((TextureType.D, GroupColor.Red), contentManager.Load<Texture2D>("DoorRed"));
            texturesBuilding.Add((TextureType.D, GroupColor.Green), contentManager.Load<Texture2D>("DoorGreen"));

            texturesBuilding.Add((TextureType.A, GroupColor.White), contentManager.Load<Texture2D>("Agent-White"));
            texturesBuilding.Add((TextureType.A, GroupColor.Red), contentManager.Load<Texture2D>("Agent-Red"));
            texturesBuilding.Add((TextureType.A, GroupColor.Green), contentManager.Load<Texture2D>("Agent-Green"));
            texturesBuilding.Add((TextureType.A, GroupColor.Blue), contentManager.Load<Texture2D>("Agent-Blue"));

            texturesPavment.Add(TextureType.P0, contentManager.Load<Texture2D>("P0"));
            texturesPavment.Add(TextureType.P1, contentManager.Load<Texture2D>("P1"));
            texturesPavment.Add(TextureType.P2, contentManager.Load<Texture2D>("P2"));
            texturesPavment.Add(TextureType.P3, contentManager.Load<Texture2D>("P3"));
        }
    }

    public enum CellType
    {
        EmptyCell,
        Pavment,
        Building,
    }

    public enum TextureType
    {
        P0,P1, P2, P3, 

        B0, B1, B2, B3, B4, B5, B6, B7, B8,

        D,

        A,
    }

    public enum TextureRotation
    {
        deg0,
        deg90,
        deg180,
        deg270,
    }

    public enum GroupColor
    {
        White,
        Red,
        Green,
        Purple,
        Blue,
    }
}