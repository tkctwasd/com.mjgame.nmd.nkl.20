using UnityEngine;

namespace MJGame
{
    public class DeadState : State<StateMachinePlayer>
    {
        public DeadState(StateMachine<StateMachinePlayer> stateMachine, StateMachinePlayer context)
            : base(stateMachine, context) { }

        public override void Enter()
        {
            context.Die();
            Debug.Log("Entered Dead State");
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            Debug.Log("Exiting Dead State");
        }
    }
}
