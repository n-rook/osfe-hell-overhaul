using HarmonyLib;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(DeckCtrl))]
    [HarmonyPatch("ParseDescription")]
    class IgnoreExistingTranslationsForPacts
    {
        [HarmonyPrefix]
        static void DeleteTranslationIDIfHellPass(ItemObject itemObj, ref string translationID)
        {
            // For some reason, checking "itemObj.pactObj.hellpass doesn't work.
            if (itemObj.type != ItemType.Pact || !itemObj.itemID.StartsWith("HellPass1"))
            {
                Debug.Log($"Item is type {itemObj.type} or not hellpass so ignoring");
                return;
            }

            Debug.Log("Replacing translation ID with dummy string to prevent translations from being looked up.");
            translationID = "NonExistentHellPass";
        }

        //[HarmonyPostfix]
        //static void LogOutcome(string __result, ItemObject itemObj, string translationID)
        //{
        //    Debug.Log($"Got translation for {itemObj?.itemID}, desc {itemObj?.description}, translationID {translationID}, outcome {__result}");
        //}
    }
}
