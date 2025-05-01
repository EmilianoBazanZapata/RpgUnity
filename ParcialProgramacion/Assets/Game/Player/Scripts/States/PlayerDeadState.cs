using Game.Managers;
using Game.Player.Scripts.StateMachine;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            
            GameManager.Instance.LoseGame();
        }
    }
}