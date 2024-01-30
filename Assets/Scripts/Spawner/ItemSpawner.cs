using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    /// <summary>
    /// 아이템 프리팹
    /// </summary>
    public GameObject itemPrefab;

    /// <summary>
    /// 아이템 스폰 간격
    /// </summary>
    public float Interval = 0.5f;

    /// <summary>
    /// 아이템 스폰 높이
    /// </summary>
    const float DownY = -1.8f;
    const float UpY = 1.0f;

    const float MinX = -0.3f;
    const float MaxX = 0.3f;

    float elapsedTime = 0.0f;

    int spawnCounter = 0;

    private void Awake()
    {

    }

    private void Start()
    {
        elapsedTime = 0.0f; // 초기화
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; // Time.deltaTime을 누적시키기 : 시간 측정하기

        if (elapsedTime > Interval)
        {
            elapsedTime = 0.0f;
            Spawn();
        }
    }

    /// <summary>
    /// 아이템을 하나 스폰하는 함수
    /// </summary>
    void Spawn()
    {
        GameObject obj = Instantiate(itemPrefab, GetSpawnPosition(), Quaternion.identity); // (동적) 생성
        obj.transform.SetParent(transform); // 부모 설정
        obj.name = $"Item_{spawnCounter}"; // 게임 오브젝트 이름 바꾸기
        spawnCounter++;
    }

    /// <summary>
    /// 스폰할 위치를 리턴하는 함수
    /// </summary>
    /// <returns>스폰할 위치</returns>
    Vector3 GetSpawnPosition()
    {
        Vector3 pos = transform.position;
        float spawnItem = Random.Range(DownY, UpY);
        pos.y += (spawnItem < 0) ? DownY : UpY;
        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 색깔 지정
        Vector3 p0 = transform.position + Vector3.up * UpY; // 선의 시작점 계산
        Vector3 p1 = transform.position + Vector3.up * DownY; // 선의 도착점 계산
        Gizmos.DrawLine(p0, p1); // 시작점에서 도착점으로 선을 긋는다.
    }

    private void OnDrawGizmosSelected()
    {
        // 이 오브젝트를 선택했을 때, 사각형 그리기 (색상 변경하기)
        Gizmos.color = Color.blue;
        Vector3 p0 = transform.position + Vector3.up * UpY + Vector3.right * MinX;
        Vector3 p1 = transform.position + Vector3.up * DownY + Vector3.right * MinX;
        Vector3 p2 = transform.position + Vector3.up * UpY + Vector3.right * MaxX;
        Vector3 p3 = transform.position + Vector3.up * DownY + Vector3.right * MaxX;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p2);
        Gizmos.DrawLine(p1, p3);
        Gizmos.DrawLine(p2, p3);
    }
}
