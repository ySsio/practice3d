using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // ü��
    [SerializeField]
    private int hp;
    private int currentHp;

    // ��� =���¹̳�
    [SerializeField]
    private int sp;
    private int currentSp;

    // ��� ȸ���ӵ�
    [SerializeField]
    private int spRecoverSpeed;

    // ��� ��ȸ�� ������ (��� ��� �� ȸ�� ���۱��� �ɸ��� �ð�)
    [SerializeField]
    private int spRecoverTime;
    private int currentSpRecoverTime;

    // ��� ���� �ൿ�ߴ��� ����
    private bool spUsedTrigger;

    // ����
    [SerializeField]
    private int dp;
    private int currentDp;

    // �����
    [SerializeField]
    private int hungry;
    private int currentHungry;
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // �񸶸�
    [SerializeField]
    private int thirsty;
    private int currentThirsty;
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // ������?
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // �ʿ��� �̹���
    [SerializeField]
    private Image[] image_Gauge;
    private const int HP = 0, SP = 1, DP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRecoverTime();
        SPRecover();
        GaugeUpdate();
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            // currentHungryDecreaseTime�� hungryDecreaseTime�� ������ hungry 1 ���ҽ�Ű�� �ٽ� 0���� �ʱ�ȭ�ؼ� �ٽ� ����
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
                
        }
        else
        {
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�");
        }
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            // currentHungryDecreaseTime�� hungryDecreaseTime�� ������ hungry 1 ���ҽ�Ű�� �ٽ� 0���� �ʱ�ȭ�ؼ� �ٽ� ����
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }

        }
        else
        {
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�");
        }
    }

    private void GaugeUpdate()
    {
        image_Gauge[HP].fillAmount = (float)currentHp/hp;
        image_Gauge[SP].fillAmount = (float)currentSp / sp;
        image_Gauge[DP].fillAmount = (float)currentDp / dp;
        image_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        image_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        image_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void RecoverHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else 
            currentHp = hp;
    }

    public void DecreaseHP(int _count)
    {
        if (currentDp >0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp = Mathf.Max(currentHp - _count,0);

        if (currentHp < 0)
            Debug.Log("ĳ������ hp�� 0�� �Ǿ����ϴ�");
    }

    public void RecoverDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count)
    {
        currentDp = Mathf.Max(currentDp - _count, 0);

        if (currentDp < 0)
            Debug.Log("ĳ������ ������ 0�� �Ǿ����ϴ�");
    }

    public void RecoverHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        currentHungry = Mathf.Max(currentHungry - _count, 0);
    }

    public void RecoverThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        currentThirsty = Mathf.Max(currentThirsty - _count, 0);
    }

    // ����� Ư�� �ൿ�� �ϸ� ��� �ҰŴϱ� �ൿ�� �ϴ� ��ũ��Ʈ���� ȣ���ؾ� �� .. public���� ����
    public void DecreaseStamina(int _count)
    {
        spUsedTrigger = true;
        currentSpRecoverTime = 0; // ��� ����� ������ ��� ȸ���ð� ī��Ʈ �ϴ°� �ʱ�ȭ ��Ŵ

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;

    }

    private void SPRecoverTime()
    {
        if (spUsedTrigger)
        {
            if (currentSpRecoverTime < spRecoverTime)
                currentSpRecoverTime++;
            else
                spUsedTrigger = false;
        }
    }

    private void SPRecover()
    {
        if(!spUsedTrigger && currentSp < sp)
        {
            currentSp += spRecoverSpeed;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
