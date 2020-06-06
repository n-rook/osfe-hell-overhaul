using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(DuelDisk))]
    [HarmonyPatch("Update")]
    class DuelDiskUpdatePatch
    {
        private static readonly float MANA_REGEN_MULTIPLIER = 0.5f;

        [HarmonyPrefix]
        static bool PossiblyHalfSpeedUpdate(DuelDisk __instance)
        {
            DuelDisk disk = __instance;

            // It would be nice if we could use a transpiler here, but it's not easily possible.

            if (!CustomHell.IsHellEnabled(disk.ctrl.runCtrl, CustomHellPassEffect.HALF_MANA_REGEN))
            {
                return true;
            }

            updateHealthBar(disk);
            regenMana(disk);
            disk.manaBar.UpdateBar(disk.currentMana, disk.player.maxMana);
            return false;
        }

        private static void updateHealthBar(DuelDisk disk)
        {
            // I don't really know what's up with this weird cast,
            // but this code is copied from OSFE proper
            if (disk.healthBar.gameObject.activeSelf && (bool)(UnityEngine.Object)disk.player)
                disk.healthBar.UpdateBar((float)disk.player.health.current, (float)disk.player.health.max);
        }

        private static void regenMana(DuelDisk disk)
        {
            var player = disk.player;
            if (!((player.manaRegen >= 0.0 && disk.currentMana >= player.maxMana) ||
                (player.manaRegen < 0.0 && disk.currentMana <= 0)))
            {
                disk.currentMana += Time.deltaTime * player.manaRegen * MANA_REGEN_MULTIPLIER;
            }
            disk.currentMana = Mathf.Clamp(disk.currentMana, 0.0f, player.maxMana);
        }
    }
}
