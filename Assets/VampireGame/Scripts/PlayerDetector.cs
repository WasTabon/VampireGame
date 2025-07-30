using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out PlayerTriggerController _))
        {
            UIController.Instance.ShowLostPanel();
        }
    }
}
