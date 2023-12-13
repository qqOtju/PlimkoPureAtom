using System.Collections.Generic;
using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.Data.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class ShopUI: MonoBehaviour
    {
        [Header("Shop Items")]
        [SerializeField] private TextMeshProUGUI[] _names;
        [SerializeField] private Button[] _buyButtons;
        [SerializeField] private Image[] _images;
        [SerializeField] private Image[] _secImage;
        [SerializeField] private TextMeshProUGUI[] _prices;
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _shopPanel;
        [Header("Other")]
        [SerializeField] private TextMeshProUGUI[] _goldTexts;
        [SerializeField] private Button _backButton;

        private readonly Dictionary<int, SkinSO> _abilitiesData = new();
        private SoundManager _soundManager;
        private GameData _data;

        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        {
            _data = data;
            _soundManager = soundManager;
            foreach (var abilityData in _data.Skins.Skins)
            {
                var id = abilityData.ID;
                _names[id].text = abilityData.Name;
                _abilitiesData.Add(id, abilityData);
                _buyButtons[id].onClick.AddListener(() => OnAbilityButtonClicked(id));
                _images[id].sprite = abilityData.ElectronSprite;
                _images[id].color = abilityData.ElectronColor;
                _secImage[id].sprite = abilityData.ProtonSprite;
                _secImage[id].color = abilityData.ProtonColor;
                _prices[id].text = abilityData.Price.ToString();
            }
            UpdateButtons();
            SetGoldCount();
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnAbilityButtonClicked(int index)
        {
            _soundManager.PlayButtonClick();
            var ability = _abilitiesData[index];
            if(_data.Skins.SkinsState[ability] == SkinState.Equipped) return;
            if(_data.Skins.SkinsState[ability] == SkinState.Unlocked)
            {
                _data.Skins.EquipSkin(ability);
                UpdateButtons();
                return;
            }
            if(_data.Gold.GoldCount < ability.Price) return;
            _data.Gold.GoldCount -= ability.Price;
            _data.Skins.UnlockSkin(ability);
            SetGoldCount();
            UpdateButtons();
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _mainMenuPanel.SetActive(true);
            _shopPanel.SetActive(false);
        }
        
        private void UpdateButtons()
        {
            foreach (var abilityData in _data.Skins.Skins)
            {
                var id = abilityData.ID;
                if (_data.Skins.SkinsState[abilityData] == SkinState.Equipped)
                {
                    _prices[id].text = "Equipped";
                }
                else if(_data.Skins.SkinsState[abilityData] == SkinState.Unlocked)
                {
                    _prices[id].text = "Equip";
                    _buyButtons[id].interactable = true;
                }
                else if(_data.Skins.SkinsState[abilityData] == SkinState.Locked)
                {
                    _prices[id].text = abilityData.Price.ToString();
                    _buyButtons[id].interactable = _data.Gold.GoldCount >= abilityData.Price;
                }
            }
        }

        private void SetGoldCount()
        {
            foreach (var text in _goldTexts)
                text.text = _data.Gold.GoldCount.ToString();
        }
    }
}