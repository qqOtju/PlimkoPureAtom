using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "MyAssets/PlayerSO")]
    public class PlayerSO: ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        
        public int MaxHealth => _maxHealth;
    }
}