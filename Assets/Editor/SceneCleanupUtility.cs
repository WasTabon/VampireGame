using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneCleanupUtility : MonoBehaviour
{
    [MenuItem("Tools/Cleanup Scene/Remove Colliders & Set Static & Mixed Lights")]
    public static void CleanScene()
    {
        if (!EditorUtility.DisplayDialog("Cleanup Scene", "Это действие необратимо. Продолжить?", "Да", "Отмена"))
            return;

        int removedColliders = 0;
        int staticObjects = 0;
        int mixedLights = 0;
        
        foreach (Collider collider in GameObject.FindObjectsOfType<Collider>())
        {
            GameObject obj = collider.gameObject;
            Undo.DestroyObjectImmediate(collider);
            removedColliders++;
        }
        
        foreach (MeshRenderer renderer in GameObject.FindObjectsOfType<MeshRenderer>())
        {
            GameObject obj = renderer.gameObject;
            GameObjectUtility.SetStaticEditorFlags(obj, StaticEditorFlags.ContributeGI |
                                                        StaticEditorFlags.BatchingStatic |
                                                        StaticEditorFlags.NavigationStatic |
                                                        StaticEditorFlags.OccludeeStatic |
                                                        StaticEditorFlags.OccluderStatic |
                                                        StaticEditorFlags.OffMeshLinkGeneration |
                                                        StaticEditorFlags.ReflectionProbeStatic |
                                                        StaticEditorFlags.ContributeGI);
            staticObjects++;
        }
        
        foreach (Light light in GameObject.FindObjectsOfType<Light>())
        {
            if (light.lightmapBakeType != LightmapBakeType.Mixed)
            {
                Undo.RecordObject(light, "Set Light to Mixed");
                light.lightmapBakeType = LightmapBakeType.Mixed;
                mixedLights++;
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log($"✅ Завершено:\nУдалено коллайдеров: {removedColliders}\nStatic назначено: {staticObjects}\nСветов переведено в Mixed: {mixedLights}");
    }
}
