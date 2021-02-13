using HarmonyLib;
using System;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(HarmonyMain))]
    [HarmonyPatch("LoadMod")]
    class HarmonyMainDoNotPatchTwice
    {
        private static readonly int THIS_MOD_ID = 2098305763;

        [HarmonyPrefix]
        static bool LoadMod(string DLLPath, string modName)
        {
            if (DLLPath.Contains(THIS_MOD_ID.ToString()))
            {
                Debug.LogError("Loading Hell Overhaul a second time! This should never happen. I have done my best to make Hell Overhaul still work under these circumstances, but you may see strange bugs, especially around item descriptions. Proceed at your own risk.");
                return false;
            }

            return true;
        }
    }
}
