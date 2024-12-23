using System;
using UnityEngine;

public enum Size
{
    Small,
    Medium,
    Big,
    Max,
}

public enum BaseColor
{
    White, Black, Yellow, Red, Blue, Green
}
public enum QuationCircle
{
    XZ, XY, YZ
}
public enum FixedColor
{
    BabyBlue,
    Black,
    Blue,
    CherryRed,
    CloudWhite,
    Cyan,
    DarkGray,
    DustyBlue,
    Gray,
    Green,
    IceWhite,
    Magenta,
    Orange,
    PressedBlue,
    Purple,
    Red,
    Yellow,
    Primary,
    Secondary,
    Success,
    Info,
    Warning,
    Danger,
    Light,
    Teal,
    Pink,
    /// <summary> Color(0, 0, 0, 0) </summary>
    Transparent,
}
public enum LabelStyle
{
    /// <summary>
    /// No label. draws the value over full width
    /// </summary>
    NoLabel,
    /// <summary>
    /// No label, but draws the value only in the value Area
    /// </summary>
    EmptyLabel,
    /// <summary>
    /// Makes the label as small as the label text
    /// </summary>
    NoSpacing,
    /// <summary>
    /// The common: label has default labelwith and value field starts in next column
    /// </summary>
    FullSpacing,
}

public enum DisabledStyle
{
    /// <summary> greys out and forbids editing </summary>
    GreyedOut,
    /// <summary> hides the field completely </summary>
    Invisible
}

public enum InspectorIcon
{
    AddFolder,
    AreaLight,
    Arrow_left,
    Arrow_right,
    AssetLabel,
    Camera,
    Error,
    Warning,
    Wrench,
    Help,
    Menu,
    Settings,
    Account,
    Lock,
    Lightbaking,
    Lightbaking_Off,
    Server_Connected,
    Server_Disabled,
    Server_Disconnected,
    Cross,
    Cloud,
    Picker,
    Info_Console,
    Info_Inactive,
    Script,
    CustomSorting,
    Debug,
    DebuggerAttached,
    DebuggerDisabled,
    DebuggerEnabled,
    DefaultAsset,
    Edit_Collision,
    Edit_Constraints,
    Exposure,
    Favorite,
    Filter_By_Label,
    Filter_By_Type,
    Folder,
    Folder_Favorite,
    Folder_Opened,
    Frame_Capture,
    GameObject,
    Gizmos,
    GridAndSnap,
    GridAxisY,
    GridAxisY_On,
    Lighting,
    Linked,
    Unlinked,
    Material,
    Mesh,
    More,
    MoreOptions,
    Move,
    Minus,
    Minus_Filled,
    Minus_Act,
    Plus,
    Plus_Filled,
    Plus_Act,
    Orientation,
    PackageManager,
    Pause,
    Pick,
    Play,
    Loop,
    Prefab,
    Context,
    Ticked,
    Rect,
    Rotate,
    Scale,
    Scene,
    pickable,
    Not_Pickable,
    Pickable_Mixed,
    Not_Pickable_Mixed,
    _2D,
    Sound,
    Sound_Off,
    Sound_Muted,
    SceneCamera,
    SceneView,
    Lighting_Off,
    Snap,
    ScriptableObject,
    Search,
    Search_Menu,
    Search_Jump,
    Shaded,
    ShadedWireframe,
    Shortcut,
    SnapIncrement,
    Step,
    TextAsset,
    Center,
    Global,
    Local,
    Pivot,
    ToolSettings,
    Transform,
    Undo,
    Console,
    Game,
    Info,
    Hierarchy,
    ViewOptions,
    View_Move,
    Eye,
    Zoom,
    Loading,
    Wireframe,
    Light,
    Download,
    Lock_Opened,
    Ticked_Green,
    Lens_Flare,
    Light_Probe,
    Package,
    Particle_System,
    Force_Field,
    PointLight,
    Reflection_Probe,
    Refresh,
    SpotLight,
    Unity,
    Update,
    Visual_Effect,
    WindZone
}
public static class StylesConvert
{
    public static Color ToColor(this FixedColor c)
    {
        return c switch
        {
            FixedColor.CloudWhite => new Color(.93f, .93f, .93f, 1),
            FixedColor.IceWhite => Color.white,
            FixedColor.Black => Color.black,
            FixedColor.Gray => new Color(0.42f, 0.46f, 0.49f),
            FixedColor.DarkGray => new Color(0.2f, 0.23f, 0.25f),
            FixedColor.Blue => Color.blue,
            FixedColor.PressedBlue => new Color(.27f, .38f, .49f) * 2.5f,
            FixedColor.BabyBlue => new Color(.73f, .89f, .96f, 1),
            FixedColor.DustyBlue => new Color(.31f, .4f, .5f, 1),
            FixedColor.Purple => new Color(0.44f, 0.26f, 0.76f),
            FixedColor.Red => new Color(0.86f, 0.21f, 0.27f),
            FixedColor.CherryRed => new Color(.8f, 0, .1f, 1),
            FixedColor.Orange => new Color(.95f, .55f, .09f, 1),
            FixedColor.Cyan => new Color(0.09f, 0.64f, 0.72f),
            FixedColor.Green => new Color(0.16f, 0.65f, 0.27f),
            FixedColor.Magenta => Color.magenta,
            FixedColor.Yellow => new Color(1, 0.76f, 0.03f),
            FixedColor.Transparent => new Color(0f, 0f, 0f, 0f),
            FixedColor.Pink => new Color(0.91f, 0.24f, 0.55f),
            FixedColor.Teal => new Color(0.13f, 0.79f, 0.59f),
            FixedColor.Primary => new Color(0, 0.48f, 1),
            FixedColor.Secondary => new Color(0.42f, 0.46f, 0.49f),
            FixedColor.Success => new Color(0.16f, 0.65f, 0.27f),
            FixedColor.Info => new Color(0.09f, 0.64f, 0.72f),
            FixedColor.Warning => new Color(1, 0.76f, 0.03f),
            FixedColor.Danger => new Color(0.86f, 0.21f, 0.27f),
            FixedColor.Light => new Color(0.97f, 0.98f, 0.98f),
            _ => throw new System.NotImplementedException($"{c} currently not supported")
        };
    }

