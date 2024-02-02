using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    // 연사 속도 계산
    private float currentFireRate = 0;

    // 상태 변수
    private bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;

    // 효과음 재생
    private AudioSource audioSource;

    // 원래 포지션 값
    private Vector3 originPos;

    // 충돌 정보 받아옴.
    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam; // 카메라 시점으로 정 가운데 (크로스헤어)로부터 ray를 쏘기 위해 카메라 받아옴

    // 피격이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
        Debug.DrawRay(theCam.transform.position, theCam.transform.forward * currentGun.range, Color.red);
    }

    // 연사 속도 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // update 될때마다 그 update되는데 걸린 시간만큼 빼줌
        }
    }

    // 발사 시도
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }


    // 발사 직전 
    private void Fire() // 방아쇠 당김 ㅋ; (연사속도 초기화)
    {
        if (currentGun.currentBulletCount > 0)
            Shoot();
        else
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }


    // 발사 후
    private void Shoot() // 진짜 총알 발사되는 액션 구현
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit(); // 총알을 진짜로 구현하려면 미리 총알을 만들어두고 object pooling을 이용해서 구현해야 함. 여기서는 그냥 쏘는대로 맞는 것으로 구현
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {
        
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
        {
            // Instantiate : 객체 생성 메서드
            // hitInfo.point : 충돌한 지점의 좌표 반환
            // Quaternion.LookRotation() : 해당 벡터를 바라보는 회전 상태 반환
            // hitInfo.normal : 충돌한 표면의 노멀값 반환
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    // 재장전시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }


    // 재장전 코루틴
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            yield return new WaitForSeconds(currentGun.reloadTime); // 재장전 시간 동안 대기

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.carryBulletCount -= currentGun.reloadBulletCount - currentGun.currentBulletCount;
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                
            } else
            {
                currentGun.currentBulletCount += currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    // 정조준 시도
    private void TryFineSight()
    {
        if (Input.GetButton("Fire2") && !isReload)
        {
            FineSight();
        }
        else if (Input.GetButtonUp("Fire2") && !isReload)
        {
            DeFineSight();
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            DeFineSight();
        }
    }


    // 정조준 함수
    private void FineSight()
    {
        isFineSightMode = true;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        StopAllCoroutines();
        StartCoroutine(FineSightActivateCoroutine());
    }


    // 정조준 해제 함수
    private void DeFineSight()
    {
        isFineSightMode = false;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        StopAllCoroutines();
        StartCoroutine(FineSightDeactivateCoroutine());
    }


    // 정조준 코루틴
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }


    // 정조준 해제 코루틴
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }


    // 반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        // x축 왔다갔다로 반동 구현하는 듯
        // 각각 최대로 당겨지는 위치를 의미하는 벡터값
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode) // 조준 아닐 때 반동 = 정조준보다 더 반동 크게 (originPos와 retroActionForce 차이가 크게)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작 (총이 뒤로 당겨짐)
            // 여기서 currentGun.transform.localPosition은 총의 원래 위치, 즉 originPos와 같음

            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원위치 (총을 다시 앞으로)
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        } else // 정조준일때 반동 = 평소보다 반동 작게 (fineSightOriginPos와 retroActionFineSightForce의 차이가 크지 않게)
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작 (총이 뒤로 당겨짐)
            // 여기서 currentGun.transform.localPosition은 총의 정조준 위치, 즉 currentGun.fineSightOriginPos와 같음

            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치 (총을 다시 앞으로)
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public Gun GetGun()
    {
        return currentGun;
    }
}
