using System.Collections;
using Game.Player.Scripts.Inputs;
using Game.Player.Scripts.StateMachine;
using Game.Player.Scripts.States;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts
{
    public class Player : Entity
    {
        [Header("Attack details")] 
        public Vector2[] AttackMovement;
        public float CounterAttackDuration = .2f;
        public bool IsBusy { get; private set; }
        
        [Header("Move info")] 
        public float MoveSpeed = 9f;
        public float JumpForce = 8f;
        public float SwordReturnImpact;
        public float DefaultMoveSpeed;
        public float DefaultJumpForce;
        public int MaxJumpCount = 2;
        public int JumpCount;
        
        [Header("Dash info")] 
        public float DashSpeed = 12f;
        public float DashDuration = 0.2f;
        public float DashDir { get; private set; }
        public float DefaultDashSpeed;
        
        public PlayerInputHandler InputHandler { get; private set; }

        #region States
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerAirState AirState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler = new PlayerInputHandler();
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            DashState = new PlayerDashState(this, StateMachine, "Dash");
            JumpState = new PlayerJumpState(this, StateMachine, "Jump");
            AirState = new PlayerAirState(this, StateMachine, "Jump");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
            PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        }

        protected override void Start()
        {
            base.Start();
            
            StateMachine.Initialize(IdleState);
            DefaultMoveSpeed = MoveSpeed;
            DefaultJumpForce = JumpForce;
            DefaultDashSpeed = DashSpeed;
        }
        
        protected override void Update()
        {
            base.Update();

            StateMachine.CurrentState.Update();

            CheckForDashInput();
        }
        
        private void CheckForDashInput()
        {
            if (IsWallDetected())
                return;

            if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        
            DashDir = Input.GetAxisRaw("Horizontal");

            if (DashDir != 0)
                DashDir = FacingDir;

            StateMachine.ChangeState(DashState);
        }
        public IEnumerator BusyFor(float seconds)
        {
            IsBusy = true;

            yield return new WaitForSeconds(seconds);

            IsBusy = false;
        }
        public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    }
}