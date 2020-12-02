using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnHit")]
    class OnHitPlayerLoseDefense
    {
        [HarmonyPostfix]
        static void DoStuff(Player __instance, Projectile attackRef)
        {
            Debug.Log($"Projectile is: ${attackRef}");
            if (attackRef != null)
            {
                Debug.Log(attackRef.name);
                Debug.Log(attackRef.being);
                Debug.Log(attackRef.spell.name);
                Debug.Log(attackRef.spellObj.shortName);
            }
        }
    }
}
