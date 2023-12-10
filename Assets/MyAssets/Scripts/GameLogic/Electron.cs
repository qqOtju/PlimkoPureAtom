using System;
using MyAssets.Scripts.Data;

namespace MyAssets.Scripts.GameLogic
{
    public class Electron: Particle
    {
        [NonSerialized] public bool Obtained;
        
        public override void Init(GameData gameData)
        {
            base.Init(gameData);
            _sr.sprite = gameData.Skins.CurrentSkin.ElectronSprite;
            _sr.color = gameData.Skins.CurrentSkin.ElectronColor;
        }
    }
}