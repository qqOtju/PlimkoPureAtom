using System;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class GoldData
    {
        private const string GoldKey = "GoldCount"; 
        
        private int _goldCount;
        
        public event Action OnGoldCountChanged; 
        
        public int GoldCount
        {
            get => _goldCount;
            set
            {
                _goldCount = value;
                PlayerPrefs.SetInt(GoldKey, _goldCount);
                OnGoldCountChanged?.Invoke();
            }
        } 
        
        public GoldData() =>
            _goldCount = PlayerPrefs.GetInt(GoldKey);
    }
}