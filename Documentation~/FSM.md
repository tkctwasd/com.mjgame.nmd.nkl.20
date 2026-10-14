# FSM (namespace `FSM`)

State machine nhẹ, state tra theo **type** (mỗi class state = 1 instance trong 1 máy).

## Thành phần
| Loại | Vai trò |
|---|---|
| `FSMBehaviour` | MonoBehaviour giữ máy, tự chạy `Update/LateUpdate/FixedUpdate` (player, bot, UI, cửa, boss...). `AIBehaviour` = tên cũ, obsolete. |
| `StateMachine` | Lưu state, transition, `ToState`, `ToPrevState`, event `OnStateChanged(prev, next)`. |
| `State` / `State<TOwner>` | Override `OnEnter/OnExit/OnUpdate/OnLateUpdate/OnFixedUpdate`. `Timer` = thời gian trong state. `State<TOwner>` có `Owner` đúng kiểu (bản thường: `stateMachine.Owner`). |
| `Transition` | Tự chuyển khi `check` trả true (kiểm tra sau `OnUpdate`, tối đa 1 lần/frame, Any State trước). |

## Dùng nhanh
```csharp
public class DoorFSM : FSMBehaviour
{
    public bool IsOpen;
    protected override StateMachine CreateMachine()
    {
        var m = new StateMachine(this);
        var closed = new DoorClosed(m);
        var open = new DoorOpen(m);
        m.DefaultState = closed;
        m.AddTransition(new Transition(closed, open, () => IsOpen));
        m.AddTransition(new Transition(open, closed, () => !IsOpen));
        return m;
    }
}

public class DoorOpen : State<DoorFSM>
{
    public DoorOpen(IStateMachine m) : base(m) { }
    public override void OnEnter(IState prev) { base.OnEnter(prev); /* Owner.... */ }
}
```

## API trên `FSMBehaviour`
- `Add<T>(m => new StateX(m))`: đăng ký nếu chưa có (module/tầng trên tự gắn state, gọi nhiều lần an toàn).
- `SetState<T>()` / `ToState<T>()`: chuyển state (phải đã đăng ký, nếu không chỉ cảnh báo).
- `IsState<T>()`, `GetState<T>()`, `CurrentState`.
- `AddTransition(from, to, check)`, `AddAnyTransition(to, check)`.
- Máy tạo lười qua `Machine` → `CreateMachine()`; không cần override `Init()`.
- `DeltaTime`: override trả `Time.unscaledDeltaTime` cho UI chạy khi `timeScale = 0`.

## Lưu ý
- `AddTransition(from, to)` **không có check** chỉ đăng ký state, không bao giờ tự chuyển.
- Chuyển sang state đang ở: bỏ qua (không OnEnter lại).
- Gọi `ToState` trong `OnEnter/OnExit/OnStateChanged`: được xếp hàng, chạy sau khi lần đổi hiện tại xong.
- Cần 2 state cùng hành vi khác tham số → 2 class (kế thừa chung).
- UI: FSM cho chế độ trong 1 panel (Loading/Content/Error) hoặc luồng màn; mở/đóng panel vẫn dùng `UIManager`.
