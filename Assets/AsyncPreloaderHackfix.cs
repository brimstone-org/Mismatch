using UnityEngine;
using System.Collections;
using UnityEngine.U2D;
using System;

public class AsyncPreloaderHackfix : MonoBehaviour {
    public static AsyncPreloaderHackfix instance { get; set; }

    public GameObject Loadingscreen;
    public string atlasName;
	public string path;
    SpriteAtlas atlas;


    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        LoadSprites();
    }

    public void LoadSprites(){
        Debug.LogWarning("STOPWATCHMESSAGE2: Started PRELOADING");
        StartCoroutine(IELoadSprites());
    }

    IEnumerator IELoadSprites(){
		atlas = Resources.LoadAsync<SpriteAtlas>(path).asset as SpriteAtlas;
        yield return null;
    }

    public void grabSprites(Sprite[] x)
    {
        atlas.GetSprites(x);
    }

}
