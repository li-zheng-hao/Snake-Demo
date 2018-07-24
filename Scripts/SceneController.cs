using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private static SceneController _instance;
    public static SceneController Instance {
        get {
            if(_instance==null)
                _instance = FindObjectOfType<SceneController>();
            if(_instance==null)
            {
                GameObject gm = new GameObject("SceneController");
                _instance = gm.AddComponent<SceneController>();
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        //如果实例化的对象不是当前的这个对象，则把当前这个对象附加在的gameobject销毁
        if (Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        //如果实例化的对象是当前这个对象，则在加载的时候不销毁
        DontDestroyOnLoad(gameObject);
    }
    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
