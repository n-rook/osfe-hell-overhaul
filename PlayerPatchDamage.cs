using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    //[HarmonyPatch(typeof(Being))]
    //[HarmonyPatch("HitAmount")]
    //class DebugDebug
    //{
    //    [HarmonyPrefix]
    //    static void Log(int damage,
    //bool onHitTriggerArts,
    //bool onHitShake,
    //Projectile attackRef,
    //bool pierceDefense,
    //bool pierceShield,
    //bool link)
    //    {
    //        Debug.Log($"HitAmount {damage} {attackRef == null} {attackRef} {attackRef?.name} {attackRef?.spell} {attackRef?.spellObj}");
    //    }
    //}

    //[HarmonyPatch(typeof(SpellObject))]
    //[HarmonyPatch("OnHit")]
    //class SpellObjectDebugOnHit
    //{
    //    [HarmonyPrefix]
    //    static void Log(SpellObject __instance, Being hitBeing)
    //    {
    //        Debug.Log($"SpellObject#OnHit {__instance?.itemID}, {hitBeing?.name}");
    //    }

    //    [HarmonyPostfix]
    //    static void Log()
    //    {
    //        Debug.Log("SpellObject#OnHit postfix");
    //    }
    //}

    internal static class ConcussionGlobals
    {
        internal static bool ConcussionOkay { get; private set; } = true;

        internal static void TemporarilyDisableConcussionDamage()
        {
            ConcussionOkay = false;
        }

        internal static void ReenableConcussionDamage()
        {
            ConcussionOkay = true;
        }
    }

    [HarmonyPatch(typeof(SpellObject))]
    [HarmonyPatch("OnHit")]
    class DisableExtraDamageWhileCertainEffectsAreResolved
    {
        private static string UNSTOPPABLE_VIO_ATTACK = "ViPathPunish";

        public delegate void ConcussionHitAmountCallback();

        private static void DoNothing() { }
        private static void ReenableConcussionDamage()
        {
            Debug.Log("Reenabling conclussion damage.");
            ConcussionGlobals.ReenableConcussionDamage();
        }

        private static bool blockConcussion(SpellObject obj, Being hitBeing)
        {
            Debug.Log($"obj: {obj.itemID}, hitBeing: {hitBeing?.name} {hitBeing?.GetType()}");
            Player p = hitBeing as Player;
            if (p is null || !CustomHell.IsHellEnabled(p, CustomHellPassEffect.IMPERFECT_SHIELDS))
            {
                return false;
            }
            if (obj.itemID != UNSTOPPABLE_VIO_ATTACK)
            {
                return false;
            }

            Debug.Log($"Since spell is {obj.itemID}, disabling concussion damage.");
            return true;
        }

        [HarmonyPrefix]
        static void OnHitPrefix(SpellObject __instance, Being hitBeing, out ConcussionHitAmountCallback __state)
        {
            if (!blockConcussion(__instance, hitBeing))
            {
                __state = DoNothing;
                return;
            }

            ConcussionGlobals.TemporarilyDisableConcussionDamage();
            __state = ReenableConcussionDamage;
        }

        [HarmonyPostfix]
        static void HitAmountPostfix(ConcussionHitAmountCallback __state)
        {
            Debug.Log("Postfix");
            __state();
        }
    }

    [HarmonyPatch(typeof(Being))]
    [HarmonyPatch("Damage")]
    class PlayerPatchDamage
    {
        // health damage = shield damage / this value
        private static readonly int DIVISOR = 10;

        [HarmonyPrefix]
        static void AddShieldDamage(Being __instance, int amount, bool pierceDefense, bool pierceShield, bool pierceInvince, ItemObject itemObj)
        {
            Debug.Log($"ItemObj is null?? {itemObj == null}");
            Debug.Log($"ItemObj {itemObj}");
            Debug.Log($"ItemObj id {itemObj?.itemID}");

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

            if (!ConcussionGlobals.ConcussionOkay)
            {
                Debug.Log("Concussion temporarily disabled due to unblockable attack");
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
            Debug.Log($"Player invincetime is {player.invinceTime}");
            return (player.invinceTime >= Time.time && !pierceInvince);
        }
    }
}
