using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToolSpawnLevel : MonoBehaviour
{
    public List<TextAsset> lsTextData;
    public LevelData levelData;

    [Button]
    public void HandleSpawnAll()
    {
        for(int i = 0;  i < lsTextData.Count; i ++)
        {
            var temp = Instantiate(levelData);
            temp.name = "Level_" + (i +1);
            temp.data = lsTextData[i].text;
            temp.idEmoji = i;
            temp.SetUp();
        }

    }    
}
