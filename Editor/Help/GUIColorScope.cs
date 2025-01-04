#if UNITY_EDITOR

using System;
using UnityEngine;

namespace MJGame
{

    public class BackgroundColorScope : IDisposable
    {
        readonly Color prevColor;
        public BackgroundColorScope()
        {
            prevColor = GUI.backgroundColor;
        }
        public BackgroundColorScope(Color newColor)
        {
            prevColor = GUI.backgroundColor;
            GUI.backgroundColor = newColor;
        }

        public void Dispose()
        {
            GUI.backgroundColor = prevColor;
        }
    }

    public class GUIColorScope : IDisposable
    {
        readonly Color prevColor;
        public GUIColorScope()
        {
            prevColor = GUI.backgroundColor;
        }
        public GUIColorScope(Color newColor)
        {
            prevColor = GUI.backgroundColor;
            GUI.backgroundColor = newColor;
        }

        public void Dispose()
        {
            GUI.backgroundColor = prevColor;
        }
    }
}
#endif