using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Skin", menuName = "MyAssets/Skin")]
    public class SkinSO: ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _electronSprite;
        [SerializeField] private Color _electronColor;
        [SerializeField] private Sprite _protonSprite;
        [SerializeField] private Color _protonColor;
        [Header("Values")]
        [SerializeField] private int _price;

        public int ID => _id;
        public string Name => _name;
        public Sprite ElectronSprite => _electronSprite;
        public Color ElectronColor => _electronColor;
        public Sprite ProtonSprite => _protonSprite;
        public Color ProtonColor => _protonColor;
        public int Price => _price;
    }
}