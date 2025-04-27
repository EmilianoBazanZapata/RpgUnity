using Game.Player.Scripts.Inputs;
using Game.Player.Scripts.StateMachine;
using Game.Player.Scripts.States;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts
{
    public class Player : Entity
    {
        [Header("Move info")] 
        public float MoveSpeed = 9f;
        public float JumpForce = 8f;
        public float SwordReturnImpact;
        public float DefaultMoveSpeed;
        public float DefaultJumpForce;
        
        [Header("Dash info")] 
        public float DashSpeed = 12f;
        public float DashDuration = 0.4f;
        public float DashDir { get; private set; }
        public float DefaultDashSpeed;
        
        public bool IsBusy { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }

        #region States
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler = new PlayerInputHandler();
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            DashState = new PlayerDashState(this, StateMachine, "Dash");
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
    }
}