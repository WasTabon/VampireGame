using UnityEngine;

public class PlayerWalkParticles : MonoBehaviour
{
    [Header("Player Parts")] 
    [SerializeField] private Transform _leftLeg;
    [SerializeField] private Transform _rightLeg;
    
    [Header("Particles")] 
    [SerializeField] private GameObject _stepParticle;
    
    public void HandleLeftStep()
    {
        Instantiate(_stepParticle, _leftLeg.position, Quaternion.identity);
    }

    public void HandleRightStep()
    {
        Instantiate(_stepParticle, _rightLeg.position, Quaternion.identity);
    }
}
