using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class PlayerController
    {
        private TicTacToePiecesType _operatePiecesType;
        private OperateControllerType _operateControllerType;
        private OperateControllerBase _operateController;

        private bool _isCurOperational = false;

        public PlayerController(TicTacToePiecesType operatePiecesType, OperateControllerType operateControllerType)
        {
            InitData(operatePiecesType, operateControllerType);
        }

        private void InitData(TicTacToePiecesType operatePiecesType, OperateControllerType operateControllerType)
        {
            this._operatePiecesType = operatePiecesType;
            this._operateControllerType = operateControllerType;
            var operateController = _CreateOperateController(operateControllerType);
            if (operateController != null)
            {
                operateController.InitData(this, TryOperatePiece);
            }
            this._operateController = operateController;
        }

        public void Dispose()
        {
            if (_operateController != null)
            {
                _operateController.Dispose();
                _operateController = null;
            }
        }
        
        public TicTacToePiecesType GetOperatePiecesType()
        {
            return _operatePiecesType;
        }

        public OperateControllerType GetOperateControllerType()
        {
            return _operateControllerType;
        }

        public bool GetIsCurOperational()
        {
            return _isCurOperational;
        }

        public void SetIsCurOperational(bool isCurOperational)
        {
            bool isChange = _isCurOperational != isCurOperational;
            _isCurOperational = isCurOperational;
        }

        public void StartOperate()
        {
            if (_operateController != null)
            {
                _operateController.StartOperate();
            }
        }

        private void TryOperatePiece(int id)
        {
            if (!GetIsCurOperational())
            {
                return;
            }
            TicTacToeGameMgr.TryOperatePieceById(id, this._operatePiecesType);
        }

        private OperateControllerBase _CreateOperateController(OperateControllerType operateControllerType)
        {
            OperateControllerBase result = null;
            switch (operateControllerType)
            {
                case OperateControllerType.UIClick :
                    result = new UIClickOperateController();
                    break;
                case OperateControllerType.AIMiniMax :
                    result = new AIMiniMaxOperateController();
                    break;
                default:
                    Debug.LogError($"传入不支持操作控制器类型:{operateControllerType}");
                    break;
            };
            return result;
        }
    }
}