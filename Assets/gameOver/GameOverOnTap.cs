using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOnTap : MonoBehaviour
{
    // タップしたら
    public void onClick()
    {
        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }
}
