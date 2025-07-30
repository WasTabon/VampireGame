using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWalkParticles : MonoBehaviour
{
    [Header("Player Parts")]
    [SerializeField] private Transform _leftLeg;
    [SerializeField] private Transform _rightLeg;

    [Header("Particles")]
    [SerializeField] private GameObject _stepParticle;
    [SerializeField] private int _poolSize = 10;

    private Queue<ParticleWrapper> _particlePool;

    private void Awake()
    {
        _particlePool = new Queue<ParticleWrapper>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject particleGO = Instantiate(_stepParticle);
            particleGO.SetActive(false);

            ParticleWrapper wrapper = new ParticleWrapper(particleGO);
            _particlePool.Enqueue(wrapper);
        }
    }

    public void HandleLeftStep()
    {
        SpawnParticle(_leftLeg.position);
    }

    public void HandleRightStep()
    {
        SpawnParticle(_rightLeg.position);
    }

    private void SpawnParticle(Vector3 position)
    {
        ParticleWrapper wrapper = GetParticleFromPool();
        wrapper.GameObject.transform.position = position;
        wrapper.GameObject.SetActive(true);

        if (wrapper.ParticleSystem != null)
        {
            wrapper.ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            wrapper.ParticleSystem.Play();
            StartCoroutine(ReturnToPoolAfterDelay(wrapper, wrapper.ParticleSystem.main.duration));
        }
        else
        {
            StartCoroutine(ReturnToPoolAfterDelay(wrapper, 1f));
        }
    }

    private ParticleWrapper GetParticleFromPool()
    {
        if (_particlePool.Count > 0)
        {
            return _particlePool.Dequeue();
        }
        else
        {
            GameObject particleGO = Instantiate(_stepParticle);
            particleGO.SetActive(false);
            return new ParticleWrapper(particleGO);
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(ParticleWrapper wrapper, float delay)
    {
        yield return new WaitForSeconds(delay);
        wrapper.GameObject.SetActive(false);
        _particlePool.Enqueue(wrapper);
    }

    private class ParticleWrapper
    {
        public GameObject GameObject { get; }
        public ParticleSystem ParticleSystem { get; }

        public ParticleWrapper(GameObject gameObject)
        {
            GameObject = gameObject;
            ParticleSystem = gameObject.GetComponent<ParticleSystem>();
        }
    }
}
