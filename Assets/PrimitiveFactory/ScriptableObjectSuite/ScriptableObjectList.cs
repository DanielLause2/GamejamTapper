using System.Collections.Generic;
using UnityEngine;

namespace PrimitiveFactory.ScriptableObjectSuite
{
    public abstract class ScriptableObjectList<T> : ScriptableObject where T : ScriptableObjectExtended
    {
        [SerializeField]
        protected List<T> m_ObjectList = new List<T>();

        public abstract List<T> GetOrderedObjectList();

        public T GetObjectByName(string name)
        {
            for (int i = 0; i < m_ObjectList.Count; i++)
            {
                if (m_ObjectList[i] != null && m_ObjectList[i].FileName == name)
                {
                    return m_ObjectList[i];
                }
            }

            throw new System.Exception("Object not found");
        }

#if UNITY_EDITOR
        // Only used for editor purposes
        public List<T> ObjectList { get { return m_ObjectList; } }

        public static ListType CreateNewList<ListType>() where ListType:ScriptableObjectList<T>
        {
            return ScriptableObjectUtility.CreateAsset<ListType>("Assets/" + typeof(T).ToString() + "List.asset");
        }
#endif
    }
}