using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEditor;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
	[InjectPatch]
    internal sealed class OnlineDisablerPatch {
        private static MethodBase TargetMethod() {
            return AccessTools.Method("PreInitSceneScript:ChooseLaunchOption");
        }
        
        private static bool Prefix(bool online) {
            if (online) {
                EditorUtility.DisplayDialog("Error", "Online mode is not supported!", "OK");
                return false;
            }
    
            return true;
        }
    }
}