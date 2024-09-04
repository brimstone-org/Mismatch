using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public String SceneToLoad;
    public void Load()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
