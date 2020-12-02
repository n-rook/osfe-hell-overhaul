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
    [HarmonyPatch("PlayCredits")]
    class MusicOverride
    {
        [HarmonyPrepare]
        public static bool InitializeAudioClips()
        {
            return true;
        }

        private static bool playGhostOfEden(MusicCtrl ctrl)
        {
            var clip = MusicHelper.GetAudioClipOrNull(MusicHelper.GHOST_OF_EDEN_KEY);
            if (clip == null)
            {
                return false;
            }
            ctrl.StopIntroLoop();
            ctrl.Play(clip, false);
            return true;
        }

        [HarmonyPrefix]
        public static bool PlayCredits(MusicCtrl __instance, Ending ending)
        {
            MusicCtrl musicCtrl = __instance;
            if (CustomHell.IsHellEnabled(musicCtrl.runCtrl, CustomHellPassEffect.SPECIAL_CREDITS_THEME))
            {
                Debug.Log("Custom hell credits achieved!");
                if (ending == Ending.Genocide)
                {
                    if (playGhostOfEden(musicCtrl))
                    {
                        return false;
                    }
                }
            }
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
