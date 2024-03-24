using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dt : MonoBehaviour
{
    // 変数いろいろ
    public static int score;            // スコア
    public static int highscore;        // ハイスコア
    public static int quesTail;         // 設定クイズ番号のお尻

    // 問題の待ち時間棒の時間当たり増分
    public static readonly float[] Inc = {
        0.01f,   // ０番目はダミー
        0.05f, 0.10f, 0.15f, 0.20f, 0.25f, 0.30f, 0.35f, 0.40f, 0.45f, 0.50f, 
        0.55f, 0.60f, 0.65f, 0.70f, 0.75f, 0.80f, 0.85f, 0.90f, 0.95f, 1.00f, 
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
