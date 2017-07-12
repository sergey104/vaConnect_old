using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaConnect
{
    class OnboardingService
    {
        private static OnboardingService instance;

        private OnboardingService()
        { }

        public static OnboardingService getInstance()
        {
            if (instance == null)
            {
                instance = new OnboardingService();
            }
            return instance;
        }
        public void getWiFiProfile(String token, String identifier, WiFiProfile wp)
        {

        }
    }
}
