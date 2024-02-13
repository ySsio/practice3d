using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; // ���� ���� 100�� = ���� ���� 1��

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc; // �Ȱ��е� ������ ����

    [SerializeField] private float nightFogDensity;
    private float dayFogDensity;
    private float currentFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        // 2�� ° overload �Լ� (ȸ���� ����, ȸ����)
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            isNight = true;
        else if (transform.eulerAngles.x <= 340)
            isNight = false;

        if (isNight)
        {
            if (currentFogDensity > nightFogDensity)
                return;
            currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }
        else
        {
            if (currentFogDensity < dayFogDensity)
                return;
            currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }

    }
}
