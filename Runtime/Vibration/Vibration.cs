using UnityEngine;
using System.Collections;

public static class Vibration
{
      public static void Vibrate()
      {
            if (DataSetting.Current.Vibrate)
            {
#if !UNITY_STANDALONE
                  Handheld.Vibrate();
#endif
            }
      }
}
