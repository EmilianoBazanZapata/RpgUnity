using Game.Player.Scripts;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.Triggers
{
    public class SkeletonAnimationTriggers : MonoBehaviour
    {
        private EnemySkeleton _enemySkeleton => GetComponentInParent<EnemySkeleton>();

        private void AnimationTrigger()
        {
            _enemySkeleton.AnimationFinishTrigger();
        }

        private void AttackTrigger()
        {
            var colliders =
                Physics2D.OverlapCircleAll(_enemySkeleton.attackCheck.position, _enemySkeleton.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Player.Scripts.Player>() == null) continue;


                var target = hit.GetComponent<PlayerStats>();
                _enemySkeleton.Stats.DoDamage(target);
            }
        }

        private void OpenCounterWindow() => _enemySkeleton.OpenCOunterAttackWindow();
        private void CloseCounterWindow() => _enemySkeleton.CloseCounterAttackWindow();
        public void AnimationDeadTrigger() => _enemySkeleton.NotifyDead();
    }
}