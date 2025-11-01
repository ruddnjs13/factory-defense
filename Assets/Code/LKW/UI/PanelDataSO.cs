using UnityEngine;

namespace Code.LKW.UI
{
    [CreateAssetMenu(fileName = "Panel data", menuName = "SO/UI/Panel data", order = 10)]
    public class PanelDataSO : ScriptableObject
    {
        [field:SerializeField] public string PanelName {get; private set;} = "Panel";
    }
}