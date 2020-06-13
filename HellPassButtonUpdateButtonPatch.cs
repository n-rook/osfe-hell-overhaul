using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(HellPassButton))]
    [HarmonyPatch("UpdateButton")]
    class HellPassButtonUpdateButtonPatch
    {
        [HarmonyPostfix]
        static void Postfix(HellPassButton __instance)
        {
            var currentPass = __instance.hellPasses[__instance.displayedHellPassNum];

            Debug.Log($"Fixing description post-hoc; {__instance.description.text} -> {currentPass.description}");
            __instance.description.text = currentPass.description;

            // Special fix for hell pass 20.
            if (currentPass.itemID == "HellPass20")
            {
                __instance.description.text = currentPass.description.Replace(" (efApp.defense)", "");
            }
        }
    }
}
