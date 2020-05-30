using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hell_Overhaul
{
    public enum CustomHellPassEffect
    {
        // note that the underlying ints of these enums are NOT the same as their hell pass values
        NO_BUSY_ROOM_SLOWDOWN,
        NO_INITIAL_REMOVE,  // not yet implemented
        TWO_REMOVES_FOR_PACT,
    }

    public static class CustomHell
    {
        // If A: i, then A is enabled at or above hell pass i
        private static Dictionary<CustomHellPassEffect, int> HellLevels = new Dictionary<CustomHellPassEffect, int>()
        {
            { CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN, 14 },
            { CustomHellPassEffect.NO_INITIAL_REMOVE, 15 },
            { CustomHellPassEffect.TWO_REMOVES_FOR_PACT, 15 },
        };

        public static bool IsHellEnabled(RunCtrl rc, CustomHellPassEffect e)
        {
            return IsHellEnabled(rc.currentHellPassNum, e);
        }

        /**
         * Return whether a given effect should be enabled
         */
         public static bool IsHellEnabled(Run r, CustomHellPassEffect e)
        {
            return IsHellEnabled(r.hellPassNum, e);
        }

        private static bool IsHellEnabled(int hellPassNumber, CustomHellPassEffect e)
        {
            if (!HellLevels.ContainsKey(e))
            {
                Debug.LogWarning($"Hell effect not in hell effect dictionary: {e}");
                return false;
            }
            return hellPassNumber >= HellLevels[e];
        }
    }
}
