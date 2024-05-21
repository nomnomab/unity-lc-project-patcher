using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [CreateAssetMenu(fileName = "LethalCompanyBepinexPluginSettings", menuName = "Unity Project Patcher/Lethal Company Plugin Settings", order = 0)]
    public sealed class LethalCompanyPluginSettings: ScriptableObject {
        public StatsData Stats => _stats;
        public SkipIntroData SkipIntro => _skipIntro;
        public TerminalData Terminal => _terminal;
        public MoonData Moon => _moon;

        [SerializeField] private StatsData _stats = new StatsData();
        [SerializeField] private SkipIntroData _skipIntro = new SkipIntroData();
        [SerializeField] private TerminalData _terminal = new TerminalData();
        [SerializeField] private MoonData _moon = new MoonData();
        
        [Serializable]
        public struct StatsData {
            public bool infiniteHealth;
            public bool infiniteStamina;
        }

        [Serializable]
        public struct SkipIntroData {
            public SkipIntroTarget target;
            public int saveFileIndex;
            public bool resetSaveFile;
        }

        [Serializable]
        public struct TerminalData {
            public bool skipIntro;
            public bool startingCreditsEnabled;
            public int startingCredits;
        }

        [Serializable]
        public struct MoonData {
            public bool autoLoadMoonEnabled;
            public string levelSceneName;
            
#if ENABLE_BEPINEX
            [TypeObjectField("SelectableLevel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")]
#endif
            public ScriptableObject selectableLevel;
        }

        public enum SkipIntroTarget {
            None,
            IntoMainMenu,
            IntoGame
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Unity Project Patcher/Configs/" + nameof(LethalCompanyPluginSettings))]
        private static void OpenUPPatcherUserSettings() {
            var config = GetSettings();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = config;
            UnityEditor.EditorGUIUtility.PingObject(config);
        }

        private static LethalCompanyPluginSettings GetSettings() {
            var assets = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(LethalCompanyPluginSettings)}");
            if (assets.Length == 0) {
                var settings = ScriptableObject.CreateInstance<LethalCompanyPluginSettings>();
                UnityEditor.AssetDatabase.CreateAsset(settings, "Assets/LethalCompanyPluginSettings.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                Debug.LogWarning($"Created {nameof(LethalCompanyPluginSettings)} asset since it was missing");
                assets = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(LethalCompanyPluginSettings)}");
            }

            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<LethalCompanyPluginSettings>(assetPath);
        }
#endif
    }
}