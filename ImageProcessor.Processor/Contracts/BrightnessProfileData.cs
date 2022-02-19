using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Processor.Contracts
{
    public class BrightnessProfileData
    {
        public int[] RedChanalProfile { get; private set; }
        public int[] GreenChanalProfile { get; private set; }
        public int[] BlueChanalProfile { get; private set; }

        public BrightnessProfileData(int[] redChanalProfile,
            int[] greenChanalProfile, int[] blueChanalProfile)
        {
            RedChanalProfile = redChanalProfile;
            GreenChanalProfile = greenChanalProfile;
            BlueChanalProfile = blueChanalProfile;
        }
    }
}
