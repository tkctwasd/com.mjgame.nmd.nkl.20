using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerIndicator : MonoBehaviour
{
    [SerializeField, IndicatorType, FormerlySerializedAs("indicatorType")] int type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player))
        {
            IndicatorManager.SetShowState(type, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player))
        {
            IndicatorManager.SetShowState(type, false);
        }
    }
}
