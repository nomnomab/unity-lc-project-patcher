using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;

namespace Nomnom.LethalCompanyProjectPatcher.Editor {
    [UPPatcher]
    public static class LethalCompanyWrapper {
        public static void GetSteps(StepPipeline stepPipeline) {
            stepPipeline.SetInputSystem(InputSystemType.InputSystem_New);
            stepPipeline.IsUsingNetcodeForGameObjects();
            stepPipeline.InsertAfter<InjectHDRPAssetsStep>(
                new PatchLDRTexturesStep("LDR_RGB1_"),
                new PatchDiageticAudioMixersStep("Diagetic.mixer"),
                new ChangeSceneListStep("InitSceneLaunchOptions")
            );
            stepPipeline.InsertBefore<CopyAssetRipperExportToProjectStep>(
                new MakeProxyScriptsStep(new MakeProxyScriptsStep.Proxy("ES3Defaults", "LethalCompany"))
            );
            stepPipeline.SetGameViewResolution("16:9");
            stepPipeline.OpenSceneAtEnd("InitSceneLaunchOptions");
            stepPipeline.InsertLast(new FixES3Step());
            stepPipeline.InsertAfter<FixES3Step>(new RenameAnimatorParametersStep(
                    new RenameAnimatorParametersStep.Replacement("SunAnimContainer", ("eclipse", "eclipsed")),
                    new RenameAnimatorParametersStep.Replacement("SunAnimContainer 1", ("eclipse", "eclipsed"))
                )
            );
        }
    }
}