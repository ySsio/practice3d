using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; // 물 속 중력
    private float originDrag; // 물 속 빠져나오면 원래대로 돌아옴

    [SerializeField] private Color waterColor;      // 물 속 화면색
    [SerializeField] private float waterFogDensity; // 물 속 fogdensity

    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterBreath;

    [SerializeField] private float breathTime;
    private float currentBreathTime;

    private Color originColor;
    private float originFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0; // Player rigidbody의 drag 값 그냥 하드코딩으로 가져 옴
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

        GameManager.isWater = false;
        _Player.transform.GetComponent<Rigidbody>().drag = originDrag;
        SoundManager.instance.PlaySE(sound_WaterOut);

        RenderSettings.fogColor = originColor;
        RenderSettings.fogDensity = originFogDensity;
    }
}
