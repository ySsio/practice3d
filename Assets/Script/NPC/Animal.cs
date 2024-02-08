using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    [SerializeField] protected string animalName; // 동물 이름
    [SerializeField] protected int hp;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    protected float applySpeed;
    [SerializeField] protected float rotationSpeed;
    protected Vector3 direction; // 방향 설정

    // 상태변수
    protected bool isAction; // 행동 중인지 아닌지 판별.
    protected bool isWalking; // 걷는지여부
    protected bool isRunning; // 뛰는지여부
    protected bool isDead = false; // 죽엇냐

    [SerializeField] protected float walkTime; // 걷는 시간
    [SerializeField] protected float waitTime; // 대기 시간
    [SerializeField] protected float runTime; // 뛰는 시간
    protected float currentTime; // isAction true일 때만 currentTime 깎고 0되면 다시 isAction false

    // 내가 지금 이해한 바로는 isAction이라는 변수를 두고 여러 가지 동작 중 1개를 골라서 실행 시간 동안 실행하고 
    // currentTime이 0이되면 isAction 다시 false로 바꾼 뒤 다른 동작을 랜덤으로 골라서 실행하는 것을 반복하는 듯?
    // wait, walk 등이 동작인 듯.


    // 필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource theAudioSource;
    [SerializeField] protected AudioClip[] sounds;
    [SerializeField] protected AudioClip soundHurt;
    [SerializeField] protected AudioClip soundDead;


    // 뭐 얘는 이렇게 구현했는데
    // TryWalk() 함수 밑에 보간법 이용한 코루틴으로 해도 됐을 듯? .. # Update vs Coroutine 알아보기!
    protected void Move()
    {
        if (isWalking || isRunning)
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), rotationSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    protected void ElapseTime()
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
    protected virtual void ResetAction()
    {
        isWalking = false; isRunning = false; isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walk", isWalking); anim.SetBool("Run", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walk", isWalking);
        currentTime = walkTime; // 이 동작을 시작하고 다음 동작 시작하기까지의 시간
        applySpeed = walkSpeed;
        Debug.Log("걷기");
    }

    

    public virtual void Damaged(int _dmg, Vector3 _targetPos)
    {
        if (isDead)
            return;

        hp -= _dmg;
        if (hp <= 0)
        {
            Dead();
            return;
        }

        // 맞는 애니메이션 실행
        anim.SetTrigger("Hurt");

        // 소리 재생
        PlaySE(soundHurt);
        // 플레이어로부터 도망간다
        
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 4); // 일상 사운드 3개
        PlaySE(sounds[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudioSource.clip = _clip;
        theAudioSource.Play();
    }

    protected void Dead()
    {
        isDead = true;
        PlaySE(soundDead);
        isWalking = false; isRunning = false; // 움직이면 안되니까 멈춤
        anim.SetTrigger("Dead");

    }
}
