using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MJGame;
public class PanelLose : BaseUI
{

    [Header("Button")]
    public Button BtnRevice;
    public Button BtnRestart;
    public Button BtnHome;

    public override void Init()
    {
        base.Init();
        BtnRevice.onClick.AddListener(OnClickBtnRevice);
        BtnRestart.onClick.AddListener(OnClickBtnRestart);
        BtnHome.onClick.AddListener(OnClickBtnHome);
    }



    #region Button
    private void OnClickBtnHome()
    {
        Debug.Log("OnClickBtnHome");
    }

    private void OnClickBtnRestart()
    {
        Debug.Log("OnClickBtnRestart");
    }

    private void OnClickBtnRevice()
    {
        Debug.Log("OnClickBtnRevice");

    }
    #endregion

}