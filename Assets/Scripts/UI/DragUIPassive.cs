using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

public class DragUIPassive : MonoBehaviour
{

    public bool bDrag = false;
    private Vector2 initialOffset;
    public int fingerId = -1;

    public void DragMeBegin()
    {

        if (bDrag == true) // Do not allow other touches to drag this object if the previous drag hasn't ended.
        {
            return;
        }
        ////Debug.Log("Drag Begin");
        bDrag = true;
        DragMe(begin: true);
        //Debug.Log("DragUIPassive: Drag Started");
    }
    public void DragMeContinue()
    {
        //for (int i = 0; i < Input.touchCount;i++ )
        //    Debug.Log(Input.GetTouch(i).fingerId);

        if (bDrag == false) 
            return;

        if (isOurFinger())
        {
            //Debug.Log("Drag Continue" + fingerId);
            DragMe(begin: false);
            //Debug.Log("DragUIPassive: Drag Continues");
        }

    }
    public void DragMe(bool begin)
    {
        if (bDrag == false) return; //Must be some interference. The drag hasn't started.

        Vector2 position = GetTouchOrMousePosition();
        Vector2 pointerToWorld = (Vector2)Camera.main.ScreenToWorldPoint(position);

        if (begin)
        {
            //Setup initial offset
            initialOffset = (Vector2)transform.position - pointerToWorld;
        }
        Vector2 point = (Vector2)Camera.main.ScreenToWorldPoint(position) + initialOffset;
        //Vector2 distance = point - (Vector2)transform.position;

        transform.position = point;
    }
    public bool isOurFinger()
    {

        if (Input.touchCount == 0) return true; // Bypass for no touches on screens
        if (fingerId < 0) return true;  // No finger id set yet

        TrackedTouch findTouch = getTouchFresh(fingerId);
        if (findTouch == null)
            return true;

        if (findTouch.touch.fingerId == fingerId)
            return true;

        return false;
    }
    public Vector2 GetTouchOrMousePosition()
    {
        //Mouse case
        if (Input.touchCount == 0)
            return Input.mousePosition;
        
        //Single touch
        if (Input.touchCount == 1)
        {
            fingerId = Input.GetTouch(0).fingerId;
            return Input.GetTouch(0).position;
        }
        
        //Try the finger id first
        TrackedTouch currentTouch = getTouchFresh(fingerId);
        if (currentTouch != null)
            return currentTouch.touch.position;
        
        //Touch case multiple touches, return the closest touch
        Vector2 ObjScreenPos = (Vector2)Camera.main.WorldToScreenPoint(transform.position);

        Touch tClosest = Input.GetTouch(0);
        Vector3 distClosest = tClosest.position - ObjScreenPos;
        float distMagMin = distClosest.sqrMagnitude;

        for (int i = 1; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            Vector3 distVec = t.position - ObjScreenPos;
            float distMag = distVec.sqrMagnitude;
            if (distMag < distMagMin)
                tClosest = t;
        }
        //set the finger ID for the future
        fingerId = tClosest.fingerId;

        return tClosest.position;
    }

    public void StopDrag()
    {
        if (bDrag == false) return; // Something already ended the drag
       
        TrackedTouch t = getTouchOld(fingerId);
        if (t != null)
        {
                if ((t.touch.phase == TouchPhase.Canceled) ||
                   (t.touch.phase == TouchPhase.Ended))
                {
                    fingerId = -1;
                    bDrag = false;
                    //Debug.Log("Drag Ended "+ this.gameObject.name);
                    return;
                }
                else
                    return;
        }
        fingerId = -1;
        bDrag = false;
        //Debug.Log("DragUIPassive: Drag Ended");
     
    }
    TrackedTouch getTouchOld(int fingerId)
    {
        if (fingerId < 0) return null;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].fingerId == fingerId)
                return new TrackedTouch(Input.touches[i]);
        }
        return null;
    }
    public TrackedTouch getTouchFresh(int fingerId)
    {
        if (fingerId < 0) return null;
        TrackedTouch newTouch = null;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.fingerId == fingerId)
            {
                newTouch = new TrackedTouch(t);
                break;
            }
        }
        if (newTouch == null)
            return new TrackedTouch( Input.GetTouch(0) );
        return newTouch;
    }

}

[System.Serializable]
public class TrackedTouch
{
    public Touch touch;
    public TrackedTouch(Touch newTouch)
    {
        touch = newTouch;
    }
}