using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate = 0;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // update 될때마다 그 update되는데 걸린 시간만큼 빼줌
        }
        TryFire();
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire() // 방아쇠 당김 ㅋ; (연사속도 초기화)
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot() // 진짜 총알 발사되는 액션 구현
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Debug.Log("발사함");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
