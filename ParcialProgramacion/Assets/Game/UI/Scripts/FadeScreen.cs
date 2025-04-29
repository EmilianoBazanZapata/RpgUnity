using UnityEngine;

namespace Game.UI.Scripts
{
    public class FadeScreen : MonoBehaviour
    {
        private Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void FadeOut() => anim.SetTrigger("fadeOut");
        public void FadeIn() => anim.SetTrigger("fadeIn");
    }
}