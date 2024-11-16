using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearOnTap : MonoBehaviour
{
    // タップしたら
    public void onClick()
    {
        // オープニング画面へ
        SceneManager.LoadScene("OpeningScene");
    }
}
