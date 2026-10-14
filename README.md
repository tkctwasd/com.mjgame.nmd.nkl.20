# mindongun — Core 3.0

| | |
|---|---|
| Package | `com.mjgame.nmd.nkl.20` |
| Ten hien thi | mindongun |
| Phien ban | **3.0.0** (2026-10-14, Core 3: Save he moi `DataModule`) — xem [CHANGELOG](CHANGELOG.md) |
| Tai lieu chi tiet | [Documentation~/Data.md](Documentation~/Data.md) (Save) |
| Unity | 2022.3 tro len |
| Tac gia | mindongun |

Khung dung chung cho game mobile Unity (hybrid-casual). Gom he thong UI, Save, Pool, State Machine,
Audio, Camera, Joystick, Indicator, Interaction, Extensions va tool Editor.
Khung la **assembly rieng** va **khong goi toi bat ky class nao cua game**: game cam vao khung bang
ke thua, override, interface va lua chon tren Inspector.

### Co gi moi o 3.0

- **Save viet lai theo `DataModule<T>`**: moi khoi data 1 class, tu luu bang key `data_<ten_snake>`, goi `T.Current`
  (hoac `DataSave.Instance.X` neu khai bao property trong `DataSave` cua game). Khong con key / load / save viet tay.
- `SaveBase` chi con vong doi save; hook override doi thanh event `OnLoaded/OnSaved/OnNewGame/OnDeleted`.
- Package chi giu `DataSetting` (Audio/Camera/Vibration dung). `DataAvatar`, `DataRate` chuyen ve game.
- **Nang tu 2.x**: lam theo muc "Chuyen tu ban cu" trong [Documentation~/Data.md](Documentation~/Data.md). Setting cu cua nguoi choi se mat (doi key).

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
// 1 class trong game (gan len prefab Bootstrap), chi chua property de goi DataSave.Instance.X
public class DataSave : SaveBase<DataSave>
{
    public DataGame DataGame => DataGame.Current;
}

// Data cua game: moi khoi data 1 class, dat trong folder tinh nang dung no
[Serializable]
public class DataGame : DataModule<DataGame>
{
    public double Money;
}

// Dung o bat cu dau
DataSave.Instance.DataGame.Money += 10;
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

> Tai lieu day du (cach hoat dong, quy tac, loi hay gap, chuyen tu ban cu): [Documentation~/Data.md](Documentation~/Data.md)

`SaveBase<T> : Singleton<T>` (game ke thua 1 class `DataSave` lam muc luc property) + `DataModule<T>`:
moi khoi data la 1 class, tu luu JSON (Newtonsoft) vao PlayerPrefs bang key rieng `data_<ten_snake>`.

- `DataSave` tu load trong `Awake`, DontDestroyOnLoad, tu save khi pause / mat focus / thoat.
- API: `DataSave.Instance.SaveGame()` (luu moi module da load), `LoadGame()`, `NewGame()` (xoa het roi tao moi), `DeleteSave()`.
- Can xu ly them: dang ky event static `DataSave.OnLoaded`, `OnSaved`, `OnNewGame`, `OnDeleted` (data thuong khong can).

