using UnityEngine;
using UnityEngine.Events;

namespace LevelSelector
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 3)]
    public class LevelData : ScriptableObject
    {
        public static UnityAction assetsChanged;

        public int level;
        [Header("Dominoes")]
        [Range(1, 200)] public int dominoesAmount;

        [Header("LevelGoals")]
        [Range(1, 400)] public int time = 200;
        [Range(1, 200)] public int goal = 5;
        [Range(1, 10)] public int streak = 3;
        public float streakWaitTime = 15f;

    }
}
