using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ������Ʈ�� �߰��� �� ���� Ŭ����. MonoBehaviour�� ��ӹ޾ƾ� ������Ʈ�� �߰� ����
[System.Serializable]
public class Sound
{
    public string name; // ���� �̸�
    public AudioClip clip; // ��
}


public class SoundManager : MonoBehaviour
{
    // �̱��� Singleton ȭ ���Ѿ� �Ѵ�. �ϳ���
    // �ٸ� scene���� �̵��ص� sound manager�� �ı����� �ʰ� ����
    // �ٸ� scene���� �̵��ص� sound manager�� ���� �������� ���� �ı��ؼ� 1���� ����

    static public SoundManager instance; // �ڱ� �ڽ��� instance�� ����

    // void Awake() : ��ü ������ ���� 1ȸ ����
    // void OnEnable() : Ȱ��ȭ�� ������ ����. ���� �Ӷ����� �����. �ڷ�ƾ ���� �Ұ���
    // void Start() : Ȱ��ȭ�� ������ ����. ���� �Ӷ����� �����. �ڷ�ƾ ���� ����

    #region singleton
    void Awake()  // �⺻���� �̱���ȭ
    {
        // ���ʷ� ���� ������ ��, ������ SoundManager�� instance�� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���ο� ���� �ҷ��� �ı����� �� ��
        }
        // �ٸ� �� ���ٰ� �ٽ� �� ���� ������ ��, �̹� instance�� �����Ǿ� �����Ƿ� ���� ������ �� SoundManager�� �ı���
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    // AudioSource�� �����÷��̾�� AudioClip�� ������
    public AudioSource[] audioSourceEffects; // ȿ������ ���� ���� �ߺ��ؼ� �鸱 �� �����Ƿ� ���� ���� �÷��̾ �迭�� �����Ѵ�.
    public AudioSource audioSourceBgm; // ������� 1���� �����ϹǷ� �迭 x

    public string[] playSoundName;

    public Sound[] effectSounds; // ��� ȿ���� ����
    public Sound[] bgmSounds; // ��� ����� ����

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].name == _name) // _name�̶�� �̸��� ���� �� �߿�
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying) // ���� ���� ������ҽ����� ���� ��������� ���� �༮�� ã��
                    {
                        playSoundName[j] = effectSounds[j].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� AudioSource�� ������Դϴ�.");
                return;
            }
                
        }
        Debug.Log(_name + " ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
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
        Debug.Log("��� ���� " + _name + " ���尡 �����ϴ�.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
