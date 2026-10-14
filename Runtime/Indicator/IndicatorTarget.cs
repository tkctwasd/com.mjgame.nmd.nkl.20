using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Attach this script to all the target game objects in the scene.
/// </summary>
public class IndicatorTarget : MonoBehaviour
{
    [SerializeField, IndicatorType, FormerlySerializedAs("indicatorType")] int type;
    /// <summary>Id loai indicator (enum cua game ep sang int). 0 = khong co.</summary>
    public int Type => type;
    public int DistanceDisplay = 3;
    public Vector2 Offset;
    /// <summary>
    /// Please do not assign its value yourself without understanding its use.
    /// A reference to the target's indicator, 
    /// its value is assigned at runtime by the offscreen indicator script.
    /// </summary>
    public Indicator Indicator;

    /// <summary>
    /// On enable add this target object to the targets list.
    /// </summary>
    private void OnEnable()
    {
        IndicatorManager.Register(this);
    }

    /// <summary>
    /// On disable remove this target object from the targets list.
    /// </summary>
    private void OnDisable()
    {
        IndicatorManager.Unregister(this);
    }

    public void SetIndicator(int newType, int distance, Vector2 offset = default)
    {
        type = newType;
        DistanceDisplay = distance;
        Offset = offset;
    }
    public void SetIndicatorType(int newType)
    {
        if (type == newType) return;

        IndicatorManager.Unregister(this);

        type = newType;

        if (type != 0)
            IndicatorManager.Register(this);
    }

    /// <summary>
    /// Gets the distance between the camera and the target.
    /// </summary>
    /// <param name="cameraPosition">Camera position</param>
    /// <returns></returns>
    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }
}
