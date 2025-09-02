using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core.Enums
{
    public enum GameStatus
    {
        Created, // Игра создана, но не начата
        InProgress, // Игра в процессе
        Paused, // Игра приостановлена
        Won, // Игра выиграна
        Lost // Игра проиграна
    }
}
