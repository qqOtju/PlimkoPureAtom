using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class HistoryUI: MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI[] _scores;
        [SerializeField] private GameObject _historyPanel;
        
        private SoundManager _soundManager;
        private GameData _gameData;
        
        [Inject]
        private void Construct(SoundManager soundManager, GameData gameData)
        {
            _soundManager = soundManager;
            _gameData = gameData;
            for (int i = 0; i < _gameData.Score.LastFiveScores.Length; i++)
            {
                var score = _gameData.Score.LastFiveScores[i];
                _scores[i].text = score.ToString();
            }
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _historyPanel.SetActive(false);
        }
    }
}