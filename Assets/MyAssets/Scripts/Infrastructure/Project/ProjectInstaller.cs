using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.Data.SO;
using MyAssets.Scripts.MyInput;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.Project
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private SoundManager _soundManager;

        public override void InstallBindings()
        {
            BindGameData();
            BindSoundManager();
            BindInputHandler();
        }

        private void BindGameData()
        {
            var gameData = new GameData(_gameDataSO.AbilitiesSO, _gameDataSO.SkinsSO, 
                _gameDataSO.LevelsSO ,_gameDataSO.PlayerSO);
            Container.Bind<GameData>().FromInstance(gameData).AsSingle();
        }
        
        private void BindSoundManager() =>
            Container.Bind<SoundManager>().
                FromInstance(Instantiate(_soundManager)).AsSingle();

        private void BindInputHandler()
        {
            var inputHandler = new GameObject("InputHandler").AddComponent<InputHandler>();
            Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
        }
    }
}