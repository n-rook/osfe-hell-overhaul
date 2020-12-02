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
    class SpellObjectPinchHack
    {
        private static HashSet<string> SELF_DAMAGE_SPELLS = new HashSet<string>() {
            "Pinch",
            "Transfuse",
            "Corset"
            };

        private static bool shouldRunHack(ItemObject itemObj, FTrigger fTrigger, Being hitBeing)
        {
            if (itemObj.spellObj == null)
            {
                return false;
            }
            SpellObject obj = itemObj.spellObj;

            if (!(fTrigger == FTrigger.OnHit && CustomHell.IsHellEnabled(obj?.ctrl?.runCtrl, CustomHellPassEffect.LOSE_DEF_ON_HIT) && SELF_DAMAGE_SPELLS.Contains(obj.itemID))) {
                return false;
            }

            Player player = hitBeing?.player;
            if (!player)
            {
                Debug.LogWarning($"Unexpected hitBeing for ${obj.itemID} (${hitBeing}, ${hitBeing?.player}); moving on");
                return false;
            }

            return true;
        }


        [HarmonyPrefix]
        static void OnHitPinchPrefixToCheckState(ItemObject __instance, FTrigger fTrigger, Being hitBeing, out bool __state)
        {
            if (!shouldRunHack(__instance, fTrigger, hitBeing))
            {
                __state = false;
                return;
            }
            Player player = hitBeing.player;

            // Only Reflect can short-circuit a spell like this. See Being#HitAmount.
            if (player.HasStatusEffect(Status.Reflect))
            {
                Debug.Log("Player has reflect, so no pinch hack necessary");
                __state = false;
                return;
            }

            __state = true;
        }

        [HarmonyPostfix]
        static void RaiseDefenseIfPinch(ItemObject __instance, FTrigger fTrigger, Being hitBeing, bool __state)
        {
            if (!__state)
            {
                return;
            }
            SpellObject obj = __instance.spellObj;

            if (SELF_DAMAGE_SPELLS.Contains(obj.itemID))
            {
                Player player = hitBeing?.player;
                if (!player)
                {
                    Debug.LogWarning($"Unexpected hitBeing for ${obj.itemID} (${hitBeing}, ${hitBeing?.player}); moving on");
                    return;
                }

                var pact = CustomHell.GetHellPact(player, CustomHellPassEffect.LOSE_DEF_ON_HIT);
                int newDef = pact.defense + 1;
                Debug.Log($"Pinch hack ({obj.itemID}): Refunding 1 defense to Hell Pass 20 ({pact.defense} -> {newDef})");
                pact.defense = newDef;
            }
        }
    }
}
