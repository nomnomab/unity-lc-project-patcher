using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch]
    internal sealed class InfiniteStaminaPatch {
        private static readonly FieldInfo _sprintMeterField = AccessTools
            .TypeByName("PlayerControllerB")
            .GetField("sprintMeter");
        
        public static MethodBase TargetMethod() {
            return AccessTools.Method("PlayerControllerB:Update");
        }
        
        public static void Postfix(object __instance) {
            var settings = PluginUtility.GetUserSettings();
            if (!settings) return;
            if (!settings.Stats.infiniteStamina) return;

            var value = (float) _sprintMeterField.GetValue(__instance);
            if (value >= 1f) return;
            
            _sprintMeterField.SetValue(__instance, 1f);
        }
    }
}