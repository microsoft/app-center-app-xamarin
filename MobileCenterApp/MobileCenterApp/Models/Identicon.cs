using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MobileCenterApp;

namespace MobileCenterApp
{
    public class Identicon
    {
        private String value;
        private int size;

        private static Dictionary<string, string[]> colors = new Dictionary<string, string[]> {
            { "colors", new [] { "#1FAECE", "#44B8A8", "#91CA47", "#9378CD", "#F56D4F", "#F1C40F" } },
            { "light1Colors", new [] { "#4FCAE6", "#7AD5C9", "#B3E770", "#B5A1E0", "#F69781", "#F9D33C" } },
            { "dark1Colors", new [] { "#3192B3", "#38A495", "#6FA22E", "#7E68C2", "#E2553D", "#F0B240" } },
            { "light2Colors", new [] { "#91E2F4", "#A3EBE1", "#CFEFA7", "#CEC0EC", "#F8C6BB", "#F7E28B" } },
            { "dark2Colors", new [] { "#2C7797", "#278E80", "#5A8622", "#614CA0", "#BC3C26", "#E7963B" } }
        };

        public Identicon(string value, int size)
        {
            this.value = value;
            this.size = size;
        }

        private string getColor(String value, String[] colors)
        {
            byte[] input = Encoding.ASCII.GetBytes(value.Substring(0, 1));
            uint hash = Murmur3.MurmurHash3(input, 1, 0);
            long index = hash % colors.Length;

            return colors[index];
        }

        public Dictionary<string, string> Style {
            get {
                string color = getColor(value, colors["light1Colors"]);

                Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "background", color },
                { "width", size.ToString() },
                { "height", size.ToString() },
                { "lineHeight", size - (size * 0.075) + "px" }
            };

                return dict;
            }
        }

        public Dictionary<string, string> TextStyle {
            get {
                string color = getColor(value, colors["dark1Colors"]);
                float fontsize = size / 2;

                Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "color", color },
                { "fontSize", Math.Floor(fontsize).ToString() }
            };

                return dict;
            }
        }
    }
}
