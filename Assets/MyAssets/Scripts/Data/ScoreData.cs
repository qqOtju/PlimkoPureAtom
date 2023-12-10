using System;
using System.Linq;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class ScoreData
    {
        private const string HighScoreKey = "Highscore";

        private int _currentScore;
        
        public int[] LastFiveScores { get; }
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnScoreUpdated?.Invoke(value);
                UpdateHighScore(value);
            }
        }
        
        public event Action<int> OnScoreUpdated;
        
        public ScoreData()
        {
            LastFiveScores = new int[5];
            for (var i = 0; i < LastFiveScores.Length; i++)
                LastFiveScores[i] = PlayerPrefs.GetInt($"{HighScoreKey}{i}", 0);
            Array.Sort(LastFiveScores);
        }
        
        private void UpdateHighScore(int newScore)
        {
            var isBetter = LastFiveScores.Any(scr => newScore > scr);
            if (isBetter)
            {
                LastFiveScores[0] = newScore;
                Array.Sort(LastFiveScores);
            }
            for (var i = 0; i < LastFiveScores.Length; i++)
                PlayerPrefs.SetInt($"{HighScoreKey}{i}", LastFiveScores[i]);
        }
    }
}