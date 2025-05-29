using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance
        {
            get { return _instance; }
        }
        private static AudioManager _instance;
        [SerializeField] private float musicVolume = 0.75f;

        private AudioSource _music1;
        private AudioSource _music2;
        private AudioSource _sfxSource;
        
        private bool _isFirstMusicActive = true;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
              
            DontDestroyOnLoad(this);
            
            _music1 = gameObject.AddComponent<AudioSource>();
            _music2 = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();
            
            _music1.loop = true;
            _music2.loop = true;
        }

        public void PlaySfx(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }
        public void PlaySfx(AudioClip clip, float volume, bool pitchRandomize = false)
        {
            if (pitchRandomize)
            {
                _sfxSource.pitch = Random.Range(0.9f, 1.1f);
            }
            else
            {
                _sfxSource.pitch = 1;
            }
            _sfxSource.PlayOneShot(clip, volume);
        }
        
        public void PlayMusicWithFade(AudioClip clip, float transitionTime = 1.0f)
        {
            AudioSource activeSource = (_isFirstMusicActive) ? _music1 : _music2;
            AudioSource newSource = (_isFirstMusicActive) ? _music2 : _music1;
            _isFirstMusicActive = !_isFirstMusicActive;
            // newSource.clip = clip;
            // newSource.Play();
            StartCoroutine(UpdateMusicWithFade(activeSource, newSource, clip, transitionTime));
        }

        private IEnumerator UpdateMusicWithFade(AudioSource original, AudioSource newSource, AudioClip music,
            float transitionTime)
        {
            if (!original.isPlaying)
            {
                original.Play();
            }
            
            newSource.Stop();
            newSource.clip = music;
            newSource.Play();
            float t = 0f;
            
            while (t <= transitionTime)
            {
                original.volume = musicVolume - ((t/transitionTime) * musicVolume);
                newSource.volume = (t / transitionTime) * musicVolume;
                t += Time.deltaTime;
                yield return null;
            }
            
            original.volume = 0;
            newSource.volume = musicVolume;
            original.Stop();
        }
    }
}
