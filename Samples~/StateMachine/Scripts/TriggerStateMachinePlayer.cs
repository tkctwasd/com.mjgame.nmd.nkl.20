using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{
    public class TriggerStateMachinePlayer : TriggerStateMachine<StateMachinePlayer>
    {
        [Button("Call Run")]
        public void Run()
        {
            stateMachineContext.Trigger("run");
        }

        [Button("Call Idle")]
        public void Idle()
        {
            stateMachineContext.Trigger("idle");
        }

        [Button("Call Dead")]
        public void Dead()
        {
            stateMachineContext.Trigger("dead");
        }
    }
}