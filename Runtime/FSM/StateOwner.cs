namespace FSM
{
    public class State<TOwner> : State where TOwner : FSMBehaviour
    {
        protected TOwner Owner { get; }

        public State(IStateMachine stateMachine) : base(stateMachine)
        {
            Owner = stateMachine.Owner as TOwner;
        }
    }
}
