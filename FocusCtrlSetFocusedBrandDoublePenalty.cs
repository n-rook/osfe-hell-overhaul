using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Hell_Overhaul
{


    [HarmonyPatch(typeof(FocusCtrl))]
    [HarmonyPatch("SetFocusedBrand")]
    class FocusCtrlSetFocusedBrandDoublePenalty
    {
        private static readonly int DOUBLE_FOCUS_LUCK_PENALTY = 5;

        private static bool HasDoubleFocus(FocusCtrl focusCtrl)
        {
            HashSet<Brand> focusedBrands = new HashSet<Brand>();
            foreach (var brandListCard in focusCtrl.brandDisplayButtons)
            {
                var brand = brandListCard.brand;
                if (brand == Brand.None)
                {
                    continue;
                }

                if (focusedBrands.Contains(brand))
                {
                    return true;
                }
                focusedBrands.Add(brand);
            }

            return false;
        }

        [HarmonyPostfix]
        static void SetFocusedBrand(FocusCtrl __instance)
        {
            var focusCtrl = __instance;
            var postCtrl = S.I.poCtrl;
            if (CustomHell.IsHellEnabled(S.I.runCtrl, CustomHellPassEffect.DOUBLE_FOCUS_LUCK_PENALTY) && HasDoubleFocus(focusCtrl))
            {
                Debug.Log("Double focus; decreasing focus luck");
                postCtrl.focusLuck -= DOUBLE_FOCUS_LUCK_PENALTY;
                focusCtrl.deCtrl.statsScreen.UpdateStatsText(null);
            }
        }
    }
}
