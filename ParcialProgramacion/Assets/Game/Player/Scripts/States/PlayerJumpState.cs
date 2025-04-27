using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Player.JumpForce);
            Player.JumpCount++;
        }

        public override void Update()
        {
            base.Update();

            if (Rigidbody2D.velocity.y < 0)
                StateMachine.ChangeState(Player.AirState);
        }

    }
}