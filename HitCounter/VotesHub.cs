using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HitCounter
{
    [HubName("votesHub")]
    public class VotesHub : Hub
    {
        private static readonly List<Vote> _votes = new List<Vote>();

        public void Vote(bool reaction)
        {
            var vote = new Vote(reaction);
            _votes.Add(vote);
            BroadcastVotes();
        }

        public void BroadcastVotes()
        {
            var getVotesFrom = DateTime.Now.AddSeconds(-3);
            var votes = _votes.Where(x => x.TimeStamp > getVotesFrom && x.TimeStamp < DateTime.Now).ToList();


            var upVotes = votes.Count(w => w.Reaction);
            var downVotes = votes.Count(w => !w.Reaction)*-1;
            var broadcastVote = new BroadcastVote
            {
                TimeStamp = getVotesFrom,
                VoteCount = upVotes + downVotes
            };
            Clients.All.onNewBroadcastMessageReceived(broadcastVote);
        }


        
    }

    public class Vote
    {
        public Vote(bool reaction)
        {
            Reaction = reaction;
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; set; }
        public bool Reaction { get; set; }
    }

    public class BroadcastVote
    {
        public DateTime TimeStamp { get; set; }
        public int VoteCount { get; set; }
    }
}