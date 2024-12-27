using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
namespace MJGame
{

    public class UIManager
    {
        static GameObject CanvasUI { get; set; }
        static PanelConfigSO Config { get; set; }
        static Dictionary<string, BaseUI> Panels;
        public static BaseUI PrevUI { get; private set; }
        public static Canvas Canvas { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnInit()
        {
            InitUI();
        }

        static void InitUI()
        {
            Config = Resources.Load<PanelConfigSO>("PanelConfig");
            SpawnRoot();
            InitPanels();
            Object.DontDestroyOnLoad(CanvasUI);
        }
        public static Canvas CreateCanvas(string name, int sortingOrder)
        {
            return SpawnCanvas(name, sortingOrder);
        }

        static void InitPanels()
        {
            if (Panels == null)
            {
                Panels = new Dictionary<string, BaseUI>();
            }
            if (Config == null) return;
            foreach (var _panel in Config.Panels)
            {
                var panel = SpawnUI(_panel, CanvasUI);
                Panels.Add(_panel.GetType().Name, panel);
            }
        }
        /// <summary>
        /// Spawns the root UI GameObject and sets up its components.
        /// </summary>
        static void SpawnRoot()
        {
            if (CanvasUI != null) return;
            Canvas = SpawnCanvas("CanvasUI", 50);
            CanvasUI = Canvas.gameObject;
            CanvasUI.AddComponent<GraphicRaycaster>();
            Object.DontDestroyOnLoad(CanvasUI);
        }

        public static void AddPanels(List<BaseUI> panels, GameObject root)
        {
            if (Panels == null)
            {
                Panels = new Dictionary<string, BaseUI>();
            }
            foreach (var _panel in panels)
            {
                var panel = SpawnUI(_panel, root, false);
                Panels.Add(_panel.GetType().Name, panel);
            }
        }

        static Canvas SpawnCanvas(string name, int sortingOrder)
        {
            CanvasUI = new GameObject(name);
            var canvas = CanvasUI.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;
            var canvasScaler = CanvasUI.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            if (Application.isMobilePlatform)
            {
                if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
                {
                    canvasScaler.referenceResolution = new Vector2(1920, 1080);
                }
                else
                {
                    canvasScaler.referenceResolution = new Vector2(1080, 1920);
                }
            }
            else
            {
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
                if (screenWidth > screenHeight)
                {
                    canvasScaler.referenceResolution = new Vector2(1920, 1080);
                }
                else
                {
                    canvasScaler.referenceResolution = new Vector2(1080, 1920);
                }
            }

            canvasScaler.matchWidthOrHeight = 0.5f;
            return canvas;
        }
        public static void HideAllPanel()
        {
            if (Panels == null)
            {
                Panels = new Dictionary<string, BaseUI>();
            }
            foreach (var panel in Panels)
            {
                PrevUI = null;
                // panel.Value.gameObject.SetActive(false);
                panel.Value.Hide();
            }
        }
        /// <summary>
        /// Hides the specified panel.
        /// </summary>
        /// <typeparam name="T">The type of the panel to hide. This type must derive from BaseUI.</typeparam>
        public static void HidePanel<T>() where T : BaseUI
        {
            string name = (typeof(T).Name);
            if (Panels == null)
            {
                Panels = new Dictionary<string, BaseUI>();
            }
            if (Panels.TryGetValue(name, out var panel))
            {
                PrevUI = panel;
                // panel.gameObject.SetActive(false);
                panel.Hide();
                if (panel.transform.parent == null)
                {
                    panel.transform.SetParent(CanvasUI.transform, false);
                }
            }
        }

        public static T ShowPanel<T>(float duration = 1) where T : BaseUI
        {
            if (CanvasUI == null)
            {
                SpawnRoot();
            }
            string name = (typeof(T).Name);
            Debug.Log(name);

            if (Panels == null)
            {
                Panels = new Dictionary<string, BaseUI>();
            }
            if (Panels.TryGetValue(name, out var panel))
            {
                panel.gameObject.SetActive(true);
            }
            else
            {
                panel = SpawnUI<T>();
                Panels.Add(name, panel);
                panel.gameObject.SetActive(true);
            }
            if (panel.transform.parent == null)
            {
                panel.transform.SetParent(CanvasUI.transform, false);
            }
            panel.SetInfo();
            panel.Show(duration);
            panel.transform.SetAsLastSibling();
            return panel as T;
        }

        public static T Panel<T>() where T : BaseUI
        {
            string name = typeof(T).Name;
            foreach (var p in Panels)
            {
                if (p.Key == name) return p.Value as T;
            }
            return null;
        }

        protected static BaseUI SpawnUI(BaseUI _panel, GameObject _root, bool _createNew = true)
        {
            if (_root == null) _root = CanvasUI;

            var panel = _panel;
            if (_createNew)
                panel = Object.Instantiate(_panel);
            panel.transform.SetParent(_root.transform, false);
            panel.gameObject.SetActive(false);
            panel.name = _panel.name;
            return panel;
        }
        protected static T SpawnUI<T>(bool isActivate = false) where T : BaseUI
        {
            string name = (typeof(T).Name);
            //T rs = Resources.Load<T>($"UI/{name}");
            T rs = GetPanel<T>();
            Debug.Assert(rs != null);
            var panel = Object.Instantiate<T>(rs);

            panel.transform.SetParent(CanvasUI.transform, false);

            panel.gameObject.SetActive(isActivate);
            panel.name = name;
            panel.hideFlags = HideFlags.None;
            //panel.Init();
            return panel;
        }
        static T GetPanel<T>() where T : BaseUI
        {
            foreach (var p in Config.Panels)
            {
                if (p is T) return p as T;
            }
            return default;
        }
        
        /// <summary>
        /// panel opacity
        /// </summary>
        /// <param name="opacity"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetPanelOpacity<T>(float opacity) where T : BaseUI
        {
            string name = typeof(T).Name;

            if (Panels != null && Panels.TryGetValue(name, out var panel))
            {
                var canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = Mathf.Clamp01(opacity);
            }
            else
            {
                Debug.LogWarning($"Panel {name} not found or not registered in UIManager.");
            }
        }

        /// <summary>
        /// all panels opacity
        /// </summary>
        /// <param name="opacity"></param>
        public static void SetAllPanelsOpacity(float opacity)
        {
            if (Panels == null || Panels.Count == 0)
            {
                Debug.LogWarning("No panels are registered in UIManager.");
                return;
            }

            foreach (var panel in Panels.Values)
            {
                var canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = Mathf.Clamp01(opacity);
            }
        }

    }

}