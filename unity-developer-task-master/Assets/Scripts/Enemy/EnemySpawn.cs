using System.Collections.Generic;
using Enemy.Dto;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private List<Transform> _listSpawnPositions;

        private GameObject spawnEnemy;
        public void StateKilling(EnemyKilledDto dto)
        {
            spawnEnemy = dto.EnemyKilledGO;
            var random = Random.Range(0, _listSpawnPositions.Count);
            spawnEnemy.transform.position = _listSpawnPositions[random].position;
            spawnEnemy.GetComponent<EnemyDie>()._setDefault();
        }
    }
}