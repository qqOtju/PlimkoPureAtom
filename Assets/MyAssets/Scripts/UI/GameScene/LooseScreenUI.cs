using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    [RequireComponent(typeof(Canvas))]
    public class LooseScreenUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _skipLevelButton;
        [Header("Scene Names")]
        [SerializeField] private string _gameSceneName;
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private GameData _gameData;
        
        [Inject]
        private void Construct(GameData gameData)
        { 
            _gameData = gameData;
            if(_gameData.Gold.GoldCount < 2)
                _skipLevelButton.interactable = false;
            if(_gameData.CurrentLevel.LevelState == LevelState.Completed)
                _skipLevelButton.gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            _skipLevelButton.onClick.AddListener(OnSkipLevelClicked);
        }

        private void OnSkipLevelClicked()
        {
            if (!_gameData.SkipCurrentLevel()) return;
            _gameData.SetNextLevel();
            OnRestartClicked();
        }

        private void OnMainMenuClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
        }

        private void OnRestartClicked() =>
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);

        public void Init(string description)
        {
            _descriptionText.text = description;
        }
    }
}