using UnityEngine;
using System.Collections;
using ChartboostSDK;
using GDPR;

public class ChartboostInterstitials : MonoBehaviour {

    static public bool hasSeenMoreApps = false;
    static public bool hasInterstitial = false;

    private static ChartboostInterstitials _instance;

    private bool personalized;
    public bool Personalized 
    {
        get { return personalized; }
        set { personalized = value; }
    }
    public static ChartboostInterstitials Instance {
        get { return _instance; }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;

        } else {
            //Debug.LogError(name + " already instantiated");
            Destroy(gameObject);
        }
        Chartboost.restrictDataCollection(personalized);
    }

    void Start() {
        Chartboost.setAutoCacheAds(true);
        Chartboost.cacheMoreApps (CBLocation.Default);
        Chartboost.cacheInterstitial(CBLocation.Default);
    }
    void OnDisable()
    {
        Chartboost.didDismissMoreApps -= DidDismissMoreApps;
        Chartboost.didCloseMoreApps -= DidDismissMoreApps;
        Chartboost.didDisplayMoreApps -= DidDisplayMoreApps;
        Chartboost.didFailToLoadMoreApps -= DidFailToLoadMoreApps;
        Chartboost.didCacheInterstitial -= DidCacheInterstitial;
        Chartboost.didDismissInterstitial -= DidDismissInterstitial;
        Chartboost.didDisplayInterstitial -= DidDisplayInterstitial;
    }
    void OnEnable() {
        Chartboost.didDismissMoreApps += DidDismissMoreApps;
        Chartboost.didCloseMoreApps += DidDismissMoreApps;
        Chartboost.didDisplayMoreApps += DidDisplayMoreApps;
        Chartboost.didFailToLoadMoreApps += DidFailToLoadMoreApps;
        Chartboost.didFailToLoadInterstitial += DidFailToLoadInterstitial;
        Chartboost.didCacheInterstitial += DidCacheInterstitial;
        Chartboost.didDismissInterstitial += DidDismissInterstitial;
        Chartboost.didDisplayInterstitial += DidDisplayInterstitial;
    }

    private void DidFailToLoadInterstitial(CBLocation arg1, CBImpressionError arg2)
    {
        Debug.Log("Chartboost: Failed to load interstitial: " + arg2.ToString());
    }

    private void DidDisplayInterstitial(CBLocation obj)
    {
        hasInterstitial = false;
    }

    private void DidDismissInterstitial(CBLocation obj)
    {
        //throw new System.NotImplementedException();
    }

    private void DidCacheInterstitial(CBLocation obj)
    {
        hasInterstitial = true;
    }

    void DidFailToLoadMoreApps (CBLocation arg1, CBImpressionError arg2)
    {
        Debug.Log("Chartboost: Failed to load more apps: " + arg2.ToString());
    }

    void DidDisplayMoreApps (CBLocation obj)
    {
        hasSeenMoreApps = true;
    }

    void DidDismissMoreApps(CBLocation location)
    {
        Chartboost.cacheMoreApps (CBLocation.Default);
        hasSeenMoreApps = true;
    }

    public void DisplayMoreApps()
    {
        Debug.Log("Chartboost : Display More Apps");
        Chartboost.showMoreApps (CBLocation.Default);
    }

    public bool DisplayMoreAppsOnBackButton()
    {
        //More apps link
        //http://play.google.com/store/search?q=pub:Cooler

        if (hasSeenMoreApps)
            return false;

        hasSeenMoreApps = true;
        Debug.Log("Chartboost : Display More Apps on back button");
        DisplayMoreApps();
        return true;
    }

    public bool ShowInterstitial()
    {
        Debug.Log("Chartboost: Check for Interstitial");
        #if UNITY_EDITOR
        hasInterstitial = true;
        #endif
        if (hasInterstitial)
        {

            Debug.Log("Chartboost Interstitial");
            Chartboost.showInterstitial(CBLocation.Default);

            return true;
        }
        else
        {
            Chartboost.cacheInterstitial(CBLocation.Default);
            return false;
        }
    }
}
