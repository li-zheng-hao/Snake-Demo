using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateChange : MonoBehaviour {

	public void RestartGame()
    {
        GameController.Instance.RestartGame();
    }
    public void BackToStart()
    {
        GameController.Instance.BackToStartScene();
    }
}
