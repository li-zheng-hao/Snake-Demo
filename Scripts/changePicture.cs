using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changePicture : MonoBehaviour {
    public RawImage bg1, bg2, bg3;
    private static int currentIndex=1;
	public void BackPicture()
    {
       
        --currentIndex;
        if (currentIndex < 1)
            currentIndex = 1;
        switch (currentIndex)
        {
            case 1:
                bg1.gameObject.SetActive(true);
                bg2.gameObject.SetActive(false);
                bg3.gameObject.SetActive(false);
                break;
            case 2:
                bg1.gameObject.SetActive(false);
                bg2.gameObject.SetActive(true);
                bg3.gameObject.SetActive(false);
                break;
            case 3:
                bg1.gameObject.SetActive(false);
                bg2.gameObject.SetActive(false);
                bg3.gameObject.SetActive(true);
                break;
        }
    }
    public void NextPicture()
    {
       
        ++currentIndex;
        if (currentIndex >3)
            currentIndex = 3;
        switch (currentIndex)
        {
            case 1:
                bg1.gameObject.SetActive(true);
                bg2.gameObject.SetActive(false);
                bg3.gameObject.SetActive(false);
                break;
            case 2:
                bg1.gameObject.SetActive(false);
                bg2.gameObject.SetActive(true);
                bg3.gameObject.SetActive(false);
                break;
            case 3:
                bg1.gameObject.SetActive(false);
                bg2.gameObject.SetActive(false);
                bg3.gameObject.SetActive(true);
                break;
        }
    }
}
