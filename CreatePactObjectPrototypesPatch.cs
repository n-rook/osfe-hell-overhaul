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
    [HarmonyPatch("CreatePactObjectPrototypes")]
    class CreatePactObjectPrototypesPatch
    {
        [HarmonyPostfix]
        static void Postfix(ItemManager __instance)
        {
            Debug.Log("Uniquifying Hell Passes");

            // Remove earlier items in the list, preferring later ones with the same ID.
            var hellDict = new Dictionary<string, PactObject>();
            foreach (var h in __instance.hellPasses) {
                if (hellDict.ContainsKey(h.itemID))
                {
                    Debug.Log($"Overwriting {h.itemID}");
                }
                hellDict[h.itemID] = h;
            }
            var endList = hellDict.Values.OrderBy(h => h.itemID);

            Debug.Log($"New hell list: {endList.Select(h => h.itemID).Join()}");
            __instance.hellPasses.Clear();
            __instance.hellPasses.AddRange(endList);
        }
    }
}
