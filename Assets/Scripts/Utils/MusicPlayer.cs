using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mismatch.Utils
{
    public class MusicPlayer : MonoBehaviour
    {
        public List<Utils.Sound> sounds;

        public static MusicPlayer Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            Utils.SoundManager.Instance.PlayBackgroundMusic(sounds[0].clip, Vector3.zero);
        }


        //private void OnLevelWasLoaded(int level)
        //{
        //    Start();
        //}
    }

}
