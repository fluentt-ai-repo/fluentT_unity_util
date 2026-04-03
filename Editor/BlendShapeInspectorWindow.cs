using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FluentT.UnityUtil.Editor
{
    /// <summary>
    /// Tool to inspect blendshape names from selected FBX or SkinnedMeshRenderer.
    /// Shows relative path from root and allows copying the list.
    /// </summary>
    public class BlendShapeInspectorWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private string resultText = "";
        private bool hasResult;

        [MenuItem("FluentT/Tools/BlendShape Inspector")]
        public static void ShowWindow()
        {
            GetWindow<BlendShapeInspectorWindow>("BlendShape Inspector");
        }

        private void OnEnable()
        {
            Selection.selectionChanged += Repaint;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("BlendShape Inspector", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Select a GameObject with SkinnedMeshRenderer(s) in the Scene or Hierarchy,\n" +
                "or select an FBX/Model asset in the Project window.",
                MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("Inspect Selected", GUILayout.Height(28)))
            {
                InspectSelection();
            }

            EditorGUILayout.Space();

            if (hasResult)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy to Clipboard"))
                {
                    EditorGUIUtility.systemCopyBuffer = resultText;
                    Debug.Log("[BlendShapeInspector] Copied to clipboard.");
                }
                if (GUILayout.Button("Open in Text Editor"))
                {
                    OpenInTextEditor(resultText);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                EditorGUILayout.TextArea(resultText, EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndScrollView();
            }
        }

        private void InspectSelection()
        {
            var sb = new StringBuilder();
            bool found = false;

            // Case 1: Scene GameObject selected
            var go = Selection.activeGameObject;
            if (go != null && go.scene.IsValid())
            {
                var renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                if (renderers.Length > 0)
                {
                    Transform root = go.transform;
                    foreach (var skmr in renderers)
                    {
                        if (skmr.sharedMesh == null || skmr.sharedMesh.blendShapeCount == 0)
                            continue;

                        string relativePath = GetRelativePath(root, skmr.transform);
                        sb.AppendLine($"## {relativePath}  ({skmr.sharedMesh.blendShapeCount} blendshapes)");
                        for (int i = 0; i < skmr.sharedMesh.blendShapeCount; i++)
                        {
                            sb.AppendLine($"  [{i}] {skmr.sharedMesh.GetBlendShapeName(i)}");
                        }
                        sb.AppendLine();
                        found = true;
                    }
                }
            }

            // Case 2: Project asset (FBX/Model) selected
            if (!found)
            {
                foreach (var obj in Selection.objects)
                {
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    if (string.IsNullOrEmpty(assetPath)) continue;

                    // Check if it's a model asset
                    var importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                    if (importer == null) continue;

                    var allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                    foreach (var sub in allAssets)
                    {
                        var mesh = sub as Mesh;
                        if (mesh == null || mesh.blendShapeCount == 0) continue;

                        sb.AppendLine($"## {mesh.name}  ({mesh.blendShapeCount} blendshapes)  [asset: {assetPath}]");
                        for (int i = 0; i < mesh.blendShapeCount; i++)
                        {
                            sb.AppendLine($"  [{i}] {mesh.GetBlendShapeName(i)}");
                        }
                        sb.AppendLine();
                        found = true;
                    }
                }
            }

            if (!found)
            {
                sb.AppendLine("No blendshapes found in selection.");
                sb.AppendLine("Select a GameObject with SkinnedMeshRenderer or an FBX asset.");
            }

            resultText = sb.ToString();
            hasResult = true;
        }

        private static void OpenInTextEditor(string text)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "BlendShapeInspector.txt");
            File.WriteAllText(tempPath, text);

            // Opens with OS-associated default app for .txt
            EditorUtility.OpenWithDefaultApp(tempPath);
        }

        private static string GetRelativePath(Transform root, Transform target)
        {
            if (target == root)
                return root.name;

            var parts = new List<string>();
            var current = target;
            while (current != null && current != root)
            {
                parts.Add(current.name);
                current = current.parent;
            }
            parts.Add(root.name);
            parts.Reverse();
            return string.Join("/", parts);
        }
    }
}
