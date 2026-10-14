using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public interface IStateMachine
    {
        /// <summary>FSMBehaviour so huu may.</summary>
        FSMBehaviour Owner { get; }
        [System.Obsolete("Dung Owner")]
        FSMBehaviour AIBehaviour { get; }
        IState CurrentState { get; }
        IState DefaultState { get; set; }
        void ToPrevState();
        void AddTransition(ITransition transition);
        void ToState<T>() where T : IState;
    }
}