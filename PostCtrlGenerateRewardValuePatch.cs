using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace Hell_Overhaul
{
    [HarmonyPatch(typeof(PostCtrl))]
    [HarmonyPatch("GenerateRewardValue")]
    class PostCtrlGenerateRewardValuePatch
    {
        private static readonly FieldInfo RUN_CTRL = typeof(PostCtrl).GetField("runCtrl", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly int REWARD_PENALTY = 10;
        private static readonly int DOUBLE_FOCUS_PENALTY = 5;

        private static RunCtrl getRunCtrl(PostCtrl postCtrl)
        {
            RunCtrl runCtrl = (RunCtrl) RUN_CTRL.GetValue(postCtrl);
            if (runCtrl == null)
            {
                throw new ArgumentException("postCtrl.runCtrl is null. Maybe code changed? Complain to hell dev.");
            }
            return runCtrl;
        }

        [HarmonyPrefix]
        static void DowngradeRewardQuality(PostCtrl __instance, ref int bonus)
        {
            var postCtrl = __instance;
            RunCtrl runCtrl = getRunCtrl(postCtrl);
            if (CustomHell.IsHellEnabled(runCtrl, CustomHellPassEffect.BAD_REWARDS))
            {
                UnityEngine.Debug.Log($"Downgrading bonus luck by {REWARD_PENALTY}");
                bonus -= REWARD_PENALTY;
            }
        }
    }
}
