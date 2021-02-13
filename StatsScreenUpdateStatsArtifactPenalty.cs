using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(StatsScreen))]
    [HarmonyPatch("UpdateStats")]
    class StatsScreenUpdateStatsArtifactPenalty
    {
        class StatsScreenInfo
        {
            internal readonly float? manaRegen;

            internal StatsScreenInfo(float? manaRegen)
            {
                this.manaRegen = manaRegen;
            }
        }

        [HarmonyPrefix]
        static void CheckManaRegenBefore(out StatsScreenInfo __state)
        {
            float? playerManaRegen = S.I.batCtrl?.currentPlayer?.manaRegen;
            __state = new StatsScreenInfo(playerManaRegen);
        }

        [HarmonyPostfix]
        static void DecreaseManaRegen(StatsScreen __instance, StatsScreenInfo __state, Player player = null)
        {
            StatsScreen statsScreen = __instance;
            // This is how the base function works
            if (player == null)
            {
                player = S.I.batCtrl?.currentPlayer;
            }
            if (player == null)
            {
                return;
            }

            if (!CustomHell.IsHellEnabled(player, CustomHellPassEffect.HALF_MANA_REGEN))
            {
                return;
            }

            // I've had bug reports come in that this has taken effect multiple times somehow.
            // I suspect the cause is multiple installation, but just in case, I've put in this
            // check as well.
            StatsScreenInfo previousState = __state;
            if (player.manaRegen == previousState.manaRegen)
            {
                return;
            }

            float manaRegenFromArtifacts = 0.0f;
            foreach (var a in player.artObjs)
            {
                manaRegenFromArtifacts += a.manaRegen;
            }

            player.manaRegen -= manaRegenFromArtifacts / 2;
        }
    }
}