#### Khai bao data moi

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class DataAnimalPen : DataModule<DataAnimalPen>
{
    public int Level;

    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public List<int> Slots = new();

    // Bat buoc: constructor khong tham so (Newtonsoft + tao moi deu dung)

    // Tuy chon: chi chay khi chua co save (gia tri khoi dau khac mac dinh)
    protected override void OnNew() { Level = 1; }

    // Tuy chon: chay sau moi lan load/tao, va field null / bu phan tu cho save cu
    protected override void OnLoaded() { Slots ??= new List<int>(); }
}
```

Dat file ngay trong folder cua tinh nang (vd `_MODES/ANIMAL/.../DataAnimal.cs`). Khong can dang ky o dau ca.

#### Doc / ghi

```csharp
DataAnimalPen.Current.Level++;          // lan dau truy cap se load tu PlayerPrefs (hoac tao moi)
DataSave.Instance.SaveGame();           // luu ngay (khong bat buoc: tu luu khi pause/thoat)
```

- Chi module **da truy cap** moi duoc luu lai; module chua dung thi save cu giu nguyen.
- `NewGame()` / `DeleteSave()` xoa key cua **moi** `DataModule` trong project (ke ca chua load) roi tha bo nho;
  lan truy cap sau tao moi. Khong giu tham chieu `var d = X.Current` qua `NewGame()`: luon goi lai `X.Current`.

#### Data co san cua package

| Class | Truong |
|---|---|
| `DataSetting` | `Music`, `Sound`, `Vibrate`, `Sensitivity` (JSON key `Sensivity`), `Distance`. Duoc nap san trong `LoadGame` (Audio/Camera doc ngay) |

Truy cap: `DataSave.Instance.DataSetting` (co san trong `SaveBase`) hoac `DataSetting.Current`. Avatar, rate... la data cua game (khai bao DataModule ben game).

#### Luu y khi viet class save

- Key = ten class: **doi ten class la mat save** cua class do. Hai class trung ten (khac namespace) se de key nhau.
- List dung `[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]`, neu khong Newtonsoft se **noi them** vao list tao trong constructor.
- Doi ten field thi giu key cu: `[JsonProperty("TenCu")]`.
- Enum luu bang so: chi them gia tri vao cuoi.
- Bu du lieu cho save cu trong `OnLoaded()`.
- Data dung chung nhieu mode -> dat o tang chung (Common); data rieng 1 mode -> trong folder mode do.

`JsonDataUtility`: `SaveData(key, obj)`, `LoadData<T>(key)` (tra `default` neu chua co key), `DeleteData(key)` — dung truc tiep khi can key tu dat.

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
| `StateMachine` | `new StateMachine(aiBehaviour)`; `AddTransition(t)`, `AddState(s)`, `ToState<T>()`, `ToState(s)`, `ToPrevState()`, `GetState<T>()`, `IsInState<T>()`, `CurrentState`, `PrevState`, `DefaultState`, event `OnStateChanged(prev, next)` |
| `Transition` | `new Transition(from, to)` hoac `new Transition(from, to, check)`; `from = null` la Any State. Event `OnCheck` (tu chuyen khi tra true), `OnTransition` (tra false de chan) |
| `AIBehaviour` | MonoBehaviour chu FSM. Implement `Init()` (goi trong `Awake`, phai tao `StateMachine`); `AddTransition(from, to[, check])`, `AddAnyTransition(to, check)`, `AddTransitionsTo(target, froms...)`, `AddState(s)`, `ToState<T>()`; tu goi update cua FSM |

```csharp
public class StateEnemyIdle : State
{
    public StateEnemyIdle(IStateMachine sm) : base(sm) { }
    public override void OnEnter(IState prev) { base.OnEnter(prev); /* bat animation idle */ }
}

public class EnemyAnim : AIBehaviour
{
    public float Speed;
    public bool IsDead;

