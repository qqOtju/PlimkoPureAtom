using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class MenuUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _historyButton;
        [Header("Panels")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _historyPanel;
        [SerializeField] private GameObject _levelsPanel;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI[] _goldCountTexts;
        [Header("Scene Name")]
        [SerializeField] private string _gameSceneName;

        private SoundManager _soundManager;
        private GameData _data;
        
        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        { 
            Debug.Log("MenuUI Construct");
            _soundManager = soundManager;
            _data = data;
            SetGoldCount();
            _data.Gold.OnGoldCountChanged += SetGoldCount;
        }
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnStartButtonClicked);
            _shopButton.onClick.AddListener(OnShopButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _historyButton.onClick.AddListener(OnHistoryButtonClicked);
        }
        
        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnStartButtonClicked);
            _shopButton.onClick.RemoveListener(OnShopButtonClicked);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _historyButton.onClick.RemoveListener(OnHistoryButtonClicked);
            _data.Gold.OnGoldCountChanged -= SetGoldCount;
        }

        private void OnStartButtonClicked()
        { 
            _soundManager.PlayButtonClick();
            _levelsPanel.SetActive(true);
            _menuPanel.SetActive(false);
        }

        private void OnOptionsButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _optionsPanel.SetActive(true);
        }

        private void OnShopButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _menuPanel.SetActive(false);
            _shopPanel.SetActive(true);
        }

        private void OnHistoryButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _historyPanel.SetActive(true);
        }
        
        private void SetGoldCount()
        {
            foreach (var text in _goldCountTexts)
            {
                text.text = _data.Gold.GoldCount.ToString();
            }
        }
    }
}