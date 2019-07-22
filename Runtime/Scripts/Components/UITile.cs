using UnityEngine;
using UnityEditor;
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
            if(icon) icon.sprite = item.tileIcon;
            if(title) title.text = item.tileTitle;
            if(text) text.text = item.tileText;
            if(coloredImage) coloredImage.color = item.tileColor;

            if (addOnClickEvent)
            {
                button.onClick.RemoveListener(item.OnTileClick);
                button.onClick.AddListener(item.OnTileClick);
            }
        }
        #endregion

        #region Editor Methods
        [MenuItem("GameObject/UI/DGTools/UITile", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            UITile tile = Instantiate(Resources.Load<UITile>("Prefabs/UITile"));
            tile.name = tile.name.Replace("(Clone)", "");
            GameObjectUtility.SetParentAndAlign(tile.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(tile.gameObject, "Create " + tile.name);
            Selection.activeObject = tile;
        }
        #endregion
    }
}
