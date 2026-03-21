namespace MindGuard.Data
{
    using UnityEngine;

    /// <summary>
    /// ScriptableObject que contiene los datos de configuración de un enemigo.
    /// </summary>
    [CreateAssetMenu(menuName = "MindGuard/Data/Enemigo", fileName = "NewEnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        [SerializeField]
        private string enemyName;

        [SerializeField]
        [TextArea(2, 4)]
        private string description;

        [SerializeField]
        private float maxHP;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float damagePerSecond;

        [SerializeField]
        private float inspirationReward;

        [SerializeField]
        [Range(1, 10)]
        private int spawnWeight;

        public string EnemyName => enemyName;
        public string Description => description;
        public float MaxHP => maxHP;
        public float MoveSpeed => moveSpeed;
        public float DamagePerSecond => damagePerSecond;
        public float InspirationReward => inspirationReward;
        public int SpawnWeight => spawnWeight;
    }
}
