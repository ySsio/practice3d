using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; // 게임 세계 100초 = 현실 세계 1초


    [SerializeField] private float fogDensityCalc; // 안개밀도 증감량 비율

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
        // 2번 째 overload 함수 (회전축 벡터, 회전각)
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            GameManager.isNight = true;
        else if (transform.eulerAngles.x <= 340)
            GameManager.isNight = false;

        if (GameManager.isNight)
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
