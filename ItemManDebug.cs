using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(ItemManager))]
    [HarmonyPatch("LoadItemData")]
    class ItemManDebug
    {
        [HarmonyPrepare]
        static bool Prepare(Boss __instance)
        {
            return true;
        }

        [HarmonyPostfix]
        static void Postfix(ItemManager __instance)
        {
            var hells = __instance.hellPasses;

            // Debug.Log("Loaded hells have IDs " + hells.Select(i => i.itemID).Join());
            // Debug.Log("HP13 is: " + hells[14].description);
        }
    }
}
