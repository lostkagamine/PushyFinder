using System.Diagnostics;

namespace PushyFinder.Util;

public class TimedBool
{
    private bool running;
    private readonly float time;
    private readonly Stopwatch watch;

    public TimedBool(float time)
    {
        watch = new Stopwatch();
        running = false;
        this.time = time;
    }

    public bool Value
    {
        get
        {
            var sw = watch.Elapsed.TotalSeconds < time;
            if (!sw) Stop();
            return running && sw;
        }
    }

    public TimedBool Start()
    {
        running = true;
        watch.Restart();
        return this;
    }

    public TimedBool Stop()
    {
        running = false;
        watch.Stop();
        return this;
    }
}
