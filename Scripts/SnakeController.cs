using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour {
    [Tooltip("左侧虚拟摇杆")]
    public EasyJoystick easyJoystick;
    [Tooltip("蛇移动的速度")]
    public int speed;
    [Tooltip("蛇头")]   
    public Sprite[] snakeHeads;
    [Tooltip("蛇身体")]
    public Sprite[] snakeBodys;
    [Tooltip("初始身体的数量")]
    public int initBodyNum=4;
    [Tooltip("蛇身体对象")]
    public GameObject snakeBody;
    private Quaternion direction;
    //蛇头产生的一些坐标
    private List<Vector2> oldPositionList;
    //蛇身体移动的步数
    private int positionLength=5;
    //生成的蛇身体
    public List<GameObject> _bodys;
    //皮肤的编号
    private int skinNum;
    // Use this for initialization
    public int addLengthNeedFood = 10;
    public int addLengthNeedFoodReset = 10;
    [Tooltip("击杀的敌人")]
    public int killEnemyNum = 0;
    public bool isSpeedUp=false;
    public Text gameOverLengthText;
    public Text killEnemyText;
    void Start () {
        //Debug.Log("游戏开始了，蛇头的名字是" + "skin" + StaticData.Instance.usingSkinName + "head");
        
        InitHead();
        InitBody();
       
    }
	
	// Update is called once per frame
	void Update ()
    {

        GameObject gj=GameObject.Find("length_text");
        GameObject gj2= GameObject.Find("kill_text");
        Text text=gj.GetComponent<Text>();
        Text text2 = gj2.GetComponent<Text>();

        text.text = "长度:  " + @"<color=blue>"+_bodys.Count.ToString()+@"</color>";
        text2.text = "击杀:  " + @"<color=blue>" + killEnemyNum.ToString() + @"</color>";

    }
    private void FixedUpdate()
    {
        if (GameController.Instance.isGameOver)
        {
            return;
        }
        UpdateRotationAndMove();
        
    }
    /// <summary>
    /// 更新头部的旋转角度
    /// </summary>
    private void UpdateRotationAndMove()
    {
        Vector3 joystickAxis = easyJoystick.JoystickAxis;
        int tempRunTime = 1;
        if (isSpeedUp == true)
            tempRunTime = 2;
        for (int i=0;i<tempRunTime;i++)
        {
            oldPositionList.Insert(0, transform.position);
            if (joystickAxis == Vector3.zero)
            {
                Vector3 vec = direction * Vector3.up;
                transform.position += vec.normalized * speed * Time.deltaTime;

            }
            else
            {
                transform.position += joystickAxis.normalized * speed * Time.deltaTime;
                direction = Quaternion.FromToRotation(Vector2.up, joystickAxis);
                transform.rotation = direction;

            }

            FollowHead();
        }
      
    }

    /// <summary>
    /// 初始化头部
    /// </summary>
    private void InitHead()
    {
        skinNum = System.Int32.Parse(StaticData.Instance.usingSkinName);
        var sprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = snakeHeads[System.Int32.Parse(StaticData.Instance.usingSkinName) - 1];
        if (skinNum == 2 || skinNum == 4)
            transform.localScale = new Vector3(1f, 1f, 1);
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        //创建蛇身体的存储
        oldPositionList = new List<Vector2>();
        //一开始有5个蛇身体，每个身体的间隔为positionLength个单元
       
        for (int i = 0; i < 6* positionLength + 1; i++)
        {
            oldPositionList.Add(new Vector2(transform.position.x, transform.position.y - 0.07f * (i + 1) ));
        }

    }
    /// <summary>
    /// 初始化身体
    /// </summary>
    private void InitBody()
    {
        _bodys = new List<GameObject>();
        for (int i=0;i<initBodyNum;i++)
        {
            GameObject go = new GameObject("body");
            var comp = go.AddComponent<SpriteRenderer>();
            comp.sprite = snakeBodys[skinNum - 1];
            //因为父类对象缩小了0.5，所以这里要除以2
         
            
            go.transform.SetParent(snakeBody.transform);
            //go.transform.position = new Vector3(transform.position.x,
            //go.transform.position=new Vector3(transform.position.x,
            //-0.5f * (i + 1),
            //0);
            //    , transform.position.z);
            comp.sortingLayerName = "character";
            if (skinNum == 2 || skinNum == 4)
                go.transform.localScale = new Vector3(1f, 1f, 1f);
            else
                go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            var collider = go.AddComponent<CircleCollider2D>();
            go.tag = "Player";
            _bodys.Add(go);
           
         
        }
        //for (int i = 0; i < _bodys.Count; i++)
        //{
        //    Debug.Log(oldPositionList[(i + 1) * positionLength-1]);
        //}

    }
    /// <summary>
    /// 增加身体
    /// </summary>
    private void  AddBody()
    {
        GameObject go = new GameObject("body");
        var comp = go.AddComponent<SpriteRenderer>();
        comp.sprite = snakeBodys[skinNum - 1];
        //因为父类对象缩小了0.5，所以这里要除以2


        go.transform.SetParent(snakeBody.transform);
        comp.sortingLayerName = "character";
        if (skinNum == 2||skinNum==4)
            go.transform.localScale = new Vector3(1f, 1f, 1f);
        else
            go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        go.transform.position = transform.position;
        var collider = go.AddComponent<CircleCollider2D>();
        go.tag = "Player";
       // AddAIBody(go.transform);
        _bodys.Add(go);

    }
    /// <summary>
    /// 跟随头部
    /// </summary>
    private void FollowHead()
    {

        for (int i = 0; i < _bodys.Count; i++)
        {

            _bodys[i].transform.position = oldPositionList[(i + 1) *( positionLength)];

        }
       
       
        if (oldPositionList.Count>_bodys.Count*positionLength+40)
        {
            oldPositionList.RemoveAt(oldPositionList.Count - 1);
        }
            
            
        //}
        //if ((_bodys.Count+2)*positionLength> oldPositionList.Count)
           
        
    }
    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
       //撞到墙，游戏结束
        if (collision.tag=="Border")
        {
            
            gameOverLengthText.text = "长度   " + "<color=red>" + _bodys.Count.ToString() + "</color>";
            killEnemyText.text = "击杀   " + "<color=red>" + _bodys.Count.ToString() + "</color>";
            GameController.Instance.GameOver();
        }else if (collision.tag=="Food")
        {
            addLengthNeedFood--;
            FoodPoolManager.Instance.changeFoodState(collision.gameObject);
            if (addLengthNeedFood == 0)
            {
                AddBody();
                addLengthNeedFood = addLengthNeedFoodReset;
            }
        }else if (collision.tag == "Player")
        {
            if (_bodys.Contains(collision.gameObject))
                return;
            Debug.Log(gameOverLengthText.text);
            gameOverLengthText.text = "长度   " + "<color=red>" + _bodys.Count.ToString() + "</color>";
            killEnemyText.text = "击杀   " + "<color=red>" + _bodys.Count.ToString() + "</color>";
            var sc = collision.transform.parent.parent.GetChild(0).GetComponent<AISnakeController>();
            sc.KillEnemy();
            GameController.Instance.GameOver();
        }
        else if(collision.tag=="AIBorder")
        {
        }
        else if (collision.tag == "BigFood")
        {
            addLengthNeedFood -= 3;
            Destroy(collision.gameObject);
            if (addLengthNeedFood <= 0)
            {
                AddBody();
                addLengthNeedFood = addLengthNeedFoodReset;
            }
        }


    }
    /// <summary>
    /// 击杀了敌人
    /// </summary>
    public void KillEnemy()
    {
        this.killEnemyNum++;
    }
    public void SetSpeedUp(bool result)
    {
        isSpeedUp = result;
    }

    //public void AddAIBody(Transform parent)
    //{
    //    var go=new GameObject("AIBody");
    //    go.tag = "AIBody";
        
    //    go.transform.localScale = new Vector3(1, 1, 1);
    //    go.AddComponent<TriggerEnter>();
    //    var cc=go.AddComponent<CircleCollider2D>();
    //    cc.radius = 1;
    //    cc.isTrigger = true;
    //    go.transform.parent = parent;
    //    go.transform.localPosition = new Vector3(0f, 0f, 0f);
        
    //}
}
