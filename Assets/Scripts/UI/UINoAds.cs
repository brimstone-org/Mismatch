using UnityEngine;
using System.Collections;

public class UINoAds : MonoBehaviour {

    public void UpdateNoAdsButton()
    {
        bool isActive = PlayerPrefs.GetInt("noAds",0)==0 ? true : false;
        this.gameObject.SetActive(isActive);
    }

    public void NativeInApp_OnItemPurchased(string SKU) 
    {
        if (SKU==NativeInApps.NativeInApp_IDS.NO_ADS_ID) 
        {
            PlayerPrefs.SetInt("noAds",1);
        }
    }

    private void OnEnable() {
        NativeInApps.NativeInApp.OnItemPurchased += NativeInApp_OnItemPurchased;
    }

    private void OnDisable() {
        NativeInApps.NativeInApp.OnItemPurchased -= NativeInApp_OnItemPurchased;
    }

    private void Update() {
        UpdateNoAdsButton();
    }

}
