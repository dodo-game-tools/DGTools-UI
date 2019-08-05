using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DGTools.UI {
    [RequireComponent(typeof(ScrollRect))]
    public class UIGrid : UIComponent
    {
        #region Public Variables
        [Header("Relations")]
        [SerializeField] protected GridLayoutGroup gridLayout;
        [SerializeField] protected UITile tileModel;
        [SerializeField] protected Image selector;

        [Header("Settings")]
        [Tooltip("If true, clicked tile will be selected")]
        [SerializeField] protected bool selectOnClick;
        [Tooltip("Time to reach the focuses tile (in sec)")]
        [SerializeField] [Range(0.001f, 5f)] protected float searchSpeed;
        #endregion

        #region Private Variables
        protected List<UITile> tiles = new List<UITile>();
        #endregion

        #region Events
        [Serializable] public class GridSelectEvent : UnityEvent<UITile> { }
        [SerializeField] public GridSelectEvent OnGridSelect = new GridSelectEvent();
        #endregion

        #region Properties
        public UITile selected { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get Instantiated <see cref="UITile"/>
        /// </summary>
        /// <returns>A <see cref="List"/> of <see cref="UITile"/></returns>
        public virtual List<UITile> GetTiles() {
            return tiles;
        }

        /// <summary>
        /// Refresh each <see cref="UITile"/> on grid display
        /// </summary>
        public virtual void RefreshDisplay() {
            foreach (UITile tile in tiles) {
                tile.RefreshTile();
            }
        }

        /// <summary>
        /// Makes grid empty
        /// </summary>
        public override void Clear()
        {
            foreach (UITile tile in gridLayout.GetComponentsInChildren<UITile>())
            {
                Destroy(tile.gameObject);
            }

            tiles = new List<UITile>();
        }
        
        /// <summary>
        /// Add a <see cref="IUITilable"/> object to grid and instantiate a tile
        /// </summary>
        /// <param name="item">An object that implements <see cref="IUITilable"/></param>
        public void AddItem(IUITilable item)
        {
            InstantiateTile(item);
        }

        /// <summary>
        /// Add some <see cref="IUITilable"/> objects to grid and instantiate their tiles
        /// </summary>
        /// <param name="items">An array of objects that implements <see cref="IUITilable"/></param>
        public void AddItems(params IUITilable[] items)
        {
            foreach (IUITilable item in items) {
                InstantiateTile(item);
            }
        }

        /// <summary>
        /// Remove a <see cref="IUITilable"/> object from grid and destroy its tile
        /// </summary>
        /// <param name="item">An object that implements <see cref="IUITilable"/></param>
        public void RemoveItem(IUITilable item) {           
            tiles.Remove(tiles.Where(t => t.item == item).First());
        }

        /// <summary>
        /// Focus a <see cref="UITile"/> and selects its <see cref="IUITilable"/> object
        /// </summary>
        /// <param name="tile">The <see cref="UITile"/> to select</param>
        public virtual void SelectTile(UITile tile) {
            if (tiles.Contains(tile)){
                selected = tile;
                MoveSelector();
                
                StartCoroutine(GoToTile(tile));

                OnGridSelect.Invoke(tile);
            }
        }

        /// <summary>
        /// Focus a <see cref="UITile"/> and selects its <see cref="IUITilable"/> object from index
        /// </summary>
        /// <param name="index">The index of the tile</param>
        public virtual void SelectTile(int index)
        {
            SelectTile(tiles[index]);
        }

        /// <summary>
        /// Selects a <see cref="IUITilable"/> object and focus it
        /// </summary>
        /// <param name="item">An object that implements <see cref="IUITilable"/></param>
        public virtual void SelectItem(IUITilable item)
        {
            SelectTile(tiles.Where(t => t.item == item).First());
        }

        /// <summary>
        /// Unselect the selected tile
        /// </summary>
        public virtual void Unselect() {
            selector.enabled = false;
            selected = null;
        }
        #endregion

        #region Private Methods
        protected virtual UITile InstantiateTile(IUITilable item) {
            UITile tile = Instantiate(tileModel, gridLayout.transform);
            tile.SetItem(item, true);
            tile.button.onClick.AddListener(delegate { OnTileClick(tile); });
            tiles.Add(tile);
            return tile;
        }

        protected virtual void MoveSelector() {
            if (selector != null)
            {
                selector.transform.SetAsLastSibling();
                selector.enabled = true;
                selector.transform.localPosition = selected.transform.localPosition;
            }
        }

        protected virtual void CheckStructure()
        {
            if (selector != null)
            {
                if (selector.GetComponentInParent<GridLayoutGroup>() != gridLayout)
                    Debug.LogError(string.Format("<b>{0}({1}) invalid :</b> {2} should be a child of {3}", name, GetType(), selector.name, gridLayout.name));
                if (selector.GetComponent<LayoutElement>() == null)
                    Debug.LogError(string.Format("<b>{0}({1}) invalid :</b> {2} should have a LayoutElement", name, GetType(), selector.name, gridLayout.name));
                else if (!selector.GetComponent<LayoutElement>().ignoreLayout)
                    selector.GetComponent<LayoutElement>().ignoreLayout = true;
            }
        }

        protected override void Build()
        {
            CheckStructure();
        }
        #endregion

        #region Event Methods
        protected virtual void OnTileClick(UITile tile) {
            if (selectOnClick) {
                SelectTile(tile);
            }
        }
        #endregion

        #region Coroutines
        IEnumerator GoToTile(UITile tile)
        {
            float progress = 0;
            float targetY = -tile.transform.localPosition.y;
            RectTransform contentRect = gridLayout.GetComponent<RectTransform>();
            Vector2 startPos = contentRect.localPosition;

            Vector2 targetPos = new Vector2(
                gridLayout.transform.localPosition.x,
                targetY < 0 ? 0 : targetY
            );

            while (progress < 1)
            {
                progress += Time.deltaTime / searchSpeed;
                if (progress > 1) progress = 1;
                contentRect.localPosition = Vector2.Lerp(startPos, targetPos, progress);
                yield return null;
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Editor Methods
        [MenuItem("GameObject/UI/DGTools/UIGrid", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            UIGrid grid = Instantiate(Resources.Load<UIGrid>("Prefabs/UIGrid"));
            grid.name = grid.name.Replace("(Clone)", "");
            GameObjectUtility.SetParentAndAlign(grid.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(grid.gameObject, "Create " + grid.name);
            Selection.activeObject = grid;
        }
        #endregion
#endif
    }
}

