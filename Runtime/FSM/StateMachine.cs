using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine : IStateMachine
    {
        public AIBehaviour AIBehaviour { get; }
        public IState CurrentState
        {
            get
            {
                return _currentState;
            }
        }
        public IState DefaultState
        {
            get
            {
                return _defaultState;
            }
            set
            {
                _defaultState = value;
            }
        }

        public IState PrevState { get; set; }

        private IState _currentState;
        private IState _defaultState;
        private Dictionary<Type, IState> _states;
        private List<ITransition> _transitions;
        public StateMachine(AIBehaviour aIBehaviour)
        {
            _transitions = new List<ITransition>();
            _states = new Dictionary<Type, IState>();
            AIBehaviour = aIBehaviour;
        }
        public void AddTransition(ITransition transition)
        {
            if (!_transitions.Contains(transition))
            {
                _transitions.Add(transition);
                RegisterState(transition.From);
                RegisterState(transition.To);
            }
        }
        private void RegisterState(IState state)
        {
            if (state != null && !_states.ContainsKey(state.GetType()))
            {
                _states.Add(state.GetType(), state);
                state.Parent = this;
            }
        }
        public void OnUpdate(float deltaTime)
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }

            else
                _currentState.OnUpdate(deltaTime);
        }
        public void OnLateUpdate(float deltaTime)
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }
            else
                _currentState.OnLateUpdate(deltaTime);
        }
        public void OnFixedUpdate()
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }
            else
            {
                _currentState.OnFixedUpdate();
            }
        }
        public void ToState<T>() where T : IState
        {
           if (_states.TryGetValue(typeof(T), out IState newState))
            {
                ToState(newState);
            }
        }
        protected void ToState(IState state)
        {
            if (state == _currentState || state == null) return;

          _currentState?.OnExit(state);
            IState oldState = _currentState;
            _currentState = state;
            PrevState = oldState;
            _currentState.OnEnter(oldState);
        }

        public void ToPrevState()
        {
            ToState(PrevState);
        }
    }
}