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

    private Rigidbody rb;
    private Animator animator;
    private BoxCollider boxCollider;
    private int currentPointIndex = 0;

    private bool _isDead;

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
}
