using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.Collections.Generic;

[CustomEditor(typeof(MenuCameraController))]
public class MenuCameraVisibilityEditor : Editor
{
    private static List<GameObject> _disabledObjects = new List<GameObject>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Скрыть объекты вне зоны всех камер"))
        {
            MenuCameraController controller = (MenuCameraController)target;

            if (controller.mainCam == null || controller.settingsCam == null || controller.shopCam == null)
            {
                Debug.LogWarning("Одна или несколько камер не назначены.");
                return;
            }

            Camera sceneCam = SceneView.lastActiveSceneView.camera;
            Transform[] allTransforms = FindObjectsOfType<Transform>();
            _disabledObjects.Clear();

            foreach (Transform obj in allTransforms)
            {
                if (obj == controller.transform) continue;
                if (obj.GetComponent<Renderer>() == null) continue;
                if (PrefabUtility.IsPartOfAnyPrefab(obj.gameObject) && !PrefabUtility.IsPartOfPrefabInstance(obj.gameObject)) continue;

                bool visible = IsVisibleFromAny(obj, controller, sceneCam);

                if (!visible && obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(false);
                    _disabledObjects.Add(obj.gameObject);
                }
            }

            Debug.Log($"Скрыто {_disabledObjects.Count} объектов.");
        }

        if (GUILayout.Button("Показать все скрытые ранее объекты"))
        {
            int restoredCount = 0;
            foreach (GameObject go in _disabledObjects)
            {
                if (go != null)
                {
                    go.SetActive(true);
                    restoredCount++;
                }
            }

            _disabledObjects.Clear();
            Debug.Log($"Восстановлено {restoredCount} объектов.");
        }
    }

    private bool IsVisibleFromAny(Transform obj, MenuCameraController controller, Camera sceneCam)
    {
        Vector3 pos = obj.position;
        return
            IsVisibleFrom(controller.mainCam, pos, sceneCam) ||
            IsVisibleFrom(controller.settingsCam, pos, sceneCam) ||
            IsVisibleFrom(controller.shopCam, pos, sceneCam);
    }

    private bool IsVisibleFrom(CinemachineVirtualCamera vcam, Vector3 worldPos, Camera sceneCam)
    {
        if (vcam == null) return false;

        vcam.Priority = 100;
        SceneView.RepaintAll();

        Vector3 viewportPoint = sceneCam.WorldToViewportPoint(worldPos);

        return viewportPoint.z > 0 &&
               viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}
