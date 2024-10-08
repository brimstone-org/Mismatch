﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (ScrollRect))]
public class UICenterToItem : MonoBehaviour 
{

	public float AnimTime = 0.15f;
	public bool Log = true;
	public RectTransform MaskTransform;

	[SerializeField]
	private RectTransform _content;
	[SerializeField]
	private ScrollRect _scrollRect;

	private void Start()
	{
		//_scrollRect = this.gameObject.GetComponent<ScrollRect>();
		//_content = _scrollRect.content;
	}
	
	public void CenterOnItem(RectTransform target)
	{
		if (Log) Debug.Log("Updating scrollrect for item: " + target);
		
		//this is the center point of the visible area
		var maskHalfSize = MaskTransform.rect.size*0.5f;
		var contentSize = _content.rect.size;
		//get object position inside content
		var targetRelativePosition =
			_content.InverseTransformPoint(target.parent.TransformPoint(target.anchoredPosition));
		//adjust for item size
		targetRelativePosition += new Vector3(target.rect.size.x, target.rect.size.y, 0f)*0.25f;
		//get the normalized position inside content
		var normalizedPosition = new Vector2(
			Mathf.Clamp01(targetRelativePosition.x/(contentSize.x - maskHalfSize.x)),
			1f - Mathf.Clamp01(targetRelativePosition.y/-(contentSize.y - maskHalfSize.y))
			);
		//we want the position to be at the middle of the visible area
		//so get the normalized center offset based on the visible area width and height
		var normalizedOffsetPosition = new Vector2(maskHalfSize.x / contentSize.x, maskHalfSize.y / contentSize.y);
		//and apply it
		normalizedPosition.x -= (1f -normalizedPosition.x)*normalizedOffsetPosition.x;
		normalizedPosition.y += normalizedPosition.y*normalizedOffsetPosition.y;
		
		normalizedPosition.x = Mathf.Clamp01(normalizedPosition.x);
		normalizedPosition.y = Mathf.Clamp01(normalizedPosition.y);
		
		if (Log)
			Debug.Log(string.Format(
				  @"Target normalized position [{3}]
					Mask half size [{0}]
					Content size: [{1}]
					Target relative position [{2}]",
				maskHalfSize,
				contentSize,
				targetRelativePosition,
				normalizedPosition
				));
		
		_scrollRect.normalizedPosition = normalizedPosition;
		//Go.to(_sr, AnimTime, new GoTweenConfig().vector2Prop("normalizedPosition", normalizedPosition));
	}
	
	/// <summary>
	/// Takes a float value from a [0f,1f] range and translates it to a [-1f,1f] range
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	float Transtale01RangeToMinus11Range(float value)
	{
		return (value + ((1f - value)*-1f));
	}
}
