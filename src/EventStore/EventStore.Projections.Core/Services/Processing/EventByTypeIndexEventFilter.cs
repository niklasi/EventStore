// Copyright (c) 2012, Event Store LLP
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
// 
// Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// Redistributions in binary form must reproduce the above copyright
// notice, this list of conditions and the following disclaimer in the
// documentation and/or other materials provided with the distribution.
// Neither the name of the Event Store LLP nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

using System.Collections.Generic;
using System.Linq;

namespace EventStore.Projections.Core.Services.Processing
{
    public class EventByTypeIndexEventFilter : EventFilter
    {
        //NOTE: this filter will pass both events and links to these events from index streams resulting
        //      in resolved events re-appearing in the event stream.  This must be filtered out by a 
        //      reader subscription
        private readonly HashSet<string> _events;
        private readonly HashSet<string> _streams;

        public EventByTypeIndexEventFilter(HashSet<string> events)
            : base(false, events)
        {
            _events = events;
            _streams = new HashSet<string>(from eventType in events select "$et-" + eventType);
        }

        public override bool PassesSource(bool resolvedFromLinkTo, string positionStreamId, string eventType)
        {
            //TODO: add tests to assure that resolved by link events are not passed twice into the subscription?!!
            return !resolvedFromLinkTo || _streams.Contains(positionStreamId);
        }

        public override string GetCategory(string positionStreamId)
        {
            return null;
        }
    }
}