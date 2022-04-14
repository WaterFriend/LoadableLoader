using UnityEngine;

public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance => _instance;

    protected virtual void Awake()
    {
        if( _instance == null)
        {
            _instance = gameObject.GetComponent<T>();
            if (_instance != null)
                DontDestroyOnLoad(_instance.gameObject);
        }
        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            throw new System.Exception($"[DontDestroySingleton] Instance of {GetType().FullName} already exists, removing {ToString()}");
        }
    }
}
