using System;
using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch]
    internal static class SkipTerminalIntroPatch {
        private static MethodBase TargetMethod() {
            // return AccessTools.Method("PreInitSceneScript:Start");
            return AccessTools.Method("Terminal:BeginUsingTerminal");
        }
        
        private static void Prefix(object __instance) {
            var settings = PluginUtility.GetUserSettings();
            if (!settings) return;
            if (!settings.Terminal.skipIntro) return;

            try {
                var es3Save = AccessTools.Method("ES3:Save", new[] { typeof(string), typeof(object), typeof(string) });
                es3Save.Invoke(null, new object[] { "HasUsedTerminal", true, "LCGeneralSaveData" });
            } catch(Exception e) {
                Debug.LogError("Failed to save HasUsedTerminal to ES3.");
                Debug.LogException(e);
            }

            try {
                var usedTerminalThisSession = __instance.GetType().GetField("usedTerminalThisSession", BindingFlags.NonPublic | BindingFlags.Instance);
                usedTerminalThisSession.SetValue(__instance, true);
            } catch(Exception e) {
                Debug.LogError("Failed to set usedTerminalThisSession to true.");
                Debug.LogException(e);
            }
        }
    }
}