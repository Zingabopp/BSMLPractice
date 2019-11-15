using System;
using VRUI;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using UnityEngine;
using BSMLPractice.Views;

namespace BSMLPractice.UI
{
    class ExampleFlowCoordinator : FlowCoordinator
    {
        private ExampleView _contentViewController;
        private ExampleView _leftViewController;
        public ExampleView _rightViewController;
        public Func<ExampleView, string> OnContentCreated;

        private Vector3 MainScreenPosition;
        private GameObject MainScreen;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            MainScreen = GameObject.Find("MainScreen");
            MainScreenPosition = MainScreen.transform.position;

            if (firstActivation)
            {
                _contentViewController = BeatSaberUI.CreateViewController<ExampleView>();
                _leftViewController = BeatSaberUI.CreateViewController<ExampleView>();
                _rightViewController = BeatSaberUI.CreateViewController<ExampleView>();
                title = "BSMLPractice";
            }
            if (activationType == FlowCoordinator.ActivationType.AddedToHierarchy)
            {
                ProvideInitialViewControllers(_contentViewController, _leftViewController, _rightViewController);
                //MainScreen.transform.position = new Vector3(0, -100, 0); // "If it works it's not stupid" - Caeden117
                //_contentViewController.onBackPressed += backButton_DidFinish;
            }
        }

        private void backButton_DidFinish()
        {
            MainScreen.transform.position = MainScreenPosition;
            //_rightViewController.onBackPressed -= backButton_DidFinish;
        }

        protected override void DidDeactivate(DeactivationType type)
        {

        }
    }
}
