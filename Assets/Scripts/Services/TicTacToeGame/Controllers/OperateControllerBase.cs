using UnityEngine.Events;

namespace TTT.TicTacToeGame
{
    public class OperateControllerBase
    {
        protected PlayerController _playerCtrl;
        protected UnityAction<int> _operateCallback;

        public void InitData(PlayerController playerController, UnityAction<int> operateCallback)
        {
            this._playerCtrl = playerController;
            this._operateCallback = operateCallback;
            OnInit();
        }

        public virtual void OnInit()
        {
            
        }
        
        public virtual void Dispose()
        {
            
        }
    }
}