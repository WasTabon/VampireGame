using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out PlayerDetector _))
        {
            // додоати ключі, двері відкриття ними і ui шоб показувало шо треба ключ і кнопку шоб взяти ключ
        }
    }
}
