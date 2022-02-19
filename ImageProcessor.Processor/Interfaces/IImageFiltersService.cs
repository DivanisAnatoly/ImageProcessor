using System.Drawing;

namespace ImageProcessor.Processor.Interfaces
{
    public interface IImageFiltersService
    {
        byte[] Binarize(byte[] image, int level, Color mainColor, Color backgroundColor);

        byte[] Grayscale(byte[] image);

        byte[] Negative(byte[] image);

        byte[] ChangeBrightness(byte[] originImage, int brightnessChange);

        byte[] ChangeContrast(byte[] originImage, int contrastChange);

        public byte[] Noise(byte[] image, double probability);

        public byte[] LinearSmoothing(byte[] image);

        public byte[] MedianSmoothing(byte[] image);

        byte[] KirschsMethod(byte[] image);

        byte[] SobelMethod(byte[] image);

        byte[] LaplasMethod(byte[] image);

        byte[] RobertsMethod(byte[] image);

        byte[] WallisMethod(byte[] image);

        byte[] StaticMethod(byte[] image);

    }
}
