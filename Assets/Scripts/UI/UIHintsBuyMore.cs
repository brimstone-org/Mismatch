using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIHintsBuyMore : MonoBehaviour {

    public Image gradient;
    public Text buy10MorePrice;
	public Text buy30MorePrice;
    public void setActive(bool active)
    {
        gradient.enabled = active;
        gameObject.SetActive(active);
        if (active == true)
        {
            buy10MorePrice.text = NativeInApps.NativeInApp.Instance.GetLocalizedPrice(NativeInApps.NativeInApp_IDS.HINTS_10_ID);
            buy30MorePrice.text = NativeInApps.NativeInApp.Instance.GetLocalizedPrice(NativeInApps.NativeInApp_IDS.HINTS_30_ID);
        }
    }
    public void buy10More()
    {
        //GameControl.me.playControl.NativeInApp_OnItemPurchased(NativeInApps.NativeInApp_IDS.HINTS_10_ID);
    }

	public void buy30More()
	{
		//GameControl.me.playControl.NativeInApp_OnItemPurchased(NativeInApps.NativeInApp_IDS.HINTS_30_ID);
	}
}
