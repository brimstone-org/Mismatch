using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;
using GDPR;

public class AdMobInterstitialsV2 : MonoBehaviour {

    public string interstitialId = "ca-app-pub-8530091499387924/5978220894";
    string personalizedAds = "0";
    private bool personalized;
    public bool Personalized 
    {
        get { return personalized; }
        set { personalized = value; personalizedAds = personalized ? "1" : "0"; }
    }
    public InterstitialAd currentInterstitial;
    public bool hasInterstitial = false;

    // Use this for initialization

    //interstitial.AdLoaded += HandleInterstitialLoaded;
    //interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
    //interstitial.AdOpened += HandleInterstitialOpened;
    //interstitial.AdClosing += HandleInterstitialClosing;
    //interstitial.AdClosed += HandleInterstitialClosed;
    //interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
    void OnEnable()
    {

    }
    void Start()
    {
        currentInterstitial = RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private InterstitialAd RequestInterstitial()
    {
        #if UNITY_ANDROID
                 interstitialId = "ca-app-pub-8530091499387924/5978220894";
#elif UNITY_IPHONE
				         interstitialId = "ca-app-pub-8530091499387924/7590184494";
#else
                         interstitialId = "unexpected_platform";
#endif

        Debug.Log("ADSTEST: Interstitial Request");
        // Initialize an InterstitialAd.
        InterstitialAd interstitial = new InterstitialAd(interstitialId);
        // Create an empty ad request.


        AdRequest request = new AdRequest.Builder()
         .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
         .AddTestDevice("CB5A1YZ5HM")  // My test device.
         .AddExtra("npa",personalizedAds)
         .Build();

        // Load the interstitial with the request.
        interstitial.LoadAd(request);
		interstitial.OnAdClosed += HandleInterstitialClosed;
		interstitial.OnAdFailedToLoad+= HandleInterstitialFailedToLoad;
		interstitial.OnAdLoaded += HandleInterstitialAdLoaded;

        return interstitial;
    }

    private void HandleInterstitialAdLoaded(object sender, EventArgs e)
    {
        hasInterstitial = true;
        Debug.Log("ADSTEST: Interstitial Ad has loaded");
    }

    private void HandleInterstitialFailedToLoad(object sender, EventArgs e)
    {
        hasInterstitial = false;
        Debug.Log("ADSTEST: InterstitialFailed to load event received");
    }

    private void HandleInterstitialClosed(object sender, EventArgs e)
    {
        Debug.Log("ADSTEST: HandleInterstitialClosed event received");

        InterstitialAd thisAd = (InterstitialAd)sender;

        thisAd.Destroy();
        hasInterstitial = false;
        Debug.Log("ADSTEST: HandleInterstitial Destroyed");


        currentInterstitial = RequestInterstitial();
        Debug.Log("ADSTEST: HandleInterstitial Requested new interstitial");
    }

    public void ShowAdmobSimple()
    {
        ShowAdmob();
    }
    public bool ShowAdmob()
    {
        //returns false when ad wasn't loaded

        bool Result = false;
        if ((currentInterstitial != null) && (currentInterstitial.IsLoaded()))
        {
            Debug.Log("ADSTEST: Interstitial is loaded and displaying");
            Result = true;
            currentInterstitial.Show();
        }
        else
        {
            Result = false;
            currentInterstitial = RequestInterstitial();
        }
        return Result;
    }
    public bool checkForAds()
    {
        bool hasAds = false;
        if ((currentInterstitial != null) && (currentInterstitial.IsLoaded())) hasAds = true;
        return hasAds;
    }


}


