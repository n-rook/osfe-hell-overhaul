using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(Being))]
    [HarmonyPatch("Damage")]
    class PlayerPatchDamage
    {
        // health damage = shield damage / this value
        private static readonly int DIVISOR = 10;

        [HarmonyPrefix]
        static void AddShieldDamage(Being __instance, int amount, bool pierceDefense, bool pierceShield, bool pierceInvince, ItemObject itemObj)
        {
            // Self-damage (ie from Corset) does proc this extra damage.
            // So beware!
            if (!(__instance is Player))
            {
                return;
            }

            Player player = (Player) __instance;
            if (!CustomHell.IsHellEnabled(player.runCtrl, CustomHellPassEffect.IMPERFECT_SHIELDS))
            {
                return;
            }

            if (playerDamageWouldReturnImmediately(player, pierceInvince))
            {
                Debug.Log("Returning immediately.");
                return;
            }

            if (pierceShield)
            {
                Debug.Log("Attack pierces shields; returning immediately");
                return;
            }

            var concussionDamage = GetConcussionDamage(player, ExpectedShieldDamage(player, amount, pierceDefense));
            if (concussionDamage > 0)
            {
                player.health.ModifyHealth(-concussionDamage, pierceShield: true);
            }
        }

        private static int GetConcussionDamage(Player player, int damageToShield)
        {
            var damage = damageToShield / DIVISOR;
            // Piercing damage can't kill the player.
            damage = Mathf.Min(damage, player.health.current - 1);
            Debug.Log($"Player sustaining {damage} concussion damage.");
            return damage;
        }

        private static int ExpectedShieldDamage(Player player, int amount, bool pierceDefense)
        {
            // This is a bug, but it's going to be minor and it's tough to fix:
            // Defense may be double-applied in some cases when an attack breaks shields.
            var amountAfterDefense = player.shieldDefense && !pierceDefense ?
                amount - getDefense(player) :
                amount;

            int r = Mathf.Min(amount, player.health.shield);
            Debug.Log($"Expecting {r} shield damage.");
            return r;
        }

        private static int getDefense(Player player)
        {
            var defense = player.beingObj.defense;
            StatusEffect defenseEffect = player.GetStatusEffect(Status.Defense);
            if (defenseEffect != null)
            {
                defense += Mathf.RoundToInt(defenseEffect.amount);
            }
            return defense;
        }

        static bool playerDamageWouldReturnImmediately(Player player, bool pierceInvince)
        {
            // Copies first line of Damage()
            return ((double)player.invinceTime >= (double)Time.time && !pierceInvince || ((UnityEngine.Object)player == (UnityEngine.Object)null));
        }
    }
}
