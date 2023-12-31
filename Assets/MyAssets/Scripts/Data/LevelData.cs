﻿using MyAssets.Scripts.Data.SO;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class LevelData
    {
        private const string LevelNumberKey = "LevelNumber";

        private LevelState _levelState;
        public LevelState LevelState
        {
            get => _levelState;
            set
            {
                _levelState = value;
                PlayerPrefs.SetInt($"{LevelNumberKey}{LevelSO.ID}", (int) value);
            } 
        }
        public LevelSO LevelSO { get; }

        public LevelData(LevelSO level)
        {
            LevelSO = level;
            // LevelState = LevelState.Locked;
            LevelState = (LevelState) PlayerPrefs.
                GetInt($"{LevelNumberKey}{level.ID}", 0);
            if (level.ID == 0 && LevelState != LevelState.Completed)
                LevelState = LevelState.Unlocked;
        }
    }
    
    public enum LevelState
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2
    }
}