using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DGTools.UI
{
    public abstract class UIPalet : UIComponent, IFillable<IUITilable>
    {
        #region Public Variables
        [Tooltip("Insert your tile model")]
        [SerializeField] protected UISelectableTile tileModel;
        [SerializeField] [TypeConstraint(typeof(IUITilable))] List<GameObject> items;
        #endregion

        #region Properties
        protected UISelectableTile[] tiles => GetComponentsInChildren<UISelectableTile>();
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
            foreach (GameObject obj in items)
                tmpT.Add(obj.GetComponent<IUITilable>());

            AddItems(tmpT.ToArray());
        }

        protected virtual UISelectableTile InstantiateTile(IUITilable item)
        {
            UISelectableTile tile = Instantiate(tileModel, rectTransform);
            tile.SetItem(item);
            tile.button.onClick.AddListener(delegate { OnTileSelect(tile); });
            return tile;
        }
        #endregion

        #region Abstract Methods
        protected abstract void OnTileSelect(UISelectableTile tile);
        #endregion
    }
}
