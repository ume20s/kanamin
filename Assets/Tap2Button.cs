using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap2Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // タップしたら
    public void onClick()
    {
        GameDirector.tapNum = 1;
        GameDirector.gameState++;
    }
}
