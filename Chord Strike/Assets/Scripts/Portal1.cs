using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal1 : Portal
{
    void Start()
    {
        base.Start();

        gameOverScene = "GameCompletion1";
        enemyThreshold = 12;
    }
}
