using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch]
    internal sealed class InfiniteHealthInitPatch {
        private static FieldInfo _allowLocalPlayerDeathField;
        
        public static MethodBase TargetMethod() {
            return AccessTools.Method("StartOfRound:Awake");
        }
        
        public static void Postfix() {
            var settings = PluginUtility.GetUserSettings();
            if (!settings) return;
            
            var startOfRound = PluginUtility.GetStartOfRound();
            if (!startOfRound) return;
            
            _allowLocalPlayerDeathField ??= startOfRound.GetType().GetField("allowLocalPlayerDeath");
            _allowLocalPlayerDeathField.SetValue(startOfRound, !settings.Stats.infiniteHealth);
        }
    }
    
    [InjectPatch]
    internal sealed class InfiniteHealthRuntimePatch {
        private static FieldInfo _allowLocalPlayerDeathField;
        
        public static MethodBase TargetMethod() {
            return AccessTools.Method("PlayerControllerB:Update");
        }
        
        public static void Postfix() {
            var settings = PluginUtility.GetUserSettings();
            if (!settings) return;
            
            var startOfRound = PluginUtility.GetStartOfRound();
            if (!startOfRound) return;
            
            _allowLocalPlayerDeathField ??= startOfRound.GetType().GetField("allowLocalPlayerDeath");
            
            var currentValue = _allowLocalPlayerDeathField.GetValue(startOfRound);
            var infiniteHealthEnabled = settings.Stats.infiniteHealth;
            if ((bool)currentValue == infiniteHealthEnabled) {
                _allowLocalPlayerDeathField.SetValue(startOfRound, !infiniteHealthEnabled);
            }
        }
    }
}