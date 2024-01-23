using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using P2Controller.p2Command;
using StbImageSharp;

namespace P2Controller.Class;

internal class P2WebAPI
{
  private const int config_port = 8080;

  private const int camera_port = 8329;

  private static HttpClient client = new HttpClient();

  private static IntPtr cam = softcamHelper.scCreateCamera(4656, 3496, 0f);

  public p2_irResponse irCommand(string ip, int index, string action)
  {
    //IL_003a: Unknown result type (might be due to invalid IL or missing references)
    //IL_0040: Expected O, but got Unknown
    string uri = $"http://{ip}:{8080}/peripheral/ir_led";
    StringContent content = new StringContent(JsonSerializer.Serialize(new p2_irRequest
    {
      action = action,
      index = index
    }, new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
    return JsonSerializer.Deserialize<p2_irResponse>(client.PostAsync(uri, (HttpContent)(object)content).Result.Content.ReadAsStringAsync().Result);
  }

  public p2_Response lightCommand(string ip, int idx, int value)
  {
    //IL_0045: Unknown result type (might be due to invalid IL or missing references)
    //IL_004b: Expected O, but got Unknown
    string uri = $"http://{ip}:{8080}/peripheral/fill_light";
    StringContent content = new StringContent(JsonSerializer.Serialize(new p2_lightRequest
    {
      action = "set_bri",
      idx = idx,
      value = value
    }, new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
    return JsonSerializer.Deserialize<p2_Response>(client.PostAsync(uri, (HttpContent)(object)content).Result.Content.ReadAsStringAsync().Result);
  }

  public p2_lightsResponse light0Command(string ip, int idx, int value)
  {
    //IL_0045: Unknown result type (might be due to invalid IL or missing references)
    //IL_004b: Expected O, but got Unknown
    string uri = $"http://{ip}:{8080}/peripheral/fill_light";
    StringContent content = new StringContent(JsonSerializer.Serialize(new p2_lightRequest
    {
      action = "set_bri",
      idx = idx,
      value = value
    }, new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
    return JsonSerializer.Deserialize<p2_lightsResponse>(client.PostAsync(uri, (HttpContent)(object)content).Result.Content.ReadAsStringAsync().Result);
  }

  public void cameraCommand(string ip, int stream)
  {
    Uri uri = new Uri($"http://{ip}:{8329}/camera/snap?stream={stream}");

    var fetch = Stopwatch.StartNew();
    HttpResponseMessage result = client.GetAsync(uri, (HttpCompletionOption)1).Result;
    result.EnsureSuccessStatusCode();
    Trace.WriteLine($"fetch {fetch.Elapsed}");

    var copy = Stopwatch.StartNew();
    if (true)
    {
      MemoryStream jpg = new MemoryStream();
      result.Content.CopyToAsync((Stream)jpg).Wait();
    }
    else
    {
      using (var f = File.OpenWrite("camera_shot.jpg"))
      {
        result.Content.CopyToAsync(f).Wait();
      }
    }
    Trace.WriteLine($"copy {copy.Elapsed}");

    result.Dispose();

    var decode = Stopwatch.StartNew();
    ImageResult img = ImageResult.FromStream(jpg);
    Trace.WriteLine($"decode {decode.Elapsed}");

    var send = Stopwatch.StartNew();
    softcamHelper.scSendFrame(cam, img.Data);
    Trace.WriteLine($"send {send.Elapsed}");
  }

  public p2_Response exposureCommand(string ip, int stream, int value)
  {
    //IL_0033: Unknown result type (might be due to invalid IL or missing references)
    //IL_0039: Expected O, but got Unknown
    Uri uri = new Uri($"http://{ip}:{8329}/camera/exposure?stream={stream}");
    StringContent content = new StringContent(JsonSerializer.Serialize(value, new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
    return JsonSerializer.Deserialize<p2_Response>(client.PostAsync(uri, (HttpContent)(object)content).Result.Content.ReadAsStringAsync().Result);
  }

  public p2_Response moveCommand(string ip, int x, int y, int f)
  {
    //IL_0059: Unknown result type (might be due to invalid IL or missing references)
    //IL_005f: Expected O, but got Unknown
    Uri uri = new Uri($"http://{ip}:{8080}/peripheral/laser_head");
    StringContent content = new StringContent(JsonSerializer.Serialize(new p2_laserHead_MoveRequest
    {
      x = x,
      y = y,
      f = f,
      waitTime = 30000
    }, new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    }));
    return JsonSerializer.Deserialize<p2_Response>(client.PostAsync(uri, (HttpContent)(object)content).Result.Content.ReadAsStringAsync().Result);
  }
}
