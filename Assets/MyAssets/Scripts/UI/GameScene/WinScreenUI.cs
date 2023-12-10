using MyAssets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    public class WinScreenUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _nextLevelButton;
        [Header("Scene Names")]
        [SerializeField] private string _gameSceneName;
        [SerializeField] private string _mainMenuSceneName;

        private GameData _gameData;
        
        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;   
        }
        
        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        }
        
        private void OnMainMenuClicked() =>
            UnityEngine.SceneManagement.SceneManager.LoadScene(_mainMenuSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        
        private void OnNextLevelClicked()
        { 
            _gameData.SetNextLevel();
            UnityEngine.SceneManagement.SceneManager.LoadScene(_gameSceneName,
                UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        
    }
}