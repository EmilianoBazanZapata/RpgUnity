using System.Collections.Generic;
using Game.Player.Scripts.Controllers;
using UnityEngine;

namespace Game.Player.Scripts.Skills
{
    public class CrystalSkill : Skill
    {
        [Header("Crystal Base Settings")] [SerializeField]
        private GameObject _crystalPrefab;

        [SerializeField] private float _crystalDuration;
        private GameObject _currentCrystal;

        [Header("Crystal Types")] [SerializeField]
        private bool _cloneInsteadOfCrystal; // Crystal mirage

        [SerializeField] private bool _canExplode; // Explosive crystal
        [SerializeField] private bool _canMoveToEnemy; // Moving crystal
        [SerializeField] private float _moveSpeed;

        [Header("Multi Stack Settings")] [SerializeField]
        private bool _canUseMultiStack;

        [SerializeField] private int _amountOfStacks;
        [SerializeField] private float _multiStackCooldown;
        [SerializeField] private float _useTimeWindow;
        [SerializeField] private List<GameObject> _crystalLefth = new List<GameObject>();

        public override void UseSkill()
        {
            base.UseSkill();

            if (CanUseMultiCrystal())
                return;

            HandleCrystalCreationOrTeleport();
        }

        private void HandleCrystalCreationOrTeleport()
        {
            if (_currentCrystal == null)
            {
                CreateCrystal();
                return;
            }

            if (_canMoveToEnemy)
                return;

            TeleportPlayerToCrystal();
            HandleCrystalEffect();
        }

        private void TeleportPlayerToCrystal()
        {
            var playerPos = Player.transform.position;
            Player.transform.position = playerPos;
            Player.transform.position = _currentCrystal.transform.position;
        }

        private void HandleCrystalEffect()
        {
            _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
        }

        public void CreateCrystal()
        {
            _currentCrystal = Instantiate(_crystalPrefab, Player.transform.position, Quaternion.identity);
            SetupCrystalController();
        }

        private void SetupCrystalController()
        {
            var currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();

            if (currentCrystalScript == null) return;

            currentCrystalScript.SetupCrystal(
                _crystalDuration,
                _canExplode,
                _canMoveToEnemy,
                _moveSpeed,
                FindClosestEnemy(_currentCrystal.transform),
                Player
            );
        }

        private bool CanUseMultiCrystal()
        {
            if (!_canUseMultiStack || _crystalLefth.Count <= 0)
                return false;

            return HandleMultiCrystalLogic();
        }

        private bool HandleMultiCrystalLogic()
        {
            if (_crystalLefth.Count == _amountOfStacks)
                Invoke(nameof(ResetAbility), _useTimeWindow);

            coolDown = 0;
            SpawnStackedCrystal();

            if (_crystalLefth.Count <= 0)
            {
                coolDown = _multiStackCooldown;
                RefilCrystal();
            }

            return true;
        }

        private void SpawnStackedCrystal()
        {
            var crystalToSpawn = _crystalLefth[_crystalLefth.Count - 1];
            var newCrystal = Instantiate(crystalToSpawn, Player.transform.position, Quaternion.identity);
            _crystalLefth.Remove(crystalToSpawn);

            newCrystal.GetComponent<CrystalSkillController>()?.SetupCrystal(
                _crystalDuration,
                _canExplode,
                _canMoveToEnemy,
                _moveSpeed,
                FindClosestEnemy(newCrystal.transform),
                Player
            );
        }

        private void RefilCrystal()
        {
            var amountAdd = _amountOfStacks - _crystalLefth.Count;
            for (int i = 0; i < amountAdd; i++)
            {
                _crystalLefth.Add(_crystalPrefab);
            }
        }

        private void ResetAbility()
        {
            if (CoolDownTimer > 0)
                return;

            CoolDownTimer = _multiStackCooldown;
            RefilCrystal();
        }
    }
}