using System.Collections.Generic;
using System.Linq;

namespace GTASA.SymulationGeneric
{
    public static class SimulationStats
    {
        private static readonly Dictionary<int, int> citizenTakeoversByGang = new Dictionary<int, int>();

        public static void Reset()
        {
            citizenTakeoversByGang.Clear();
        }

        public static void RegisterCitizenTakeover(int gangId)
        {
            if (!citizenTakeoversByGang.ContainsKey(gangId))
            {
                citizenTakeoversByGang[gangId] = 0;
            }

            citizenTakeoversByGang[gangId]++;
        }

        public static int GetCitizenTakeoversByGang(int gangId)
        {
            if (!citizenTakeoversByGang.ContainsKey(gangId))
            {
                return 0;
            }

            return citizenTakeoversByGang[gangId];
        }

        public static int GetTotalCitizenTakeovers()
        {
            return citizenTakeoversByGang.Values.Sum();
        }
    }
}