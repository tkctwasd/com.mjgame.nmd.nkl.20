using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;

public class InputButton : MonoBehaviour
{
    [SerializeField] Button btnBooster;
    public GameObject hand;
    public GameObject notice;

    protected virtual void Awake()
    {
        btnBooster.onClick.AddListener(OnClickInputBtn);
        btnBooster.AddResizeEffect();
    }

    protected virtual void OnEnable()
    {
        ActiveHand(false);
        ActiveNotice(false);
    }

    public void ActiveHand(bool isActive)
    {
        GameObjectExtensions.ActiveObj(hand, isActive);
    }

    public void ActiveNotice(bool isActive)
    {
        GameObjectExtensions.ActiveObj(notice, isActive);
    }

    public virtual void OnClickInputBtn()
    {
        AudioManager.Instance.AudioClickBtn();
        ActiveHand(false);
    }

    public virtual void Complete()
    {
        btnBooster.interactable = true;
    }
}
