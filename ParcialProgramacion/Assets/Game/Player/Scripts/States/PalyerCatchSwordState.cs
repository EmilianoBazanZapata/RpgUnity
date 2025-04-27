using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PalyerCatchSwordState : PlayerState
    {
        private Transform _sword;

        public PalyerCatchSwordState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _sword = Player.Sword.transform;

            if (Player.transform.position.x > _sword.position.x && Player.FacingDir == 1)
                Player.Flip();
            else if (Player.transform.position.x < _sword.position.x && Player.FacingDir == -1)
                Player.Flip();

            Rigidbody2D.velocity = new Vector2(Player.SwordReturnImpact * -Player.FacingDir, Rigidbody2D.velocity.y);
        }

        public override void Update()
        {
            base.Update();

            if (TriggerCalled)
                StateMachine.ChangeState(Player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine("BusyFor", .1f);
        }
    }
}