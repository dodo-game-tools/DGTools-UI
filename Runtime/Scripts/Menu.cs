namespace DGTools.UI {
    public abstract class Menu : UIController
    {
        //METHODS
        public override void Show()
        {
            Build();
            base.Show();
        }

        public virtual void Previous()
        {
            MenuManager.PreviousMenu();
        }

        public virtual void Next()
        {
            MenuManager.NextMenu();
        }

        public virtual void Close()
        {
            MenuManager.CloseMenu(this);
        }

        public void Reload()
        {
            Clear();
            Build();
        }

        //ABSTRACT METHODS
        protected abstract void Build();

        protected abstract void Clear();
    }

    public abstract class Menu<Tparam> : Menu
    {
        //VARIABLES
        protected Tparam param;

        //METHODS
        public virtual void SetParams(Tparam param)
        {
            this.param = param;
        }
    }
}

