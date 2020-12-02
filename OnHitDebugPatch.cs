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
        static void DebugLog(ItemObject __instance, FTrigger fTrigger, bool doublecast, Being hitBeing, int forwardedHitDamage)
        {
            // Debug.Log($"Item ID: {__instance.itemID}");
            if (!ENABLE_DEBUG_LOG_HERE)
            {
                return;
            }
            var obj = __instance;
            if (obj.itemID != "HellPass20" && obj.itemID != "Pinch" && obj.itemID != "Transfuse" && obj.itemID != "Corset") {
                return;
            }
            if (fTrigger == FTrigger.Hold)
            {
                // Too spammy
                return;
            }

            Debug.Log($"[{obj.itemID}] Arguments are: {fTrigger}, {doublecast}, {hitBeing?.name}, {forwardedHitDamage}");

            switch (obj.itemID) {
                case "Pinch":
                case "Transfuse":
                case "Corset":
                    {
                        Debug.Log($"Being player is {hitBeing?.player}");
                        Debug.Log($"SpellObj is {obj.spellObj}");
                        break;
                    }
                case "HellPass20":
                    {
                        Debug.Log($"Item def is {obj.pactObj.defense}");
                        //Debug.Log($"Effect tags are {obj.effectTags.Join()}");
                        //Debug.Log($"Param dict is {obj.paramDictionary.Join()}");
                        //Debug.Log($"Parent and origin spells are {obj.parentSpell}, {obj.originSpell}");
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
