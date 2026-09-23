# _mindongun — Khung dung chung (framework)

Tach tu Steal an Egg thang 9/2026. Khung **tu bien dich duoc thanh mot assembly rieng**
(da build thu: khung 82 file runtime + 10 editor, 0 loi; game tham chieu toi khung cung 0 loi),
tuc la san sang dong thanh package Unity.

Khung **khong goi toi bat ky class nao cua game**. Moi thu khung can tu game deu di qua
interface hoac vai tro, xem muc "Cach game cam vao khung".

## Co gi trong khung

| Folder | Noi dung |
|---|---|
| `Core` | `CoreConsts`, `CoreConverter`, `CoreSettings` (+`ICoreSettings`), `CoreRemoteConfig` (+`ICoreRemoteConfig`), `IChargeSource`, `IPlayerRoot` |
| `Utility` | `Singleton<T>`, `CoroutineHelper`, `LogMessage` |
| `Extension` | `UnityExtensions` (`ActiveObj`, `AddResizeEffect`, `AddDelayedReveal`, tween UI...) |
| `Attribute` | `[Button]`, `[ReadOnly]`, `[ColorLabel]`, `[ShowIf]` |
| `Pool` | `PoolManager`, `ObjectPool`, `IPoolable`, `PoolableObject` |
| `FSM` | `State`, `StateMachine`, `Transition`, `AIBehaviour` |
| `Bootstrap` | `BootstrapScene` (load prefab tu Resources), `PlayerBootstrap` (registry) |
| `Data` | `JsonData` (`JsonDataUtility`) |
| `UI` | `UIManager`, `UIPanel`, `UIItem`/`UIReusableList`, `UIProgress`, `LoadingOverlay`, `IPanelRoles`, `UI/Utility` (animation), `UI/Editor`, `UI/Input` (`InputButton`, `FixedTouchField`, touch raycast), `LoopText`, `UIScroll` |
| `Audio` | `AudioManager` (static), `AudioConfig`, `Sound3D` |
| `Camera` | `MainCamController`, `CameraShake`, `CameraZoom` |
| `Joystick` | `Joystick`, `FloatingJoystick` |
| `Indicator` | `Indicator`, `IndicatorTarget` (+`IndicatorType`), `IndicatorConfig`, `IndicatorManager`, `IndicatorController`, `TriggerIndicator` |
| `Interaction` | `ItemsTriggerBase`, `ItemsAction`, `ItemsInteract`, `IItems`, `Door*` |
| `Vibration`, `DrawMesh`, `DebugGUI` | Rung, ve duong hint, FPS counter |
| `Editor` | Tool editor: PlayerPrefs window, RemoveMissingScripts, ReplaceTexture, SO → JSON |

**Khong nam trong khung:** Player, Bot, Enemy, Map, Controller, Manager, Tutorial, Obstacle,
SO cua game, Analytic, `_Monetization` (Ads/IAP/RemoteConfig). Nhung phan do con goi thang
class cua game nen tiep tuc chep tu project gan nhat.

## Thu vien can co

DOTween, TextMeshPro, Newtonsoft Json (`com.unity.nuget.newtonsoft-json`), Cinemachine 2.x.
Khong can MaxSdk/Firebase/Adjust nua vi monetization nam ben game.
Neu dong package: DOTween phai bam **Create ASMDEF** trong DOTween Utility Panel.

## Cach game cam vao khung

Khung khong co file "hook" nao het. Game gan vai tro vao chinh class cua minh:

| Khung can gi | Game lam gi |
|---|---|
| Panel mo dau | Panel do implement `IPanelFirstShow` (vd: `PanelLoading`) |
| Panel luon nam tren cung | Panel do implement `IPanelAlwaysOnTop` (vd: `PanelNotice`) |
| Panel che man hinh khi quay lai tu App Open Ad | Panel do implement `IPanelResume` (vd: `PanelResume`) |
| Setting am thanh, rung, khoang cach camera | Class setting cua game implement `ICoreSettings`, gan vao `CoreSettings.Current` (vd: `DataSetting`, `DataSave` gan luc load) |
| Cau hinh tu xa (icon quang cao, thoi gian auto) | Class remote config cua game implement `ICoreRemoteConfig`, gan vao `CoreRemoteConfig.Current` trong `Awake` |
| Nguon "giu de sac" cho camera rung/zoom | Nut cua game implement `IChargeSource`, goi `ChargeSource.Register(this)` trong `OnEnable` |
| Vi tri player (indicator) | Class player implement `IPlayerRoot`, goi `PlayerBootstrap.Register<IPlayerRoot>(this)` |
| Bot/enemy di vao vung trigger | Game tao `ItemsTrigger : ItemsTriggerBase`, override `OnOtherEnter/OnOtherExit` |

Panel cua khung tim nhau bang `UIManager.Instance.Find<T>()` (co cache).
Joystick va `FixedTouchField` tu giu object huong dan cua minh (`tutHint`), khong hoi UI.

## Dung o project khac

Khung da co san asmdef (`Mindongun`, `Mindongun.Editor`, `Mindongun.UI.Editor`) va `package.json`
(`com.mindongun.core`), nen dung duoc theo 2 kieu:

**1. Chep thang vao Assets** (nhanh, sua truc tiep duoc)
- Chep ca folder nay vao `Assets/` cua project moi.
- `package.json` nam trong `Assets` thi Unity bo qua, khong sao ca.

**2. Dung nhu package** (tach han khoi game)
- Dong Unity, chuyen folder nay ra `Packages/com.mindongun.core`, mo lai Unity.
- Hoac day folder nay len git rieng roi cai bang URL trong Package Manager.
- GUID cua file khong doi khi chuyen, nen prefab va scene khong mat tham chieu.

**Ca 2 kieu deu can DOTween** (khong co tren UPM):
import DOTween vao project, mo `Tools > Demigiant > DOTween Utility Panel`,
bam nut o muc ASMDEF de sinh `DOTween.Modules.asmdef`. Thieu buoc nay thi khung khong bien dich duoc.

Cac package con lai (uGUI, TextMeshPro, Cinemachine, Newtonsoft Json) da khai bao trong
`package.json`, Unity tu cai khi dung kieu 2; dung kieu 1 thi tu them vao `Packages/manifest.json`.

## Viet phan game

Gan cac vai tro (`PanelRole`, `IPlayer`, `ICoreSettings`...) vao panel va class tuong ung cua game.
Khong sua file trong khung de chieu mot game cu the — neu thieu cho cam, them mot vai tro moi vao khung.
