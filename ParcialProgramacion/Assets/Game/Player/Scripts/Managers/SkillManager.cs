using Game.Player.Scripts.Skills;
using UnityEngine;

namespace Game.Player.Scripts.Managers
{
    public class SkillManager: MonoBehaviour
    {
        public static SkillManager Instance;

        public DashSkill DashSkill { get; private set; }
        public CrystalSkill CrystalSkill { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            DashSkill = GetComponent<DashSkill>();
            CrystalSkill = GetComponent<CrystalSkill>();
        }
    }
}