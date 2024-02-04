using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;
    private int currentHp;

    // 기력 =스태미나
    [SerializeField]
    private int sp;
    private int currentSp;

    // 기력 회복속도
    [SerializeField]
    private int spRecoverSpeed;

    // 기력 재회복 딜레이 (기력 사용 후 회복 시작까지 걸리는 시간)
    [SerializeField]
    private int spRecoverTime;
    private int currentSpRecoverTime;

    // 기력 감소 행동했는지 여부
    private bool spUsedTrigger;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도?
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
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
            // currentHungryDecreaseTime이 hungryDecreaseTime을 넘으면 hungry 1 감소시키고 다시 0으로 초기화해서 다시 시작
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
            Debug.Log("배고픔 수치가 0이 되었습니다");
        }
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            // currentHungryDecreaseTime이 hungryDecreaseTime을 넘으면 hungry 1 감소시키고 다시 0으로 초기화해서 다시 시작
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
            Debug.Log("목마름 수치가 0이 되었습니다");
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
            Debug.Log("캐릭터의 hp가 0이 되었습니다");
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
            Debug.Log("캐릭터의 방어력이 0이 되었습니다");
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

    // 기력은 특정 행동을 하면 닳게 할거니까 행동을 하는 스크립트에서 호출해야 함 .. public으로 만듦
    public void DecreaseStamina(int _count)
    {
        spUsedTrigger = true;
        currentSpRecoverTime = 0; // 기력 사용할 때마다 기력 회복시간 카운트 하는거 초기화 시킴

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
