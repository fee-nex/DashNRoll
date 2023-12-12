using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform playerPos;
    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    void Update()
    {
        if(playerPos != null)
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
    }
}
