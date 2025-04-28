using Game.Enemies;
using Game.Player.Scripts.Managers;
using UnityEngine;

namespace Game.Player.Scripts
{
    public class Skill: MonoBehaviour
    {
        [SerializeField] protected float coolDown;
        protected float CoolDownTimer;

        protected Player Player;
        protected virtual void Start()
        {
            Player = PlayerManager.Instance.Player; 
        }

        protected virtual void Update()
        {
            CoolDownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if (!(CoolDownTimer < 0)) return false;
        
            UseSkill();
            CoolDownTimer = coolDown;
            return true;
        }

        public virtual void UseSkill()
        {
        
        }

        protected virtual Transform FindClosestEnemy(Transform checkTransform)
        {
            var colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);

            var clossetDistance = Mathf.Infinity;

            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() == null) continue;
            
                var distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (!(distanceToEnemy < clossetDistance)) continue;
            
                clossetDistance = distanceToEnemy;
                closestEnemy = hit.transform;
            }

            return closestEnemy;
        }
    }
}