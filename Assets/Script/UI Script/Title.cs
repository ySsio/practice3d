using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Game";

    private SaveAndLoad theSaveAndLoad;

    // �̱���
    public static Title instance;

    private void Awake()
    {   
        // �̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        SceneManager.LoadScene(sceneName);

        // �� ����Ÿ��Ʋ ĵ������ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine());
        theSaveAndLoad.LoadData();
    }

    IEnumerator LoadCoroutine()
    {
        // ���� ������ �̵��ϴµ� ������ ����ȭ �ϱ���� �ε� �ϱ� ���� ���
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            // operation.progress; �ε� �����Ȳ�� ��ġ�� ��Ÿ��. 0.9�� �ִ��ε�.?
            yield return null;
        }

        // ���� �� ������ �ε� �� LoadData() ����
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        theSaveAndLoad.LoadData();

        // �� ����Ÿ��Ʋ ĵ������ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }

}
