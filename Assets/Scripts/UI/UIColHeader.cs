using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIColHeader : MonoBehaviour {

    public Text headerText;
  //  public bool dontSet = false;
    public void SetVisible(bool visible)
    {
        headerText.enabled = visible;
    }
    public void SetHeaderText(string titletext)
    {
       // if (dontSet) return; // Don't change Mismatch column header for now.
        headerText.text = titletext;
    }
    public void SetHeaderColor(Color color)
    {
        // if (dontSet) return; // Don't change Mismatch column header for now.
        headerText.color = color;
    }
}
