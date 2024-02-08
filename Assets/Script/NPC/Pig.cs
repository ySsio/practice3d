using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; // 동물 이름
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;
    private Vector3 direction; // 방향 설정

    // 상태변수
    private bool isAction; // 행동 중인지 아닌지 판별.
    private bool isWalking; // 걷는지여부
    private bool isRunning; // 뛰는지여부
    private bool isDead = false; // 죽엇냐

    [SerializeField] private float walkTime; // 걷는 시간
    [SerializeField] private float waitTime; // 대기 시간
    [SerializeField] private float runTime; // 뛰는 시간
    private float currentTime; // isAction true일 때만 currentTime 깎고 0되면 다시 isAction false

    // 내가 지금 이해한 바로는 isAction이라는 변수를 두고 여러 가지 동작 중 1개를 골라서 실행 시간 동안 실행하고 
    // currentTime이 0이되면 isAction 다시 false로 바꾼 뒤 다른 동작을 랜덤으로 골라서 실행하는 것을 반복하는 듯?
    // wait, walk 등이 동작인 듯.


    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;
    private AudioSource theAudioSource;
    [SerializeField] private AudioClip[] pigSounds;
    [SerializeField] private AudioClip pigSoundHurt;
    [SerializeField] private AudioClip pigSoundDead;


    void Start()
    {
        theAudioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (isDead)
            return;
        ElapseTime();
        Rotation(); // 코루틴이 아니라 보간법으로 회전하면서 걸어감
        Move();
    }


    // 뭐 얘는 이렇게 구현했는데
    // TryWalk() 함수 밑에 보간법 이용한 코루틴으로 해도 됐을 듯? .. # Update vs Coroutine 알아보기!
    private void Move()
    {
        if(isWalking || isRunning)
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3 (0f,direction.y,0f), 0.01f);
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
        isWalking = false; isRunning = false; isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walk",isWalking); anim.SetBool("Run", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();
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
        applySpeed = walkSpeed;
        Debug.Log("걷기");
    }

    // 위협받거나(아마 조준?당했을떄인가?) 공격받았을 때 뛰도록. 플레이어 반대 방향으로
    private void Run(Vector3 _targetPos)
    {
        // 전에도 했는데 Quaternion.LookRotation(포지션)은 현재 transform.position에서 설정한 포지션을 바라보는 rotation을 구하는 거임.
        // 대신 포지션이 월드좌표계가 아닌 상대좌표계라서 진짜 원하는 위치 포지션에서 transform.position를 뺴주면 target을 바라보는 방향으로 구하는 것.
        // 여기서는 target 바라보는 반대방향을 계산하기 위해 반대로 빼줬다.
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        applySpeed = runSpeed;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Run",isRunning);
    }

    public void Damaged(int _dmg, Vector3 _targetPos)
    {
        if (isDead)
            return;

        hp -= _dmg;
        if (hp<=0)
        {
            Dead();
            return;
        }

        // 맞는 애니메이션 실행
        anim.SetTrigger("Hurt");

        // 소리 재생
        PlaySE(pigSoundHurt);
        // 플레이어로부터 도망간다
        Run(_targetPos);
    }

    private void RandomSound()
    {
        int _random = Random.Range(0, 4); // 일상 사운드 3개
        PlaySE(pigSounds[_random]);
    }

    private void PlaySE(AudioClip _clip)
    {
        theAudioSource.clip = _clip;
        theAudioSource.Play();
    }

    private void Dead()
    {
        isDead = true;
        PlaySE(pigSoundDead);
        isWalking = false; isRunning = false; // 움직이면 안되니까 멈춤
        anim.SetTrigger("Dead");

    }
}
