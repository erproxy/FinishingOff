using System.Collections.Generic;
using UnityEngine;
using Enemy.Dto;
using UnityEngine.Events;
using System.Collections;

namespace Enemy
{
    public class EnemyDie : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyModel;
        [SerializeField] private List<Rigidbody> _ragdollElementsl;
        [SerializeField] private Transform _mainRoot;
        private Animator _enemyAnimator;
        
        
        private GameObject _cashEnemyKilled;
        private GameObject _cashEnemyRespawn;
        public UnityEvent<EnemyKilledDto> EnemyKilled;
        public UnityEvent<EnemyRespawnDto> EnemyRespawn;
        public UnityEvent<EnemyRemoveFromListDto> EnemyRemoveFromList;
        private void Start()
        {
            foreach (var element in _ragdollElementsl)
            {
                element.isKinematic = true;
            }
            
            _enemyAnimator = _enemyModel.GetComponent<Animator>();
        }

        public void TakeDmg()
        {
            _enemyAnimator.enabled = false;
            foreach (var element in _ragdollElementsl)
            {
                element.isKinematic = false;
            }
            EnemyRemoveFromListDto dto = new EnemyRemoveFromListDto {EnemyRemoveGO = gameObject};
            EnemyRemoveFromList?.Invoke(dto);
            StartCoroutine(BeforeRespawn());
        }

        IEnumerator BeforeRespawn()
        {
            yield return new WaitForSeconds(5f);
            EnemyKilledDto dto = new EnemyKilledDto {EnemyKilledGO = gameObject};
            EnemyKilled?.Invoke(dto);


        }
        public void _setDefault()
        {
            foreach (var element in _ragdollElementsl)
            {
                element.isKinematic = true;
            }
            _enemyAnimator.enabled = true;
            foreach (var element in _ragdollElementsl)
            {
                element.transform.rotation = Quaternion.identity;
            }

            _mainRoot.position = _enemyModel.transform.position;

            EnemyRespawnDto dto = new EnemyRespawnDto {EnemyRespawnGO =  gameObject};
            EnemyRespawn?.Invoke(dto);
        }
    }
}