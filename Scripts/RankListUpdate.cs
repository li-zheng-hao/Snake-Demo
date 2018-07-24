using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListUpdate : MonoBehaviour {
    [Tooltip("排行榜的标签")]
    public GameObject[] rankList;
    [Tooltip("所有的蛇")]
    public GameObject[] snakes;
	
	// Update is called once per frame
	void Update () {
      
        
    }
    private void LateUpdate()
    {
        // Debug.Log(snakes[4].transform.GetChild(0).GetComponent<SnakeController>()._bodys.Count);
        Sort();
        UpdateList();
    }
    private void Sort()
    {
        for (int i=0;i<snakes.Length;i++)
        {
            for (int j=i+1;j<snakes.Length;j++)
            {
                //if (snakes[j].name == "Snake")
                //{
                //    Debug.LogError("没错啊，输出了4啊");
                //}
                //Debug.LogError(i + ":::" + j);
                int lengthOne = (snakes[i].name=="Snake"? snakes[i].transform.GetChild(0).GetComponent<SnakeController>()._bodys.Count : snakes[i].transform.GetChild(0).GetComponent<AISnakeController>()._bodys.Count);
                int lengthTwo=(snakes[j].name == "Snake" ? snakes[j].transform.GetChild(0).GetComponent<SnakeController>()._bodys.Count : snakes[j].transform.GetChild(0).GetComponent<AISnakeController>()._bodys.Count);
                if (lengthOne<lengthTwo)
                {
                    var temp = snakes[i];
                    snakes[i] = snakes[j];
                    snakes[j] = temp;
                }
            }
        }
    }
    private void UpdateList()
    {
        for (int i=0;i<rankList.Length;i++)
        {
            int length= (snakes[i].name == "Snake" ? snakes[i].transform.GetChild(0).GetComponent<SnakeController>()._bodys.Count : snakes[i].transform.GetChild(0).GetComponent<AISnakeController>()._bodys.Count);
            var name=rankList[i].transform.GetChild(0);
            var score= rankList[i].transform.GetChild(1);
            name.GetComponent<Text>().text = snakes[i].name;
            score.GetComponent<Text>().text = length.ToString();
        }
    }
   

}
