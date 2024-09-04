using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UIHintsMenu : MonoBehaviour {

    public UICard hint1;
    public UICard hint2;
    public Image gradient;

    public void setActive(bool active)
    {
        gradient.enabled = active;
        this.gameObject.SetActive(active);
    }
    
}
