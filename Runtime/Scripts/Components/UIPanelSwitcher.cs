using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI
{
    public class UIPanelSwitcher : UISelectorSimple
    {
        #region Public Variables
        [Header("Relations")]
        [Tooltip("Define the container where instanciate palets")]
        [SerializeField] protected RectTransform panelsContainer;
        [Tooltip("Define the text where appear the palet's name")]
        [SerializeField] protected Text titleText;
        [Tooltip("Check if you want to open the default palet at the lauch")]
        [SerializeField] bool openDefaultPalet = true;
        [Tooltip("Define a palet to open a the lauch of the menu header")]
        [SerializeField] int defaultPaletIndex;
        #endregion

        #region Properties
        public UIPanel[] panels => panelsContainer.GetComponentsInChildren<UIPanel>(true);
        #endregion

        #region Private Methods
        protected override void Build()
        {
            base.Build();
            if (openDefaultPalet && panels.Length > 0) OpenDefaultPalet();
        }

        protected override UISelectableTile InstantiateTile(IUITilable item)
        {
            UISelectableTile tile = base.InstantiateTile(item);

            UIPanel palet = tile.item as UIPanel;
            tile.SetItem(Instantiate(palet, panelsContainer));
            onTileSelect.AddListener(SwitchPalet);
            return tile;
        }

        /// <summary>
        /// Select the target palet and deselect other palets
        /// </summary>
        /// <param name="tile"></param>
        protected virtual void SwitchPalet(UISelectableTile tile)
        {
            Deselect();
            titleText.text = tile.item.tileTitle;

            UIPanel palet = tile.item as UIPanel;
            palet.Show();
        }

        /// <summary>
        /// Allow to set a default palet to open a the start
        /// </summary>
        protected virtual void OpenDefaultPalet()
        {
            SwitchPalet(tiles[defaultPaletIndex]);
        }

        /// <summary>
        /// Deselect all palets
        /// </summary>
        protected virtual void Deselect()
        {
            UIPanel[] panels = this.panels;
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Hide();
            }
        }
        #endregion

        #region Editor Methods
        private void OnValidate()
        {
            if (defaultPaletIndex < 0) defaultPaletIndex = 0;
            if (defaultPaletIndex >= items.Count) defaultPaletIndex = items.Count - 1;
        }
        #endregion
    }
}


