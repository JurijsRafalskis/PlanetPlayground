using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanetPlayground.Game.Simulation;
using PlanetPlayground.Presets;
using System;
using System.Collections.Generic;
using System.IO;
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
            Mass = 1
        };
        SpriteGenerator generator = new(initialization);
        using var generatedSprite = generator.Generate();
        Assert.IsNotNull(generatedSprite);
        Assert.IsTrue(generatedSprite.CanSeek);
        using var output = new FileStream($"{Environment.CurrentDirectory}/../../../TemporaryTestResults/Should_be_able_to_generate_sprite.png", FileMode.Create);
        generatedSprite.CopyTo(output);
    }

    [TestMethod]
    public void Same_seed_should_lead_to_generation_of_the_same_sprite() 
    {
        CelestialBodyInitialization initialization = new()
        {
            Mass = 1
        };
        SpriteGenerator generator = new(initialization);
        using var generatedSprite1 = generator.Generate();
        Assert.IsNotNull(generatedSprite1);
        var reader1 = new StreamReader(generatedSprite1);
        using var generatedSprite2 = generator.Generate();
        Assert.IsNotNull(generatedSprite2);
        var reader2 = new StreamReader(generatedSprite2);
        Assert.AreEqual(reader1.ReadToEnd(), reader2.ReadToEnd());
    }
}