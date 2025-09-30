using System;
using RuddnjsLib.Pool.Editor;
using RuddnjsPool;
using UnityEditor;
using UnityEngine.UIElements;

public class ItemInspector : IDisposable
{
    private TextField _assetNameField;
    private Button _nameChangeBtn;

    private IMGUIContainer _typeView;
    private IMGUIContainer _itemView;

    private PoolEditorWindow _editorWindow;

    private PoolingItemSO _targetItem;
    private Editor _typeEditor, _itemEditor;
    
    public delegate void NameChangeDelegate(PoolingItemSO target, string newName);
    public event NameChangeDelegate NameChangeEvent; 
    public ItemInspector(VisualElement content, PoolEditorWindow editorWindow)
    {
        _editorWindow = editorWindow;

        _assetNameField = content.Q<TextField>("AssetNameField");
        _nameChangeBtn = content.Q<Button>("BtnChange");

        _typeView = content.Q<IMGUIContainer>("TypeInspectorView");
        _itemView = content.Q<IMGUIContainer>("ItemInspectorView");

        _typeView.onGUIHandler += HandleTypeViewGUI;
        _itemView.onGUIHandler += HandleItemViewGUI;

        _nameChangeBtn.clicked += HandleNamaeChange;
    }

    private void HandleNamaeChange()
    {
            if (_targetItem == null) return;
            if (string.IsNullOrEmpty(_assetNameField.value.Trim())) return;

            string newName = _assetNameField.value;
            if (EditorUtility.DisplayDialog("Rename", $"Rename this asset to {newName}?", "Yes", "No"))
            {
                NameChangeEvent?.Invoke(_targetItem, newName);
            }
    }

    private void HandleItemViewGUI()
    {
        if (_targetItem == null) return;
        Editor.CreateCachedEditor(_targetItem, null, ref _itemEditor);
        _itemEditor.OnInspectorGUI();
    }

    private void HandleTypeViewGUI()
    {
        if (_targetItem == null) return;
        Editor.CreateCachedEditor(_targetItem.poolType, null, ref _typeEditor);
        _typeEditor.OnInspectorGUI();
    }

    public void UpdateInspector(PoolingItemSO item)
    {
        _assetNameField.SetValueWithoutNotify(item.poolType.name);
        _targetItem = item;
    }

    public void ClearInspector()
    {
        _assetNameField.SetValueWithoutNotify(string.Empty);
        _targetItem = null;
    }

    public void Dispose()
    {
        UnityEngine.Object.DestroyImmediate(_itemEditor);
        UnityEngine.Object.DestroyImmediate(_typeEditor);
    }
}