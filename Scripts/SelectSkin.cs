using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkin : MonoBehaviour {

     public Texture selectedTexture;
    /// <summary>
    /// 点击选择了一个皮肤
    /// </summary>
    /// <param name="obj"></param>
	 public void clickSkin(GameObject obj)
    {
        GameObject g = GameObject.Find("selected");
        Destroy(g);
        GameObject child = new GameObject("selected");
        child.AddComponent<RawImage>().texture = selectedTexture;
        child.transform.SetParent(obj.transform);
        RectTransform rect=child.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(-50, -45);
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        
        rect.localScale = new Vector3(1, 1, 1);
        rect.localRotation = new Quaternion(0, 0, 0, 0);

        StaticData.Instance.usingSkinName = obj.name;
        Debug.Log("当前选择的皮肤名字" + obj.name);

     
    }
}
