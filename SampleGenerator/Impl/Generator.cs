using System;
using System.IO;
using MathNet.Numerics;
using MathNet.Numerics.Random;
using TimeCalculator.BL;
using TimeCalculator.Data;

namespace SampleGenerator.Impl
{
    public sealed class Generator
    {
        private class DoubleRundom
        {
            private readonly RandomSource _base;
            private readonly int _min;
            private readonly int _max;
            private int _next;

            public DoubleRundom(RandomSource @base, int min, int max)
            {
                _base = @base;
                _min = min;
                _max = max;
                _next = @base.Next(min, max);
            }

            public int Next()
            {
                var realNext = _next == _min ? _min : _base.Next(_min, _next);
                _next = _base.Next(_min, _max);
                return realNext;
            }
        }

        private RunData Data { get; }
        private Action<Progress> ProgressAction { get; }

        public Generator(RunData data, Action<Progress> progressAction)
        {
            Data = data;
            ProgressAction = progressAction;
        }

        private DoubleRundom _iterationTime;
        private DoubleRundom _setupTime;


        public void Generate()
        {
            DataBaseFactory.UserNameOverride = Data.DatabaseName;
            if(File.Exists(DataBaseFactory.DatabasePath))
                File.Delete(DataBaseFactory.DatabasePath);

            PaperFormat[] formats =
            {
                new PaperFormat(50, 70), new PaperFormat(32, 46)
            };

            RandomSource random = new WH2006(Environment.TickCount, false);
            DoubleRundom iterations = new DoubleRundom(random, 1, 10);
            DoubleRundom speed = new DoubleRundom(random, 0, 10);
            SpeedNotes speedNotes = new SpeedNotes("Speed.Notes");
            _iterationTime = new DoubleRundom(random, 5, 20);
            _setupTime = new DoubleRundom(random, 10, 120);

            double[] quarterMultipler =
            {
                1.0,
                0.5,
                1.5,
                2.0
            };
            int perQuater = Data.ToGenerate / 4;
            int currentQuater = 0;
            int quaterCounter = perQuater;

            int swift = 0;
            int compledAmount = Data.ToGenerate * formats.Length;
            int currentAmount = 0;
            int error = 0;

            foreach (var paperFormat in formats)
            {
                DateTime datetime = DateTime.Now;
                if (swift > 0)
                    datetime += TimeSpan.FromDays(1);
                swift++;

                for (int i = 0; i < Data.ToGenerate; i++)
                {
                    currentAmount++;
                    quaterCounter--;
                    if (quaterCounter == 0)
                    {
                        currentQuater++;
                        if (currentQuater == 4)
                            currentQuater = 0;
                        quaterCounter = perQuater;
                    }

                    var realIterations = iterations.Next();
                    var realamount = random.Next(100, 10000);
                    var realSpeed = speedNotes.CalculateSpeed(speed.Next() >= 5 ? 15 : 4);
                    var realtime = CalculateTime(realamount, realSpeed, realIterations, quarterMultipler[currentQuater], out var realProblem, out var realBigProblem);

                    SaveInput input = new SaveInput(realamount, realIterations, realProblem, realBigProblem, paperFormat, realSpeed, datetime, realtime);

                    var result = BusinessRules.Save.Action(input);
                    if (!result.Succsess)
                        error++;

                    ProgressAction(new Progress { Amount = compledAmount, Generated = currentAmount, Errors = error});

                    datetime += TimeSpan.FromDays(2);
                }
            }
        }

        private TimeSpan CalculateTime(int amount, double speed, int iterations, double quaterMultipler, out bool problem, out bool bigProblem)
        {
            problem = false;
            bigProblem = false;

            double[] point = {0.06, 0.7};
            double[] value = {3.22, 42};

            double perhoure = Interpolate.Linear(point, value).Interpolate(speed);

            double runtime = amount / perhoure * iterations;
            double iteration = _iterationTime.Next() * iterations;
            double setup = _setupTime.Next();

            if (setup > 50)
                problem = true;
            if (setup > 100)
                bigProblem = true;

            return TimeSpan.FromMinutes((runtime + iteration + setup) * quaterMultipler);
        }
    }
}