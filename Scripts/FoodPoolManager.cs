using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoolManager : MonoBehaviour {
    private static FoodPoolManager _instance;
    public static FoodPoolManager Instance {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<FoodPoolManager>() as FoodPoolManager;
            if(_instance==null)
            {
                GameObject gj = new GameObject("FoodPoolManager");
                _instance = gj.AddComponent<FoodPoolManager>();
            }
            return _instance;
        }
    }

    [Tooltip("食物创建的位置限制")]
    public Vector2 leftButtomPosition, rightUpPosition;
    [Tooltip("食物的prefab")]
    public GameObject[] foods;

   

    [Tooltip("屏幕食物的数量")]
    public int foodCount;
    //[Tooltip("使用中的食物")]
    public List<GameObject> workFood;
    //[SerializeField][Tooltip("未使用的食物")]
    public List<GameObject> idleFood;
  
    //使用中的食物挂载的父类
    private GameObject workObj;
    //空闲的食物挂载的父类
    private GameObject idleObj;

    public void Awake()
    {
        InitListAndTwoObj();
        InitFood();
    }
    /// <summary>
    /// 创建食物
    /// </summary>
    /// <returns>返回食物</returns>
    public GameObject CreateFood()
    {
        GameObject food = GameObject.Instantiate(foods[Random.Range(0, foods.Length)]);
        Vector2 position = new Vector2(Random.Range(leftButtomPosition.x, rightUpPosition.x), Random.Range(leftButtomPosition.y, rightUpPosition.y));
        food.transform.position = position;
        food.layer = 3;
        return food;
    }
    

    /// <summary>
    /// 初始化两个链表以及挂载所需的两个gameobject
    /// </summary>
    public void InitListAndTwoObj()
    {
        workFood = new List<GameObject>();
        idleFood = new List<GameObject>();
        workObj = new GameObject("work");
        idleObj = new GameObject("idle");
        workObj.transform.SetParent(this.transform);
        idleObj.transform.SetParent(this.transform);
        idleObj.SetActive(false);
    }
    /// <summary>
    /// 根据foodCount创建食物
    /// </summary>
    public void InitFood()
    {
        
        for (int i=0;i<foodCount;i++)
        {
            GameObject go=CreateFood();
            workFood.Add(go);
            //go.transform.SetParent(workObj.transform);
            go.transform.SetParent(workObj.transform);
        }
    }
    public void changeFoodState(GameObject food)
    {
        food.transform.position = new Vector2(Random.Range(leftButtomPosition.x, rightUpPosition.x), Random.Range(leftButtomPosition.y, rightUpPosition.y));
    }

}
