using System;
using System.IO;
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
            if (File.Exists(DataBaseFactory.DatabasePath))
                File.Delete(DataBaseFactory.DatabasePath);

            PaperFormat[] formats =
            {
                new PaperFormat(50, 100),
                new PaperFormat(50, 70),
                new PaperFormat(30, 45),
                new PaperFormat(20, 30),
            };

            RandomSource random = new WH2006(false);
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

            int compledAmount = Data.ToGenerate * 4;
            int currentAmount = 0;
            int error = 0;


            DateTime datetime = DateTime.Now;

            for (int i = 0; i < compledAmount; i++)
            {
                if (datetime.Hour > 16)
                {
                    int toZero = 24 - datetime.Hour;

                    datetime += TimeSpan.FromHours(toZero + 7);
                }

                int currentQuater;
                if (datetime.Month <= 3)
                    currentQuater = 0;
                else if (datetime.Month <= 6)
                    currentQuater = 1;
                else if (datetime.Month <= 9)
                    currentQuater = 2;
                else
                    currentQuater = 3;

                var realIterations = iterations.Next();
                var realamount = random.Next(100, 10000);
                var realSpeed = speedNotes.CalculateSpeed(speed.Next() >= 5 ? 15 : 4);
                var realformat = formats[random.Next(0, 3)];
                var realtime = CalculateTime(realamount, realSpeed, realIterations, quarterMultipler[currentQuater], realformat, out var realProblem, out var realBigProblem,
                                              out var iterationMinutes, out var setupMinutes);

                SaveInput input = new SaveInput(realamount, realIterations, realProblem, realBigProblem, realformat, realSpeed, datetime, realtime, iterationMinutes, setupMinutes);

                datetime += realtime;

                var result = BusinessRules.Save.Action(input);
                if (!result.Succsess)
                    error++;

                ProgressAction(new Progress {Amount = compledAmount, Generated = currentAmount, Errors = error});

                currentAmount++;
            }
        }


        private TimeSpan CalculateTime(int amount, double speed, int iterations, double quaterMultipler, PaperFormat realformat, out bool problem, out bool bigProblem, 
                                        out int iterationMinutes, out int setupMinutes)
        {
            problem = false;
            bigProblem = false;

            // ReSharper disable once PossibleInvalidOperationException
            double mperunit = (realformat.Lenght.Value + 10)  / 100d;
            double compledMeters = amount * iterations * mperunit;
            
            double runtime = compledMeters / speed / 60;
            double iteration = _iterationTime.Next() * iterations;
            double setup = _setupTime.Next();

            iterationMinutes = (int) iteration;
            setupMinutes = (int) setup;

            if (setup > 50)
                problem = true;
            if (setup > 100)
                bigProblem = true;

            return TimeSpan.FromMinutes((runtime + iteration + setup) * quaterMultipler);
        }
    }
}