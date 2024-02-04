using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // ���� ���� �̸�

    // weapon type
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range; // ���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾� �ӵ�
    public float attackDelay; // ���� ������
    public float attackDelayA; // ���� Ȱ��ȭ �������� �ð� (�ָ� ������ ���������� �ð�)
    public float attackDelayB; // ���� ��Ȱ��ȭ �������� �ð�

    public Animator anim;

}
