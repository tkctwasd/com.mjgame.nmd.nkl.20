namespace MJGame
{
    public class StateMachine<T>
    {
        private State<T> currentState;
        private T context;
        private StateTransitionManager<T> transitionManager;

        public StateMachine(T context)
        {
            this.context = context;
            this.transitionManager = new StateTransitionManager<T>();
        }

        public void SetState(State<T> newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void ExecuteState()
        {
            currentState?.Execute();
        }

        /// <summary>
        /// Đăng ký chuyển tiếp trạng thái qua trigger
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        /// <param name="triggerName"></param>
        public void RegisterTransition(State<T> fromState, State<T> toState, string triggerName)
        {
            transitionManager.RegisterTransition(fromState, toState, triggerName);
        }

        /// <summary>
        /// Gọi trigger để chuyển tiếp trạng thái
        /// </summary>
        /// <param name="triggerName"></param>
        public void Trigger(string triggerName)
        {
            transitionManager.Trigger(triggerName, currentState, this);
        }

        public T GetContext() => context;
    }

}