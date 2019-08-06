using UnityEngine;

namespace DGTools.UI {
    public class MenuManager : StaticManager<MenuManager>
    {
        #region Variables
        [Header("Relations")]
        [Tooltip("A GameObjet that will be the parent of all menus")]
        [SerializeField] protected GameObject container;
        [Tooltip("The path of the folder that contains Menus prefabs")]
        [SerializeField] [FolderPath(folderPathRestriction = "Resources")] protected string menusFolder;
        [Tooltip("This menu will open on Awake()")]
        [SerializeField] protected Menu defaultMenu;
        #endregion

        #region Private Variables
        Menu _activeMenu;
        #endregion

        #region Properties
        /// <summary>
        /// The currently opened menu
        /// </summary>
        /*public static Menu activeMenu {
            get
            {
                Menu[] loaded = loadedMenus;
                for (int i = loaded.Length - 1; i >= 0; i--)
                {
                    if (loaded[i].isActiveAndEnabled)
                        return loaded[i];
                }
                return null;
            }
            protected set
            {
                foreach (Menu menu in loadedMenus)
                {
                    if (menu == value)
                    {
                        menu.Show();
                    }
                    else
                    {
                        if (menu.isActiveAndEnabled)
                            CloseMenu(menu);
                    }
                }
            }
        }*/
        public static Menu activeMenu {
            get {
                return active._activeMenu;
            }
            set {
                active._activeMenu.Hide();
                active._activeMenu = value;
                active._activeMenu.Show();
            }
        }

        /// <summary>
        /// All menus currently loaded in hierarchy
        /// </summary>
        public static Menu[] loadedMenus
        {
            get
            {
                return active.container.GetComponentsInChildren<Menu>(true);
            }
        }

