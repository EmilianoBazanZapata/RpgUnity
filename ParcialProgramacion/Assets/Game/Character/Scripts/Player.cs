using System.Collections;
using Game.Character.Scripts.Inputs;
using Game.Character.Scripts.Managers;
using Game.Character.Scripts.StateMachine;
using Game.Character.Scripts.States;
using Game.InventoryAndObjects.Scripts;
using Game.Managers;
using Game.Shared.Enums;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Character.Scripts
{
    public class Player : Entity
    {
        #region Singleton

        public static Player Instance { get; private set; }

        #endregion

        #region Serialized Fields

        [Header("Detalles de ataque")]
        public Vector2[] AttackMovement;
        public float CounterAttackDuration = 0.2f;

        [Header("Movimiento")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private float _swordReturnImpact;
        [SerializeField] private int _maxJumpCount = 2;

        [Header("Dash")]
        [SerializeField] private float _dashSpeed = 12f;
        [SerializeField] private float _dashDuration = 0.2f;

        #endregion

        #region Public Properties

        public float MoveSpeed => _moveSpeed;
        public float JumpForce => _jumpForce;
        public float DashSpeed => _dashSpeed;
        public float DashDuration => _dashDuration;
        public int MaxJumpCount => _maxJumpCount;
        public float SwordReturnImpact => _swordReturnImpact;
        public float DashDir { get; private set; }
        public int JumpCount { get; set; }
        public bool IsBusy { get; private set; }
        public GameObject Sword { get; private set; }

        #endregion

        #region Components

        public PlayerInputHandler InputHandler { get; private set; }
        public SkillManager Skill { get; private set; }

        #endregion

        #region Estados del jugador

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerAirState AirState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }
        public PlayerCatchSwordState CatchSwordState { get; private set; }
        public PlayerAimSwordState AimSwordState { get; private set; }
        public PlayerCounterAttackState CounterAttackState { get; private set; }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            // Asignación del singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InputHandler = new PlayerInputHandler();
            StateMachine = new PlayerStateMachine();

            // Inicialización de estados
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            DashState = new PlayerDashState(this, StateMachine, "Dash");
            JumpState = new PlayerJumpState(this, StateMachine, "Jump");
            AirState = new PlayerAirState(this, StateMachine, "Jump");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
            PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
            DeadState = new PlayerDeadState(this, StateMachine, "Die");
            CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
            AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");
            CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
        }
        
        protected override void Start()
        {
            base.Start();

            Skill = SkillManager.Instance;
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.Instance.CurrentState != GameState.InGame)
                return;

            HandleStateMachine();
            HandleInputActions();
        }

        #endregion

        #region Lógica de Entrada y Estados

        private void HandleStateMachine()
        {
            StateMachine.CurrentState.Update();
        }

        private void HandleInputActions()
        {
            CheckForDashInput();

            if (Input.GetKeyDown(KeyCode.F))
                Skill.CrystalSkill.CanUseSkill();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Inventory.instance.UseFlask();
        }

        private void CheckForDashInput()
        {
            if (IsWallDetected())
                return;

            if (!Input.GetKeyDown(KeyCode.LeftShift) || !Skill.DashSkill.CanUseSkill())
                return;

            DashDir = Input.GetAxisRaw("Horizontal");
            if (DashDir != 0)
                DashDir = FacingDir;

            StateMachine.ChangeState(DashState);
        }

        #endregion

        #region Acciones

        public IEnumerator BusyFor(float seconds)
        {
            IsBusy = true;
            yield return new WaitForSeconds(seconds);
            IsBusy = false;
        }

        public void AssignNewSword(GameObject newSword)
        {
            Sword = newSword;
        }

        public void CatchTheSword()
        {
            StateMachine.ChangeState(CatchSwordState);
            Destroy(Sword);
        }

        public void AnimationTrigger()
        {
            StateMachine.CurrentState.AnimationFinishTrigger();
        }

        public override void Die()
        {
            base.Die();
            StateMachine.ChangeState(DeadState);
        }

        #endregion
    }
}
