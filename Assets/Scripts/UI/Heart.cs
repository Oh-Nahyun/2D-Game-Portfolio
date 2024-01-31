using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    /// <summary>
    /// 하트 체력
    /// </summary>
    public int health;

    /// <summary>
    /// 하트 컨테이너 수
    /// </summary>
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Awake()
    {
        health = 5;
        numOfHearts = 5;      
    }

    private void Update()
    {
        OnPrint();
        if (health == 0)
        {
            Player player = FindAnyObjectByType<Player>(); // 플레이어 찾기
            player.OnDie();
        }
    }

    /// <summary>
    /// 하트 설정
    /// </summary>
    private void OnHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // 체력 최대치 설정
            if (numOfHearts < health) // 체력이 하트 컨테이너 수보다 커지면 안되므로 커질 경우
                health = numOfHearts; // 같도록 만들어준다.

            // 하트 이미지 선택
            if (i < health) // i가 health값보다 작으면
                hearts[i].sprite = fullHeart; // 꽉 찬 하트를 가진다.
            else // i가 health값보다 크거나 같으면
                hearts[i].sprite = emptyHeart; // 빈 하트를 가진다.

            // 전체 하트 개수 설정
            if (i < numOfHearts) // 하트 수가 numOfHearts보다 작으면
                hearts[i].enabled = true; // 활성화
            else // 하트 수가 numOfHearts보다 크거나 같으면
                hearts[i].enabled = false; // 비활성화
        }
    }

    /// <summary>
    /// 체력에 따른 하트 출력
    /// </summary>
    private void OnPrint()
    {
        Player player = FindAnyObjectByType<Player>(); // 플레이어 찾기
        health = player.hp / 20;
        OnHeart();
    }
}
