using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public abstract class ScriptableObjectEditorWindow<ObjectType> : EditorWindow where ObjectType:ScriptableObjectExtended
    {
        /****************
        * Color, layout *
        ****************/
        protected static readonly Color COLOR_DEFAULT      = new Color(1f, 1f, 1f);
        protected static readonly Color COLOR_SAVE         = new Color(0.2f, 0.9f, 0.2f);
        protected static readonly Color COLOR_NEW          = new Color(0.2f, 0.9f, 0.2f);
        protected static readonly Color COLOR_CURRENT      = new Color(0.4f, 0.4f, 1.0f);
        protected static readonly Color COLOR_DUPLICATE    = Color.cyan;
        protected static readonly Color COLOR_DELETE       = new Color(1.0f, 0.0f, 0.0f);
        protected static readonly Color COLOR_INACTIVE     = new Color(0.7f, 0.7f, 0.7f);

        protected const float SCROLLLIST_WIDTH_PERCENT = 0.25f;

        protected Color m_DefaultGUIColor;
        protected Color m_DefaultGUIBackgroundColor;

        /**********
        * Helpers *
        ***********/
        public static List<T> GetAllScriptableObjectsOfType<T>() where T : ScriptableObject
        {
            List<T> res = new List<T>();
            string typeName = typeof(T).FullName;

            string[] guids = AssetDatabase.FindAssets("t:" + typeName, new string[] { "Assets" });
            foreach (string guid in guids)
                res.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));

            return res;
        }

        /*******
        * Core *
        *******/ 
        // Abstract parameters
        protected abstract string c_ObjectName { get; }
        protected abstract string c_ObjectFullPath { get; }

        // Private variables
        private List<ObjectType> m_AllObjects;

        private Vector2 m_MenuScrollView = Vector2.zero;
        private Vector2 m_MainWindowScrollView = Vector2.zero;

        protected ObjectType m_CurrentObject;
        protected SerializedObject m_CurrentSerializedObject;

        private bool m_Dirty = false;

        /********************
        * Drawing Callbacks *
        ********************/ 
        public void OnGUI()
        {
            m_DefaultGUIColor = GUI.color;
            m_DefaultGUIBackgroundColor = GUI.backgroundColor;

            EditorGUILayout.BeginHorizontal();
            DrawLeftMenu();
            DrawMainWindow();
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void OnEnable()
        {
            RefreshObjectList();
        }

        void OnFocus()
        {
            RefreshObjectList();
        }

        /************
        * Left Menu *
        ************/
        string m_SearchText = "";

        private bool _StringContains(string cheatName, string[] searchTexts)
        {
            foreach (string txt in searchTexts)
            {
                if (!cheatName.ToUpper().Contains(txt.ToUpper()))
                    return false;
            }
            return true;
        }

        private void DrawLeftMenu()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(SCROLLLIST_WIDTH_PERCENT * position.width));
            {
                EditorGUILayout.LabelField("Menu", EditorStyles.boldLabel);

                m_MenuScrollView = EditorGUILayout.BeginScrollView(m_MenuScrollView, false, false);
                {
                    GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
                    {
                        m_SearchText = "";
                    }
                    m_SearchText = GUILayout.TextField(m_SearchText, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();

                    string[] searchTexts = m_SearchText.Split(' ');

                    // New
                    GUI.backgroundColor = COLOR_NEW;
                    if (GUILayout.Button("New " + c_ObjectName))
                    {
                        NewObject();
                    }
                    GUI.backgroundColor = m_DefaultGUIBackgroundColor;

                    DrawCustomFunctions();
                    EditorGUILayout.Space();

                    // List of all objects
                    EditorGUILayout.LabelField(c_ObjectName + "s", EditorStyles.boldLabel);

                    m_AllObjects = GetAllScriptableObjectsOfType<ObjectType>();

                    for (int i = 0; i < m_AllObjects.Count; ++i)
                    {
                        ObjectType thisObject = m_AllObjects[i];
                        if (thisObject != null)
                        {
                            if (_StringContains(thisObject.FileName, searchTexts))
                            {
                                if (thisObject == m_CurrentObject)
                                {
                                    GUI.backgroundColor = COLOR_CURRENT;
                                }

                                if (GUILayout.Button(thisObject.FileName))
                                {
                                    SelectObject(thisObject);
                                }
                            }

                            GUI.backgroundColor = m_DefaultGUIBackgroundColor;
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        /**************
        * Main Window *
        **************/
        private void DrawMainWindow()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginVertical("box");
            m_MainWindowScrollView = EditorGUILayout.BeginScrollView(m_MainWindowScrollView, false, false);
            {
                GUIStyle centeredLabel = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.UpperCenter
                };
                EditorGUILayout.LabelField(c_ObjectName + " Edition", centeredLabel);

                EditorGUILayout.Space();
                if (m_CurrentObject != null)
                {
                    EditorGUI.BeginChangeCheck();
                    m_CurrentObject.FileName = EditorGUILayout.TextField(c_ObjectName + " File Name: ", m_CurrentObject.FileName);

                    DrawEditor(m_CurrentObject);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_Dirty = true;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Select an object or create a new one in the menu", MessageType.Info);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            BottomControlsPanel();
            EditorGUILayout.EndVertical();
        }

        private void BottomControlsPanel()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (Button("Save", COLOR_SAVE, m_CurrentObject != null && m_Dirty))
            {
                SaveCurrentObject();
            }
            EditorGUILayout.Space();
            if (Button("Duplicate " + c_ObjectName, COLOR_DUPLICATE, m_CurrentObject != null))
            {
                ObjectType newObject = DuplicateCurrentObject();
                SelectObject(newObject);
            }
            EditorGUILayout.Space();
            if (Button("Delete " + c_ObjectName, COLOR_DELETE, m_CurrentObject != null))
            {
                DeleteCurrentObject();
            }
            if (Button("Revert " + c_ObjectName, COLOR_DELETE, m_CurrentObject != null && m_Dirty))
            {
                RevertCurrentObject();
            }
            EditorGUILayout.EndHorizontal();
        }

        /*******
        * CRUD *
        *******/ 
        protected ObjectType NewObject(string objectName = null)
        {
            if (objectName == null)
            {
                objectName = "New" + c_ObjectName;
            }
            ObjectType data = ScriptableObjectUtility.CreateAsset<ObjectType>(string.Concat(c_ObjectFullPath, "/",objectName,".asset"));


            data.FileName = objectName;
            RefreshObjectList();
            SelectObject(data);
            OnObjectNew(data);
            return data;
        }

        protected void SaveObject(ObjectType o)
        {
            o.OnSave();
            EditorUtility.SetDirty(o);
            if (ScriptableObjectUtility.GetAssetNameWithoutExtension(o) != o.FileName)
            {
                ScriptableObjectUtility.RenameAsset(o, o.FileName);
            }
            AssetDatabase.SaveAssets();
            m_Dirty = false;
            OnObjectSaved(o);
        }

        private void SaveCurrentObject()
        {
            Assert.IsNotNull(m_CurrentObject);

            if (m_Dirty)
            {
                SaveObject(m_CurrentObject);
            }
        }

        private ObjectType DuplicateCurrentObject()
        {
            ObjectType clone = ScriptableObjectUtility.DuplicateAsset(m_CurrentObject);

            clone.FileName += " (Clone)";
            RefreshObjectList();
            OnObjectDuplicated(clone);
            return clone;
        }

        private void DeleteCurrentObject()
        {
            OnObjectDeleted(m_CurrentObject);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(m_CurrentObject));
            m_Dirty = false;
            RefreshObjectList();
        }

        private void RevertCurrentObject()
        {
            ScriptableObjectUtility.ReloadAsset(ref m_CurrentObject);
            OnSelectionChanged(m_CurrentObject, m_CurrentObject);
            m_Dirty = false;
        }

        protected void SelectObject(ObjectType obj)
        {
            ObjectType oldObject = m_CurrentObject;

            if (m_Dirty)
            {
                if (EditorUtility.DisplayDialog("Unsaved changes", "You have unsaved changes on your current object. Do you want to save them ?", "Yes", "No"))
                {
                    SaveCurrentObject();
                }
                else
                {
                    m_Dirty = false;
                }
            }
            obj.RefreshLoDReferences();
            obj.Load();
            m_CurrentObject = obj;
            m_CurrentSerializedObject = new SerializedObject(obj);
            GUIUtility.keyboardControl = 0;
            Undo.RecordObject(m_CurrentObject, "Changed Object");
            OnSelectionChanged(oldObject, m_CurrentObject);
        }

        /********************
        * Virtual callbacks *
        ********************/
        protected virtual void OnObjectNew(ObjectType obj)
        {
        }

        protected virtual void OnObjectDuplicated(ObjectType obj)
        {
        }

        protected virtual void OnObjectSaved(ObjectType obj)
        {
        }

        protected virtual void OnObjectDeleted(ObjectType obj)
        {
        }

        protected virtual void OnSelectionChanged(ObjectType oldObj, ObjectType newObj)
        {
        }

        /******************
        * Drawing Helpers *
        ******************/
        protected static bool Button(string text, Color background, params GUILayoutOption[] options)
        {
            return Button(text, background, true, options);
        }

        protected static bool Button(string text, Color background, bool enabled, params GUILayoutOption[] options)
        {
            return Button(text, background, enabled, "button", options);
        }

        protected static bool Button(string text, Color background, bool enabled, GUIStyle style, params GUILayoutOption[] options)
        {
            bool oldEnabled = GUI.enabled;
            Color oldBackground = GUI.backgroundColor;

            GUI.enabled = enabled;
            GUI.backgroundColor = background;
            bool result = GUILayout.Button(text, style, options);
            GUI.enabled = oldEnabled;
            GUI.backgroundColor = oldBackground;
            return result;
        }

        protected static void DrawProperty(SerializedObject obj, string propertyName, params GUILayoutOption[] options)
        {
            SerializedProperty property = obj.FindProperty(propertyName);
            EditorGUILayout.PropertyField(property, true, options);
        }

        protected static void DrawProperty(SerializedObject obj, string propertyName, GUIContent label, params GUILayoutOption[] options)
        {
            SerializedProperty property = obj.FindProperty(propertyName);
            EditorGUILayout.PropertyField(property, label, true, options);
        }

        /***************************
        * Core overridable methods *
        ***************************/
        protected virtual void DrawEditor(ObjectType target)
        {
            Dictionary<string, FieldInfo> lodFields = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo field in typeof(ObjectType).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object[] attributes = field.GetCustomAttributes(typeof(LoadOnDemand), true);
                if (attributes.Length == 1)
                {
                    LoadOnDemand lodAttribute = (LoadOnDemand)attributes[0];
                    lodFields.Add(lodAttribute.FieldName, field);
                }
            }

            SerializedObject targetSerialized = new SerializedObject(target);
            SerializedProperty nextProperty;
            FieldInfo[] fields = typeof(ObjectType).GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (fields[i].Name != "FileName")
                {
                    nextProperty = targetSerialized.FindProperty(fields[i].Name);
                    try
                    {
                        EditorGUILayout.PropertyField(nextProperty, true);
                    }
                    catch (System.Exception e)
                    {
                        if (e.GetType() != typeof(ExitGUIException))
                        {
                            Object newValue = EditorGUILayout.ObjectField(fields[i].Name, (Object)fields[i].GetValue(target), fields[i].FieldType, false);

                            fields[i].SetValue(target, newValue);
                        }
                    }
                }
                if (lodFields.ContainsKey(fields[i].Name))
                {
                    EditorGUILayout.LabelField("Load on demand", EditorStyles.boldLabel, GUILayout.Width(115));
                    EditorGUILayout.EndHorizontal();
                    object fieldVal = lodFields[fields[i].Name].GetValue(target);
                    if (fieldVal != null)
                    {
                        LoadOnDemandInfo lodInfo = (LoadOnDemandInfo)fieldVal;
                        if (lodInfo.ResourcePath != null && lodInfo.ResourcePath.StartsWith("Assets/"))
                        {
                            EditorGUILayout.HelpBox("This attribute is a Load on Demand field that requires either an object in a Resource folder or another Scriptable Object", MessageType.Error);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.EndHorizontal();
                }
            }
            targetSerialized.ApplyModifiedProperties();
        }

        protected virtual void DrawCustomFunctions()
        {
        }

        protected void DrawCustomFunction(string buttonName, System.Action action)
        {
            if (GUILayout.Button(buttonName))
                action();
        }

        protected void RefreshObjectList()
        {
            m_AllObjects = GetAllScriptableObjectsOfType<ObjectType>();
        }
    }
}