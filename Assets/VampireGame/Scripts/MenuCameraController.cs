using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera mainCam;
    [SerializeField] public CinemachineVirtualCamera settingsCam;
    [SerializeField] public CinemachineVirtualCamera shopCam;

    private CinemachineVirtualCamera _currentTarget;
    private Action _onCameraArrived;

    private const float PositionTolerance = 0.05f;
    private const float RotationTolerance = 1f;

    private void Update()
    {
        if (_currentTarget == null || _onCameraArrived == null) return;

        Transform camTransform = Camera.main.transform;
        Transform targetTransform = _currentTarget.transform;

        float posDiff = Vector3.Distance(camTransform.position, targetTransform.position);
        float rotDiff = Quaternion.Angle(camTransform.rotation, targetTransform.rotation);

        if (posDiff < PositionTolerance && rotDiff < RotationTolerance)
        {
            _onCameraArrived.Invoke();
            _onCameraArrived = null;
            _currentTarget = null;
        }
    }
    
    [ContextMenu("Go To Main Camera")]
    public void GoToMain()
    {
        GoToMain(null);
    }

    [ContextMenu("Go To Settings Camera")]
    public void GoToSettings()
    {
        GoToSettings(null);
    }

    [ContextMenu("Go To Shop Camera")]
    public void GoToShop()
    {
        MenuUIController.Instance.CloseMenuButtons().OnComplete((() =>
        {
            GoToShop(null);
        }));
    }
    
    public void GoToMain(Action onComplete)
    {
        SetActiveCamera(mainCam, onComplete);
    }

    public void GoToSettings(Action onComplete)
    {
        SetActiveCamera(settingsCam, onComplete);
    }

    public void GoToShop(Action onComplete)
    {
        SetActiveCamera(shopCam, onComplete);
    }

    private void SetActiveCamera(CinemachineVirtualCamera targetCam, Action onComplete = null)
    {
        mainCam.Priority = 0;
        settingsCam.Priority = 0;
        shopCam.Priority = 0;

        targetCam.Priority = 10;

        _currentTarget = targetCam;
        _onCameraArrived = onComplete;
    }
}
