using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(SpawnCtrl))]
    [HarmonyPatch("SpawnBattleZone")]
    class SpawnCtrlSpawnBattleZonePatch
    {
        private static readonly MethodInfo SPAWN_CPU = typeof(SpawnCtrl).GetMethod("SpawnCpu", BindingFlags.Instance | BindingFlags.NonPublic);

        [HarmonyPostfix]
        static void PutInEvilHostage(SpawnCtrl __instance)
        {
            // The chance of a hostage appearing in a battle zone is 50%.
            // As such, this change keeps about the same number of evil hostages in the game,
            // but also preserves the good ones.

            var spawnCtrl = __instance;
            if (!CustomHell.IsHellEnabled(spawnCtrl.ctrl.runCtrl, CustomHellPassEffect.EVIL_HOSTAGE_IF_NO_GOOD_ONE)) {
                return;
            }

            BattleGrid battleGrid = spawnCtrl.ti.mainBattleGrid;
            foreach (Cpu currentEnemy in battleGrid.currentEnemies)
            {
                if (currentEnemy.beingObj.tags.Contains(Tag.Hostage))
                {
                    Debug.Log("Already a hostage, not spawning evil hostage.");
                    return;
                }
            }

            Debug.Log("No good hostages, spawning an evil one!");
            SpawnCpu(spawnCtrl, "HostageEvil");
        }

        private static void SpawnCpu(SpawnCtrl ctrl, string beingId)
        {
            SPAWN_CPU.Invoke(ctrl, new object[] { beingId });
        }
    }
}