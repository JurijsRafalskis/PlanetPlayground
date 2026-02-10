using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Presets
{
    public class GeneratedSprite : IDisposable
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public MemoryStream Data { get; set; }

        public void Dispose()
        {
            Data?.Dispose();
        }
    }
}
