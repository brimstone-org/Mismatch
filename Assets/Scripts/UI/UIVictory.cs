using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

public class UIVictory : MonoBehaviour
{

    public Image gradient;
    public Text description;
    public Text title;
    private string baseDescription;
    public GameObject NextLevelButton;

    void Awake()
    {
        baseDescription = LanguageManager.instance.Get("complete_level");

    }

    public void setActive(bool active)
    {
        gradient.enabled = active;
        this.gameObject.SetActive(active);
        // if (active) initialize(); 
    }
    public void nextLevel()
    {
        GameControl.me.playControl.UI_LoadNextLevel();
    }
    public void goBack()
    {
        GameControl.me.playControl.UI_GoBack();
    }
    public void setTexts(string titleText, string bodyText)
    {
        description.text = bodyText;
        title.text = titleText;
    }
    public void initialize(string title, string body)
    {
        // string level = (GameControl.me.viewsControl.levelnum+1).ToString();
        // string desc = baseDescription.Replace("%levelnumber%", level);
        setTexts(title, body);
        gradient.enabled = true;
        this.gameObject.SetActive(true);
    }

}
