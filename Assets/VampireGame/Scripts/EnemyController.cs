using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyBehavior
    {
        Idle,
        Patrol
    }

    [Header("Behavior Settings")]
    public EnemyBehavior currentBehavior = EnemyBehavior.Idle;

    [Header("Patrol Settings")]
    public List<Transform> patrolPoints;
    public float moveSpeed = 3f;
    public float reachThreshold = 0.2f;
    public float rotationSpeed = 5f;

    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private Light _light;
    
    private Rigidbody rb;
    private Animator animator;
    private BoxCollider boxCollider;
    private int currentPointIndex = 0;

    public bool _isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        
        switch (currentBehavior)
        {
            case EnemyBehavior.Patrol:
                animator.SetBool("Walk", true);
                break;
        }
    }

    public void TakeDamage()
    {
        _isDead = true;
        _playerDetector.enabled = false;
        _light.enabled = false;
        boxCollider.enabled = false;
        rb.isKinematic = true;
        animator.SetBool("Die", true);
    }
    
    private void FixedUpdate()
    {
        if (_isDead)
            return;
        
        switch (currentBehavior)
        {
            case EnemyBehavior.Idle:
                rb.velocity = Vector3.zero;
                break;

            case EnemyBehavior.Patrol:
                Patrol();
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position);
        direction.y = 0f;
        direction.Normalize();
        
        rb.velocity = direction * moveSpeed;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPoint.position.x, 0, targetPoint.position.z);
        if (Vector3.Distance(flatPos, flatTarget) < reachThreshold)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (currentBehavior != EnemyBehavior.Patrol || patrolPoints == null || patrolPoints.Count < 2)
            return;

        if (!TryGetComponent(out BoxCollider boxCollider)) return;
        float lineThickness = boxCollider.size.x;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (patrolPoints[i] == null) continue;

            Vector3 current = patrolPoints[i].position;
            Vector3 next = patrolPoints[(i + 1) % patrolPoints.Count]?.position ?? current;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(current, 0.2f);
            
            Vector3 midPoint = (current + next) / 2f;
            Vector3 direction = next - current;
            float length = direction.magnitude;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Vector3 lineSize = new Vector3(lineThickness, 0.05f, length);

            Gizmos.color = new Color(1f, 1f, 0f, 0.6f);
            Gizmos.matrix = Matrix4x4.TRS(midPoint, rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, lineSize);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

}
