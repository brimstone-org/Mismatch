using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRefresher : MonoBehaviour {
    [SerializeField]
    GameObject[] canvasObjectsToRefresh;


	// Use this for initialization
	void Start () {
        foreach (GameObject GO in canvasObjectsToRefresh)
        {
            GO.SetActive(false);
            GO.SetActive(true);

        }
	}
	
}
