using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class UILevelButtonManager : MonoBehaviour {

    public List<UILevelButton> levelButtons;
    public Transform levelButtonPrefab;
    public bool updateList = false;
    public Sprite levelNormal;
    public Sprite levelLocked;
	public UICenterToItem scriptCenterToItem;
	

    void OnEnable()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying) return;
        if (GameControl.me == null) return;
        if (GameControl.me.levelsLoaded == false) return;
        #endif
        ViewsControl viewsControl = GameControl.me.viewsControl;
        LevelPack lp = GameControl.me.levelsLoader.LevelPacks[viewsControl.packnum];
        int i = 0;

        while (levelButtons.Count < lp.no_levels)
        {
            i++; if (i > 100) break;
            var t = Instantiate(levelButtonPrefab);
            var rt = t.gameObject.GetComponent<RectTransform>();
            var ui = t.gameObject.GetComponent<UILevelButton>();
            levelButtons.Add(ui);
            rt.SetParent(this.gameObject.GetComponent<RectTransform>(),false);
        }
        
        //Set states and remove extra buttons
        List<UILevelButton> removeButtons = new List<UILevelButton>();
        for (i = 0; i < levelButtons.Count; i++)
        {
            UILevelButton lb = levelButtons[i];
            if (i < lp.no_levelsUnlocked)  // Unlocked levels
            {
                lb.buttonGraphic.sprite = levelNormal;
                lb.buttonText.enabled = true;
                lb.buttonText.text = (i + 1).ToString();
                lb.button.interactable = true;
                lb.gameObject.SetActive(true);
            }
            else if (i < lp.no_levels) // Locked levels
            {
                lb.buttonText.enabled = false;
                lb.buttonGraphic.sprite = levelLocked;
                lb.button.interactable = false;
                lb.gameObject.SetActive(true);

            }
            else                    // Unused levels
            {
                lb.gameObject.SetActive(false);
                removeButtons.Add(lb);
            }
        }
        levelButtons.RemoveAll(x => removeButtons.Contains(x));
        foreach (var v in removeButtons) { Destroy(v.gameObject); }
        
		// Center on current level
		var centerOnItem = levelButtons [GameControl.me.viewsControl.levelnum].gameObject.GetComponent<RectTransform> ();
		scriptCenterToItem.CenterOnItem(centerOnItem);
    }
    void OnDisable()
    {
        #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            if (GameControl.me == null) return;
        #endif
        
        //Cleanup some buttons
        // ViewsControl viewsControl = GameControl.me.viewsControl;
        // LevelPack lp = GameControl.me.textLoader.LevelPacks[viewsControl.packnum];
        // List<UILevelButton> removeButtons = new List<UILevelButton>();
        // 
        // for (int i = 50; i < levelButtons.Count; i++)
        // {
        //     removeButtons.Add(levelButtons[i]);
        // }
        // 
        // levelButtons.RemoveAll(x => removeButtons.Contains(x));
        // foreach (var v in removeButtons) { Destroy(v.gameObject); }
    }
    void Update()
    {
        #if UNITY_EDITOR
        if (updateList)
        {
            updateList = false;
            UpdateList();
        }
        #endif
    }
   
    void UpdateList()
    {
        #if UNITY_EDITOR
        levelButtons.Clear();
        var buttons = GameObject.FindObjectsOfType<UILevelButton>();
        foreach (var button in buttons)
        {
            if (button.gameObject.activeInHierarchy)
            {
                levelButtons.Add(button);
            }

        }

        levelButtons = levelButtons.OrderBy(a => a.GetComponent<RectTransform>().GetSiblingIndex()).ToList();
        foreach(var lb in levelButtons)
        {
            lb.buttonGraphic.sprite = levelNormal;
            lb.buttonText.enabled = true;
            lb.buttonText.text = (lb.GetComponent<RectTransform>().GetSiblingIndex() + 1).ToString();
            lb.button.interactable = true;
            lb.gameObject.SetActive(true);
        }
        #endif
    }
    

}
