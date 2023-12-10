using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class SceneObjects: MonoBehaviour
    {
        [SerializeField] private ElectronCircle _mainCircle;
        
        public ElectronCircle MainCircle => _mainCircle;
    }
}