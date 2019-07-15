using UnityEngine;

namespace DGTools.UI
{
    public interface IUITilable
    {
        Color tileColor { get; }
        Sprite tileIcon { get; }
        string tileTitle { get; }
        string tileText { get; }

        void OnTileClick();
    }
}
