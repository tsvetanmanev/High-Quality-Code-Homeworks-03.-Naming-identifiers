namespace Minesweeper
{
    public class ScoreboardEntry
    {
        private string name;
        private int points;

        public ScoreboardEntry(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public ScoreboardEntry()
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }
    }
}
