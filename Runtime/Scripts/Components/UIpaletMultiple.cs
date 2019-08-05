using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace DGTools.UI {
    public class UIPaletMultiple : UIPalet
    {
        #region Events
        [Serializable] public class SelectedTilesEvent : UnityEvent<List<UISelectableTile>> { }
        [Header("Events")]
        [SerializeField] public SelectedTilesEvent onTileSelect = new SelectedTilesEvent();
        #endregion

        #region Properties
        public List<UISelectableTile> selectedTiles { get; protected set; } = new List<UISelectableTile>();
        #endregion

        #region Event Methods
        protected override void OnTileSelect(UISelectableTile tile)
        {
            if (tile.isSelected)
            {
                selectedTiles.Remove(tile);
                tile.isSelected = false;
            }
            else if (!tile.isSelected)
            {
                selectedTiles.Add(tile);
                tile.isSelected = true;

                onTileSelect.Invoke(selectedTiles);
            }
        }
        #endregion

        #region Public Methods
        public override void Clear()
        {
            base.Clear();
            selectedTiles = new List<UISelectableTile>();
        }
        #endregion

        #region Private Methods
        protected override void Build()
        {
            base.Build();
            onTileSelect = new SelectedTilesEvent();
        }
        #endregion
    }
}

