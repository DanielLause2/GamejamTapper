using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public class ScriptableObjectWizard : EditorWindow
    {
        /*******************
        * Consts and types *
        *******************/ 
        private const string TYPE_REGEXP = @"^[a-zA-Z][a-zA-Z0-9_\-]*$";
        private const string SUITE_FOLDER = "/PrimitiveFactory/ScriptableObjectSuite/Snippets/";

        private const float FIELDS_PANEL_WIDTH = 90f / 100;
        private const float FIELDS_TOGGLE_WIDTH = 10f;
        private const float FIELDS_CUSTOMTYPE_WIDTH = 72f;
        private const float FIELDS_LOD_WIDTH = 70f;
        private const float FIELDS_TYPE_WIDTH = 50f / 100;
        private const float FIELDS_NAME_WIDTH = 50f / 100;
        private const float FIELDS_REMOVEFIELD_WIDTH = 30f;

        private class FieldData
        {
            public string FieldName;
            public string FieldType;
            public bool LoadOnDemand;
            public bool IsCustomFieldType;
        }

        private static readonly List<string> COMMON_FIELD_TYPES = new List<string>
        {
            "string",
            "int",
            "float",
            "bool",
            "Transform",
            "GameObject",
            "AudioClip",
            "Sprite",
            "Texture2D",
            "Material",
        };

        /**************
        * Wizard data *
        **************/ 
        private string m_TypeName = "MyScriptableObject";
        private string m_MenuPath = string.Concat("MyGame/MyObjectEditorWindow");
        private string m_ObjectsFolder = "";
        private string m_ScriptsFolder = "";
        private List<FieldData> m_Fields = new List<FieldData>();

        private bool m_PreviewRollout = false;
        private Vector2 m_MainScroll = Vector2.zero;

        /*******
        * Init *
        *******/
        internal static void ShowWindow()
        {
            ScriptableObjectWizard window = GetWindow<ScriptableObjectWizard>("Scriptable Object Wizard");
            window.Show();
        }

        public void OnEnable()
        {
            if (m_ObjectsFolder == "")
                m_ObjectsFolder = "Assets/Resources/MyScriptableObjects";

            if (m_ScriptsFolder == "")
                m_ScriptsFolder = "Assets/Scripts";
        }

        /*****************
        * Core UI Method *
        *****************/ 
        public void OnGUI()
        {
            bool error = false;

            EditorGUILayout.BeginVertical("box");
            {
                m_MainScroll = EditorGUILayout.BeginScrollView(m_MainScroll);
                //---- Info ----//
                // Type Name
                EditorGUILayout.LabelField("Type Info", EditorStyles.boldLabel);
                m_TypeName = EditorGUILayout.TextField("Type Name", m_TypeName);

                if (!Regex.IsMatch(m_TypeName, TYPE_REGEXP))
                {
                    error = true;
                    EditorGUILayout.HelpBox("Type Name must start with an uppercase letter and can only contain alphanumeric characters and '-' '_'", MessageType.Error);
                }

                EditorGUILayout.Space();

                m_MenuPath = EditorGUILayout.TextField("Menu Access", m_MenuPath);

                //---- Folders ----//
                EditorGUILayout.LabelField("Folders Info", EditorStyles.boldLabel);

                // Objects
                EditorGUILayout.BeginHorizontal();
                { 
                    m_ObjectsFolder = EditorGUILayout.TextField("Objects Folder", m_ObjectsFolder);
                    if (GUILayout.Button("...", GUILayout.Width(50)))
                    {
                        m_ObjectsFolder = string.Concat("Assets", EditorUtility.OpenFolderPanel("Choose folder", "Assets", null).Replace(Application.dataPath, ""));
                    }
                }
                EditorGUILayout.EndHorizontal();

                // Scripts
                EditorGUILayout.BeginHorizontal();
                {
                    m_ScriptsFolder = EditorGUILayout.TextField("Scripts Folder", m_ScriptsFolder);
                    if (GUILayout.Button("...", GUILayout.Width(50)))
                    {
                        m_ScriptsFolder = string.Concat("Assets", EditorUtility.OpenFolderPanel("Choose folder", "Assets", null).Replace(Application.dataPath, ""));
                    }
                }
                EditorGUILayout.EndHorizontal();

                string objectClassFile = string.Concat(m_ScriptsFolder, "/", m_TypeName, ".cs");
                string windowClassFile = string.Concat(m_ScriptsFolder, "/Editor/", m_TypeName, "Window.cs");

                if (File.Exists(string.Concat(Application.dataPath, "/", objectClassFile.Substring(6))))
                {
                    EditorGUILayout.HelpBox(string.Concat("File ", objectClassFile, " already exists"), MessageType.Error);
                    error = true;
                }
                else if (File.Exists(string.Concat(Application.dataPath, "/", windowClassFile.Substring(6))))
                {
                    EditorGUILayout.HelpBox(string.Concat("File ", windowClassFile, " already exists"), MessageType.Error);
                    error = true;
                }
                else
                {
                    string scriptsInfo = string.Concat("The following scripts will be created:\n- ", objectClassFile, "\n- ", windowClassFile);
                    EditorGUILayout.HelpBox(scriptsInfo, MessageType.Info);
                }

                EditorGUILayout.Space();

                //---- Fields ----//
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Fields", EditorStyles.boldLabel);

                    float width = EditorGUIUtility.currentViewWidth * 0.9f;
                    float flexibleWidth = width - FIELDS_CUSTOMTYPE_WIDTH - FIELDS_LOD_WIDTH - FIELDS_REMOVEFIELD_WIDTH;

                    if (m_Fields.Count > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Cust. Type", EditorStyles.boldLabel, GUILayout.Width(FIELDS_CUSTOMTYPE_WIDTH));
                        EditorGUILayout.LabelField("Type", EditorStyles.boldLabel, GUILayout.Width(flexibleWidth * FIELDS_TYPE_WIDTH));
                        EditorGUILayout.LabelField("Name", EditorStyles.boldLabel, GUILayout.Width(flexibleWidth * FIELDS_NAME_WIDTH));
                        EditorGUILayout.LabelField("Load on D.", EditorStyles.boldLabel, GUILayout.Width(FIELDS_LOD_WIDTH));
                        EditorGUILayout.EndHorizontal();
                    }

                    // Field enum
                    for (int i = 0; i < m_Fields.Count; i++)
                    {
                        FieldData fd = m_Fields[i];

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Space((FIELDS_CUSTOMTYPE_WIDTH - FIELDS_TOGGLE_WIDTH) * 0.5f);
                            fd.IsCustomFieldType = EditorGUILayout.Toggle(fd.IsCustomFieldType, GUILayout.Width(FIELDS_TOGGLE_WIDTH));
                            GUILayout.Space((FIELDS_CUSTOMTYPE_WIDTH - FIELDS_TOGGLE_WIDTH) * 0.5f);

                            if (fd.IsCustomFieldType)
                            {
                                fd.FieldType = EditorGUILayout.TextField(fd.FieldType, GUILayout.Width(flexibleWidth * FIELDS_TYPE_WIDTH));
                            }
                            else
                            {
                                if (COMMON_FIELD_TYPES.IndexOf(fd.FieldType) < 0)
                                    fd.FieldType = COMMON_FIELD_TYPES[0];

                                fd.FieldType = COMMON_FIELD_TYPES[EditorGUILayout.Popup(COMMON_FIELD_TYPES.IndexOf(fd.FieldType), COMMON_FIELD_TYPES.ToArray(), GUILayout.Width(flexibleWidth * FIELDS_TYPE_WIDTH))];
                            }
                            fd.FieldName = EditorGUILayout.TextField(fd.FieldName, GUILayout.Width(flexibleWidth * FIELDS_NAME_WIDTH));

                            GUILayout.Space((FIELDS_LOD_WIDTH - FIELDS_TOGGLE_WIDTH) * 0.5f);
                            fd.LoadOnDemand = EditorGUILayout.Toggle(fd.LoadOnDemand, GUILayout.Width(FIELDS_TOGGLE_WIDTH));
                            GUILayout.Space((FIELDS_LOD_WIDTH - FIELDS_TOGGLE_WIDTH) * 0.5f);

                            if (GUILayout.Button("-", GUILayout.Width(FIELDS_REMOVEFIELD_WIDTH)))
                            {
                                m_Fields.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    // Field buttons
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add Field"))
                        {
                            m_Fields.Add(new FieldData());
                        }
                        if (GUILayout.Button("Clear Fields"))
                        {
                            m_Fields.Clear();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();
            }

            //---- Preview ----//
            m_PreviewRollout = EditorGUILayout.Foldout(m_PreviewRollout, "Class Preview");
            if (m_PreviewRollout)
            {
                GUI.enabled = false;
                EditorGUILayout.TextArea(GenerateObjectScriptFile());
                GUI.enabled = true;
            }

            EditorGUILayout.Space();

            //---- Final button ----//
            GUI.enabled = !error;
            if (GUILayout.Button("Generate"))
            {
                // Objects folder
                string pathOnDisk = string.Concat(Application.dataPath, "/", m_ObjectsFolder.Substring(6));
                if (!Directory.Exists(pathOnDisk))
                    Directory.CreateDirectory(pathOnDisk);
                
                // Object class file
                pathOnDisk = string.Concat(Application.dataPath, "/", m_ScriptsFolder.Substring(6));
                if (!Directory.Exists(pathOnDisk))
                    Directory.CreateDirectory(pathOnDisk);

                pathOnDisk = string.Concat(pathOnDisk, "/", m_TypeName, ".cs");
                StreamWriter fileStream = File.CreateText(pathOnDisk);
                fileStream.Write(GenerateObjectScriptFile());
                fileStream.Close();

                // Editor window file
                pathOnDisk = string.Concat(Application.dataPath, "/", m_ScriptsFolder.Substring(6), "/Editor");
                if (!Directory.Exists(pathOnDisk))
                    Directory.CreateDirectory(pathOnDisk);

                pathOnDisk = string.Concat(pathOnDisk, "/", m_TypeName, "Window.cs");
                fileStream = File.CreateText(pathOnDisk);
                fileStream.Write(GenerateWindowScriptFile());
                fileStream.Close();

                m_TypeName = "MyScriptableObject";
                m_MenuPath = string.Concat("MyGame/MyObjectEditorWindow");
                m_ObjectsFolder = "";
                m_ScriptsFolder = "";
                m_Fields.Clear();
                EditorUtility.DisplayDialog("Generation successful", "Your files have been correctly created", "OK");

                AssetDatabase.Refresh();
            }
            GUI.enabled = true;

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /*******************************
        * Snippet reading / generation *
        *******************************/ 
        private string GenerateObjectScriptFile()
        {
            string objectSnippet = ReadSnippet("Object");

            string fieldsString = "";
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldData fd = m_Fields[i];

                string fieldSnippet;
                if (fd.LoadOnDemand)
                    fieldSnippet = ReadSnippet("FieldLoD");
                else
                    fieldSnippet = ReadSnippet("Field");

                fieldSnippet = fieldSnippet.Replace("@FieldType@", fd.FieldType);
                fieldSnippet = fieldSnippet.Replace("@FieldName@", fd.FieldName);

                fieldsString += fieldSnippet;
                if (i < m_Fields.Count - 1)
                    fieldsString += "\n";
            }

            objectSnippet = objectSnippet.Replace("@Fields@", fieldsString);

            objectSnippet = objectSnippet.Replace("@ClassName@", m_TypeName);

            return objectSnippet;
        }

        private string GenerateWindowScriptFile()
        {
            string windowSnippet = ReadSnippet("Window");

            windowSnippet = windowSnippet.Replace("@ClassName@", m_TypeName);
            windowSnippet = windowSnippet.Replace("@MenuPath@", m_MenuPath);
            windowSnippet = windowSnippet.Replace("@FullPath@", m_ObjectsFolder);

            return windowSnippet;
        }

        private static string ReadSnippet(string name)
        {
            return File.ReadAllText(string.Concat(Application.dataPath, SUITE_FOLDER, name, ".snippet"));
        }
    }
}