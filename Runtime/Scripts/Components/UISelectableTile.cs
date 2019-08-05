using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UISelectableTile : UITile, ISelectable
    {
        #region Public Variables
        [Header("Colors of the tile")]
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;
        #endregion

        #region Private Variables
        bool _isSelected;
        #endregion

        #region Properties
        /// <summary>
        /// True if tile is selected
        /// </summary>
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    OnSelect();
                }
                else
                {
                    OnDeselect();
                }
            }
        }

        public Image image => GetComponent<Image>();
        #endregion

        #region Private Methods
        protected virtual void OnSelect()
        {
            image.color = selectedColor;
        }

        protected virtual void OnDeselect()
        {
            image.color = deselectedColor;
        }
        #endregion

    }
}