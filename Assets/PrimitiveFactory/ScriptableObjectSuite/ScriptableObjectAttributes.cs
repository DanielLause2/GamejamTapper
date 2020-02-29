using System;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field)]
    public class LoadOnDemand : Attribute
    {
        private string m_FieldName;

        public LoadOnDemand(string fieldName)
        {
            m_FieldName = fieldName;
        }

        public string FieldName { get { return m_FieldName; } }
    }

    [Serializable]
    public class LoadOnDemandInfo
    {
        public string EditorGUID;
        public string ResourcePath;

        public LoadOnDemandInfo(string editorGUID, string resourcePath)
        {
            EditorGUID = editorGUID;
            ResourcePath = resourcePath;
        }
    }
}