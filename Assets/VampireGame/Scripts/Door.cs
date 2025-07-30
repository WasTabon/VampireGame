using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider _leftCollider;
    [SerializeField] private BoxCollider _rightCollider;

    public bool isOpened;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        isOpened = true;

        _leftCollider.enabled = false;
        _rightCollider.enabled = false;
        
        _animator.SetTrigger("OpenDoor");
    }
}
