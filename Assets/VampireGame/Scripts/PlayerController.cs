using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    
    [Header("Joystick Reference")]
    [SerializeField] private Joystick _joystick;

    [SerializeField] private Rigidbody _rigidbody;
    
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        
        _rigidbody.velocity = direction.normalized * _moveSpeed;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rigidbody.MoveRotation(Quaternion.Lerp(_rigidbody.rotation, targetRotation, 10f * Time.deltaTime));
        }
    }
}
