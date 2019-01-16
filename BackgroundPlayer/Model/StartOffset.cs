namespace BackgroundPlayer.Model
{
    public class StartOffset
    {
        public int? Month { get; set; }

        public int? Day { get; set; }

        public int? Hour { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StartOffset other && Equals(other);
        }

        protected bool Equals(StartOffset other)
        {
            return Month == other.Month && Day == other.Day && Hour == other.Hour;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Month.GetHashCode();
                hashCode = (hashCode * 397) ^ Day.GetHashCode();
                hashCode = (hashCode * 397) ^ Hour.GetHashCode();
                return hashCode;
            }
        }
    }
}