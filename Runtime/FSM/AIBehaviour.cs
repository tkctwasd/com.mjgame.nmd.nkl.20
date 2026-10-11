using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AIBehaviour : MonoBehaviour
{
    protected StateMachine StateMachine;
    public IState CurrentState => StateMachine?.CurrentState;
    public abstract void Init();
    protected virtual void Awake()
    {
        Init();
        if (StateMachine == null)
        {
            Debug.LogError($"[FSM] {GetType().Name}.Init() phai tao StateMachine = new StateMachine(this).", this);
        }
    }

    public void AddTransition<T>(T from, T to) where T : IState
    {
        StateMachine.AddTransition(new Transition(from, to));
    }
    /// <summary>
    /// Transition tu chuyen khi check tra true.
    /// </summary>
    public void AddTransition(IState from, IState to, TransitionDelegate check)
    {
        StateMachine.AddTransition(new Transition(from, to, check));
    }
    /// <summary>
    /// Any State: tu bat ky state nao sang target khi check tra true.
    /// </summary>
    public void AddAnyTransition(IState to, TransitionDelegate check)
    {
        StateMachine.AddTransition(new Transition(null, to, check));
    }
    public void AddTransitionsTo<T>(T target, params T[] froms) where T : IState
    {
        foreach (var from in froms)
        {
            StateMachine.AddTransition(new Transition(from, target));
        }
    }
    public void AddState(IState state)
    {
        StateMachine.AddState(state);
    }

    /// <summary>
    /// Set current state
    /// </summary>
    /// <param name="state"></param>
    public void ToState<T>() where T : IState
    {
        StateMachine.ToState<T>();
    }

    public virtual void Update()
    {
        StateMachine?.OnUpdate(Time.deltaTime);
    }

    public virtual void FixedUpdate()
    {
        StateMachine?.OnFixedUpdate();
    }
    public virtual void LateUpdate()
    {
        StateMachine?.OnLateUpdate(Time.deltaTime);
    }
}
