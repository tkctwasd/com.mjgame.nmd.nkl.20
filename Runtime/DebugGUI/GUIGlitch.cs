using UnityEngine;
using UnityEngine.UI;

public class GUIGlitch : MonoBehaviour
{
    public Image img;
    public float moveAmount = 20f;  
    public float flickerSpeed = 0.05f; 
    public float alphaMin = 0.2f;    
    public float alphaMax = 0.5f;     
    public float scaleAmount = 0.03f; 

    Vector3 startPos;
    Vector3 startScale;

    void OnEnable()
    {
        startPos = img.rectTransform.anchoredPosition;
        startScale = img.rectTransform.localScale;
        StartCoroutine(GlitchLoop());
    }

    System.Collections.IEnumerator GlitchLoop()
    {
        while (true)
        {
            float offsetX = Random.Range(-moveAmount, moveAmount);
            float offsetY = Random.Range(-moveAmount, moveAmount);
            img.rectTransform.anchoredPosition = startPos + new Vector3(offsetX, offsetY, 0);

            float alpha = Random.Range(alphaMin, alphaMax);
            var c = img.color;
            c.a = alpha;
            img.color = c;

            float scale = 1f + Random.Range(-scaleAmount, scaleAmount);
            img.rectTransform.localScale = startScale * scale;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
