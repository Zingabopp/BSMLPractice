using System;
using VRUI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using UnityEngine;
using BSMLPractice.Views;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;

namespace BSMLPractice.UI
{
    internal class ExampleFlowCoordinator : FlowCoordinator
    {
        private ExampleViewCenter _centerViewController;
        private ExampleViewLeft _leftViewController;
        private ExampleViewRight _rightViewController;
        protected VRUINavigationController leftNavigationController;
        public Func<ExampleViewCenter, string> OnContentCreated;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    leftNavigationController = BeatSaberUI.CreateViewController<VRUINavigationController>();
                    Logger.log?.Warn("First activation");
                    _centerViewController = BeatSaberUI.CreateViewController<ExampleViewCenter>();
                    Logger.log?.Warn("Center created");
                    _leftViewController = BeatSaberUI.CreateViewController<ExampleViewLeft>();
                    SetViewControllerToNavigationConctroller(leftNavigationController, _leftViewController);
                    Logger.log?.Warn("Left created");
                    _rightViewController = BeatSaberUI.CreateViewController<ExampleViewRight>();
                    Logger.log?.Warn("Right created");
                    base.title = "BSMLPractice";
                }
                if (activationType == FlowCoordinator.ActivationType.AddedToHierarchy)
                {
                    Logger.log?.Warn("AddedToHierarchy");
                    ProvideInitialViewControllers(_centerViewController, _leftViewController, _rightViewController);
                    _leftViewController.OnBackPressed -= BackButton_Pressed;
                    _leftViewController.OnBackPressed += BackButton_Pressed;
                    _centerViewController.OnReloadPressed -= OnTestPressed;
                    _centerViewController.OnReloadPressed += OnTestPressed;
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
                watcher.Path = Path.GetDirectoryName(_leftViewController._altResourcePath);
                watcher.Filter = "*Left.bsml";
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += Watcher_Changed;
                watcher.EnableRaisingEvents = true;
                while (this.isActivated)
                {
                    if (fileChanged && !isReloading)
                    {
                        OnTestPressed();
                    }
                    yield return waitTime;
                }
            }

        }
        private bool isReloading = false;
        private bool fileChanged = false;
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            fileChanged = true;
        }

        protected void OnTestPressed()
        {
            isReloading = true;
            try
            {
                _leftViewController.RefreshContent = true;
                _leftViewController.__Deactivate(VRUIViewController.DeactivationType.NotRemovedFromHierarchy, false);
                for (int i = 0; i < _leftViewController.transform.childCount; i++)
                    GameObject.Destroy(_leftViewController.transform.GetChild(i).gameObject);
                fileChanged = false;
                _leftViewController.__Activate(VRUIViewController.ActivationType.NotAddedToHierarchy);
            }
            catch (Exception ex)
            {
                Logger.log?.Error(ex);
            }
            isReloading = false;
            if (fileChanged)
                OnTestPressed();
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
