using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dt : MonoBehaviour
{
    // 変数いろいろ
    public static int score;            // スコア
    public static int highscore;        // ハイスコア
    public static string SAVE_KEY = "HighScore";    // ハイスコア保存キー
    public static int quesTail;         // 設定クイズ番号のお尻

    // 問題の待ち時間棒の時間当たり増分
    public static readonly float[] Inc = {
        0.01f,   // ０番目はダミー
        0.025f, 0.050f, 0.075f, 0.100f, 0.125f, 0.150f, 0.175f, 0.200f, 0.225f, 0.250f, 
        0.275f, 0.300f, 0.325f, 0.350f, 0.375f, 0.400f, 0.425f, 0.450f, 0.475f, 0.500f 
    };

    // 正解数インジケーター
    public static readonly string[] seikaiGuage = {
        "□□□□□",
        "■□□□□",
        "■■□□□",
        "■■■□□", 
        "■■■■□",
        "■■■■■",
    }; 
}
