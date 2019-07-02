using UnityEngine;

namespace DGTools.UI {
    public class MenuManager : StaticManager<MenuManager>
    {
        //VARIABLES
        [Header("Relations")]
        [Tooltip("A GameObjet that will be the parent of all menus")]
        [SerializeField] GameObject container;
        [Tooltip("The path of the folder that contains Menus prefabs")]
        [SerializeField] [FolderPath(folderPathRestriction = "Resources")] string menusFolder;
        [Tooltip("This menu will open on Start()")]
        [SerializeField] Menu defaultMenu;

        //PROPERTIES
        /// <summary>
        /// The currently opened menu
        /// </summary>
        public static Menu activeMenu {
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
            private set
            {
                foreach (Menu menu in loadedMenus)
                {
                    if (menu == value)
                    {
                        menu.ShowAsync();
                    }
                    else
                    {
                        if (menu.isActiveAndEnabled)
                            CloseMenu(menu);
                    }
                }
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

        //STATIC METHODS
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
        /// Closes all menus according to their <see cref="UIController.OnHideAction"/>
        /// </summary>
        public static void CloseAllMenus()
        {
            foreach (Menu menu in loadedMenus)
            {
                CloseMenu(menu);
            }
        }

        /// <summary>
        /// Closes a menu according to its <see cref="UIController.OnHideAction"/>
        /// </summary>
        /// <param name="menu">The menu to close</param>
        public static void CloseMenu(Menu menu)
        {
            menu.HideAsync();
        }


        //METHODS
        TMenu RunOpenMenu<TMenu>(TMenu menu) where TMenu : Menu
        {
            if (activeMenu != null)
                CloseMenu(activeMenu);

            activeMenu = menu;

            return menu;
        }

        TMenu RunOpenMenu<TMenu, TParam>(TParam param, string name = null) where TMenu : Menu<TParam>
        {
            TMenu menu = RunLoadMenu<TMenu>(name);
            menu.SetParams(param);
            return RunOpenMenu(menu);
        }

        TMenu RunLoadMenu<TMenu>(string name = null) where TMenu : Menu
        {
            TMenu menuInstance = null;
            foreach (Menu menu in loadedMenus) {
                if (menu is TMenu && (string.IsNullOrEmpty(name) || menu.name == name)) {
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
                    if (menu.name == name) menuInstance = menu;
                }
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

        void OpenDefaultMenu()
        {
            OpenMenu(Instantiate(defaultMenu, container.transform));
        }

        //RUNTIME METHODS
        private void Start()
        {
            OpenDefaultMenu();
        }
    }
}
