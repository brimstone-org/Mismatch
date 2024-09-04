using UnityEngine;
using System.Collections;

public class UIPlayButtonBack : MonoBehaviour {
    
    public void onClick()
    {
        GameControl.me.playControl.UI_GoBack();
    }
}
