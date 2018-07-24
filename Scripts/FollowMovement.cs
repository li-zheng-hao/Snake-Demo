using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]

public class FollowMovement : MonoBehaviour {

    public Transform TargetTF;          //要跟随的目标
    public float RecordGap = 0.2f;      //目标移动多远记录一次距离
    public float StopCount = 2f;        //记录还剩多少时停止移动
    public float WalkSpeed = 10f;       //走速度
    public float RunSpeed = 20f;        //跑速度
    public float SpeedLerpRant = 0.1f;  //速度变化的缓动率
    public float StartRunCount = 5f;    //距离目标多远后开始跑（单位是List的Item数）
    public bool Running {
        get { return PosList.Count > StartRunCount; }
        set { }
    }

    private Rigidbody2D Rig;
    private List<Vector2> PosList = new List<Vector2>();

	void Awake () {

        Rig = GetComponent<Rigidbody2D>();
        Rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rig.gravityScale = 0f;

	}


    void OnDisabled() {
        //失去意识时清空记录
        PosList.Clear();
    }


    void FixedUpdate() {
        
        if (TargetTF) {
            //清除已经到达的点
            while (PosList.Count > 0 && Vector2.Distance(transform.position, PosList[0]) < RecordGap) {
                PosList.RemoveAt(0);
            }
            if (PosList.Count > 0) {
                //添加当前Target位置
                if (Vector2.Distance(TargetTF.position, PosList[PosList.Count - 1]) > RecordGap) {
                    PosList.Add(TargetTF.position);
                }
                //处理移动
                if (PosList.Count > StopCount) {
                    Rig.velocity = Vector2.Lerp(Rig.velocity, new Vector2(
                        PosList[0].x - transform.position.x,
                        PosList[0].y - transform.position.y
                    ).normalized * (Running ? RunSpeed : WalkSpeed), SpeedLerpRant);
                } else {
                    Rig.velocity = Vector2.Lerp(Rig.velocity, Vector2.zero, SpeedLerpRant);
                }
            } else {
                PosList.Add(TargetTF.position);
            }
        }

    }




}
