using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class SceneObjects: MonoBehaviour
    {
        [SerializeField] private ElectronCircle _mainCircle;
        [SerializeField] private EmptyProton[] _emptyProtons;
        [SerializeField] private Electron[] _electrons;
        
        public ElectronCircle MainCircle => _mainCircle;

        public void Init(GameData gameData)
        {
            foreach (var proton in _emptyProtons)
                proton.Init(gameData);
            foreach (var electron in _electrons)
                electron.Init(gameData);
        }
    }
}