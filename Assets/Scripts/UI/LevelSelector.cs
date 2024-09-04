using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

  
    public void OnClick()
    {
        GameControl.me.viewsControl.SwitchLevel( this.GetComponent<RectTransform>().GetSiblingIndex() );
        GameControl.me.viewsControl.SwitchView(GameView.PlayGame);
    }

}
