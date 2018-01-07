using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics;

namespace TimeCalculator.BL
{
    public class SpeedNotes
    {
        [PublicAPI]
        public const string DefaultFileName = "Speed.Notes";

        public class SpeedNode
        {
            public double Speed { get; set; }

            public int Drops { get; set; }
        }

        private readonly List<SpeedNode> _nodes;
        private readonly string _filePath;

        public List<SpeedNode> Nodes => _nodes;

        public SpeedNotes()
            :this(DefaultFileName)
        {
            
        }

        public SpeedNotes(string filePath)
        {
            _filePath = filePath;
            _nodes = new List<SpeedNode>();

            if(!File.Exists(filePath)) return;

            bool ok = false;

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        SpeedNode node = new SpeedNode();

                        string line = reader.ReadLine();

                        string[] segments = line?.Split(new[] {'-'}, 2, StringSplitOptions.RemoveEmptyEntries);

                        if (segments?.Length != 2)
                            return;

                        if(!double.TryParse(segments[0], out var speed)) return;
                        if(!int.TryParse(segments[1], out var drops)) return;

                        node.Speed = speed;
                        node.Drops = drops;

                        _nodes.Add(node);
                    }

                    ok = true;
                }
            }
            finally
            {
                if(!ok)
                    _nodes.Clear();
            }
        }

        public double CalculateSpeed(int drops)
        {
            if (_nodes.Count < 2) return 0;

            try
            {
                return Math.Round(Interpolate.Linear(_nodes.Select(n => (double) n.Drops), _nodes.Select(n => n.Speed)).Interpolate(drops), 3);
            }
            catch (ArgumentException)
            {
                return 0;
            }
        }

        public void Save()
        {
            using (var stream = new StreamWriter(new FileStream(DefaultFileName, FileMode.Create)))
                foreach (var node in Nodes)
                    stream.WriteLine(node.Speed + "-" + node.Drops);
        }
    }
}