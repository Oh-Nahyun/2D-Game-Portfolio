using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    /// <summary>
    /// 재활용 오브젝트가 비활성화 될 때 실행되는 델리게이트
    /// </summary>
    public Action onDisable;

    protected virtual void OnEnable() ///// 활성화될 경우
    {
        transform.localPosition = Vector3.zero; // 부모의 위치로 보내기
        transform.localRotation = Quaternion.identity; // 부모의 회전과 같게 만들기

        StopAllCoroutines(); // 리셋 용도 (없어도 상관없음)
    }

    protected virtual void OnDisable() ///// 비활성화될 경우
    {
        onDisable?.Invoke(); // 비활성화 되었음을 알림 (풀 만들 때 할일이 등록되어야 함)
    }

    /// <summary>
    /// 일정 시간 후에 이 게임 오브젝트를 비활성화 시키는 코루틴
    /// </summary>
    /// <param name="delay">비활성화 될 때까지 걸리는 시간</param>
    /// <returns></returns>
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay만큼 기다리고
        gameObject.SetActive(false); // 비활성화
    }
}
