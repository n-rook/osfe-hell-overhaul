using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(HellPassListCard))]
    [HarmonyPatch("UpdatePactText")]
    class HellPassListCardDescriptionPatch
    {
        [HarmonyPostfix]
        static void Postfix(HellPassListCard __instance)
        {
            HellPassListCard listCard = __instance;
            var currentPass = listCard.pactObj;
            string description = CustomHell.GetGenericDescription(currentPass);

            Debug.Log($"Fixing description post-hoc; {listCard.tmpText.text} -> {description}");
            listCard.tmpText.text = description;
        }
    }
}
