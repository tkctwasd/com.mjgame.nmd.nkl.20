using System;
using UnityEngine;
using UnityEngine.Events;

public class ServiceAds : BaseModule
{
    public AdsSetting Setting { get; private set; }
    public bool ShowedFirstOpen { get; private set; } = false;
    public bool ShowFirstOpenDone { get; set; } = false;
    public float LastTimeShowInterstitial { get; private set; }

    public bool IsAdsLeaveApp { get; set; }

    protected DateTime lastTimeRefocusShow;

    public bool IsReady { get; private set; } = false;
    public bool IsInitialized { get; private set; } = false;

    static IAdsService adsService;
    public override async void Initialize()
    {
        string timeData = PlayerPrefs.GetString("LastTimeRefocusShow", "");
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(nameof(AdsSetting), "")))
        {
            Setting = new AdsSetting();
        }
        else
        {
            Setting = JsonUtility.FromJson<AdsSetting>(PlayerPrefs.GetString(nameof(AdsSetting), ""));
        }

        await API.Get<ServiceRemoteConfig>().FetchRemoteConfig();
        var remoteSetting = API.Get<ServiceRemoteConfig>().GetValue<string>(nameof(AdsSetting));
        if (!string.IsNullOrEmpty(remoteSetting))
        {
            Setting = JsonUtility.FromJson<AdsSetting>(remoteSetting);
        }
        if (string.IsNullOrEmpty(timeData))
        {
            lastTimeRefocusShow = DateTime.Now.AddDays(-1);
        }
        else
        {
            if (!DateTime.TryParse(timeData, out lastTimeRefocusShow))
            {
                lastTimeRefocusShow = DateTime.Now.AddDays(-1);
            }
        }
        IsInitialized = true;
        TickUpdateManager.OnPause += OnPause;
        TickUpdateManager.OnQuit += OnQuit;
    }

    public bool IsSetConsent()
    {
        if (adsService == null) return true;
        return Consent.IsSetConsent;
    }
    public override void SetInfo() { }
    public override void Dispose()
    {
        TickUpdateManager.OnPause -= OnPause;
        TickUpdateManager.OnQuit -= OnQuit;
    }
    public static void Register(IAdsService _adsService)
    {
        adsService = _adsService;
    }

    private void OnQuit()
    {
        PlayerPrefs.SetString("LastTimeRefocusShow", lastTimeRefocusShow.ToString());
        PlayerPrefs.SetString(nameof(AdsSetting), JsonUtility.ToJson(Setting));
    }
    private void OnPause(bool pause)
    {
        if (!Setting.IsRefocusShowAds)
        {
            return;
        }
        if ((DateTime.Now - lastTimeRefocusShow).TotalSeconds < Setting.IntervalRefocusShow)
        {
            return;
        }
        if (!pause)
        {
            ShowAppOpen(LogEventParameters.APP_UNPAUSE);
            lastTimeRefocusShow = DateTime.Now;
            PlayerPrefs.SetString("LastTimeRefocusShow", lastTimeRefocusShow.ToString());
        }
    }
    public bool CanShowAds()
    {
        return !Setting.IsRemoveAds;
    }

    public void SetReady()
    {
        IsReady = true;
        LastTimeShowInterstitial = Time.time - Setting.IntervalShowInterstitial;
    }
    public void SetRefocus(float interval, bool isShow)
    {
        Setting.IsRefocusShowAds = isShow;
        Setting.IntervalRefocusShow = interval;
    }

#if USE_FACEBOOK
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);
#endif

    public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
    {
#if USE_FACEBOOK
        FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
#endif
    }

    public static void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
    {
#if UNITY_ANDROID && USE_FACEBOOK
            AndroidJavaClass adSettings = new AndroidJavaClass("com.facebook.ads.AdSettings");
            adSettings.CallStatic("setDataProcessingOptions", (object)dataProcessingOptions, country, state);
#endif

#if UNITY_IOS && USE_FACEBOOK
            FBAdSettingsBridgeSetDetailedDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length, country, state);
#endif
    }
#if UNITY_IOS && USE_FACEBOOK
    [DllImport("__Internal")]
    private static extern void FBAdSettingsBridgeSetDataProcessingOptions(string[] dataProcessingOptions, int length);

    [DllImport("__Internal")]
    private static extern void FBAdSettingsBridgeSetDetailedDataProcessingOptions(string[] dataProcessingOptions, int length, int country, int state);
#endif
    #region APP_OPEN
    public void ShowAppOpen(string location, UnityAction callback = null)
    {
        if (adsService == null)
        {
            callback?.Invoke();
            return;
        }
        adsService?.ShowAppOpen(location, callback);
    }
    public bool CanShowAppOpen()
    {
        if (Setting.IsUseAppOpenAd == false) return false;
        if (adsService == null) return false;
        return adsService.CanShowAds();
    }
    public void OnAppOpenLoaded()
    {
        if (!ShowedFirstOpen)
        {
            Debug.Log("Show First Open");
            ShowAppOpen(LogEventParameters.FIRST_OPEN);
            ShowedFirstOpen = true;
        }
    }
    #endregion

    #region BANNER
    int ClickedBanner { get; set; }
    public bool CanRequestBanner() => ClickedBanner < Setting.MaxClickBanner;
    public void ShowBanner()
    {
        adsService?.ShowBanner();
    }

    public void HideBanner()
    {
        adsService?.HideBanner();
    }
    #endregion

    #region RMEC
    public void ShowRMEC()
    {
        if (adsService == null)
        {
            return;
        }
        adsService?.ShowRMEC();
    }
    public void HideRMEC()
    {
        adsService?.HideRMEC();
    }
    #endregion
    #region INTERSTITIAL
    public bool CanShowInterstitial()
    {
        if (Setting.IsShowInterstitial == false) return false;
        if (adsService == null) return false;
        if (Time.time - LastTimeShowInterstitial >= Setting.IntervalShowInterstitial)
        {
            return adsService.CanShowInterstitial();
        }
        return false;
    }
    public void OnShowInterstitial()
    {
        LastTimeShowInterstitial = Time.time;
        IsAdsLeaveApp = true;
    }
    public void ShowInterstitial(string location, UnityAction callback = null)
    {
        if (adsService == null)
        {
            callback?.Invoke();
            return;
        }
        adsService?.ShowInterstitial(location, callback);
    }
    #endregion

    #region REWARDED
    int countRewardedClicked { get; set; }

    public bool CanRequestVideo()
    {
        return countRewardedClicked < Setting.MaxClickRewarded;
    }
    public bool CanShowRewarded()
    {
        if (Setting.IsShowInterstitial == false) return false;
        if (!CanShowAds()) return false;
        if (adsService == null) return false;
        return adsService.CanShowRewarded();
    }
    public void ShowRewarded(string location, UnityAction<bool> callback)
    {
        if (adsService == null)
        {
            callback?.Invoke(true);
            return;
        }
        adsService?.ShowRewarded(location, callback);
    }

    public void OnRewardedAdClicked(string placement)
    {
        countRewardedClicked++;
        if (!string.IsNullOrEmpty(placement))
        {
            API.Get<ServiceLogEvent>().OnRewardedClick(placement);
        }
    }
    #endregion
}