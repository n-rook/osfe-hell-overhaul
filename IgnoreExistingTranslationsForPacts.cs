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

    [HarmonyPatch(typeof(DeckCtrl))]
    [HarmonyPatch("ParseDescription")]
    class EfAppDefense
    {
        [HarmonyPostfix]
        static string InsertCurrentDefense(string __result, ItemObject itemObj)
        {
            // Heavily inspired by current treatment of "efApp.spellPower" in current code
            string text = __result;
            if (text.Contains("efApp.defense"))
            {
                int defense = itemObj?.artObj?.defense ?? itemObj?.pactObj?.defense ?? 0;
                text = text.Replace("efApp.defense", $"<b>{defense}</b>");
                Debug.Log("Changed text to: " + text);
            }
            return text;
        }
    }
}
