using MyAssets.Scripts.Data;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    public class EmptyProton: MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _sr;
        
        [Inject]
        public virtual void Construct(GameData gameData)
        {
            _sr.sprite = gameData.Skins.CurrentSkin.ProtonSprite;
            _sr.color = gameData.Skins.CurrentSkin.ProtonColor;
        }
    }
}