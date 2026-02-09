using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Configuration;

public static class Presets
{
    public record Preset
    {
        public Preset() { }
        public Preset(string Label, string PresetName)
        {
            this.Label = Label;
            this.PresetName = PresetName;
        }
        public string Label { get; set; }
        public string PresetName { get; set; }
    }

    public static readonly IReadOnlyCollection<Preset> ExistingPresets = new List<Preset>()
    {
        new ("Two bodies", "TwoBodies"),
        new ("Three bodies", "ThreeBodies"),
        new ("Star and body", "SimpleStar"),
        new ("Star and three bodies", "StarThree")
    };
}
