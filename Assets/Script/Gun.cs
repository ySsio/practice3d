using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime;
    public int damage;

    public int reloadBulletCount; // 탄창 총알 개수
    public int currentBulletCount; // 현재 남아있는 총알 수
    public int maxBulletCount; // 최대 소유 가능 총알 수 (들고 다닐 수 있는 최대 총알 수)
    public int carryBulletCount; // 현재 소유 중인 총알 수 (들고 있는 총알 수)

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준 시 반동 세기

    public Vector3 fineSightOriginPos; // 정조준 시 총이 가운데로 오는 걸 구현하기 위한 위치 값이라고 한다..
    public Animator anim;
    public ParticleSystem muzzleFlash; // 총 발사할 때 이펙트 구현하기 위한 변수. 이펙트 구현할 때 사용하는 듯?

    public AudioClip fireSound; // 총 발사 소리

}
