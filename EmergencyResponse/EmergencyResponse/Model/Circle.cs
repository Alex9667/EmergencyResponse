namespace EmergencyResponse.Model
{
    public class Circle
    {
        public Address Center { get; private set; }
        public double Radius { get; private set; } = 10; // Hardcoded radius 10 

        public Circle(Address center)
        {
            if (center == null)
            {
                throw new ArgumentNullException(nameof(center), "Center address cannot be null.");
            }

            if (!center.Latitude.HasValue || !center.Longitude.HasValue)
            {
                throw new InvalidOperationException("Address must have both latitude and longitude set.");
            }

            Center = center;
        }
    }
}
