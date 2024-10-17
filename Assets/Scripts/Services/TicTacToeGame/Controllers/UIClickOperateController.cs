namespace TTT.TicTacToeGame
{
    public class UIClickOperateController : OperateControllerBase
    {

        public override void OnInit()
        {
            TicTacToeGameService.OnPieceUIClickEvent.AddListener(_OnClickPiece);
        }

        public override void Dispose()
        {
            TicTacToeGameService.OnPieceUIClickEvent.RemoveListener(_OnClickPiece);
        }

        private void _OnClickPiece(int id)
        {
            if (this._operateCallback == null)
            {
                return;
            }
            this._operateCallback.Invoke(id);
        }
    }
}