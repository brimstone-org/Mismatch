using UnityEngine;
using System.Collections;

public class ShareButton : MonoBehaviour {

	public void OnClick(){
		#if UNITY_ANDROID
		//AndroidSocialGate.StartShareIntent("Share", "https://play.google.com/store/apps/details?id=com.tedrasoft.mismatched");
        //ShareBunch.GetInstance().ShareText("https://play.google.com/store/apps/details?id=com.tedrasoft.mismatched");
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.tedrasoft.mismatched");
		#elif UNITY_IPHONE
		//IOSSocialManager.Instance.ShareMedia("https://itunes.apple.com/app/id972429993");
		#endif
	}
}
