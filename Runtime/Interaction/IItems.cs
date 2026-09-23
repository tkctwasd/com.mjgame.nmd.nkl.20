public interface IItems
{
}

public interface IItemInteract
{
      public abstract void Interact();
}

public interface IItemTrigger
{
      public virtual void CancelPlayer() { }
      public virtual void InteractPlayer() { }
}

public interface IItemAction
{
      public virtual void Action() { }
      public virtual void Cancel() { }
}
