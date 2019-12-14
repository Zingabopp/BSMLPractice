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
using HMUI;

namespace BSMLPractice.UI
{
    public abstract class HotReloadableViewController : BSMLViewController
    {
        #region FileSystemWatcher
        internal class WatcherGroup
        {
            internal WatcherGroup(string directory)
            {
                ContentDirectory = directory;
                if (!Directory.Exists(ContentDirectory)) return;
                Watcher = new FileSystemWatcher(directory, "*.bsml");
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.Changed += Watcher_Changed;
            }

            private void Watcher_Changed(object sender, FileSystemEventArgs e)
            {
                foreach (var pair in BoundControllers.ToArray())
                {
                    if (!pair.Value.TryGetTarget(out var controller))
                    {
                        BoundControllers.Remove(pair.Key);
                        continue;
                    }
                    if (e.FullPath == Path.GetFullPath(controller.ResourceFilePath))
                    {
                        controller.MarkDirty();
                        HMMainThreadDispatcher.instance.Enqueue(HotReloadCoroutine());
                    }
                }
                if (BoundControllers.Count == 0)
                    Watcher.EnableRaisingEvents = false;
            }
            WaitForSeconds HotReloadDelay = new WaitForSeconds(.5f);
            internal bool IsReloading { get; private set; }
            private IEnumerator<WaitForSeconds> HotReloadCoroutine()
            {
                if (IsReloading) yield break;
                IsReloading = true;
                yield return HotReloadDelay;
                foreach (var pair in BoundControllers.ToArray())
                {
                    if (!pair.Value.TryGetTarget(out var controller))
                    {
                        UnbindController(controller);
                        continue;
                    }
                    if (controller.ContentChanged)
                        RefreshViewController(controller);
                }
                IsReloading = false;
            }
            internal FileSystemWatcher Watcher { get; private set; }
            internal string ContentDirectory { get; private set; }
            Dictionary<int, WeakReference<HotReloadableViewController>> BoundControllers = new Dictionary<int, WeakReference<HotReloadableViewController>>();
            internal bool BindController(HotReloadableViewController controller)
            {
                if (BoundControllers.ContainsKey(controller.GetInstanceID())) return false;
                BoundControllers.Add(controller.GetInstanceID(), new WeakReference<HotReloadableViewController>(controller));
                Watcher.EnableRaisingEvents = true;
                return true;
            }

            internal bool UnbindController(HotReloadableViewController controller)
            {
                bool remove = BoundControllers.Remove(controller.GetInstanceID());
                if (BoundControllers.Count == 0)
                    Watcher.EnableRaisingEvents = false;
                return remove;
            }
        }
        private static Dictionary<string, WatcherGroup> WatcherDictionary = new Dictionary<string, WatcherGroup>();
        public static bool RegisterViewController(HotReloadableViewController controller)
        {
            string contentFile = controller.ResourceFilePath;
            if (string.IsNullOrEmpty(contentFile)) return false;
            string contentDirectory = Path.GetDirectoryName(contentFile);
            if (!Directory.Exists(contentDirectory)) return false;
            WatcherGroup watcherGroup;
            if(!WatcherDictionary.TryGetValue(contentDirectory, out watcherGroup))
                watcherGroup = new WatcherGroup(contentDirectory);
            watcherGroup.BindController(controller);

            return true;
        }


        #endregion

        public static void RefreshViewController(HotReloadableViewController viewController, bool forceReload = false)
        {
            if (viewController.ContentChanged || forceReload)
            {
                try
                {
                    viewController.__Deactivate(ViewController.DeactivationType.NotRemovedFromHierarchy, false);
                    for (int i = 0; i < viewController.transform.childCount; i++)
                        GameObject.Destroy(viewController.transform.GetChild(i).gameObject);
                    viewController.__Activate(ViewController.ActivationType.NotAddedToHierarchy);
                }
                catch (Exception ex)
                {
                    Logger.log?.Error(ex);
                }
            }
        }

        public abstract string ResourceName { get; }
        public abstract string ResourceFilePath { get; }

        public virtual string FallbackContent { get; }

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
            if (HotReloadableViewController.RegisterViewController(this))
                Logger.log.Warn($"Registered {this.name}");
            else
                Logger.log.Error($"Failed to register {this.name}");
            base.DidActivate(firstActivation, type);
        }
        public void MarkDirty()
        {
            ContentChanged = true;
            _content = null;
        }
    }
}
