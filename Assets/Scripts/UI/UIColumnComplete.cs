using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIColumnComplete : MonoBehaviour {

    public static string[] messages = new string[]{"GREAT!","GOOD WORK!","NICELY DONE!","COLUMN COMPLETE!","WELL DONE!","VERY GOOD!"};
    public GameObject graphics;
    public Text completedText;

    public bool animEnded;
	void Awake()
	{
		messages = new string[]{ LanguageManager.instance.Get("great"), LanguageManager.instance.Get("good_work"),
			LanguageManager.instance.Get("nicely_done"), LanguageManager.instance.Get("column_complete"), LanguageManager.instance.Get("well_done"), LanguageManager.instance.Get("very_good") };
	}
    public void SetEnabled(bool enabled)
    {
        messages = new string[]{ LanguageManager.instance.Get("great"), LanguageManager.instance.Get("good_work"),
            LanguageManager.instance.Get("nicely_done"), LanguageManager.instance.Get("column_complete"), LanguageManager.instance.Get("well_done"), LanguageManager.instance.Get("very_good") };
        CompleteAnimator.enabled = enabled;

        if (enabled && !graphics.activeSelf)
        {
            int i = Random.Range(0, messages.Length);
            completedText.text = messages[i];

            animEnded = false;
            CompleteAnimator.CrossFade(ColumnCompleteAnimState, 0f, 0, 0f);
            CompleteAnimator.speed = 1;
        }

        if (!enabled)
        {
            CompleteAnimator.speed = 0;
            CompleteAnimator.CrossFade(ColumnCompleteAnimState, 0f, 0, 0f);
        }
        graphics.SetActive(enabled);

  
    }
    public void Update()
    {
        if (CompleteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            animEnded = true;
        else
            animEnded = false;
    }
    public Animator CompleteAnimator;
    int ColumnCompleteAnimState;
    // Use this for initialization
    void Start()
    {
        ColumnCompleteAnimState = Animator.StringToHash("Base Layer.ColumnComplete");
        CompleteAnimator.speed = 0;
    }

}
