#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
public enum BlockType
{
Active,
Horizontal,
Vertical,

Random
}
public class EditLevel : SerializedMonoBehaviour
{
    public int id;
    
    [SerializeField, OnValueChanged("OnSizeChanged")]
    private int row = 5; // Mặc định 5 hàng
    
    [SerializeField, OnValueChanged("OnSizeChanged")] 
    private int col = 5; // Mặc định 5 cột
     [SerializeField] 
    private int numMove ; // Mặc định 5 cột

    [SerializeField] private List<BlockType> listSpecialBlock;
    
    [TableMatrix(HorizontalTitle = "Data Block", SquareCells = true, DrawElementMethod = "DrawDataElement")]
    public Data[,] dataBlock;
   
    public List<DataRandomRowCol> lsDataRandomRowCols;

    public GameObject block_Normal;
    public List<GameObject> listBlock;
    private void OnEnable()
    {
        // Khởi tạo mảng với giá trị mặc định
        if(dataBlock == null)
        {
            dataBlock = new Data[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    dataBlock[i, j] = new Data { valueNumber = 0, isActive = 0 };
                }
            }
        }
    }

    // Phương thức vẽ cho mỗi ô trong TableMatrix
    private static Data DrawDataElement(Rect rect, Data value)
    {
        if (value == null)
            value = new Data { valueNumber = 0, isActive = 0 };

        // Vẽ giá trị và checkbox
        value.valueNumber = EditorGUI.IntField(
            new Rect(rect.x, rect.y, rect.width, rect.height * 0.7f), 
            value.valueNumber
        );
        
        value.isActive = EditorGUI.IntField(
            new Rect(rect.x, rect.y + rect.height * 0.7f, rect.width, rect.height * 0.3f),
            value.isActive
        );

        return value;
    }

    [Button("Random Array")]
    private void RandomArray()
    {
        // Reset mảng về 0
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                dataBlock[i,j] = new Data { valueNumber = 0, isActive = 0 };
            }

        // Chọn vị trí bắt đầu ngẫu nhiên cho số 1
        int currentX = Random.Range(0, row);
        int currentY = Random.Range(0, col);
        dataBlock[currentX, currentY] = new Data { valueNumber = 1, isActive = 0 };

        int totalElements = row * col;
        int currentNumber = 2;

        while (currentNumber <= totalElements)
        {
            List<Vector2Int> possibleMoves = new List<Vector2Int>();
            CheckPossibleMove(currentX + 1, currentY, possibleMoves);
            CheckPossibleMove(currentX - 1, currentY, possibleMoves);
            CheckPossibleMove(currentX, currentY + 1, possibleMoves);
            CheckPossibleMove(currentX, currentY - 1, possibleMoves);

            if (possibleMoves.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleMoves.Count);
                Vector2Int nextMove = possibleMoves[randomIndex];
                dataBlock[nextMove.x, nextMove.y] = new Data 
                { 
                    valueNumber = currentNumber,
                    isActive = 0 
                };
                currentX = nextMove.x;
                currentY = nextMove.y;
                currentNumber++;
            }
            else
            {
                bool found = false;
                for (int i = 0; i < row && !found; i++)
                {
                    for (int j = 0; j < col && !found; j++)
                    {
                        if (dataBlock[i,j].valueNumber == 0)
                        {
                            if (HasAdjacentNumber(i, j, currentNumber - 1))
                            {
                                dataBlock[i,j] = new Data 
                                { 
                                    valueNumber = currentNumber,
                                    isActive = 0 
                                };
                                currentX = i;
                                currentY = j;
                                currentNumber++;
                                found = true;
                            }
                        }
                    }
                }
                if (!found) break;
            }
        }

        // Xử lý special blocks sau khi đã tạo xong mảng số
        if(listSpecialBlock != null && listSpecialBlock.Count > 0)
        {
            // Đếm số lượng mỗi loại block
            Dictionary<BlockType, int> blockTypeCounts = new Dictionary<BlockType, int>();
            foreach(BlockType type in listSpecialBlock)
            {
                if(blockTypeCounts.ContainsKey(type))
                    blockTypeCounts[type]++;
                else
                    blockTypeCounts[type] = 1;
            }

            List<Vector2Int> availablePositions = new List<Vector2Int>();
            
            // Tạo danh sách các vị trí có thể đặt special block
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    availablePositions.Add(new Vector2Int(i, j));
                }
            }

            // Xử lý từng loại block theo số lượng
            foreach(var blockCount in blockTypeCounts)
            {
                BlockType specialType = blockCount.Key;
                int count = blockCount.Value;

                // Xử lý số lượng block cho mỗi loại
                for(int i = 0; i < count; i++)
                {
                    if(availablePositions.Count > 0)
                    {
                        int randomIndex = Random.Range(0, availablePositions.Count);
                        Vector2Int pos = availablePositions[randomIndex];

                        switch(specialType)
                        {
                            case BlockType.Active:
                                dataBlock[pos.x, pos.y].isActive = 1;
                                break;
                            case BlockType.Horizontal:
                                dataBlock[pos.x, pos.y].isActive = 2;
                                break;
                            case BlockType.Vertical:
                                dataBlock[pos.x, pos.y].isActive = 3;
                                break;
                            case BlockType.Random:
                                dataBlock[pos.x, pos.y].isActive = 4;
                                break;
                        }

                        availablePositions.RemoveAt(randomIndex);
                    }
                }
            }
        }
        HandleData(dataBlock);
    }

    private void CheckPossibleMove(int x, int y, List<Vector2Int> moves)
    {
        if (IsValidPosition(x, y) && dataBlock[x,y].valueNumber == 0)
        {
            moves.Add(new Vector2Int(x, y));
        }
    }

    private bool HasAdjacentNumber(int x, int y, int number)
    {
        if (IsValidPosition(x + 1, y) && dataBlock[x + 1, y].valueNumber == number) return true;
        if (IsValidPosition(x - 1, y) && dataBlock[x - 1, y].valueNumber == number) return true;
        if (IsValidPosition(x, y + 1) && dataBlock[x, y + 1].valueNumber == number) return true;
        if (IsValidPosition(x, y - 1) && dataBlock[x, y - 1].valueNumber == number) return true;
        return false;
    }

    private bool IsValidPosition(int r, int c)
    {
        return r >= 0 && r < row && c >= 0 && c < col;
    }

    private void OnSizeChanged()
    {
        Data[,] newArray = new Data[row, col];
        if (dataBlock != null)
        {
            int minRow = Mathf.Min(row, dataBlock.GetLength(0));
            int minCol = Mathf.Min(col, dataBlock.GetLength(1));
            
            for (int i = 0; i < minRow; i++)
            {
                for (int j = 0; j < minCol; j++)
                {
                    newArray[i,j] = dataBlock[i,j];
                }
            }
        }
        dataBlock = newArray;
    }

    // Properties để truy cập từ code
    public int Row
    {
        get { return row; }
        set 
        { 
            row = value;
            OnSizeChanged();
        }
    }

    public int Col
    {
        get { return col; }
        set
        {
            col = value;
            OnSizeChanged();
        }
    }

    [Button("Save Level")]
    private void SaveLevel()
    {
        // Tạo đối tượng LevelConfig để serialize
        LevelConfig levelConfig = new LevelConfig
        {
            id = this.id,
            numbMove = this.numMove,
            dataBlock = dataBlock // Không cần copy mảng vì Newtonsoft.Json có thể serialize mảng 2 chiều
        };

        // Convert sang JSON sử dụng Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(levelConfig, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented, // Format JSON đẹp
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        // Tạo đường dẫn tới thư mục Resources/Levels
        string directoryPath = Path.Combine(Application.dataPath, "Resources/Levels");
        
        // Tạo thư mục nếu chưa tồn tại
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Tạo tên file
        string fileName = $"Level_{id}.txt";
        string fullPath = Path.Combine(directoryPath, fileName);

        // Lưu file
        File.WriteAllText(fullPath, jsonData);
        
        Debug.Log($"Đã lưu level tại: {fullPath}");

#if UNITY_EDITOR
        // Refresh để Unity nhận biết file mới trong Editor
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    [Button("Spam Level")]
    private void SpamLevel()
    {
        for (int levelId = 26; levelId <= 500; levelId++)
        {
            // Random kích thước mảng từ lsDataRandomRowCols
            var temp = Random.Range(0, lsDataRandomRowCols.Count);
            col = lsDataRandomRowCols[temp].col;
            row = lsDataRandomRowCols[temp].row;
            OnSizeChanged();

            // Tạo danh sách BlockType mới (trừ Normal)
            listSpecialBlock = new List<BlockType>();
            
            // Random số lượng special blocks (1-8)
            int specialBlockCount = Random.Range(1, 9);
            
            // Tạo mảng các BlockType có thể (trừ Normal)
            BlockType[] availableTypes = new BlockType[]
            {
                BlockType.Active,
                BlockType.Horizontal,
                BlockType.Vertical,
                BlockType.Random
            };

            // Thêm ngẫu nhiên các BlockType vào listSpecialBlock
            for (int i = 0; i < specialBlockCount; i++)
            {
                // Chọn ngẫu nhiên một loại block từ các loại có sẵn
                BlockType randomType = availableTypes[Random.Range(0, availableTypes.Length)];
                
                // Thêm vào list
                listSpecialBlock.Add(randomType);
            }

            // Tạo mảng số ngẫu nhiên (sẽ tự xử lý các special blocks dựa trên listSpecialBlock)
            RandomArray();

            // Lưu level
            id = levelId;
            numMove = Random.Range(lsDataRandomRowCols[temp].minTime,lsDataRandomRowCols[temp].maxTime ); // Đảm bảo numMove được cập nhật
            SaveLevel();
        }

        Debug.Log("Đã tạo xong 500lv");
    }
    public TextAsset textAsset;
    [Button]
    public void Test()
    {
       var  levelConfig = JsonConvert.DeserializeObject<LevelConfig>(textAsset.text);

            int rows = levelConfig.dataBlock.GetLength(0);
            int cols = levelConfig.dataBlock.GetLength(1);
            dataBlock = levelConfig.dataBlock;
            

    }
    private const float BLOCK_SPACING = 1.25f; 
    public void HandleData(Data[,] datas)
    {
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

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    
                 if(datas[i,j].valueNumber != 0)
                 {
                           var temp = SimplePool2.Spawn(block_Normal) ;
                              temp.transform.SetParent(transform);
                              temp.transform.localPosition = new Vector3(
                              j * BLOCK_SPACING + offsetX,
                              -i * BLOCK_SPACING + offsetY,
                             0
                             );
                             temp.gameObject.name = $"Block_{datas[i, j]}";
                            // temp.Init(datas[i, j], i, j);
                            // temp.transform.localScale = Vector3.zero;
                             listBlock.Add(temp); 
                 }
            
                  
                }
            }
            
            // Sort listBlock theo valueNumber tăng dần
      //      listBlock = listBlock.OrderBy(x => x.valueNumber).ToList();

            // Cập nhật thứ tự trong hierarchy
            for(int i = 0; i < listBlock.Count; i++)
            {
                listBlock[i].transform.SetSiblingIndex(i);
            }
        
    }
    
}       
#endif
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