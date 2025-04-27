namespace Game.Player.Scripts.Inputs
{
    /// <summary>
    /// Separa completamente la captura de inputs del motor Unity (Input.GetAxisRaw).
    /// ✅ Si mañana se cambia a un nuevo sistema de inputs (como Input System de Unity), solo se debe modificar PlayerInputHandler,
    /// no todos los estados
    /// </summary>
    public class PlayerInputHandler
    {
        public float XInput => UnityEngine.Input.GetAxisRaw("Horizontal");
        public float YInput => UnityEngine.Input.GetAxisRaw("Vertical");
    }
}