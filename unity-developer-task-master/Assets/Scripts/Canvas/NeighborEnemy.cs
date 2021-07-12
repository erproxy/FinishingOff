using UnityEngine;


namespace UI
{
    public class NeighborEnemy : MonoBehaviour
    {

        [SerializeField] private GameObject _neighborEnemyPanel;


        public void OpenNeighborEnemyPanel() => _neighborEnemyPanel.SetActive(true);
        
        public void CloseNeighborEnemyPanel() => _neighborEnemyPanel.SetActive(false);
    }
}