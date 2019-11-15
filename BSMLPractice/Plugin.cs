﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine;
using Harmony;
using IPALogger = IPA.Logging.Logger;
using BeatSaberMarkupLanguage.Settings;
using System.Reflection;

namespace BSMLPractice
{
    public class Plugin : IBeatSaberPlugin
    {
        // TODO: Change YourGitHub to the name of your GitHub account, or use the form "com.company.project.product"
        public const string HarmonyId = "com.github.YourGitHub.BSMLPractice";
        public const string SongCoreHarmonyId = "com.kyle1413.BeatSaber.SongCore";
        internal static HarmonyInstance harmony;
        internal static string Name => "BSMLPractice";
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        #region Gameplay Settings
        internal static bool ExampleGameplayBoolSetting { get; set; }
        public static float ExampleGameplayListSetting { get; set; }
        #endregion


        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            Logger.log.Debug("Logger initialied.");

            configProvider = cfgProvider;

            config = configProvider.MakeLink<PluginConfig>((p, v) =>
            {
                // Build new config file if it doesn't exist or RegenerateConfig is true
                if (v.Value == null || v.Value.RegenerateConfig)
                {
                    Logger.log.Debug("Regenerating PluginConfig");
                    p.Store(v.Value = new PluginConfig()
                    {
                        // Set your default settings here.
                        RegenerateConfig = false,
                        ExampleBoolSetting = false,
                        ExampleIntSetting = 5,
                        ExampleColorSetting = UnityEngine.Color.blue.ToFloatAry(),
                        ExampleTextSegment = 0,
                        ExampleStringSetting = "example",
                        ExampleSliderSetting = 2,
                        ExampleListSetting = 2
                    });
                }
                config = v;
            });
            harmony = HarmonyInstance.Create(HarmonyId);
        }

        public void OnApplicationStart()
        {
            ExampleGameplayBoolSetting = true;
            Logger.log.Debug("OnApplicationStart");
            CustomUI.Utilities.BSEvents.menuSceneLoadedFresh += MenuLoadedFresh;
            ApplyHarmonyPatches();
        }

        /// <summary>
        /// Attempts to apply all the Harmony patches in this assembly.
        /// </summary>
        public static void ApplyHarmonyPatches()
        {
            try
            {
                Logger.log.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error applying Harmony patches: " + ex.Message);
                Logger.log.Debug(ex);
            }
        }

        /// <summary>
        /// Attempts to remove all the Harmony patches that used our HarmonyId.
        /// </summary>
        public static void RemoveHarmonyPatches()
        {
            try
            {
                // Removes all patches with this HarmonyId
                harmony.UnpatchAll(HarmonyId);
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error removing Harmony patches: " + ex.Message);
                Logger.log.Debug(ex);
            }
        }

        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }

        /// <summary>
        /// Called when the active scene is changed.
        /// </summary>
        /// <param name="prevScene">The scene you are transitioning from.</param>
        /// <param name="nextScene">The scene you are transitioning to.</param>
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "HealthWarning")
            {
                var reflectionUtilExample = new GameObject($"{Name}.ReflectionUtilExample").AddComponent<ReflectionUtilExample>();
            }
            if (nextScene.name == "MenuCore")
            {
                CustomUI.MenuButton.MenuButtonUI.AddButton("BSMLPractice", "BSMLPractice hint text.", OnClick);
                BSMLSettings.instance.AddSettingsMenu(Name, "BSMLPractice.Views.BSMLSettingsView.bsml", Views.BSMLSettingsView.instance);
                try
                {
                    BeatSaberMarkupLanguage.MenuButtons.MenuButtons.instance.RegisterButton(new UI.ModButton("BSMLPractice", "BSMLPractice hint text.", OnClick));
                }
                catch (Exception ex)
                {
                    Logger.log?.Error(ex);
                }
                var exampleGameObject = new GameObject($"{Name}.ExampleMonobehaviour").AddComponent<ExampleMonobehaviour>();
            }
            if (nextScene.name == "GameCore")
            {

            }
        }

        internal static UI.ExampleFlowCoordinator ExampleView;
        internal static void OnClick()
        {
            if (ExampleView == null)
                ExampleView = new GameObject("BSMLPractice.ExampleFlowCoordinator").AddComponent<UI.ExampleFlowCoordinator>();
            MainFlowCoordinator main = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            foreach (var item in main.mainScreenViewControllers)
            {
                Logger.log?.Critical($"{item.name}: {item.navigationController?.name}");
            }
            if (main != null)
            {
                main.InvokePrivateMethod("PresentFlowCoordinator", new object[] { ExampleView, null, false, false });
            }
            else
                Logger.log?.Warn("MainFlowCoordinator not found");
        }

        /// <summary>
        /// This is called every frame.
        /// </summary>
        public void OnUpdate()
        {

        }

        /// <summary>
        /// Called when BSEvents.menuSceneLoadedFresh is triggered. UI creation is in here instead of
        /// OnSceneLoaded because some settings won't work otherwise.
        /// </summary>
        public void MenuLoadedFresh()
        {
            {
                configProvider.Store(config.Value); // Save settings.
                Logger.log.Debug("Creating plugin's UI");
                UI.BSMLPractice_SettingsUI.CreateUI();

            }
        }

        /// <summary>
        /// Runs at a fixed intervalue, generally used for physics calculations. 
        /// </summary>
        public void OnFixedUpdate()
        {

        }

        /// <summary>
        /// Called when the a scene's assets are loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneMode"></param>
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {



        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
