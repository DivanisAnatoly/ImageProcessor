using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Processor.Interfaces
{
    public interface IImageAreasService
    {
        byte[] MagicWand(byte[] image, int X, int Y, int level);

        byte[] Dekstra(byte[] image, int X1, int Y1, int X2, int Y2);

        byte[] Clustarization(byte[] image);
    }
}