    public override void Init()
    {
        StateMachine = new StateMachine(this);
        var idle = new StateEnemyIdle(StateMachine);
        var run = new StateEnemyRun(StateMachine);
        var dead = new StateEnemyDead(StateMachine);
        StateMachine.DefaultState = idle;                 // OnEnter(null) duoc goi o frame dau
        AddTransition(idle, run, () => Speed > 0.1f);     // tu chuyen
        AddTransition(run, idle, () => Speed <= 0.1f);
        AddAnyTransition(dead, () => IsDead);             // tu moi state
    }
}
```

- Moi frame: `OnUpdate` cua state hien tai chay truoc, sau do kiem tra Any State roi transition cua state hien tai; doi toi da 1 lan/frame.
- Transition khong co `OnCheck` khong tu chuyen, chi dung de dang ky state; doi bang `ToState<T>()`.
- State dang ky theo **kieu (Type)**: moi class state chi co 1 instance trong mot `StateMachine`. Muon 2 state giong nhau thi tao 2 class.
- `ToState<T>()` voi state chua dang ky (qua `AddState`, transition hoac `DefaultState`) se log warning va khong doi.
- `ToState` sang chinh state hien tai bi bo qua (khong goi lai `OnEnter`).

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

Nut hanh dong noi tren object trong the gioi 3D (mua, mo khoa, lay do...). Hien tren object gan player nhat trong tam, moi loai 1 nut.

Package **khong biet ten loai indicator nao**: khoa la `int`, game tu khai bao enum.

#### Setup (1 lan cho moi game)

**1. Khai bao enum loai indicator** (o game, vd `Script/Indicator/IndicatorType.cs`):
```csharp
[IndicatorTypeEnum]
public enum IndicatorType
{
    None = 0,          // 0 luon la "khong co"
    UnlockDoor = 1,
    BuyItem = 2,
}
```
- Gan **so co dinh** cho tung gia tri. Prefab/scene luu so, nen **khong doi so da dung**; them moi lay so tiep theo; xoa thi bo dong, khong dung lai so.
- Doi ten gia tri thoai mai (du lieu khong luu ten).
- Moi project chi 1 enum co `[IndicatorTypeEnum]` (drawer lay enum dau tien tim thay).

**2. Extension de goi bang enum** (cung file, tuy chon nhung nen co):
```csharp
public static class IndicatorTypeExtensions
{
    public static void SetIndicator(this IndicatorTarget target, IndicatorType type, int distance, Vector2 offset = default)
        => target.SetIndicator((int)type, distance, offset);
    public static void SetIndicatorType(this IndicatorTarget target, IndicatorType type) => target.SetIndicatorType((int)type);
    public static void SetShowState(IndicatorType type, bool state) => IndicatorManager.SetShowState((int)type, state);
}
```

**3. `IndicatorConfig`**: `Create > Indicator > IndicatorConfig`, dat ten `IndicatorConfig` trong 1 folder `Resources/` (load bang `Resources.Load` khi khoi dong).
- `IsEnabled`: tat toan bo indicator.
- `AutoClickTime`: giay giu truoc khi tu bam (che do `Auto`).
- `Indicators`: moi dong `{ Type, Prefab, Show }`, chon `Type` tu dropdown.

**4. `IndicatorController`**: dat 1 object co component nay trong Canvas HUD (cho phep tat san). Indicator duoc spawn lam con cua no.

**5. Player** implement `IPlayer` va dang ky vao `PlayerBootstrap` (controller lay vi tri player de tinh khoang cach).

#### Them 1 loai indicator moi

1. Them gia tri vao enum: `OpenChest = 3`.
2. Tao prefab UI (Button + `Image` vong tron, `hand`, `txt`, `txtTitle`), gan component `Indicator` hoac lop con, chon `Type = OpenChest`, chon `clickType`.
3. Them dong vao `IndicatorConfig`: `Type = OpenChest`, `Prefab` vua tao.
4. Tren object 3D: them `IndicatorTarget`, chon `Type = OpenChest`, chinh `DistanceDisplay` (ban kinh hien, met) va `Offset` (lech theo pixel man hinh).

Gan type luc runtime (object pool, type doi theo trang thai):
```csharp
indicatorTarget.SetIndicator(IndicatorType.OpenChest, distance: 4, offset: new Vector2(0, 50));
indicatorTarget.SetIndicatorType(IndicatorType.None); // an + huy dang ky
```

#### Viet logic cho nut

Ke thua `Indicator`, lay object dich qua `ObjectIndicator`:
```csharp
public class IndicatorUnlockDoor : Indicator
{
    public override void Refresh()          // goi khi hien va sau moi lan bam
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

`clickType`: `Click` bam 1 lan; `Hold` giu cho day vong `circleImage`; `Auto` dung yen trong tam `AutoClickTime` giay thi tu bam.

#### Bat / tat theo vung hoac tutorial

- `TriggerIndicator` (collider trigger): player vao vung thi hien loai do, ra thi an.
- Code: `IndicatorTypeExtensions.SetShowState(IndicatorType.BuyItem, false)`. Loai chua tung set mac dinh **duoc hien**.

#### API

| Type | API |
|---|---|
| `Indicator` | `Type` (int), `clickType`, `hand`, `txt`, `txtTitle`, `ObjectIndicator`; virtual `Refresh()`, `Click()`, `SetText`, `SetTitle`, `Configure(target)`; `Activate`, `ActiveHand`, `StartAuto`, `StopAuto` |
| `IndicatorTarget` | `Type` (int), `DistanceDisplay`, `Offset`; `SetIndicator(int, distance, offset)`, `SetIndicatorType(int)` |
| `IndicatorManager` (static) | `SetShowState(int, bool)`, `CanShow(int)`, `GetIndicator(int)`, `RefreshAllActive()` |
| `IndicatorConfig` (SO) | `IsEnabled`, `AutoClickTime`, `Indicators {Type, Prefab, Show}`; `GetPrefab(int)`, `CanShow(int)` |
| `TriggerIndicator` | Hien / an 1 loai khi player o trong vung |
| `[IndicatorType]` | Gan len field `int` -> dropdown theo enum `[IndicatorTypeEnum]` |

#### Loi hay gap

- Dropdown hien `(no [IndicatorTypeEnum])`: game chua co enum gan attribute.
- Field hien `(?)` + o so: so dang luu khong co trong enum (gia tri da bi xoa/doi so).
- Indicator khong hien: kiem tra `Type != 0`, co dong trong `IndicatorConfig` voi `Prefab`, `IsEnabled`, `SetShowState` chua tat, player da dang ky `IPlayer`, `DistanceDisplay` du lon, co `IndicatorController` trong scene.

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
| Data rieng cua game | class ke thua `DataModule<T>` + 1 property trong `DataSave : SaveBase<DataSave>` |
| Setting am thanh, rung, do nhay, camera | Doc/ghi `DataSetting.Current` |
| Panel mo dau / luon tren cung | `PanelRole` tren Inspector: `ShowOnStart` / `AlwaysOnTop` |
| Vi tri player | Implement `IPlayer`, `PlayerBootstrap.Register<IPlayer>(this)` |
| Nut HUD | `ButtonXxx : InputButton` |
| Nut hanh dong tren world | `IndicatorXxx : Indicator` + gia tri moi (so tiep theo) trong enum `[IndicatorTypeEnum]` cua game |
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
- Danh so theo SemVer `MAJOR.MINOR.PATCH`:

  | Tang | Khi nao | Vi du | Tieu de CHANGELOG |
  |---|---|---|---|
  | PATCH | Chi sua loi, khong doi API | 2.1.0 -> 2.1.1 | `## [2.1.1] - ngay` |
  | MINOR | Them tinh nang, game dang dung nang len **khong phai sua gi** | 2.1.0 -> 2.2.0 | `## [2.2.0] - ngay` |
  | MAJOR | Game nang len **se loi compile hoac mat data** (bo/doi ten API, doi key save, doi hanh vi) | 2.1.0 -> 3.0.0 | `# [3.0.0] - ngay` + khung `[!IMPORTANT]` (co gi moi) va `[!WARNING]` (breaking, cach chuyen) |

  Ban MAJOR: cap nhat them `release` (`type: major`, `breakingChanges: true`, `generation`) trong `package.json`
  va muc "Co gi moi" o dau tai lieu nay.
- Doi ten assembly runtime: giu nguyen file `.meta` (GUID) de asmdef Editor khong gay.

Lich su thay doi: xem [CHANGELOG.md](CHANGELOG.md).
