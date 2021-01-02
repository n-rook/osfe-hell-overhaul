using System.Collections.Generic;
using System.Linq;
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
        DOUBLE_FOCUS_LUCK_PENALTY,
        IMPERFECT_SHIELDS,
        LATE_BOSSES_HARDER,
        LOSE_DEF_ON_HIT,
        HALF_MANA_REGEN,
        SPECIAL_CREDITS_THEME,
    }

    public static class CustomHell
    {
        // If A: i, then A is enabled at hell pass i
        private static Dictionary<CustomHellPassEffect, int> HellLevels = new Dictionary<CustomHellPassEffect, int>()
        {
            { CustomHellPassEffect.NO_BUSY_ROOM_SLOWDOWN, 14 },
            { CustomHellPassEffect.NO_INITIAL_REMOVE, 15 },
            { CustomHellPassEffect.TWO_REMOVES_FOR_PACT, 15 },
            { CustomHellPassEffect.BAD_REWARDS, 16 },
            { CustomHellPassEffect.DOUBLE_FOCUS_LUCK_PENALTY, 16 },
            { CustomHellPassEffect.IMPERFECT_SHIELDS, 17 },
            { CustomHellPassEffect.LATE_BOSSES_HARDER, 18 },
            { CustomHellPassEffect.SPECIAL_CREDITS_THEME, 18 },
            { CustomHellPassEffect.HALF_MANA_REGEN, 19 },
            // Known issue: Modified stats from artifacts aren't saved properly.
            // Cheating is wrong, so don't do it.
            { CustomHellPassEffect.LOSE_DEF_ON_HIT, 20 }
        };

        /**
         * Return the actual artifact entity for a given hell pact. May return null.
         */
        public static PactObject GetHellPact(Player p, CustomHellPassEffect e)
        {
            int num = HellLevels[e];
            string expectedId = $"HellPass{num}";
            foreach (PactObject pactObj in p.pactObjs)
            {
                if (pactObj.itemID == expectedId)
                {
                    return pactObj;
                }
            }
            Debug.LogWarning($"Could not find {expectedId}");
            return null;
        }

        /**
         * Get a description for a hell pass.
         * 
         * This exists because HP20 has a variable in its description that we have to manually exclude.
         */
        public static string GetGenericDescription(PactObject pactObj)
        {
            string text = pactObj.description;
            if (pactObj.itemID == "HellPass20")
            {
                text = text.Replace(" (efApp.defense)", "");
            }

            return text;
        }

        public static bool IsHellEnabled(RunCtrl rc, CustomHellPassEffect e)
        {
            // currentHellPasses is a list of all currently enabled hell passes, by index
            return IsHellEnabled(rc.currentHellPasses, e);
        }

        /**
         * Return whether a given effect should be enabled
         */
         public static bool IsHellEnabled(Run r, CustomHellPassEffect e)
        {
            return IsHellEnabled(r.hellPasses, e);
        }

        public static bool IsHellEnabled(Player p, CustomHellPassEffect e)
        {
            return IsHellEnabled(p.runCtrl, e);
        }

        private static bool IsHellEnabled(List<int> hellPasses, CustomHellPassEffect e)
        {
            if (!HellLevels.ContainsKey(e))
            {
                Debug.LogWarning($"Hell effect not in hell effect dictionary: {e}");
                return false;
            }
            int desiredHellPass = HellLevels[e];

            foreach (var i in hellPasses) {
                if (i == desiredHellPass)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
