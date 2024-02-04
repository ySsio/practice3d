using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 컴포넌트로 추가할 수 없는 클래스. MonoBehaviour를 상속받아야 컴포넌트로 추가 가능
[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름
    public AudioClip clip; // 곡
}


public class SoundManager : MonoBehaviour
{
    // 싱글턴 Singleton 화 시켜야 한다. 하나로
    // 다른 scene으로 이동해도 sound manager을 파괴하지 않고 유지
    // 다른 scene으로 이동해도 sound manager을 새로 생성하자 마자 파괴해서 1개로 유지

    static public SoundManager instance; // 자기 자신을 instance로 만듦

    // void Awake() : 객체 생성시 최초 1회 실행
    // void OnEnable() : 활성화될 때마다 실행. 껐다 켤때마다 실행됨. 코루틴 실행 불가능
    // void Start() : 활성화될 때마다 실행. 껐다 켤때마다 실행됨. 코루틴 실행 가능

    #region singleton
    void Awake()  // 기본적인 싱글톤화
    {
        // 최초로 씬을 실행할 때, 생성된 SoundManager을 instance로 지정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 새로운 씬을 불러도 파괴하지 말 것
        }
        // 다른 씬 갔다가 다시 이 씬을 실행할 때, 이미 instance가 지정되어 있으므로 새로 생성된 이 SoundManager을 파괴해
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    // AudioSource가 음악플레이어고 AudioClip이 음악임
    public AudioSource[] audioSourceEffects; // 효과음은 여러 개가 중복해서 들릴 수 있으므로 여러 개의 플레이어를 배열로 관리한다.
    public AudioSource audioSourceBgm; // 배경음은 1개만 유지하므로 배열 x

    public string[] playSoundName;

    public Sound[] effectSounds; // 모든 효과음 종류
    public Sound[] bgmSounds; // 모든 배경음 종류

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].name == _name) // _name이라는 이름을 가진 곡 중에
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying) // 여러 개의 오디오소스에서 현재 재생중이지 않은 녀석만 찾음
                    {
                        playSoundName[j] = effectSounds[j].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
                
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인 " + _name + " 사운드가 없습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
