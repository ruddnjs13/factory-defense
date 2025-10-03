using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
 
namespace Blade.FSM.Editor
{
    [CustomEditor(typeof(StateDataSO))]
    public class StateDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspectorUI = default;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            inspectorUI.CloneTree(root); //ui를 복제해서 root의 자식으로 넣어주라.

            DropdownField dropdown = root.Q<DropdownField>("ClassDropdownField");

            CreateDropdownChoices(dropdown);
            
            return root;
        }

        private void CreateDropdownChoices(DropdownField dropdown)
        {
            dropdown.choices.Clear();
            //EntityState 라는 클래스가 속해있는 어셈블리를 가져온다.
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(EntityState));

            List<string> typeList = fsmAssembly.GetTypes()
                .Where(type => type.IsAbstract == false && type.IsSubclassOf(typeof(EntityState)))
                .Select(type => type.FullName)
                .ToList();
            
            dropdown.choices.AddRange(typeList);
        }
    }
}