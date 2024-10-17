using System;
using TTT.Test;

namespace TTT
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public class Startup : SingletonMonoBehaviour<Startup>
    {
        void Start()
        {
            _InitServices();
            _StartGame();
        }

        private void _InitServices()
        {
            _RegisterService(TicTacToeGame.TicTacToeGameService.Instance);
        }
        
        private void _StartGame()
        {
            UIManager.Init();
            TicTacToeGame.TicTacToeGameService.StartGame();
        }

        private void _RegisterService(IService iService)
        {
            if (iService == null)
            {
                return;
            }
            iService.OnInit();
        }

        
    }
}
