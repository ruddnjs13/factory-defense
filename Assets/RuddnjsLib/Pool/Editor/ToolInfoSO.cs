using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pool/ToolInfo")]
public class ToolInfoSO : ScriptableObject
{
    [Header("PoolManager")] 
    public string poolingFolder = "Assets/08.SO/01.Pool";
    public string poolAssetName = "PoolManager.asset";
    public string typeFolder = "Types";
    public string itemFolder = "Items";
}