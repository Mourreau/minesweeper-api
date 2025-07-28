using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core.Settings
{
    public static class DifficultyPresets
    {
        public static readonly Dictionary<string, (int Width, int Height, int Mines)> Presets = new Dictionary<string, (int, int, int)>
        {
            { "Easy", (9, 9, 10) },
            { "Medium", (16, 16, 40) },
            { "Hard", (30, 16, 99) },
            { "Custom", (0, 0, 0) } // Custom settings can be defined by the user
        };
        public static (int Width, int Height, int Mines) GetPreset(string difficulty)
        {
            if (Presets.TryGetValue(difficulty, out var preset))
            {
                return preset;
            }
            throw new ArgumentException("Invalid difficulty level.", nameof(difficulty));
        }
    }
}
