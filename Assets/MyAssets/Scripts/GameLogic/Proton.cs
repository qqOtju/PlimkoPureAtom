using System;
using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class Proton: Particle
    {
        [SerializeField] private ElectronCircle _electronCircle;
        [SerializeField] private ParticleSystem _ps;
        
        private bool _entered;
        
        public event Action<Proton, ElectronCircle> OnMainCircleCollision;

        protected override void Start()
        {
            base.Start();
            _electronCircle.gameObject.SetActive(false);
        }
        
        public override void Init(GameData gameData)
        {
            base.Init(gameData);
            _sr.sprite = gameData.Skins.CurrentSkin.ProtonSprite;
            _sr.color = gameData.Skins.CurrentSkin.ProtonColor;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Main Electron Circle")) return;
            if (_entered) return;
            Rb.velocity = Vector2.zero;
            _ps.Play();
            _electronCircle.gameObject.SetActive(true);
            _electronCircle.transform.parent = gameObject.transform.parent;
            OnMainCircleCollision?.Invoke(this, _electronCircle);
            Debug.Log("Proton collided with main circle");
            _entered = true;
        }
    }
}