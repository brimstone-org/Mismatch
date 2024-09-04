#define FACEBOOK_SDK
using UnityEngine;
using System.Collections;
using System;


public class FacebookManager : MonoBehaviour
{

    public string androidShareLink = "";
    public string iOSShareLink = "";

    string shareLink;


	public string imageLink = "http://tedrasoft.com/mismatched/img/iconOri.png";

    public static FacebookManager instance;

    public delegate void ShareBonus();
    public static event ShareBonus OnShareBonus;

    public bool shareGUI = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {

#if UNITY_ANDROID
			shareLink = androidShareLink;
#else
    shareLink = iOSShareLink;
#endif

			instance = this;
#if FACEBOOK_SDK
           // FB.Init();
#endif
        }
        //Only works for root objects - enable when it's a root object
		//DontDestroyOnLoad(this.gameObject);
    }
    public void ShareLink()
    {
        //Share(shareLink, "Mismatched Images", "Try this game!", imageLink);
        shareGUI = true;
    }
    public void OnGUI()
    {
        if (shareGUI)
        {
            shareGUI = false;
            Share(shareLink, "Mismatched Images", "Match the images into categories! The new puzzle sensation!", imageLink);
        }
    }
    public void Share(string link, string title, string description, string image)
    {
        image = image.Replace(' ', '_');
        Debug.Log(image);

#if FACEBOOK_SDK
        // FB.ShareLink(new Uri(link), title, description, new Uri(image), ShareCallback);
#endif
    }

#if FACEBOOK_SDK
    /*
    private void ShareCallback(IShareResult result)
    {
        shareGUI = false;
        
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log("ShareLink PostId: " + result.PostId);
            GiveShareBonus();
        }
        else
        {
            // Share succeeded without postID
            GiveShareBonus();
            Debug.Log("ShareLink success! result: " + result.RawResult);
        }
    }
    */
#endif

    void GiveShareBonus()
    {
        string date = DateTime.Now.ToString("dd/MM/yyyy");

        if (date != GameControl.me.playerData.GetPlayerDataString("LastShareDate"))
        {
            GameControl.me.playerData.SetPlayerDataString("LastShareDate", date);
            //GameControl.me.soomlaManager.GiveHintBonus(3);
        }
        OnShareBonus();
    }
}