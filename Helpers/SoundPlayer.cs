using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MXTools.Helpers
{
  internal class SoundPlayer : IDisposable
  {
    private readonly string _sound;
    private readonly WaveOutEvent _outputDevice;
    private readonly AudioFileReader _audioFile;
    private CancellationTokenSource _cts;

    //----------------------------------------------------------------
    public SoundPlayer(string sound, float volume)
    {
      _sound = sound;
      string exeDirectory = Path.GetDirectoryName(Environment.ProcessPath);
      string file = Path.Combine(exeDirectory, _sound);
      _outputDevice = new WaveOutEvent()
      {
        Volume = 1.0f,
      };
      _audioFile = new AudioFileReader(file)
      {
        Volume = volume
      };
      _outputDevice.Init(_audioFile);

      _cts = new CancellationTokenSource();
    }
    //----------------------------------------------------------------
    public async Task Play(TimeSpan duration)
    {
      _cts = new CancellationTokenSource();
      var sw = Stopwatch.StartNew();

      while (sw.ElapsedMilliseconds < duration.TotalMilliseconds)
      {
        if (_cts.Token.IsCancellationRequested)
        {
          break;
        }
        if (_outputDevice.PlaybackState != PlaybackState.Playing)
        {
          _audioFile.CurrentTime = TimeSpan.Zero;
          _outputDevice.Volume = 1.0f;
          _outputDevice.Play();
        }
        await Task.Delay(200, _cts.Token);
      }

      Stop();
    }
    //---------------------------------------------------------------- 
    public void Stop()
    {
      if (_outputDevice.PlaybackState != PlaybackState.Playing)
      {
        return;
      }
      _cts.Cancel();
      _outputDevice.Stop();
    }
    //----------------------------------------------------------------
    public void Dispose()
    {
      Stop();
      _outputDevice?.Dispose();
      _audioFile?.Dispose();
      _cts?.Dispose();
    }
    //----------------------------------------------------------------
    public float Volume
    {
      get => _audioFile.Volume;
      set
      {
        _audioFile.Volume = value;
      }
    }
    //---------------------------------------------------------------------------------
    public bool IsPlaying()
    {
      return _outputDevice.PlaybackState == PlaybackState.Playing;
    }
  }
}
