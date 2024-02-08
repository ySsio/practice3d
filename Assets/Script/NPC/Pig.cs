using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : WeakAnimal
{   
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

    protected override void ResetAction()
    {
        base.ResetAction();
        RandomAction();
    }
}
