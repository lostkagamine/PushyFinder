using System.Diagnostics;

namespace PushyFinder.Util;

public class TimedBool
{
    private Stopwatch watch;
    private float time;
    private bool running = false;

    public TimedBool(float time)
    {
        watch = new Stopwatch();
        running = false;
        this.time = time;
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
    
    public bool Value
    {
        get
        {
            var sw = watch.Elapsed.TotalSeconds < time;
            if (!sw)
            {
                Stop();
            }
            return running && sw;
        }
    }
}
