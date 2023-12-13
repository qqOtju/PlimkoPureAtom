using System.Collections.Generic;
using MyAssets.Scripts.Data.SO;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class SkinsData
    {
        private const string CurrentSkinKey = "CurrentSkin";

        public SkinSO CurrentSkin { get; private set; }
        public Dictionary<SkinSO, SkinState> SkinsState { get; } = new();

        public SkinSO[] Skins { get; }

        public SkinsData(SkinSO[] skinsSO)
        {
            Skins = skinsSO;
            foreach (var skin in Skins)
            {
                var state = PlayerPrefs.GetInt($"{CurrentSkinKey}{skin.ID}", 0);
                SkinsState.Add(skin, (SkinState) state);
                if (state == (int) SkinState.Equipped)
                    CurrentSkin = skin;
            }
            if(CurrentSkin == null)
            {
                CurrentSkin = Skins[0];
                SkinsState[CurrentSkin] = SkinState.Equipped;
                PlayerPrefs.SetInt($"{CurrentSkinKey}{CurrentSkin.ID}", (int) SkinState.Equipped);
            }
        }
        
        public void UnlockSkin(SkinSO skin)
        {
            SkinsState[skin] = SkinState.Unlocked;
            PlayerPrefs.SetInt($"{CurrentSkinKey}{skin.ID}", (int) SkinState.Unlocked);
        }
        
        public void EquipSkin(SkinSO skin)
        {
            SkinsState[CurrentSkin] = SkinState.Unlocked;
            SkinsState[skin] = SkinState.Equipped;
            PlayerPrefs.SetInt($"{CurrentSkinKey}{CurrentSkin.ID}", (int) SkinState.Unlocked);
            PlayerPrefs.SetInt($"{CurrentSkinKey}{skin.ID}", (int) SkinState.Equipped);
            CurrentSkin = skin;
        }
    }
    
    public enum SkinState
    {
        Locked = 0,
        Unlocked = 1,
        Equipped = 2
    }
}