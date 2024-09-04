using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedLoadingScript : MonoBehaviour {
    public Text loadingText;
    int itr = 0;
	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateText",0,500);	
	}

	
    void UpdateText(){
        loadingText.text = "LOADING";

        for (int i = 0; i <= itr % 3; i++)
            loadingText.text = loadingText.text + ".";

        itr++;
    }
}
