using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(
            _player,
            _stateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Player.Skill.SwordSkill.DotsActive(true);
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();

            if (Input.GetKeyUp(KeyCode.Mouse1))
                StateMachine.ChangeState(Player.IdleState);

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Player.transform.position.x > mousePosition.x && Player.FacingDir == 1 ||
                Player.transform.position.x < mousePosition.x && Player.FacingDir == -1)
                Player.Flip();
        }

        public override void Exit()
        {
            base.Exit();

            Player.StartCoroutine("BusyFor", .2f);
        }
    }
}