using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private LayerMask levelLayerMask;

    private void OnTriggerEnter(Collider coll)
    {
        if (!_enemyController._isDead && !PlayerController.Instance.isInvisible) 
        {
            if (coll.gameObject.TryGetComponent(out PlayerTriggerController _))
            {
                Transform playerTransform = coll.transform;
                Vector3 origin = transform.position;
                Vector3 direction = (playerTransform.position - origin).normalized;
                float distance = Vector3.Distance(origin, playerTransform.position);

                if (!Physics.Raycast(origin, direction, distance, levelLayerMask))
                {
                    UIController.Instance.ShowLostPanel();
                }

#if UNITY_EDITOR
                Debug.DrawLine(origin, playerTransform.position, Color.red, 1f);
#endif
            }
        }
    }
}