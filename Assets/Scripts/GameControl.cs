using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl me;

    public ImageManager imageManager;
    public LevelsLoader levelsLoader;
    public PlayControl playControl;
    public ViewsControl viewsControl;
    public PlayerData playerData;

    // SoomlaManager soomlaManager;
    public FacebookManager facebookManager;
    public AdvertisingManager adsManager;
    public bool levelsLoaded = false;
    public bool playerDataLoaded = false;

    public bool FinishedEasyPack;
    public bool FinishedMediumPack;
    public bool FinishedHardPack;
    public bool FinishedWinterPack;
    public bool FinishedExtraPack;
    //public int HintsRemaining = 10;

    public bool unloadAll;

    void Awake()
    {       
        SingletonCheck();

        if (PlayerPrefs.HasKey("hintsNo") == false) {
            PlayerPrefs.SetInt("hintsNo",10);
        }

        if(PlayerPrefs.HasKey("noAds") == false) {
            PlayerPrefs.SetInt("noAds",0);
        }

        if(PlayerPrefs.HasKey("winterPack") == false) {
            PlayerPrefs.SetInt("winterPack",0);
        }


    }
	// Use this for initialization
	void Start () 
    {

       // PlayerPrefs.SetInt("winterPack",0);
        DOTween.Init();
        GetComponents();

        if (!PlayerPrefs.HasKey("EasyPack"))
        {
            PlayerPrefs.SetInt("EasyPack", 0);
        }
        if (PlayerPrefs.GetInt("EasyPack") == 1)
        {
            FinishedEasyPack = true;
        }
        if (!PlayerPrefs.HasKey("MediumPack"))
        {
            PlayerPrefs.SetInt("MediumPack", 0);
        }
        if (PlayerPrefs.GetInt("MediumPack") == 1)
        {
            FinishedMediumPack = true;
        }
        if (!PlayerPrefs.HasKey("HardPack"))
        {
            PlayerPrefs.SetInt("HardPack", 0);
        }
        if (PlayerPrefs.GetInt("HardPack") == 1)
        {
            FinishedHardPack = true;
        }
        if (!PlayerPrefs.HasKey("WinterPack"))
        {
            PlayerPrefs.SetInt("WinterPack", 0);
        }
        if (PlayerPrefs.GetInt("WinterPack") == 1)
        {
            FinishedWinterPack = true;
        }
        if (!PlayerPrefs.HasKey("ExtraPack"))
        {
            PlayerPrefs.SetInt("ExtraPack", 0);
        }
        if (PlayerPrefs.GetInt("ExtraPack") == 1)
        {
            FinishedExtraPack = true;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
       /// Debug.Log(PlayerPrefs.GetInt("hintsNo"));
       
        //if (unloadAll)
        //{
        //    unloadAll = false;
        //    Resources.UnloadUnusedAssets();
        //    System.GC.Collect();
        //}

        //Input checks - Escape button
        BackButtonCheck();

    }
    public void OnLevelsLoaded()
    {
        levelsLoaded = true;

        if (playerDataLoaded) return;
        playerDataLoaded = playerData.LoadData();
    }
    void OnSceneLoaded()
    {

    }
    void SingletonCheck()
    {
        if (me == null)
        {
            me = this;
            //GameObject.DontDestroyOnLoad(this.gameObject);
			//DontDestroyOnLoad(this.gameObject);
        }
        if (me != this)
        {
            Destroy(this.gameObject);
        }
    }

    void GetComponents()
    {
        if (imageManager == null)
            imageManager = GetComponentInChildren<ImageManager>();
    }
    void BackButtonCheck()
    {
        if (Input.GetKeyUp(KeyCode.Escape) ) 
        {
            if (viewsControl.currentView == GameView.PlayGame )
            {
                playControl.BackToLevelSelect();
            }
            else if (viewsControl.currentView == GameView.LevelSelect)
            {
                viewsControl.SwitchView(GameView.PackSelect);
            }
            else if (viewsControl.currentView == GameView.PackSelect)
            {
                viewsControl.SwitchView(GameView.MainMenu);
            }
            else if (viewsControl.currentView == GameView.LanguageSelect)
            {
                viewsControl.SwitchView(GameView.MainMenu);
                // playControl.BackToLevelSelect();
            }
            else if ( viewsControl.currentView == GameView.MainMenu)
            {
                //Show more apps if havent shown yet. Else quit.
               // bool displayMoreApps = ChartboostInterstitials.Instance.DisplayMoreAppsOnBackButton();
                //if (!displayMoreApps)
                    Application.Quit();
            }

        }
    }

    public void OpenLanguageSelect()
    {
        viewsControl.Views[4].gameObject.SetActive(true); //open pause panel over the play panel
    }

}
