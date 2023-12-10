using MyAssets.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class OptionsUI: MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;
        [Header("Other")]
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private Button _backButton;
        
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundVolumeKey = "SoundVolume";
        
        private SoundManager _soundManager;
        
        [Inject]
        private void Construct(SoundManager soundManager)
        {
            Debug.Log("OptionsUI Construct");
            _soundManager = soundManager;
            var musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
            var effectsVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1);
            SetMusic(musicVolume);
            SetSound(effectsVolume);
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _optionsPanel.SetActive(false);
        }
        
        private void OnMusicVolumeChange(float value) =>
            SetMusic(value);

        private void OnSoundVolumeChange(float value) =>
            SetSound(value);

        private void SetMusic(float value)
        {
            _soundManager.SetMusicVolume(value);
            _musicSlider.value = value;
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        private void SetSound(float value)
        {
            _soundManager.SetSoundVolume(value);
            _soundSlider.value = value;
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
        }
    }
}