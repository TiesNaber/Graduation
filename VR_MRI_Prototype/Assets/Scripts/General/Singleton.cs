using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class will make a singleton from each class that inherits from it. This allows it to be called from anywhere without a refrence.
/// </summary>
/// <typeparam name="T">Name of the script to inherit from this class</typeparam>
public class Singleton<T> : MonoBehaviour where T : Component {
    private static T instance;  // interal refrence to this class.
    public static T Instance {
        get {
            if(instance == null) {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }
            return instance;
        }       
    }
	
    public virtual void Awake() {
        if (instance == null) {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
