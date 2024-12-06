using UnityEngine;

namespace MJGame
{
    public class RunState : State<StateMachinePlayer>
    {
        public RunState(StateMachine<StateMachinePlayer> stateMachine, StateMachinePlayer context)
            : base(stateMachine, context) { }

        public override void Enter()
        {
            Debug.Log("Entered Run State");
        }

        public override void Execute()
        {
            context.Move();
            base.Execute();
        }

        public override void Exit()
        {
            Debug.Log("Exiting Run State");
        }
    }
}
