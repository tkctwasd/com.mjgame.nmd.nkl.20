#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace MJGame
{

    public class BaseUIHieraechy
    {
        [MenuItem("GameObject/MJGame/UI/BaseUI/1080x1920", false, 10)]
        static void CreateBaseUI_1080x1920()
        {
            CreateUIBase.CreatePanelUI(new Vector2(1080, 1920));
        }

        [MenuItem("GameObject/MJGame/UI/BaseUI/1920x1080", false, 10)]
        static void CreateBaseUI_1920x1080()
        {
            CreateUIBase.CreatePanelUI(new Vector2(1920, 1080));
        }


        [MenuItem("GameObject/MJGame/UI/BaseUI/PanelGamePlay", false, 10)]
        static void CreatePanelGamePlay()
        {
            CreateUIBase.CreatePanelGamePlay(new Vector2(1920, 1080));
        }

        [MenuItem("GameObject/MJGame/UI/BaseUI/PanelLose", false, 10)]
        static void CreatePanelLose()
        {
            CreateUIBase.CreatePanelLose(new Vector2(1920, 1080));
        }
    }

}
#endif