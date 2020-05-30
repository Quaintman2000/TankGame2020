using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
    public static bool IsInitiatlized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (IsInitiatlized)
        {
            Debug.LogError("[Singleton] Attemted to make 2nd singleton class");
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance == null)
        {
            instance = null;
        }
    }
}
