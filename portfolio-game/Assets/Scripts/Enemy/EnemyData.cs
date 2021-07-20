using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "new enemy", menuName = "enemey/dummy", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public float maxHealth;
    }
}