using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour
{

    public bool bDrag = false;
    RectTransform myRT;
    EventTrigger et;

    private Vector2 initialOffset;

    // Use this for initialization
    void Start()
    {
        myRT = this.GetComponent<RectTransform>();
        et = this.GetComponent<EventTrigger>();
        AddEventTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        //if (bDrag == true)
        //{
        //    Vector2 position;
        //    if (Input.touches.Length != 0)
        //        position = Input.touches[0].position;
        //    else
        //        position = Input.mousePosition;
        //    Vector2 pointerToWorld = (Vector2)Camera.main.ScreenToWorldPoint(position);
        //
        //    Vector2 point = (Vector2)Camera.main.ScreenToWorldPoint(position) + initialOffset;
        //    Vector2 distance = point - (Vector2)myRT.position;
        //    Debug.Log(distance);
        //
        //    myRT.position = point;
        //}

    }
    public void DragMeBegin()
    {
        DragMe(true);
    }
    public void DragMeContinue()
    {
        DragMe(false);
    }
    public void DragMe(bool begin)
    {

        bDrag = true;
        Vector2 position;
        if (Input.touches.Length != 0)
            position = Input.touches[0].position;
        else
            position = Input.mousePosition;
        Vector2 pointerToWorld = (Vector2)Camera.main.ScreenToWorldPoint(position);

        if (begin)
        {
            initialOffset = (Vector2)myRT.position - pointerToWorld;
        }
        Vector2 point = (Vector2)Camera.main.ScreenToWorldPoint(position) + initialOffset;
        
        //Vector2 distance = point - (Vector2)myRT.position;
        

        myRT.position = point;
     

    }

    public void StopDrag()
    {
        bDrag = false;
        // rigidbody2d.isKinematic = false;
    }

    public void AddEventTrigger()
    {
        if (et != null) return;

        if (et == null)
        {
            et = this.gameObject.AddComponent<EventTrigger>();
        }
        et.triggers.Clear();
      //EventTrigger.Entry entry1 = new EventTrigger.Entry();
      //entry1.eventID = EventTriggerType.Drag;
      //entry1.callback.AddListener((eventData) => { DragMeContinue(); });
      //  et.triggers.Add(entry1);
      //
      //EventTrigger.Entry entry2 = new EventTrigger.Entry();
      //entry2.eventID = EventTriggerType.PointerUp;
      //entry2.callback.AddListener((eventData) => { StopDrag(); });
      //  et.triggers.Add(entry2);
        
        EventTrigger.Entry entry3= new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.EndDrag;
        entry3.callback.AddListener((eventData) => { StopDrag(); });
        et.triggers.Add(entry3);

        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.BeginDrag;
        entry4.callback.AddListener((eventData) => { DragMeBegin(); });
        et.triggers.Add(entry4);

        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        entry5.eventID = EventTriggerType.Drag;
        entry5.callback.AddListener((eventData) => { DragMeContinue(); });
        et.triggers.Add(entry5);

        //DragMeBegin();
        //entry1.callback.Invoke( new BaseEventData(EventSystem.current));
    }
}
