using UnityEngine;

namespace DGTools.UI {
    public class UIComponentStateMachine : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            UIComponent component = animator.GetComponent<UIComponent>();
            if (animator.GetBool("isShow") == true)
            {
                component.OnShow();
                return;
            }
            else if (animator.GetBool("isShow") == false)
            {
                component.OnHide();
                return;
            }
        }
    }
}
