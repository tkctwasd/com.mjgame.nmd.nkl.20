using UnityEngine;

namespace MJGame
{
    public class IdleState : State<StateMachinePlayer>
    {
        public IdleState(StateMachine<StateMachinePlayer> stateMachine, StateMachinePlayer context)
            : base(stateMachine, context) { }

        public override void Enter()
        {
            Debug.Log("Entered Idle State");
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            Debug.Log("Exiting Idle State");
        }
    }
}
