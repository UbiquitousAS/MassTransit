// Copyright 2007-2018 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Transports.InMemory
{
    using System;
    using System.Threading.Tasks;
    using Topology;


    public class InMemoryPublishTransportProvider :
        IPublishTransportProvider
    {
        readonly InMemoryHost _host;
        readonly IInMemoryPublishTopology _publishTopology;
        readonly ISendTransportProvider _transportProvider;

        public InMemoryPublishTransportProvider(ISendTransportProvider transportProvider, IInMemoryPublishTopology publishTopology)
        {
            _transportProvider = transportProvider;
            _publishTopology = publishTopology;
            _host = transportProvider as InMemoryHost;
        }

        public Task<ISendTransport> GetPublishTransport<T>(Uri publishAddress)
            where T : class
        {
            ApplyTopologyToMessageFabric<T>();

            return _transportProvider.GetSendTransport(publishAddress);
        }

        void ApplyTopologyToMessageFabric<T>()
            where T : class
        {
            var builder = _host.CreatePublishTopologyBuilder();

            _publishTopology.GetMessageTopology<T>().Apply(builder);
        }
    }
}