using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hell_Overhaul
{
    public enum CustomHellPassEffect
    {
        // note that the underlying ints of these enums are NOT the same as their hell pass values
        NO_BUSY_ROOM_SLOWDOWN
    }

    public static class CustomHell
    {
        // If A: i, then A is enabled at or above hell pass i
        private static Dictionary<CustomHellPassEffect, int> HellLevels = new Dictionary<CustomHellPassEffect, int>()
        {
            { CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN, 14 }
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
            return hellPassNumber >= HellLevels[e];
        }
    }
}
