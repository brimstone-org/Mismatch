using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LOADDELOAD : MonoBehaviour {
    WaitForSeconds x = new WaitForSeconds(0.2f);
    AsyncOperation op;
	// Use this for initialization
	void Start () {
        StartCoroutine(GETINLOADEDSCENE());
        //Invoke("GoLoad",200);
	}


   /* void GoLoad(){
        SceneManager.LoadScene("FullGameStd");
    }*/

    IEnumerator GETINLOADEDSCENE(){
        yield return x;
        op = SceneManager.LoadSceneAsync("FullGameStd");

          while(!op.isDone)
          {
              yield return null;
          }
        yield return null;
    }
	
	
}
