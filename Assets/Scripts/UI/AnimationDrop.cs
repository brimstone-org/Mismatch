using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnimationDrop : MonoBehaviour {

    Vector3 startLocalPos;
    RectTransform objectRC;
    public float distance = 1000f;
    public float time = 1f;
    public Ease animEase = Ease.OutQuad;
    private Tween anim;
	// Use this for initialization
    void Awake()
    {
        objectRC = this.GetComponent<RectTransform>();
        startLocalPos = objectRC.localPosition;
    }
	
    public void PlayAnimationDrop()
    {
        PlayAnimationDrop(null);
    }
    public void PlayAnimationDrop(TweenCallback onDone)
    {
        float delay = 0;// UnityEngine.Random.Range(0f, 0.2f);
        // Tween a Vector3 called myVector to 3,4,8 in 1 second

        if (anim != null)
            anim.Kill();

        objectRC.localPosition = (objectRC.anchoredPosition3D + new Vector3(0f, distance, 0f));
        anim = DOTween.To(() => objectRC.localPosition, x => objectRC.localPosition = x, startLocalPos, time)
            .SetEase(animEase)
            .SetDelay(delay)
            .SetId(anim)
            .OnKill(onDone);
        
    }
}
