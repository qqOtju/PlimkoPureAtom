using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class LevelsUI: MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _levelsPanel;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private Button[] _levelButtons;
        [SerializeField] private TextMeshProUGUI[] _levelNameTexts;
        [SerializeField] private Image[] _levelImages;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private Color _unlockedColor;
        [SerializeField] private Color _completedColor;
        
        private int _currentPage = 0;
        private LevelData _lastLevelData;
        private GameData _gameData;
        private SoundManager _soundManager;
        
        [Inject]
        private void Construct(GameData gameData, SoundManager soundManager)
        {
            _gameData = gameData;
            _soundManager = soundManager;
        }
        
        private void Awake()
        {
            _nextPageButton.onClick.AddListener(OnNextPageClicked);
            _prevPageButton.onClick.AddListener(OnPrevPageClicked);
            _backButton.onClick.AddListener(OnBackClicked);
            UpdateButtons();
        }

        private void OnDestroy()
        {
            _nextPageButton.onClick.RemoveListener(OnNextPageClicked);
            _prevPageButton.onClick.RemoveListener(OnPrevPageClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
        }
        
        private void OnBackClicked()
        {
            _soundManager.PlayButtonClick();
            _levelsPanel.SetActive(false);
            _mainPanel.SetActive(true);
        }

        private void OnPrevPageClicked()
        {
            _soundManager.PlayButtonClick();
            if(_currentPage == 0) 
                _currentPage = 10;
            else
                _currentPage--;
            UpdateButtons();
        }

        private void OnNextPageClicked()
        {
            _soundManager.PlayButtonClick();
            if(_currentPage == 10) 
                _currentPage = 0;
            else
                _currentPage++;
            UpdateButtons();
        }
        
        private void UpdateButtons()
        {
            for (var i = 0; i < _levelButtons.Length; i++)
            {
                var level = _currentPage * 8 + i;
                LevelData lvlData = null;
                foreach (var lvl in _gameData.Levels)
                {
                    if(lvl.LevelSO.ID == level)
                        lvlData = lvl;                       
                }

                if (lvlData == null)
                    lvlData = new LevelData(_lastLevelData.LevelSO);
                else
                    _lastLevelData = lvlData;
                if(lvlData.LevelState == LevelState.Locked)
                {
                    _levelButtons[i].interactable = false;
                    _levelImages[i].gameObject.SetActive(false);
                }
                else if(lvlData.LevelState == LevelState.Unlocked)
                {
                    _levelButtons[i].interactable = true;
                    _levelImages[i].gameObject.SetActive(true);
                    _levelImages[i].color = _unlockedColor;
                }
                else if (lvlData.LevelState == LevelState.Completed)
                {
                    _levelButtons[i].interactable = true;
                    _levelImages[i].gameObject.SetActive(true);
                    _levelImages[i].color = _completedColor;
                }
                _levelNameTexts[i].text = level + 1 + "";
                _levelButtons[i].onClick.RemoveAllListeners();
                _levelButtons[i].onClick.AddListener(() => OnLevelClicked(lvlData));
            }
        }
        
        private void OnLevelClicked(LevelData levelData)
        {
            _soundManager.PlayButtonClick();
            if(levelData.LevelState == LevelState.Locked) return;
            _gameData.CurrentLevel = levelData;
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }
    }
}