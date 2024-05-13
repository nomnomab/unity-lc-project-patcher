using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Nomnom.BepInEx.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nomnom.LethalCompanyProjectPatcher.BepInEx {
    [InjectPatch(PatchLifetime.Always)]
    public sealed class FixSelectableLevelsPatch {
        public static MethodBase TargetMethod() {
            return AccessTools.Method("PreInitSceneScript:Awake");
        }

        public static void Postfix() {
            var selectableLevelType = Type.GetType("SelectableLevel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            if (selectableLevelType == null) {
                Debug.LogError("SelectableLevel type not found!");
                return;
            }
            
            var levels = AssetDatabase.FindAssets("t:SelectableLevel")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(x => AssetDatabase.LoadAssetAtPath<ScriptableObject>(x));

            foreach (var level in levels) {
                // Debug.Log($"[{level.name}] Checking");

                // arrays
                if (BepInExUtility.TryRemoveNullsFromArray(level, AccessTools.Field(selectableLevelType, "randomWeathers"))) {
                    Debug.LogWarning($"[{level.name}] Removed null weather");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromArray(level, AccessTools.Field(selectableLevelType, "dungeonFlowTypes"))) {
                    Debug.LogWarning($"[{level.name}] Removed null dungeon flow type");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromArray(level, AccessTools.Field(selectableLevelType, "spawnableMapObjects"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "prefabToSpawn").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null spawnable map object");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromArray(level, AccessTools.Field(selectableLevelType, "spawnableOutsideObjects"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "spawnableObject").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null spawnable outside object");
                    EditorUtility.SetDirty(level);
                }

                // lists
                if (BepInExUtility.TryRemoveNullsFromList(level, AccessTools.Field(selectableLevelType, "spawnableScrap"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "spawnableItem").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null spawnable scrap");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromList(level, AccessTools.Field(selectableLevelType, "Enemies"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "enemyType").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null enemy");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromList(level, AccessTools.Field(selectableLevelType, "OutsideEnemies"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "enemyType").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null outside enemy");
                    EditorUtility.SetDirty(level);
                }

                if (BepInExUtility.TryRemoveNullsFromList(level, AccessTools.Field(selectableLevelType, "DaytimeEnemies"), x => BepInExUtility.IsValidRef(AccessTools.Field(x.GetType(), "enemyType").GetValue(x)))) {
                    Debug.LogWarning($"[{level.name}] Removed null daytime enemy");
                    EditorUtility.SetDirty(level);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}