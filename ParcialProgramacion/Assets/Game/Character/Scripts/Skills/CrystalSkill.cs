using System.Collections.Generic;
using Game.Character.Scripts.Controllers;
using UnityEngine;

namespace Game.Character.Scripts.Skills
{
    /// <summary>
    /// Habilidad que crea un cristal con distintos efectos (explosión, movimiento, clonación).
    /// También soporta un sistema de múltiples cristales en stack.
    /// </summary>
    public class CrystalSkill : Skill
    {
        [Header("Configuración base del cristal")]
        [SerializeField] private GameObject _crystalPrefab;
        [SerializeField] private float _crystalDuration;

        private GameObject _currentCrystal;

        [Header("Tipos de cristal")]
        [SerializeField] private bool _cloneInsteadOfCrystal;
        [SerializeField] private bool _canExplode = true;
        [SerializeField] private bool _canMoveToEnemy = true;
        [SerializeField] private float _moveSpeed;

        [Header("Configuración multistack")]
        [SerializeField] private bool _canUseMultiStack;
        [SerializeField] private int _amountOfStacks;
        [SerializeField] private float _multiStackCooldown;
        [SerializeField] private float _useTimeWindow;
        [SerializeField] private List<GameObject> _availableCrystals = new();

        public override void UseSkill()
        {
            base.UseSkill();

            if (CanUseMultiCrystal())
                return;

            HandleCrystalCreationOrTeleport();
        }

        /// <summary>
        /// Lógica principal para crear un cristal o teletransportarse si ya existe uno.
        /// </summary>
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

        /// <summary>
        /// Teletransporta al jugador a la posición del cristal actual.
        /// </summary>
        private void TeleportPlayerToCrystal()
        {
            Player.Instance.transform.position = _currentCrystal.transform.position;
        }

        /// <summary>
        /// Ejecuta el efecto final del cristal (como explosión).
        /// </summary>
        private void HandleCrystalEffect()
        {
            if (_currentCrystal.TryGetComponent(out CrystalSkillController controller))
                controller.FinishCrystal();
        }

        /// <summary>
        /// Crea un nuevo cristal en la posición actual del jugador.
        /// </summary>
        public void CreateCrystal()
        {
            _currentCrystal = Instantiate(_crystalPrefab, Player.Instance.transform.position, Quaternion.identity);
            SetupCrystalController(_currentCrystal);
        }

        /// <summary>
        /// Configura el controlador del cristal con los parámetros definidos.
        /// </summary>
        private void SetupCrystalController(GameObject crystal)
        {
            if (!crystal.TryGetComponent(out CrystalSkillController controller))
                return;

            controller.SetupCrystal(
                _crystalDuration,
                _canExplode,
                _canMoveToEnemy,
                _moveSpeed,
                FindClosestEnemy(crystal.transform),
                Player.Instance
            );
        }

        /// <summary>
        /// Verifica si se puede usar una pila de cristales.
        /// </summary>
        private bool CanUseMultiCrystal()
        {
            if (!_canUseMultiStack || _availableCrystals.Count <= 0)
                return false;

            return HandleMultiCrystalLogic();
        }

        /// <summary>
        /// Maneja la lógica de spawn de múltiples cristales.
        /// </summary>
        private bool HandleMultiCrystalLogic()
        {
            if (_availableCrystals.Count == _amountOfStacks)
                Invoke(nameof(ResetAbility), _useTimeWindow);

            Cooldown = 0;
            SpawnStackedCrystal();

            if (_availableCrystals.Count > 0) return true;
            Cooldown = _multiStackCooldown;
            RefillCrystals();

            return true;
        }

        /// <summary>
        /// Instancia un cristal desde el stack y lo configura.
        /// </summary>
        private void SpawnStackedCrystal()
        {
            var prefab = _availableCrystals[^1];
            var newCrystal = Instantiate(prefab, Player.Instance.transform.position, Quaternion.identity);
            _availableCrystals.RemoveAt(_availableCrystals.Count - 1);

            SetupCrystalController(newCrystal);
        }

        /// <summary>
        /// Rellena la pila de cristales disponibles al máximo.
        /// </summary>
        private void RefillCrystals()
        {
            int refillAmount = _amountOfStacks - _availableCrystals.Count;
            for (int i = 0; i < refillAmount; i++)
                _availableCrystals.Add(_crystalPrefab);
        }

        /// <summary>
        /// Reinicia la habilidad si la ventana de uso múltiple terminó.
        /// </summary>
        private void ResetAbility()
        {
            if (CooldownTimer > 0)
                return;

            CooldownTimer = _multiStackCooldown;
            RefillCrystals();
        }
    }
}
