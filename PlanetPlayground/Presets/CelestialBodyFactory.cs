using Godot;
using Godot.NativeInterop;
using PlanetPlayground.Game.Simulation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlanetPlayground.Configuration.Presets;

namespace PlanetPlayground.Presets;

public static class CelestialBodyFactory
{
    private static readonly PackedScene CelestialBodyScene = GD.Load<PackedScene>($"res://Game/Simulation/CelestialBody.tscn");

    public static CelestialBody Create(CelestialBodyInitialization initData)
    {
        var body = Create(
            new Vector2(initData.PlacementX, initData.PlacementY),
            new Vector2(initData.VelocityX, initData.VelocityY),
            initData.Mass
        );
        SpriteGenerator generator = new(initData);
        using var sprite = generator.Generate();
        var spriteClass = body.FindChild(nameof(CelestialBodySprite)) as CelestialBodySprite;
        //Do we need to dispose of image?
        //Image image = Image.CreateEmpty(sprite.Width, sprite.Height, false, Image.Format.Rgba16);
        Image image = new ();
        var buffer = sprite.Data.ToArray();
        /*ArrayBufferWriter<byte[]> writer = new (buffer.Length);
        writer.*/
        var error = image.LoadPngFromBuffer(buffer);
        if(error == Error.Ok)
        {
            var texture = ImageTexture.CreateFromImage(image);
            texture.ResourceLocalToScene = true;
            spriteClass.Texture = texture;
        }
        return body;
    }

    private static CelestialBody Create(Vector2 postion, Vector2 velocity, float mass)
    {
        var body = Create();
        body.PhysicalPosition = postion;
        body.Velocity = velocity;
        body.Mass = mass;
        return body;
    }

    private static CelestialBody Create()
    {
        return CelestialBodyScene.Instantiate<CelestialBody>(); ;
    }
}
