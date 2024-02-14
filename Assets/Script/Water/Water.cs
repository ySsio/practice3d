using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; // �� �� �߷�
    private float originDrag; // �� �� ���������� ������� ���ƿ�

    [SerializeField] private Color waterColor;      // �� �� ȭ���
    [SerializeField] private float waterFogDensity; // �� �� fogdensity

    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterBreath;

    [SerializeField] private float breathTime;
    private float currentBreathTime;

    [SerializeField] private float totalBreath;
    private float currentBreath;
    private float temp = 0;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Text text_totalBreath;
    [SerializeField] private Text text_currentBreath;
    [SerializeField] private Image image_gauge;

    private StatusController thePlayerStat;

    private Color originColor;
    private float originFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0; // Player rigidbody�� drag �� �׳� �ϵ��ڵ����� ���� ��

        thePlayerStat = FindObjectOfType<StatusController>();
        currentBreath = totalBreath;
        text_totalBreath.text = "/" + Mathf.RoundToInt(totalBreath).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.isWater )
        {
            currentBreathTime += Time.deltaTime;
            if (currentBreathTime >= breathTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreath);
                currentBreathTime = 0;
            }
        }


        DecreaseBreath();
    }

    private void DecreaseBreath()
    {
        if (GameManager.isWater)
        {
            if (currentBreath <= 0 )
            {
                temp += Time.deltaTime;
                // 1�ʸ��� �� 1 ��� �ӽ÷� ����
                if (temp >= 1)
                {
                    thePlayerStat.DecreaseHP(10);
                    temp = 0;
                }
                    
            }
            else
            {
                currentBreath -= Time.deltaTime;
                text_currentBreath.text = Mathf.RoundToInt(currentBreath).ToString();
                image_gauge.fillAmount = currentBreath / totalBreath;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            originColor = RenderSettings.fogColor;
            originFogDensity = RenderSettings.fogDensity;
            GetInWater(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetInWater(Collider _Player)
    {
        go_BaseUI.SetActive(true);

        GameManager.isWater = true;
        _Player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        SoundManager.instance.PlaySE(sound_WaterIn);

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void GetOutWater(Collider _Player)
    {
        if (!GameManager.isWater)
            return;

        go_BaseUI.SetActive(false);

        currentBreath = totalBreath;
        GameManager.isWater = false;
        _Player.transform.GetComponent<Rigidbody>().drag = originDrag;
        SoundManager.instance.PlaySE(sound_WaterOut);

        RenderSettings.fogColor = originColor;
        RenderSettings.fogDensity = originFogDensity;
    }
}
