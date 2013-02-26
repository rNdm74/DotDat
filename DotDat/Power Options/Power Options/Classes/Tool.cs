using System;

namespace Power_Options.Classes
{
    public class Tool
    {
        private enum Scheme
        {
            Balanced,
            PowerSaver,
            HighPerformance,
            Custom,
        }

        private struct Guids
        {
            public static readonly Guid Balanced = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e");
            public static readonly Guid HighPerformance = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
            public static readonly Guid PowerSaver = new Guid("a1841308-3541-4fab-bc81-f71556f20b4a");
        }

        private struct Tips
        {
            public const string Balanced = "Automatically balances performance\n" + "with energy consumption on capable\nhardware.";

            public const string PowerSaver = "Saves energy by reducing your\n" + "computer's performance where\npossible.";

            public const string HighPerformance = "Favor performance, but may use\n" + "more energy.";

            public const string Custom = "Custom power scheme.";
        }

        private static Scheme Schemes(Guid schemeGuid)
        {
            if (schemeGuid == Guids.Balanced) return Scheme.Balanced;

            if (schemeGuid == Guids.HighPerformance) return Scheme.HighPerformance;

            return schemeGuid == Guids.PowerSaver ? Scheme.PowerSaver : Scheme.Custom;
        }

        public static string Tip(Guid schemeGuid)
        {
            switch (Schemes(schemeGuid))
            {
                case Scheme.Balanced:
                    return Tips.Balanced;
                case Scheme.PowerSaver:
                    return Tips.PowerSaver;
                case Scheme.HighPerformance:
                    return Tips.HighPerformance;
                default:
                    return Tips.Custom;
            }
        }
    }
}