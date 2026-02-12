using PlanetPlayground.Game.Simulation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace PlanetPlayground.Presets;

public class SpriteGenerator
{
    private Random Random { get; set; }
    public string Seed { get; }
    public float Mass { get; }

    public SpriteGenerator(CelestialBodyInitialization initialData)
    {
        if (initialData == null) throw new ArgumentNullException(nameof(initialData));
        Mass = initialData.Mass > 0 ? initialData.Mass : throw new ArgumentOutOfRangeException(nameof(initialData), $"{nameof(initialData.Mass)} is either 0 or negative.");
        Seed = initialData.SpriteSeed ?? Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Generates random png sprite from provided seed.
    /// </summary>
    /// <returns>Stream of created sprite</returns>
    public GeneratedSprite Generate()
    {
        //TODO: find out how to do generation outside of Windows, so target could be more global...
        //Seeding random here, to ensure same results for same seed.
        Random = new(Seed.GetHashCode());
        return Mass switch
        {
            < 5 => DrawTerrestrial(),
            < 20 => DrawGas(),
            _ => DrawStar(),
        };
    }

    private GeneratedSprite DrawTerrestrial()
    {
        //Allows sizes between 10 and 20 for mass values from 1 to 5;
        int size =  Convert.ToInt32(Math.Floor(10 + 6.21 * Math.Log(Mass)));
        using var bmp = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bmp);
        Color color = (Random.Next() % 4) switch
        {
            1 => Color.MediumVioletRed,
            2 => Color.ForestGreen,
            3 => Color.AliceBlue,
            _ => Color.Wheat
        };
        //Pen pen = new(color);
        //Try texture brush?
        using SolidBrush brush = new(color);
        graphics.FillEllipse(brush, 0, 0, size - 1, size - 1);

        return new GeneratedSprite()
        {
            Height = size,
            Width = size,
            Data = BmpToMemoryStream(bmp)
        };
    }

    private GeneratedSprite DrawGas()
    {
        //Allows sizes between 25 and 70 for mass values from 5 to 20;
        int size = Convert.ToInt32(20 + 20 * Math.Cbrt(Mass - 5));
        int sliceCount = 3 + (Random.Next() % 6); //Add possiblity of monocolor?
        using var bmp = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bmp);
        //Gets 'sliceCount' random non repeating colors
        //Do we really want non repeating?
        var colors = GetNonRepeatingRandomNumbers(10, sliceCount).Select(GetGasColor).ToList();
        for(int i = 0; i < sliceCount; i++)
        {
            using var sliceBmp = new Bitmap(size, size / sliceCount + 1);
            using var slice = Graphics.FromImage(sliceBmp);
            using SolidBrush brush = new(colors[i]);
            int sliceCoordinates = Convert.ToInt32(Math.Round(1.0 * i * size / sliceCount, MidpointRounding.ToNegativeInfinity));
            slice.FillEllipse(brush, 0, 0 - sliceCoordinates, size - 1, size - 1);
            graphics.DrawImage(sliceBmp, 0, sliceCoordinates);
        }

        return new GeneratedSprite()
        {
            Height = size,
            Width = size,
            Data = BmpToMemoryStream(bmp)
        };
    }

    private GeneratedSprite DrawStar()
    {
        int size = Convert.ToInt32(55 + 65 * Math.Pow(Mass - 20, 0.25));
        using var bmp = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bmp);
        Color primaryColor = Mass switch
        {
            < 32 => Color.OrangeRed,
            < 44 => Color.GreenYellow,
            < 56 => Color.WhiteSmoke,
            < 80 => Color.LightSkyBlue,
            _ => Color.DeepSkyBlue
        };
        double gradient = 1 - 0.6 * Random.NextDouble();
        Color secondaryColor = Color.FromArgb(primaryColor.A, (int)(primaryColor.R * gradient), (int)(primaryColor.G * gradient), (int)(primaryColor.B * gradient));
        using GraphicsPath path = new();
        path.AddEllipse(0, 0, size - 1, size - 1);
        using PathGradientBrush gradientBrush = new(path);
        gradientBrush.CenterColor = primaryColor;
        gradientBrush.SurroundColors = [secondaryColor];
        graphics.FillEllipse(gradientBrush, 0, 0, size - 1, size - 1);
        return new GeneratedSprite()
        {
            Height = size,
            Width = size,
            Data = BmpToMemoryStream(bmp)
        };
    }

    private Color GetGasColor(int number) => number switch
    {
        0 => Color.RebeccaPurple,

        1 => Color.DarkGray,
        2 => Color.Magenta,
        3 => Color.DeepPink,
        4 => Color.DarkGreen,
        5 => Color.DarkViolet,
        6 => Color.PaleTurquoise,
        7 => Color.CadetBlue,
        8 => Color.DarkOrange,
        9 => Color.Azure,
        _ => Color.BlueViolet
    };

    private static MemoryStream BmpToMemoryStream(Bitmap image)
    {
        MemoryStream stream = new();
        image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private List<int> GetNonRepeatingRandomNumbers(int max, int count)
    {
        HashSet<int> result = [];
        do
        {
            int random = Random.Next() % max;
            if (!result.Contains(random)) result.Add(random);
        } while (result.Count < count);

        return [.. result];
    }
}
