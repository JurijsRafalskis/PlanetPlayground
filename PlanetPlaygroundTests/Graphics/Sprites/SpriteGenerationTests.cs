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
    public void Should_be_able_to_generate_terrestrial_sprite()
    {
        CelestialBodyInitialization init = new ()
        {
            Mass = 1
        };

        SpriteGenerator generator = new(init);
        using var generatedSprite = generator.Generate();
        Assert.IsNotNull(generatedSprite);
        Assert.IsTrue(generatedSprite.CanSeek);
        using var output = new FileStream($"{Environment.CurrentDirectory}/../../../TemporaryTestResults/{nameof(Should_be_able_to_generate_terrestrial_sprite)}.png", FileMode.Create);
        generatedSprite.CopyTo(output);
    }

    [TestMethod]
    public void Should_be_able_to_generate_gas_giant_sprite()
    {
        CelestialBodyInitialization init = new()
        {
            Mass = 8
        };

        SpriteGenerator generator = new(init);
        using var generatedSprite = generator.Generate();
        Assert.IsNotNull(generatedSprite);
        Assert.IsTrue(generatedSprite.CanSeek);
        using var output = new FileStream($"{Environment.CurrentDirectory}/../../../TemporaryTestResults/{nameof(Should_be_able_to_generate_gas_giant_sprite)}.png", FileMode.Create);
        generatedSprite.CopyTo(output);
    }

    [TestMethod]
    public void Should_be_able_to_generate_star_sprite()
    {
        CelestialBodyInitialization init = new()
        {
            Mass = 25
        };

        SpriteGenerator generator = new(init);
        using var generatedSprite = generator.Generate();
        Assert.IsNotNull(generatedSprite);
        Assert.IsTrue(generatedSprite.CanSeek);
        using var output = new FileStream($"{Environment.CurrentDirectory}/../../../TemporaryTestResults/{nameof(Should_be_able_to_generate_star_sprite)}.png", FileMode.Create);
        generatedSprite.CopyTo(output);
    }

    [TestMethod]
    public void Should_throw_on_ivalid_input()
    {
        Assert.Throws<ArgumentNullException>(() => new SpriteGenerator(null));
        CelestialBodyInitialization invalidInitData1 = new()
        {
            Mass = 0,
        };
        Assert.Throws<ArgumentOutOfRangeException>(() => new SpriteGenerator(invalidInitData1));
        CelestialBodyInitialization invalidInitData2 = new()
        {
            Mass = 0,
        };
        Assert.Throws<ArgumentOutOfRangeException>(() => new SpriteGenerator(invalidInitData2));
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

    [TestMethod]
    public void Should_be_able_to_handle_a_range_of_masses()
    {
        List<CelestialBodyInitialization> list = new() {
            new(){
                Mass = 1
            },
            new(){
                Mass = 2.5f
            },
            new(){
                Mass = 5
            }
        };

        foreach(var initItem in list)
        {
            SpriteGenerator generator = new(initItem);
            using var generatedSprite = generator.Generate();
            Assert.IsNotNull(generatedSprite);
            Assert.IsTrue(generatedSprite.CanSeek);
        }
    }

    [TestMethod]
    public void Should_be_able_to_handle_a_range_of_seeds()
    {
        List<CelestialBodyInitialization> list = new() {
            new(){
                Mass = 1,
                SpriteSeed = "Default"
            },
            new(){
                Mass = 1,
                SpriteSeed = "This is a long not really random seed"
            },
            new(){
                SpriteSeed = $"{Random.Shared.Next()}",
                Mass = 1
            },
            new()
            {
                Mass = 1,
            }
        };

        foreach (var initItem in list)
        {
            SpriteGenerator generator = new(initItem);
            using var generatedSprite = generator.Generate();
            Assert.IsNotNull(generatedSprite);
            Assert.IsTrue(generatedSprite.CanSeek);
        }
    }

    [TestMethod]
    public void Should_be_able_to_handle_full_range_of_masses()
    {
        for(int i = 1; i <= 150; i++)
        {
            CelestialBodyInitialization initialization = new()
            {
                Mass = 1
            };

            SpriteGenerator generator = new(initialization);
            using var generatedSprite = generator.Generate();
            Assert.IsNotNull(generatedSprite);
            Assert.IsTrue(generatedSprite.CanSeek);
        }
    }
}