        static int currentSibling {
            get { return activeMenu.transform.GetSiblingIndex(); }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Opens the given Menu
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <param name="menu">The menu to Open</param>
        /// <returns>Returns the active menu</returns>
        public static TMenu OpenMenu<TMenu>(TMenu menu) where TMenu : Menu
        {
            return active.RunOpenMenu(menu);
        }

        /// <summary>
        ///  Load a menu from "Resources/{menusFolder}/" and open it
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <param name="name">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <returns>Returns the active menu</returns>
        public static TMenu OpenMenu<TMenu>(string name = null) where TMenu : Menu
        {
            return OpenMenu(LoadMenu<TMenu>(name));
        }

        /// <summary>
        ///  Load a menu from "Resources/{menusFolder}/" and open it
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <typeparam name="Tparam">Type of menu's param</typeparam>
        /// <param name="param">Menu's param</param>
        /// <param name="param"></param>
        /// <returns>Returns the active menu</returns>
        public static TMenu OpenMenu<TMenu, Tparam>(TMenu menu, Tparam param) where TMenu : Menu<Tparam> {
            menu = active.RunLoadMenu(menu, param);
            return active.RunOpenMenu(menu);
        }

        /// <summary>
        /// Load a menu from "Resources/{menusFolder}/" and open it
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <typeparam name="Tparam">Type of menu's param</typeparam>
        /// <param name="param">Menu's param</param>
        /// <param name="name">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <returns>Returns the active menu</returns>
        public static TMenu OpenMenu<TMenu, Tparam>(Tparam param, string name = null) where TMenu : Menu<Tparam>
        {
            return active.RunOpenMenu<TMenu,Tparam>(param, name);
        }

        /// <summary>
        /// Load and instantiate a menu from "Resources/{menusFolder}/" (don't show it!)
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <param name="name">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <returns>Returns the loaded menu</returns>
        public static TMenu LoadMenu<TMenu>(string name = null) where TMenu : Menu
        {
            return active.RunLoadMenu<TMenu>(name);
        }

        /// <summary>
        /// Load and instantiate a menu from "Resources/{menusFolder}/" (don't show it!)
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <typeparam name="TParam">Type of menu's param</typeparam>
        /// <param name="param">Param value of the menu</param>
        /// <param name="name">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <returns>Returns the loaded menu</returns>
        public static TMenu LoadMenu<TMenu, TParam>(TParam param, string name = null) where TMenu : Menu<TParam>
        {
            return active.RunLoadMenu<TMenu, TParam>(param, name);
        }

        /// <summary>
        /// Load and instantiate a menu from "Resources/{menusFolder}/" (don't show it!)
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <param name="menu">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <returns>Returns the loaded menu</returns>
        public static TMenu LoadMenu<TMenu>(TMenu menu) where TMenu : Menu
        {
            return active.RunLoadMenu(menu);
        }

        /// <summary>
        /// Load and instantiate a menu from "Resources/{menusFolder}/" (don't show it!)
        /// </summary>
        /// <typeparam name="TMenu">Type of the menu</typeparam>
        /// <param name="menu">Name of menu's prefab (takes first menu found if null or empty)</param>
        /// <typeparam name="TParam">Type of menu's param</typeparam>
        /// <param name="param">Param value of the menu</param>
        /// <returns>Returns the loaded menu</returns>
        public static TMenu LoadMenu<TMenu, TParam>(TMenu menu, TParam param) where TMenu : Menu<TParam> {
            return active.RunLoadMenu(menu, param);
        }

        /// <summary>
        /// Opens the previous menu in hierarchy (does nothing if is already at the first sibling index)
        /// </summary>
        public static void PreviousMenu()
        {
            Menu[] loaded = loadedMenus;
            if (currentSibling > 0)
                activeMenu = loaded[currentSibling - 1];
        }

        /// <summary>
        /// Opens the next menu in hierarchy (does nothing if is already at the last sibling index)
        /// </summary>
        public static void NextMenu()
        {
            Menu[] loaded = loadedMenus;
            if (currentSibling < loaded.Length - 1)
                activeMenu = loaded[currentSibling + 1];
        }

        /// <summary>
        /// Closes all menus according to their <see cref="UIComponent.OnHideAction"/>
        /// </summary>
        public static void CloseAllMenus()
        {
            foreach (Menu menu in loadedMenus)
            {
                CloseMenu(menu);
            }
        }

        /// <summary>
        /// Closes a menu according to its <see cref="UIComponent.OnHideAction"/>
        /// </summary>
        /// <param name="menu">The menu to close</param>
        public static void CloseMenu(Menu menu)
        {
            menu.Hide();
        }
        #endregion


        #region Private Methods
        protected virtual TMenu RunOpenMenu<TMenu>(TMenu menu) where TMenu : Menu
        {
            if (activeMenu != null)
                CloseMenu(activeMenu);

            activeMenu = menu;

            return menu;
        }

        protected virtual TMenu RunOpenMenu<TMenu, TParam>(TParam param, string name = null) where TMenu : Menu<TParam>
        {
            TMenu menu = RunLoadMenu<TMenu, TParam>(param, name);
            return RunOpenMenu(menu);
        }

        protected virtual TMenu RunLoadMenu<TMenu>(string name = null) where TMenu : Menu
        {
            TMenu menuInstance = null;
            foreach (Menu menu in loadedMenus) {
                if (menu is TMenu && (string.IsNullOrEmpty(name) || menu.name.Replace("(Clone)", "") == name)) {
                    menuInstance = menu as TMenu;
                    menuInstance.Reload();
                    return menuInstance;
                }
            }

            TMenu[] menus = Resources.LoadAll<TMenu>(menusFolder);

            if (menus.Length == 0)
                throw new System.Exception(string.Format("No Menu of type {0} found at {1}", typeof(TMenu), menusFolder));

            if (!string.IsNullOrEmpty(name))
            {
                foreach (TMenu menu in menus)
                {
                    if (menu.name.Replace("(Clone)", "") == name)
                    {
                        menuInstance = menu;
                        break;
                    }
                }

                if(menuInstance == null)
                    throw new System.Exception(string.Format("No Menu of type {0} with name {1} found at {2}", typeof(TMenu), name, menusFolder));
            }
            else
            {
                menuInstance = menus[0];
            }

            menuInstance = Instantiate(menuInstance, container.transform);
            menuInstance.gameObject.SetActive(false);

            return menuInstance;
        }

        protected virtual TMenu RunLoadMenu<TMenu, TParam>(TParam param, string name = null) where TMenu : Menu<TParam>
        {
            TMenu menu = RunLoadMenu<TMenu>(name);
            menu.SetParams(param);
            return menu;
        }

        protected virtual TMenu RunLoadMenu<TMenu>(TMenu menu) where TMenu : Menu {
            foreach (Menu m in loadedMenus)
            {
                if (m == menu) {
                    return menu;
                }
            }

            menu = Instantiate(menu, container.transform);
            menu.gameObject.SetActive(false);

            return menu;
        }

        protected virtual TMenu RunLoadMenu<TMenu, TParam>(TMenu menu, TParam param) where TMenu : Menu<TParam>
        {
            menu = RunLoadMenu(menu);
            menu.SetParams(param);
            return menu;
        }

        protected virtual void OpenDefaultMenu()
        {
            if (defaultMenu == null) return;
            OpenMenu(Instantiate(defaultMenu, container.transform));
        }
        #endregion

        #region Runtime Methods
        protected virtual void Start()
        {
            OpenDefaultMenu();
        }
        #endregion
    }
}
