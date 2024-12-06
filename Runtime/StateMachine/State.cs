using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJGame
{
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine;
        protected T context;
        public State(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        /// <summary>
        /// Ham se goi khi bat dau State 
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Ham se chay lien tuc State, thuc thi hanh dong cua State
        /// </summary>
        public virtual void Execute() { }


        /// <summary>
        /// Ham se goi khi ket thuc State
        /// </summary>
        public virtual void Exit() { }
    }

}