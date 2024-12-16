using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal3 : Portal
{
    void Start()
    {
        base.Start();

        gameOverScene = "GameCompletion3";
        enemyThreshold = 12;
    }
}
