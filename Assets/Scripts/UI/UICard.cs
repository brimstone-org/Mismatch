using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UICard : MonoBehaviour {

    public Image frame;
    public Image picture;
    public Image check;
    public Text cardText;
    public AnimationDrop animDrop;
    public static Color ColorNormal = Color.white;
    public static Color ColorFaded = new Color(0.273f, 0f, 0f, 0.86f);

    public bool clone = false;
    public bool isPictureNull = false;
    public CardMode cardMode = CardMode.image;
    public DragUIPassive dragUI;
    public bool isDropping = false;

    
    public void Awake()
    {
        if (animDrop == null)  animDrop = picture.GetComponent<AnimationDrop>();
        if (dragUI == null) dragUI = this.GetComponent<DragUIPassive>();
    }


    public void setPicSprite(Sprite sprite)      { setSprite(picture, sprite); }
    public void setFrameSprite(Sprite sprite)    { setSprite(frame, sprite);   }
    void setSprite(Image image, Sprite sprite)
    {
        if (sprite != null)
        {
            isPictureNull = false;
            image.enabled = true;
            image.sprite = sprite;
        }
        else
        {
            isPictureNull = true;
            
            //Just switch to text
            image.enabled = false;
            cardMode = CardMode.text;
        }

    }
    public void setPicVisible(bool visible) { picture.enabled = visible; } // SaveCardState();
    public void setTextVisible(bool visible) { cardText.enabled = visible; } // SaveCardState();
    public void setCardText(string text)   {        cardText.text = text;    }
    public void setFrameColor(Color color)    {        frame.color = color;    }
    public void setPictureColor(Color color) { picture.color = color; }
    public void setPicAndTextColor(Color color) { picture.color = color; cardText.color = color; }
    public void setFrameVisbile(bool visible) { frame.enabled = visible; check.enabled = visible; } // SaveCardState();
    public void setCardMode(CardMode mode)
    {
        cardMode = mode;
        switch(cardMode)
        {
            case CardMode.image:
                    if (!isPictureNull)
                    {
                        setTextVisible(false);
                        setPicVisible(true);
                        break;
                    }
                    else goto case CardMode.text;
            case CardMode.text:
                    setTextVisible(true);
                    setPicVisible(false);
                    break;
            default:setTextVisible(true);
                    setPicVisible(false);
                    break;
        }
    }

    #region Animation
    public void PlayIntroAnim()
    {
        isDropping = true;
        animDrop.PlayAnimationDrop(IntroAnimEnded);
        //cardText.GetComponent<AnimationDrop>().PlayAnimationDrop();
    }
    public void IntroAnimEnded()
    {
        isDropping = false;
    }
    #endregion

    #region Drag Handling
    public void CloneCard()
    {
        //This function will replace us with a clone of ourselves. We will become the true clone.
        if (clone == true) return;

        //Can only drag one card at a time.
        if (GameControl.me.playControl.lastDragged != null)
            return;

        GameObject go = (GameObject)Instantiate(this.gameObject);
       
        //go.GetComponent<EventTrigger>().triggers.Clear();
        //go.AddComponent<DragUI>();
        clone = true;

        var myRT = this.gameObject.GetComponent<RectTransform>();
        var newRT = go.GetComponent<RectTransform>();
        var parentRT = GameControl.me.playControl.cloneParent.GetComponent<RectTransform>();
        
        int index = myRT.GetSiblingIndex();
        var position = myRT.position;
        var pivot = myRT.pivot;
        var localScale = myRT.localScale;
        var sizeDelta = myRT.sizeDelta;
        var oldparent = myRT.parent;
        var myname = gameObject.name;

        //Unparent self but keep position
        myRT.SetParent(parentRT, false);
        myRT.pivot = pivot;
        myRT.localScale = localScale;
        myRT.position = position;
        myRT.sizeDelta = sizeDelta;
        this.gameObject.name = "Clone!";


        //Setup our replacement
        newRT.SetParent(oldparent, false);
        newRT.SetSiblingIndex(index);

        newRT.position = myRT.position;
        newRT.pivot = myRT.pivot;
        newRT.localScale = myRT.localScale;
        newRT.sizeDelta = myRT.sizeDelta;
        go.name = myname;

        // Update the playcontrol reference
        var selectedCard = go.GetComponent<UICard>();
        GameControl.me.playControl.ReplaceCardReferenceInAll(this, selectedCard);
        GameControl.me.playControl.lastDragged = this;
        GameControl.me.playControl.lastSelected = selectedCard;
    }
    public void setActive(bool active)
    {
        frame.gameObject.SetActive(active);
        picture.gameObject.SetActive(active);
        check.gameObject.SetActive(active);
        cardText.gameObject.SetActive(active);
    }

    public void DragStart()
    {
        //Debug.Log("Drag Try Start");

        //Can't drag while anim is playing
        if (isDropping)
            return;
        
        CloneCard();
        
        //we can only drag clones
        if (!clone) return;
        //Debug.Log("Drag Started");
        dragUI.DragMeBegin();
    }
    public void Drag()
    {
        //Can't drag while anim is playing
        if (isDropping) 
            return;
        //we can only drag clones
        if (!clone) return;

        //Debug.Log("Drag Continues");
        dragUI.DragMeContinue();
    }
    public void DragEnd()
    {
        //Can't drag while anim is playing
        if (isDropping)
            return;
        //we can only drag clones
        if (!clone) return;

        //Debug.Log("Drag Ends");
        dragUI.StopDrag();
    }
    #endregion
}

public enum CardMode
{
    text = 1,
    image = 2
}
