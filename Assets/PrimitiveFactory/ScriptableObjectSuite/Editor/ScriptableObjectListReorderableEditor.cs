using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public abstract class ScriptableObjectReorderableListEditor<T> : Editor where T:ScriptableObjectExtended
    {
        private ReorderableList m_ReorderableList;
        protected ScriptableObjectList<T> m_BaseList;

        private void OnEnable()
        {
            m_BaseList = (ScriptableObjectList<T>)target;

            m_ReorderableList = new ReorderableList(m_BaseList.ObjectList, typeof(T), true, true, true, true);

            // Add listeners to draw events
            m_ReorderableList.onAddCallback += AddItem;
            m_ReorderableList.onRemoveCallback += RemoveItem;

            m_ReorderableList.drawHeaderCallback += DrawHeader;
            m_ReorderableList.drawElementCallback += DrawElement;
        }

        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            m_ReorderableList.onAddCallback -= AddItem;
            m_ReorderableList.onRemoveCallback -= RemoveItem;

            m_ReorderableList.drawHeaderCallback -= DrawHeader;
            m_ReorderableList.drawElementCallback -= DrawElement;
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            T item = m_BaseList.ObjectList[index];

            EditorGUI.BeginChangeCheck();

            m_BaseList.ObjectList[index] = (T)EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, rect.height), item, typeof(T), false);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Skin List");
        }

        private void AddItem(ReorderableList list)
        {
            m_BaseList.ObjectList.Add(null);

            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            m_BaseList.ObjectList.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            // Actually draw the list in the inspector
            m_ReorderableList.DoLayoutList();

            if (GUILayout.Button("Add Missing Objects"))
            {
                List<T> skins = ScriptableObjectUtility.GetAllScriptableObjectsOfType<T>();
                for (int i = 0; i < skins.Count; i++)
                {
                    if (!m_BaseList.ObjectList.Contains(skins[i]))
                    {
                        m_BaseList.ObjectList.Add(skins[i]);
                    }
                }
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}