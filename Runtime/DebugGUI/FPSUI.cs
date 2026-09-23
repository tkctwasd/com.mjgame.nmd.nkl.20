using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSUI : MonoBehaviour
{
    [SerializeField] TMP_Text fpsText;
    FPSCounter fpsCounter;

    private void Awake()
    {
        fpsCounter = new FPSCounter();
    }

    public void Update()
    {
        fpsCounter.Update(Time.deltaTime);

#if UNITY_EDITOR 
        fpsText.text = "FPS: " + fpsCounter.GetAverageFps(1f).ToString("###");
        fpsText.color = Color.Lerp(Color.red, Color.green, fpsCounter.GetAverageFps(1f) / 60f);
#else
        fpsText.text =  "";
#endif
    }

}
