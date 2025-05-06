using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PlayerContain : MonoBehaviour
{
    public LevelData levelData;
    public EmojiController emojiController;
    public CameraScale cameraScale;
 
    public void Init()
    {
        string pathLevel = StringHelper.PATH_CONFIG_LEVEL_TEST;
        levelData = Instantiate(Resources.Load<LevelData>(string.Format(pathLevel, UseProfile.CurrentLevel)));
        levelData.Init();
        emojiController.Init(levelData);
        cameraScale.Init(levelData.leftPost.position, levelData.rightPost.position);

    }

   


}
