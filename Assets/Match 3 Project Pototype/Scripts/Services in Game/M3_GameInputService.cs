using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.M3
{
    public class M3_GameInputService : BaseService
    {
        private bool consumeInput = true;

        public bool ConsumeInput { get { return consumeInput; } set { consumeInput = value; } }

        public override void Deinit()
        {

        }

        public override void Init()
        {

        }
    }
}