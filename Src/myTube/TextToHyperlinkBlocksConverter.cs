// myTube.VideoDetails

using System;
using Windows.UI.Xaml.Documents;

namespace myTube
{
    internal class TextToHyperlinkBlocksConverter
    {
        public TextToHyperlinkBlocksConverter()
        {
        }

        internal Block Convert(string description)
        {
            Block b = default;
            //b = (Block)description;
            return b;
        }
    }
}