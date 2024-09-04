using UnityEngine;
using System.Collections;
using System.Diagnostics;

[ExecuteInEditMode]
public class Instantiate : MonoBehaviour {

    public Transform prefab;
    public int number;
    public bool go;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (go)
        {
            go = false;
            instantiate(number);
        }
	}
    void instantiate(int num)
    {
        System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var myrt = this.gameObject.GetComponent<RectTransform>();
        for (int i = 0; i<number; i++)
        {
            var clone = GameObject.Instantiate(prefab);
            var rt = clone.gameObject.GetComponent<RectTransform>();
            rt.SetParent(myrt, false);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("instantiated " + number + " objects in: " + stopwatch.ElapsedMilliseconds + " milisecs");
        
    }
}
