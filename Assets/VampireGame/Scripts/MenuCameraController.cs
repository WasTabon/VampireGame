using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [SerializeField] public CinemachineVirtualCamera mainCam;
    [SerializeField] public CinemachineVirtualCamera settingsCam;
    [SerializeField] public CinemachineVirtualCamera shopCam;

    private bool _isLevels;
    private bool _isShop;
    private bool _isSettings;
    
    private CinemachineVirtualCamera _currentTarget;
    private Action _onCameraArrived;

    private const float PositionTolerance = 0.05f;
    private const float RotationTolerance = 1f;

    private void Start()
    {
        GoToMain(null);
    }

    private void Update()
    {
        if (_currentTarget == null || _onCameraArrived == null) return;

        Transform camTransform = _camera.transform;
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
        if (_isShop)
        {
            MenuUIController.Instance.CloseShopPanelTween()
                .OnComplete((() =>
                {
                    GoToMain((() =>
                    {
                        MenuUIController.Instance.OpenMenuButtonTween();
                    }));
                }));
        }
    }

    [ContextMenu("Go To Settings Camera")]
    public void GoToSettings()
    {
        MenuUIController.Instance.CloseMenuButtonTween().OnComplete((() =>
        {
            GoToSettings(null);
        }));
    }

    [ContextMenu("Go To Shop Camera")]
    public void GoToShop()
    {
        _isShop = true;
        MenuUIController.Instance.CloseMenuButtonTween().OnComplete((() =>
        {
            GoToShop(() =>
            {
                Debug.Log("MoveCompleted");
                MenuUIController.Instance.OpenShopPanel();
            });
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
