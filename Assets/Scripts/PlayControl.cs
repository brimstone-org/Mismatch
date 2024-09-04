using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Mismatch.Utils;

[ExecuteInEditMode]

public class PlayControl: MonoBehaviour {
    // public GameControl gameControl; //Get and keep a reference to this

    public static PlayControl instance = null;

    public List<UIColHeader> headerList = new List<UIColHeader>();
    public Level currentLevel = null;
    public List<UICard> cardsList = new List<UICard>();
    public bool refreshlist;

    //Other UI elements
    public List<UIColumnComplete> ColumnComplete = new List<UIColumnComplete>();
    public UIHintsMenu hintsMenu;
    public UIHintsBuyMore hintsBuyMore;
    public UIVictory victoryMenu;
    public UIReview reviewMenu;
    public Button btnSideNextLevel;
    public Text hintsRemaining;
    public Text levelText;

    //Partial Cards list
    public List<UICard> cardsMoveable = new List<UICard>();
    public List<UICard> cardsLocked = new List<UICard>();

    //Single cards
    public Transform cloneParent;
    public UICard lastClosest;
    public UICard lastDragged;
    public UICard lastSelected;

    public Color colorUICardSwitch = Color.green;
    public Color colorUICardNormal = Color.white;
    public Color colorUICardDragged = new Color(1f,1f,1f,0.5f);
    public Color colorMismatch = new Color(1f,111f / 255f,0f);

    //simple bool
    public bool hasVictory = false;

    //for testing
    public bool loadedLevel = false;
    public bool testColors = false;
    public Sprite emptySprite;
    public bool unloadLevel = false;
    public bool loadLevel = false;
    public bool loadAllLevels = false;

    private CardMode _cardsMode = CardMode.image;
    public CardMode cardsMode = CardMode.image;

    public bool pressedQuit;
    public bool closedMoreApps;

