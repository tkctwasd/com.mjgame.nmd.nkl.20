using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : ItemsAction
{
    public Door door;

    private void Awake()
    {
        door.Init(this.transform);
    }

    public override void Action(object obj)
    {
        base.Action(obj);
        door.ToggleDoor(obj != null ? (DirectionDoor)obj : DirectionDoor.A);
    }
}
