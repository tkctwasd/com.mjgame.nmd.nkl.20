using System.Collections;
using System.Collections.Generic;
using MJGame;
using UnityEngine;

public static class ColorUtility
{
    #region Base Color
    public static UnityEngine.Color ToUnityColor(this BaseColor color, float alpha = 1)
    {
        switch (color)
        {
            case BaseColor.Black: return UnityEngine.Color.black.SetAlpha(alpha);
            case BaseColor.Blue: return UnityEngine.Color.blue.SetAlpha(alpha);
            case BaseColor.Green: return UnityEngine.Color.green.SetAlpha(alpha);
            case BaseColor.Red: return UnityEngine.Color.red.SetAlpha(alpha);
            case BaseColor.White: return UnityEngine.Color.white.SetAlpha(alpha);
            case BaseColor.Yellow: return UnityEngine.Color.yellow.SetAlpha(alpha);
        }
        return UnityEngine.Color.white.SetAlpha(alpha);
    }
    public static UnityEngine.Color SetAlpha(this UnityEngine.Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    #endregion
}
