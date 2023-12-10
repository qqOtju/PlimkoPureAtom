using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D))]
    public abstract class Particle: MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _sr;
        
        protected GameData GameData;
        
        public Rigidbody2D Rb { get; private set; }

        public virtual void Init(GameData gameData)
        {
            GameData = gameData;
            _sr.sprite = GameData.Skins.CurrentSkin.ElectronSprite;
            _sr.color = GameData.Skins.CurrentSkin.ElectronColor;
        }
        
        protected virtual void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }
        
        public void Shoot(float speed)
        {
            var lookDirection = transform.right;
            Rb.velocity = lookDirection * speed;
        }
    }
}