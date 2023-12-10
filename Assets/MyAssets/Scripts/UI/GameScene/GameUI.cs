using System;
using System.Collections;
using System.Collections.Generic;
using LeanTween.Framework;
using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using Tools.MyGridLayout;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    public class GameUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthLayout _horizontalLayout;
        [Header("Health Settings")]
        [SerializeField] private Image _healthSprite;
        [SerializeField] private Color _healthColor;
        [SerializeField] private Color _healthLostColor;
        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _pauseButton;
        [Header("Abilities")] 
        [SerializeField] private TextMeshProUGUI[] _abilitiesNames;
        [SerializeField] private TextMeshProUGUI[] _abilitiesCountText;
        [SerializeField] private Button[] _abilitiesButtons;
        [SerializeField] private Image[] _abilitiesFillImages;
        [Header("Other")]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private TextMeshProUGUI _counterText;
        [SerializeField] private TextMeshProUGUI _protonCountText;
        [SerializeField] private TextMeshProUGUI _electronCountText;
        [SerializeField] private Image _goalImage;
        [SerializeField] private TextMeshProUGUI _goalText;
        [SerializeField] private Button _protonButton;
        [SerializeField] private Button _electronButton;
        [SerializeField] private Button _shootButton;

        private readonly List<Image> _healthImages = new();
        
        private GameData _gameData;
        private int _currentHealth;
        private bool _paused;
        private bool _proton;
        private int _currentElectronsCount;
        private int _currentProtonsCount;
        private SoundManager _soundManager;

        [Inject]
        private void Construct(GameData gameData, SoundManager soundManager)
        {
            _gameData = gameData;
            _soundManager = soundManager;
            _currentElectronsCount = _gameData.CurrentLevel.LevelSO.ElectronsCount;
            _currentProtonsCount = _gameData.CurrentLevel.LevelSO.ProtonsCount;
            _protonCountText.text = _currentProtonsCount.ToString();
            _electronCountText.text = _currentElectronsCount.ToString();
            _goalImage.sprite = _gameData.CurrentLevel.LevelSO.GoalSprite;
            _goalText.text = _gameData.CurrentLevel.LevelSO.Goal;
            /*_gameData.Score.OnScoreUpdated += OnScoreChanged;
            _currentHealth = _gameData.PlayerSO.MaxHealth;
            if(_abilitiesButtons.Length != _gameData.Abilities.Length)
                throw new System.Exception("Abilities buttons count must be equal to abilities count");
            for (var i = 0; i < _gameData.Abilities.Length; i++)
            {
                _abilitiesNames[i].text = _gameData.Abilities[i].Ability.Name;
                _abilitiesCountText[i].text = "x" + _gameData.Abilities[i].Count;
                _abilitiesButtons[i].interactable = _gameData.Abilities[i].Count > 0;
                _abilitiesButtons[i].onClick.AddListener(() => OnAbilityUse(_gameData.Abilities[i]));
            }
            for (var i = 0; i < _currentHealth; i++)
            {
                var image = Instantiate(_healthSprite, _horizontalLayout.transform);
                image.color = _healthColor;
                _healthImages.Add(image);
            }
            _horizontalLayout.Align(_currentHealth);*/
        }
        

        private void Awake()
        {
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _shootButton.onClick.AddListener(OnShootButtonClicked);
            _protonButton.onClick.AddListener(OnProtonButtonClicked);
            _electronButton.onClick.AddListener(OnElectronButtonClicked);
            _counterText.text = "0";
        }

        private void OnDestroy()
        {
            _gameData.Score.OnScoreUpdated -= OnScoreChanged;
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            _shootButton.onClick.RemoveListener(OnShootButtonClicked);
            _protonButton.onClick.RemoveListener(OnProtonButtonClicked);
            _electronButton.onClick.RemoveListener(OnElectronButtonClicked);
        }
        
        private void OnScoreChanged(int score)
        {
            _counterText.text = score.ToString();
            LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
        }

        private void OnPauseButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _paused = !_paused;
            Time.timeScale = _paused ? 0 : 1;
        }

        private void OnHomeButtonClicked()
        { 
            _soundManager.PlayButtonClick();
            Time.timeScale = 1;
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
        }
        
        private void OnPlayerHit()
        {
            if(_currentHealth <= 0) return;
            _currentHealth--;
            _healthImages[_currentHealth].color = _healthLostColor;
        }

        private void OnAbilityUse(AbilityData ability)
        {
            if(ability.Count <= 0) return;
            StartCoroutine(AbilityCooldown(_abilitiesFillImages[ability.Ability.ID], 
                _abilitiesButtons[ability.Ability.ID], ability.Ability.Cooldown));
            _abilitiesCountText[ability.Ability.ID].text = "x" + --ability.Count;
        }

        private IEnumerator AbilityCooldown(Image fill,Button button, float duration)
        {
            var time = 0f;
            button.interactable = false;
            while (time < duration)
            {
                fill.fillAmount = time / duration;
                time += Time.deltaTime;
                yield return null;
            }
            button.interactable = true;
        }

        private void OnShootButtonClicked()
        {
            if (_proton)
            {
                if (_currentProtonsCount <= 0) return;
                _currentProtonsCount--;
                _protonCountText.text = _currentProtonsCount.ToString();
            }
            else
            {
                if (_currentElectronsCount <= 0) return;
                _currentElectronsCount--;
                _electronCountText.text = _currentElectronsCount.ToString();
            }
        }

        private void OnElectronButtonClicked()
        {
            _proton = false;
        }

        private void OnProtonButtonClicked()
        {
            _proton = true;
        }
    }
}