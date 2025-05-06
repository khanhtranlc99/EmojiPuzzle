using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System;
using DG.Tweening;
using Newtonsoft.Json;
public enum Difficult
{
    Normal,
    Hard,
    VeryHard,
    Boss

}

public class LevelData : SerializedMonoBehaviour
{
    public string data;
    public LevelConfig levelConfig;
    private const float BLOCK_SPACING = 1.25f;
    public EmojiBase block_Normal;
    public List<EmojiBase> lsEmoji;
    public int idEmoji;
    public Transform parentEmoji;
    public Transform leftPost;
    public Transform rightPost;


    public void Init()
    {
        foreach(var item in lsEmoji)
        {
            item.SetFirstPost();
        }
    }

    [Button]  
    
    public void SetUp()
    {
        Debug.Log("SetUp");
        //var pathLevel = $"Levels/Level_{UseProfile.CurrentLevel}";
        //TextAsset lvJson = Resources.Load<TextAsset>(pathLevel);
        levelConfig = JsonConvert.DeserializeObject<LevelConfig>(data);
        HandleData(levelConfig.dataBlock);
        foreach(var item in lsEmoji)
        {
            item.HandleRemove(); 
        }


    }
    public void HandleData(Data[,] datas)
    {
        if(lsEmoji.Count > 0)
        {
            foreach(var item in lsEmoji)
            {
                DestroyImmediate(item.gameObject);
            }
            lsEmoji.Clear();
        }
        int rows = datas.GetLength(0);
        int cols = datas.GetLength(1);

        // Tính toán vị trí để căn giữa màn hình
        float gridWidth = (cols - 1) * BLOCK_SPACING;
        float gridHeight = (rows - 1) * BLOCK_SPACING;

        // Tính offset để căn giữa
        float offsetX = -gridWidth * 0.5f;
        float offsetY = -gridHeight * 0.5f + 4f; // Dịch lên trên một chút

        // Đặt parent object ở giữa màn hình
        transform.position = new Vector3(0, 0, 0);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

                if (datas[i, j].valueNumber != 0)
                {
                    var temp = SimplePool2.Spawn(block_Normal);
                    temp.transform.SetParent(parentEmoji);
                    temp.transform.localPosition = new Vector3(
                    j * BLOCK_SPACING + offsetX,
                    -i * BLOCK_SPACING + offsetY,
                   0
                   );
                    temp.gameObject.name = $"Block_{datas[i, j]}";
                     temp.Init(idEmoji, datas[i, j], i, j);
                    // temp.transform.localScale = Vector3.zero;
                    lsEmoji.Add(temp);
                }


            }
        }

        // Sort listBlock theo valueNumber tăng dần
        //      listBlock = listBlock.OrderBy(x => x.valueNumber).ToList();

        // Cập nhật thứ tự trong hierarchy
        for (int i = 0; i < lsEmoji.Count; i++)
        {
            lsEmoji[i].transform.SetSiblingIndex(i);
        }

    }
}




[System.Serializable]
public class Data
{
    public int valueNumber;
    public int isActive;


    public override string ToString()
    {
        return valueNumber.ToString();
    }
}

[System.Serializable]
public class LevelConfig
{
    public int id;
    public int numbMove;
    public Data[,] dataBlock;

}

[System.Serializable]
public class DataRandomRowCol
{
    public int row;
    public int col;
    public int minTime;
    public int maxTime;
}

