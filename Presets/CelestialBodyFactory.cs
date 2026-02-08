using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Presets;

public static class CelestialBodyFactory
{
    private static readonly PackedScene CelestialBodyScene = GD.Load<PackedScene>($"res://Game/Simulation/CelestialBody.tscn");

    public static CelestialBody Create(Vector2 postion, Vector2 velocity, float mass)
    {
        var body = Create();
        body.Position = postion;
        body.Velocity = velocity;
        body.Mass = mass;
        return body;
    }

    public static CelestialBody Create()
    {
        return CelestialBodyScene.Instantiate<CelestialBody>(); ;
    }
}
