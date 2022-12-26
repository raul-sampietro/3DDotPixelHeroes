using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton pattern non-persistent among scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject
                {
                    name = typeof(T).Name,
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        } 
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

/// <summary>
/// Singleton pattern persistent among scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public virtual void Awake()
    {
        if (_instance != null) Destroy(gameObject);
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}