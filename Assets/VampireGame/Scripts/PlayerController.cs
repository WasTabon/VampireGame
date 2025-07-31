using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Joystick Reference")]
    [SerializeField] private Joystick _joystick;

    [Header("Player Parts")] 
    [SerializeField] private Transform _rightHand;
    
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [Header("Dash Settings")]
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _dashCooldown = 30f;

    [Header("Particles")] 
    [SerializeField] private GameObject _dashParticle;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject _chestParticle;
    [SerializeField] private GameObject _fogParticle;

    public int keys;

    public bool isInvisible;
    
    public GameObject chest;
    public GameObject door;

    public bool _canWalk = true;
    private bool _isWalking = false;
    private bool _canCheckHit = false;
    private bool _canDash = true;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (_canWalk)
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
                Instantiate(_hitParticle, _rightHand.position, Quaternion.identity);
                enemy.TakeDamage();
                _canCheckHit = false;
            }
        }
    }

    public void GetKey()
    {
        keys++;
        Instantiate(_chestParticle, chest.transform.position, quaternion.identity);
        chest.SetActive(false);
        
        UIController.Instance.HidePanel(UIPanelType.GetKeyButton);
    }

    public void UseKey()
    {
        keys--;
        door.GetComponent<Door>().OpenDoor();
        
        UIController.Instance.HidePanel(UIPanelType.UseKeyButton);
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

    public void HandleSkillDash()
    {
        if (!_canDash) return;
        StartCoroutine(PerformDash());
    }

    public void HandleInvisible()
    {
        if (_fogParticle.activeSelf)
        {
            _fogParticle.SetActive(false);
            isInvisible = false;
        }
        else
        {
            _fogParticle.SetActive(true);
            isInvisible = true;
        }
    }

    public void HandleFall()
    {
        if (_canWalk)
        {
            _canWalk = false;
            isInvisible = true;
            _animator.SetTrigger("Fall");
        }
        else
        {
            _canWalk = true;
            isInvisible = false;
            _animator.SetTrigger("Stand");
        }
    }

    private IEnumerator PerformDash()
    {
        _canDash = false;

        Vector3 dashDirection = _rigidbody.gameObject.transform.forward.normalized;
        Vector3 startPosition = _rigidbody.position;

        // Проверка столкновений на пути
        float dashDistance = _dashForce;
        if (Physics.Raycast(startPosition, dashDirection, out RaycastHit hit, dashDistance))
        {
            dashDistance = hit.distance - 0.05f; // немного не до препятствия
        }

        // Спавн партикла на старой позиции в направлении рывка
        if (_dashParticle != null)
        {
            Instantiate(_dashParticle, startPosition, Quaternion.LookRotation(dashDirection));
        }

        // Смещение игрока
        Vector3 targetPosition = startPosition + dashDirection * dashDistance;
        _rigidbody.MovePosition(targetPosition);

        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }


    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
}
