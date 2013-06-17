using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
  class Program
  {

    class Point
    {
      public string Name;
      public string Headers;
      public double? X;
      public double? Y;
      public double? Z;
      public double? Distance;
    }

    private readonly static Point[] Points = new[]
    {

new Point {Name = "Michiel", X =-289.175, Y=-200, Z=242.647},
new Point {Name = "Rik", X =212.662, Y=40, Z=368.341},
new Point {Name = "Jeroen", X =-129.11, Y=200, Z=-354.726},
new Point {Name = "Jaap", X =0,Y =200, Z=-377.492}, //==> X was null => X=40?
new Point {Name = "Bob", X =-242.647, Y=200, Z=289.175},
new Point {Name = "Arian", X =0, Y=-40, Z=-425.323}, //==> X was null => X=24?
new Point {Name = "Bas", X =-188.746, Y=-200, Z=-326.917},

new Point {Name = "Marcel", X =-205, Y=-120, Z=355.07},
new Point {Name = "Tim", X=-355.07, Y=-120, Z= 205},

new Point {Name = "Chris", X =-65.551, Y=200, Z=-371.757},
new Point {Name = "Remco", X =242.647, Y=-200, Z=289.175},
new Point {Name = "Lukas", X =377.492, Y=200, Z= 0},
new Point {Name = "Christiaan", X =263.543, Y=120, Z=314.078},
new Point {Name = "Roelf", X =242.647, Y=200, Z=289.175},
new Point {Name = "Guus", X =273.393, Y=-40, Z=325.817},
new Point {Name = "Arjen", X = 425.323, Y= 40, Z= 0},
new Point {Name = "JasperK", X = 410, Y = 120, Z = 0},
new Point {Name = "Thijs", X = 399.673, Y = 40, Z= 145.469},

new Point { Name = "MartijnL", X = -73.857, Y = -40, Z = -418.862},
new Point { Name = "Kamil", X = 377.492, Y = -200, Z = 0},
new Point { Name = "Johan", X = 73.857, Y = 40, Z = -418.862},

new Point {Name = "Korjan", X = 71.196, Y = 120, Z= -403.771},
new Point { Name = "Tom", X = -205, Y = -120, Z = -355.07},

    };


    static double? ParseCoord(string axis, string value)
    {
      var match = Regex.Match(value, "Q-Position-" + axis.ToUpper() + @":\s*([.0-9\-]+)");
      if (!match.Success) return null;
      return double.Parse(match.Groups[1].Value);
    }

    static void Main(string[] args)
    {
      //      foreach (var point in Points)
      //{
      //        point.X = ParseCoord("X", point.Headers);
      //        point.Y = ParseCoord("Y", point.Headers);
      //point.Z = ParseCoord("Z", point.Headers);
      //      }

      List<string> outputs = new List<string>();
      outputs.Add(@"(*sphere*) SphericalPlot3D[427.2, {\[Theta], 0, \[Pi]}, {\[Phi], 0, 2 \[Pi]}, Mesh -> None,
PlotRange -> All, Lighting -> ""Neutral"",
PlotStyle -> Opacity[0.8], PlotPoints -> 50]");
      outputs.Add("(*AARDE*) Graphics3D[{Blue, Thickness[0.1], Sphere[{0, 0, 0}, 25]}]");
      //outputs.Add("(*QLOGO*) Graphics3D[{Orange, Thickness[0.1], Line[{{0, 0, 427.2}, {0, 0, 427.2}}]}]");



      foreach (var point in Points)
      {
        if (point.Name == null)
          continue;

        if (!point.X.HasValue || !point.Y.HasValue || !point.Z.HasValue)
        {
          outputs.Add(string.Format("(*{0} IGNORED*)", point.Name));
          continue;
        }

        point.Distance = Math.Sqrt(point.X.Value * point.X.Value + point.Y.Value * point.Y.Value + point.Z.Value * point.Z.Value);


        outputs.Add(
          string.Format(CultureInfo.InvariantCulture,
            "(*{0}, D={4:0.00}*) Graphics3D[{{Green, Thickness[0.03], Sphere[{{{1}, {2}, {3}}}, 10]}}]",
            point.Name,
            point.X.Value,
            point.Y.Value,
            point.Z.Value,
            point.Distance ?? 0
            ));
        var extra =
          string.Format(CultureInfo.InvariantCulture,
            "(*{0}, D={7:0.00}*) Graphics3D[{{Green, Thickness[0.03], Line[{{{{{1}, {2}, {3}}}, {{{4}, {5}, {6}}}}}]}}]",
            point.Name,
            -point.Z.Value,
            -point.Y.Value,
            -point.X.Value,
            -point.Z.Value,
            -point.Y.Value,
            -point.X.Value,
            point.Distance ?? 0
            );
        //outputs.Add(extra);
      }

      Console.WriteLine("Show[");
      Console.WriteLine(string.Join(",\n", outputs));
      Console.WriteLine(", Axes -> True, AxesLabel -> {X, Y, Z}]");
      Console.ReadKey();
    }
  }
}
