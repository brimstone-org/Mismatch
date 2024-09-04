using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRewardLoaded : MonoBehaviour
{
    public GameObject loaded;
    public GameObject notLoaded;
    public PlayControl playControl;

    public GameObject hintPanel;

    private void OnEnable()
    {
        loaded.SetActive(false);
        notLoaded.SetActive(true);
        Rewards.OnAdLoaded += Show;
        Rewards.OnAdCompleted += Hint;
        
    }

    private void Hint(string obj)
    {
        hintPanel.gameObject.SetActive(false);
        playControl.UI_HintsBuyMoreHints(1);
        playControl.UI_HintsActivate();
    }

    private void OnDisable()
    {
        Rewards.OnAdLoaded -= Show;
        Rewards.OnAdCompleted -= Hint;

    }

    private void Show(string obj)
    {
        loaded.SetActive(true);
        notLoaded.SetActive(false);
    }
}
