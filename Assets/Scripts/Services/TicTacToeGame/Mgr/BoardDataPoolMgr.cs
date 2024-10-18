using UnityEngine.Pool;

namespace TTT.TicTacToeGame
{
    public class BoardDataPoolMgr : SingletonBase<BoardDataPoolMgr>
    {
        private ObjectPool<TicTacToeBoardData> _boardDataPool = new ObjectPool<TicTacToeBoardData>();

        public static TicTacToeBoardData GetBoardData()
        {
            var result = Instance._boardDataPool.GetObject();
            result.InitData();
            return result;
        }

        public static void ReleaseBoardData(TicTacToeBoardData boardData)
        {
             Instance._boardDataPool.ReleaseObject(boardData);
        }

        public static TicTacToeBoardData CopyBoardData(TicTacToeBoardData originBoardData)
        {
            var result = GetBoardData();
            if (originBoardData == null)
            {
                return result;
            }
            result.CopyData(originBoardData);
            return result;
        }

    }
}