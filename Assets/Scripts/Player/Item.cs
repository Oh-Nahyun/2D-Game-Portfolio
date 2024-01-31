using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 1.5f;

    /// <summary>
    /// 아이템 스폰 높이
    /// </summary>
    float spawnY = 0.0f;

    /// <summary>
    /// 아이템의 수명
    /// </summary>
    //public float lifeTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //elapsedTime = 0.0f; // 초기화
        //Destroy(gameObject, lifeTime); // 아이템 수명 // lifeTime 이후에 스스로 사라지기
        spawnY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // 계속 왼쪽으로 진행
                                        spawnY, // spawnY에 따라 높이 변동하기
                                        0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Instantiate(effectPrefab, transform.position, Quaternion.identity); // 아이템 먹었을 때 나오는 이팩트
            Destroy(gameObject); // 자기 자신은 무조건 삭제
        }
    }
}
