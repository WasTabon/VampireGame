using UnityEngine;
using Cinemachine;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera mainCam;
    [SerializeField] public CinemachineVirtualCamera settingsCam;
    [SerializeField] public CinemachineVirtualCamera shopCam;

    [ContextMenu("Go To Main Camera")]
    public void GoToMain()
    {
        SetActiveCamera(mainCam);
    }

    [ContextMenu("Go To Settings Camera")]
    public void GoToSettings()
    {
        SetActiveCamera(settingsCam);
    }

    [ContextMenu("Go To Shop Camera")]
    public void GoToShop()
    {
        SetActiveCamera(shopCam);
    }

    private void SetActiveCamera(CinemachineVirtualCamera targetCam)
    {
        mainCam.Priority = 0;
        settingsCam.Priority = 0;
        shopCam.Priority = 0;

        targetCam.Priority = 10;
    }
}