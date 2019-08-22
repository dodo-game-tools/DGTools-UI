using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI {
    [RequireComponent(typeof(Button))]
    public class UITile : UIComponent
    {
        #region Variables
        [Header("Relations")]
        [SerializeField] Image icon;
        [SerializeField] Text title;
        [SerializeField] Text text;
        [SerializeField] Image coloredImage;
        #endregion

        #region Properties
        public IUITilable item { get; private set; }

        public virtual Button button { get { return GetComponent<Button>(); } }
        #endregion

        #region Public Methods
        public void SetItem(IUITilable item, bool addOnClickEvent = false) {
            this.item = item;
            RefreshTile(addOnClickEvent);
        }

        public virtual void RefreshTile(bool addOnClickEvent = false) {
            if (icon)
            {                
                icon.sprite = item != null ? item.tileIcon : null;
                icon.gameObject.SetActive(icon.sprite != null);
            }
            if(title) title.text = item != null ? item.tileTitle : null;
            if(text) text.text = item != null ? item.tileText : null;
            if(coloredImage) coloredImage.color = item != null ? item.tileColor : Color.white;

            if (addOnClickEvent && item != null)
            {
                button.onClick.RemoveListener(item.OnTileClick);
                button.onClick.AddListener(item.OnTileClick);
            }
        }

        public override void Clear()
        {
            SetItem(null);
        }
        #endregion

        #region Private Methods
        protected override void Build()
        {
        }
        #endregion
    }
}
