using Game.Shared.Enums;
using UnityEngine;

namespace Game.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Audio Sources")] [SerializeField]
        private AudioSource musicSource;

        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")] public AudioClip backgroundMusic;
        public AudioClip optionsMusic;
        public AudioClip gameOverMusic;
        public AudioClip victoryMusic;
        public AudioClip buttonClick;
        public AudioClip craftSound;
        public AudioClip attackSound;
        public AudioClip enemyHitSound;
        public AudioClip enemyDeathSound;
        public AudioClip playerDeathSound;
        public AudioClip errorCraftSound;

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
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            HandleGameStateChanged(GameManager.Instance.CurrentState);
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void PlaySound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.BackgroundMusic:
                    PlayMusic(backgroundMusic);
                    break;
                case SoundType.ButtonClick:
                    PlaySFX(buttonClick);
                    break;
                case SoundType.Craft:
                    PlaySFX(craftSound);
                    break;
                case SoundType.Attack:
                    PlaySFX(attackSound);
                    break;
                case SoundType.EnemyHit:
                    PlaySFX(enemyHitSound);
                    break;
                case SoundType.EnemyDeath:
                    PlaySFX(enemyDeathSound);
                    break;
                case SoundType.PlayerDeath:
                    PlaySFX(playerDeathSound);
                    break;
                case SoundType.ErrorCraft:
                    PlaySFX(errorCraftSound);
                    break;
                case SoundType.OptionMusic:
                    PlayMusic(optionsMusic);
                    break;
            }
        }
        
        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.GameOver:
                    PlayMusic(gameOverMusic);
                    break;
                case GameState.Victory:
                    PlayMusic(victoryMusic);
                    break;
                case GameState.InGame:
                    PlayMusic(backgroundMusic);
                    break;
            }
        }

        public void PauseMusic()
        {
            if (musicSource.isPlaying)
                musicSource.Pause();
        }

        public void ResumeMusic()
        {
            if (!musicSource.isPlaying)
                musicSource.UnPause();
        }

        public void SetSFXVolume(float volume) => sfxSource.volume = volume;
        public void SetMusicVolume(float volume) => musicSource.volume = volume;
        public void PlayButtonEffect() => PlaySFX(buttonClick);
        public void StartBackgroundMusic() => PlayMusic(backgroundMusic);
    }
}