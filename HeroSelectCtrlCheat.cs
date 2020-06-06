using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(HeroSelectCtrl))]
    [HarmonyPatch("Open")]
    class HeroSelectCtrlCheat
    {
        private static readonly bool UNLOCK_ALL_HELL_PASSES = false;

        [HarmonyPrefix]
        static void Cheat(HeroSelectCtrl __instance)
        {
            if (UNLOCK_ALL_HELL_PASSES)
            {
                Debug.Log("Unlocking all hell passes (temp)");
                __instance.runCtrl.unlockedHellPassNum = 20;
            }
        }
    }
}
