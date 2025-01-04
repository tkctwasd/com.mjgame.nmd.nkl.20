using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAdsService 
{
    void Initialize();
    bool IsSetConsent();
    bool CanShowAds();
    void ShowAppOpen(string location, UnityAction callback);
    void ShowBanner();
    void ShowRMEC();
    void HideRMEC();
    void HideBanner();
    bool CanShowInterstitial();
    void ShowInterstitial(string location, UnityAction callback);
    bool CanShowRewarded();
    void ShowRewarded(string location, UnityAction<bool> callback);
}
