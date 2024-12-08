using System;
using UnityEngine;

namespace MJGame
{

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