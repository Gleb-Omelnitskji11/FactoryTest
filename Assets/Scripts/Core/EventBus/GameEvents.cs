using ConfigData;

namespace Core.BusEvents
{
    public class GameResultEvent : IEvent
    {
        public readonly bool IsWin;

        public GameResultEvent(bool isWin)
        {
            IsWin = isWin;
        }
    }
    
    public class RestartEvent : IEvent
    {
        public readonly bool IsFirstGame;

        public RestartEvent(bool isFirstGame = false)
        {
            IsFirstGame = isFirstGame;
        }
    }
    
    public class PauseEvent : IEvent
    {
        public readonly bool IsPause;

        public PauseEvent(bool isPause)
        {
            IsPause = isPause;
        }
    }

    public class EnemyDiedEvent : IEvent
    {
        public readonly EnemyUnitModel EnemyModel;

        public EnemyDiedEvent(EnemyUnitModel enemyModel)
        {
            EnemyModel = enemyModel;
        }
    }
}
