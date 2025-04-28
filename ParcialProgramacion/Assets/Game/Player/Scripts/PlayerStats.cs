using Game.Shared.Scripts;

namespace Game.Player.Scripts
{
    public class PlayerStats: CharacterStats
    {
        private Player _player;
    
        protected override void Start()
        {
            base.Start();
        
            _player = GetComponent<Player>();
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
        
            GetComponent<PlayerItemDrop>()?.GenerateDrop();
        }
    }
}