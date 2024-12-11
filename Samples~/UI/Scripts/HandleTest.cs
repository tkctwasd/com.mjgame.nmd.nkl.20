using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MJGame;
public class HandleTest : TickBehaviour
{
    private void Start()
    {
        UIManager.ShowPanel<PanelProfile>();
    }
    [Button]
    public void OpenPanelLose()
    {
        UIManager.HideAllPanel();
        UIManager.ShowPanel<PanelLose>();
    }
    [Button]
    public void OpenPanelGamePlay()
    {
        UIManager.ShowPanel<PanelGamePlay>();
    }
    [Button]
    public void OpenPanelProfile()
    {
        UIManager.ShowPanel<PanelProfile>();
    }
    [Button]
    public void HidePanelGamePlay()
    {
        UIManager.HidePanel<PanelGamePlay>() ;
    }
    [Button]
    public void HidePanelProfile()
    {
        UIManager.HidePanel<PanelProfile>();
    }
    [Button]
    public void HidePanelLose()
    {
        UIManager.HidePanel<PanelLose>();
    }


}