#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace MJGame{
    
public class CreateUIBase
{
    public static Canvas CreateCanvasUI(Vector2 resolution)
    {
        GameObject canvasObject = new("CanvasUI");

        EventSystem eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject newEventSystem = new("EventSystem");
            newEventSystem.AddComponent<EventSystem>();
            newEventSystem.AddComponent<StandaloneInputModule>();
        }

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = resolution;

        canvasObject.AddComponent<GraphicRaycaster>();

        return canvas;
    }



    public static RectTransform CreatePanelUI(Vector2 resolution)
    {
        Canvas canvasObject = GameObject.FindAnyObjectByType<Canvas>();

        if (canvasObject == null)
        {
            canvasObject = CreateCanvasUI(resolution);
        }

        GameObject panelObject = new("Panel");
        panelObject.transform.parent = canvasObject.transform;
        RectTransform panelTransform = panelObject.AddComponent<RectTransform>();
        panelObject.AddComponent<CanvasRenderer>();

        panelTransform.anchorMin = new Vector2(0, 0);
        panelTransform.anchorMax = new Vector2(1, 1);
        panelTransform.offsetMin = new Vector2(0, 0);
        panelTransform.offsetMax = new Vector2(0, 0);

        panelTransform.localScale = Vector3.one;

        Image panelImage = panelObject.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.6f);


        return panelTransform;
    }


    public static void CreatePanelGamePlay(Vector2 resolution)
    {
        RectTransform panel = CreatePanelUI(resolution);
        panel.name = "PanelGamePlay";

        // tao Button Home
        CreatButtonUI("BtnHome", Vector2.one * 100, panel, new Vector2(0, 1));
        // tao Button Setting 
        CreatButtonUI("BtnSetting", Vector2.one * 100, panel, new Vector2(1, 1));
    }

    public static void CreatePanelLose(Vector2 resolution)
    {
        RectTransform panel = CreatePanelUI(resolution);
        panel.name = "PanelLose";

        // tao Button Home
        CreatButtonUI("BtnHome", Vector2.one * 100, panel, new Vector2(0, 1));

        // tao Image 
        RectTransform imgButton = CreateImage(panel, new Vector2(500, 500), new Vector2(0.5f, 0f), Color.white).GetComponent<RectTransform>();
        imgButton.name = "ButtonContainer";
        imgButton.gameObject.AddComponent<GridLayoutGroup>();
        imgButton.anchoredPosition = new Vector3(0, 300, 0);
        GridLayoutGroup gridLayoutGroup = imgButton.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(350, 100);
        gridLayoutGroup.spacing = new Vector2(100, 0);
        gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 1;
        imgButton.gameObject.AddComponent<ContentSizeFitter>();
        imgButton.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        imgButton.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        imgButton.GetComponent<Image>().color = new Color(0, 0, 0, 0f);

        // tao Button Revice
        CreatButtonUI("BtnRevice", new Vector2(350, 100), imgButton, new Vector2(0.5f, 0.5f));
        // // tao Button Restart 
        CreatButtonUI("BtnRestart", new Vector2(350, 100), imgButton, new Vector2(0.5f, 0.5f));
    }



    private static void CreatButtonUI(string name, Vector2 size, RectTransform parent, Vector2 anchor)
    {
        GameObject btn = CreateImage(parent, size, anchor, Color.white);
        btn.name = name;
        btn.AddComponent<Button>();

        GameObject icon = CreateImage(btn.GetComponent<RectTransform>(), size - Vector2.one * 10, new Vector2(0.5f, 0.5f), Color.red);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="size"></param>
    /// <param name="anchor">new Vector2(0,1) can trai tren</param>
    /// <param name="anchor">new Vector2(0,0) can trai duoi</param>
    /// <param name="anchor">new Vector2(1,0) can phai duoi</param>
    /// <param name="anchor">new Vector2(1,1) can phai tren</param>
    /// <param name="anchor">new Vector2(0.5f,0.5f) can giua</param>
    /// <returns></returns>
    private static GameObject CreateImage(RectTransform parent, Vector2 size, Vector2 anchor, Color color = default)
    {
        GameObject img = new GameObject("Icon");

        img.AddComponent<Image>();
        img.GetComponent<Image>().color = color;
        // img.AddComponent<CanvasRenderer>();

        RectTransform rect = img.GetComponent<RectTransform>();

        rect.SetParent(parent);

        rect.anchorMin = new Vector2(anchor.x, anchor.y);
        rect.anchorMax = new Vector2(anchor.x, anchor.y);
        rect.pivot = new Vector2(anchor.x, anchor.y);

        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        return img;
    }




}
}

#endif