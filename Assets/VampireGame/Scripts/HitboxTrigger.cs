using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private void OnTriggerStay(Collider other)
    {
        _player?.OnHitboxTriggerStay(other);
    }
}