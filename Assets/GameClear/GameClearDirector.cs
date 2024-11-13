using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearDirector : MonoBehaviour
{
    // 音声まわりのあれこれ
    AudioSource audioSource;
    public AudioClip seGameClear;               // ゲームクリア効果音
    public AudioClip vGameClear;                // ゲームクリア音声

    // ゲームオブジェクト
    GameObject kanaminHyoshojo;                 // 表彰状グラフィック
    GameObject tappingArea;                     // タップ領域

    // Start is called before the first frame update
    void Start()
    {
        // ゲームクリアエフェクト
        StartCoroutine("GameClearEffect");
    }

    // ゲームクリアエフェクト
    IEnumerator GameClearEffect() 
    {
        // ゲームオブジェクトの取得
        kanaminHyoshojo = GameObject.Find("hyoshojo");              // 表彰状グラフィック
        tappingArea = GameObject.Find("TappingArea");               // タップ領域

        // ゲームオブジェクトのトランスフォームコンポーネントの取得
        Transform hyoshojoTf = kanaminHyoshojo.GetComponent<Transform>();

        // タップ無効
        tappingArea.SetActive(false);

        // ゲームクリア効果音
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(seGameClear);

        yield return new WaitForSeconds(0.01f);

    }

}
