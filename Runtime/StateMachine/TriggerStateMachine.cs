using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MJGame
{
    public abstract class TriggerStateMachine<T> : TickBehaviour where T : StateMachineContext<T>
    {
        public T stateMachineContext;
    }
}