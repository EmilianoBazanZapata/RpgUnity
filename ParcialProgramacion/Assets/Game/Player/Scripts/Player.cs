using Game.Player.Scripts.Inputs;
using Game.Shared.Scripts;

namespace Game.Player.Scripts
{
    public class Player : Entity
    {
        public PlayerInputHandler InputHandler { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler = new PlayerInputHandler();
        }
    }
}