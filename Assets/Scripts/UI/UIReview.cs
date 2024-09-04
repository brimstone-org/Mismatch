using UnityEngine;
using System.Collections;

public class UIReview : MonoBehaviour {

	#if UNITY_IPHONE
	protected string appId = "972429993";
	#elif UNITY_ANDROID
	protected string appId = "com.tedrasoft.mismatched";
	#endif

    public int RateToShow = 10;
    private int PlayedLevels = 0;

    public void Show()
    {
        /*var PlayerDataJson = GameControl.me.playerData.data;
        var playedLevels = PlayerDataJson["playerdata"]["levelsplayed"].AsInt;
        playedLevels++;
        PlayerDataJson["playerdata"]["levelsplayed"].AsInt = playedLevels;*/

        //PlayedLevels = playedLevels;
        PlayedLevels++;

        //var reviewGiven = PlayerDataJson["playerdata"]["reviewGiven"].AsBool;

        //GameControl.me.playerData.SaveData();

        // Review was given. don't ask again
        //if (reviewGiven) return;

        // Ask for review. open review window;
        //if (playedLevels % 10 == 0)
		if (PlayedLevels % RateToShow == 0)
			ShowRateWindow ();
    }
	public void Yes()
    {
		PlayerPrefs.SetInt ("rated", 1);
        //var PlayerDataJson = GameControl.me.playerData.data;
        //PlayerDataJson["playerdata"]["reviewGiven"].AsBool = true;
        //GameControl.me.playerData.SaveData();

        //Open Url
        #if UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.tedrasoft.mismatched");
#elif UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=" + appId + "&pageNumber=0&sortOrdering=2&type=Purple+Software&mt=8");
#elif UNITY_ANDROID
		Application.OpenURL("market://details?id=" + appId);
#endif

		//Close review panel
		this.gameObject.SetActive(false);

    }
	public void No()
    {
        //Close review panel
        this.gameObject.SetActive(false);
    }
	
	public void ShowRateWindow()
	{
		this.gameObject.SetActive(true);
	}
}
