using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [HarmonyPriority(priority: Priority.Last)]
    [InjectPatch]
    internal static class AutomaticMoonLandingPatch {
        public static MethodBase TargetMethod() {
            return AccessTools.Method("StartOfRound:Start");
        }
        
        public static void Postfix() {
            var settings = PluginUtility.GetUserSettings();
            var moon = settings.Moon;
            if (!moon.autoLoadMoonEnabled) return;
            if (!moon.selectableLevel && string.IsNullOrEmpty(moon.levelSceneName)) {
                return;
            }
            
            var startOfRound = PluginUtility.GetStartOfRound();
            if (!startOfRound) return;

            startOfRound.StartCoroutine(LoadMoon());
        }
        
        private static IEnumerator LoadMoon() {
            yield return new WaitForSeconds(0.1f);
            
            var settings = PluginUtility.GetUserSettings();
            int levelId = 0;
            var startOfRound = PluginUtility.GetStartOfRound();

            var moon = settings.Moon;
            if (moon.selectableLevel) {
                levelId = (int)moon.selectableLevel.GetType().GetField("levelID").GetValue(moon.selectableLevel);
            } else if(!string.IsNullOrEmpty(moon.levelSceneName)) {
                var levels = (Array)startOfRound.GetType().GetField("levels").GetValue(startOfRound);
                var found = false;
                foreach (var level in levels) {
                    var sceneName = (string)level.GetType().GetField("sceneName").GetValue(level);
                    if (sceneName == moon.levelSceneName) {
                        levelId = (int)level.GetType().GetField("levelID").GetValue(level);
                        found = true;
                        break;
                    }
                }
                
                if (!found) {
                    Debug.LogError($"Failed to find level with scene name {moon.levelSceneName}");
                    yield break;
                }
            } else {
                yield break;
            }
            
            var changeLevelFunction = AccessTools.Method("StartOfRound:ChangeLevel");
            var arriveAtLevelFunction = AccessTools.Method("StartOfRound:ArriveAtLevel");
            changeLevelFunction.Invoke(startOfRound, new object[] { levelId });
            arriveAtLevelFunction.Invoke(startOfRound, null);

            yield return new WaitForSeconds(0.1f);
            
            var pullLeverFunction = AccessTools.Method("StartOfRound:StartGame");
            pullLeverFunction.Invoke(startOfRound, null);
            Debug.Log("Auto loaded moon.");
        }
    }
}