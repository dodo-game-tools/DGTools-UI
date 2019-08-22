using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class UIPanel : UIComponent, IUITilable
    {
        #region Properties
        public virtual Image background => GetComponent<Image>();

        public virtual Mask mask => GetComponent<Mask>();

        public virtual Color tileColor => background.color;

        public virtual Sprite tileIcon => null;

        public virtual string tileTitle => name;

        public virtual string tileText => null;
        #endregion

        #region Public Methods
        public override void Clear() { }

        public virtual void OnTileClick() { }
        #endregion

        #region Private Methods
        protected override void Build() { }
        #endregion
    }
}


 