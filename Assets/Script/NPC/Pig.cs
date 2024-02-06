using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; // ���� �̸�
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    private Vector3 direction; // ���� ����

    // ���º���
    private bool isAction; // �ൿ ������ �ƴ��� �Ǻ�.
    private bool isWalking; // �ȴ�������
    [SerializeField] private float walkTime; // �ȴ� �ð�
    [SerializeField] private float waitTime; // ��� �ð�
    private float currentTime; // isAction true�� ���� currentTime ��� 0�Ǹ� �ٽ� isAction false

    // ���� ���� ������ �ٷδ� isAction�̶�� ������ �ΰ� ���� ���� ���� �� 1���� ��� ���� �ð� ���� �����ϰ� 
    // currentTime�� 0�̵Ǹ� isAction �ٽ� false�� �ٲ� �� �ٸ� ������ �������� ��� �����ϴ� ���� �ݺ��ϴ� ��?
    // wait, walk ���� ������ ��.


    // �ʿ��� ������Ʈ
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
        Rotation(); // �ڷ�ƾ�� �ƴ϶� ���������� ȸ���ϸ鼭 �ɾ
        Move();
    }


    // �� ��� �̷��� �����ߴµ�
    // TryWalk() �Լ� �ؿ� ������ �̿��� �ڷ�ƾ���� �ص� ���� ��? .. # Update vs Coroutine �˾ƺ���!
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
                ResetAction(); // �ൿ �ʱ�ȭ, ���� �ൿ ���� ����
            }
        }
    }

    // ���� �ൿ�� ���� ���� �ʱ�ȭ
    private void ResetAction()
    {
        isWalking = false; isAction = true;
        anim.SetBool("Walk",isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
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
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walk", isWalking);
        currentTime = walkTime; // �� ������ �����ϰ� ���� ���� �����ϱ������ �ð�
        Debug.Log("�ȱ�");
    }
}
