using UnityEngine;
using UnityEngine.Events;
using System;

namespace DGTools.UI {
    public abstract class Modal : UIComponent
    {
        #region Events
        [Serializable] public class ModalEvent : UnityEvent<Modal> { }
        [SerializeField] public ModalEvent OnModalClosed = new ModalEvent();
        #endregion

        #region Public Methods
        public virtual void Close() {
            ModalManager.PreviousModal();
        }

        public override void OnHide()
        {
            OnModalClosed.Invoke(this);
            base.OnHide();
        }
        #endregion
    }

    public abstract class Modal<Tparam> : Modal {
        public Tparam param;
    }
}
