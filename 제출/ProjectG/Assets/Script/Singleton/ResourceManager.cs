using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _instance;
    public static ResourceManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ResourceManager>();

            return _instance;
        }
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform position)
    {
        GameObject prefab = Load<GameObject>(path);
        if(prefab == null)
        {
            Debug.Log($"Failed to Load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, position);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;

        Object.Destroy(obj);
    }
}
