# Data / Save — cach hoat dong va cach dung

He thong save cua package `com.mjgame.nmd.nkl.20` (tu ban 3.0.0). Doc nhanh: [Cach dung](#cach-dung). Can hieu ben trong: [Cach hoat dong](#cach-hoat-dong).

---

## Tom tat

- **Moi khoi data la 1 class** ke thua `DataModule<T>`. Dat file ngay trong folder tinh nang dung no.
- Truy cap: `DataSave.Instance.X` (property trong class `DataSave` cua game) hoac thang `X.Current`. Lan dau truy cap se tu load (hoac tao moi neu chua co save).
- **`DataSave : SaveBase<DataSave>`** (1 class cua game, gan tren prefab bootstrap) tu luu moi module khi pause / mat focus / thoat.
- Khong co danh sach dang ky, khong can override gi. Class `DataSave` cua game chi la **muc luc** property.

```csharp
[Serializable]
public class DataWallet : DataModule<DataWallet>
{
    public double Money;
}

// them 1 dong vao DataSave cua game
public DataWallet DataWallet => DataWallet.Current;

DataSave.Instance.DataWallet.Money += 100;   // xong. Tu luu.
```

---

## Cac file

| File (`Runtime/Data/`) | Vai tro |
|---|---|
| `DataModule.cs` | `DataModule<T>` (base cua moi data), `IDataModule`, `DataModules` (so cai: SaveAll / DeleteAll) |
| `SaveBase.cs` | `SaveBase<T> : Singleton<T>`: vong doi save (Awake load, save khi pause/thoat), API `SaveGame/LoadGame/NewGame/DeleteSave`, event |
| `DataSerializable.cs` | `DataSetting` (DataModule) — data duy nhat package giu, vi Audio/Camera/Vibration cua package doc no |
| `JsonData.cs` | `JsonDataUtility`: `SaveData/LoadData/DeleteData` — JSON (Newtonsoft) vao PlayerPrefs |

---

## Cach dung

### 1. Setup (1 lan cho moi game)

Game viet **1 class** `DataSave` (dat o tang cao nhat cua game, vd `_ROOT/Script/Data/DataSave.cs`) va gan len 1 prefab
trong bootstrap (vd `Resources/DataSave`):

```csharp
public class DataSave : SaveBase<DataSave>
{
    // CORE
    public DataMap DataMap => DataMap.Current;

    // ANIMAL
    public DataAnimalPen DataAnimalPen => DataAnimalPen.Current;
}
```

`DataSetting` co san trong `SaveBase`, khong can khai bao lai. Data khac (ke ca avatar, rate) la cua game.

**Quy tac: data nam cung tang voi code dung no.** Package chi giu data ma chinh code package doc.
Class nay chi la muc luc: biet het data cua moi tang/mode, nen phai nam o tang cao nhat (tang thap khong tham chieu duoc).

`DataSave` phai ton tai luc chay. Neu chua co object, `DataSave.Instance` (Singleton) se tu tao, nhung nen dat san
trong bootstrap de chac chan `Awake` chay som (nap `DataSetting` cho Audio/Camera).

### 2. Khai bao data moi

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class DataAnimalPen : DataModule<DataAnimalPen>
{
    public int Level;
    public long LastIncomeTicks;

    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public List<DataPenSlot> Slots = new();

    // Bat buoc co constructor khong tham so (mac dinh da co neu khong viet constructor nao)

    // Tuy chon: chi chay khi CHUA co save -> gia tri khoi dau cua nguoi choi moi
    protected override void OnNew()
    {
        Level = 1;
    }

    // Tuy chon: chay sau MOI lan load/tao -> va field null, bu phan tu cho save cu
    protected override void OnLoaded()
    {
        Slots ??= new List<DataPenSlot>();
    }
}
```

Quy tac dat file:

| Data dung boi | Dat o |
|---|---|
| Moi game (setting, avatar, rate) | package (`DataSerializable.cs`) |
| Khung game (map, checkpoint) | `_CORE/Data/` |
| Chung cua dong game (gameplay, treadmill, inventory...) | `_ROOT/Common/...` |
| Rieng 1 mode | folder cua mode (vd `_MODES/ANIMAL/.../DataAnimal.cs`) |

### 3. Doc / ghi

```csharp
var pen = DataSave.Instance.DataAnimalPen;   // = DataAnimalPen.Current, load lan dau neu chua load
pen.Level++;

DataSave.Instance.SaveGame();      // luu ngay (khong bat buoc: tu luu khi pause / mat focus / thoat)
```

Data co san cua package:

| Class | Truong |
|---|---|
| `DataSave.Instance.DataSetting` | `Music`, `Sound`, `Vibrate`, `Sensitivity` (JSON key `Sensivity`), `Distance`, `ResetData()` |

### 4. API cua `DataSave` (ke thua tu `SaveBase`)

| Ham | Lam gi |
|---|---|
| `SaveGame()` | Phat `OnSaved`, roi luu moi module **da load** |
| `LoadGame()` | Nap `DataSetting`, phat `OnLoaded`. Tu goi trong `Awake` |
| `NewGame()` | Xoa save cua **moi** module, nap lai `DataSetting`, phat `OnNewGame`, roi `SaveGame()` |
| `DeleteSave()` | Phat `OnDeleted`, roi xoa save cua **moi** module |

### 5. Xu ly them khi load / save / new game (event)

Event static, dang ky o bat cu dau (khong can override):

```csharp
private void OnEnable()  { DataSave.OnNewGame += HandleNewGame; }
private void OnDisable() { DataSave.OnNewGame -= HandleNewGame; }

private void HandleNewGame()
{
    // vd: dat lai UI, tang thong ke...
}
```

| Event | Phat khi |
|---|---|
| `OnLoaded` | Cuoi `LoadGame()` (sau khi nap `DataSetting`) |
| `OnSaved` | Dau `SaveGame()`, **truoc** khi ghi -> chinh data lan cuoi o day |
| `OnNewGame` | Sau khi xoa het data, truoc `SaveGame()` -> khoi tao them o day |
| `OnDeleted` | Dau `DeleteSave()`, truoc khi xoa |

Event la `static`: nho `-=` khi object bi huy.

### 6. Luu key tu dat (truong hop dac biet)

Can luu ngoai he DataModule (vd file tam, cache): dung thang `JsonDataUtility.SaveData(key, obj)` /
`LoadData<T>(key)` / `DeleteData(key)`. Luu y: `NewGame()` / `DeleteSave()` **khong** xoa key tu dat — tu xoa trong `OnDeleted` / `OnNewGame`.

---

## Cach hoat dong

### Vong doi 1 module

```
truy cap T.Current lan dau
   ├─ co key "data_T" trong PlayerPrefs -> JsonConvert.Deserialize -> T
   └─ chua co                           -> new T() -> OnNew()
   -> OnLoaded()
   -> DataModules.Track(module)   (vao danh sach "da load")
   -> giu trong static field, lan sau tra ve ngay

DataSave.SaveGame()            -> moi module da Track: JsonConvert.Serialize -> PlayerPrefs "data_T"
DataSave.NewGame/DeleteSave()  -> Unload moi module da load (static = null)
                                  + xoa key "data_<TenClass>" cua MOI class DataModule trong project (quet reflection)
                                  -> lan truy cap sau tao moi
```

### Key

- Key = `"data_" + typeof(T).Name`, vd `DataAnimalPen` -> `data_DataAnimalPen`.
- Key lay tu **ten class** (khong co namespace). Hai class trung ten o namespace khac nhau se de len nhau.

### Load luoi (lazy)

- Module chi load khi co code truy cap `Current`. Mode khong chay thi data cua mode do khong bi load, khong ton bo nho.
- `DataSave.LoadGame()` chi nap san `DataSetting` (Audio, Camera doc ngay khi khoi dong). Cac module khac load khi can.
- Thu tu load giua cac module khong quan trong vi moi module doc lap.

### Luu

- `SaveGame()` chi ghi cac module **da load** trong phien choi. Module chua dung -> save cu trong PlayerPrefs giu nguyen (dung, vi khong co gi thay doi).
- Tu goi khi `OnApplicationPause(true)`, `OnApplicationFocus(false)`, `OnApplicationQuit`.
- Moi lan `SaveData` goi `PlayerPrefs.Save()` -> ghi xuong dia. Khong goi `SaveGame()` moi frame.

### Xoa (NewGame / DeleteSave)

- Quet moi assembly tim class khong abstract ke thua `DataModule<>` -> xoa key cua tung class, ke ca module chua load trong phien nay.
- Chi chay khi NewGame / DeleteSave (hiem), chi phi reflection vai ms la chap nhan duoc.

### Tai sao khong can dang ky

- `Current` la static generic: moi `T` co 1 static field rieng -> tu quan ly instance cua minh.
- `DataModules` chi giu danh sach module da load (de save) va tu tim moi module bang reflection (de xoa).
- `SaveBase` khong biet game co nhung data gi -> package doc lap voi game, game/mode them data khong sua package.
- Property trong `DataSave` cua game chi de goi cho quen tay (`DataSave.Instance.X`); quen khai bao thi data **van luu binh thuong**, chi la phai goi `X.Current`.

---

## Luu y khi viet class data

| Viec | Cach dung |
|---|---|
| List / Dictionary | Gan `[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]`. Neu khong, Newtonsoft **noi them** vao list tao san trong constructor -> nhan doi phan tu moi lan load |
| Doi ten field | Giu key JSON cu: `[JsonProperty("TenCu")] public float TenMoi;` |
| Doi ten class | **Mat save** cua class do (doi key). Can giu thi viet migrate: load key cu bang `JsonDataUtility` trong `OnLoaded` |
| Enum | Luu bang so: chi them gia tri vao **cuoi**, khong chen giua, khong xoa |
| Them field moi | Save cu se thieu field -> nhan gia tri mac dinh cua C#. Can gia tri khac -> gan trong `OnLoaded` (kiem tra null / 0) |
| Gia tri khoi dau | Dat trong `OnNew()` (chi nguoi choi moi), khong dat trong constructor neu khong muon ghi de luc load |
| Field khong luu | `[JsonIgnore]` |
| Tham chieu Unity (`GameObject`, `Sprite`...) | Khong luu duoc. Luu id / ten roi tra cuu lai |

---

## Loi hay gap

| Trieu chung | Nguyen nhan / cach sua |
|---|---|
| Sau `NewGame()` data cu van hien | Dang giu tham chieu cu `var d = X.Current` trong field. Luon goi lai `X.Current`, hoac lam moi trong `DataSave.OnNewGame` |
| List bi nhan doi sau moi lan vao game | Thieu `ObjectCreationHandling.Replace` |
| Data cu mat sau khi doi ten class | Key = ten class. Xem muc doi ten class o tren |
| Data khong luu khi tat Play mode trong Editor | Khong co object `DataSave` trong scene (khong co `OnApplicationQuit`). Dat prefab vao bootstrap |
| Gia tri lan dau sai (0 / null) | Thieu khoi tao trong `OnNew()` / `OnLoaded()` |
| Bat Enter Play Mode Options (tat Domain Reload) -> data phien truoc con | Static `Current` khong bi reset. Goi `DataSave.Instance.LoadGame()` khong du; can tat tuy chon do hoac reset static trong `[RuntimeInitializeOnLoadMethod]` |

Xem / xoa save trong Editor: `Edit > Clear All PlayerPrefs`, hoac goi `DataSave.Instance.DeleteSave()`.

---

## Chuyen tu ban cu (`SaveBase<T>`, < 3.0.0)

| Ban cu | Ban moi |
|---|---|
| `public class DataSave : SaveBase<DataSave> { ... }` chua key + load/save | Giu class, xoa het key va code load/save, chi con property `X => X.Current` |
| Property + `const KEY_...` + `LoadData/SaveData/DeleteData` trong `OnLoadGame/OnSaveGame/OnDeleteSave` | Class data ke thua `DataModule<T>`, bo het key va code load/save |
| `OnNewGame() => DataX = new DataX(true)` | `protected override void OnNew()` trong `DataX` |
| `[OnDeserialized]` bu du lieu | `protected override void OnLoaded()` |
| `public DataX DataX { get; private set; }` | `public DataX DataX => DataX.Current;` (cho goi van la `DataSave.Instance.DataX`) |
| `protected override void OnXxxGame()` xu ly them | `DataSave.OnLoaded / OnSaved / OnNewGame / OnDeleted += ...` |

Key moi dang `data_<TenClass>` nen save cu khong doc duoc: nguoi choi cu se nhu nguoi choi moi (tru khi tu viet migrate).
