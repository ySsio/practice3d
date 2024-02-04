using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy; // ��Ȯ��
    public float fireRate; // ����ӵ�
    public float reloadTime;
    public int damage;

    public int reloadBulletCount; // źâ �Ѿ� ����
    public int currentBulletCount; // ���� �����ִ� �Ѿ� ��
    public int maxBulletCount; // �ִ� ���� ���� �Ѿ� �� (��� �ٴ� �� �ִ� �ִ� �Ѿ� ��)
    public int carryBulletCount; // ���� ���� ���� �Ѿ� �� (��� �ִ� �Ѿ� ��)

    public float retroActionForce; // �ݵ� ����
    public float retroActionFineSightForce; // ������ �� �ݵ� ����

    public Vector3 fineSightOriginPos; // ������ �� ���� ����� ���� �� �����ϱ� ���� ��ġ ���̶�� �Ѵ�..
    public Animator anim;
    public ParticleSystem muzzleFlash; // �� �߻��� �� ����Ʈ �����ϱ� ���� ����. ����Ʈ ������ �� ����ϴ� ��?

    public AudioClip fireSound; // �� �߻� �Ҹ�

}
