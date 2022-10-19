using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Modula
{
    public static class ModulaSetup
    {
        private static string twistappsFolder => Path.Combine("Assets", "TwistApps");
        private static string modulaFolder => Path.Combine(twistappsFolder, "Modula");
        private const string SettingsFilename = "ModulaSettings";
        
        private static ModulaSetupSettings _settings;
        public static ModulaSetupSettings LoadSettingsAsset()
        {
            var settingsPath = Path.ChangeExtension(Path.Combine(modulaFolder, SettingsFilename), ".asset");
            _settings = (ModulaSetupSettings)AssetDatabase.LoadAssetAtPath(settingsPath, typeof(ModulaSetupSettings));

            if (_settings != null) return _settings;
            
            var asset = ScriptableObject.CreateInstance<ModulaSetupSettings>();
            Directory.CreateDirectory(modulaFolder);
            AssetDatabase.CreateAsset(asset, settingsPath);
            AssetDatabase.SaveAssets();
            
            _settings = asset;
            return _settings;
        }

        // [DidReloadScripts]
        // private static void SetupModula()
        // {
        //     if (_settings == null) LoadSettingsAsset();
        //     if (_settings.setupComplete) return;
        //
        //     AddDefineSymbols();
        //     
        //     _settings.setupComplete = true;
        // }
        
        [InitializeOnLoadMethod]
        public static void AddDefineSymbols()
        {
            var buildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            var symbolsHashSet = new HashSet<string>(symbols.Split(';')) { "MODULA" };

            var modifiedSymbols = string.Join(";", symbolsHashSet);
            if (symbols == modifiedSymbols) return;
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, modifiedSymbols);
            Debug.Log("Adding 'MODULA' to scripting defines...");
        }
    }
}
