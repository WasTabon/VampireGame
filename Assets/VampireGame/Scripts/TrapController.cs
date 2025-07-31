using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private ParticleSystem trapParticle;

    private bool _isActive;

    private void Start()
    {
        if (trapParticle != null)
        {
            StartCoroutine(ParticleLoopRoutine());
        }
    }

    private IEnumerator ParticleLoopRoutine()
    {
        while (true)
        {
            trapParticle.Play();
            _isActive = true;
            yield return new WaitForSeconds(3f);
            trapParticle.Stop();
            _isActive = false;
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isActive)
        {
            UIController.Instance.ShowLostPanel();
        }
    }
}