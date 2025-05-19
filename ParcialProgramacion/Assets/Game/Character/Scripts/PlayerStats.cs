using Game.Shared.Scripts;

namespace Game.Character.Scripts
{
    public class PlayerStats: CharacterStats
    {
        private Character.Scripts.Player _player;
    
        protected override void Start()
        {
            base.Start();
        
            _player = GetComponent<Character.Scripts.Player>();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        
            _player.DamageImpact();
        }

        protected override void Die()
        {
            base.Die();
        
            _player.Die();
        }
    }
}