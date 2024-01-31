using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))] // 반드시 특정 컴포넌트가 필요한 경우에 추가
public class Player : MonoBehaviour
{
    /// <summary>
    /// 유니티의 새로운 입력 방
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 마지막으로 입력된 방향을 기록하는 변수
    /// </summary>
    Vector3 inputDir = Vector3.zero;
    
    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float moveSpeed = 0.5f;

    /// <summary>
    /// 애니메이터 & 애니메이터 변수 호출
    /// </summary>
    Animator anim;
    readonly int InputX_String = Animator.StringToHash("InputX");
    readonly int InputY_String = Animator.StringToHash("InputY");
    readonly int InputPower_String = Animator.StringToHash("InputPower");
    float inputY;
    float inputPower;

    /// <summary>
    /// SnowBall 프리팹
    /// </summary>
    public GameObject snowPrefab;

    /// <summary>
    /// SnowBall 발사 위치 지정용
    /// </summary>
    Transform attackTransform;

    /// <summary>
    /// 플레이어의 HP
    /// </summary>
    public int hp = 100;
    // int hpMax = 100;

    public int HP
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = Math.Min(value, 100); // 최대 체력 100
                onHPChange?.Invoke(hp); // 이 델리게이트에 함수를 등록한 모든 대상에게 변경된 체력을 알림
            }

            if (hp <= 0) // HP가 0 이하가 되면 죽는다.
            {
                hp = 0;
            }
        }
    }

    /// <summary>
    /// 체력이 변경되었을 때 알리는 델리게이트 (파라메터 : 변경된 체력)
    /// </summary>
    public Action<int> onHPChange;

    /// <summary>
    /// 아이템을 먹으면 플레이어가 얻는 점수
    /// </summary>
    public int score = 0;

    public int Score
    {
        get => score;
        private set
        {
            if (score != value)
            {
                score = Math.Min(value, 99999); // 최대 점수 99999
                onScoreChange?.Invoke(score); // 이 델리게이트에 함수를 등록한 모든 대상에게 변경된 점수를 알림
            }
        }
    }

    /// <summary>
    /// 점수가 변경되었을 때 알리는 델리게이트 (파라메터 : 변경된 점수)
    /// </summary>
    public Action<int> onScoreChange;

    /// <summary>
    /// 플레이어 점프 정도
    /// </summary>
    public float jumpPower = 1.5f;

    /// <summary>
    /// 플레이어 점프 횟수와 최댓값
    /// </summary>
    private int jumpCount = 0;
    public int jumpMax = 2;

    Rigidbody2D rigid2d;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 맞았을 때 무적 시간
    /// </summary>
    public float invincibleTime = 2.0f;

    public Action<int> onDie;

    /// <summary>
    /// 시작 함수 (실행 순서 1)
    /// </summary>
    public void Awake()
    {
        inputY = 0.0f;
        Score = 0;
        
        inputActions = new PlayerInputActions(); // 인풋 액션 생성

        anim = GetComponent<Animator>(); // 이 스크립트가 들어있는 게임 오브젝트에서 컴포넌트를 찾아서 anim에 저장하기

        attackTransform = transform.GetChild(0); // 이 게임 오브젝트의 첫번째 자식 찾기

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 이 스크립트가 포함된 게임 오브젝트가 활성화되면 호출된다.
    private void OnEnable()
    {
        inputActions.Player.Enable(); // 활성화될 때 Player 액션맵을 활성화
        inputActions.Player.Attack.performed += OnAttack; // Player 액션맵의 Attack 액션에 OnAttack 함수를 연결 (눌렀을 때만 연결된 함수 실행)
        inputActions.Player.Attack.canceled += OnAttack; // Player 액션맵의 Attack 액션에 OnAttack 함수를 연결 (땠을 때만 연결된 함수 실행)

        inputActions.Player.Slide.performed += OnSlide;
        inputActions.Player.Slide.canceled += OnSlide;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJump;

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    // 이 스크립트가 포함된 게임 오브젝트가 비활성화되면 호출된다.
    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;

        inputActions.Player.Jump.canceled -= OnJump;
        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Slide.canceled -= OnSlide;
        inputActions.Player.Slide.performed -= OnSlide;

        inputActions.Player.Attack.canceled -= OnAttack; // Player 액션맵의 Attack 액션에 OnAttack 함수를 연결 (땠을 때만 연결된 함수 실행)
        inputActions.Player.Attack.performed -= OnAttack; // Player 액션맵의 Attack 액션에서 OnAttack 함수를 연결 해제
        inputActions.Player.Disable(); // Player 액션맵을 비활성화
    }

    /// <summary>
    /// Move 액션이 발동했을 때, 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("OnMove");
        inputDir = context.ReadValue<Vector2>();
        anim.SetFloat(InputX_String, 5.0f + inputDir.x); // 애니메이터가 가지는 InputX 파라메터(변수)에 inputDir.x값을 넣기
    }

    /// <summary>
    /// Jump 액션이 발동했을 때, 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("OnJump");
        if (context.performed)
        {
            inputY = 5.0f;
            anim.SetFloat(InputY_String, inputY);

            if (jumpCount < jumpMax)
            {
                // 점프 파워 조정
                rigid2d.gravityScale = 1.6f;
                rigid2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                jumpCount++;

                Debug.Log($"Player의 JumpCount : {jumpCount}");
            }
        }

        if (context.canceled)
        {
            inputY = 0.0f;
            anim.SetFloat(InputY_String, inputY);
        }
    }

    /// <summary>
    /// Slide 액션이 발동했을 때, 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnSlide(InputAction.CallbackContext context)
    {
        //Debug.Log("OnSlide");
        if (context.performed)
        {
            inputY = -5.0f;
            anim.SetFloat(InputY_String, inputY);
        }

        if (context.canceled)
        {
            inputY = 0.0f;
            anim.SetFloat(InputY_String, inputY);
        }
    }

    /// <summary>
    /// Attack 액션이 발동했을 때, 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("OnAttack");
        if (context.performed)
        {
            inputPower = 5.0f;
            anim.SetFloat(InputPower_String, inputPower);
            Instantiate(snowPrefab, attackTransform.position, Quaternion.identity); // 플레이어가 눈을 던지는 이팩트
        }

        if (context.canceled)
        {
            inputPower = -5.0f;
            anim.SetFloat(InputPower_String, inputPower);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1초당 moveSpeed만큼의 속도로, inputDir 방향으로 움직이게 만들기
        transform.Translate(Time.deltaTime * moveSpeed * inputDir);

        /*
        if (Input.GetKeyUp(KeyCode.W) && jumpCount <= 1)
        {
            //Debug.Log("Jump Key가 눌러짐");
            rigid2d.gravityScale = 1.0f;
            rigid2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item")) // 플레이어가 아이템을 먹으면 점수 얻기
        {
            Score += 10; // score 값만큼 점수 얻기
            Debug.Log($"Player의 Score : {Score}"); // Score 값 출력
            Destroy(collision.gameObject); // 충돌한 대상 삭제
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Cone")) // 플레이어가 적을 만나면 죽기
        {
            HP -= 20; // 20만큼 체력 감소 // 하트 5개 배정 예정

            StartCoroutine(InvincibleMode()); // 무적 모드에 들어감

            Debug.Log($"Player의 HP : {HP}"); // HP 값 출력
        }

        if (collision.gameObject.CompareTag("Ways"))
        {
            jumpCount = 0;
        }
    }

    /// <summary>
    /// 무적 모드 처리용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초 동안 계속하기
        {
            timeElapsed += Time.deltaTime;

            // 플레이어의 알파값 : 1 -> 0 -> 1 -> 0...
            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f; // 코사인 결과를 1 ~ 0 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha); // 알파에 지정 (깜박거리게 된다.)

            yield return null;
        }

        //2초가 지난 후
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white; // 알파값도 원상복구
    }

    /// <summary>
    /// 플레이어가 죽는 함수
    /// </summary>
    public void OnDie()
    {
        //Instantiate(diePrefab, transform.position, Quaternion.identity); // 플레이어가 죽는 이팩트
        Destroy(gameObject); // 자기 자신 삭제

        // 사망했음을 알림
        onDie?.Invoke(Score);
    }
}
