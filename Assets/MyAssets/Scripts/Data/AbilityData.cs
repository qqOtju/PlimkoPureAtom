using MyAssets.Scripts.Data.SO;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class AbilityData
    {
        private const string CountKey = "AbilityCount";
        private int _count;

        public AbilitySO Ability { get; }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                SetCount(_count);
            }
        }
        public bool Activated { get; set; }
        
        public AbilityData(AbilitySO ability)
        {
            Ability = ability;            
            _count = GetCount();
            Activated = false;
        }
        
        private int GetCount() =>
            PlayerPrefs.GetInt($"{CountKey}{Ability.Name}");
        
        private void SetCount(int count) =>
            PlayerPrefs.SetInt($"{CountKey}{Ability.Name}", count);
    }
}