using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillEditor : OdinMenuEditorWindow
{
    private string originalScene;
    /// <summary>
    /// 摄像机
    /// </summary>
    private static GameObject CameraObj;
    public static GameObject editorObj;
    public static RenderTexture aView;


    [MenuItem("BattleTools/SkillEditor")]
    public static void OpenSkillEditor()
    {
        var window = GetWindow<SkillEditor>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        window.Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        return tree;
    }

    protected override void OnDestroy()
    {
        // 返回原场景
        EditorSceneManager.OpenScene(originalScene, OpenSceneMode.Single);
    }

    protected override void OnEnable()
    {
        originalScene = EditorSceneManager.GetActiveScene().path;
        EditorSceneManager.OpenScene("Assets/Scenes/SkillEditorScene.unity", OpenSceneMode.Single);
        editorObj = new GameObject("SkillEditor");
        CameraObj = new GameObject("myCamera");
        CameraObj.transform.SetParent(editorObj.transform);
        Camera cam = SkillEditor.CameraObj.AddComponent<Camera>();
        cam.depth = -1;
        aView = new RenderTexture(500, 500, 32);
        cam.targetTexture = aView;
        CameraObj.transform.position = new Vector3(0, 2.897f, 1.221f);
        CameraObj.transform.rotation = new Quaternion(0, 1, 0, 0);
    }
    protected override void OnBeginDrawEditors()
    {
        base.OnBeginDrawEditors();
        var rect = EditorGUILayout.GetControlRect(GUILayout.Width(500), GUILayout.Height(500f));
        GUI.DrawTexture(rect, aView);
    }
}
