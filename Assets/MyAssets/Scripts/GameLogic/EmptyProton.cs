using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class EmptyProton: MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _sr;
        
        public void Init(GameData gameData)
        {
            _sr.sprite = gameData.Skins.CurrentSkin.ProtonSprite;
            _sr.color = gameData.Skins.CurrentSkin.ProtonColor;
        }
    }
}