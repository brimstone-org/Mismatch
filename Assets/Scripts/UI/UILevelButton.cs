using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Diagnostics;
using Mismatch.Utils;

[ExecuteInEditMode]
public class UILevelButton : MonoBehaviour {

    public Text buttonText;
    public Image buttonGraphic;
    public Button button;

    public void OnClick () 
    {
        StopAllCoroutines();
        AsyncPreloaderHackfix.instance.Loadingscreen.SetActive(true);
        StartCoroutine(doStuff());
	}

    public IEnumerator doStuff(){

//		GameControl.me.loadingText.SetActive(true);
 
 //  yield return null;


		int index = this.GetComponent<RectTransform>().GetSiblingIndex();


        SoundManager.Instance.BackgroundMusic.Stop();
        SoundManager.Instance.PlayBackgroundMusic(MusicPlayer.Instance.sounds[1].clip, Vector3.zero);
        GameControl.me.viewsControl.SwitchLevel(index);


		GameControl.me.viewsControl.SwitchView((int)GameView.PlayGame);



		GameControl.me.playControl.ReloadLevel();


		//GameControl.me.loadingText.SetActive(false);
        yield return null;
        AsyncPreloaderHackfix.instance.Loadingscreen.SetActive(false);
    }
}
