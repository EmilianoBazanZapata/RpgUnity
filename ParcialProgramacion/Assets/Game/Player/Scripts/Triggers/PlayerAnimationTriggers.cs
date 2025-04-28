using Game.Enemies;
using Game.Player.Scripts.Managers;
using Game.Shared.Enums;
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
        
        private void AttackTrigger()
        {
            var colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() == null) continue;

                var target = hit.GetComponent<EnemyStats>();
            
                if (target != null)
                    _player.Stats.DoDamage(target);
            }
        }

        private void TrhowSword()
        {
            SkillManager.Instance.SwordSkill.CreateSword();
        }
    }
}