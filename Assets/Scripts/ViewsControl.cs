using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class ViewsControl : MonoBehaviour {

    public CanvasViewId[] Views;
    public UILevelPackManager uiLevelPackManager;
    public UINoAds uiNoAds;
    private GameView _lastView = GameView.MainMenu;
    public GameView currentView;
    public int packnum;
    public int levelnum;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (_lastView != currentView)
        {
            SwitchView((int)currentView);
        }
	}


    public void SwitchLevel(int level)
    {
        levelnum = level;
    }
    public void SwitchPack(int pack)
    {
        packnum = pack;
        levelnum = 0;
    }
    public void SwitchView(int gameViewInt)
    {
        GameView newGameView = (GameView)gameViewInt;
        SwitchView(newGameView);
    }

    public void SwitchView(GameView newGameView)
    {
        CanvasViewId thisViewID = Views.Where(x => x.gameView == _lastView).FirstOrDefault();
        GameObject thisView = thisViewID.gameObject;
        CanvasViewId newViewID = Views.Where(x => x.gameView == newGameView).FirstOrDefault();
        GameObject newView = newViewID.gameObject;
     
        if (newView == null) return;
        
        switch (newGameView)
        {
            case GameView.PackSelect: uiLevelPackManager.UpdateLevelPacks(); break;
            //case GameView.PlayGame: GameControl.me.playControl.ReloadLevel(); break;
            default: break;
        }

        thisView.SetActive(false);
        thisView = newView;
        newView.SetActive(true);
        _lastView = newGameView;
        currentView = newGameView;

        //Here we used to do ResourcesUnload but it was too strong.
        //Resources.UnloadUnusedAssets();
    }

    public void Update_UIs()
    {
        uiLevelPackManager.UpdateLevelPacks();
        uiNoAds.UpdateNoAdsButton();
    }
}

public enum GameView
{
    MainMenu = 0,
    PackSelect = 1,
    LevelSelect = 2,
    PlayGame = 3,
    AreYouSure = 4, //are you sure you want to quit panel 
    Pause = 5,
    LanguageSelect = 6
}
