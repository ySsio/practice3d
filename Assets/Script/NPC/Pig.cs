using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; // ���� �̸�
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;
    private Vector3 direction; // ���� ����

    // ���º���
    private bool isAction; // �ൿ ������ �ƴ��� �Ǻ�.
    private bool isWalking; // �ȴ�������
    private bool isRunning; // �ٴ�������
    private bool isDead = false; // �׾���

    [SerializeField] private float walkTime; // �ȴ� �ð�
    [SerializeField] private float waitTime; // ��� �ð�
    [SerializeField] private float runTime; // �ٴ� �ð�
    private float currentTime; // isAction true�� ���� currentTime ��� 0�Ǹ� �ٽ� isAction false

    // ���� ���� ������ �ٷδ� isAction�̶�� ������ �ΰ� ���� ���� ���� �� 1���� ��� ���� �ð� ���� �����ϰ� 
    // currentTime�� 0�̵Ǹ� isAction �ٽ� false�� �ٲ� �� �ٸ� ������ �������� ��� �����ϴ� ���� �ݺ��ϴ� ��?
    // wait, walk ���� ������ ��.


    // �ʿ��� ������Ʈ
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
        Rotation(); // �ڷ�ƾ�� �ƴ϶� ���������� ȸ���ϸ鼭 �ɾ
        Move();
    }


    // �� ��� �̷��� �����ߴµ�
    // TryWalk() �Լ� �ؿ� ������ �̿��� �ڷ�ƾ���� �ص� ���� ��? .. # Update vs Coroutine �˾ƺ���!
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
                ResetAction(); // �ൿ �ʱ�ȭ, ���� �ൿ ���� ����
            }
        }
    }

    // ���� �ൿ�� ���� ���� �ʱ�ȭ
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
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");
    }

    // �����ްų�(�Ƹ� ����?���������ΰ�?) ���ݹ޾��� �� �ٵ���. �÷��̾� �ݴ� ��������
    private void Run(Vector3 _targetPos)
    {
        // ������ �ߴµ� Quaternion.LookRotation(������)�� ���� transform.position���� ������ �������� �ٶ󺸴� rotation�� ���ϴ� ����.
        // ��� �������� ������ǥ�谡 �ƴ� �����ǥ��� ��¥ ���ϴ� ��ġ �����ǿ��� transform.position�� ���ָ� target�� �ٶ󺸴� �������� ���ϴ� ��.
        // ���⼭�� target �ٶ󺸴� �ݴ������ ����ϱ� ���� �ݴ�� �����.
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

        // �´� �ִϸ��̼� ����
        anim.SetTrigger("Hurt");

        // �Ҹ� ���
        PlaySE(pigSoundHurt);
        // �÷��̾�κ��� ��������
        Run(_targetPos);
    }

    private void RandomSound()
    {
        int _random = Random.Range(0, 4); // �ϻ� ���� 3��
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
        isWalking = false; isRunning = false; // �����̸� �ȵǴϱ� ����
        anim.SetTrigger("Dead");

    }
}
