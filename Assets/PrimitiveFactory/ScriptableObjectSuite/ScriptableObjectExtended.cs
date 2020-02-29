using System.Reflection;
using UnityEngine;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public class ScriptableObjectExtended : ScriptableObject
    {
        public string FileName;

#if UNITY_EDITOR
        public void OnSave()
        {
            foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object[] attributes = field.GetCustomAttributes(typeof(LoadOnDemand), true);
                if (attributes.Length == 1)
                {
                    LoadOnDemand attribute = (LoadOnDemand)attributes[0];
                    FieldInfo linkedField = GetType().GetField(attribute.FieldName);

                    object value = linkedField.GetValue(this);
                    Object valueAsUnityObject = (Object)value;
                    if (value != null)
                    {
                        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(valueAsUnityObject);
                        string assetGuid = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
                        string resourcePath = AssetToResourcePath(assetPath);
                        LoadOnDemandInfo lodInfo = new LoadOnDemandInfo(assetGuid, resourcePath);
                        field.SetValue(this, lodInfo);
                    }
                    else
                    {
                        field.SetValue(this, null);
                    }
                }
            }
        }

        public static void RefreshAllLoDReferences()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObjectExtended", new string[] { "Assets" });
            foreach (string guid in guids)
            {
                ScriptableObjectExtended o = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObjectExtended>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                o.RefreshLoDReferences();
            }
        }

        public void RefreshLoDReferences()
        {
            foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object[] attributes = field.GetCustomAttributes(typeof(LoadOnDemand), true);
                if (attributes.Length == 1)
                {
                    LoadOnDemand attribute = (LoadOnDemand)attributes[0];
                    LoadOnDemandInfo lodInfo = (LoadOnDemandInfo)field.GetValue(this);
                    if (lodInfo != null && lodInfo.EditorGUID != null)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(lodInfo.EditorGUID);
                        if (path == null)
                        {
                            Debug.LogWarning(string.Concat("[Scriptable Object Suite] ", name, " - Asset with guid ", lodInfo.EditorGUID, " referenced for ", attribute.FieldName, " does not exist anymore. Removing reference"));
                            lodInfo.EditorGUID = null;
                            lodInfo.ResourcePath = null;
                        }
                        else
                        {
                            string previousPath = lodInfo.ResourcePath;
                            lodInfo.ResourcePath = AssetToResourcePath(path);
                            if (previousPath != lodInfo.ResourcePath)
                            {
                                Debug.LogWarning(string.Concat("[Scriptable Object Suite] ", name, " - Updated reference path for ", attribute.FieldName, " from ", previousPath, " to ", lodInfo.ResourcePath));
                            }
                        }
                    } 
                }
            }
        }
#endif

        public void Load(string fieldName = null, bool async = false)
        {
            foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object[] attributes = field.GetCustomAttributes(typeof(LoadOnDemand), true);
                if (attributes.Length == 1)
                {
                    LoadOnDemand attribute = (LoadOnDemand)attributes[0];
                    string attributeFieldName = attribute.FieldName;
                    if (fieldName == null || attributeFieldName == fieldName)
                    {
                        FieldInfo linkedField = GetType().GetField(attributeFieldName);
                        LoadOnDemandInfo fieldValue = (LoadOnDemandInfo)field.GetValue(this);
                        if (fieldValue != null)
                        {
                            if (async)
                            {
                                if (typeof(Sprite) == linkedField.FieldType)
                                {
                                    linkedField.SetValue(this, Resources.LoadAsync<Sprite>(fieldValue.ResourcePath));
                                }
                                else
                                {
                                    linkedField.SetValue(this, Resources.LoadAsync(fieldValue.ResourcePath));
                                }
                            }
                            else
                            {
                                if (typeof(Sprite) == linkedField.FieldType)
                                {
                                    linkedField.SetValue(this, Resources.Load<Sprite>(fieldValue.ResourcePath));
                                }
                                else
                                {
                                    linkedField.SetValue(this, Resources.Load(fieldValue.ResourcePath));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Unload(string fieldName = null)
        {
            foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object[] attributes = field.GetCustomAttributes(typeof(LoadOnDemand), true);
                if (attributes.Length == 1)
                {
                    LoadOnDemand attribute = (LoadOnDemand)attributes[0];
                    string attributeFieldName = attribute.FieldName;
                    if (fieldName == null || attributeFieldName == fieldName)
                    {
                        FieldInfo linkedField = GetType().GetField(attributeFieldName);
                        linkedField.SetValue(this, null);
                    }
                }
            }
        }

        public static string AssetToResourcePath(string path)
        {
            string find = "Resources/";
            if (path.IndexOf(find) == -1)
                return path;

            int beginning = path.IndexOf(find) + find.Length;
            int end = path.LastIndexOf('.');
            return path.Substring(beginning, end - beginning);
        }
    }
}