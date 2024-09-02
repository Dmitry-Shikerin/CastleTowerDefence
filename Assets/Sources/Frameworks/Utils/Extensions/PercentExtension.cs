using Sources.Frameworks.Domain.Implementation.Constants;

namespace Sources.Frameworks.Utils.Extensions
{
    public static class PercentExtension
    {
        public static float FloatToPercent(this float value, float max)
        {
            float percent = max / 100f;
            int currentPercents = 0;
            float currentHealth = 0;

            while (currentHealth < value)
            {
                currentHealth += percent;
                currentPercents++;
            }

            return currentPercents;
        }
        
        public static int IntToPercent(this int value, int max) =>
            (int)FloatToPercent((int)value, (int)max);

        public static float FloatPercentToUnitPercent(this float value) =>
            value * MathConst.UnitMultiplier;
        
        public static float IntPercentToUnitPercent(this int value) =>
            value * MathConst.UnitMultiplier;
    }
}