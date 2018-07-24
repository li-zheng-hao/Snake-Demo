using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeController : MonoBehaviour {
   
    [Tooltip("蛇移动的速度")]
    public int speed;
    [Tooltip("蛇头")]
    public Sprite[] snakeHeads;
    [Tooltip("蛇身体")]
    public Sprite[] snakeBodys;
    [Tooltip("初始身体的数量")]
    public int initBodyNum = 4;
    [Tooltip("蛇身体对象")]
    public GameObject snakeBody;
    public Quaternion direction;
    //蛇头产生的一些坐标
    private List<Vector2> oldPositionList;
    //蛇身体移动的步数
    private int positionLength = 5;
    //生成的蛇身体
    public List<GameObject> _bodys;
    //皮肤的编号
    public int skinNum;
    // Use this for initialization
    public int addLengthNeedFood = 10;
    public int addLengthNeedFoodReset = 10;
    [Tooltip("击杀的敌人")]
    public int killEnemyNum = 0;
    void Start()
    {
        //Debug.Log("游戏开始了，蛇头的名字是" + "skin" + StaticData.Instance.usingSkinName + "head");
        direction = Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f)));
        
        InitHead();
        InitBody();
        AddAIBody(transform);


    }

    // Update is called once per frame
    void Update()
    {

      

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

        oldPositionList.Insert(0, transform.position);
        Vector3 vec =direction * Vector3.up;
        transform.position += vec* speed * Time.deltaTime;
        transform.rotation = direction;
        FollowHead();

    }

    /// <summary>
    /// 初始化头部
    /// </summary>
    private void InitHead()
    {

        skinNum = Random.Range(1,5);
        var sprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = snakeHeads[skinNum - 1];
        if (skinNum == 2 || skinNum == 4)
            transform.localScale = new Vector3(1f, 1f, 1);
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        transform.localPosition = new Vector3(0, 0, 0);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        var cc=gameObject.AddComponent<CircleCollider2D>();
        var rb = gameObject.AddComponent<Rigidbody2D>();
        cc.radius = 0.5f;
        cc.isTrigger = true;
        rb.gravityScale = 0;
        //创建蛇身体的存储
        oldPositionList = new List<Vector2>();
        //一开始有5个蛇身体，每个身体的间隔为positionLength个单元
        for (int i = 0; i < 6 * positionLength + 1; i++)
        {
            oldPositionList.Add(new Vector2(transform.position.x, transform.position.y - 0.07f * (i + 1)));
        }

    }
    /// <summary>
    /// 初始化身体
    /// </summary>
    private void InitBody()
    {
        _bodys = new List<GameObject>();
        for (int i = 0; i < initBodyNum; i++)
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
            go.transform.position = transform.position;
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
    private void AddBody()
    {
        GameObject go = new GameObject("body");
        var comp = go.AddComponent<SpriteRenderer>();
        comp.sprite = snakeBodys[skinNum - 1];
        //因为父类对象缩小了0.5，所以这里要除以2


        go.transform.SetParent(snakeBody.transform);
        comp.sortingLayerName = "character";
        if (skinNum == 2 || skinNum == 4)
            go.transform.localScale = new Vector3(1f, 1f, 1f);
        else
            go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        go.transform.position = transform.position;
        var collider = go.AddComponent<CircleCollider2D>();
        go.tag = "Player";
        _bodys.Add(go);

    }
    /// <summary>
    /// 跟随头部
    /// </summary>
    private void FollowHead()
    {

        for (int i = 0; i < _bodys.Count; i++)
        {

            _bodys[i].transform.position = oldPositionList[(i + 1) * (positionLength)];

        }


        if (oldPositionList.Count > _bodys.Count * positionLength + 40)
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
        if (collision.tag == "Food")
        {
            FoodPoolManager.Instance.changeFoodState(collision.gameObject);
            addLengthNeedFood--;
            if (addLengthNeedFood <= 0)
            {
                AddBody();
                addLengthNeedFood = addLengthNeedFoodReset;
            }
        }
        else if (collision.tag == "Border")
        {
            Dead();
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
        //}else if (collision .tag=="AIBody")
        //{
        //    //if (_bodys.Contains(collision.gameObject))
        //    //    return;
        //    //var vec3 = direction.eulerAngles;
        //    //vec3.z = Random.Range(vec3.z + 180 - 10, vec3.z+ 180 + 10);
        //    //direction = Quaternion.Euler(vec3);
        //}


        else if (collision.tag == "Player")
        {
            if (_bodys.Contains(collision.gameObject))
                return;
            if (collision.transform.parent.tag == "AI")
            {
                AISnakeController sc = collision.transform.parent.parent.gameObject.GetComponent<AISnakeController>();
                sc.KillEnemy();
            }
            else
            {
                SnakeController sc2;
                if (collision.transform.parent.name == "SnakeBody")
                    sc2 = collision.transform.parent.gameObject.GetComponent<SnakeController>();
                else
                    sc2 = collision.gameObject.GetComponent<SnakeController>();
                sc2.KillEnemy();
            }
            Dead();

        }
    }
    /// <summary>
    /// 击杀了敌人
    /// </summary>
    public void KillEnemy()
    {
        this.killEnemyNum++;
    }
    public void ResetState()
    {
        foreach (var i in _bodys)
        {
            Destroy(i);
        }
        oldPositionList.Clear();
        _bodys.Clear();
        //蛇身体移动的步数
        positionLength = 5;
        //生成的蛇身体
        //皮肤的编号
        skinNum = Random.Range(1, 5);
        // Use this for initialization
        addLengthNeedFood = 10;
        addLengthNeedFoodReset = 10;
        killEnemyNum = 0;
        var sprite = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = snakeHeads[skinNum - 1];
        if (skinNum == 2 || skinNum == 4)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        this.transform.localPosition = new Vector3(0, 0, 0);
        direction = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        //一开始有5个蛇身体，每个身体的间隔为positionLength个单元
        for (int i = 0; i < 6 * positionLength + 1; i++)
        {
            oldPositionList.Add(new Vector2(transform.position.x, transform.position.y - 0.07f * (i + 1)));
        }
        InitBody();
    }
    public void AddAIBody(Transform parent)
    {
        var go = new GameObject("AIBody");
        go.tag = "AIBody";
        go.transform.parent = parent;
        if (skinNum == 2 || skinNum == 4)
            go.transform.localScale = new Vector3(1f, 1f, 1f);
        else
            go.transform.localScale = new Vector3(2f, 2f, 1f);
        //go.transform.localScale = new Vector3(2f, 2f, 1);
        go.transform.localPosition = new Vector3(0f, 0f, 0f);

        var rb=go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        var cc = go.AddComponent<CircleCollider2D>();
        cc.radius = 1f;
        cc.isTrigger = true;

        go.AddComponent<TriggerEnter>();
        
        
    }
    public void Dead()
    {
        List<Vector2> pos = new List<Vector2>();
        foreach (var i in _bodys)
        {
            pos.Add(i.transform.position);
        }
        var gj = GameObject.Find("BigFoodSpawner");
        gj.GetComponent<BigFoodSpawner>().CreateBigFood(pos);
        ResetState();
    }
}
