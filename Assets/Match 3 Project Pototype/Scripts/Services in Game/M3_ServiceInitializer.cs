using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.M3
{
    public class M3_ServiceInitializer : ServiceBinder
    {
        protected override void BindAllServicesInGame()
        {
            //Bind all services
            BindService(new M3_Service_BoardData());
            BindService(new M3_Service_BoardMatcher());
        }
    }
}