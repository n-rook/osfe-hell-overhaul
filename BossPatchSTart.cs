using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(Boss))]
    [HarmonyPatch("Start")]
    class BossPatchStart
    {
        static int GetAdjustment(Boss boss)
        {
            if (!CustomHell.IsHellEnabled(boss.runCtrl, CustomHellPassEffect.LATE_BOSSES_HARDER))
            {
                return 0;
            }
            
            //   3455667
            // - 3445566
            // = 0010101

            switch(boss.runCtrl.currentRun.worldTierNum)
            {
                case 2:
                case 4:
                case 6:
                    return 1;
                default:
                    return 0;
            }
        }

        [HarmonyPrefix]
        static void TemporarilyIncrementBossTier(Boss __instance, out int __state)
        {
            var boss = __instance;
            var adjustment = GetAdjustment(boss);
            __state = adjustment;

            Debug.Log($"Adjusting boss tier temporarily by {adjustment}.");
            boss.ctrl.baseBossTier += adjustment;
        }

        [HarmonyFinalizer]
        static void ResetBossTier(Boss __instance, int __state)
        {
            var boss = __instance;
            var adjustment = __state;
            Debug.Log($"Resetting boss state by subtracting {adjustment}.");
            boss.ctrl.baseBossTier -= adjustment;
        }
    }
}
