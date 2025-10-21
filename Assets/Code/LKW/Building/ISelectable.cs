namespace Code.LKW.Building
{
    public interface ISelectable
    {
        bool IsSelected { get; }
        void Select();
        void DeSelect();
    }
}