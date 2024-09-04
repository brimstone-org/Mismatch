using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class AdMobInterstitials : MonoBehaviour {

    public InterstitialAd firstInterstitial;
    public InterstitialAd secondInterstitial;
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
	void Start () {
        firstInterstitial = RequestInterstitial();
        //secondInterstitial = RequestInterstitial();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private InterstitialAd RequestInterstitial()
    {
        #if UNITY_ANDROID
        		string adUnitId = "ca-app-pub-8530091499387924/5978220894";
        #elif UNITY_IPHONE
				string adUnitId = "ca-app-pub-8530091499387924/7590184494";
        #else
                string adUnitId = "unexpected_platform";
        #endif

        //Debug.Log("ADSTEST: Interstitial Request");
        // Initialize an InterstitialAd.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.

        //Debug.Log(SystemInfo.deviceUniqueIdentifier);
        //AdRequest request = new AdRequest.Builder().Build();
        AdRequest request = new AdRequest.Builder()
         .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
         //.AddTestDevice("3AEC0CADDEC2FF9A")
         .AddTestDevice("CB5A1YZ5HM")  // My test device.
         //.AddTestDevice(SystemInfo.deviceUniqueIdentifier)  // My test device.
         .Build();
        
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdLoaded += HandleInterstitialAdLoaded;
        
        return interstitial;
    }

    private void HandleInterstitialAdLoaded(object sender, EventArgs e)
    {
        //Debug.Log("ADSTEST: Interstitial Ad has loaded");
    }

    private void HandleInterstitialFailedToLoad(object sender, EventArgs e)
    {
        //Debug.Log("ADSTEST: InterstitialFailed to load event received");
    }

    private void HandleInterstitialClosed(object sender, EventArgs e)
    {
        //Debug.Log("ADSTEST: HandleInterstitialClosed event received");

        InterstitialAd thisAd = (InterstitialAd)sender;

        thisAd.Destroy();
        //Debug.Log("ADSTEST: HandleInterstitial Destroyed");

        
        //currentInterstitial = RequestInterstitial();
        //Debug.Log("ADSTEST: HandleInterstitial Requested new interstitial");
    }


    public void ShowAdmob()
    {
        if ((secondInterstitial != null) && (secondInterstitial.IsLoaded()))
        {
            //Debug.Log("ADSTEST: Interstitial is loaded and displaying");
            secondInterstitial.Show();

            if ((firstInterstitial == null) || (!firstInterstitial.IsLoaded()))
                firstInterstitial = RequestInterstitial();
         
        }
        else if ((firstInterstitial != null) && (firstInterstitial.IsLoaded()))
        {
            //Debug.Log("ADSTEST: Interstitial is loaded and displaying");
            firstInterstitial.Show();

            if ((secondInterstitial == null) || (!secondInterstitial.IsLoaded()))
            {
                //Debug.Log("ADSTEST: Load the other interstital");
                secondInterstitial = RequestInterstitial();
            }
            
        }
    }
    public bool checkForAds()
    {
        bool hasAds = false;

        if ((firstInterstitial != null) && (firstInterstitial.IsLoaded())) hasAds = true;
        if ((secondInterstitial != null) && (secondInterstitial.IsLoaded())) hasAds = true;

        return hasAds;
    }
    public void setToTryToLoad()
    {
        firstInterstitial = RequestInterstitial();
    }


}

