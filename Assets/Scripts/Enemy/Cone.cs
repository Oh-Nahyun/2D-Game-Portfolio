using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 1.5f;

    /// <summary>
    /// 장애물 스폰 높이
    /// </summary>
    float spawnY = -3.5f;

    /// <summary>
    /// 장애물의 수명
    /// </summary>
    //public float lifeTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //elapsedTime = 0.0f; // 초기화
        //Destroy(gameObject, lifeTime); // 장애물 수명 // lifeTime 이후에 스스로 사라지기
        spawnY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // 계속 왼쪽으로 진행
                                        spawnY, // spawnY에 따라 높이 변동하기
                                        0.0f);
    }
}
