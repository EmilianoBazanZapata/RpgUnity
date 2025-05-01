using Game.Player.Scripts.Controllers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Player.Scripts.Skills
{
    public class SwordSkill : Skill
    {
        [Header("Skill info")] [SerializeField]
        private GameObject _swordPrefab;

        [SerializeField] private Vector2 _launchForce;
        [SerializeField] private float _swordGravity = 0;
        [SerializeField] private float _hitCooldown = .35f;
        [SerializeField] private float _freezeTimeDuration = .7f;
        [SerializeField] private float _returnSpeed = 12f;

        [Header("Aim dots")] [SerializeField] private int _numberOfDots;
        [SerializeField] private float _spaceBeetwenDots;
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private Transform _dotsParent;
        [SerializeField] private Transform _SpawnSwordPosition;

        private Vector2 _finalDirection;
        private GameObject[] dots;
        
        protected override void Start()
        {
            base.Start();
            GenerateDots();
        }

        protected override void Update()
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
                _finalDirection = new Vector2(AimDirection().normalized.x * _launchForce.x,
                    AimDirection().normalized.y * _launchForce.y);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPosition(i * _spaceBeetwenDots);
                }
            }
        }

        public void CreateSword()
        {
            var newSword = Instantiate(_swordPrefab, _SpawnSwordPosition.position, transform.rotation);

            var newSwordScript = newSword.GetComponent<SwordSkillController>();
            
            newSwordScript.SetupSword(_finalDirection, _swordGravity, Player, _freezeTimeDuration, _returnSpeed);

            Player.AssignNewSword(newSword);

            DotsActive(false);
        }

        public Vector2 AimDirection()
        {
            var playerPosition = Player.transform.position;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - playerPosition;

            return direction;
        }

        public void DotsActive(bool isActive)
        {
            foreach (var dot in dots)
            {
                dot.SetActive(isActive);
            }
        }

        private void GenerateDots()
        {
            dots = new GameObject[_numberOfDots];
            for (int i = 0; i < _numberOfDots; i++)
            {
                dots[i] = Instantiate(_dotPrefab, Player.transform.position, Quaternion.identity, _dotsParent);
                dots[i].SetActive(false);
            }
        }

        private Vector2 DotsPosition(float t)
        {
            var position = (Vector2)Player.transform.position + new Vector2(
                AimDirection().normalized.x * _launchForce.x,
                AimDirection().normalized.y * _launchForce.y) * t + .5f * (Physics2D.gravity * _swordGravity) * (t * t);

            return position;
        }
    }
}