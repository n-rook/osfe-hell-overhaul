using HarmonyLib;
using System;
using UnityEngine;
using E7.Introloop;
using System.IO;
using System.Collections.Generic;


namespace Hell_Overhaul
{
    class MusicHelper
    {
        public static string GHOST_OF_EDEN_KEY = "ghost of eden";

        public static AudioClip GetAudioClipOrNull(string key)
        {
            var allAudioClips = Traverse.Create(S.I.itemMan).Field("allAudioClips").GetValue<Dictionary<String, AudioClip>>();
            if (allAudioClips.ContainsKey(key))
            {
                Debug.Log($"Found audio track {key}");
                return allAudioClips[key];
            }
            else
            {
                Debug.LogWarning($"Could not find audio track {key}");
                return null;
            }

        }

        public static IntroloopAudio GetAudioClipAsIntroloop(string key)
        {
            // When I use this, I can't seem to play the clip multiple times, so it doesn't quite work right.

            var clip = GetAudioClipOrNull(key);
            if (clip == null)
            {
                return null;
            }

            var introloop = ScriptableObject.CreateInstance<IntroloopAudio>();
            introloop.audioClip = clip;
            introloop.Volume = 255;
            Debug.Log($"Loaded {key}");
            return introloop;
        }

        public static void ListAllAudioClipsForDebugging()
        {
            var allAudioClips = Traverse.Create(S.I.itemMan).Field("allAudioClips").GetValue<Dictionary<String, AudioClip>>();
            Debug.Log($"{allAudioClips}");
            Debug.Log($"len ${allAudioClips.Count}");
            foreach (var c in allAudioClips.Keys)
            {
                Debug.Log(c);
            }
        }
    }
}
