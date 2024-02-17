using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;   // �÷��̾� ��ġ ������
    public Vector3 playerRot;   // �÷��̾� ���� ������ (y)
    public Vector3 camearaRot;   // ī�޶� ���� ������ (x)

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

        // System.IO ���̺귯��
        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // �� ���丮�� �������� ������
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // ��������
    }

    public void SaveData()
    {
        // json �̿��ؼ� ����
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

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);   // ������ ��ο� json �̶�� ������ ������ txt�� ����

        Debug.Log("���� �Ϸ�");
        Debug.Log(json);
    }

    public void LoadData()
    {
        // ���̺� ������ ������ ���� ����.
        if (!File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            Debug.Log("���̺� ���� ����");
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
        
        Debug.Log("�ε� �Ϸ�");
    }
}
