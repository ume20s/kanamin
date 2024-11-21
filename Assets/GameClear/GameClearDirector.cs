using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearDirector : MonoBehaviour
{
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip seGameClear1;              // ゲームクリア効果音きらきら
    public AudioClip seGameClear2;              // ゲームクリア効果音ファンファーレ
    public AudioClip vGameClear;                // ゲームクリア音声

    // ゲームオブジェクト
    GameObject[] hyoshojoText = new GameObject[6];  // 表彰状テキスト
    GameObject kanaminHyoshojo;                     // 表彰状グラフィック
    GameObject hyoshojoBackGround;                  // 表彰状背景
    GameObject tappingArea;                         // タップ領域

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトの取得
        hyoshojoText[0] = GameObject.Find("hyoshoText0");               // 表彰状テキスト
        hyoshojoText[1] = GameObject.Find("hyoshoText1");
        hyoshojoText[2] = GameObject.Find("hyoshoText2");
        hyoshojoText[3] = GameObject.Find("hyoshoText3");
        hyoshojoText[4] = GameObject.Find("hyoshoText4");
        hyoshojoText[5] = GameObject.Find("hyoshoText5");
        kanaminHyoshojo = GameObject.Find("hyoshojo");                  // 表彰状グラフィック
        hyoshojoBackGround = GameObject.Find("GameClearBackground2");   // 表彰状背景
        tappingArea = GameObject.Find("TappingArea");                   // タップ領域

        // 余計なものは非表示にしておく
        for(int i=0; i<6; i++) {
            hyoshojoText[i].SetActive(false);
        }
        hyoshojoBackGround.SetActive(false);
        kanaminHyoshojo.SetActive(false);

        // ゲームクリアエフェクト
        StartCoroutine("GameClearEffect");
    }

    // ゲームクリアエフェクト
    IEnumerator GameClearEffect() 
    {
        // タップ無効
        tappingArea.SetActive(false);

        // ゲームクリア効果音きらきら
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(seGameClear1);

        // 表彰状テキスト表示
        for(int i=0; i<6; i++) {
            hyoshojoText[i].GetComponent<Text>().color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
            hyoshojoText[i].SetActive(true);
            for(int j=0; j<10; j++) {
                hyoshojoText[i].GetComponent<Text>().color = new Color (0.0f, 0.0f, 0.0f, (float)j/10.0f);
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(2.0f);

        // 背景チェンジ
        hyoshojoBackGround.SetActive(true);
        kanaminHyoshojo.SetActive(true);

        // ゲームクリア効果音ファンファーレ
        audioSource.PlayOneShot(seGameClear2);

        // 表彰状ズームイン
        for(int i=150; i>=15; i-=5) {
            kanaminHyoshojo.GetComponent<Transform>().transform.localScale = new Vector3((float)(i)/10.0f, (float)(i)/10.0f, 1);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.2f);

        // ゲームクリア音声
        audioSource.PlayOneShot(vGameClear);

        // タップ有効
        tappingArea.SetActive(true);
    }
}
