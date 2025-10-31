using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.EJY.UI
{
    public class EnemyIcon : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        
        public void SetIcon(Sprite icon) => iconImage.sprite = icon;
    }
}