using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal3 : Portal
{
    private StoneGiant boss;
    void Start()
    {
        base.Start();

        boss = GameObject.FindGameObjectsWithTag(enemySpawner.enemyTag)[0].GetComponent<StoneGiant>();
        gameOverScene = "GameCompletion3";
        enemyThreshold = 2;

        // start a coroutine to check if the boss is dead and activate the portal
        StartCoroutine(CheckBossDeath());
    }

    IEnumerator CheckBossDeath()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (boss.isDead)
            {
                activated = true;
                var main = ps.main;
                main.startColor = Color.blue;
                Debug.Log("Portal activated!");
            }
        }
    }
}
