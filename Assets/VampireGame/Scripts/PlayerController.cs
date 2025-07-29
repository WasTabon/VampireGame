using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Joystick Reference")]
    [SerializeField] private Joystick _joystick;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    private bool _isWalking = false;
    private bool _canCheckHit = false;

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerStay(Collider coll)
    {
        if (_canCheckHit)
        {
            EnemyController enemy = coll.GetComponent<EnemyController>();
            Debug.Log("Trigger");
            if (enemy != null)
            {
                Debug.Log("Enemy");
                enemy.TakeDamage();
                _canCheckHit = false;
            }
        }
    }

    public void HandleAttack()
    {
        _animator.SetTrigger("Attack");
        StartCoroutine(DelayedHitCheck());
    }
    
    public void OnHitboxTriggerStay(Collider coll)
    {
        if (_canCheckHit)
        {
            EnemyController enemy = coll.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                _canCheckHit = false;
            }
        }
    }


    private IEnumerator DelayedHitCheck()
    {
        yield return new WaitForSeconds(1.05f);
        _canCheckHit = true;

        yield return new WaitForFixedUpdate();
        _canCheckHit = false;
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        _rigidbody.velocity = direction.normalized * _moveSpeed;

        bool hasMovement = direction.sqrMagnitude > 0.01f;

        if (hasMovement != _isWalking)
        {
            _isWalking = hasMovement;
            _animator.SetBool("Walk", _isWalking);
        }

        if (hasMovement)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rigidbody.MoveRotation(Quaternion.Lerp(_rigidbody.rotation, targetRotation, 10f * Time.deltaTime));
        }
    }
}