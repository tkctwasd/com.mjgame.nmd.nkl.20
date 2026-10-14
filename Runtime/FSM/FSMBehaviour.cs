using System;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// MonoBehaviour giu va chay StateMachine (player, bot, UI, cua, boss phase...).
/// </summary>
public abstract class FSMBehaviour : MonoBehaviour
{
    protected StateMachine StateMachine;
    public IState CurrentState => StateMachine?.CurrentState;

    /// <summary>
    /// Lan dau truy cap goi CreateMachine(). Cho phep module/tang tren
    /// Add state bat ke thu tu Awake.
    /// </summary>
    protected StateMachine Machine => StateMachine ??= CreateMachine();

    /// <summary>Tao may (override de dung StateMachine con, AddState state loi).</summary>
    protected virtual StateMachine CreateMachine() => new StateMachine(this);

    /// <summary>Delta time truyen cho state. UI chay khi pause: override tra Time.unscaledDeltaTime.</summary>
    protected virtual float DeltaTime => Time.deltaTime;

    /// <summary>Khoi tao (mac dinh: tao may). Override de dang ky state/transition.</summary>
    public virtual void Init() { }

    protected virtual void Awake()
    {
        Init();
        _ = Machine;
    }

    /// <summary>Dang ky state 1 lan (bo qua neu type da co). Dung cho module tu gan state.</summary>
    public void Add<T>(Func<IStateMachine, T> create) where T : class, IState
    {
        if (Machine.GetState<T>() != null) return;
        Machine.AddState(create(Machine));
    }

    /// <summary>Chuyen state (phai da dang ky).</summary>
    public void SetState<T>() where T : IState => Machine.ToState<T>();

    /// <summary>Dang o state T?</summary>
    public bool IsState<T>() where T : IState => StateMachine != null && StateMachine.IsInState<T>();

    /// <summary>Lay instance state da dang ky (null neu chua co).</summary>
    public T GetState<T>() where T : class, IState => Machine.GetState<T>();

    /// <summary>
    /// Chi DANG KY cap state, KHONG tu chuyen (khong co dieu kien). Muon tu chuyen: dung ban co check.
    /// </summary>
    public void AddTransition<T>(T from, T to) where T : IState
    {
        Machine.AddTransition(new Transition(from, to));
    }
    /// <summary>
    /// Transition tu chuyen khi check tra true.
    /// </summary>
    public void AddTransition(IState from, IState to, TransitionDelegate check)
    {
        Machine.AddTransition(new Transition(from, to, check));
    }
    /// <summary>
    /// Any State: tu bat ky state nao sang target khi check tra true.
    /// </summary>
    public void AddAnyTransition(IState to, TransitionDelegate check)
    {
        Machine.AddTransition(new Transition(null, to, check));
    }
    public void AddTransitionsTo<T>(T target, params T[] froms) where T : IState
    {
        foreach (var from in froms)
        {
            Machine.AddTransition(new Transition(from, target));
        }
    }
    public void AddState(IState state)
    {
        Machine.AddState(state);
    }

    /// <summary>
    /// Set current state
    /// </summary>
    /// <param name="state"></param>
    public void ToState<T>() where T : IState
    {
        Machine.ToState<T>();
    }

    public virtual void Update()
    {
        StateMachine?.OnUpdate(DeltaTime);
    }

    public virtual void FixedUpdate()
    {
        StateMachine?.OnFixedUpdate();
    }
    public virtual void LateUpdate()
    {
        StateMachine?.OnLateUpdate(DeltaTime);
    }
}
