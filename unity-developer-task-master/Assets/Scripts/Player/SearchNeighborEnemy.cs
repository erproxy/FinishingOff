using System.Collections.Generic;
using UnityEngine;
using Player.Dto;
using UnityEngine.Events;
using Enemy.Dto;

namespace Player
{
    public class SearchNeighborEnemy : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _enemy;
        [SerializeField] private GameObject _player;
        
        [Tooltip("Дистанция необходимая для добивания")]
        [SerializeField] private float _killDistance;
        private float _dist;
        private float _minDist;
        private GameObject _cashEnemy;

        public UnityEvent<EnemyDto> EnemySearched;

        public void RemoveEnemyFromList(EnemyRemoveFromListDto dto)
        {
            Debug.Log("Remove");
            _enemy.Remove(dto.EnemyRemoveGO);
            Debug.Log(dto.EnemyRemoveGO.name);
        }

        public void AddEnemyInList(EnemyRespawnDto dto)
        {
            _enemy.Add(dto.EnemyRespawnGO);
        }
        private void Update()
        {
            _cashEnemy = null;
            _minDist = _killDistance;
            foreach (var GO in _enemy)
            {
                _dist = Vector3.Distance(GO.transform.position, _player.transform.position);
                if (_dist<=_minDist)
                {
                    _cashEnemy = GO;
                    _minDist = _dist;
                }
            }

            if (_cashEnemy != null)
            {
                EnemyDto dto = new EnemyDto {EnemyGO = _cashEnemy};
                EnemySearched?.Invoke(dto);
            }

            
            
        }

    }
}