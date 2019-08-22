using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGTools.UI.Editor
{
    [CreateAssetMenu(menuName = "Temp/DemoItem")]
    public class DemoItem : ScriptableObject, IUITilable
    {
        public string description;
        public Color color;
        public Sprite icon;

        public Color tileColor => color;

        public Sprite tileIcon => icon;

        public string tileTitle => name;

        public string tileText => description;

        public void OnTileClick()
        {
            Debug.Log(string.Format("{0} clicked!", name));
        }
    }
}
