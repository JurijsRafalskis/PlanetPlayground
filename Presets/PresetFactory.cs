using Godot;
using PlanetPlayground.Extensions;
using PlanetPlayground.Game.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlanetPlayground.Presets;

public static class PresetFactory
{
    public static IEnumerable<CelestialBody> CreateFromPreset(Space space, string presetName)
    {
        //Do awaiting here?
        using var file = FileAccess.Open($"res://Presets/PresetData/{presetName}.json", FileAccess.ModeFlags.Read);
        var presetValues = JsonSerializer.Deserialize<List<CelestialBodyInitialization>>(file.GetAsText());
        var viewPort = space.GetViewportRect();
        var width = viewPort.GetWidth();
        var height = viewPort.GetHeight();

        List<CelestialBody> result = new();
        foreach (var preset in presetValues)
        {
            result.Add(CelestialBodyFactory.Create(
                new Vector2(preset.PlacementX, preset.PlacementY),
                new Vector2(preset.VelocityX, preset.VelocityY),
                preset.Mass
            ));
        }

        return result;
    }
}
