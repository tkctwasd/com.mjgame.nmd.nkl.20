using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{

    [CreateAssetMenu(fileName = "PanelConfig", menuName = "Data/PanelConfigSO", order = 0)]
    public class PanelConfigSO : ScriptableObject
    {
        public List<BaseUI> Panels;
    }
}