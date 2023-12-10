using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "MyAssets/GameDataSO")]
    public class GameDataSO: ScriptableObject
    {
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private AbilitySO[] _abilitiesSO;
        [SerializeField] private SkinSO[] _skinsSO;
        [SerializeField] private LevelSO[] _levelsSO;
        
        public PlayerSO PlayerSO => _playerSO;
        public AbilitySO[] AbilitiesSO => _abilitiesSO;
        public SkinSO[] SkinsSO => _skinsSO;
        public LevelSO[] LevelsSO => _levelsSO;
    }
}