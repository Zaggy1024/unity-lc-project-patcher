using System.IO;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;

namespace Nomnom.LethalCompanyProjectPatcher.Editor {
    [UPPatcher("com.nomnom.unity-lc-project-patcher")]
    public static class LethalCompanyWrapper {
        public static void GetSteps(StepPipeline stepPipeline) {
            stepPipeline.SetInputSystem(InputSystemType.InputSystem_New);
            stepPipeline.IsUsingNetcodeForGameObjects();
            stepPipeline.InsertAfter<InjectHDRPAssetsStep>(
                new PatchLDRTexturesStep("LDR_RGB1_"),
                new PatchDiageticAudioMixersStep("Diagetic.mixer"),
                new ChangeSceneListStep("InitSceneLaunchOptions")
            );
            
            stepPipeline.InsertAfter<AssetRipperStep>(
                new MoveFilesInExportStep(
                    (Path.Combine("Scripts", "Assembly-CSharp", "DunGen", "Editor", "*"), Path.Combine("Scripts", "Assembly-CSharp", "DunGen"))
                )
            );
            stepPipeline.InsertAfter<CopyAssetRipperExportToProjectStep>(
                // https://github.com/flamacore/UnityHDRPSimpleWater/tree/master
                new MigrateProjectMaterialsStep(
                    ("VowWater", "Packages/com.nomnom.unity-lc-project-patcher/Runtime/Water/VowWater_REPLACEMENT.mat"),
                    ("Water_mat_04", "Packages/com.nomnom.unity-lc-project-patcher/Runtime/Water/Water_mat_04_REPLACEMENT.mat")
                )
            );
            stepPipeline.SetGameViewResolution("16:9");
            stepPipeline.OpenSceneAtEnd("InitSceneLaunchOptions");
        }
    }
}