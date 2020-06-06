using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(SpawnCtrl))]
    [HarmonyPatch("SpawnCampsiteZone")]
    class SpawnCtrlSpawnCampsiteZonePatch
    {
        private static string STACKING_DEF_HELL_PASS = "HellPass20";

        [HarmonyPostfix]
        static void HalveHellPass20Defense(SpawnCtrl __instance)
        {
            var spawnCtrl = __instance;
            foreach (var pact in spawnCtrl.ctrl.currentPlayer.pactObjs)
            {
                if (pact.itemID == STACKING_DEF_HELL_PASS)
                {
                    var oldDef = pact.defense;
                    pact.defense /= 2;
                    Debug.Log($"At campsite, so -def reduced from {oldDef} to {pact.defense}");
                }
            }
        }
    }
}
