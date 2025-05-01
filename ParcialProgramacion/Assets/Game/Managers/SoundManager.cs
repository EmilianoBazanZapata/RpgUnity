using UnityEngine;

namespace Game.Managers
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")]
        public AudioClip backgroundMusic;
        public AudioClip buttonClick;
        public AudioClip craftSound;
        public AudioClip attackSound;
        public AudioClip enemyHitSound;
        public AudioClip enemyDeathSound;
        public AudioClip playerDeathSound;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            PlayMusic(backgroundMusic);
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void SetSFXVolume(float volume) => sfxSource.volume = volume;
        public void SetMusicVolume(float volume) => musicSource.volume = volume;
    }
}