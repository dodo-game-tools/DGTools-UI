using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI {
    public abstract class Modal : UIController
    {
        //VARIABLES
        [Header("Components")]
        public Text content;
        public Button validateButton;
        public Button denyButton;
        public Button cancelButton;

        //METHODS
        public virtual void Close() {
            ModalManager.PreviousModal();
        }

        /// <summary>
        /// You can override this method to add other default actions, otherwise you 
        /// can modify behaviour of default actions by implementing abstract default methods
        /// </summary>
        void SetDefaultActions() {
            if(validateButton != null)
                validateButton.onClick.AddListener(DefaultValidate);
            if (denyButton != null)
                denyButton.onClick.AddListener(DefaultDeny);
            if (cancelButton != null)
                cancelButton.onClick.AddListener(DefaultCancel);
        }

        public override void Show()
        {
            Build();
            base.Show();
        }

        /// <summary>
        /// You can override this methods with you own params and set the content of the modal from this
        /// </summary>
        /// <param name="content">String content</param>
        public virtual void SetContent(string content) {
            this.content.text = content;
        }

        //ABSTRACT METHODS
        public abstract void Build();        

        public abstract void DefaultValidate();

        public abstract void DefaultDeny();

        public abstract void DefaultCancel();

        //RUNTIME METHODS
        protected virtual void Awake()
        {
            SetDefaultActions();
        }
    }
}
