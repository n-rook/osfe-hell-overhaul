using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hell_Overhaul
{
    // How I make custom hell mode images
    // 1. Take 256x256 blank canvas
    // 2. Paste hell sprite in so it's in the center
    // 3. Edit as needed (hue: 130)
    // In theory I could make custom images but... I'm not an artist.

    public enum CustomHellPassEffect
    {
        // note that the underlying ints of these enums are NOT the same as their hell pass values
        EVIL_HOSTAGE_IF_NO_GOOD_ONE,
        NO_BUSY_ROOM_SLOWDOWN,
        NO_INITIAL_REMOVE,  // not yet implemented
        TWO_REMOVES_FOR_PACT,
        BAD_REWARDS,
        IMPERFECT_SHIELDS,
        LATE_BOSSES_HARDER,
    }

    public static class CustomHell
    {
        // If A: i, then A is enabled at or above hell pass i
        private static Dictionary<CustomHellPassEffect, int> HellLevels = new Dictionary<CustomHellPassEffect, int>()
        {
            { CustomHellPassEffect.EVIL_HOSTAGE_IF_NO_GOOD_ONE, 11 },
            { CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN, 14 },
            { CustomHellPassEffect.NO_INITIAL_REMOVE, 15 },
            { CustomHellPassEffect.TWO_REMOVES_FOR_PACT, 15 },
            { CustomHellPassEffect.BAD_REWARDS, 16 },
            { CustomHellPassEffect.IMPERFECT_SHIELDS, 17 },
            { CustomHellPassEffect.LATE_BOSSES_HARDER, 18 },
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
