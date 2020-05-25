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
        static bool Prepare(Boss __instance)
        {
            Debug.Log("Loaded ItemMan debug");
            return true;
        }

        static void Postfix(ItemManager __instance)
        {
            var hells = __instance.hellPasses;

            Debug.Log("Loaded hells have IDs " + hells.Select(i => i.itemID).Join());
        }
    }
}
