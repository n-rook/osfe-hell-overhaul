using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(Enemy))]
    [HarmonyPatch("CalculateLoopDelay")]
    static class EnemyCalculateLoopDelayPatch
    {
        private static readonly float ONE_ENEMY_DELAY = 1.0f;
        private static readonly float PER_ENEMY_DELAY = 0.1f;

        private static readonly float LOG_EVERY_N = 1000;
        private static int log_count = 0;

        [HarmonyPostfix]
        static void Postfix(Enemy __instance)
        {
            if (!CustomHell.IsHellEnabled(__instance.runCtrl, CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN))
            {
                return;
            }

            // Default delay is:
            // 0.9f
            // + .18f if 2 enemies, or
            // + .36f if 3 enemies, or
            // + .54f if 4+ enemies, plus
            // 0.09f per minion, 0.135f per structure

            // PER_ENEMY_DELAY is for "each individual enemy", so we set things up so the delay with
            // 1 enemy is ONE_ENEMY_DELAY
            var delayMultiplier = ONE_ENEMY_DELAY - PER_ENEMY_DELAY;

            foreach (Cpu currentEnemy in __instance.battleGrid.currentEnemies)
            {
                if (!currentEnemy.beingObj.tags.Contains(Tag.Structure) && !currentEnemy.minion)
                {
                    delayMultiplier += PER_ENEMY_DELAY;
                }
            }

            var newDelay = __instance.baseLoopDelay * delayMultiplier;
            LogAdjustment(__instance.beingObj.loopDelay, newDelay);

            __instance.beingObj.loopDelay = newDelay;
        }

        private static void LogAdjustment(float original, float newValue) {
            log_count += 1;
            if (log_count >= LOG_EVERY_N)
            {
                log_count = 0;
                Debug.Log($"Changing loop delay from {original} to {newValue}");
            }
        }
    }
}
