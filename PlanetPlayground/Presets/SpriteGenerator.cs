using PlanetPlayground.Game.Simulation;
using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;

namespace PlanetPlayground.Presets;

public class SpriteGenerator
{
    private Random Random { get; set; }
    public string Seed { get; }
    public float Mass { get; }

    public SpriteGenerator(CelestialBodyInitialization initialData)
    {
        Seed = initialData.SpriteSeed ?? Guid.NewGuid().ToString();
        Mass = initialData.Mass > 0 ? initialData.Mass : throw new ArgumentOutOfRangeException(nameof(initialData), $"{nameof(initialData.Mass)} is either 0 or negative.");
    }

    /// <summary>
    /// Generates random png sprite from provided seed.
    /// </summary>
    /// <returns>Stream of created sprite</returns>
    public Stream Generate()
    {
        //TODO: find out how to do generation outside of Windows, so target could be more global...

        //Seeding random here, to ensure same results for same seed.
        Random = new(Seed.GetHashCode());
        if (Mass < 5)
        {
            return DrawTerrestrial();
        }
        if (Mass < 20)
        {
            return DrawGas();
        }
        else
        {
            return DrawStar();
        }
    }

    private Stream DrawTerrestrial()
    {
        //Allows sizes between 10 and 20 for mass values from 1 to 5;
        int size =  Convert.ToInt32(Math.Floor(10 + 6.21 * Math.Log(Mass)));
        using var bmp = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bmp);
        Color color = (Random.NextInt64() % 3) switch
        {
            1 => Color.MediumVioletRed,
            2 => Color.ForestGreen,
            3 => Color.AliceBlue,
            _ => Color.Wheat // Imposible Fallback
        };
        Pen pen = new(color);
        graphics.DrawEllipse(pen, 0, 0, size - 1, size - 1);
        //Try texture brush?
        SolidBrush brush = new(color);
        graphics.FillClosedCurve(brush, [new(0, 0)]);
        MemoryStream stream = new();
        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private Stream DrawGas()
    {
        using var bmp = new Bitmap(20, 20);
        using var graphics = Graphics.FromImage(bmp);

        return Stream.Null;
    }

    private Stream DrawStar()
    {
        using var bmp = new Bitmap(20, 20);
        using var graphics = Graphics.FromImage(bmp);

        return Stream.Null;
    }
}
