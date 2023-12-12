using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    [SerializeField] private float radius, time;

    public GameObject[] enemy;
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        Vector2 spawnPos = GameObject.Find("Player").transform.position;
        spawnPos = Random.insideUnitCircle.normalized * radius;

        Instantiate(enemy[Random.Range(0,enemy.Length)], spawnPos,Quaternion.identity);
        yield return new WaitForSeconds(time);
        StartCoroutine(SpawnEnemy());
    }
}
