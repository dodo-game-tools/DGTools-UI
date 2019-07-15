using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DGTools.UI
{
    public class ModalManager : StaticManager<ModalManager>
    {
        #region Variables
        [Header("Settings")]
        [Tooltip("If true, container will be set as last sibling on Awake in order to display modals over the other elements")]
        [SerializeField] bool autoSibling = true;

        [Header("Relations")]
        [FolderPath(folderPathRestriction = "Resources")] public string modalsFolder;
        [Tooltip("An image that will be the parent of all modals and will fadeIn when a modal is open")]
        [SerializeField] Image container;

        [Header("Background")]
        [SerializeField] [Range(0, 1)] float backgroundAlpha;
        [SerializeField] [Range(0.01f, 1)] float transitionTime;
        #endregion

        #region Properties
        /// <summary>
        /// The currently opened modal
        /// </summary>
        public static Modal activeModal {
            get {
                Modal[] loaded = loadedModals;
                for (int i = loaded.Length - 1; i >= 0; i--)
                {
                    if (loaded[i].isActiveAndEnabled)
                        return loaded[i];
                }
                return null;
            }
            private set {
                foreach (Modal modal in loadedModals) {
                    if (modal == value)
                    {
                        modal.transform.SetAsLastSibling();
                        modal.Show();
                    }
                    else {
                        if(modal.isActiveAndEnabled)
                            modal.Show();
                    }
                }
            }
        }

        /// <summary>
        /// All modals currently loaded in hierarchy
        /// </summary>
        public static Modal[] loadedModals {
            get
            {
                return active.container.GetComponentsInChildren<Modal>(true);
            }
        }

        /// <summary>
        /// True if the background masking is enabled
        /// </summary>
        public static bool isMasking {
            get {
                return active.container.color.a == active.backgroundAlpha;
            }
        }

        static int currentSibling
        {
            get { return activeModal.transform.GetSiblingIndex(); }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Load a Modal from "Resources/{modalsFolder}/" and open it
        /// </summary>
        /// <typeparam name="Tmodal">Type of the modal</typeparam>
        /// <param name="name">Name of modal's prefab (takes first modal found if null or empty)</param>
        /// <param name="closeCurrent">Close the active modal if true</param>
        /// <returns>Returns the active Modal</returns>
        public static Tmodal OpenModal<Tmodal>(string name = null, bool closeCurrent = true) where Tmodal : Modal
        {
            return OpenModal(LoadModal<Tmodal>(name), closeCurrent);
        }

        /// <summary>
        /// Opens the given modal
        /// </summary>
        /// <typeparam name="Tmodal">Type of the modal</typeparam>
        /// <param name="modal">The modal to open</param>
        /// <param name="closeCurrent">Close the active modal if true</param>
        /// <returns>Returns the active Modal</returns>
        public static Tmodal OpenModal<Tmodal>(Tmodal modal, bool closeCurrent = true) where Tmodal : Modal
        {
            return active.RunOpenModal(modal, closeCurrent);
        }

        /// <summary>
        /// Load and instantiate a Modal from "Resources/{modalsFolder}/"
        /// </summary>
        /// <typeparam name="Tmodal">The type of the modal</typeparam>
        /// <param name="name">Name of modal's prefab (takes first modal found if null or empty)</param>
        /// <returns>Returns the active Modal</returns>
        public static Tmodal LoadModal<Tmodal>(string name = null) where Tmodal : Modal
        {
            return active.RunLoadModal<Tmodal>(name);
        }

        /// <summary>
        /// Opens the previous menu in hierarchy (fade out background if no modal found)
        /// </summary>
        public static void PreviousModal() {
            Modal[] loaded = loadedModals;
            CloseModal(activeModal, true);
            if (currentSibling - 1 >= 0)
            {
                activeModal = loaded[currentSibling - 1];
            }
            else
            {
                active.StartCoroutine(active.Fade(false));
            }
        }

        /// <summary>
        /// Closes all and fade out the background
        /// </summary>
        public static void CloseAllModals() {
            foreach (Modal modal in loadedModals)
            {
                CloseModal(modal, true);
            }
            active.StartCoroutine(active.Fade(false));
        }

        /// <summary>
        /// Closes the given modal
        /// </summary>
        /// <param name="modal">The modal to close</param>
        /// <param name="destroy">true : Destroy the modal; false : Just hide it</param>
        public static void CloseModal(Modal modal, bool destroy) {
            if (modal.gameObject.activeInHierarchy)
            {
                modal.onHideAction = destroy ? UIComponent.OnHideAction.destroy : UIComponent.OnHideAction.disable;
                modal.Show();
            }
            else {
                Destroy(modal.gameObject);
            }
        }
        #endregion

        #region Private Methods
        Tmodal RunOpenModal<Tmodal>(Tmodal modal, bool closeCurrent) where Tmodal : Modal {
            if (!isMasking)
                StartCoroutine(Fade(true));

            if (activeModal != null)
                CloseModal(activeModal, closeCurrent);

            activeModal = modal;

            return modal;
        }

        Tmodal RunLoadModal<Tmodal>(string name = null) where Tmodal : Modal {
            Tmodal[] modals = Resources.LoadAll<Tmodal>(modalsFolder);
            Tmodal modalInstance;

            if (modals.Length == 0)
                throw new System.Exception(string.Format("No Modal of type {0} found at {1}", typeof(Tmodal), modalsFolder));

            if (!string.IsNullOrEmpty(name)) {
                foreach (Tmodal modal in modals) {
                    if (modal.name == name) modalInstance = modal;
                }
                throw new System.Exception(string.Format("No Modal of type {0} with name {1} found at {2}", typeof(Tmodal), name, modalsFolder));
            }
            else {
                modalInstance = modals[0];
            }

            modalInstance = Instantiate(modalInstance, container.transform);
            modalInstance.gameObject.SetActive(false);

            return modalInstance;
        }
        #endregion

        #region Coroutines
        IEnumerator Fade(bool fadeIn) {
            if (fadeIn) container.enabled = true;
            while (fadeIn ? container.color.a < backgroundAlpha : container.color.a > 0) {
                Color color = container.color;
                if (fadeIn)
                {
                    color.a += backgroundAlpha * (Time.deltaTime / transitionTime);
                    if (color.a > backgroundAlpha)
                        color.a = backgroundAlpha;
                }
                else {
                    color.a -= backgroundAlpha * (Time.deltaTime / transitionTime);
                    if (color.a < 0)
                        color.a = 0;
                }
                container.color = color;
                yield return null;
            }
            if (!fadeIn) container.enabled = false;
        }
        #endregion

        #region Runtime Methods
        protected override void Awake()
        {
            base.Awake();
            if(autoSibling)
                container.transform.SetAsLastSibling();
        }
        #endregion
    }
}
