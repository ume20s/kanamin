using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class taptoStart : MonoBehaviour
{
    // ゲームオブジェクト
    GameObject kanaminWaiting;
    GameObject kanaminStart;
    GameObject pushStartStay;
    GameObject startFlower;
    
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip bgmOpening;
    public AudioClip seStart;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトの取得
        kanaminWaiting = GameObject.Find("kanaminWaiting");
        kanaminStart = GameObject.Find("kanaminStart");
        pushStartStay = GameObject.Find("pushStartStay");
        startFlower = GameObject.Find("startFlower");

        // 初期画面
        kanaminStart.SetActive(false);
        pushStartStay.SetActive(false);
        startFlower.SetActive(false);

        // BGM開始
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bgmOpening);
    }

    // タップしたら
    public void onClick()
    {
        // 画面遷移
        kanaminWaiting.SetActive(false);
        kanaminStart.SetActive(true);
        pushStartStay.SetActive(true);
        startFlower.SetActive(true);
        
        // BGM止めてきらりん効果音
        audioSource.Stop();
        audioSource.PlayOneShot(seStart);

        // １秒待ってクイズ画面へ
        StartCoroutine(waitOneSecAndStart());
    }

    // １秒待ってクイズ画面へ
    IEnumerator waitOneSecAndStart()
    {
        // １．８秒待つ
        yield return new WaitForSeconds(1.8f);

        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }

}
