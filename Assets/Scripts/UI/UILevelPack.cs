using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NativeInApps;

public class UILevelPack: MonoBehaviour {

    public Text difText;
    public Text packName;
    public Text price;
    public Image buyImage;
    public bool purchaseable = false;
    public LevelPack levelPack = null;

    public void setPackname(string name) {
        //Debug.Log (name);
        //packName.text = name;
        packName.text = LanguageManager.instance.Get(name.ToLower());
    }
    public void setDifText(string difString) {
        difText.text = difString.ToUpper();
    }
    public void setDifColor(Color difColor) {
        difText.color = difColor;
    }
    public void onClick() {
        Debug.Log("OnClick");
        //LevelPack lp = GameControl.me.levelsLoader.LevelPacks[index];
        if(purchaseable==false)
          //  buyLevelPack(levelPack);
       // else
            enterLevelPack();
    }

    void enterLevelPack() {
        int index = this.GetComponent<RectTransform>().GetSiblingIndex();

        //Start Imagepack preload for this level pack
        string imagepack = levelPack.imagepack;
        GameControl.me.imageManager.ImageAtlasPreload(imagepack);

        //Switch the view
        GameControl.me.viewsControl.SwitchPack(index);
        GameControl.me.viewsControl.SwitchView((int)GameView.LevelSelect); //this will also release unused assets
    }
    void buyLevelPack(LevelPack lp) {
       // wrapper.Buy(lp.soomlaID);
    }

    public void NativeInApp_OnPackPurchased(string sku) {

        if(sku == NativeInApp_IDS.WINTER_PACK_ID) 
        {
            PlayerPrefs.SetInt("winterPack",1);
            DisablePack();  
        }
    }

    public void DisablePack() 
    {
        this.purchaseable = false;
        UILevelPackManager.instance.UpdateLevelPacks();
    }

    public void RefreshPack() 
    {
        int enabled = PlayerPrefs.GetInt(NativeInApp_IDS.WINTER_PACK_ID);
        if (enabled==1) 
        {
            PlayerPrefs.SetInt("winterPack",1);
            DisablePack();
        }

    }

    public void OnEnable() {
        NativeInApp.OnRefreshCompleted += RefreshPack;
        NativeInApp.OnItemPurchased += NativeInApp_OnPackPurchased;
       
    }

    private void Update() {
        RefreshPack();
    }
}

