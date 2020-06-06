using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(ItemObject))]
    [HarmonyPatch("Trigger")]
    class ItemObjectTriggerPatch
    {
        private static readonly string MINUS_DEF_HELL_PASS = "HellPass20";

        [HarmonyPostfix]
        static void RecalculatePlayerStatsIfNecessary(ItemObject __instance, bool ___checkFailed)
        {
            var itemObj = __instance;
            if (itemObj.itemID != MINUS_DEF_HELL_PASS) {
                return;
            }

            if (___checkFailed)
            {
                Debug.Log("Check failed, returning");
                return;
            }

            Player p = __instance.being?.player;
            if (p == null)
            {
                Debug.LogWarning($"Hit being was not player, but rather {__instance.being?.ToString()}");
                return;
            }

            // TODO: This appears to recalculate stats every frame, for some reason
            p.deCtrl.statsScreen.UpdateStats(p);
        }
    }
}
