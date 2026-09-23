using UnityEngine;

public class DoorInteract : ItemsInteract
{
      [SerializeField] private Door door;
      private void Awake()
      {
            door.Init(this.transform);
      }

      [Button]
      public override void Interact()
      {
            base.Interact();
            door.ToggleDoor();
      }
}
