using UnityEngine;

namespace MJGame
{
    public abstract class StateMachineContext<T> : TickBehaviour where T : StateMachineContext<T>
    {
        protected StateMachine<T> stateMachine;

        private void Start()
        {
            stateMachine = new StateMachine<T>((T)this);
            RegisterTransition();
        }

        protected virtual void RegisterTransition() { }

        public override void OnUpdate()
        {
            base.OnUpdate();
            stateMachine.ExecuteState();
        }

        public void Trigger(string triggerName)
        {
            stateMachine.Trigger(triggerName);
        }
    }
}
