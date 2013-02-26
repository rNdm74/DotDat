using System;
using System.Collections.Generic;

namespace Power_Options.Classes
{
    public struct Power
    {
        public static List<Scheme> Schemes;
        
        public static Scheme CreateScheme(string name, Guid guid, string toolTip)
        {
            return new Scheme(name, guid, toolTip);
        }
    }

    public class Scheme
    {
        public Guid Guid;
        public string Name;
        public string Tooltip;

        public Scheme(string name, Guid guid, string toolTip)
        {
            Guid = guid;
            Name = name;
            Tooltip = toolTip;
        }
    }
}