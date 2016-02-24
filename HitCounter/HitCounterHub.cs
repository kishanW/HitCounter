using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HitCounter
{
    [HubName("hitCounter")]
    public class HitCounterHub : Hub
    {
        private static int _hitCount;


        public void RecordHit()
        {
            _hitCount += 1;
            Clients.All.onHitRecorded(_hitCount);
        }
    }
}