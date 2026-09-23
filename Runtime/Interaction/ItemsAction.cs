using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsAction : MonoBehaviour, IItemAction
{
    public virtual void Action(object obj= null) { }
    public virtual void Cancel() { }
}
