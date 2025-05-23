﻿using Game.Character.Scripts.StateMachine;
using Game.Enemies;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    public class PlayerCounterAttackState: PlayerState
    {
        private bool _canCreateClone;

        public PlayerCounterAttackState(Player player, 
                                        PlayerStateMachine stateMachine, 
                                        string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _canCreateClone = true;
            StateTimer = Player.CounterAttackDuration;
            Player.Anim.SetBool("SuccessfullCounterAttack", false);
            SoundManager.Instance.PlaySound(SoundType.Attack);
        }

        public override void Update()
        {
            base.Update();
            
            Player.SetZeroVelocity();

            var colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() == null) continue;
                if (!hit.GetComponent<Enemy>().CanBeStunned) continue;
            
                StateTimer = 10; // any value bigger than 1
                Player.Anim.SetBool("SuccessfullCounterAttack", true);

                if (!_canCreateClone) continue;
            
                _canCreateClone = false;
            }

            if (StateTimer < 0 || TriggerCalled)
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}