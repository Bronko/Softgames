using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RectTransform), true)]
    public class RectTransformEditor : Editor
    {
        private Editor editorInstance;
        private Vector2 newAnchorMin;
        private Vector2 newAnchorMax;
        private RectTransform targetRectTransform;
        private RectTransform parentRect;
        private Vector2 parentDimensions;
        private float offset = 0;
        private bool foldout = true;

        private void OnEnable()
        {
            if (editorInstance != null)
                DestroyImmediate(editorInstance);
            
            Assembly ass = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type rtEditor = ass.GetType("UnityEditor.RectTransformEditor");
            targetRectTransform = (RectTransform) target;
            parentRect = targetRectTransform.parent != null ? targetRectTransform.parent.transform as RectTransform : null;
            var width = parentRect != null ? parentRect.rect.width : 60;
            var height = parentRect != null ? parentRect.rect.height : 60;
            parentDimensions = new Vector2(width, height);
            
            editorInstance = CreateEditor(target, rtEditor);
        }

        public override void OnInspectorGUI()
        {
            newAnchorMin = targetRectTransform.anchorMin;
            newAnchorMax = targetRectTransform.anchorMax;


            if (parentRect != null)
            {
                foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Anchor Helper");
                if (foldout)
                {
                    DrawAnchorsTool();
                }
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            editorInstance.OnInspectorGUI();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAnchorsTool()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            float scaleFactor = Mathf.Min(1f, 80 / Mathf.Max(parentDimensions.x, parentDimensions.y));
            var normalizedWidth = parentDimensions.x * scaleFactor;
            var normalizedHeight = parentDimensions.y * scaleFactor;
            
            Rect fullRect = GUILayoutUtility.GetRect(normalizedWidth + offset * 2, normalizedHeight + offset * 2, GUILayout.ExpandWidth(false));
            Rect graphicRect = GUILayoutUtility.GetRect(normalizedWidth, normalizedHeight, GUILayout.ExpandWidth(false));
            Vector2 fullRectCenter = new Vector2(fullRect.x + fullRect.width / 2, fullRect.y + fullRect.height / 2);
            float graphicRectX = fullRectCenter.x - graphicRect.width / 2;
            float graphicRectY = fullRectCenter.y - graphicRect.height / 2;
            
            graphicRect.x = graphicRectX;
            graphicRect.y = graphicRectY;
            
            Handles.DrawSolidRectangleWithOutline(graphicRect, Color.clear, Color.white);
            Handles.DrawSolidRectangleWithOutline(fullRect, Color.clear, Color.clear); 
            
            float minX = newAnchorMin.x;
            float minY = newAnchorMin.y;
            float maxX = 1f - newAnchorMax.x; 
            float maxY = 1f - newAnchorMax.y; 

            Vector2 minHandlePos = new Vector2(graphicRect.x + graphicRect.width * minX, graphicRect.y + graphicRect.height * (1f - minY));
            Vector2 maxHandlePos =
                new Vector2(graphicRect.x + graphicRect.width * (1f - maxX),
                    graphicRect.y + graphicRect.height * maxY); 
            
            Handles.color = Color.green;
            var targetRect = offset > 0 ? fullRect : graphicRect;
            Handles.DrawLine(new Vector2(minHandlePos.x, targetRect.y), new Vector2(minHandlePos.x, targetRect.y + targetRect.height), 0.3f);
            Handles.DrawLine(new Vector2(targetRect.x, minHandlePos.y), new Vector2(targetRect.x + targetRect.width, minHandlePos.y), 0.3f);
            Handles.DrawLine(new Vector2(maxHandlePos.x, targetRect.y), new Vector2(maxHandlePos.x, targetRect.y + targetRect.height), 0.3f);
            Handles.DrawLine(new Vector2(targetRect.x, maxHandlePos.y), new Vector2(targetRect.x + targetRect.width, maxHandlePos.y), 0.3f);
            
            Handles.color = new Color(0f, 1f, 0f, 0.3f);
            Vector3[] solidBoxVertices = new Vector3[4];
            solidBoxVertices[0] = new Vector3(minHandlePos.x, minHandlePos.y, 0f);
            solidBoxVertices[1] = new Vector3(minHandlePos.x, maxHandlePos.y, 0f);
            solidBoxVertices[2] = new Vector3(maxHandlePos.x, maxHandlePos.y, 0f);
            solidBoxVertices[3] = new Vector3(maxHandlePos.x, minHandlePos.y, 0f);
            Handles.DrawAAConvexPolygon(solidBoxVertices);
            
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("|X|   Left:", GUILayout.Width(60));
            minX = EditorGUILayout.FloatField(minX, GUILayout.Width(50));
            GUILayout.Label("Right:", GUILayout.Width(50));
            maxX = EditorGUILayout.FloatField(maxX, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(6f); 
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("|Y|   Top:", GUILayout.Width(60));
            maxY = EditorGUILayout.FloatField(maxY, GUILayout.Width(50));
            GUILayout.Label("Bottom:", GUILayout.Width(50));
            minY = EditorGUILayout.FloatField(minY, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
            
            minX = Mathf.Min(2, minX);
            minY = Mathf.Min(2,minY);
            maxX = Mathf.Min(2,maxX);
            maxY = Mathf.Min(2,maxY);

            var min = Mathf.Min(minX, maxX, minY, maxY);
            if (min >= 0)
                offset = 0;
            else
                offset = Mathf.Min(50, (min * -1) * 100);
            
            newAnchorMin = new Vector2(minX, minY);
            newAnchorMax = new Vector2(1f - maxX, 1f - maxY);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            if (GUI.changed)
            {
                Undo.RecordObject(targetRectTransform, "Anchor Change");
                targetRectTransform.anchorMin = newAnchorMin;
                targetRectTransform.anchorMax = newAnchorMax;
                EditorUtility.SetDirty(targetRectTransform);
            }
        }
        
        private void OnSceneGUI()
        {
            MethodInfo onSceneGUI_Method =
                editorInstance.GetType().GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);
            onSceneGUI_Method.Invoke(editorInstance, null);
        }

        private void OnDestroy()
        {
            if (editorInstance != null)
                DestroyImmediate(editorInstance);
        }
        
        private void OnDisable()
        {
            if (editorInstance != null)
                DestroyImmediate(editorInstance);
        }
    }