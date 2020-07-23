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
        private static readonly float SLOW_START_SECS = 2.0f;
        private static readonly float SLOW_START_END_SECS = 3.0f;
        private static readonly float SLOW_START_MODIFIER = .5f;

        private static readonly DelayTracker singletonDelayTracker = new DelayTracker();

        [HarmonyPostfix]
        static void Postfix(Enemy __instance)
        {
            Enemy enemy = __instance;
            if (!CustomHell.IsHellEnabled(enemy.runCtrl, CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN))
            {
                return;
            }

            var newDelay = enemy.baseLoopDelay * EnemyDelayMultiplier(enemy) * SlowStartMultiplier(enemy);
            singletonDelayTracker.trackDelay(enemy, enemy.beingObj.loopDelay, newDelay);

            enemy.beingObj.loopDelay = newDelay;
        }

        private static float EnemyDelayMultiplier(Enemy enemy)
        {
            // Default delay is:
            // 0.9f
            // + .18f if 2 enemies, or
            // + .36f if 3 enemies, or
            // + .54f if 4+ enemies, plus
            // 0.09f per minion, 0.135f per structure

            // PER_ENEMY_DELAY is for "each individual enemy", so we set things up so the delay with
            // 1 enemy is ONE_ENEMY_DELAY
            var delayMultiplier = ONE_ENEMY_DELAY - PER_ENEMY_DELAY;

            foreach (Cpu currentEnemy in enemy.battleGrid.currentEnemies)
            {
                if (!currentEnemy.beingObj.tags.Contains(Tag.Structure) && !currentEnemy.minion)
                {
                    delayMultiplier += PER_ENEMY_DELAY;
                }
            }
            return delayMultiplier;
        }

        private static float SlowStartMultiplier(Enemy enemy)
        {
            // We introduce a "slow start" adjustment so packed enemy rooms don't immediately
            // crush the player

            // TODO: Exempt bosses, not that it matters much.

            float battleDuration = enemy.ctrl.stopWatch.timeInSeconds;

            // With 4 enemies and 2 structures, the normal delay would be .9 + .54 + .27 = 1.71
            // But with this mod, it's 1.2.
            // So for the sake of kindness, we'll start with a 50% additional modifier.

            float multiplierScale;
            if (battleDuration < SLOW_START_SECS)
            {
                multiplierScale = SLOW_START_MODIFIER;
            }
            else if (battleDuration < SLOW_START_END_SECS)
            {
                var timeThroughScalePeriod = battleDuration - SLOW_START_SECS;
                var proportionThroughScalePeriod = timeThroughScalePeriod / (SLOW_START_END_SECS - SLOW_START_SECS);
                multiplierScale = SLOW_START_MODIFIER * (1 - proportionThroughScalePeriod);
            }
            else
            {
                multiplierScale = 0.0f;
            }

            return 1 + multiplierScale;
        }
    }
}
