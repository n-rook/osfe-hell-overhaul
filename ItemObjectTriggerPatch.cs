using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(EffectActions))]
    [HarmonyPatch("CallFunctionWithItem")]
    class EffectActionsRecalculateStatsIfNecessary
    {
        private static readonly List<string> RECALCULATE_STATS_FOR = new List<string> { "Defense" };

        [HarmonyPostfix]
        static void RecalculatePlayerStatsIfNecessary(string fn, ItemObject itemObj)
        {
            if (!RECALCULATE_STATS_FOR.Contains(fn))
            {
                return;
            }
            Player p = itemObj?.being?.player;
            if (p == null)
            {
                return;
            }

            Debug.Log("Recalculating player stats.");
            p.deCtrl.statsScreen.UpdateStats(p);
        }
    }
}
