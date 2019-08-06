namespace DGTools.UI {
    public abstract class Menu : UIComponent
    {
        #region Public Methods
        /// <summary>
        /// Load the previous <see cref="Menu"/> in <see cref="MenuManager.container"/> hierarachy
        /// </summary>
        public virtual void Previous()
        {
            MenuManager.PreviousMenu();
        }

        /// <summary>
        /// Load the next <see cref="Menu"/> in <see cref="MenuManager.container"/> hierarachy
        /// </summary
        public virtual void Next()
        {
            MenuManager.NextMenu();
        }

        /// <summary>
        /// Close this menu
        /// </summary>
        public virtual void Close()
        {
            MenuManager.CloseMenu(this);
        }
        #endregion
    }

    public abstract class Menu<Tparam> : Menu
    {
        #region Variables
        /// <summary>
        /// The type of this value is inherited by the generic type class
        /// </summary>
        protected Tparam param;
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets <see cref="Menu{Tparam}.param"/>
        /// </summary>
        /// <param name="param">The type of the param</param>
        public virtual void SetParams(Tparam param)
        {
            this.param = param;
        }
        #endregion
    }
}

