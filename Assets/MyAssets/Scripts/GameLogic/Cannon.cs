using System;
using System.Collections;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.DesignPatterns.ObjPool;
using MyAssets.Scripts.MyInput;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    [SelectionBase]
    public class Cannon: MonoBehaviour
    {
        [Header("Arrow Settings")]
        [SerializeField] private Transform _arrow;
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private float _leftConstraint = 45f; 
        [SerializeField] private float _rightConstraint = 45f; 
        [Header("Particle Settings")]
        [SerializeField] private Proton _protonPrefab;
        [SerializeField] private Electron _electronPrefab;
        [SerializeField] private Transform  _electronContainer;
        [SerializeField] private Transform  _protonContainer;
        [SerializeField] private float _particleSpeed = 10f;
        [Header("Buttons")]
        [SerializeField] private Button _positiveButton;
        [SerializeField] private Button _negativeButton;
        [SerializeField] private Button _shootButton;

        private MonoBehaviourPool<Electron> _electronPool;
        private MonoBehaviourPool<Proton> _protonPool;
        private Coroutine _shootCoroutine;
        private bool _isRotatingRight = true;
        private Electron _currentElectron;
        private Proton _currentProton;
        private GameData _gameData;
        private int _currentElectronsCount;
        private int _currentProtonsCount;

        public event Action<ElectronCircle> OnCircleCreated;
        public event Action OnAllPariclesShooted;

        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
            _protonPool = new MonoBehaviourPool<Proton>(_protonPrefab, _protonContainer);
            _electronPool = new MonoBehaviourPool<Electron>(_electronPrefab, _electronContainer);
            _electronPool.Initialize(_gameData.CurrentLevel.LevelSO.ElectronsCount);
            _protonPool.Initialize(_gameData.CurrentLevel.LevelSO.ProtonsCount);
            _currentElectronsCount = _gameData.CurrentLevel.LevelSO.ElectronsCount;
            _currentProtonsCount = _gameData.CurrentLevel.LevelSO.ProtonsCount;
            SetNegative();
        }

        private void Awake() 
        {
            _positiveButton.onClick.AddListener(SetPositive);
            _negativeButton.onClick.AddListener(SetNegative);
            _shootButton.onClick.AddListener(Shoot);
        }
        
        void Update()
        {
            RotateArrow();
        }
        
        private void RotateArrow()
        {
            if (!_isRotatingRight)
            {
                _arrow.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
                if (_arrow.eulerAngles.z >= _leftConstraint && _arrow.eulerAngles.z < 180)
                    _isRotatingRight = true;
            }
            else
            {
                _arrow.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
                if (_arrow.eulerAngles.z <= 360 - _rightConstraint && _arrow.eulerAngles.z > 180)
                    _isRotatingRight = false;
            }
        }

        private void Shoot()
        {
            if(_shootCoroutine != null) return;
            _shootCoroutine = StartCoroutine(ShootCoroutine());
        }
        
        private IEnumerator ShootCoroutine()
        {
            Particle currentParticle;
            if(_currentProton != null)
            {
                currentParticle = _currentProton;
            }
            else
                currentParticle = _currentElectron;            
            var particleTransform = currentParticle.transform;
            particleTransform.position = _arrow.position;
            particleTransform.rotation = Quaternion.Euler(0,0, _arrow.eulerAngles.z + 90);
            currentParticle.Shoot(_particleSpeed);
            if (currentParticle == _currentElectron)
            {
                _currentElectronsCount--;
                _currentElectron = null;
            }
            else
            {
                _currentProtonsCount--;
                _currentProton = null;
            }
            yield return new WaitForSeconds(0.5f);
            SetNegative();
            _shootCoroutine = null;
            if(_currentProton == null && _currentElectron == null)
                OnAllPariclesShooted?.Invoke();
        }

        private void SetNegative()
        {
            if(_currentElectronsCount <= 0)
            {
                if(_currentProtonsCount > 0)
                    SetPositive();
                return;
            }
            if(_currentProton != null)
            {
                _protonPool.Release(_currentProton);
                _currentProton.OnMainCircleCollision -= OnMainCircleCollision;
                _currentProton = null;
            }
            if(_currentElectron == null)
                _currentElectron = _electronPool.Get();
            _currentElectron.Init(_gameData);
            _currentElectron.transform.position = _arrow.position;
        }

        private void SetPositive()
        {
            if(_currentProtonsCount <= 0)
            {
                if(_currentElectronsCount > 0)
                    SetNegative();
                return;
            }
            if(_currentProton != null)
            {
                _protonPool.Release(_currentProton);
                _currentProton.OnMainCircleCollision -= OnMainCircleCollision;
                _currentProton = null;
            }
            if(_currentElectron != null)
            {
                _electronPool.Release(_currentElectron);
                _currentElectron = null;
            }
            if(_currentProton == null)
                _currentProton = _protonPool.Get();
            _currentProton.Init(_gameData);
            _currentProton.OnMainCircleCollision += OnMainCircleCollision;
            _currentProton.transform.position = _arrow.position;
        }

        private void OnMainCircleCollision(Proton proton, ElectronCircle electronCircle)
        {
            proton.OnMainCircleCollision -= OnMainCircleCollision;
            OnCircleCreated?.Invoke(electronCircle);
        }

        private void OnDrawGizmos()
        {
            var leftAngle = Quaternion.Euler(0, 0, 90 + _leftConstraint);
            var rightAngle = Quaternion.Euler(0, 0,  90 + 360 - _rightConstraint);
            var position = _arrow.transform.position;
            var leftVector = leftAngle * Vector3.right;
            var rightVector = rightAngle * Vector3.right;
            Gizmos.DrawLine(position, leftVector * 10);
            Gizmos.DrawLine(position, rightVector * 10);
        }
    }
}