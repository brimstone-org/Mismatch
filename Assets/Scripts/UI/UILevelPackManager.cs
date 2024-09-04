using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UILevelPackManager: MonoBehaviour {

    public static UILevelPackManager instance = null;
    public Color easyColor;
    public Color mediumColor;
    public Color hardColor;
    public string easyString = "EASY";
    public string mediumString = "MEDIUM";
    public string hardString = "HARD";


    void Awake() {
        //Check if instance already exists
        if(instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if(instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
      //  DontDestroyOnLoad(gameObject);
    }
    public List<UILevelPack> levelPacksUI = new List<UILevelPack>();

    public void UpdateLevelPacks() {

        if(GameControl.me == null) return;
        if(!GameControl.me.levelsLoaded) return;

        List<LevelPack> levelpacks = GameControl.me.levelsLoader.LevelPacks;

        for(int i = 0;i < levelPacksUI.Count;i++) {
            UILevelPack packCard = levelPacksUI[i];

            //Check if levelpack can be found for this level pack card, disable if not found
            bool PackWasFound = i < levelpacks.Count;
            packCard.gameObject.SetActive(PackWasFound);
            if(!PackWasFound) continue;

            // Get the levelpack info and set card to match
            LevelPack levelpack = GameControl.me.levelsLoader.LevelPacks[i];
            string name = levelpack.name;
            Difficulty diff = levelpack.difficulty;
            switch(diff) {
                case Difficulty.easy:
                    packCard.setDifColor(easyColor);
                    packCard.setDifText(LanguageManager.instance.Get(easyString.ToLower())); break;
                case Difficulty.medium:
                    packCard.setDifColor(mediumColor);
                    packCard.setDifText(LanguageManager.instance.Get(mediumString.ToLower())); break;
                case Difficulty.hard:
                    packCard.setDifColor(hardColor);
                    packCard.setDifText(LanguageManager.instance.Get(hardString.ToLower())); break;
            }
            packCard.setPackname(name);


            //Buyable Levelpacks
            bool PackPurchase = false;
            if(levelpack.soomlaID != "free" && levelpack.soomlaID != "")
                if(!(levelpack.soomlaID == "com.tedrasoft.mismatched.winterpack" && PlayerPrefs.GetInt("winterPack") == 1)) {
                    PackPurchase = true;
                }

            packCard.purchaseable = PackPurchase;
            packCard.levelPack = levelpack;

            packCard.buyImage.gameObject.SetActive(PackPurchase ? true : false);
            packCard.buyImage.enabled = PackPurchase ? true : false;
            packCard.price.gameObject.SetActive(PackPurchase ? true : false);
            packCard.price.enabled = PackPurchase ? true : false;
            //packCard.price.text = PackPurchase? soomlaManager.getPriceByItemId(levelpack.soomlaID) : "";
        }
    }
}
