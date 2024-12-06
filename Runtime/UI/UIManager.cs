using UnityEngine;
using System.Collections.Generic;

public class UIManager : TickBehaviour
{
    private Dictionary<System.Type, BaseUI> panels = new Dictionary<System.Type, BaseUI>();
    public enum TypeLoadPanel
    {
        ScriptableObject,
        Manual,
    }
    public TypeLoadPanel Type;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (Type == TypeLoadPanel.ScriptableObject)
            LoadPanelsFromConfig();
        else
        {
            var allPanels = GetComponentsInChildren<BaseUI>(true);
            foreach (var panel in allPanels)
            {
                panels[panel.GetType()] = panel;
            }
        }

        HideAllPanels();

    }

    private void LoadPanelsFromConfig()
    {
        PanelConfigSO panel = Resources.Load<PanelConfigSO>("PanelConfig");
        if (panel == null)
        {
            Debug.Log("UIManagerConfig is not assigned!");
            return;
        }

        foreach (var panelPrefab in panel.panel)
        {
            if (panelPrefab == null)
            {
                Debug.LogWarning("A null panel prefab was found in UIManagerConfig!");
                continue;
            }

            BaseUI panelInstance = Instantiate(panelPrefab, transform);
            panelInstance.gameObject.SetActive(false);
            panelInstance.name = panelPrefab.GetType().Name;
            panels[panelPrefab.GetType()] = panelInstance;
            Debug.Log("Panel loaded from ScriptableObject config: " + panelPrefab.GetType().Name);
        }
        Debug.Log("All panels loaded from ScriptableObject config.");
    }

    public T Panel<T>() where T : BaseUI
    {
        System.Type panelType = typeof(T);
        if (panels.ContainsKey(panelType))
        {
            return (T)panels[panelType];
        }
        else
        {
            Debug.LogError("Panel not found: " + panelType.Name);
            return default;
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels.Values)
        {
            panel.OnHide();
        }
    }
}
