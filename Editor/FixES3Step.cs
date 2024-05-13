using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace Nomnom.LethalCompanyProjectPatcher.Editor {
    public readonly struct FixES3Step: IPatcherStep {
        public UniTask<StepResult> Run() {
            var settings = this.GetSettings();
            var arSettings = this.GetAssetRipperSettings();
            if (!arSettings.TryGetFolderMapping("Resources", out var resourcesFolder, out var exclude) || exclude) {
                Debug.Log("Skipping FixES3Step because no resources folder was found");
                return UniTask.FromResult(StepResult.Success);
            }

            var soPath = Path.GetFullPath(Path.Combine(settings.ProjectGameAssetsPath, resourcesFolder, "es3", "ES3Defaults.asset"));
            if (!File.Exists(soPath)) {
                Debug.Log("Skipping FixES3Step because no ES3Defaults was found");
                return UniTask.FromResult(StepResult.Success);
            }

            var newGuid = AssetDatabase.FindAssets("t:MonoScript")
                .Where(x => Path.GetFileName(AssetDatabase.GUIDToAssetPath(x)) == "ES3Defaults.cs")
                .Select(x => Path.GetFullPath(AssetDatabase.GUIDToAssetPath(x)))
                .Where(x => File.Exists(x))
                .Select(x => AssetScrubber.GetMetaGuid(x))
                .FirstOrDefault();
            
            if (string.IsNullOrEmpty(newGuid)) {
                Debug.Log("Skipping FixES3Step because no ES3Defaults.cs was found");
                return UniTask.FromResult(StepResult.Success);
            }
            
            AssetScrubber.ReplaceAssetGuids(soPath, newGuid);
            return UniTask.FromResult(StepResult.Success);
        }

        public void OnComplete(bool failed) { }
    }
}