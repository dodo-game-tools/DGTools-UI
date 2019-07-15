namespace DGTools.UI {
    public abstract class Menu : UIComponent
    {
        #region Properties
        /// <summary>
        /// If true, that menu has been built
        /// </summary>
        public bool built { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Displays this menu
        /// </summary>
        public override void Show(bool skipAnim = false)
        {
            Build();
            base.Show();
        }

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

        /// <summary>
        /// Reload this menus (Calls <see cref="Menu.Clear"/> then <see cref="Menu.Build"/>)
        /// </summary>
        public void Reload()
        {
            Clear();
            Build();
        }

        /// <summary>
        /// Builds that menu
        /// </summary>
        public void Build() {
            if (built == false)
                OnBuild();
            built = true;
        }

        /// <summary>
        /// Clear that menus
        /// </summary>
        public void Clear() {
            if (built == true) {
                OnClear();
            }
            built = false;
        }
        #endregion

        #region Abstract Methods
        protected abstract void OnBuild();

        protected abstract void OnClear();
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

