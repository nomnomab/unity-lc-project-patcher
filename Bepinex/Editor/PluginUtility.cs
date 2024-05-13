using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    internal static class PluginUtility {
        private static LethalCompanyPluginSettings _settings;
        private static MonoBehaviour _startOfRound;
        private static MonoBehaviour _terminal;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnLoad() {
            _settings = GetUserSettings();
            _startOfRound = null;
            _terminal = null;
            GetUserSettings();
        }
        
        public static LethalCompanyPluginSettings GetUserSettings() {
            if (_settings) return _settings;
            
            var assets = AssetDatabase.FindAssets($"t:{nameof(LethalCompanyPluginSettings)}");
            if (assets.Length == 0) {
                CreateUserSettings();
                Debug.LogWarning("Created LethalCompanyPluginSettings asset since it was missing");
                assets = AssetDatabase.FindAssets($"t:{nameof(LethalCompanyPluginSettings)}");
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
            _settings = AssetDatabase.LoadAssetAtPath<LethalCompanyPluginSettings>(assetPath);
            return _settings;
        }
        
        public static void CreateUserSettings() {
            // create one at root
            var settings = ScriptableObject.CreateInstance<LethalCompanyPluginSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/LethalCompanyPluginSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static MonoBehaviour GetStartOfRound() {
            if (_startOfRound) {
                return _startOfRound;
            }
            
            _startOfRound = GameObject
                .FindObjectsOfType<MonoBehaviour>()
                .FirstOrDefault(x => x.GetType().Name == "StartOfRound");

            return _startOfRound;
        }

        public static MonoBehaviour GetTerminal() {
            if (_terminal) {
                return _terminal;
            }
            
            _terminal = GameObject
                .FindObjectsOfType<MonoBehaviour>()
                .FirstOrDefault(x => x.GetType().Name == "Terminal");

            return _terminal;
        }
    }
}