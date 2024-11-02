using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setsumei1Director : MonoBehaviour
{
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip bgmSetsumei;

    // Start is called before the first frame update
    void Start()
    {
        // BGM開始
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bgmSetsumei);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // タップしたら
    public void onClick()
    {
        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }
}
