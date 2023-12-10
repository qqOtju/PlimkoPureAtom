using System;
using UnityEngine;

namespace MyAssets.Scripts.MyInput
{
    public class InputHandler: MonoBehaviour
    {
        private Controls _controls;

        public event Action OnChargeStarted;
        public event Action OnChargeCanceled;

        private int _count;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _count = PlayerPrefs.GetInt("ScreenshotsCount");
            _controls = new Controls();   
            _controls.Player.Charge.started += StartCharge;
            _controls.Player.Charge.canceled += CancelCharge;
        }
                
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.A)) return;
            _count++;
            ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
            PlayerPrefs.SetInt("ScreenshotsCount", _count);
            Debug.Log("A screenshot was taken!");
        }
        
        private void OnEnable() => _controls.Enable();
        
        private void OnDisable() => _controls.Disable();
        
        private void StartCharge(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            OnChargeStarted?.Invoke();
        
        private void CancelCharge(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            OnChargeCanceled?.Invoke();
    }
}