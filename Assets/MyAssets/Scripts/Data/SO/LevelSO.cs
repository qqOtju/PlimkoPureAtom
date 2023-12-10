using MyAssets.Scripts.GameLogic;
using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "New Level", menuName = "MyAssets/Level")]
    public class LevelSO: ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _goal;
        [SerializeField] private Sprite _goalSprite;
        [SerializeField] private int _protonsCount;
        [SerializeField] private int _electronsCount;
        [SerializeField] private SceneObjects _sceneObjects;
        [SerializeField] private Vector3 _sceneObjectsPosition;
        [SerializeField] private int _mainCircleCount;
        [SerializeField] private int _newCircleCount;
        [SerializeField] private int _newCircleElectronsCount;
        
        public int ID => _id;
        public string Goal => _goal;
        public Sprite GoalSprite => _goalSprite;
        public int ProtonsCount => _protonsCount;
        public int ElectronsCount => _electronsCount;
        public SceneObjects SceneObjects => _sceneObjects;
        public Vector3 SceneObjectsPosition => _sceneObjectsPosition;
        public int MainCircleCount => _mainCircleCount;
        public int NewCircleCount => _newCircleCount;
        public int NewCircleElectronsCount => _newCircleElectronsCount;
    }
}