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
        [HarmonyPostfix]
        static void DecreaseManaRegen(StatsScreen __instance, Player player = null)
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

            float manaRegenFromArtifacts = 0.0f;
            foreach (var a in player.artObjs)
            {
                manaRegenFromArtifacts += a.manaRegen;
            }

            // Debug.Log($"{manaRegenFromArtifacts} being halved. Old number was {player.manaRegen}");
            player.manaRegen -= manaRegenFromArtifacts / 2;
            // Debug.Log($"{manaRegenFromArtifacts} being halved. New number is {player.manaRegen}");
        }
    }
}
