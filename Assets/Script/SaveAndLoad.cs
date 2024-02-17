using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;   // 플레이어 위치 저장함
    public Vector3 playerRot;   // 플레이어 방향 저장함 (y)
    public Vector3 camearaRot;   // 카메라 방향 저장함 (x)

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}


public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";
    

    private PlayerController thePlayer;
    private Inventory theInventory;

    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        // System.IO 라이브러리
        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 이 디렉토리가 존재하지 않으면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 생성해줌
    }

    public void SaveData()
    {
        // json 이용해서 저장
        thePlayer = FindObjectOfType<PlayerController>();
        theInventory = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;
        saveData.camearaRot = thePlayer.theCamera.transform.eulerAngles;

        Slot[] slots = theInventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                continue;

            saveData.invenArrayNumber.Add(i);
            saveData.invenItemName.Add(slots[i].item.itemName);
            saveData.invenItemNumber.Add(slots[i].itemCount);
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);   // 지정한 경로에 json 이라는 변수의 내용을 txt로 저장

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        // 세이브 파일이 없으면 실행 안함.
        if (!File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            Debug.Log("세이브 파일 없음");
            return;
        }
            

        string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        thePlayer = FindObjectOfType<PlayerController>();
        theInventory = FindObjectOfType<Inventory>();

        thePlayer.transform.position = saveData.playerPos;
        thePlayer.transform.eulerAngles = saveData.playerRot;
        thePlayer.theCamera.transform.eulerAngles = saveData.camearaRot;
        Debug.Log(saveData.invenArrayNumber.Count);

        for (int i = 0; i < saveData.invenArrayNumber.Count ; i++)
        {
            Debug.Log(i);
            theInventory.LoadToInven(saveData.invenArrayNumber[i],
                                     saveData.invenItemName[i],
                                     saveData.invenItemNumber[i]);
        }
        
        Debug.Log("로드 완료");
    }
}
