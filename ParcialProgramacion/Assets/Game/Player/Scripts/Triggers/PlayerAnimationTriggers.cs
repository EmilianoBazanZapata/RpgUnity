using Game.Player.Scripts.Managers;
using UnityEngine;

namespace Game.Player.Scripts.Triggers
{
    public class PlayerAnimationTriggers: MonoBehaviour
    {
        private Player _player => GetComponentInParent<Player>();

        private void animationTrigger()
        {
            _player.AnimationTrigger();
        }

        private void TrhowSword()
        {
            SkillManager.Instance.SwordSkill.CreateSword();
        }
    }
}