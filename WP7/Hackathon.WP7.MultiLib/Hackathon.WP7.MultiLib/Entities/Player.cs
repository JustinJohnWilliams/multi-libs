using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Hackathon.WP7.MultiLib.Entities
{
    public class Player
    {
        public string id { get; set; }
        public string name { get; set; }
        public string selectedWhiteCardId { get; set; }
        public bool isCzar { get; set; }
        public int awesomePoints { get; set; }
        public List<string> cards { get; set; }
    }
}
