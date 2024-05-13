using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch]
    internal static class SkipIntroPatch {
        private static MethodBase TargetMethod() {
            // return AccessTools.Method("PreInitSceneScript:Start");
            return AccessTools.Method("PreInitSceneScript:SkipToFinalSetting");
        }
        
        private static void Postfix() {
            var settings = PluginUtility.GetUserSettings();
            if (settings.SkipIntro.target == LethalCompanyPluginSettings.SkipIntroTarget.None) {
                return;
            }
            
            Object.FindObjectOfType<BepInExSceneLifetime>().StartCoroutine(Waiter());
        }
        
        private static IEnumerator Waiter() {
            // yield return new WaitForSeconds(0.5f + 0.2f);
            yield return SceneManager.LoadSceneAsync("InitSceneLANMode");
            yield return new WaitForSeconds(0.2f);
            yield return SceneManager.LoadSceneAsync("MainMenu");
        }
    }
    
    [InjectPatch]
    internal static class MenuSkipper {
        private static MethodBase TargetMethod() {
            return AccessTools.Method("MenuManager:Start");
        }
        
        private static void Postfix(MonoBehaviour __instance) {
            var settings = PluginUtility.GetUserSettings();
            if ((int)settings.SkipIntro.target <= (int)LethalCompanyPluginSettings.SkipIntroTarget.IntoMainMenu) {
                return;
            }
            __instance.StartCoroutine(Waiter());
        }
    
        private static IEnumerator Waiter() {
            yield return new WaitForSeconds(0.1f);
            var menuManagerObj = GameObject.Find("Canvas/MenuManager");
            var menuManager = menuManagerObj.GetComponents<MonoBehaviour>().FirstOrDefault(x => x.GetType().Name == "MenuManager");
            if (!menuManager) {
                Debug.LogError("Failed to find MenuManager!");
                yield break;
            }
            
            var clickHostButtonMethod = AccessTools.Method(menuManager.GetType(), "ClickHostButton");
            clickHostButtonMethod.Invoke(menuManager, null);
            
            // set save file
            var settings = PluginUtility.GetUserSettings();
            // var gameNetworkManager = settings.GetGameNetworkManager();
            // var currentSaveFileName = gameNetworkManager.GetType().GetField("currentSaveFileName");
            // currentSaveFileName.SetValue(gameNetworkManager, settings.SaveFileIndex == -1 ? "LCChallengeFile" : $"LCSaveFile{settings.SaveFileIndex + 1}");
            var lobbyHostSettingsObj = GameObject.Find("LobbyHostSettings");
            var filesPanelObj = lobbyHostSettingsObj.transform.Find("FilesPanel");
            var files = filesPanelObj
                .GetComponentsInChildren<MonoBehaviour>()
                .Where(x => x.GetType().Name == "SaveFileUISlot")
                .ToArray();

            var saveFileIndex = settings.SkipIntro.saveFileIndex;
            if (saveFileIndex > files.Length) {
                saveFileIndex = files.Length - 1;
                Debug.LogWarning("Save file index out of range. Setting to last available save file.");
            }
            
            var wantedFile = files.FirstOrDefault(x => (int)(x.GetType().GetField("fileNum")?.GetValue(x) ?? -1) == saveFileIndex);
            if (!wantedFile) {
                Debug.LogError($"Failed to save file for index {saveFileIndex}!");
                yield break;
            }
            
            Debug.Log($"> Using {wantedFile}");

            if (settings.SkipIntro.resetSaveFile) {
                var deleteFileButtonObj = lobbyHostSettingsObj.transform.parent.Find("DeleteFileConfirmation/Panel/Delete");
                var deleteFileButton = deleteFileButtonObj
                    .GetComponents<MonoBehaviour>()
                    .FirstOrDefault(x => x.GetType().Name == "DeleteFileButton");
                deleteFileButton.GetType().GetMethod("SetFileToDelete").Invoke(deleteFileButton, new object[] {saveFileIndex});
                deleteFileButton.GetType().GetMethod("DeleteFile").Invoke(deleteFileButton, null);
                Debug.Log($"> Deleted save file {saveFileIndex}.");
            }

            // var saveFileNum = gameNetworkManager.GetType().GetField("saveFileNum");
            // saveFileNum.SetValue(gameNetworkManager, settings.SaveFileIndex == -1 ? -1 : settings.SaveFileIndex);

            wantedFile.GetType().GetMethod("SetFileToThis").Invoke(wantedFile, null);
            Debug.Log($"> Set save file to {saveFileIndex}.");
            
            yield return new WaitForSeconds(0.1f);
            
            var confirmHostButtonMethod = AccessTools.Method(menuManager.GetType(), "ConfirmHostButton");
            confirmHostButtonMethod.Invoke(menuManager, null);
        }
    }
}