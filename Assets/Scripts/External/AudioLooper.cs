using UnityEngine;
using UnityEngine.Events;

namespace External
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioLooper : MonoBehaviour
    {
        [SerializeField] public float loopStartTime;
        [SerializeField] public float loopEndTime;

        private int loopStartSamples;
        private int loopEndSamples;
        private int loopLengthSamples;

        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

        }

        public UnityEvent OnLoop;

        private void Update()
        {
            loopStartSamples = (int)(loopStartTime * audioSource.clip.frequency);
            loopEndSamples = (int)(loopEndTime * audioSource.clip.frequency);
            loopLengthSamples = loopEndSamples - loopStartSamples;
            if (audioSource.timeSamples >= loopEndSamples)
            {
                audioSource.timeSamples -= loopLengthSamples;
                OnLoop.Invoke();
            }
        }
    }
}