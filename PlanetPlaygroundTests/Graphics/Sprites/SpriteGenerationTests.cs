using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanetPlayground.Game.Simulation;
using PlanetPlayground.Presets;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanetPlaygroundTests.Graphics.Sprites;

[TestClass]
public class SpriteGenerationTests
{
    [TestMethod]
    public void Should_be_able_to_generate_sprite()
    {
        CelestialBodyInitialization initialization = new()
        {
            Mass = 1,
            PlacementX = 0,
            PlacementY = 0,
            VelocityX = 0,
            VelocityY = 0
        };
        SpriteGenerator generator = new(initialization);
        using var generatedSprite = generator.Generate();
        Assert.IsNotNull(generatedSprite);
    }
}