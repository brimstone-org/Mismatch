using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class UIHintsShare : MonoBehaviour {

    private RectTransform rc;
    private Vector3 localPos;
    public void Awake()
    {
        rc = this.gameObject.GetComponent<RectTransform>();
        localPos = rc.localPosition;
    }
    public void OnEnable()
    {

        HideCheck();
        FacebookManager.OnShareBonus += HideCheck;

    }
    public void OnDisable()
    {
        FacebookManager.OnShareBonus -= HideCheck;
    }
    public void HideCheck()
    {
        string date = DateTime.Now.ToString("dd/MM/yyyy");
        if (date == GameControl.me.playerData.GetPlayerDataString("LastShareDate"))
            rc.localPosition = localPos + new Vector3(0, 2000, 0);
        else
            rc.localPosition = localPos;
    }
    public void shareOnFacebook()
    {
        GameControl.me.facebookManager.ShareLink();
    }
}
