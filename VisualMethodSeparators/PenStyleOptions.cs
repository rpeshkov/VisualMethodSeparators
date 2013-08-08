using System;
using System.ComponentModel;
using System.Drawing;

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace RomanPeshkov.VisualMethodSeparators
{
    [CLSCompliant(false), ComVisible(true)]
    public class PenStyleOptions : DialogPage
    {
        public PenStyleOptions()
        {
            Color = Color.Red;
            PenDashStyle = DashStyle.Solid;
            Thickness = 1.0;
        }

        public enum DashStyle
        {
            Solid, Dash, Dot, DashDot, DashDotDot
        }

        [Category("Visual")]
        public Color Color { get; set; }

        [DisplayName(@"Dash style")]
        [Category("Visual")]
        public DashStyle PenDashStyle { get; set; }
        
        [Category("Visual")]
        public double Thickness { get; set; }
    }
}