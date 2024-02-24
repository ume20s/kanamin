using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameDirector : MonoBehaviour
{
    // 音声関連
    AudioSource audioSource;
    public AudioClip gameBGM;
    public AudioClip vStageStart;

    // ゲームオブジェクト
    GameObject questionText;                                // 問題文
    GameObject[] choiceText = new GameObject[4];            // 選択肢
    GameObject stageEyeCatchFrame;                          // ステージアイキャッチ背景
    GameObject stageNumberText;                             // ステージ番号テキスト
    GameObject kanaminThinking;                             // かなみん考え中グラフィック
    GameObject additionalInfoFrame;                         // 追加情報背景
    GameObject additionalInfoText;                          // 追加情報テキスト

    // 各種変数
    int stage;              // ステージ
    int quesCounter;        // 現在何問目？
    int gameState;          // 状態遷移変数

    // 各状態実行中フラグ
    bool dispStageFlg;      // ステージアイキャッチ表示

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトの取得
        questionText = GameObject.Find("questionText");             // 問題文
        choiceText[0] = GameObject.Find("choiceText0");             // 選択肢
        choiceText[1] = GameObject.Find("choiceText1");
        choiceText[2] = GameObject.Find("choiceText2");
        choiceText[3] = GameObject.Find("choiceText3");
        stageEyeCatchFrame = GameObject.Find("stageEyeCatchFrame"); // ステージアイキャッチ背景
        stageNumberText = GameObject.Find("stageNumberText");       // ステージ番号テキスト
        additionalInfoFrame = GameObject.Find("additionalInfoFrame"); // 追加情報背景
        additionalInfoText = GameObject.Find("additionalInfoText"); // 追加情報テキスト
        kanaminThinking = GameObject.Find("kanaminThinking");       // かなみん考え中グラフィック

        // 音声コンポーネントの取得
        audioSource = GetComponent<AudioSource>();

        // 各種変数の初期化
        stage = 1;
        quesCounter = 0;
        gameState = 0;

        // 各状態実行中フラグは全部false
        dispStageFlg = false;

        // クイズ出題順のシャッフル
        int n = 100;
        while(n > 1) {
            n--;
            int k = Random.Range(0, 100);
            int temp = Ques.Order[k];
            Ques.Order[k] = Ques.Order[n];
            Ques.Order[n] = temp;
        }

        // 最初に余計なものを消しておく
        stageEyeCatchFrame.SetActive(false);
        stageNumberText.SetActive(false);
        additionalInfoFrame.SetActive(false);
        kanaminThinking.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState) {
            // ステージアイキャッチの表示
            case 0:
                StartCoroutine("DispStage");
                break;
            
            // クイズ出題
            case 1:
                AskQues();
                break;

            default:
                // NOTREACHED
                break;
        }
    }

    // ステージアイキャッチの表示
    IEnumerator DispStage()
    {
        if(!dispStageFlg) {
            // ステージアイキャッチ中
            dispStageFlg = true;
            
            // アイキャッチ背景のトランスフォームコンポーネントの取得
            Transform tf = stageEyeCatchFrame.GetComponent<Transform>();

            // ステージ文字列をセット
            stageNumberText.GetComponent<Text>().text = "STAGE: " + stage.ToString();

            // アイキャッチ背景の高さをゼロにしてから表示
            tf.transform.localScale = new Vector3(1, 0, 1);
            stageEyeCatchFrame.SetActive(true);

            // アイキャッチ背景をジワジワ大きくする
            for(int i=0; i<10; i++) {
                tf.transform.localScale = new Vector3(1, (float)(i) / 10.0f, 1);
                yield return new WaitForSeconds(0.02f);
            }

            // ステージ番号テキストを表示して2.5秒待つ
            stageNumberText.SetActive(true);
            audioSource.PlayOneShot(vStageStart);
            yield return new WaitForSeconds(2.5f);

            // ステージアイキャッチを非表示
            stageEyeCatchFrame.SetActive(false);
            stageNumberText.SetActive(false);

            // かなみん表示
            kanaminThinking.SetActive(true);

            // ＢＧＭ開始
            audioSource.Play();

            // クイズ出題フェーズへ遷移
            gameState++;

            // ステージアイキャッチ終了
            dispStageFlg = false;
        }
    }

    // クイズ出題
    private void AskQues()
    {
        questionText.GetComponent<Text>().text = Ques.Q[Ques.Order[quesCounter]];

    }


}
