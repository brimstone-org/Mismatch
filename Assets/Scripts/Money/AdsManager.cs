using UnityEngine;
using System.Collections;
using NativeInApps;
using GDPR;

public class AdsManager : MonoBehaviour {

    public ChartboostInterstitials cbAds;
    public AdMobInterstitialsV2 admobAds;

    public float am2cbRatio = 2f;
    public int admobTimes = 1;
    public int cbTimes = 1;

    public int TimesRequested = 0;
    public int RateToShow = 2;

    public bool AdsDisabled { get; private set; }


    void UpdatePersonalized() 
    {
        admobAds.Personalized = GDPR.GDPRConsentPanel.Instance.AsBool;
        cbAds.Personalized = GDPR.GDPRConsentPanel.Instance.AsBool;
    }

    private void Start() {
       // PlayerPrefs.DeleteAll();
        UpdatePersonalized();
    }
    public void ShowInterstitial()
    {
        if(PlayerPrefs.GetInt("noAds") == 1)
            return;

        TimesRequested++;

        if (TimesRequested % RateToShow == 0)
        {
            if ((float)admobTimes / (float)cbTimes > am2cbRatio) // It's time to add some chartboost as well
             {
            if (ShowAdmob())
                ShowAdmob();
            }
            else  // Admob, as usual
            {
                if (!ShowAdmob())
                    ShowCB();
            }
        }
    }

    public bool ShowCB()
    {
        bool result = cbAds.ShowInterstitial();
        if (result) cbTimes++;
        return result;
    }
    public bool ShowAdmob()
    {
        bool result = admobAds.ShowAdmob();
        if (result) admobTimes++;
        return result;
    }
    private void OnEnable() {
        NativeInApp.OnRefreshCompleted += RefreshAdsStatus;
        NativeInApp.OnItemPurchased += OnItemPurchasedAds;
        GDPRConsentPanel.Instance.OnConsentChange += UpdatePersonalized;
    }

    private void OnDisable() {
        //NativeInApp.OnRefreshCompleted -= RefreshAdsStatus;
        NativeInApp.OnItemPurchased -= OnItemPurchasedAds;
        GDPRConsentPanel.Instance.OnConsentChange -= UpdatePersonalized;
    }
    public static void DisableAds() {
        PlayerPrefs.SetInt("noAds",1);
    }

    void OnItemPurchasedAds(string sku) {
        if(sku == NativeInApps.NativeInApp_IDS.NO_ADS_ID) {
            DisableAds();
        }
        RefreshAdsStatus();
    }
    void RefreshAdsStatus() {
        int enabled = PlayerPrefs.GetInt("noAds");
        AdsDisabled = enabled == 0 ? false : true;
    }

    [System.Obsolete("Method no longer used by Chartboost",true)]
	public void ShowMoreApps()
	{
        //Chartboost.showMoreApps(CBLocation.Default);
        //cbAds.DisplayMoreApps();
        //Debug.LogWarning("Method no longer usable.");
	}
}
