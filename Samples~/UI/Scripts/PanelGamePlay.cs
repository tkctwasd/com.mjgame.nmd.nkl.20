using System.Collections;
using System.Collections.Generic;
using MJGame;
using UnityEngine;
using UnityEngine.UI;

public class PanelGamePlay : BaseUI
{
    [Header("Button")]
    public Button BtnHome;
    public Button BtnSetting;

    [Header("Other")]
    private FloatingJoystick floatingJoystick;

    public override void Init()
    {
        base.Init();
        if (GetComponentInChildren<FloatingJoystick>() != null)
            floatingJoystick = GetComponentInChildren<FloatingJoystick>();

        BtnHome.onClick.AddListener(OnClickHome);
        BtnSetting.onClick.AddListener(OnClickBtnSetting);
    }

    public override void SetInfo()
    {
        base.SetInfo();
        UIStart();
    }

    public void UIStart()
    {
        ActiveJoystick(false);
    }

    #region Active UI
    public void ActiveJoystick(bool active)
    {
        GameObjectExtension.ActiveObj(floatingJoystick?.gameObject, active);
    }
    #endregion


    #region Button
    private void OnClickBtnSetting()
    {
        TimeExtension.PauseGame();
        Debug.Log("OnClickBtnSetting");
    }

    private void OnClickHome()
    {
        Debug.Log("OnClickHome");
    }
    #endregion
}
