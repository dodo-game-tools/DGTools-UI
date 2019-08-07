using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System;
using System.Linq;

namespace DGTools.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        #region Enums
        public enum OnHideAction { none, clear, disable, disableAndClear, destroy }
        #endregion

        #region Public Variables
        [Header("UI Component Settings")]
        [Tooltip("Choose what will the UIComponent do when hidden")]
        public OnHideAction onHideAction = OnHideAction.disable;
        [Tooltip("Show() will be called on Awake() if true")]
        public bool showOnAwake = false;
        #endregion

        #region Events
        [Serializable] public class UIComponentEvent : UnityEvent<UIComponent> { }
        [SerializeField] protected UIComponentEvent onShow = new UIComponentEvent();
        [SerializeField] protected UIComponentEvent onHide = new UIComponentEvent();
        #endregion

        #region Properties
        public Animator animator { get; private set; }
        public RectTransform rectTransform { get; private set; }

        /// <summary>
        /// True if component has an <see cref="Animator"/> with a Controller attached to it
        /// </summary>
        public bool hasAnimation {
            get { return animator != null && animator.runtimeAnimatorController != null; }
        }

        /// <summary>
        /// True if an animation is playing
        /// </summary>
        public bool isPlaying {
            get {
                if (!hasAnimation) return false;
                return animator.GetBool("isShow") != gameObject.activeInHierarchy;
            }
        }

        /// <summary>
        /// True if component is shown
        /// </summary>
        public bool isShown {
            get {
                return hasAnimation ? animator.GetBool("isShow") : gameObject.activeInHierarchy;
            }
            set {
                if (value == true)
                {
                    Show();
                }
                else {
                    Hide();
                }
            }
        }

        /// <summary>
        /// True if component has been built
        /// </summary>
        public bool built { get; protected set; } = false;
        #endregion

        #region Public Methods
        /// <summary>
        /// Displays the component and plays the animation if an <see cref="Animator"/> has been attached to it
        /// </summary>
        /// <param name="skipAnim">Skips the animation if set to true</param>
        public virtual void Show(bool skipAnim = false)
        {
            gameObject.SetActive(true);
            if (!skipAnim && hasAnimation)
            {
                animator.SetBool("isShow", true);
                animator.SetTrigger("play");
            }
            else {
                OnShow();
            }
        }

        /// <summary>
        /// Displays the component and plays the animation if an <see cref="Animator"/> has been attached to it
        /// </summary>
        /// <param name="action">The action to preform in <see cref="UIComponent.OnShow()"/></param>
        public virtual void Show(UnityAction<UIComponent> action) {
            onShow.AddListener(action);
            Show();
        }

        /// <summary>
        /// Hides the component and plays the animation if an <see cref="Animator"/> has been attached to it
        /// </summary>
        /// <param name="skipAnim">Skips the animation if set to true</param>
        public virtual void Hide(bool skipAnim = false)
        {
            if (!skipAnim && hasAnimation)
            {
                animator.SetBool("isShow", false);
                animator.SetTrigger("play");
            }
            else {
                OnHide();
            }
        }

        /// <summary>
        /// Hides the component and plays the animation if an <see cref="Animator"/> has been attached to it
        /// </summary>
        /// <param name="action">The action to preform in <see cref="UIComponent.OnHide()"/></param>
        public virtual void Hide(UnityAction<UIComponent> action)
        {
            onHide.AddListener(action);
            Hide();
        }

        /// <summary>
        /// Rebuild the component
        /// </summary>
        public void Reload() {
            Clear();
            Build();
        }
        #endregion

        #region Private Methods
        protected virtual void CheckComponent() {
            if (hasAnimation)
            {
                Assert.IsTrue(
                        animator.parameters.Any(param => param.name == "isShow"),
                        string.Format("The animator of {0} should have a parameter of type bool called \"isShow\"", name)
                    );
                Assert.IsTrue(
                    animator.parameters.Any(param => param.name == "play"),
                    string.Format("The animator of {0} should have a trigger parameter called \"play\"", name)
                );
            }
        }

        void RunBuild() {
            if (!built) {
                Build();
                built = true;
            }
        }

        void RunClear() {
            if (built)
            {
                Clear();
                built = false;
            }
        }
        #endregion

        #region Abstract Methods
        protected abstract void Build();

        public abstract void Clear();
        #endregion

        #region Event Methods
        public virtual void OnShow()
        {
            RunBuild();
            onShow.Invoke(this);
            onShow.RemoveAllListeners();
        }

        public virtual void OnHide()
        {
            switch (onHideAction)
            {
                case OnHideAction.none:
                    break;
                case OnHideAction.disable:
                    gameObject.SetActive(false);
                    break;
                case OnHideAction.clear:
                    RunClear();
                    break;
                case OnHideAction.disableAndClear:
                    gameObject.SetActive(false);
                    RunClear();
                    break;
                case OnHideAction.destroy:
                    Destroy(gameObject);
                    break;
            }
            onHide.Invoke(this);
            onHide.RemoveAllListeners();

        }
        #endregion

        #region Runtime Methods
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rectTransform = GetComponent<RectTransform>();

            if (Application.isEditor) {
                CheckComponent();
            }

            if (showOnAwake)
                Show();
        }
        #endregion
    }
}
