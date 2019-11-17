using System;
using VRUI;
using BeatSaberMarkupLanguage;
using UnityEngine;
using BSMLPractice.Views;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace BSMLPractice.UI
{
    internal class ExampleFlowCoordinator : FlowCoordinator
    {
        private ExampleViewCenter _centerViewController;
        private ExampleViewLeft _leftViewController;
        private ExampleViewRight _rightViewController;
        //protected VRUINavigationController leftNavigationController;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    //leftNavigationController = BeatSaberUI.CreateViewController<VRUINavigationController>();
                    Logger.log?.Warn("First activation");
                    _centerViewController = BeatSaberUI.CreateViewController<ExampleViewCenter>();
                    Logger.log?.Warn("Center created");
                    _leftViewController = BeatSaberUI.CreateViewController<ExampleViewLeft>();
                    //SetViewControllerToNavigationConctroller(leftNavigationController, _leftViewController);
                    Logger.log?.Warn("Left created");
                    _rightViewController = BeatSaberUI.CreateViewController<ExampleViewRight>();
                    Logger.log?.Warn("Right created");
                    base.title = "BSMLPractice";
                }
                if (activationType == ActivationType.AddedToHierarchy)
                {
                    Logger.log?.Warn("AddedToHierarchy");
                    ProvideInitialViewControllers(_centerViewController, _leftViewController, _rightViewController);
                    _leftViewController.OnBackPressed -= BackButton_Pressed;
                    _leftViewController.OnBackPressed += BackButton_Pressed;
                }
                else
                {
                    Logger.log?.Warn("NotAddedToHierarchy");
                }
                StartCoroutine(HotReloadCoroutine());
            }
            catch (Exception ex)
            {
                Logger.log?.Error(ex);
            }
        }
        private IEnumerator<WaitForSeconds> HotReloadCoroutine()
        {
            var waitTime = new WaitForSeconds(.5f);
            using (var watcher = new FileSystemWatcher())
            {
                watcher.Path = Path.GetDirectoryName(_leftViewController.ResourceFilePath);
                watcher.Filter = "*.bsml";
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += Watcher_Changed;
                watcher.EnableRaisingEvents = true;
                while (isActivated)
                {
                    if (_leftViewController.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_leftViewController);
                    if (_rightViewController.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_rightViewController);
                    if (_centerViewController.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_centerViewController);
                    yield return waitTime;
                }
            }

        }


        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == _leftViewController.ResourceFilePath)
                _leftViewController.MarkDirty();
            if (e.FullPath == _rightViewController.ResourceFilePath)
                _rightViewController.MarkDirty();
            if (e.FullPath == _centerViewController.ResourceFilePath)
                _centerViewController.MarkDirty();
        }

        protected override void DidDeactivate(DeactivationType type)
        {
            StopCoroutine(HotReloadCoroutine());
        }

        #region From BSIPA-ModList
        private delegate void PresentFlowCoordDel(FlowCoordinator self, FlowCoordinator newF, Action finished, bool immediate, bool replaceTop);
        private static PresentFlowCoordDel presentFlow;

        public void Present(Action finished = null, bool immediate = false, bool replaceTop = false)
        {
            if (presentFlow == null)
            {
                var ty = typeof(FlowCoordinator);
                var m = ty.GetMethod("PresentFlowCoordinator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                presentFlow = (PresentFlowCoordDel)Delegate.CreateDelegate(typeof(PresentFlowCoordDel), m);
            }

            MainFlowCoordinator mainFlow = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            presentFlow(mainFlow, this, finished, immediate, replaceTop);
        }

        private delegate void DismissFlowDel(FlowCoordinator self, FlowCoordinator newF, Action finished, bool immediate);
        private static DismissFlowDel dismissFlow;

        private void BackButton_Pressed()
        {
            if (dismissFlow == null)
            {
                var ty = typeof(FlowCoordinator);
                var m = ty.GetMethod("DismissFlowCoordinator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                dismissFlow = (DismissFlowDel)Delegate.CreateDelegate(typeof(DismissFlowDel), m);
            }
            MainFlowCoordinator mainFlow = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            dismissFlow(mainFlow, this, null, false);
        }
        #endregion
    }
}
