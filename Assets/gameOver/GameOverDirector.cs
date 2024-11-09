using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip seGameOver;                // ざんねん効果音
    public AudioClip vGameOver;                 // ざんねん音声

    // ゲームオブジェクト
    GameObject kanaminZannen;                               // かなみん残念グラフィック
    GameObject gameoverTitle;                               // ゲームオーバー文字
    GameObject oncemoreTitle;                               // もういっかい文字
    GameObject donnyori;                                    // どんより


    // Start is called before the first frame update
    void Start()
    {
        // げーmジョーバー
        StartCoroutine("GameOver");
    }

    // ゲームオーバーエフェクト
    IEnumerator GameOver()
    {
        // ゲームオブジェクトの取得
        kanaminZannen = GameObject.Find("kanaminZannen");           // かなみん残念グラフィック
        gameoverTitle = GameObject.Find("GameOverTitle");           // ゲームオーバー文字
        oncemoreTitle = GameObject.Find("OnceMoreTitle");           // もういっかい文字
        donnyori = GameObject.Find("Donnyori");                     // どんより

        // ゲームオブジェクトのトランスフォームコンポーネントの取得
        Transform kanaminTf = kanaminZannen.GetComponent<Transform>();
        Transform gameoverTf = gameoverTitle.GetComponent<Transform>();
        Transform oncemoreTf = oncemoreTitle.GetComponent<Transform>();
        Transform donnyoriTf = donnyori.GetComponent<Transform>();

        // もういっかいは消しておく
        oncemoreTitle.SetActive(false);

        // ざんねん効果音
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(seGameOver);

        for(int i=0; i<100; i++) {
            // じわじわどんよりする
            donnyoriTf.transform.Translate(0, -0.1f, 0);

            // ゲームオーバーが小さくなりつつ降ってくる
            gameoverTf.transform.localScale = new Vector3(3.0f - (float)(i) / 48.0f, 3.0f - (float)(i) / 48.0f, 1);
            gameoverTf.transform.Translate(0, -0.135f, 0);


            // かなみんが小さくなりつつ中央へ移動する
            kanaminTf.transform.localScale = new Vector3(1.8f -(float)(i) / 70.0f, 1.8f - (float)(i) / 70.0f, 1);
            kanaminTf.transform.Translate(-0.06f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        // もういっかい表示
        oncemoreTitle.SetActive(true);

        // ざんねん音声
        audioSource.PlayOneShot(vGameOver);
    }

    // タップしたら
    public void onClick()
    {
        // クイズ画面へ
        SceneManager.LoadScene("QuizScene");
    }
}
