using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{

    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip vGameOver;

    // Start is called before the first frame update
    void Start()
    {
        // ざんねん音声
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(vGameOver);
    }

    // タップしたら
    public void onClick()
    {
        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }
}
