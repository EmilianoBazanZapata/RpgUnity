using Game.Character.Scripts;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.Triggers
{
    public class SkeletonAnimationTriggers : MonoBehaviour
    {
        // Accedemos al componente del esqueleto que contiene este trigger
        private EnemySkeleton _enemySkeleton => GetComponentInParent<EnemySkeleton>();

        /// <summary>
        /// Se llama desde la animación para indicar que terminó.
        /// </summary>
        private void AnimationTrigger() => _enemySkeleton.AnimationFinishTrigger();

        /// <summary>
        /// Se llama desde el evento de animación de ataque.
        /// Detecta colisión con el jugador y aplica daño.
        /// </summary>
        private void AttackTrigger()
        {
            var colliders = Physics2D.OverlapCircleAll(
                _enemySkeleton.attackCheck.position, 
                _enemySkeleton.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (!hit.TryGetComponent(out Player player)) continue;

                var targetStats = player.GetComponent<PlayerStats>();
                if (targetStats != null)
                {
                    _enemySkeleton.Stats.DoDamage(targetStats);
                    SoundManager.Instance.PlaySound(SoundType.EnemyHit);
                }
            }
        }

        /// <summary>
        /// Abre la ventana para permitir el contraataque del jugador.
        /// </summary>
        private void OpenCounterWindow() => _enemySkeleton.OpenCounterAttackWindow();

        /// <summary>
        /// Cierra la ventana de contraataque.
        /// </summary>
        private void CloseCounterWindow() => _enemySkeleton.CloseCounterAttackWindow();

        /// <summary>
        /// Trigger llamado al final de la animación de muerte.
        /// </summary>
        public void AnimationDeadTrigger() => _enemySkeleton.NotifyDead();
    }
}