using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(ListCard))]
    [HarmonyPatch("RemoveThisCard")]
    class ListCardRemoveThisCardPatch
    {
        private static readonly int HELL_PRICE = 2;
        private static readonly FieldInfo DECK_SCREEN = typeof(ListCard).GetField("deckScreen", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo REMOVE_CARD = typeof(ListCard).GetMethod("_RemoveCard", BindingFlags.Instance | BindingFlags.NonPublic);

        private static bool IsNormalPact(ListCard card)
        {
            return card.itemObj.type == ItemType.Pact && !card.itemObj.pactObj.hellPass;
        }

        private static DeckScreen GetDeckScreen(ListCard card)
        {
            return (DeckScreen) DECK_SCREEN.GetValue(card);
        }

        [HarmonyPrefix]
        static bool Prefix(ListCard __instance, bool useRemover)
        {
            var card = __instance;
            RunCtrl runCtrl = card.deCtrl.runCtrl;

            if (!CustomHell.IsHellEnabled(runCtrl, CustomHellPassEffect.TWO_REMOVES_FOR_PACT))
            {
                return true;
            }

            if (!IsNormalPact(card) || !useRemover)
            {
                return true;
            }

            DeckScreen deckScreen = GetDeckScreen(card);
            if (deckScreen.busy)
            {
                return true;
            }

            // This is clunky, but so is using a transpiler.
            if (runCtrl.currentRun.removals < HELL_PRICE)
            {
                PlayForbiddenEffect(card, deckScreen);
                return false;
            }

            runCtrl.currentRun.removals -= HELL_PRICE;
            deckScreen.UpdateRemoverCountText();
            PlayRemoveCardEffect(card);
            return false;
        }

        static void PlayForbiddenEffect(ListCard card, DeckScreen deckScreen)
        {
            S.I.PlayOnce(card.btnCtrl.lockedSound, false);
            deckScreen.removerAnimator.SetTrigger("FlashRed");
        }

        static void PlayRemoveCardEffect(ListCard card)
        {
            Debug.Log("Playing effect for removing a card.");
            card.StartCoroutine("_RemoveCard");
        }
    }
}