    public static string ToHex(this FixedColor fixedColor)
    {
        Color color = fixedColor.ToColor();
        color.r = Mathf.Clamp01(color.r);
        color.g = Mathf.Clamp01(color.g);
        color.b = Mathf.Clamp01(color.b);

        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }
    public static TextAnchor AlignmentToAnchor(TextAlignment textAlignment)
    {
        return textAlignment switch
        {
            TextAlignment.Left => TextAnchor.UpperLeft,
            TextAlignment.Center => TextAnchor.UpperCenter,
            TextAlignment.Right => TextAnchor.UpperRight,
            _ => throw new NotImplementedException(textAlignment.ToString()),
        };
    }
    public static LabelStyle ToInteralStyle(this LabelStyle labelStyle)
    {
        return labelStyle switch
        {
            LabelStyle.NoLabel => LabelStyle.NoLabel,
            LabelStyle.EmptyLabel => LabelStyle.EmptyLabel,
            LabelStyle.NoSpacing => LabelStyle.NoSpacing,
            LabelStyle.FullSpacing => LabelStyle.FullSpacing,
            _ => throw new NotImplementedException($"{labelStyle} not found"),
        };
    }
    public static string ToInternalIconName(this InspectorIcon icon)
        => ToInternalIconName(icon.ToString());
    public static string ToInternalIconName(string iconName)
    {
        return iconName switch
        {
            nameof(InspectorIcon.AddFolder) => "Add-Available",
            nameof(InspectorIcon.AreaLight) => "AreaLight Gizmo",
            nameof(InspectorIcon.Arrow_left) => "ArrowNavigationLeft",
            nameof(InspectorIcon.Arrow_right) => "ArrowNavigationRight",
            nameof(InspectorIcon.AssetLabel) => "AssetLabelIcon",
            nameof(InspectorIcon.Camera) => "Camera Gizmo",
            nameof(InspectorIcon.Error) => "console.erroricon.sml",
            nameof(InspectorIcon.Warning) => "console.warnicon.sml",
            nameof(InspectorIcon.Wrench) => "Customized",
            nameof(InspectorIcon.Help) => "d__Help",
            nameof(InspectorIcon.Menu) => "d__Menu",
            nameof(InspectorIcon.Settings) => "d_Settings",
            nameof(InspectorIcon.Account) => "d_account",
            nameof(InspectorIcon.Lock) => "d_AssemblyLock",
            nameof(InspectorIcon.Lightbaking) => "d_AutoLightbakingOn",
            nameof(InspectorIcon.Lightbaking_Off) => "d_AutoLightbakingOff",
            nameof(InspectorIcon.Server_Connected) => "d_CacheServerConnected",
            nameof(InspectorIcon.Server_Disabled) => "d_CacheServerDisabled",
            nameof(InspectorIcon.Server_Disconnected) => "d_CacheServerDisconnected",
            nameof(InspectorIcon.Cross) => "d_clear",
            nameof(InspectorIcon.Cloud) => "d_CloudConnect",
            nameof(InspectorIcon.Picker) => "d_color_picker",
            nameof(InspectorIcon.Script) => "d_cs Script Icon",
            nameof(InspectorIcon.CustomSorting) => "d_CustomSorting",
            nameof(InspectorIcon.Debug) => "d_debug",
            nameof(InspectorIcon.DebuggerAttached) => "d_DebuggerAttached",
            nameof(InspectorIcon.DebuggerDisabled) => "d_DebuggerDisabled",
            nameof(InspectorIcon.DebuggerEnabled) => "d_DebuggerEnabled",
            nameof(InspectorIcon.DefaultAsset) => "d_DefaultAsset Icon",
            nameof(InspectorIcon.Edit_Collision) => "d_editcollision_16",
            nameof(InspectorIcon.Edit_Constraints) => "d_editconstraints_16",
            nameof(InspectorIcon.Exposure) => "d_Exposure",
            nameof(InspectorIcon.Favorite) => "d_Favorite",
            nameof(InspectorIcon.Filter_By_Label) => "d_FilterByLabel",
            nameof(InspectorIcon.Filter_By_Type) => "d_FilterByType",
            nameof(InspectorIcon.Folder) => "d_Folder Icon",
            nameof(InspectorIcon.Folder_Favorite) => "d_FolderFavorite Icon",
            nameof(InspectorIcon.Folder_Opened) => "d_FolderOpened Icon",
            nameof(InspectorIcon.Frame_Capture) => "d_FrameCapture",
            nameof(InspectorIcon.GameObject) => "d_GameObject Icon",
            nameof(InspectorIcon.Gizmos) => "d_GizmosToggle",
            nameof(InspectorIcon.GridAndSnap) => "d_GridAndSnap",
            nameof(InspectorIcon.GridAxisY) => "d_GridAxisY",
            nameof(InspectorIcon.GridAxisY_On) => "d_GridAxisY On",
            nameof(InspectorIcon.Lighting) => "d_Lighting",
            nameof(InspectorIcon.Linked) => "d_Linked",
            nameof(InspectorIcon.Unlinked) => "d_Unlinked",
            nameof(InspectorIcon.Material) => "d_Material Icon",
            nameof(InspectorIcon.Mesh) => "d_Mesh Icon",
            nameof(InspectorIcon.More) => "d_more",
            nameof(InspectorIcon.MoreOptions) => "d_MoreOptions",
            nameof(InspectorIcon.Move) => "d_MoveTool on",
            nameof(InspectorIcon.Minus) => "d_Toolbar Minus",
            nameof(InspectorIcon.Minus_Filled) => "d_ol_minus",
            nameof(InspectorIcon.Minus_Act) => "d_ol_minus_act",
            nameof(InspectorIcon.Plus) => "d_Toolbar Plus",
            nameof(InspectorIcon.Plus_Filled) => "d_ol_plus",
            nameof(InspectorIcon.Plus_Act) => "d_ol_plus_act",
            nameof(InspectorIcon.Orientation) => "d_OrientationGizmo",
            nameof(InspectorIcon.PackageManager) => "d_Package Manager",
            nameof(InspectorIcon.Pause) => "d_PauseButton On",
            nameof(InspectorIcon.Pick) => "d_pick",
            nameof(InspectorIcon.Play) => "d_PlayButton On",
            nameof(InspectorIcon.Loop) => "d_preAudioLoopOff",
            nameof(InspectorIcon.Prefab) => "d_Prefab Icon",
            nameof(InspectorIcon.Context) => "d_Preset.Context",
            nameof(InspectorIcon.Ticked) => "d_Progress",
            nameof(InspectorIcon.Rect) => "d_RectTool On",
            nameof(InspectorIcon.Rotate) => "d_RotateTool On",
            nameof(InspectorIcon.Scale) => "d_ScaleTool On",
            nameof(InspectorIcon.Scene) => "d_Scene",
            nameof(InspectorIcon.pickable) => "d_scenepicking_pickable_hover",
            nameof(InspectorIcon.Not_Pickable) => "d_scenepicking_notpickable_hover",
            nameof(InspectorIcon.Pickable_Mixed) => "d_scenepicking_pickable-mixed_hover",
            nameof(InspectorIcon.Not_Pickable_Mixed) => "d_scenepicking_notpickable-mixed_hover",
            nameof(InspectorIcon._2D) => "d_SceneView2D On",
            nameof(InspectorIcon.Sound) => "d_SceneViewAudio On",
            nameof(InspectorIcon.Sound_Off) => "AudioSource Gizmo",
            nameof(InspectorIcon.Sound_Muted) => "d_SceneViewAudio",
            nameof(InspectorIcon.SceneView) => "d_SceneViewCamera",
            nameof(InspectorIcon.Lighting_Off) => "d_SceneViewLighting",
            nameof(InspectorIcon.Snap) => "d_SceneViewSnap",
            nameof(InspectorIcon.ScriptableObject) => "d_ScriptableObject Icon",
            nameof(InspectorIcon.Search) => "d_search_icon",
            nameof(InspectorIcon.Search_Menu) => "d_search_menu",
            nameof(InspectorIcon.Search_Jump) => "d_SearchJump Icon",
            nameof(InspectorIcon.Shaded) => "d_Shaded",
            nameof(InspectorIcon.ShadedWireframe) => "d_ShadedWireframe",
            nameof(InspectorIcon.Shortcut) => "d_Shortcut Icon",
            nameof(InspectorIcon.SnapIncrement) => "d_SnapIncrement",
            nameof(InspectorIcon.Step) => "d_StepButton",
            nameof(InspectorIcon.TextAsset) => "d_TextAsset Icon",
            nameof(InspectorIcon.Center) => "d_ToolHandleCenter",
            nameof(InspectorIcon.Global) => "d_ToolHandleGlobal",
            nameof(InspectorIcon.Local) => "d_ToolHandleLocal",
            nameof(InspectorIcon.Pivot) => "d_ToolHandlePivot",
            nameof(InspectorIcon.ToolSettings) => "d_ToolSettings",
            nameof(InspectorIcon.Transform) => "d_Transform Icon",
            nameof(InspectorIcon.Undo) => "d_UndoHistory",
            nameof(InspectorIcon.Console) => "d_UnityEditor.ConsoleWindow",
            nameof(InspectorIcon.Game) => "d_UnityEditor.GameView",
            nameof(InspectorIcon.Info) => "d_UnityEditor.InspectorWindow",
            nameof(InspectorIcon.Hierarchy) => "d_UnityEditor.SceneHierarchyWindow",
            nameof(InspectorIcon.ViewOptions) => "d_ViewOptions",
            nameof(InspectorIcon.View_Move) => "d_ViewToolMove On",
            nameof(InspectorIcon.Eye) => "d_scenevis_visible_hover",
            nameof(InspectorIcon.Zoom) => "d_ViewToolZoom On",
            nameof(InspectorIcon.Loading) => "d_WaitSpin02",
            nameof(InspectorIcon.Wireframe) => "d_wireframe",
            nameof(InspectorIcon.Light) => "DiscLight Gizmo",
            nameof(InspectorIcon.Download) => "Download-Available",
            nameof(InspectorIcon.Ticked_Green) => "Installed",
            nameof(InspectorIcon.Lens_Flare) => "LensFlare Gizmo",
            nameof(InspectorIcon.Light_Probe) => "LightProbeGroup Gizmo",
            nameof(InspectorIcon.Package) => "package_installed",
            nameof(InspectorIcon.Particle_System) => "ParticleSystem Gizmo",
            nameof(InspectorIcon.Force_Field) => "ParticleSystemForceField Gizmo",
            nameof(InspectorIcon.PointLight) => "PointLight Gizmo",
            nameof(InspectorIcon.Reflection_Probe) => "ReflectionProbe Gizmo",
            nameof(InspectorIcon.Refresh) => "Refresh",
            nameof(InspectorIcon.SpotLight) => "SpotLight Gizmo",
            nameof(InspectorIcon.Unity) => "d_Scene",
            nameof(InspectorIcon.Update) => "Update-Available",
            nameof(InspectorIcon.Visual_Effect) => "VisualEffect Gizmo",
            nameof(InspectorIcon.WindZone) => "WindZone Gizmo",
            _ => iconName,
        };
    }

