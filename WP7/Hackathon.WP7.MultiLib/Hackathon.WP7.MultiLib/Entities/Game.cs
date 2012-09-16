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
    public class Game
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool isStarted { get; set; }
        public string winnerId { get; set; }
        public string winningCardId { get; set; }
        public List<Player> players { get; set; }
        public string currentBlackCard { get; set; }
    }
}
