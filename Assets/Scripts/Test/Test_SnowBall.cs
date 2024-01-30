using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SnowBall : TestBase
{
    public GameObject prefab;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(prefab); // 프리팹을 이용해서 게임 오브젝트 만들기
    }
}
