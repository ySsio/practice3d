using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    [SerializeField] protected string animalName; // ���� �̸�
    [SerializeField] protected int hp;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    protected float applySpeed;
    [SerializeField] protected float rotationSpeed;
    protected Vector3 direction; // ���� ����

    // ���º���
    protected bool isAction; // �ൿ ������ �ƴ��� �Ǻ�.
    protected bool isWalking; // �ȴ�������
    protected bool isRunning; // �ٴ�������
    protected bool isDead = false; // �׾���

    [SerializeField] protected float walkTime; // �ȴ� �ð�
    [SerializeField] protected float waitTime; // ��� �ð�
    [SerializeField] protected float runTime; // �ٴ� �ð�
    protected float currentTime; // isAction true�� ���� currentTime ��� 0�Ǹ� �ٽ� isAction false

    // ���� ���� ������ �ٷδ� isAction�̶�� ������ �ΰ� ���� ���� ���� �� 1���� ��� ���� �ð� ���� �����ϰ� 
    // currentTime�� 0�̵Ǹ� isAction �ٽ� false�� �ٲ� �� �ٸ� ������ �������� ��� �����ϴ� ���� �ݺ��ϴ� ��?
    // wait, walk ���� ������ ��.


    // �ʿ��� ������Ʈ
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource theAudioSource;
    [SerializeField] protected AudioClip[] sounds;
    [SerializeField] protected AudioClip soundHurt;
    [SerializeField] protected AudioClip soundDead;


    // �� ��� �̷��� �����ߴµ�
    // TryWalk() �Լ� �ؿ� ������ �̿��� �ڷ�ƾ���� �ص� ���� ��? .. # Update vs Coroutine �˾ƺ���!
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
                ResetAction(); // �ൿ �ʱ�ȭ, ���� �ൿ ���� ����
            }
        }
    }

    // ���� �ൿ�� ���� ���� �ʱ�ȭ
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
        currentTime = walkTime; // �� ������ �����ϰ� ���� ���� �����ϱ������ �ð�
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");
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

        // �´� �ִϸ��̼� ����
        anim.SetTrigger("Hurt");

        // �Ҹ� ���
        PlaySE(soundHurt);
        // �÷��̾�κ��� ��������
        
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 4); // �ϻ� ���� 3��
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
        isWalking = false; isRunning = false; // �����̸� �ȵǴϱ� ����
        anim.SetTrigger("Dead");

    }
}
