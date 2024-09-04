using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class ObjectFinder : MonoBehaviour {
    public bool go;
    public GameObject[] objects;
 
	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
	    if (go)
        {
            go = false;
            objects = (GameObject[])Resources.FindObjectsOfTypeAll( typeof(GameObject) );
            objects = objects.OrderBy(x => x.name).ToArray();
        }
	}
   
}
