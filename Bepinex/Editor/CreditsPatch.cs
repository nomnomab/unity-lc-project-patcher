using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch]
    internal sealed class CreditsPatch {
        private static readonly FieldInfo _groupCredits = AccessTools
            .TypeByName("Terminal")
            .GetField("groupCredits");
        
        public static MethodBase TargetMethod() {
            return AccessTools.Method("Terminal:Start");
        }

        public static void Postfix() {
            var settings = PluginUtility.GetUserSettings();
            if (!settings) return;
            if (!settings.Terminal.startingCreditsEnabled) return;
            
            var terminal = PluginUtility.GetTerminal();
            if (!terminal) return;
            
            _groupCredits.SetValue(terminal, Mathf.Max(0, settings.Terminal.startingCredits));
        }
    }
}