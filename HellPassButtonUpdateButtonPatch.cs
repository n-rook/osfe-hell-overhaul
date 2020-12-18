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
            HellPassButton button = __instance;

            var currentPass = button.hellPasses[button.displayedHellPassNum];

            string description = CustomHell.GetGenericDescription(currentPass);
            Debug.Log($"Fixing description post-hoc; {button.description.text} -> {description}");
            button.description.text = description;
        }
    }
}
