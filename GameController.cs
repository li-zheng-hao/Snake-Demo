using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public bool isGameOver = false;
    

    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameController>();
            if (_instance == null)
            {
                GameObject gm = new GameObject("GameController");
                _instance = gm.AddComponent<GameController>();
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        //如果实例化的对象不是当前的这个对象，则把当前这个对象附加在的gameobject销毁
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //如果实例化的对象是当前这个对象，则在加载的时候不销毁
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        GameObject gj = GameObject.Find("GameOverPanel");
        if (gj!=null)
        {
            Transform child0 = gj.transform.GetChild(0);
            //Transform child1 = gj.transform.GetChild(1);
            child0.gameObject.SetActive(true);
            //child1.gameObject.SetActive(true);

        }
    }
    public void RestartGame()
    {
        Debug.Log("重新开始游戏");
        isGameOver = false;
        Time.timeScale = 1;
        SceneController.Instance.changeScene("unlimitGame");
    }
    public void BackToStartScene()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneController.Instance.changeScene("startScene");
    } 
}
