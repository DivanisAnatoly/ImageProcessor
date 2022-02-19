using ImageProcessor.Processor.Contracts;
using System.Drawing;

namespace ImageProcessor.Processor.Interfaces
{
    public interface IImageInfoService
    {
        BrightnessDistribution GetBrightnessDistributionInfo(byte[] image);

        BrightnessProfileData GetBrightnessProfileInfo(byte[] image, double fromX, double fromY, double toX, double toY);
    }
}
