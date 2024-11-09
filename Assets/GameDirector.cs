using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    // 音声関連
    AudioSource audioSource;
    public AudioClip gameBGM;
    public AudioClip[] vStageStart = new AudioClip[2];
    public AudioClip[] vSeikai = new AudioClip[2];
    public AudioClip vMachigai;
    public AudioClip[] vStageClear = new AudioClip[2];
    public AudioClip vGameOver;

    // ゲームオブジェクト
    GameObject stageText;                                   // ステージ表示
    GameObject seikaiGuageText;                             // 正解数ゲージ表示
    GameObject scoreText;                                   // スコア表示
    GameObject highscoreText;                               // ハイスコア表示
    GameObject questionText;                                // 問題文
    GameObject[] choiceText = new GameObject[4];            // 選択肢
    GameObject[] timeFlame = new GameObject[4];             // 時間経過棒
    GameObject stageEyeCatchFrame;                          // ステージアイキャッチ背景
    GameObject stageNumberText;                             // ステージ番号テキスト
    GameObject kanaminThinking;                             // かなみん考え中グラフィック
    GameObject kanaminSeikai;                               // かなみん正解グラフィック
    GameObject kanaminZannen;                               // かなみん残念グラフィック
    GameObject hanamaru;                                    // はなまるグラフィック
    GameObject[] otetsuki = new GameObject[3];              // おてつきグラフィック
    GameObject additionalInfoFrame;                         // 追加情報背景
    GameObject additionalInfoText;                          // 追加情報テキスト

    // 時間経過棒のトランスフォームコンポーネント
    Transform[] timeTf = new Transform[4];

    // 各種変数
    public static int gameState;    // 状態遷移変数
    public static int tapNum;       // タップしたボタン番号
    int stage;                      // ステージ
    int quesCounter;                // 現在何問目？
    int seikaiNum;                  // 正解数
    int comboNum;                   // 連続正解数
    int otetsukiNum;                // おてつき回数
    float keikaTime;                // 経過時間
    int correctAnswer;              // 正解選択肢番号

    // 各状態実行中フラグ
    bool dispStageFlg;              // ステージアイキャッチ表示
    bool askQuesFlg;                // クイズ出題
    bool waitingTapFlg;             // タップ待ち
    bool judgementFlg;              // 正誤判定

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトの取得
        stageText = GameObject.Find("StageText");                   // ステージ表示
        seikaiGuageText = GameObject.Find("SeikaiGuage");           // 正解ゲージ表示
        scoreText = GameObject.Find("ScoreText");                   // スコア表示
        highscoreText = GameObject.Find("HighScoreText");           // ハイスコア表示
        questionText = GameObject.Find("questionText");             // 問題文
        choiceText[0] = GameObject.Find("choiceText0");             // 選択肢
        choiceText[1] = GameObject.Find("choiceText1");
        choiceText[2] = GameObject.Find("choiceText2");
        choiceText[3] = GameObject.Find("choiceText3");
        timeFlame[0] = GameObject.Find("timeFlame1");               // 時間経過棒
        timeFlame[1] = GameObject.Find("timeFlame2");
        timeFlame[2] = GameObject.Find("timeFlame3");
        timeFlame[3] = GameObject.Find("timeFlame4");
        stageEyeCatchFrame = GameObject.Find("stageEyeCatchFrame"); // ステージアイキャッチ背景
        stageNumberText = GameObject.Find("stageNumberText");       // ステージ番号テキスト
        additionalInfoFrame = GameObject.Find("additionalInfoFrame"); // 追加情報背景
        additionalInfoText = GameObject.Find("additionalInfoText"); // 追加情報テキスト
        kanaminThinking = GameObject.Find("kanaminThinking");       // かなみん考え中グラフィック
        kanaminSeikai = GameObject.Find("kanaminSeikai");           // かなみん正解グラフィック
        kanaminZannen = GameObject.Find("kanaminZannen");           // かなみん残念グラフィック
        hanamaru = GameObject.Find("hanamaru");                     // はなまるグラフィック
        otetsuki[0] = GameObject.Find("otetsuki1");                 // おてつきグラフィック
        otetsuki[1] = GameObject.Find("otetsuki2");
        otetsuki[2] = GameObject.Find("otetsuki3");

        // 音声コンポーネントの取得
        audioSource = GetComponent<AudioSource>();

        // 時間経過棒のトランスフォームコンポーネントの取得
        for(int i=0; i<4; i++) {
            timeTf[i] = timeFlame[i].GetComponent<Transform>();
        }

        // 各種変数の初期化
        stage = 1;
        quesCounter = 0;
        gameState = 0;
        Dt.quesTail = 100;

        // スコア処理
        Dt.score = 0;
        scoreText.GetComponent<Text>().text = "SCORE: " + Dt.score.ToString().PadLeft(4,'0');

        // ハイスコア読み込み
        Dt.highscore = PlayerPrefs.GetInt(Dt.SAVE_KEY, 0);
        highscoreText.GetComponent<Text>().text = "HIGHSCORE: " + Dt.highscore.ToString().PadLeft(4,'0');

        // 各状態実行中フラグは全部false
        dispStageFlg = false;
        askQuesFlg = false;
        dispStageFlg = false;
        judgementFlg = false;

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
        kanaminSeikai.SetActive(false);
        kanaminZannen.SetActive(false);
        hanamaru.SetActive(false);
        for(int i=0; i<3; i++) {
            otetsuki[i].SetActive(false);
        }
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
                StartCoroutine("AskQues");
                break;
            
            // タップ待ち
            case 2:
                StartCoroutine("WaitingTap");
                break;
            
            // 正誤判定
            case 3:
                StartCoroutine("judgement");
                break;
            
            // ステージクリア
            case 4:
                StartCoroutine("StageClear");
                break;
            
            // ゲームオーバー
            case 5:
                SceneManager.LoadScene("GameOverScene");
                break;
            
            // 空ルーチン
            case 10:
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

            // 経過時間リセット
            keikaTime = 0.0f;

            // 経過時間棒リセット
            timeTf[0].transform.localScale = new Vector3(0, 0.7f, 1);
            timeTf[1].transform.localScale = new Vector3(0.7f, 0, 1);
            timeTf[2].transform.localScale = new Vector3(0, 0.7f, 1);
            timeTf[3].transform.localScale = new Vector3(0.7f, 0, 1);

            // 正解数と連続正解数リセット
            seikaiNum = 0;
            comboNum = 0;
            seikaiGuageText.GetComponent<Text>().text = Dt.seikaiGuage[seikaiNum];
            otetsukiNum = 0;

            // アイキャッチ背景のトランスフォームコンポーネントの取得
            Transform tf = stageEyeCatchFrame.GetComponent<Transform>();

            // ステージ文字列をセット
            stageText.GetComponent<Text>().text = "STAGE: " + stage.ToString();
            stageNumberText.GetComponent<Text>().text = "STAGE " + stage.ToString();

            // アイキャッチ背景の高さをゼロにしてから表示
            tf.transform.localScale = new Vector3(1, 0, 1);
            stageEyeCatchFrame.SetActive(true);

            // アイキャッチ背景をジワジワ大きくする
            for(int i=0; i<10; i++) {
                tf.transform.localScale = new Vector3(1, (float)(i) / 10.0f, 1);
                yield return new WaitForSeconds(0.02f);
            }

            // ステージ番号テキストを表示
            stageNumberText.SetActive(true);

            // ステージスタート音声のあと2.5秒待つ
            audioSource.PlayOneShot(vStageStart[Random.Range(0, 2)]);
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
    IEnumerator AskQues()
    {
        if(!askQuesFlg) {
            // DEBUG
            // Ques.Order[quesCounter] = 91;

            // クイズ出題中
            askQuesFlg = true;

            // 問題文の表示
            questionText.GetComponent<Text>().text = Ques.Q[Ques.Order[quesCounter]];

            // 選択肢順の選択
            int choiNum = Random.Range(0, 24);

            // 選択肢の表示
            choiceText[0].GetComponent<Text>().text = Ques.A[Ques.Order[quesCounter], Ques.Choices[choiNum,0]];
            choiceText[1].GetComponent<Text>().text = Ques.A[Ques.Order[quesCounter], Ques.Choices[choiNum,1]];
            choiceText[2].GetComponent<Text>().text = Ques.A[Ques.Order[quesCounter], Ques.Choices[choiNum,2]];
            choiceText[3].GetComponent<Text>().text = Ques.A[Ques.Order[quesCounter], Ques.Choices[choiNum,3]];

            // 正解選択肢番号の保持
            correctAnswer = choiNum % 4;

            // 解答待ちフェーズへ遷移
            gameState++;

            // クイズ出題終了
            yield return new WaitForSeconds(0.1f);

            askQuesFlg = false;
        }
    }

    // タップ待ち
    IEnumerator WaitingTap()
    {
        // 経過時間追加
        keikaTime += Dt.Inc[stage];

        if(keikaTime > 171.2f) {
            // タイムオーバーならゲームオーバー
            gameState = 5;  
        } else {
            // 経過時間によって増加する棒を変える
            if(keikaTime < 61.5f) {         // 上棒
                timeTf[0].transform.localScale = new Vector3(keikaTime, 0.7f, 1);
                yield return new WaitForSeconds(0.02f);
            } else if(keikaTime < 85.6f) {  // 右棒
                timeTf[0].transform.localScale = new Vector3(61.5f, 0.7f, 1);
                timeTf[1].transform.localScale = new Vector3(0.7f, keikaTime-61.5f, 1);
                yield return new WaitForSeconds(0.02f);
            } else if(keikaTime < 147.1f) { // 下棒
                timeTf[1].transform.localScale = new Vector3(0.7f, 24.1f, 1);
                timeTf[2].transform.localScale = new Vector3(keikaTime-85.6f, 0.7f, 1);
                yield return new WaitForSeconds(0.02f);
            } else {
                timeTf[2].transform.localScale = new Vector3(61.5f, 0.7f, 1);
                timeTf[3].transform.localScale = new Vector3(0.7f, keikaTime-147.1f, 1);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    // 正誤判定
    IEnumerator judgement()
    {
        if(!judgementFlg) {

            // 判定が終わるまで空ルーチンを回る
            gameState = 10;

            // タップされたら問題番号加算
            quesCounter++;
            
            // 判定中
            judgementFlg = true;

            if(tapNum == correctAnswer) {
                // 正解だったら得点追加
                comboNum++;
                Dt.score += stage * 10 * comboNum;
                scoreText.GetComponent<Text>().text = "SCORE: " + Dt.score.ToString().PadLeft(4,'0');
                checkHighscore();

                // 正解インジケーター更新
                seikaiNum++;
                seikaiGuageText.GetComponent<Text>().text = Dt.seikaiGuage[seikaiNum];

                // 正解エフェクト
                kanaminSeikai.SetActive(true);
                kanaminThinking.SetActive(false);
                hanamaru.SetActive(true);
                audioSource.PlayOneShot(vSeikai[Random.Range(0, 2)]);
                yield return new WaitForSeconds(1.0f);
                hanamaru.SetActive(false);
                kanaminThinking.SetActive(true);
                kanaminSeikai.SetActive(false);

                // ５問正解でステージクリア
                if(seikaiNum >=5) {
                    gameState = 4;
                } else {
                    gameState = 1;
                }
            } else {
                // 不正解だったら連続正解数を0に
                comboNum = 0;

                // 不正解問題番号をお尻に追加
                Ques.Order[Dt.quesTail] = Ques.Order[quesCounter];
                Dt.quesTail++;

                // かなみん残念グラフィックにして１秒待つ
                kanaminZannen.SetActive(true);
                kanaminThinking.SetActive(false);
                otetsuki[otetsukiNum].SetActive(true);
                audioSource.PlayOneShot(vMachigai);
                yield return new WaitForSeconds(1.0f);
                otetsuki[otetsukiNum].SetActive(false);

                // おてつき加算
                otetsukiNum++;

                // おてつき３回でゲームオーバー
                if(otetsukiNum >= 3) {
                    SceneManager.LoadScene("GameOverScene");
                } else {
                    gameState = 1;
                }
                
                // ゲームオーバーへの遷移対策で０．１秒待ってから
                // かなみん考え中グラフィックに戻す
                yield return new WaitForSeconds(0.1f);
                kanaminThinking.SetActive(true);
                kanaminZannen.SetActive(false);
            }

            // 判定終了
            judgementFlg = false;
        }
    }

    // ステージクリア
    IEnumerator StageClear()
    {
        // ステージクリア処理中は空ルーチンを回る
        gameState = 10;

        // BGMを止める
        audioSource.Stop();

        // 問題文と選択肢をクリア
        questionText.GetComponent<Text>().text = "";
        for(int i=0; i<4; i++) {
            choiceText[i].GetComponent<Text>().text = "";
        }

        // アイキャッチ背景のトランスフォームコンポーネントの取得
        Transform tf = stageEyeCatchFrame.GetComponent<Transform>();

        // ステージクリア文字列をセット
        stageNumberText.GetComponent<Text>().text = "STAGE " + stage.ToString() + " CLEAR";

        // アイキャッチ背景の高さをゼロにしてから表示
        tf.transform.localScale = new Vector3(1, 0, 1);
        stageEyeCatchFrame.SetActive(true);

        // アイキャッチ背景をジワジワ大きくする
        for(int i=0; i<10; i++) {
            tf.transform.localScale = new Vector3(1, (float)(i) / 10.0f, 1);
            yield return new WaitForSeconds(0.02f);
        }

        // ステージクリアテキストを表示
        stageNumberText.SetActive(true);

        // ステージクリア音声のあと2.5秒待つ
        audioSource.PlayOneShot(vStageClear[Random.Range(0, 2)]);
        yield return new WaitForSeconds(2.5f);

        // ステージアイキャッチを非表示
        stageEyeCatchFrame.SetActive(false);
        stageNumberText.SetActive(false);

        // ステージを進める
        stage++;
        gameState = 0;
    }

    // ハイスコアチェック
    private void checkHighscore()
    {
        // 現スコアがハイスコアを上回ったら
        if (Dt.score > Dt.highscore) {
            // ハイスコア更新
            Dt.highscore = Dt.score;
            highscoreText.GetComponent<Text>().text = "HIGHSCORE: " + Dt.highscore.ToString().PadLeft(4,'0');

            // ハイスコア保存
            PlayerPrefs.SetInt(Dt.SAVE_KEY, Dt.highscore);
            PlayerPrefs.Save();
        }
    }


}
