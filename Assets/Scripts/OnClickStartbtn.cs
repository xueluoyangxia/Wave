using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickStartbtn : MonoBehaviour
{
    public void OnClickStartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        Debug.Log("场景加载成功");
    }
}
