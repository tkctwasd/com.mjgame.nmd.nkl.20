using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerIndicator : MonoBehaviour
{
    [SerializeField] IndicatorType indicatorType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player))
        {
            IndicatorManager.SetShowState(indicatorType, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player))
        {
            IndicatorManager.SetShowState(indicatorType, false);
        }
    }
}
