using UnityEngine;

/// <summary>
/// Attach this script to all the target game objects in the scene.
/// </summary>
public class IndicatorTarget : MonoBehaviour
{
    public IndicatorType indicatorType;
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

    public void SetIndicator(IndicatorType type, int distance, Vector2 offset = default)
    {
        indicatorType = type;
        DistanceDisplay = distance;
        Offset = offset;
    }
    public void SetIndicatorType(IndicatorType newType)
    {
        if (indicatorType == newType) return;

        IndicatorManager.Unregister(this);

        indicatorType = newType;

        if (indicatorType != IndicatorType.None)
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
public enum IndicatorType
{
    None,
    StealBrainrot,
    SellBrainrot,
    GetBrainrot,
    UpdateBrainrot,
    TrashBrainrot,
    TakeBrainrot,
    UnlockBrainrot,
    UnlockBase,
    BuyBase,
    UnlockLuckyBlock,
    OpenLuckyBlock,
    UnlockHouseEnemy,
    BuyHouseEnemy,
    UnlockTictacBlock,
    GetTictacBlock,
    OpenTictacBlock,
    UnlockTrail,
    OpenTrail,
    X2Cup,
    SkipCheckpoint,
    UnlockTreadmill,
    UnlockJetpack,
    UnlockUpgradeAnimalPen,
    BuyUpgradeAnimalPen,
    TakeEgg,
    UnlockUpgradeTreadmill,
    BuyUpgradeTreadmill, 
    UnlockAnimal,
    UnlockEgg,
    StealAnimal
}
