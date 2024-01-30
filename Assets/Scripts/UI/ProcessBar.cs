using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public Image mask;

    /// <summary>
    /// 최대 맵 진행률
    /// </summary>
    public float maximum = 100.0f;

    /// <summary>
    /// 현재 맵 진행률
    /// </summary>
    public float current = 0.0f;

    float elapsedTime = 0.0f;

    private void Start()
    {
        // 초기화
        current = 0.0f;
        elapsedTime = 0.0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime; // Time.deltaTime을 누적시키기 : 시간 측정하기
        current = elapsedTime;
        GetCurrnetFill();
    }

    void GetCurrnetFill()
    {
        float fillAmount = current / maximum;
        mask.fillAmount = fillAmount;
    }
}
