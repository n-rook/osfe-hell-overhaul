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
    class OnHitDebugPatch
    {
        private static readonly bool ENABLE_DEBUG_LOG_HERE = false;

        [HarmonyPostfix]
        static void DebugLog(ItemObject __instance)
        {
            if (!ENABLE_DEBUG_LOG_HERE)
            {
                return;
            }
            var obj = __instance;
            if (obj.itemID != "HellPass20") {
                return;
            }

            Debug.Log($"Item def is {obj.pactObj.defense}");
            Debug.Log($"Effect tags are {obj.effectTags.Join()}");
            Debug.Log($"Param dict is {obj.paramDictionary.Join()}");
        }
    }
}
