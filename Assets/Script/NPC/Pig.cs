using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; // 동물 이름
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    private Vector3 direction; // 방향 설정

    // 상태변수
    private bool isAction; // 행동 중인지 아닌지 판별.
    private bool isWalking; // 걷는지여부
    [SerializeField] private float walkTime; // 걷는 시간
    [SerializeField] private float waitTime; // 대기 시간
    private float currentTime; // isAction true일 때만 currentTime 깎고 0되면 다시 isAction false

    // 내가 지금 이해한 바로는 isAction이라는 변수를 두고 여러 가지 동작 중 1개를 골라서 실행 시간 동안 실행하고 
    // currentTime이 0이되면 isAction 다시 false로 바꾼 뒤 다른 동작을 랜덤으로 골라서 실행하는 것을 반복하는 듯?
    // wait, walk 등이 동작인 듯.


    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        ElapseTime();
        Rotation(); // 코루틴이 아니라 보간법으로 회전하면서 걸어감
        Move();
    }


    // 뭐 얘는 이렇게 구현했는데
    // TryWalk() 함수 밑에 보간법 이용한 코루틴으로 해도 됐을 듯? .. # Update vs Coroutine 알아보기!
    private void Move()
    {
        if(isWalking)
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ResetAction(); // 행동 초기화, 다음 행동 랜덤 실행
            }
        }
    }

    // 다음 행동을 위해 상태 초기화
    private void ResetAction()
    {
        isWalking = false; isAction = true;
        anim.SetBool("Walk",isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        int _random = Random.Range(0, 4); // 0, 1, 2, 3 : 대기, 풀뜯기, 두리번, 걷기
                                            // integer일 때는 범위에서 끝값을 포함 안하는데 float일 때는 포함한다네요
        switch (_random)
        {
            case 0:
                Wait();
                break;
            case 1:
                Eat();
                break;
            case 2:
                Peek();
                break;
            case 3:
                TryWalk();
                break;

        }
    }

    private void Wait()
    {
        currentTime = waitTime; // 이 동작을 시작하고 다음 동작 시작하기까지의 시간
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime; // 이 동작을 시작하고 다음 동작 시작하기까지의 시간
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime; // 이 동작을 시작하고 다음 동작 시작하기까지의 시간
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walk", isWalking);
        currentTime = walkTime; // 이 동작을 시작하고 다음 동작 시작하기까지의 시간
        Debug.Log("걷기");
    }
}
