using UnityEngine;
using UnityEngine.UI;
public class UIScroll : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, 0.1f);

    Vector2 uv;

    void Update()
    {
        uv += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = new Rect(uv, Vector2.one);
    }
}

