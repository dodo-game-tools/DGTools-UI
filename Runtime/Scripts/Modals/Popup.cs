using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DGTools.UI
{
	public class Popup : Modal<PopupSettings>
	{
        #region Enums
        public enum ActionType { validate, deny, cancel }
        #endregion

        #region Public Variables
        [Header("Components")]
        public Text title;
        public Text message;
        public Button validateButton;
        public Button denyButton;
        public Button cancelButton;
        #endregion

        #region Static Methods
        public static Popup Open(PopupSettings settings) {
            if (ModalManager.active == null)
                throw new Exception("To use Popups, your scene should contain a ModalManager");
            return ModalManager.OpenModal<Popup, PopupSettings>(settings);
        }
        #endregion

        #region Public Methods
        public override void Clear()
        {
            validateButton.onClick.RemoveAllListeners();
            denyButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
        #endregion

        #region Private Methods
        protected override void Build()
        {
            if (!string.IsNullOrEmpty(param.title))
            {
                title.gameObject.SetActive(true);
                title.text = param.title;
            }
            else {
                title.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(param.message))
            {
                message.gameObject.SetActive(true);
                message.text = param.message;
            }
            else
            {
                message.gameObject.SetActive(false);
            }

            if (param.callback != null) {
                if (param.possibleActions.HasFlag(ActionType.validate))
                {
                    validateButton.gameObject.SetActive(true);
                    validateButton.onClick.AddListener(delegate { OnActionClick(ActionType.validate); });
                    if (!string.IsNullOrEmpty(param.validateText))
                        validateButton.GetComponentInChildren<Text>().text = param.validateText;
                }
                else
                    validateButton.gameObject.SetActive(false);

                if (param.possibleActions.HasFlag(ActionType.deny))
                {
                    denyButton.gameObject.SetActive(true);
                    denyButton.onClick.AddListener(delegate { OnActionClick(ActionType.deny); });
                    if (!string.IsNullOrEmpty(param.denyText))
                        denyButton.GetComponentInChildren<Text>().text = param.denyText;
                }
                else
                    denyButton.gameObject.SetActive(false);

                if (param.possibleActions.HasFlag(ActionType.cancel))
                {
                    cancelButton.gameObject.SetActive(true);
                    cancelButton.onClick.AddListener(delegate { OnActionClick(ActionType.cancel); });
                    if (!string.IsNullOrEmpty(param.cancelText))
                        cancelButton.GetComponentInChildren<Text>().text = param.cancelText;
                }
                else
                    cancelButton.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Event Methods
        void OnActionClick(ActionType actionType) {
            param.callback.Invoke(this, actionType);
            Close();
        }
        #endregion

#if UNITY_EDITOR
        #region Editor Methods
        [MenuItem("GameObject/UI/DGTools/Components/Popup Example", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            Popup popup = Instantiate(Resources.Load<Popup>("Prefabs/Popup"));
            popup.name = "Popup";
            GameObjectUtility.SetParentAndAlign(popup.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(popup.gameObject, "Create " + popup.name);
            Selection.activeObject = popup;
        }
        #endregion
#endif
    }

    public class PopupSettings
    {
        public string title = null;
        public string message = null;
        public string validateText = null;
        public string denyText = null;
        public string cancelText = null;
        public Popup.ActionType possibleActions = Popup.ActionType.validate | Popup.ActionType.deny | Popup.ActionType.cancel;
        public UnityAction<Popup, Popup.ActionType> callback;
    }
}
