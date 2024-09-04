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
        bDrag = true;
        DragMe(begin: true);
    }
    public void DragMeContinue()
    {
        if ( isOurFinger() )
        DragMe(begin: false);
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
        if (Input.touchCount == 0) return true;
        if (fingerId < 0) return true;

        TrackedTouch findTouch = findTouchWithID(fingerId);
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
            fingerId = Input.touches[0].fingerId;
            return Input.touches[0].position;
        }
        
        //Try the finger id first
        if (fingerId > 0)
        {
            TrackedTouch t = findTouchWithID(fingerId);
            if (t != null)
                return t.touch.position;
        }

        //Touch case multiple touches, return the closest touch
        Vector2 ObjScreenPos = (Vector2)Camera.main.WorldToScreenPoint(transform.position);

        Touch tClosest = Input.touches[0];
        Vector3 distClosest = tClosest.position - ObjScreenPos;
        float distMagMin = distClosest.sqrMagnitude;

        for (int i = 1; i < Input.touchCount; i++)
        {
            Touch t = Input.touches[i];
            Vector3 distVec = t.position - ObjScreenPos;
            float distMag = distVec.sqrMagnitude;
            if (distMag < distMagMin)
                tClosest = t;
        }
        //set the finger ID for the future
        fingerId = tClosest.fingerId;

        return tClosest.position;
    }

    TrackedTouch findTouchWithID(int fingerId)
    {
        for(int i = 0; i<Input.touchCount; i++)
        {
            if (Input.touches[i].fingerId == fingerId)
                return new TrackedTouch(Input.touches[i]);
        }
        return null;
    }
    public void StopDrag()
    {
        if (fingerId >= 0)
        {
            TrackedTouch t = findTouchWithID(fingerId);
            if (t != null)
            {
                if ((t.touch.phase == TouchPhase.Canceled) ||
                   (t.touch.phase == TouchPhase.Ended))
                {
                    fingerId = -1;
                    bDrag = false;
                }
                else
                    return;
            }
        }
        fingerId = -1;
        bDrag = false;
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