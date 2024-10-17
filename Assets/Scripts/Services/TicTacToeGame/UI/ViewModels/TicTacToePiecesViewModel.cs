using UnityEngine.Events;

namespace TTT.TicTacToeGame
{
    public class TicTacToePiecesViewModel : ViewModelBase
    {
        public Observer showXPieceGoProperty = new Observer(false);
        public Observer showOPieceGoProperty = new Observer(false);

        private int _id;
        private UnityAction<int> _clickCallback;
        
        public override void OnInit()
        {
            showXPieceGoProperty.value = false;
            showOPieceGoProperty.value = false;
        }

        public TicTacToePiecesViewModel(int id, UnityAction<int> clickCallback)
        {
            this._id = id;
            this._clickCallback = clickCallback;
            ResetView();
        }

        public void ResetView()
        {
            var curType = TicTacToeGameMgr.GetPiecesTypeById(this._id);
            showXPieceGoProperty.value = curType == TicTacToePiecesType.X;
            showOPieceGoProperty.value = curType == TicTacToePiecesType.O;
        }

        public void _OnClick()
        {
            if (_clickCallback == null)
            {
                return;
            }
            _clickCallback.Invoke(_id);
        }
    }
}