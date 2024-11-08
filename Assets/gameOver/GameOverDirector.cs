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


    // Start is called before the first frame update
    void Start()
    {
        // げーmジョーバー
        StartCoroutine("GameOver");
    }

    // ステージアイキャッチの表示
    IEnumerator GameOver()
    {
        // ざんねん効果音
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(seGameOver);

        // ゲームオブジェクトの取得
        kanaminZannen = GameObject.Find("kanaminZannen");           // かなみん残念グラフィック

        // かなみんのトランスフォームコンポーネントの取得
        Transform tf = kanaminZannen.GetComponent<Transform>();

        // かなみんをジワジワちいさくする
        for(int i=20; i>10; i--) {
            tf.transform.localScale = new Vector3(1.8f * (float)(i) / 20.0f, 1.8f * (float)(i) / 20.0f, 1);
            yield return new WaitForSeconds(0.1f);
        }


        // かなみん小さくなる




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
