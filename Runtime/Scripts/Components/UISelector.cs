using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DGTools.UI
{
    public abstract class UISelector : UIComponent, IFillable<IUITilable>
    {
        #region Public Variables
        [Tooltip("Relations")]
        [SerializeField] protected Transform tilesParent;
        [SerializeField] protected UISelectableTile tileModel;
        [Tooltip("If true, it will call the OnTileClick() method of the IUITilable when tile selected")]
        [SerializeField] protected bool useOnTileClick = false;
        [Tooltip("Items that will be instantiated by default")]
        [SerializeField] protected List<Object> items;
        #endregion

        #region Properties
        protected UISelectableTile[] tiles => tilesParent.GetComponentsInChildren<UISelectableTile>();
        #endregion

        #region Publics Methods
        /// <summary>
        /// Instantiate an <see cref="IUITilable"/>
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IUITilable item)
        {
            InstantiateTile(item);
        }

        /// <summary>
        /// Instanciate many <see cref="IUITilable"/>
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(params IUITilable[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                InstantiateTile(items[i]);
            }
        }

        /// <summary>
        /// Remove a <see cref="UISelectableTile"/> from a <see cref="IUITilable"/>
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(IUITilable item)
        {
            UISelectableTile toRemove = tiles.Where(t => t.item == item).FirstOrDefault();
            if (toRemove)
                Destroy(toRemove.gameObject);
        }

        /// <summary>
        /// Remove many <see cref="UISelectableTile"/> from a list of <see cref="IUITilable"/>
        /// </summary>
        /// <param name="items"></param>
        public void RemoveItems(params IUITilable[] items)
        {
            foreach (UISelectableTile tile in tiles.Where(t => items.Contains(t.item)))
                Destroy(tile.gameObject);
        }

        /// <summary>
        /// Returns all <see cref="IUITilable"/> items in the grid
        /// </summary>
        public List<IUITilable> GetItems()
        {
            List<IUITilable> items = new List<IUITilable>();
            foreach (UISelectableTile tile in tiles)
            {
                items.Add(tile.item);
            }
            return items;
        }

        public override void Clear()
        {
            foreach (UISelectableTile tile in tiles)
            {
                Destroy(tile.gameObject);
            }
        }
        #endregion

        #region Privates Methods
        protected override void Build()
        {
            List<IUITilable> tmpT = new List<IUITilable>();
            foreach (Object obj in items)
            {
                IUITilable item;
                if (obj is GameObject)
                    item = (obj as GameObject).GetComponent<IUITilable>();
                else
                    item = obj as IUITilable;
                tmpT.Add(item);
            }
                

            AddItems(tmpT.ToArray());
        }

        protected virtual UISelectableTile InstantiateTile(IUITilable item)
        {
            UISelectableTile tile = Instantiate(tileModel, tilesParent);
            tile.SetItem(item, useOnTileClick);
            tile.button.onClick.AddListener(delegate { OnTileSelect(tile); });
            return tile;
        }

        protected virtual bool IsValidItem(Object obj) {
            GameObject go = obj as GameObject;
            if (go != null)
                return go.GetComponent<IUITilable>() != null;
            else
                return obj is IUITilable;
        }
        #endregion

        #region Abstract Methods
        protected abstract void OnTileSelect(UISelectableTile tile);
        #endregion

        #region Editor Methods
        private void OnValidate()
        {
            if (items != null)
            {
                foreach (Object obj in items)
                {
                    if (!IsValidItem(obj))
                        Debug.Log(string.Format("{0} should have a component that implements IUITilable, {1} will ignore it", obj.name, name));
                }
            }
        }
        #endregion
    }
}
