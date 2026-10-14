using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine : IStateMachine
    {
        public FSMBehaviour Owner { get; }
        [Obsolete("Dung Owner")]
        public FSMBehaviour AIBehaviour => Owner;
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
                RegisterState(value);
            }
        }

        public IState PrevState { get; set; }

        /// <summary>
        /// Goi sau moi lan doi state (prev, next). prev = null o lan vao DefaultState dau tien.
        /// </summary>
        public event Action<IState, IState> OnStateChanged;

        private IState _currentState;
        private IState _defaultState;
        private bool _started;
        private bool _changing;
        private IState _pending;
        private Dictionary<Type, IState> _states;
        private List<ITransition> _transitions;
        private Dictionary<IState, List<ITransition>> _transitionsFrom;
        private List<ITransition> _anyTransitions;
        public StateMachine(FSMBehaviour owner)
        {
            _transitions = new List<ITransition>();
            _states = new Dictionary<Type, IState>();
            _transitionsFrom = new Dictionary<IState, List<ITransition>>();
            _anyTransitions = new List<ITransition>();
            Owner = owner;
        }

        /// <summary>
        /// Them transition. From = null nghia la "Any State": kiem tra o moi state.
        /// Transition tu chuyen khi OnCheck tra true (va OnTransition, neu co, tra true).
        /// </summary>
        public void AddTransition(ITransition transition)
        {
            if (transition == null || _transitions.Contains(transition)) return;

            _transitions.Add(transition);
            RegisterState(transition.From);
            RegisterState(transition.To);

            if (transition.From == null)
            {
                _anyTransitions.Add(transition);
            }
            else
            {
                if (!_transitionsFrom.TryGetValue(transition.From, out var list))
                {
                    list = new List<ITransition>();
                    _transitionsFrom.Add(transition.From, list);
                }
                list.Add(transition);
            }
        }

        /// <summary>
        /// Dang ky state khong can transition (de ToState&lt;T&gt;() goi duoc).
        /// </summary>
        public void AddState(IState state)
        {
            RegisterState(state);
        }

        public T GetState<T>() where T : class, IState
        {
            return _states.TryGetValue(typeof(T), out IState state) ? state as T : null;
        }

        public bool IsInState<T>() where T : IState
        {
            return _currentState is T;
        }

        private void RegisterState(IState state)
        {
            if (state != null && !_states.ContainsKey(state.GetType()))
            {
                _states.Add(state.GetType(), state);
                state.Parent = this;
            }
        }

        /// <summary>
        /// Vao DefaultState (goi OnEnter). Tu goi o frame dau neu chua goi.
        /// </summary>
        public void Start()
        {
            if (_started) return;
            _started = true;
            if (_currentState == null && _defaultState != null)
            {
                ToState(_defaultState);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            Start();
            if (_currentState == null) return;

            _currentState.OnUpdate(deltaTime);
            CheckTransitions();
        }
        public void OnLateUpdate(float deltaTime)
        {
            Start();
            _currentState?.OnLateUpdate(deltaTime);
        }
        public void OnFixedUpdate()
        {
            Start();
            _currentState?.OnFixedUpdate();
        }

        private void CheckTransitions()
        {
            if (TryTransitions(_anyTransitions)) return;
            if (_currentState != null && _transitionsFrom.TryGetValue(_currentState, out var list))
            {
                TryTransitions(list);
            }
        }

        private bool TryTransitions(List<ITransition> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ITransition t = list[i];
                if (t.To == null || t.To == _currentState) continue;
                if (t.ShouldBegin() && t.TransitionCallback())
                {
                    ToState(t.To);
                    return true;
                }
            }
            return false;
        }

        public void ToState<T>() where T : IState
        {
            if (_states.TryGetValue(typeof(T), out IState newState))
            {
                ToState(newState);
            }
            else
            {
                Debug.LogWarning($"[FSM] State {typeof(T).Name} chua duoc dang ky (AddState / AddTransition / DefaultState).");
            }
        }
        public void ToState(IState state)
        {
            if (state == null) return;
            if (_changing)
            {
                _pending = state;
                return;
            }
            if (state == _currentState) return;

            RegisterState(state);
            _started = true;
            _changing = true;
            _currentState?.OnExit(state);
            IState oldState = _currentState;
            _currentState = state;
            PrevState = oldState;
            _currentState.OnEnter(oldState);
            OnStateChanged?.Invoke(oldState, state);
            _changing = false;

            if (_pending != null)
            {
                IState next = _pending;
                _pending = null;
                ToState(next);
            }
        }

        public void ToPrevState()
        {
            ToState(PrevState);
        }
    }
}
