using System;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Shapes;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace RomanPeshkov.VisualMethodSeparators.VmsViewportAdornment
{
    class VmsViewportAdornment
    {
        private const string MethodSignatureRegex = @"\b(public|private|internal|protected)\s*(static|virtual|abstract|override)?\s*[a-zA-Z]*\s[\w\<\>_1-9]+\s*\(";

        readonly Regex _methodSignature = new Regex(MethodSignatureRegex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private readonly IAdornmentLayer _layer;
        private readonly IWpfTextView _view;
        private readonly Pen _pen;

        public VmsViewportAdornment(IWpfTextView view, SVsServiceProvider serviceProvider)
        {
            var service = (DTE)serviceProvider.GetService(typeof(DTE));
            var properties = service.Properties["Visual Method Separators", "Global"];
            
            var colorProperty = properties.Item("Color");
            var color = UIntToColor(colorProperty.Value);

            var dashStyleProperty = properties.Item("PenDashStyle");
            var dashStyle = DashStyleFromInt(dashStyleProperty.Value);

            var thicknessProperty = properties.Item("Thickness");
            var thickness = (double) thicknessProperty.Value;

            _view = view;
            _view.LayoutChanged += OnLayoutChanged;

            _layer = view.GetAdornmentLayer("VmsViewportAdornment");

            _pen = new Pen(new SolidColorBrush(color), thickness)
                {
                    DashStyle = dashStyle,
                    DashCap = PenLineCap.Flat,
                };

            _pen.Freeze();
        }

        private Color UIntToColor(uint color)
        {
            byte r = (byte)(color >> 0);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 16);
            return Color.FromArgb(255, r, g, b);
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (var line in e.NewOrReformattedLines)
            {
                CreateVisuals(line);
            }
        }

        private void CreateVisuals(ITextViewLine line)
        {
            var lineText = line.Extent.GetText();

            if (_methodSignature.IsMatch(lineText))
            {
                var span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(line.Start, line.End));

                var a = CreateLine(0, line.Top, _view.ViewportWidth, line.Top, _pen);
                _layer.AddAdornment(span, null, a);    
            }
        }

        private static Line CreateLine(double x1, double y1, double x2, double y2, Pen pen)
        {
            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeThickness = pen.Thickness,
                Stroke = pen.Brush,
                StrokeDashArray = pen.DashStyle.Dashes,
                SnapsToDevicePixels = true,
                StrokeDashCap = PenLineCap.Square
            };

            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            return line;
        }

        private static DashStyle DashStyleFromInt(int value)
        {
            switch (value)
            {
                case 0:
                    return DashStyles.Solid;
                case 1:
                    return DashStyles.Dash;
                case 2:
                    return DashStyles.Dot;
                case 3:
                    return DashStyles.DashDot;
                case 4:
                    return DashStyles.DashDotDot;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}
