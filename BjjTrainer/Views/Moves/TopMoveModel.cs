
namespace BjjTrainer.Views.Moves
{
    public class TopMovesModel
    {
        public string MoveName { get; set; } = string.Empty;
        public double UsagePercentage { get; set; } = 0.0;
        public Color? Color { get; internal set; }
    }

}
