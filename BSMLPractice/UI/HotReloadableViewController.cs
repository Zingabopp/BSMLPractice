using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRUI;

namespace BSMLPractice.UI
{
    public abstract class HotReloadableViewController : BSMLViewController
    {
        public static void RefreshViewController(HotReloadableViewController viewController, bool forceReload = false)
        {
            if (viewController.ContentChanged || forceReload)
            {
                try
                {
                    viewController.__Deactivate(VRUIViewController.DeactivationType.NotRemovedFromHierarchy, false);
                    for (int i = 0; i < viewController.transform.childCount; i++)
                        GameObject.Destroy(viewController.transform.GetChild(i).gameObject);
                    viewController.__Activate(VRUIViewController.ActivationType.NotAddedToHierarchy);
                }
                catch (Exception ex)
                {
                    Logger.log?.Error(ex);
                }
            }
        }

        public abstract string ResourceName { get; }
        public abstract string ResourceFilePath { get; }

        private string _content;
        public override string Content
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                {
                    try
                    {
                        _content = File.ReadAllText(ResourceFilePath);
                    }
                    catch
                    {
                        Logger.log?.Warn($"Unable to read file {ResourceFilePath} for {name}");
                    }
                }
                else if (!string.IsNullOrEmpty(ResourceName))
                    _content = Utilities.GetResourceContent(Assembly.GetAssembly(this.GetType()), ResourceName);
                return _content;
            }
        }

        public bool ContentChanged { get; protected set; }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            if (ContentChanged && !firstActivation)
            {
                ContentChanged = false;
                BSMLParser.instance.Parse(Content, gameObject, this);
            }
            base.DidActivate(firstActivation, type);
        }
        public void MarkDirty()
        {
            ContentChanged = true;
            _content = null;
        }
    }
}
