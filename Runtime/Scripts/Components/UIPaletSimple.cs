using UnityEngine;
using UnityEngine.Events;
using System;

namespace DGTools.UI
{
    public class UIPaletSimple : UIPalet
    {
        #region Public Variables
        [Tooltip("Check if you can deselect tiles on this palet")]
        [SerializeField] protected bool canDeselectedSelectedTile = true;
        #endregion

        #region Events
        [Serializable] public class SelectedTileEvent : UnityEvent<UISelectableTile> { }
        [Header("Events")]
        [SerializeField] public SelectedTileEvent onTileSelect = new SelectedTileEvent();
        #endregion

        #region Properties
        public UISelectableTile selectedTile { get; protected set; }
        #endregion

        #region Public Methods
        public override void Clear()
        {
            selectedTile = null;
            DeselectOtherTiles();
            base.Clear();
        }
        #endregion

        #region Private Methods
        void DeselectOtherTiles()
        {
            foreach (UISelectableTile tile in tiles)
            {
                if (tile != selectedTile)
                {
                    tile.isSelected = false;
                }
            }
        }
        #endregion

        #region Event Methods
        protected override void OnTileSelect(UISelectableTile tile)
        {
            if (tile != selectedTile)
            {
                selectedTile = tile;
                selectedTile.isSelected = true;

                DeselectOtherTiles();

                onTileSelect.Invoke(tile);
            }
            else if (tile == selectedTile && canDeselectedSelectedTile)
            {
                selectedTile.isSelected = false;
                selectedTile = null;
            }
        }
        #endregion
    }
}
