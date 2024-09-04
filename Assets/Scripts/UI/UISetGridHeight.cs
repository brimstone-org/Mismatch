using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//[ExecuteInEditMode]
/// <summary>
/// This Script will make sure the rectTransform expands with the grid layout vertically so it can be used as a Content Area for ScrollArea
/// </summary>
public class UISetGridHeight : MonoBehaviour {

    private GridLayoutGroup glg;
    private float prevHeight = 0;
    private RectTransform rt;

	// Use this for initialization
	void Start () {
        glg = this.GetComponent<GridLayoutGroup>();
        rt = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        UpdateRectTransform();
	}
    void UpdateRectTransform()
    {
        //Debug.Log(glg.preferredHeight);

        if ((prevHeight != glg.preferredHeight) && 
            (glg.preferredHeight>0))
        {
            
            //Vector3 newPos = rt.position;
            
            //Use pivot to make sure we keep the top fixed.
            Vector2 rectPivot = rt.pivot;
            rectPivot.y = 1;
            rt.pivot = rectPivot;

            Vector2 size = rt.sizeDelta;
            
            size.y = glg.preferredHeight;
            rt.sizeDelta = size;
              
            //Debug.Log("Grid Adjusted");
        }

        prevHeight = glg.preferredHeight;    
    }
}
