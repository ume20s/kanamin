using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class taptoStart : MonoBehaviour
{
    // ゲームオブジェクト
    GameObject openingGraphic;
    GameObject openingGraphic2;
    
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip bgmOpening;
    public AudioClip seStart;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトの取得
        openingGraphic = GameObject.Find("openingGraphic");
        openingGraphic2 = GameObject.Find("openingGraphic2");

        // 初期画面
        openingGraphic2.SetActive(false);

        // BGM開始
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bgmOpening);
    }

    // タップしたら
    public void onClick()
    {
        // 画面遷移
        openingGraphic2.SetActive(true);

        // BGM止めてきらりん効果音
        audioSource.Stop();
        audioSource.PlayOneShot(seStart);

        // １秒待ってクイズ画面へ
        StartCoroutine(waitOneSecAndStart());
    }

    // １秒待ってクイズ画面へ
    IEnumerator waitOneSecAndStart()
    {
        // １．５秒待つ
        yield return new WaitForSeconds(1.5f);

        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }

}
