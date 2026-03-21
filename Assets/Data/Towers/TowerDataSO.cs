namespace MindGuard.Data
{
    using UnityEngine;

    /// <summary>
    /// ScriptableObject que contiene los datos de configuración de una torre.
    /// </summary>
    [CreateAssetMenu(menuName = "MindGuard/Data/Torre", fileName = "NewTowerData")]
    public class TowerDataSO : ScriptableObject
    {
        [SerializeField]
        private string towerName;

        [SerializeField]
        [TextArea(2, 4)]
        private string description;

        [SerializeField]
        private float cost;

        [SerializeField]
        private float maxHP;

        [SerializeField]
        private float range;

        [SerializeField]
        private float fireRate;

        [SerializeField]
        private float damagePerShot;

        [SerializeField]
        private GameObject prefab;

        public string TowerName => towerName;
        public string Description => description;
        public float Cost => cost;
        public float MaxHP => maxHP;
        public float Range => range;
        public float FireRate => fireRate;
        public float DamagePerShot => damagePerShot;
        public GameObject Prefab => prefab;
    }
}
