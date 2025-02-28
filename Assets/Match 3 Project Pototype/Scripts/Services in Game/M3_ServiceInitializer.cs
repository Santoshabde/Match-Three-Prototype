using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.M3
{
    public class M3_ServiceInitializer : ServiceBinder
    {
        protected override void BindAllServicesInGame()
        {
            //Bind all services
            var boardDataService = new M3_Service_BoardData();
            var boardMatcherService = new M3_Service_BoardMatcher();
            var gamePieceInputService = new M3_Service_GamePieceInput();
            var gameBoardStatusUpdaterService = new M3_Service_GameBoardStatusUpdater();
            var gameInputService = new M3_GameInputService();

            BindService(boardDataService);
            BindService(boardMatcherService);
            BindService(gamePieceInputService);
            BindService(gameBoardStatusUpdaterService);
            BindService(gameInputService);

            boardDataService.Init();
            boardMatcherService.Init();
            gamePieceInputService.Init();
            gameBoardStatusUpdaterService.Init();
            gameInputService.Init();
        }
    }
}