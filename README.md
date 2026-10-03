# mindongun — Core 2.0

| | |
|---|---|
| Package | `com.mjgame.nmd.nkl.20` |
| Ten hien thi | mindongun |
| Phien ban | **2.0.1** (2026-10-03, Core 2, thay the Core 1.x) |
| Unity | 2022.3 tro len |
| Tac gia | mindongun |

Khung dung chung cho game mobile Unity (hybrid-casual). Gom he thong UI, Save, Pool, State Machine,
Audio, Camera, Joystick, Indicator, Interaction, Extensions va tool Editor.
Khung la **assembly rieng** va **khong goi toi bat ky class nao cua game**: game cam vao khung bang
ke thua, override, interface va lua chon tren Inspector.

## Muc luc

1. [Cai dat](#cai-dat)
2. [Assembly](#assembly)
3. [Bat dau nhanh](#bat-dau-nhanh)
4. [Cac he thong](#cac-he-thong)
   - [Singleton va PlayerBootstrap](#singleton-va-playerbootstrap)
   - [Save](#save)
   - [UI](#ui)
   - [Pool](#pool)
   - [State Machine (FSM)](#state-machine-fsm)
   - [Audio](#audio)
   - [Camera](#camera)
   - [Joystick va touch](#joystick-va-touch)
   - [Indicator](#indicator)
   - [Interaction](#interaction)
   - [Extensions va Attributes](#extensions-va-attributes)
   - [Tien ich khac](#tien-ich-khac)
   - [Tool Editor](#tool-editor)
5. [Cach game cam vao khung](#cach-game-cam-vao-khung)
6. [Quy uoc](#quy-uoc)
7. [Xu ly loi thuong gap](#xu-ly-loi-thuong-gap)
8. [Sua khung va nang phien ban](#sua-khung-va-nang-phien-ban)

---

## Cai dat

### Thu vien can co

| Thu vien | Phien ban | Cach co |
|---|---|---|
| uGUI (`com.unity.ugui`) | 1.0.0 | `package.json` |
| TextMeshPro (`com.unity.textmeshpro`) | 3.0.7 | `package.json` |
| Cinemachine (`com.unity.cinemachine`) | 2.10.7 | `package.json` |
| Newtonsoft Json (`com.unity.nuget.newtonsoft-json`) | 3.2.2 | `package.json` |
| **DOTween** | bat ky | **Import rieng** (Asset Store) |

**DOTween bat buoc lam them buoc nay:** mo `Tools > Demigiant > DOTween Utility Panel`, bam **Create ASMDEF**
de sinh `DOTween.Modules.asmdef`. Thieu buoc nay khung khong bien dich.

Khong can MaxSdk, Firebase, Adjust: quang cao, IAP va analytics nam trong package rieng
**`com.mjgame.nmd.nkl.20.monetization`** (phu thuoc Core, xem README cua package do).

### Cach 1: chep vao Assets (nhanh, sua truc tiep duoc)

1. Chep ca folder nay vao `Assets/` (Steal an Egg dat o `Assets/_ROOT/Script/_mindongun`).
2. Them 4 package o bang tren vao `Packages/manifest.json` neu project chua co.
3. Import DOTween va Create ASMDEF.

`package.json` nam trong `Assets` thi Unity bo qua, khong sao.

### Cach 2: dung nhu package (tach han khoi game)

1. Dong Unity, chuyen folder nay ra `Packages/com.mjgame.nmd.nkl.20/`, mo lai Unity.
   Hoac day folder len git rieng roi cai bang URL trong Package Manager.
2. Unity tu cai cac dependency trong `package.json`.
3. Import DOTween va Create ASMDEF.

GUID cua file khong doi khi chuyen giua 2 cach, nen prefab va scene khong mat tham chieu.

---

## Assembly

| Assembly | File | Nen tang | Tham chieu |
|---|---|---|---|
| `com.mjgame.nmd.nkl.20` | `com.mjgame.nmd.nkl.20.asmdef` | Tat ca | DOTween.Modules, Unity.TextMeshPro, Cinemachine, UnityEngine.UI |
| `com.mjgame.nmd.nkl.20.Editor` | `Editor/com.mjgame.nmd.nkl.20.Editor.asmdef` | Editor | runtime (theo GUID) + cac thu vien tren |
| `com.mjgame.nmd.nkl.20.UI.Editor` | `Runtime/UI/Editor/com.mjgame.nmd.nkl.20.UI.Editor.asmdef` | Editor | runtime (theo GUID) + cac thu vien tren |

- Ca 3 deu `autoReferenced`, nen code game (Assembly-CSharp) dung duoc ngay, khong can khai bao gi.
- Asmdef Editor tham chieu runtime bang `GUID:508b9260f11172144bdada14a1c9659e` (GUID cua `com.mjgame.nmd.nkl.20.asmdef.meta`),
  nen doi ten assembly runtime sau nay khong lam gay tham chieu. **Khong xoa/tao lai file `.meta` nay.**
- Namespace: hau het class o global. Ngoai le: `FSM` (state machine), `UnityExtensions` (extension method),
  `Assets.Scripts.Helpers` (`FPSCounter`), `PlistCS` (tool Editor).

---

## Bat dau nhanh

Set up toi thieu cho mot game moi:

**1. Save cua game**

```csharp
public partial class DataSave : SaveBase<DataSave>
{
    public DataGame DataGame { get; private set; }
    private const string KEY_GAME = "data_game_fls";

    protected override void OnLoadGame() => DataGame = JsonDataUtility.LoadData<DataGame>(KEY_GAME) ?? new DataGame();
    protected override void OnSaveGame() => JsonDataUtility.SaveData(KEY_GAME, DataGame);
    protected override void OnNewGame() => DataGame = new DataGame();
    protected override void OnDeleteSave() => JsonDataUtility.DeleteData(KEY_GAME);
}
```

`DataSave` phai **ton tai luc chay** (prefab trong Bootstrap o buoc 3, hoac
`[RuntimeInitializeOnLoadMethod] static void Bootstrap() => _ = Instance;`). Khong co object `DataSave`
thi khong load/save gi ca, PlayerPrefs se khong co key. Dung Monetization: lam them theo README cua package do.

**2. Prefab trong `Resources/`** (moi prefab mot component chinh):
`DataSave`, `AudioManager` (gan `AudioConfig`), `Canvas` (co `UIManager`, cac panel la con cua `panelParent`),
`PoolManager`, `MainCamController`.

**3. Bootstrap ben game** (load prefab truoc scene dau tien):

```csharp
public static class BootstrapScene
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        TryLoad("DataSave");
        TryLoad("AudioManager");
        TryLoad("Canvas");
        TryLoad("PoolManager");
        TryLoad("MainCamController");
    }

    static void TryLoad(string prefabName)
    {
        if (GameObject.Find(prefabName) != null) return;
        var prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null) { Debug.LogWarning($"Khong tim thay prefab {prefabName} trong Resources"); return; }
        var instance = Object.Instantiate(prefab);
        instance.name = prefabName;
        Object.DontDestroyOnLoad(instance);
    }
}
```

**4. Player** implement `IPlayer` va dang ky (indicator can vi tri player):

```csharp
public class PlayerController : MonoBehaviour, IPlayer
{
    public Transform Root => transform;
    private void OnEnable() => PlayerBootstrap.Register<IPlayer>(this);
    private void OnDisable() => PlayerBootstrap.Unregister<IPlayer>();
}
```

**5. Panel dau tien**: tren Inspector cua panel loading chon `Roles = ShowOnStart`; panel thong bao chon `AlwaysOnTop`.

**6. Am thanh click**: `AudioConfig` can mot entry ten `ClickButton` (`Consts.Au_ClickButton`).

---

## Cac he thong

### Singleton va PlayerBootstrap

`Singleton<T> : MonoBehaviour`

| Thanh vien | Mo ta |
|---|---|
| `static T Instance` | Tra instance; neu chua co thi `FindObjectOfType`, van khong co thi tao GameObject moi |
| `static bool HasInstance` | Kiem tra ma **khong tao moi**. Dung trong `OnDestroy`/luc thoat game |
| `protected virtual void Awake()` | Gan instance, huy ban trung. Override thi goi `base.Awake()` truoc |
| `protected virtual void OnDestroy()` | Xoa instance |

Singleton **khong tu DontDestroyOnLoad**; manager song xuyen scene tu goi trong `Awake`
(`SaveBase`, `UIManager`, `PoolManager` da lam san).

`PlayerBootstrap` (static): registry theo type, de cac phan cua player tim nhau ma khong can Find.

```csharp
PlayerBootstrap.Register<PlayerMovement>(this);   // OnEnable
PlayerBootstrap.Unregister<PlayerMovement>();     // OnDisable
var movement = PlayerBootstrap.Get<PlayerMovement>(); // log warning neu chua co
if (PlayerBootstrap.Has<PlayerMovement>()) { ... }
PlayerBootstrap.UnregisterAll();
```

Key la type generic: `Register<IPlayer>(this)` va `Register<PlayerController>(this)` la 2 muc khac nhau.

### Save

`SaveBase<T> : Singleton<T>` — luu JSON (Newtonsoft) vao PlayerPrefs.

- Tu load trong `Awake`, DontDestroyOnLoad, tu save khi pause / mat focus / thoat.
- Data chung moi game: `DataSetting`, `DataAvatar`, `DataRate`.
- Data cua package khac (vd `DataMonetization` cua Monetization) do `DataSave` cua game giu va luu,
  roi gan vao static `Current` cua class do (package khong thay duoc `DataSave`).
- API: `SaveGame()`, `LoadGame()`, `NewGame()`, `DeleteSave()`.
- Hook cho game: `OnLoadGame`, `OnSaveGame`, `OnNewGame`, `OnDeleteSave` (xem [Bat dau nhanh](#bat-dau-nhanh)).

| Class | Truong |
|---|---|
| `DataSetting` | `Music`, `Sound`, `Vibrate`, `Sensitivity` (JSON key `Sensivity`), `Distance`; `static Current` = ban dang dung |
| `DataAvatar` | `SkinCurrent`, `ListSkinUnlocked`, `SetUnlockSkin(id)`, `MAX_SKIN` |
| `DataRate` | `IsRate`, `NumberShowRate`, `LastShowTime` |

`JsonDataUtility`: `SaveData(key, obj)`, `LoadData<T>(key)` (tra `default` neu chua co key), `DeleteData(key)`.

Luu y khi viet class save:
- List dung `[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]`, neu khong Newtonsoft se **noi them** vao list tao trong constructor.
- Doi ten field thi giu key cu: `[JsonProperty("TenCu")]`.
- Enum luu bang so: chi them gia tri vao cuoi.
- Bu du lieu cho save cu trong `[OnDeserialized]`.

### UI

**`UIManager : Singleton<UIManager>`** (tren Canvas song xuyen scene)

- `Awake`: tim moi `UIPanel` con cua `panelParent`, dang ky theo type, tat het.
- `Start`: hien panel co role `ShowOnStart`.

| API | Mo ta |
|---|---|
| `ShowPanel<T>()` / `ShowPanel(panel)` | Hien panel, dua panel `AlwaysOnTop` len tren cung |
| `GetPanel<T>()` | Lay panel (log error neu khong co) |
| `HidePanel<T>()`, `HideAllPanels()` | An panel |
| `IsAnyPanelActive(params Type[])`, `HasAnyPanelActiveExcept(params Type[])` | Kiem tra panel dang mo |
| `SetAllPanelsOpacity(alpha)` | Doi alpha moi panel |
| `RegisterPanel(panel)` | Dang ky panel tao luc runtime |

**`UIPanel`** (can `CanvasGroup`)

- `Show()` / `Hide()` virtual; `IsShown`; event `OnPanelShown`, `OnPanelHidden`.
- `Roles` (`PanelRole`): `None`, `ShowOnStart`, `AlwaysOnTop`. Chon tren Inspector.
- `elementGroups`: nhom animation hien/an lan luot (xem va preview bang Inspector cua `UIPanel`).

```csharp
public class PanelShop : UIPanel
{
    [Header("Button")]
    public Button btnClose;

    protected override void Awake()
    {
        base.Awake();
        btnClose.onClick.AddListener(OnClickBtnClose);
        btnClose.AddResizeEffect();
        btnClose.AddDelayedReveal();
    }

    private void OnClickBtnClose()
    {
        AudioManager.Instance.AudioClickBtn();
        Hide();
    }
}

// Mo tu cho khac
UIManager.Instance.ShowPanel<PanelShop>();
```

**Danh sach tai su dung: `UIItem<TData>` + `UIReusableList<TData, TItem>`**

```csharp
public class ItemShop : UIItem<ItemShopData>
{
    public TextMeshProUGUI txtName;

    public override void Bind(ItemShopData data, int index, bool play = true)
    {
        txtName.text = data.Name;
        base.Bind(data, index, play); // goi cuoi: chay animation xuat hien theo index
    }
}

// Trong panel
reusableList = new UIReusableList<ItemShopData, ItemShop>(itemPrefab, content);
reusableList.Refresh(listData, play: true); // tao them item neu thieu, an item thua
```

**`InputButton`**: nut HUD. Tu gan click + hieu ung nhan; co `hand` (tay huong dan) va `notice` (cham do).
Override `OnClickInputBtn()` (goi `base` de phat am click va an tay). `ActiveHand(bool)`, `ActiveNotice(bool)`, `Complete()`.

**`UIProcess`** (file `UIProgress.cs`): thanh tien trinh co text. `Setup(value, min, max)`, `SetValue`, `AddValue`,
`SetNormalized`, `SetMinMax`, `SetColor`, `Refresh`. `TextMode`: `Hide`, `Percent`, `Fraction`, `Value`, `Remain`, `Custom`.

**Khac**: `LoadingOverlay`, `LoopText`, `UIScroll`; hieu ung trong `UI/Utility`
(`UIButtonPressEffect`, `UIDelayedReveal`, `UIElementAnimation`, `UIElementTransition(Auto)`, `UIElementInteractive`, `UIShineRectSync`).

### Pool

**`PoolManager : Singleton<PoolManager>`** (DontDestroyOnLoad). Khai bao san pool tren Inspector
(`PoolConfig`: `poolId`, `prefab`, `initialSize`, `autoExpand`) hoac tao luc can.

| API | Mo ta |
|---|---|
| `Spawn(poolId)`, `Spawn(poolId, pos)`, `Spawn(poolId, pos, rot)` | Lay object tu pool co san |
| `Spawn(poolId, prefab)` | Tao pool neu chua co roi lay |
| `ReturnToPool(poolId, obj)` | Tra object |
| `ReturnAllToPool(poolId)`, `ReturnAllToAllPools()` | Tra het (goi khi doi scene) |
| `GetActiveObjects(poolId)`, `GetPool(poolId)`, `CreatePool(config)`, `DestroyPool(poolId)` | Quan ly pool |

Object trong pool implement `IPoolable` (`OnSpawnFromPool`, `OnReturnToPool`) hoac ke thua `PoolableObject`:

```csharp
public class Coin : PoolableObject
{
    protected override void OnSpawn() { /* reset trang thai */ }
    protected override void OnDespawn() { /* dung tween, coroutine */ }
}
```

`PoolableObject`: `PoolId`, `ReturnToPool()` (tu `Destroy` neu khong den tu pool), tu tra sau `autoReturnDelay` neu bat `autoReturnToPool`.
`Awake/Start` chi chay 1 lan: reset **moi** trang thai trong `OnSpawn`/`OnDespawn`.

### State Machine (FSM)

`using FSM;`

| Type | Vai tro |
|---|---|
| `State` | Trang thai. Override `OnEnter(prev)`, `OnExit(next)`, `OnUpdate(dt)`, `OnLateUpdate(dt)`, `OnFixedUpdate()`. Co `Timer`, `Parent` |
| `StateMachine` | `new StateMachine(aiBehaviour)`; `AddTransition(t)`, `ToState<T>()`, `ToPrevState()`, `CurrentState`, `PrevState`, `DefaultState` |
| `Transition` | `new Transition(from, to)`; event `OnCheck` (tu chuyen khi tra true), `OnTransition` |
| `AIBehaviour` | MonoBehaviour chu FSM. Implement `Init()` (goi trong `Awake`); `AddTransition(from, to)`, `AddTransitionsTo(target, froms...)`, `ToState<T>()`; tu goi update cua FSM |

```csharp
public class StateEnemyIdle : State
{
    public StateEnemyIdle(IStateMachine sm) : base(sm) { }
    public override void OnEnter(IState prev) { base.OnEnter(prev); /* bat animation idle */ }
}

public class EnemyAnim : AIBehaviour
{
    public override void Init()
    {
        StateMachine = new StateMachine(this);
        var idle = new StateEnemyIdle(StateMachine);
        var run = new StateEnemyRun(StateMachine);
        AddTransition(idle, run);
        AddTransition(run, idle);
    }
}
```

**State chi duoc dang ky qua transition.** State khong co transition nao thi `ToState<T>()` khong lam gi.

### Audio

**`AudioManager : Singleton<AudioManager>`** tren prefab, gan `AudioConfig` (tao bang `Create > Common > AudioConfig`).
Luon goi qua `AudioManager.Instance`.

`AudioConfig`: mixer + ten tham so volume, gioi han so AudioSource moi key, 3 danh sach `AudioData`
(`AudioMusicData`, `AudioSoundData`, `AudioSpeechData`).
`AudioData`: `name` (key), `clips` (chon ngau nhien), `isLoop`, `volume`, `pitchMin/Max`, `is3D` + khoang cach, `mixerGroup`.

| API | Dung cho |
|---|---|
| `PlaySoundOne(key)`, `PlaySoundOneIfNotPlaying(key)` | SFX |
| `PlaySound3D(key, pos)` | SFX theo vi tri |
| `PlayMusic(key)`, `CrossfadeTo(key, d)`, `CrossfadeMusic(from, to, d)`, `FadeOutMusic(key, d)` | Nhac |
| `StopMusic(key)`, `StopSounds(key)`, `FadeOutSound(key, d)`, `StopAll()`, `StopAllMusic()`, `StopAllSounds()`, `StopAllSoundsSpeech()` | Dung |
| `PauseAllMusic()`, `ContinueAll()` | Tam dung / tiep tuc |
| `ToggleMusic(bool)`, `ToggleSound(bool)`, `SetMusicVolume(x)`, `SetSFXVolume(x)` | Setting; event `OnMusicToggled`, `OnSoundToggled` |
| `GetSoundIsPlaying(key)`, `GetMusicIsPlaying(key)` | Kiem tra |
| `AudioClickBtn()` | Am click (`Consts.Au_ClickButton`) |

Bat/tat am thanh da doc tu `DataSetting.Current`, noi goi khong can kiem tra lai.
`Sound3D`: component phat am 3D gan tren object.

### Camera

**`MainCamController : Singleton<MainCamController>`** (Cinemachine, camera ngoi thu 3 va thu nhat)

| API | Mo ta |
|---|---|
| `SetCameraInit(third, first)`, `SetFollowPlayer(third, first)` | Gan target sau khi spawn / dich chuyen |
| `ActiveCamera(third, first)` | Chon camera dang dung |
| `SetDistance(d)`, `ResetCameraDistance()` | Khoang cach (setting nguoi choi: `DataSetting.Distance`) |
| `SetFieldOfView(fov, duration)`, `ResetZoom()` | Zoom |
| `SetDampingCamera(x, y, z)`, `SetTimeChangeCam(t)`, `SetShakeAmplitude(a, max)` | Tinh chinh |

`CameraShake.ShakeImpulse(duration, amplitude)`: rung camera trong mot khoang thoi gian (coroutine); `IsImpulsing` = dang rung, game khong nen ghi de bien do luc nay.

### Joystick va touch

- `Joystick` / `FloatingJoystick`: `Horizontal`, `Vertical`, `Direction`, `ActiveHint(bool)`, `ResetJoystick()`;
  override `HandleInput(...)`, `OnPointerDown/Up` de doi hanh vi. `AxisOptions`: `Both`, `Horizontal`, `Vertical`.
- `FixedTouchField`: vung vuot man hinh de xoay camera.
- `TouchRaycast`, `TouchInteraction`, `RaycastContinue`: cham vao object 3D qua UI.
- Joystick va `FixedTouchField` tu giu object huong dan cua minh (`tutHint`), khong phu thuoc panel nao.

### Indicator

Nut hanh dong noi tren object trong the gioi 3D (mua, mo khoa, lay do...).

Luong hoat dong:
1. `IndicatorTarget` tren object (`indicatorType`, `DistanceDisplay`, `Offset`) tu dang ky vao `IndicatorManager` khi `OnEnable`.
2. `IndicatorController` (dat trong HUD) lay vi tri player qua `PlayerBootstrap.Get<IPlayer>()`, hien prefab tuong ung tu `IndicatorConfig`.
3. Nguoi choi bam: `Indicator.Click()` chay theo `clickType` (`Click`, `Hold` giu cho day vong, `Auto`).

| Type | API |
|---|---|
| `Indicator` | `IndicatorType`, `clickType`, `hand`, `txt`, `txtTitle`, `ObjectIndicator`; virtual `Refresh()`, `Click()`, `SetText`, `SetTitle`, `Configure(target)`; `Activate`, `ActiveHand`, `StartAuto`, `StopAuto` |
| `IndicatorTarget` | `SetIndicator(type, distance, offset)`, `SetIndicatorType(type)` |
| `IndicatorManager` (static) | `SetShowState(type, bool)`, `CanShow(type)`, `GetIndicator(type)`, `RefreshAllActive()` |
| `IndicatorConfig` (SO, `Create > Indicator > IndicatorConfig`) | Danh sach `{Type, Prefab, Show}`; `GetPrefab(type)`, `CanShow(type)` |
| `TriggerIndicator` | Cho phep / chan mot loai indicator khi player dung trong vung |

```csharp
public class IndicatorUnlockDoor : Indicator
{
    public override void Refresh()
    {
        if (ObjectIndicator != null && ObjectIndicator.TryGetComponent(out DoorLock door))
            SetText(Formatter.FormatNumber(door.Price));
    }

    protected override void Click()
    {
        if (ObjectIndicator != null && ObjectIndicator.TryGetComponent(out DoorLock door))
            door.Unlock();
        base.Click(); // goi cuoi: am click, an tay, refresh moi indicator
    }
}
```

**`enum IndicatorType`** (`Runtime/Indicator/IndicatorTarget.cs`) la cho duy nhat game phai sua trong khung:
them loai moi **vao cuoi**. Prefab va `IndicatorConfig` luu enum bang so, chen giua se lech het.

### Interaction

Interface va lop co so cho object tuong tac:

| Type | Thanh vien |
|---|---|
| `IItems` | Danh dau item |
| `IItemInteract` / `ItemsInteract` | `Interact()` |
| `IItemTrigger` | `InteractPlayer()`, `CancelPlayer()` |
| `IItemAction` / `ItemsAction` | `Action(object obj = null)`, `Cancel()` |
| `Door`, `DoorTrigger`, `DoorInteract`, `DoorAction` | Cua mo/dong mau |

Bo dinh tuyen va cham (`OnTriggerEnter` → player / bot) **khong nam trong khung** vi no can biet class bot cua game.
Game tu viet, vi du:

```csharp
public class ItemsTrigger : MonoBehaviour, IItemTrigger
{
    public virtual void InteractPlayer() { }
    public virtual void CancelPlayer() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player)) InteractPlayer();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.Tag_Player)) CancelPlayer();
    }
}
```

### Extensions va Attributes

`using UnityExtensions;`

| Ham | Mo ta |
|---|---|
| `GameObjectExtensions.ActiveObj(go, bool)` | `SetActive` an toan null, bo qua neu khong doi |
| `selectable.AddResizeEffect(scale)` | Hieu ung nhan nut |
| `button.AddDelayedReveal(delay)` | Hien nut sau mot khoang (nut dong popup) |
| `transform.GetAllChildComponents<T>()`, `GetDirectChildComponents<T>()`, `GetComponentsInChildrenExcludeSelf<T>()` | Lay component con |
| `AnimationExtensions.PlayAppear(t, index, ...)`, `PlayPunchScale(t)` | Animation xuat hien |
| `CalculationExtensions.Roll<T>(rates)`, `RollByIndexWeight(list)`, `RangeNumber(a, b)`, `IntoTimeCountDownLimitedHours(t)` | Random, thoi gian |
| `Vector3Extensions.DistanceIndependentY(a, b)` | Khoang cach bo qua truc Y |

`UITweenExtension` (global): `PopIn`, `PopOut`, `Bump`, `Deny`, `Press`, `Release`, `Fade`; `tween.Bind(owner)`: tween chay ca khi `Time.timeScale = 0` va tu kill khi owner bi tat.

Attribute cho Inspector: `[Button]` (chay ham), `[ReadOnly]`, `[ColorLabel]` / `[ColorLabel(r, g, b)]`, `[ShowIf(nameof(field), value)]`.

### Tien ich khac

| Type | Mo ta |
|---|---|
| `Formatter` | `FormatNumber(double)` (1.2k, 3.4M...), `FormatTime(int giay)` (mm:ss), `FormatName(string)`, `FormatColor(Color)` |
| `Consts` | Key khung dung: `Tag_Player`, `Au_ClickButton`. Key cua game de trong class rieng cua game |
| `Vibration.Vibrate()` | Rung (doc `DataSetting.Current.Vibrate`, bo qua tren standalone) |
| `CoroutineHelper` | `StartCoroutineStatic(ie)`, `StopCoroutineStatic(c)`, `LoopCoroutine(...)` cho code static |
| `LogMessage` | Log co tag. Tag Ads/IAP/Firebase nam o `MonetLogMessage` cua Monetization |
| `MeshDrawPath` | Ve duong huong dan tren mat dat |
| `FPSCounter`, `FPSUI`, `GUIGlitch` | Debug hieu nang |

### Tool Editor

| Tool | Vi tri |
|---|---|
| Cua so PlayerPrefs (xem, sua, xoa save) | `Editor/PlayerPref` |
| Remove Missing Scripts | `Editor/RemoteMissingScript` |
| Inspector + preview animation `UIPanel` / `UIElementTransition` | `Runtime/UI/Editor` |

---

## Cach game cam vao khung

| Khung can gi | Game lam gi |
|---|---|
| Data rieng cua game | `DataSave : SaveBase<DataSave>`, override 4 hook |
| Setting am thanh, rung, do nhay, camera | Doc/ghi `DataSetting.Current` |
| Panel mo dau / luon tren cung | `PanelRole` tren Inspector: `ShowOnStart` / `AlwaysOnTop` |
| Vi tri player | Implement `IPlayer`, `PlayerBootstrap.Register<IPlayer>(this)` |
| Nut HUD | `ButtonXxx : InputButton` |
| Nut hanh dong tren world | `IndicatorXxx : Indicator` + gia tri moi o cuoi `IndicatorType` |
| Vung trigger | `ItemsTrigger : MonoBehaviour, IItemTrigger` ben game |
| Object tai su dung | `IPoolable` / `PoolableObject` |
| Animation, AI | `State`, `AIBehaviour` |
| Am thanh | `AudioData` trong `AudioConfig` + `AudioManager.Instance.PlayXxx(key)` |
| Prefab song xuyen scene | `BootstrapScene` ben game |

**Khong nam trong khung** (game tu giu, chep tu project gan nhat roi sua): Player, Bot, Enemy, Map, Controller,
Manager, Tutorial, Obstacle, SO cua game, `BootstrapScene`, `ItemsTrigger`, key/enum/event cua game
(`Preconsts`, `Types`, `EventAction`), `Converter`.
Ads, IAP, Firebase, Remote Config: dung package `com.mjgame.nmd.nkl.20.monetization`.

---

## Quy uoc

- Field tham chieu Inspector: `public` camelCase co tien to loai (`btnClose`, `txtMoney`, `imgIcon`); field data: PascalCase.
- Field private camelCase khong tien to; hang so `UPPER_SNAKE`; list `LsXxx`, dictionary `dictXxx`.
- Nut: gan `onClick` trong `Awake`, `AddResizeEffect()`, handler `OnClickBtnXxx()` bat dau bang `AudioManager.Instance.AudioClickBtn()`.
- Bat/tat object bang `GameObjectExtensions.ActiveObj`.
- Dang ky event trong `OnEnable`, huy trong `OnDisable`. Tween giu trong field va `Kill()` truoc khi chay lai.
- Comment tieng Viet khong dau. Log co tag mau: `Debug.Log("<color=yellow>[PoolManager]</color> ...")`.
- Enum luu vao prefab/save: chi them gia tri vao cuoi.

---

## Xu ly loi thuong gap

| Trieu chung | Nguyen nhan / cach sua |
|---|---|
| `The type or namespace name 'DG' could not be found` trong khung | Chua Create ASMDEF cho DOTween (xem [Cai dat](#cai-dat)) |
| Asmdef Editor bao thieu tham chieu runtime | File `com.mjgame.nmd.nkl.20.asmdef.meta` bi tao lai, GUID doi. Lay lai `.meta` cu hoac sua GUID trong 2 asmdef Editor |
| Prefab mat script sau khi chep khung | Import khung khi project van con ban cu cua cung script → Unity cap GUID moi. Xoa ban cu truoc, roi moi dua khung vao |
| `[UIManager] Panel of type X not found` | Panel khong nam duoi `panelParent` cua Canvas, hoac thieu component |
| `Audio not found: X` | `AudioConfig` khong co entry ten `X` (ten phai trung key) |
| `[Singleton] Instance ... already exists` | Co 2 object cung manager (vd: prefab trong scene va prefab load tu Bootstrap). Giu mot |
| Indicator khong hien | Chua dang ky `IPlayer`, thieu entry trong `IndicatorConfig`, hoac `Show = false` |
| `ToState<T>()` khong doi state | State chua co transition nao |
| Save cu load ra list trung phan tu | Thieu `ObjectCreationHandling.Replace` tren list |
| PlayerPrefs khong co key cua game | Chua co object `DataSave` luc chay, hoac chua save lan nao (save khi pause / mat focus / thoat / `NewGame()`) |

Kiem tra bien dich ngoai Unity (sau khi Unity sinh `.csproj`, chay o thu muc goc project):

```
dotnet build com.mjgame.nmd.nkl.20.csproj -v q
dotnet build com.mjgame.nmd.nkl.20.Editor.csproj -v q
```

---

## Sua khung va nang phien ban

- **Khong** sua khung de chieu mot game. Neu game can mot cho cam moi, them diem mo rong **chung**
  (ham virtual, event, interface, gia tri `PanelRole`) roi de game override.
- Khung khong duoc tham chieu class cua game (assembly rieng, se khong bien dich). `partial class` khong the chia giua khung va game.
- Moi lan doi API: tang `version` (va `release`) trong `package.json`, ghi `CHANGELOG.md`, cap nhat tai lieu nay.
- Doi ten assembly runtime: giu nguyen file `.meta` (GUID) de asmdef Editor khong gay.

Lich su thay doi: xem [CHANGELOG.md](CHANGELOG.md).
