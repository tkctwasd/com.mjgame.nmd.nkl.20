using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{
    public class TriggerStateMachinePlayer : TriggerStateMachine<StateMachinePlayer>
    {
        [ContextMenu("Call Run")]
        public void Run()
        {
            stateMachineContext.Trigger("run");
        }

        [ContextMenu("Call Idle")]
        public void Idle()
        {
            stateMachineContext.Trigger("idle");
        }

        [ContextMenu("Call Dead")]
        public void Dead()
        {
            stateMachineContext.Trigger("dead");
        }
    }
}