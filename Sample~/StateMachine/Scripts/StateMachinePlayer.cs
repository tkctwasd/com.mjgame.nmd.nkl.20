using UnityEngine;

namespace MJGame
{
    public class StateMachinePlayer : StateMachineContext<StateMachinePlayer>
    {
        protected override void RegisterTransition()
        {
            base.RegisterTransition();

            // Khởi tạo các trạng thái
            var idleState = new IdleState(stateMachine, this);
            var runState = new RunState(stateMachine, this);
            var deadState = new DeadState(stateMachine, this);

            // Đăng ký các chuyển tiếp với trigger name
            stateMachine.RegisterTransition(idleState, runState, "run");
            stateMachine.RegisterTransition(runState, idleState, "idle");
            stateMachine.RegisterTransition(idleState, deadState, "dead");
            stateMachine.RegisterTransition(runState, deadState, "dead");

            // Bắt đầu với Idle state
            stateMachine.SetState(idleState);
        }

        public void Move()
        {
            Debug.Log("Player is Moving...");
        }

        public void Die()
        {
            Debug.Log("Player Died!");
        }
    }
}
