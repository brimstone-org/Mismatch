using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mismatch.Utils
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Mismatch/Sound")]
    public class Sound : ScriptableObject
    {
        public AudioClip clip;
    }

}
