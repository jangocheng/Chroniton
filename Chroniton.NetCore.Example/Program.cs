﻿using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chroniton.NetCore.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new SingularityFactory();
            var singularity = factory.GetSingularity();

            var job = new SimpleParameterizedJob<string>(
                (parameter, scheduledTime) => Task.Run(() =>
                Console.WriteLine($"{parameter}\tscheduled: {scheduledTime.ToString("o")}")));

            var schedule = new EveryXTimeSchedule(TimeSpan.FromSeconds(1));

            var scheduledJob = singularity.ScheduleParameterizedJob(
                schedule, job, "Hello World", true); //starts immediately

            var startTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(5));

            var scheduledJob2 = singularity.ScheduleParameterizedJob(
                schedule, job, "Hello World 2", startTime);

            singularity.Start();

            Task.Delay(10 * 1000).Wait();

            singularity.StopScheduledJob(scheduledJob);

            Task.Delay(5 * 1000).Wait();

            singularity.Stop();

            Console.ReadKey();
        }
    }
}