using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DGTools.UI.Editor
{
    [AddComponentMenu("")]
    public class UIPrefabCreator : MonoBehaviour
	{
        const string prefabsPath = "UIPrefabs";

        #region Static Methods
        public static void CreateUIAsset<Tasset>(MenuCommand menuCommand, string prefabName, string defaultName = null) where Tasset : MonoBehaviour
        {
            Tasset asset = Instantiate(Resources.Load<Tasset>(prefabsPath + "/" + prefabName));
            if (!string.IsNullOrEmpty(defaultName))
            {
                asset.name = defaultName;
            }
            else
            {
                asset.name = prefabName;
            }
            GameObjectUtility.SetParentAndAlign(asset.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(asset.gameObject, "Create " + asset.name);
            Selection.activeObject = asset;
        }
        #endregion

        #region Menu Definitions
        [MenuItem("GameObject/UI/DGTools/Modals/Popup Example", false, 10)]
        static void CreatePopup(MenuCommand menuCommand) {
            CreateUIAsset<Popup>(menuCommand, "Popup");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Grid", false, 10)]
        static void CreateUIGrid(MenuCommand menuCommand)
        {
            CreateUIAsset<UIGrid>(menuCommand, "UIGrid");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Tile", false, 10)]
        static void CreateUITile(MenuCommand menuCommand)
        {
            CreateUIAsset<UITile>(menuCommand, "UITile");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Selectable Tile", false, 10)]
        static void CreateUISelectableTile(MenuCommand menuCommand)
        {
            CreateUIAsset<UISelectableTile>(menuCommand, "UISelectableTile");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Selector Simple", false, 10)]
        static void CreateUISelectorSimple(MenuCommand menuCommand)
        {
            CreateUIAsset<UISelectorSimple>(menuCommand, "UISelectorSimple");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Selector Multiple", false, 10)]
        static void CreateUISelectorMultiple(MenuCommand menuCommand)
        {
            CreateUIAsset<UISelectorMultiple>(menuCommand, "UISelectorMultiple");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Panel Switcher", false, 10)]
        static void CreateUIPanelSwitcher(MenuCommand menuCommand)
        {
            CreateUIAsset<UIPanelSwitcher>(menuCommand, "UIPanelSwitcher");
        }

        [MenuItem("GameObject/UI/DGTools/Components/UI Panel", false, 10)]
        static void CreateUIPanel(MenuCommand menuCommand)
        {
            CreateUIAsset<UIPanel>(menuCommand, "UIPanel");
        }

        #endregion
    }
}