    private void Awake() {
        if(instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:

    }
    // Use this for initializatio
    void Start() {


    }
    // Update is called once per frame
    void Update() {

        TestingElements();
        PlayFunctions();
    }
    void LoadAllLevels() {//Testing function
        foreach(var levelpack in GameControl.me.levelsLoader.LevelPacks) {
            foreach(var level in levelpack.levels) {
                currentLevel = level;
                LoadLevel(true,true);
            }
        }
    }
    public void ReloadLevel() {
        LoadLevel(reload: true);
    }
    void LoadLevel(bool reload) {
        if (!victoryMenu.NextLevelButton.activeSelf)
        {
            victoryMenu.NextLevelButton.SetActive(true);
        }

        LoadLevel(reload,false);
    }
    void LoadLevel(bool reload,bool LoadCurrentLevel) {
        AsyncPreloaderHackfix.instance.Loadingscreen.SetActive(false);
        //Stopwatch sw = new Stopwatch();


        //Checks
        if(loadedLevel && !reload) return;
        if(GameControl.me == null) return;
        if(GameControl.me.levelsLoaded == false) return;
        if(GameControl.me.viewsControl.levelnum < 0) return;
        //if (!GameControl.me.viewsControl.Views[(int)GameView.PlayGame].gameObject.activeInHierarchy) return;
        if(GameControl.me.levelsLoader.LevelPacks.Count == 0) return;



        //Reset the UI
        ResetUI();




        //Init
        GameControl.me.playControl = this;
        int levelnum = GameControl.me.viewsControl.levelnum;
        int packnum = GameControl.me.viewsControl.packnum;

        if(!LoadCurrentLevel)
            currentLevel = GameControl.me.levelsLoader.LevelPacks[packnum].levels[levelnum];
        loadedLevel = true;
        Level level = currentLevel;
        level.randomizeState();




        levelText.text = currentLevel.id.ToString();

        //sw.Start();
        for(int i = 0;i < level.CurrentState.Count;i++) {
            //sw.Start();
            Sprite sprite = getSpriteByString(level.CurrentState[i]);
            //sw.Stop();
            //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE: LOADLEVEL GETSPRITEBYSTRING TOOK " + sw.Elapsed);

            String ImageName = getCleanName(level.CurrentState[i]);

            if(sprite == null) {
                UnityEngine.Debug.LogWarning("sprite not found:" + ImageName);
            }

            //sw.Start();
            UICard card = cardsList[i];
            //sw.Stop();
            //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE: UICARD CONSTRUCTOR TOOK " + sw.Elapsed);

            //sw.Start();
            card.setPicSprite(sprite);
            //sw.Stop();
            //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE: LOADLEVEL CARDSETPICSPRITE TOOK " + sw.Elapsed);

            //sw.Start();
            card.setCardText(ImageName);
            //sw.Stop();
            //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE: LOADLEVEL  SETCARDTEXT TOOK " + sw.Elapsed);
        }
        //sw.Stop();
        //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE: LOADLEVEL LOOP TOOK " + sw.Elapsed + " with levelCurrentStateCount=" + level.CurrentState.Count);


        setHeaders();



        UpdateViewModel(PlayIntroAnims: true);

        //Everything is done. Play card anims
    }
    void ResetUI() {
        //Init UIElements - Hide any open windows, set hints remaining
        hintsMenu.setActive(false);
        hintsBuyMore.setActive(false);
        victoryMenu.setActive(false);
        hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();
        //Hide next button
        btnSideNextLevel.gameObject.SetActive(false);

        //Clean column complete
        foreach(UIColumnComplete cc in ColumnComplete) {
            cc.SetEnabled(false);
        }
        //Hide Headers
        //foreach (UIColHeader header in headerList)
        //{
        //    header.headerText.enabled = false;
        //}
        //Hide other menus
        hintsMenu.setActive(false);
        hintsBuyMore.setActive(false);
        victoryMenu.setActive(false);

    }
    Sprite getSpriteByString(String ImageName) {
        //Stopwatch sw = new Stopwatch();
        ImageName = getCleanName(ImageName);
        //sw.Start();
        ImageManager imageManager = GameControl.me.imageManager;
        //sw.Stop();
        //UnityEngine.Debug.LogWarning("STOPWATCHMESSAGE2: GETSPRITEBYSTRING ImageManager ConstructorTook TOOK " + sw.Elapsed);
        return imageManager.GetSpriteByName(ImageName);
    }
    String getCleanName(String ImageName) {
        ImageName = ImageName.Replace(".png","");
        ImageName = ImageName.Trim();
        return ImageName;
    }
    void GetListUICard() {
        cardsList.Clear();
        var cards = GameObject.FindObjectsOfType<UICard>();
        foreach(var card in cards) {
            if(card.gameObject.activeInHierarchy) {
                cardsList.Add(card);
            }

        }

        cardsList = cardsList.OrderBy(a => IntFromString(a.gameObject.name)).ToList();
    }
    int IntFromString(string input) {
        Match m = Regex.Match(input,@"\d+");
        if(m.Success == false) return 0;
        int number = 0;
        try { number = Convert.ToInt32(m.Value); } catch { UnityEngine.Debug.Log("failed to convert to int value: " + m.Value); }

        return number;
    }
    void SetImageMode(CardMode cardMode) {
        _cardsMode = cardMode;
        cardsMode = cardMode;

        foreach(var card in cardsList) {
            card.setCardMode(cardMode);
        }
    }
    void TestingElements() {
#if UNITY_EDITOR
        //While playing
        if(Application.isPlaying) {
            if(loadAllLevels) {
                loadAllLevels = false;
                LoadAllLevels();
            }
        }
        //Editor only
        if(Application.isPlaying) return;
        if(refreshlist) {
            refreshlist = false;
            GetListUICard();
        }
        if(cardsMode != _cardsMode) {
            SetImageMode(cardsMode);
        }
        if(unloadLevel) {
            unloadLevel = false;
            UnloadLevel();
        }
        if(loadLevel) {
            loadLevel = false;
            LoadLevel(true);
        }

#endif
    }
    void PlayFunctions() {
#if UNITY_EDITOR
        if(!Application.isPlaying) return;
#endif
        HandleDrag();
        if(hasVictory) {
            btnSideNextLevel.gameObject.SetActive(true);
            bool done = true;
            foreach(UIColumnComplete cc in ColumnComplete) {
                if(!cc.animEnded) done = false;
            }
            if(done) {
                hasVictory = false;
               // UI_VictoryShow();
            }
        }
    }
    void HandleDrag() {
        if(lastDragged != null) {
            if(this.cardsLocked.Contains(lastSelected)) {
                // Card is locked. Abort drag
                ReleaseDrag();
            }
        }

        if(lastDragged != null) {
            if(lastDragged.dragUI.bDrag == true)// Drag has started
            {

                lastSelected.setActive(false);
                lastDragged.setPicAndTextColor(colorUICardDragged);
                showClosest(lastDragged);

            } else // Drag has ended. Do sprite swaps and cleanup
              {
                if(lastClosest != null) lastClosest.setPicAndTextColor(colorUICardNormal);
                Destroy(lastDragged.gameObject);
                lastSelected.setActive(true);

                CardSwap(lastSelected,lastClosest);
            }
        }
    }
    public void ReleaseDrag() {
        if(lastClosest != null) lastClosest.setPicAndTextColor(colorUICardNormal);
        if(lastDragged != null) Destroy(lastDragged.gameObject);

        if(lastSelected != null)
            lastSelected.setActive(true);

        lastSelected = null;
        lastClosest = null;
        lastDragged = null;
    }
    bool CardSwap(UICard card1,UICard card2) {

        if(card1 == null) return false;
        if(card2 == null) return false;

        //Level swap
        int one = cardsList.IndexOf(card1);
        int two = cardsList.IndexOf(card2);
        //Debug.Log("Swapping " + one + " / " + two);

        //Debug.Log("Checking to see if valid move...");
        if(currentLevel.PlaySwapPlaces(one,two) == false) return false;
        //Debug.Log("Valid move...swapping");

        // Sprite swapping
        var name1 = card1.gameObject.name;
        var rt1 = card1.gameObject.GetComponent<RectTransform>();
        var parent1 = rt1.parent;
        var num1 = rt1.GetSiblingIndex();

        var name2 = card2.gameObject.name;
        var rt2 = card2.gameObject.GetComponent<RectTransform>();
        var parent2 = rt2.parent;
        var num2 = rt2.GetSiblingIndex();

        card1.gameObject.name = name2;
        rt1.SetParent(parent2,false);
        rt1.SetSiblingIndex(num2);

        card2.gameObject.name = name1;
        rt2.SetParent(parent1,false);
        rt2.SetSiblingIndex(num1);

        //Card list swapping
        cardsList.RemoveAt(one);
        cardsList.Insert(one,card2);
        cardsList.RemoveAt(two);
        cardsList.Insert(two,card1);

        UpdateViewModel();
        return true;
    }
    void showClosest(UICard card) {
        if(lastClosest != null)
            lastClosest.setPicAndTextColor(colorUICardNormal);

        var closeCard = getNearest(card);
        lastClosest = closeCard;

        //Effect for closest card
        closeCard.setPicAndTextColor(colorUICardSwitch);
    }
    UICard getNearest(UICard card) {
        List<UICard> cards = new List<UICard>(cardsMoveable);

        //Remove lastSelected as well
        cards.Remove(lastSelected);

        UICard closest = null;
        float distsq = float.MaxValue;
        foreach(var c in cards) {
            Vector2 distance = card.transform.position - c.transform.position;
            float mag = distance.sqrMagnitude;
            if(mag < distsq) {
                distsq = mag;
                closest = c;
            }
        }
        return closest;
    }
    void UpdateViewModel() {
        UpdateViewModel(PlayIntroAnims: false);
    }
    void UpdateViewModel(bool PlayIntroAnims) {
        cardsLocked.Clear();
        cardsMoveable.Clear();

        cardsLocked = getLockedCards();
        cardsMoveable = getMovableCards(cardsLocked);

        for(int i = 0;i < cardsList.Count;i++) {
            var card = cardsList[i];
            //Set the picture to have a frame if it is locked;
            bool isFixed = cardsLocked.Contains(card);
            card.setFrameVisbile(isFixed);

            //Play intro animation for cards. Only on level load, only for not fixed cards
            if(!isFixed && PlayIntroAnims)
                card.PlayIntroAnim();

            //if (isFixed)
            //    card.setFrameColor(fixedFrame);

            //Set current card show mode Text/Image

            card.setCardMode(cardsMode);
        }

        //Set column headers again in case they changed
        if((currentLevel.difficulty == Difficulty.hard) || (currentLevel.difficulty == Difficulty.medium)) {
            setHeaders();
        }
        //Check if any columns are complete and set Column Complete UI Elements
        for(int i = 0;i < currentLevel.columns;i++) {
            ColumnComplete[i].SetEnabled(currentLevel.CurrentSolved[i]);
        }
        //Check for victory
        if(currentLevel.SolvedAll) { hasVictory = true; Victory(); } else { hasVictory = false; }
    }
    void setHeaders() {

        for(int i = 0;i < currentLevel.headers.Count;i++) {
            //Header visibility
            bool visible = true;
            if((currentLevel.difficulty == Difficulty.hard) || (currentLevel.difficulty == Difficulty.medium)) {
                visible = currentLevel.CurrentSolved[i];
            }

            String htext;
            if(currentLevel.headers[i] != "other") {
                htext = LanguageManager.instance.Get(currentLevel.headers[i]);
            } else {
                htext = "other";
            }
            //htext = htext.ToLower();
            htext = htext.Trim();
            Color color = Color.white;

            //Special case for mismatch header
            if(htext == "other") {
                htext = LanguageManager.instance.Get("mismatch");
                color = colorMismatch;
                if(currentLevel.difficulty == Difficulty.medium)  //Always Show Mismatch header on medium diff
                    visible = true;
            }

            headerList[i].SetVisible(visible);
            headerList[i].SetHeaderText(htext);
            headerList[i].SetHeaderColor(color);
        }
    }
    List<UICard> getMovableCards() {
        List<UICard> toReturn = new List<UICard>(cardsList).ToList();

        List<UICard> toRemove = new List<UICard>();
        foreach(var elem in currentLevel.LockedList)
            toRemove.Add(cardsList[elem]);
        toReturn.RemoveAll(x => toRemove.Contains(x));
        return toReturn;
    }
    List<UICard> getMovableCards(List<UICard> lockedCards) {
        List<UICard> toReturn = new List<UICard>(cardsList).ToList();
        toReturn.RemoveAll(x => lockedCards.Contains(x));
        return toReturn;
    }
    List<UICard> getLockedCards() {
        List<UICard> toReturn = new List<UICard>();
        foreach(var elem in currentLevel.LockedList)
            toReturn.Add(cardsList[elem]);
        return toReturn;
    }
    public void ReplaceCardReference(UICard oldCard,UICard newCard) {
        cardsList[cardsList.IndexOf(oldCard)] = newCard;
    }
    public void ReplaceCardReferenceInAll(UICard oldCard,UICard newCard) {
        cardsList[cardsList.IndexOf(oldCard)] = newCard;

        if(cardsMoveable.Contains(oldCard))
            cardsMoveable[cardsMoveable.IndexOf(oldCard)] = newCard;
        if(cardsLocked.Contains(oldCard))
            cardsLocked[cardsLocked.IndexOf(oldCard)] = newCard;
    }
    public void UnloadLevel() {
        //this function will unload the sprites in the level 
        foreach(var card in cardsList) {
            card.setCardText("");
            card.setPicSprite(emptySprite);
            //card.setCardMode(_cardsMode);
            hintsMenu.hint1.setPicSprite(emptySprite);
            hintsMenu.hint2.setPicSprite(emptySprite);
        }

        unloadLevel = false;
        loadedLevel = false;
    }
    public void Victory()
    {
        StartCoroutine(WaitABit());

     

    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(2.5f);

        //Set unlocked level
        ViewsControl viewsControl = GameControl.me.viewsControl;
        LevelPack lp = GameControl.me.levelsLoader.LevelPacks[viewsControl.packnum];

        int unlockedmax = Mathf.Max(viewsControl.levelnum + 1 + 1, lp.no_levelsUnlocked);
        lp.no_levelsUnlocked = Mathf.Min(lp.no_levels, unlockedmax);

        //Update PlayerData
        UnityEngine.Debug.Log(GameControl.me.playerData.SaveData());
        string titleText = "";
        string bodyText = "";
        switch (viewsControl.packnum)
        {
            case 0:
                if (!GameControl.me.FinishedEasyPack && viewsControl.levelnum == 79 && GameControl.me.FinishedMediumPack && GameControl.me.FinishedHardPack /*&& GameControl.me.FinishedWinterPack && GameControl.me.FinishedExtraPack*/)
                {
                    titleText = LanguageManager.instance.Get("gamewon");
                    bodyText = LanguageManager.instance.Get("complete_game");
                    victoryMenu.NextLevelButton.SetActive(false);
                    GameControl.me.FinishedEasyPack = true;
                    PlayerPrefs.SetInt("EasyPack", 1);
                }
                else if (!GameControl.me.FinishedEasyPack && viewsControl.levelnum == 79)
                {
                    titleText = LanguageManager.instance.Get("packwon");
                    bodyText = LanguageManager.instance.Get("complete_pack").Replace("%levelpack%", "EASY");
                    GameControl.me.FinishedEasyPack = true;
                    PlayerPrefs.SetInt("EasyPack", 1);
                }
                else if (viewsControl.levelnum < 79 || GameControl.me.FinishedEasyPack)
                {
                    titleText = LanguageManager.instance.Get("congrat");
                    bodyText = LanguageManager.instance.Get("complete_level").Replace("%levelnumber%", (viewsControl.levelnum + 1).ToString());
                }

                break;
            case 1:
                if (!GameControl.me.FinishedMediumPack && viewsControl.levelnum == 89 && GameControl.me.FinishedEasyPack && GameControl.me.FinishedHardPack/* && GameControl.me.FinishedWinterPack && GameControl.me.FinishedExtraPack*/)
                {
                    titleText = LanguageManager.instance.Get("gamewon");
                    bodyText = LanguageManager.instance.Get("complete_game");
                    victoryMenu.NextLevelButton.SetActive(false);
                    GameControl.me.FinishedMediumPack = true;
                    PlayerPrefs.SetInt("MediumPack", 1);
                }
                else if (!GameControl.me.FinishedMediumPack && viewsControl.levelnum == 89)
                {
                    titleText = LanguageManager.instance.Get("packwon");
                    bodyText = LanguageManager.instance.Get("complete_pack").Replace("%levelpack%", "MEDIUM");
                    GameControl.me.FinishedMediumPack = true;
                    PlayerPrefs.SetInt("MediumPack", 1);
                }
                else if (viewsControl.levelnum < 89 || GameControl.me.FinishedMediumPack)
                {
                    titleText = LanguageManager.instance.Get("congrat");
                    bodyText = LanguageManager.instance.Get("complete_level").Replace("%levelnumber%", (viewsControl.levelnum + 1).ToString());
                }

                break;
            case 2:
                if (!GameControl.me.FinishedHardPack && viewsControl.levelnum == 99 && GameControl.me.FinishedEasyPack && GameControl.me.FinishedMediumPack /*&& GameControl.me.FinishedWinterPack && GameControl.me.FinishedExtraPack*/)
                {
                    titleText = LanguageManager.instance.Get("gamewon");
                    bodyText = LanguageManager.instance.Get("complete_game");
                    victoryMenu.NextLevelButton.SetActive(false);
                    GameControl.me.FinishedHardPack = true;
                    PlayerPrefs.SetInt("HardPack", 1);
                }
                else if (!GameControl.me.FinishedHardPack && viewsControl.levelnum == 99)
                {
                    titleText = LanguageManager.instance.Get("packwon");
                    bodyText = LanguageManager.instance.Get("complete_pack").Replace("%levelpack%", "HARD");
                    GameControl.me.FinishedHardPack = true;
                    PlayerPrefs.SetInt("HardPack", 1);
                }
                else if (viewsControl.levelnum < 99 || GameControl.me.FinishedHardPack)
                {
                    titleText = LanguageManager.instance.Get("congrat");
                    bodyText = LanguageManager.instance.Get("complete_level").Replace("%levelnumber%", (viewsControl.levelnum + 1).ToString());
                }

                break;
            case 3:
                if (!GameControl.me.FinishedWinterPack && viewsControl.levelnum == 99 && GameControl.me.FinishedEasyPack && GameControl.me.FinishedHardPack && GameControl.me.FinishedMediumPack && GameControl.me.FinishedExtraPack)
                {
                    titleText = LanguageManager.instance.Get("gamewon");
                    bodyText = LanguageManager.instance.Get("complete_game");
                    victoryMenu.NextLevelButton.SetActive(false);
                    GameControl.me.FinishedWinterPack = true;
                    PlayerPrefs.SetInt("WinterPack", 1);
                }
                else if (!GameControl.me.FinishedWinterPack && viewsControl.levelnum == 99)
                {
                    titleText = LanguageManager.instance.Get("packwon");
                    bodyText = LanguageManager.instance.Get("complete_pack").Replace("%levelpack%", "WINTER");
                    GameControl.me.FinishedWinterPack = true;
                    PlayerPrefs.SetInt("WinterPack", 1);
                }
                else if (viewsControl.levelnum < 99 || GameControl.me.FinishedWinterPack)
                {
                    titleText = LanguageManager.instance.Get("congrat");
                    bodyText = LanguageManager.instance.Get("complete_level").Replace("%levelnumber%", (viewsControl.levelnum + 1).ToString());
                }


                break;
            case 4:
                if (!GameControl.me.FinishedExtraPack && viewsControl.levelnum == 99 && GameControl.me.FinishedEasyPack && GameControl.me.FinishedHardPack && GameControl.me.FinishedWinterPack && GameControl.me.FinishedMediumPack)
                {
                    titleText = LanguageManager.instance.Get("gamewon");
                    bodyText = LanguageManager.instance.Get("complete_game");
                    victoryMenu.NextLevelButton.SetActive(false);
                    GameControl.me.FinishedExtraPack = true;
                    PlayerPrefs.SetInt("ExtraPack", 1);
                }
                else if (!GameControl.me.FinishedExtraPack && viewsControl.levelnum == 99)
                {
                    titleText = LanguageManager.instance.Get("packwon");
                    bodyText = LanguageManager.instance.Get("complete_pack").Replace("%levelpack%", "EXTRA");
                    GameControl.me.FinishedExtraPack = true;
                    PlayerPrefs.SetInt("ExtraPack", 1);
                }
                else if (viewsControl.levelnum < 99 || GameControl.me.FinishedExtraPack)
                {
                    titleText = LanguageManager.instance.Get("congrat");
                    bodyText = LanguageManager.instance.Get("complete_level").Replace("%levelnumber%", (viewsControl.levelnum + 1).ToString());
                }

                break;
        }
        victoryMenu.initialize(titleText, bodyText);
    } 

    public void BackToLevelSelect() {
        ReleaseDrag();  // Release the current dragged card
        ViewsControl viewsControl = GameControl.me.viewsControl;
        viewsControl.SwitchView(GameView.LevelSelect);
        GameControl.me.playControl.UnloadLevel();
        SoundManager.Instance.BackgroundMusic.Stop();
        SoundManager.Instance.PlayBackgroundMusic(MusicPlayer.Instance.sounds[0].clip, Vector3.zero);
    }
    #region UI_Functions
    public void UI_LoadNextLevel() {
        ViewsControl viewsControl = GameControl.me.viewsControl;
        int currentLevel = viewsControl.levelnum;
        int currentPack = viewsControl.packnum;
        var levels = GameControl.me.levelsLoader.LevelPacks[currentPack].levels;

        //Show Review Menu
        ShowRateWindow();

        //Show next level
        if(currentLevel < levels.Count - 1) {
            viewsControl.levelnum = currentLevel + 1;
            ReloadLevel();
        } else {
            viewsControl.SwitchView(GameView.PackSelect);
        }
        //GameControl.me.adsManager.ShowInterstitial();
        GameControl.me.adsManager.UpdateAds();
    }


    bool cd = false;
    public void UI_ReloadLevel() {
        if(cd)
            return;

        cd = true;
        ReleaseDrag(); // Release the current dragged card
        ReloadLevel();
        StartCoroutine(ReloadCD());
    }

    IEnumerator ReloadCD() {
        float time = 0.0f;

        while(time < 0.8f) {
            time += Time.deltaTime;
            yield return null;
        }
        cd = false;
    }




    public void UI_GoBack() {
        BackToLevelSelect();
    }
    public void UI_HintsActivate() {
        ReleaseDrag(); // Release the current drag

        HintPair hp = currentLevel.getHintPair();
        if(hp == null) return;

        if(PlayerPrefs.GetInt("hintsNo") > 0) {
            print(PlayerPrefs.GetInt("hintsNo").ToString());
            PlayerPrefs.SetInt("hintsNo",PlayerPrefs.GetInt("hintsNo") - 1);
            hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();
        } else { UI_HintsBuyMore(); return; }


        //Update Hints remaining
        hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();

        //Update Hints remaining
        //hintsRemaining.text = PlayerPrefs.GetString("hintsNo");

        //Hint window
        hintsMenu.setActive(true);

        hintsMenu.hint1.setCardText(getCleanName(hp.hint1));
        hintsMenu.hint1.setPicSprite(getSpriteByString(hp.hint1));
        hintsMenu.hint1.setCardMode(cardsMode);

        hintsMenu.hint2.setCardText(getCleanName(hp.hint2));
        hintsMenu.hint2.setPicSprite(getSpriteByString(hp.hint2));
        hintsMenu.hint2.setCardMode(cardsMode);

        //Update PlayerData
        GameControl.me.playerData.SaveData();

    }
    public void UI_HintsBuyMore() {
        hintsBuyMore.setActive(true);
    }
    public void UI_HintsBuy10MoreButton() {
        //Update Hints remaining
        UnityEngine.Debug.Log(PlayerPrefs.GetInt("hintsNo"));
        PlayerPrefs.SetInt("hintsNo",PlayerPrefs.GetInt("hintsNo") + 10);
        hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();
        //Update PlayerData
        GameControl.me.playerData.SaveData();
    }

    public void UI_HintsBuyMoreHints(int i)
    {
        //Update Hints remaining
        UnityEngine.Debug.Log(PlayerPrefs.GetInt("hintsNo"));
        PlayerPrefs.SetInt("hintsNo", PlayerPrefs.GetInt("hintsNo") + i);
        hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();
        //Update PlayerData
        GameControl.me.playerData.SaveData();
    }

    public void NativeInApp_OnItemPurchased(string SKU) {
        if(SKU == NativeInApps.NativeInApp_IDS.HINTS_10_ID) {
            UI_HintsBuy10MoreButton();
        }

        if(SKU == NativeInApps.NativeInApp_IDS.HINTS_30_ID) {
            UI_HintsBuy30MoreButton();
        }
    }
    private void OnEnable() {
        NativeInApps.NativeInApp.OnItemPurchased += NativeInApp_OnItemPurchased;
    }

    private void OnDisable() {
        NativeInApps.NativeInApp.OnItemPurchased -= NativeInApp_OnItemPurchased;
    }
    public void UI_HintsBuy30MoreButton() {
        //Update Hints remaining
        PlayerPrefs.SetInt("hintsNo",PlayerPrefs.GetInt("hintsNo") + 30);
        hintsRemaining.text = PlayerPrefs.GetInt("hintsNo").ToString();

        //Update PlayerData
        GameControl.me.playerData.SaveData();
    }
    public void UI_VictoryShow() // Victory Menu
    {
        //btnSideNextLevel.gameObject.SetActive(true);
        StartCoroutine(ShowVictoryMenuDelay());
    }

    IEnumerator ShowVictoryMenuDelay()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.Debug.Log("Pulling up victory screen");
        victoryMenu.setActive(true);
    }
    #endregion

    public void ShowRateWindow() {
        if(!AlreadyRated()) {
            reviewMenu.Show();
        }

    }
    private bool AlreadyRated() {
        return (PlayerPrefs.HasKey("rated") && PlayerPrefs.GetInt("rated") == 1);
    }

}
