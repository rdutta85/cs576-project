using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal2 : Portal
{
    void Start()
    {
        base.Start();

        gameOverScene = "GameCompletion2";
        enemyThreshold = 12;
    }
}
