using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Game";

    private SaveAndLoad theSaveAndLoad;

    // 싱글톤
    public static Title instance;

    private void Awake()
    {   
        // 싱글톤
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

        // 이 게임타이틀 캔버스를 비활성화
        gameObject.SetActive(false);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        StartCoroutine(LoadCoroutine());
        theSaveAndLoad.LoadData();
    }

    IEnumerator LoadCoroutine()
    {
        // 다음 씬으로 이동하는데 완전히 동기화 하기까지 로드 하기 까지 대기
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            // operation.progress; 로드 진행상황을 수치로 나타냄. 0.9가 최대인듯.?
            yield return null;
        }

        // 다음 씬 완전히 로드 후 LoadData() 실행
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        theSaveAndLoad.LoadData();

        // 이 게임타이틀 캔버스를 비활성화
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

}
