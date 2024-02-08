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
        Rotation(); // �ڷ�ƾ�� �ƴ϶� ���������� ȸ���ϸ鼭 �ɾ
        Move();
    }

    private void RandomAction()
    {
        RandomSound();
        int _random = Random.Range(0, 4); // 0, 1, 2, 3 : ���, Ǯ���, �θ���, �ȱ�
                                          // integer�� ���� �������� ������ ���� ���ϴµ� float�� ���� �����Ѵٳ׿�
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
        currentTime = waitTime; // �� ������ �����ϰ� ���� ���� �����ϱ������ �ð�
        Debug.Log("���");
    }
    private void Eat()
    {
        currentTime = waitTime; // �� ������ �����ϰ� ���� ���� �����ϱ������ �ð�
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ���");
    }
    private void Peek()
    {
        currentTime = waitTime; // �� ������ �����ϰ� ���� ���� �����ϱ������ �ð�
        anim.SetTrigger("Peek");
        Debug.Log("�θ���");
    }

    protected override void ResetAction()
    {
        base.ResetAction();
        RandomAction();
    }
}
