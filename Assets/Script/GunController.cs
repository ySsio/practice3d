using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate = 0;

    private bool isReload = false;

    private AudioSource audioSource;

    private bool isFineSightMode = false;

    // ���� ������ ��
    [SerializeField]
    private Vector3 originPos;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // update �ɶ����� �� update�Ǵµ� �ɸ� �ð���ŭ ����
        }
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }



    private void Fire() // ��Ƽ� ��� ��; (����ӵ� �ʱ�ȭ)
    {
        if (currentGun.currentBulletCount > 0)
            Shoot();
        else
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void Shoot() // ��¥ �Ѿ� �߻�Ǵ� �׼� ����
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            yield return new WaitForSeconds(currentGun.reloadTime); // ������ �ð� ���� ���

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
            Debug.Log("������ �Ѿ��� �����ϴ�.");
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

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

    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            DeFineSight();
        }
    }

    private void FineSight()
    {
        isFineSightMode = true;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        StopAllCoroutines();
        StartCoroutine(FineSightActivateCoroutine());
    }

    private void DeFineSight()
    {
        isFineSightMode = false;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        StopAllCoroutines();
        StartCoroutine(FineSightDeactivateCoroutine());
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        // x�� �Դٰ��ٷ� �ݵ� �����ϴ� ��
        // ���� �ִ�� ������� ��ġ�� �ǹ��ϴ� ���Ͱ�
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode) // ���� �ƴ� �� �ݵ� = �����غ��� �� �ݵ� ũ�� (originPos�� retroActionForce ���̰� ũ��)
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ���� (���� �ڷ� �����)
            // ���⼭ currentGun.transform.localPosition�� ���� ���� ��ġ, �� originPos�� ����

            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // ����ġ (���� �ٽ� ������)
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        } else // �������϶� �ݵ� = ��Һ��� �ݵ� �۰� (fineSightOriginPos�� retroActionFineSightForce�� ���̰� ũ�� �ʰ�)
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // �ݵ� ���� (���� �ڷ� �����)
            // ���⼭ currentGun.transform.localPosition�� ���� ������ ��ġ, �� currentGun.fineSightOriginPos�� ����

            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // ����ġ (���� �ٽ� ������)
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }
}
