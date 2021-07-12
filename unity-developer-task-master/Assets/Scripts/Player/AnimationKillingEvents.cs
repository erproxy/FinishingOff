using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class AnimationKillingEvents : MonoBehaviour
    {
        public UnityEvent KillingEnemy;
        public UnityEvent EnemyKilled;

        public void Killing()
        {
            KillingEnemy?.Invoke();
        }
        
        public void Killed()
        {
            EnemyKilled?.Invoke();
        }
    }
}