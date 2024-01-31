using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 1.5f;

    /// <summary>
    /// 적 스폰 높이
    /// </summary>
    float spawnY = 0.0f;

    /// <summary>
    /// Enemy의 HP
    /// </summary>
    public int enemyHp = 100;

    public int EnemyHP
    {
        get => enemyHp;
        private set
        {
            if (enemyHp != value)
            {
                enemyHp = Math.Min(value, 100); // 최대 체력 100
            }

            if (enemyHp <= 0) // HP가 0 이하가 되면 죽는다.
            {
                enemyHp = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // 계속 왼쪽으로 진행
                                        spawnY,
                                        0.0f);
    }
}
