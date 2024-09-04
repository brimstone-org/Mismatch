using UnityEngine;
using System.Collections;

public class UIHelpPanel : MonoBehaviour {

    public Animator HelpAnimator;
    int HelpAnimState;
	// Use this for initialization
	void Start () 
    {
        HelpAnimState = Animator.StringToHash("Base Layer.SimpleHelpAnim");
	}
	
    void OnEnable()
    {
        HelpAnimator.CrossFade(HelpAnimState, 0f, 0, 0f);
    }

}
