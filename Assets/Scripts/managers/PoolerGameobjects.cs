using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    public class PoolerGameobjects
    {
        private static List<GameObject> pool = new List<GameObject>();

        private static PoolerGameobjects instance = new PoolerGameobjects();
        public static PoolerGameobjects Instance
        {
            get
            {
                return instance;
            }
        }

        public PoolerGameobjects()
        {

        }

        public void SaveToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (!pool.Contains(gameObject)) pool.Add(gameObject);
        }

        public GameObject RetrieveFromPool(string tag)
        {
            for (int i = pool.Count; i >=0 ; i--)
            {
                GameObject go = pool[i];
                if(go.CompareTag(tag))
                {
                    pool.Remove(go);
                    return go;
                }
            }

            return null; // TODO : instantiate Gameobject from TAG dude !

        }

    }
}
