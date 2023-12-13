using System;
using System.Collections.Generic;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.UI.GameScene;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField] private Cannon _cannon;
        [SerializeField] private LooseScreenUI _looseScreen;
        [SerializeField] private WinScreenUI _winScreen;
        
        private readonly Dictionary<ElectronCircle, int> _electronsOnCircles = new();
        
        private SceneObjects _sceneObjects;
        private int _mainCircleCount;
        private int _newCircleCount;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
            var pos = gameData.CurrentLevel.LevelSO.SceneObjectsPosition;
            _sceneObjects = Instantiate(gameData.CurrentLevel.LevelSO.SceneObjects, pos, Quaternion.identity);
            _sceneObjects.Init(gameData);
            _sceneObjects.MainCircle.OnMainElectronObtained += OnMainCircleCollision;
        }

        private void Awake()
        {
            _cannon.OnAllPariclesShooted += OnAllPariclesShooted;
            _cannon.OnCircleCreated += OnCircleCreated;
        }

        private void OnDestroy()
        {
            _sceneObjects.MainCircle.OnMainElectronObtained -= OnMainCircleCollision;
            _cannon.OnAllPariclesShooted -= OnAllPariclesShooted;
            _cannon.OnCircleCreated -= OnCircleCreated;
        }

        private void OnMainCircleCollision()
        {
            _mainCircleCount++;
            if(_mainCircleCount > _gameData.CurrentLevel.LevelSO.MainCircleCount)
            {
                _looseScreen.gameObject.SetActive(true);
                _looseScreen.Init("Too many electrons on main circle");
            }
            CheckWin();
        }

        private void OnElectronObtained(ElectronCircle obj)
        {
            if (!_electronsOnCircles.ContainsKey(obj))
                throw new Exception("Electron circle not found");
            _electronsOnCircles[obj]++;
            if (_electronsOnCircles[obj] > _gameData.CurrentLevel.LevelSO.NewCircleElectronsCount)
            {
                _looseScreen.gameObject.SetActive(true);
                _looseScreen.Init("Too many electrons on additional circle");
            }
            CheckWin();
        }

        private void OnAllPariclesShooted()
        {
            if(CheckWin()) return;
            _looseScreen.gameObject.SetActive(true);
            _looseScreen.Init("All the particles were shot");
        }

        private void OnCircleCreated(ElectronCircle electronCircle)
        {
            _electronsOnCircles.Add(electronCircle, 0);
            electronCircle.OnElectronObtained += OnElectronObtained;
            _newCircleCount++;
            if (_newCircleCount > _gameData.CurrentLevel.LevelSO.NewCircleCount)
            {
                _looseScreen.gameObject.SetActive(true);
                _looseScreen.Init("Too many additional circles");
            }
            CheckWin();
        }

        private bool CheckWin()
        {
            if (_mainCircleCount != _gameData.CurrentLevel.LevelSO.MainCircleCount ||
                _newCircleCount != _gameData.CurrentLevel.LevelSO.NewCircleCount) return false;
            foreach (var electronsOnCircle in _electronsOnCircles)
                if (electronsOnCircle.Value != _gameData.CurrentLevel.LevelSO.NewCircleElectronsCount)
                    return false;
            _gameData.CompleteCurrentLevel();
            _winScreen.gameObject.SetActive(true);
            return true;
        }
    }
}