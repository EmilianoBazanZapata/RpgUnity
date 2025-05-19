using Game.Character.Scripts.Managers;
using Game.Enemies;
using UnityEngine;

namespace Game.Character.Scripts.Triggers
{
    /// <summary>
    /// Ejecuta eventos de animaciones del jugador, como ataque, lanzar espada o atraparla.
    /// Todos los métodos se llaman desde el Animator con Animation Events.
    /// </summary>
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        private Player _player;

        private void Awake() => _player = GetComponentInParent<Player>();
        
        /// <summary>
        /// Notifica que terminó una animación (idle, dash, ataque, etc).
        /// </summary>
        private void AnimationTrigger() => _player.AnimationTrigger(); 
        
        /// <summary>
        /// Realiza el ataque al detectar enemigos dentro del radio de impacto.
        /// </summary>
        private void AttackTrigger()
        {
            var colliders = Physics2D.OverlapCircleAll(
                _player.attackCheck.position,
                _player.attackCheckRadius
            );

            foreach (var hit in colliders)
            {
                if (hit.TryGetComponent(out Enemy enemy) &&
                    enemy.TryGetComponent(out EnemyStats target))
                {
                    _player.Stats.DoDamage(target);
                }
            }
        }

        /// <summary>
        /// Ejecuta la lógica de lanzar la espada como habilidad.
        /// </summary>
        private void ThrowSword() => SkillManager.Instance.SwordSkill.CreateSword();
    }
}