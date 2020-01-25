using System;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Payroll.Application;
using Payroll.Application.Users.Projections;
using Payroll.EventSourcing;

namespace Payroll.Infrastructure
{
  public class TokenProvider : IAccessTokenProvider
  {
    private readonly char[] _secret;
    private readonly ISnapshotStore _snapshots;
    
    public TokenProvider(string secret, ISnapshotStore snapshots)
    {
      _secret = secret.ToCharArray();
      _snapshots = snapshots;
    }

    public string CreateToken(ActiveUserRecord user) {
      var token = new JwtBuilder()
        .WithAlgorithm(new HMACSHA256Algorithm())
        .WithSecret(_secret.ToString());
      token.Id(user.Id);
      token.Subject("access token");
      token.AddClaim("version", user.Version);
      token.AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
      token.AddClaim("claims", user.Claims);
      return token.Build();
    }

    public void ReadToken(string token, Action<ActiveUserRecord> cb) {
      try{
        var json = new JwtBuilder()
          .WithSecret(_secret.ToString())
          .MustVerifySignature()
          .Decode(token);
        var record = System.Text.Json.JsonSerializer.Deserialize(json, typeof(object));
        var type = record.GetType();
        var tid = type.GetProperty("tid")?.GetValue(record);
        var version = type.GetProperty("verison")?.GetValue(record);
        if(tid is null || version is null)
          return;
        else
        {
          var user = _snapshots.Get<ActiveUserRecord>(Guid.Parse(tid.ToString()));
          if(user != null)
            cb(user);
        }
      }
      catch(TokenExpiredException)
      {
        Console.WriteLine("token expired");
      }
      catch(SignatureVerificationException)
      {
        Console.WriteLine("invalid signature");
      }
    }

    public bool IsValidToken(string token) {
      try{
        var json = new JwtBuilder()
          .WithSecret(_secret.ToString())
          .MustVerifySignature()
          .Decode(token);
        return true;
      }
      catch(TokenExpiredException)
      {
        Console.WriteLine("token expired");
        return false;
      }
      catch(SignatureVerificationException)
      {
        Console.WriteLine("invalid signature");
        return false;
      }
    }
  }
}