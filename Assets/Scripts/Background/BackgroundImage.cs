using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    /// <summary>
    /// 이미지의 너비 변수
    /// </summary>
    float width;

    /// <summary>
    /// 이미지의 속도 변수
    /// </summary>
    public float speed = 1.5f;

    /// <summary>
    /// BoxCollider2D
    /// </summary>
    BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>(); // BoxCollider2D 가져오기
        width = boxCollider2D.size.x; // BoxCollider2D의 size x값을 width에 넣어주기
        speed = 1.5f; // 속도
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position.x <= -width) // 카메라에 비춰지는 배경이 안보일 때까지 움직인 경우
        {
            Reposition();
        }
    }

    /// <summary>
    /// 이미지 이동 함수
    /// </summary>
    private void Move()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime); // 이미지를 좌측 방향으로 움직이도록 만듦
    }

    /// <summary>
    /// 이미지 반복 함수
    /// </summary>
    void Reposition()
    {
        Vector3 offset = new Vector3(width * 2, 0, 0); // 두배의 x값
        transform.position = transform.position + offset; // 자기 자신의 위치 변화
    }
}
