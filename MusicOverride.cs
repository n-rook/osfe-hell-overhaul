using E7.Introloop;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(MusicCtrl))]
    [HarmonyPatch("PlayBattle")]
    class MusicOverride
    {
        [HarmonyPrepare]
        public static bool InitializeAudioClips()
        {
            return true;
        }

        [HarmonyPrefix]
        public static bool PlayBattle(MusicCtrl __instance)
        {
            return true;

            // debugging
            var clip = MusicHelper.GetAudioClipOrNull(MusicHelper.GHOST_OF_EDEN_KEY);
            if (clip == null)
            {
                return true;
            }
            Debug.Log("Playing hopefully");
            __instance.StopIntroLoop();
            __instance.Play(clip);

            return false;
        }
    }
}
