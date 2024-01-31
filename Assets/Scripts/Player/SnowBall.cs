using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    /// <summary>
    /// SnowBall의 이동 속도
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// SnowBall이 터지는 이팩트용 프리팹
    /// </summary>
    public GameObject effectPrefab;

    /// <summary>
    /// 적 체력
    /// </summary>
    public float enemyHP = 100.0f;

    /// <summary>
    /// SnowBall의 수명
    /// </summary>
    public float lifeTime = 10.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // lifeTime 이후에 스스로 사라지기
    }

    private void Update()
    {
        // 시작하자마자 계속 오른쪽으로 초속 7로 움직이게 만들기
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌이 시작했을 때 실행
        //Debug.Log($"OnCollisionEnter2D : {collision.gameObject.name}");

        Enemy enemy = FindAnyObjectByType<Enemy>(); // 적 찾기

        if (collision.gameObject.CompareTag("Enemy")) // collision의 게임 오브젝트가 "Enemy"라는 태그를 가지는지 확인하는 함수
        {
            if (enemy.enemyHp > 0)
            {
                enemy.enemyHp -= 50;
                Debug.Log($"Enemy의 HP : {enemy.enemyHp}");
            }

            if (enemy.enemyHp == 0)
            {
                Destroy(collision.gameObject); // 충돌한 대상을 제거하기

                Player player = FindAnyObjectByType<Player>();
                player.score += 100; // score 값만큼 점수 얻기
            }

            else
            {
                enemy.enemyHp = 0;
            }

            Instantiate(effectPrefab, transform.position, Quaternion.identity); // hit 이팩트 생성
            Destroy(gameObject); // 자기 자신은 무조건 삭제
        }

        //Factory.Instance.GetHitEffect(transform.position);
        //gameObject.SetActive(false); // 비활성화 -> 풀로 되돌리기
    }
}
