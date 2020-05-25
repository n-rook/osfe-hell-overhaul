using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    /*
     * To-do list:
     * 1. Fix issue where localized descriptions are used in HellPassButton
     * 2. Fix issue where localized descriptions are used in-game (I think)
     */



    /*
     * Bugs to report eventually:
     * 1. Overriding existing hell passes doesn't work as expected; the new passes replace the old ones in the dict,
     *    but they both show up in the list
     * 2. New descriptions aren't used for replacement hell passes on the select screen
     */

    [HarmonyPatch(typeof(ItemManager))]
    [HarmonyPatch("LoadItemData")]
    class ItemManDebug
    {
        [HarmonyPrepare]
        static bool Prepare(Boss __instance)
        {
            Debug.Log("Loaded ItemMan debug");
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
