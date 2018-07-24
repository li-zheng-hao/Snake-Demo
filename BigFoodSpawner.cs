using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFoodSpawner : MonoBehaviour {
    public GameObject[] bigFoodPrefabs;

	public void CreateBigFood(List<Vector2> pos)
    {
        int index = Random.Range(0, bigFoodPrefabs.Length);
        for (int i=0;i<pos.Count;i++)
        {
            //GameObject gj=GameObject.Instantiate <bigFoodPrefabs> ();
            var gj=GameObject.Instantiate(bigFoodPrefabs[index]);
            gj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            gj.transform.position = pos[i];
            gj.transform.SetParent(transform);
        }
    }
}
