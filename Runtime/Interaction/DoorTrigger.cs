using UnityEngine;

public class DoorTrigger : MonoBehaviour, IItemTrigger
{
    [SerializeField] private Door door;
    [SerializeField] private bool closeOnExit = true;
    private void Awake()
    {
        door.Init(this.transform);
    }
    [Button]
    public virtual void InteractPlayer()
    {
        door.ToggleDoor();
    }
    [Button]
    public virtual void CancelPlayer()
    {
        if (closeOnExit)
            door.CloseDoor();
    }
}
