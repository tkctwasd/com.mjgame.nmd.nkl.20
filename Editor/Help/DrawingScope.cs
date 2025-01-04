#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
namespace MJGame {

public struct DrawingScope : IDisposable
{
    private UnityEngine.Color m_DefaultColor;

    public DrawingScope(BaseColor color, float alpha=1)
    {
        m_DefaultColor = Handles.color;        
        Handles.color = color.ToUnityColor(alpha);
    }
    public Color Color=>Handles.color;
    public void Dispose()
    {
        Handles.color = m_DefaultColor;
    }
}
}
#endif