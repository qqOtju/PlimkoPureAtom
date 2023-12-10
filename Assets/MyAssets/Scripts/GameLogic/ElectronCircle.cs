using System;
using System.Collections;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class ElectronCircle: MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private int _predefinedElectronsCount = 5;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private ParticleSystem _psPrefab;
        
        public event Action OnMainElectronObtained;
        public event Action<ElectronCircle> OnElectronObtained;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Electron")) return;
            var electron = other.GetComponent<Electron>();
            if(electron.Obtained) return;
            if(gameObject.CompareTag("Electron Circle"))
            {
                var prefab = Instantiate(_psPrefab, other.transform.position, Quaternion.identity);
                prefab.Play();
                OnElectronObtained?.Invoke(this);
            }
            else
            {
                _predefinedElectronsCount--;
                if (_predefinedElectronsCount < 0)
                {
                    var prefab = Instantiate(_psPrefab, other.transform.position, Quaternion.identity);
                    prefab.Play();
                    OnMainElectronObtained?.Invoke();
                }
            }
            electron.Obtained = true;
            StartCoroutine(RotateObjectAroundCircleCoroutine(electron.Rb));
        }
        
        private IEnumerator RotateObjectAroundCircleCoroutine(Rigidbody2D rb)
        {
            var circleCenter = transform.position;
            while (true)
            {
                var toCenter = rb.transform.position - circleCenter;
                var angle = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;
                var rotation = angle + _rotationSpeed * Time.deltaTime;
                rb.MovePosition((Vector2)circleCenter + new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)) * _radius);
                yield return null;
            }
        }
    }
}