    public static float ToButtonHeight(Size size)
    {
        return size switch
        {
            Size.Small => 17,
            Size.Medium => 30,
            Size.Big => 40,
            Size.Max => 60,

            _ => throw new System.NotImplementedException(size.ToString()),
        };
    }
    public static float ToButtonWidth(float availableWidth, GUIContent buttonLabel, Size size)
    {
        return Mathf.Min(availableWidth, Math.Max(GUI.skin.label.CalcSize(buttonLabel).x + 20, size switch
        {
            Size.Small => 100,
            Size.Medium => 50 + availableWidth / 4,
            Size.Big => 50 + availableWidth / 2,
            Size.Max => float.MaxValue,
            _ => throw new System.NotImplementedException(size.ToString()),
        }));
    }
}


public static class GameColor
{

    public static Color Blue => new Color(0, 0.48f, 1);
    public static Color Indigo => new Color(0.4f, 0.06f, 0.95f);
    public static Color Purple => new Color(0.44f, 0.26f, 0.76f);
    public static Color Pink => new Color(0.91f, 0.24f, 0.55f);
    public static Color Red => new Color(0.86f, 0.21f, 0.27f);
    public static Color Orange => new Color(0.99f, 0.49f, 0.08f);
    public static Color Yellow => new Color(1, 0.76f, 0.03f);
    public static Color Green => new Color(0.16f, 0.65f, 0.27f);
    public static Color Teal => new Color(0.13f, 0.79f, 0.59f);
    public static Color Cyan => new Color(0.09f, 0.64f, 0.72f);
    public static Color White => Color.white;
    public static Color Gray => new Color(0.42f, 0.46f, 0.49f);
    public static Color GrayDark => new Color(0.2f, 0.23f, 0.25f);
    public static Color Primary => new Color(0, 0.48f, 1);
    public static Color Secondary => new Color(0.42f, 0.46f, 0.49f);
    public static Color Success => new Color(0.16f, 0.65f, 0.27f);
    public static Color Info => new Color(0.09f, 0.64f, 0.72f);
    public static Color Warning => new Color(1, 0.76f, 0.03f);
    public static Color Danger => new Color(0.86f, 0.21f, 0.27f);
    public static Color Light => new Color(0.97f, 0.98f, 0.98f);
    public static Color Dark => new Color(0.2f, 0.23f, 0.25f);
}