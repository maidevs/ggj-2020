using UnityEngine;

public class Singleton<Class> : MonoBehaviour where Class: MonoBehaviour
{
    private static Class _Instance;
    public static Class Instance {
        get {
            if(_Instance == null)
                _Instance = FindObjectOfType<Class>();

            return _Instance;
        }
        private set {
            _Instance = value;
        }
    }

}
