## State Machine


> Version `1.0.0` 

> Update 2024-12-06 11:47:20

> by nmd
----

### Document
- Create an audience that wants to use the StateMachine model  `StateMachinePlayer : StateMachineContext<StateMachinePlayer>`

- Create state for objects StateMachine `StateMachinePlayer`:           
    - `IdleState : State<StateMachinePlayer>`
    - `RunState : State<StateMachinePlayer>`
    - `DeadState : State<StateMachinePlayer>`
- Register the state for the object in the file `StateMachinePlayer` and the object's motion functions

```csharp
protected override void RegisterTransition()
{
    base.RegisterTransition();

    // Khởi tạo các trạng thái
    var idleState = new IdleState(stateMachine, this);
    var runState = new RunState(stateMachine, this);
    var deadState = new DeadState(stateMachine, this);

    // Đăng ký các chuyển tiếp với trigger name
    stateMachine.RegisterTransition(idleState, runState, "run");
    stateMachine.RegisterTransition(runState, idleState, "idle");
    stateMachine.RegisterTransition(idleState, deadState, "dead");
    stateMachine.RegisterTransition(runState, deadState, "dead");

    // Bắt đầu với Idle state
    stateMachine.SetState(idleState);
}

public void Move()
{
    Debug.Log("Player is Moving...");
}

public void Die()
{
    Debug.Log("Player Died!");
}
```
- Create object state triggers ... `TriggerStateMachinePlayer : TriggerStateMachine<StateMachinePlayer>` 
- Create trigger functions that correspond to the registered states ...

```csharp
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
```
- use it: 
```csharp
TriggerStateMachinePlayer.Run("run");
```
