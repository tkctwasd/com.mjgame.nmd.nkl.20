using System;
using System.Collections.Generic;

namespace MJGame
{

    public class StateTransitionManager<T>
    {
        private readonly Dictionary<Type, Dictionary<string, Type>> transitions;

        public StateTransitionManager()
        {
            transitions = new Dictionary<Type, Dictionary<string, Type>>();
        }

        /// <summary>
        /// Đăng ký chuyển tiếp trạng thái bằng tên trigger
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        /// <param name="triggerName"></param>
        public void RegisterTransition(State<T> fromState, State<T> toState, string triggerName)
        {
            if (!transitions.ContainsKey(fromState.GetType()))
            {
                transitions[fromState.GetType()] = new Dictionary<string, Type>();
            }

            transitions[fromState.GetType()][triggerName] = toState.GetType();
        }

        /// <summary>
        /// Kiểm tra và thực hiện chuyển tiếp khi trigger được gọi
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="currentState"></param>
        /// <param name="stateMachine"></param>
        public void Trigger(string triggerName, State<T> currentState, StateMachine<T> stateMachine)
        {
            if (transitions.ContainsKey(currentState.GetType()) && transitions[currentState.GetType()].ContainsKey(triggerName))
            {
                var targetStateType = transitions[currentState.GetType()][triggerName];
                var targetState = Activator.CreateInstance(targetStateType, stateMachine, stateMachine.GetContext()) as State<T>;

                stateMachine.SetState(targetState);
            }
        }
    }
}